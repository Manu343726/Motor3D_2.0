Imports Motor3D.Espacio2D
Imports System.Math

Public Class Form1
    Dim BMP As Bitmap
    Dim G As Graphics

    Dim P() As Figura2D
    Dim Indice As Integer = 0

    Dim Dibujando As Boolean = False
    Dim Vertices As New List(Of Punto2D)

    Dim Debug As Boolean = False
    Dim Niveles As Integer = 1
    Dim BSP As Boolean = False

    Dim Quadtree As Quadtree
    Dim NivelesQuadtree As Integer = 2

    Dim Pertenece As Boolean

    Dim RectaJordan As Recta2D
    Dim PuntosJordan As New List(Of Punto2D)

    Dim Agarrado As Boolean = False
    Dim PuntoAgarre As Punto2D
    Dim IndiceAgarre As Integer
    Dim Ancla As Punto2D

    Dim Marco As AABB2D

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Back
                Vertices.Clear()
                Dibujando = True
            Case Keys.Right
                P(Indice).Velocidad += New Vector2D(0.1, 0)
            Case Keys.Left
                P(Indice).Velocidad += New Vector2D(-0.1, 0)
            Case Keys.Up
                P(Indice).Velocidad += New Vector2D(0, 0.1)
            Case Keys.Down
                P(Indice).Velocidad += New Vector2D(0, -0.1)
            Case Keys.Z
                P(Indice).VelocidadAngular -= 0.01
            Case Keys.X
                P(Indice).VelocidadAngular += 0.01
            Case Keys.Space
                P(Indice).VelocidadAngular = 0
            Case Keys.Enter
                If Not Dibujando Then
                    P(Indice).Velocidad = New Vector2D()
                Else
                    If Vertices.Count >= 3 Then
                        P(Indice).EstablecerVertices(Vertices.ToArray)
                        Dibujando = False
                    End If
                End If
            Case Keys.A
                P(Indice).Rozamiento -= 0.0001
            Case Keys.S
                P(Indice).Rozamiento += 0.0001
            Case Keys.Q
                If Indice > 0 Then Indice -= 1
            Case Keys.W
                If Indice < P.GetUpperBound(0) Then Indice += 1
            Case Keys.D
                Debug = Not Debug
            Case Keys.C
                If Niveles > 1 Then Niveles -= 1
            Case Keys.V
                Niveles += 1
            Case Keys.B
                BSP = Not BSP
            Case Keys.E
                If NivelesQuadtree > 2 Then
                    NivelesQuadtree -= 1
                    Quadtree.Niveles = NivelesQuadtree
                End If
            Case Keys.R
                NivelesQuadtree += 1
                Quadtree.Niveles = NivelesQuadtree
        End Select
    End Sub

    Private Sub Form1_Resze(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Pic.Width <> 0 AndAlso Pic.Height <> 0 Then
            Marco = New AABB2D(0, 0, Pic.Width, Pic.Height)
            BMP = New Bitmap(Pic.Width, Pic.Height)
            G = Graphics.FromImage(BMP)
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Indice = 0

        ReDim P(1)

        For i As Integer = 0 To P.GetUpperBound(0)
            P(i) = New Figura2D(New Punto2D(Pic.Width / 2, 100 + (100 * i)), New Punto2D(100, Pic.Height - 100 - (100 * i)), New Punto2D(Pic.Width - 100, Pic.Height - 100))
            P(i).VelocidadAngular = 0
            P(i).Velocidad = New Vector2D
            P(i).FrecuenciaActualizacion = 100
            P(i).AutoActualizar = False
            P(i).Rozamiento = 0.0001
            P(i).RozamientoActivado = True
        Next

        BMP = New Bitmap(Pic.Width, Pic.Height)
        G = Graphics.FromImage(BMP)

        Quadtree = New Quadtree(NivelesQuadtree, New AABB2D(New Punto2D(), New Punto2D(Pic.Width, Pic.Height)))

        Timer1.Interval = 10
        Timer1.Start()
    End Sub

    Private Sub RedibujarQuadtree()
        Dim dx As Integer
        Dim AABB As AABB2D

        'dx = Pic.Size.Width / (2 ^ (Quadtree.Niveles - 1))
        'dy = Pic.Size.Height / (2 ^ (Quadtree.Niveles - 1))

        'For i As Integer = 0 To Pic.Width Step dx
        '    G.DrawLine(Pens.Gray, i, 0, i, Pic.Height)
        'Next
        'For j As Integer = 0 To Pic.Height Step dy
        '    G.DrawLine(Pens.Gray, 0, j, Pic.Width, j)
        'Next

        For k As Integer = 0 To P.GetUpperBound(0)
            For l As Integer = 0 To P(k).Vertices.GetUpperBound(0)
                If Quadtree.Pertenece(P(k).Vertices(l)) Then
                    Dim Sector As SectorQuadtree = Quadtree.Sector(P(k).Vertices(l))
                    Do While True

                        'Sector = Sector.Padre
                        If Sector Is Nothing Then Exit Do
                    Loop
                End If
            Next
        Next
    End Sub

    Private Sub Repintar()
        If Not Dibujando Then
            Dim col As Boolean = OBB2D.Colision(P(0).OBB, P(1).OBB)
            Me.Text = "Motor3D 2.0 - Prueba Figura2D. Niveles BSP: " & Niveles & ", Polígono seleccionado: " & Indice & " (Velocidad=" & P(Indice).Velocidad.ToString & " pixels/s , velocidad angular=" & P(Indice).VelocidadAngular.ToString & " rad/s , rozamiento=" & P(Indice).Rozamiento.ToString & ")"
            G.Clear(Color.Black)

            'RedibujarQuadtree()

            For j As Integer = 0 To P.GetUpperBound(0)
                Dibujar(P(j).OBB)
                'Dibujar(P(j).AABB, Color.LightBlue)
                Dibujar(P(j), Color.Green)
                'DibujarArbolOBB(P(j).Arbol)

                'G.DrawPolygon(IIf(col, Pens.LightGray, Pens.DarkGray), P(j).VerticesToPoint)
                'G.FillPolygon(New SolidBrush(Color.FromArgb(128, IIf(col, Color.Green, Color.Red))), P(j).VerticesToPoint)

                If Not RectaJordan Is Nothing Then
                    For i As Integer = 0 To PuntosJordan.Count - 1
                        G.DrawEllipse(Pens.LightGreen, New Rectangle(PuntosJordan(i).X - 3, PuntosJordan(i).Y - 3, 6, 6))
                    Next
                    G.DrawLine(Pens.LightGreen, RectaJordan.PuntoDiretor.ToPoint, PuntosJordan(0).ToPoint)
                End If

                If col Then
                    Dim Datos As DatosColision2D = OBB2D.DatosColision(P(0).OBB, P(1).OBB)
                    Dim Recta As Recta2D = Datos.RectaColision
                    Dim PS1() As Punto2D = OBB2D.PuntosPenetracion(P(0).OBB, P(1).OBB)
                    Dim PS2() As Punto2D = OBB2D.PuntosPenetracion(P(1).OBB, P(0).OBB)
                    Dim CONSTANTE As Integer = 2

                    'P(0).AplicarFuerza(Not Datos.Direccion * Datos.Penetracion / CONSTANTE, Datos.PuntoImpacto)
                    'P(1).AplicarFuerza(Datos.Direccion * Datos.Penetracion / CONSTANTE, Datos.PuntoImpacto)

                    'Figura2D.ReaccionColision(P(0), P(1))

                    'G.DrawEllipse(Pens.Orange, New Rectangle(Datos.PuntoImpacto.X - 3, Datos.PuntoImpacto.Y - 3, 6, 6))

                    'G.DrawLine(Pens.Orange, Recta.ObtenerPunto(-1000).ToPoint, Recta.ObtenerPunto(1000).ToPoint)
                End If
                If Agarrado Then
                    If Not PuntoAgarre Is Nothing Then G.DrawEllipse(Pens.White, New Rectangle(PuntoAgarre.X - 3, PuntoAgarre.Y - 3, 6, 6))
                    G.DrawLine(Pens.White, P(IndiceAgarre).Baricentro.ToPoint, PuntoAgarre.ToPoint)
                    G.DrawLine(Pens.White, PuntoAgarre.ToPoint, Ancla.ToPoint)
                End If

                'Dibujar(AABB2D.Interseccion(P(0).AABB, P(1).AABB), Color.White)

                If Debug Then
                    If Not BSP Then
                        For i As Integer = 0 To P(j).Lados.GetUpperBound(0)
                            For Each AABB As AABB2D In AABB2D.SubAABBsDiagonales(P(j).Lados(i).AABB, P(j).Lados(i).Pendiente, Niveles)
                                Dibujar(AABB, Color.Purple)
                            Next
                        Next
                    End If

                    G.DrawRectangle(IIf(AABB2D.Colision(P(0).AABB, P(1).AABB), Pens.Orange, Pens.Blue), New Rectangle(P(j).AABB.Posicion.ToPoint, P(j).AABB.Dimensiones.ToPoint))
                Else
                    For i As Integer = 0 To P(j).Lados.GetUpperBound(0)
                        G.DrawEllipse(Pens.White, New Rectangle(P(j).Lados(i).ExtremoInicial.X - 2, P(j).Lados(i).ExtremoInicial.Y - 2, 4, 4))
                        G.DrawEllipse(Pens.Orange, New Rectangle(P(j).Lados(i).ExtremoFinal.X - 6, P(j).Lados(i).ExtremoFinal.Y - 2, 4, 4))
                    Next
                End If
            Next

            If Debug Then
                If BSP Then
                    For i As Integer = 0 To P(0).Lados.GetUpperBound(0)
                        If AABB2D.Colision(P(1).AABB, P(0).Lados(i).AABB) Then
                            For k As Integer = 0 To P(1).Lados.GetUpperBound(0)
                                If AABB2D.Colision(P(1).Lados(k).AABB, P(0).AABB) Then
                                    For Each AABB As AABB2D In Segmento2D.UltimasAABBsBSP(P(0).Lados(i), P(1).Lados(k), Niveles)
                                        Dibujar(AABB, Color.Aqua)
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Dim SubPoligonos() As Poligono2D = Poligono2D.Particion(P(0), New Recta2D(P(0).Baricentro, P(1).Baricentro))
            Dim Intersecciones() As Punto2D = Poligono2D.PuntosInterseccion(P(0), P(1))

            'If Not SubPoligonos(0) Is Nothing Then Dibujar(SubPoligonos(0), Color.Yellow)
            'If Not SubPoligonos(1) Is Nothing Then Dibujar(SubPoligonos(1), Color.Blue)

            Dibujar(Intersecciones, Color.White)
            Dibujar(Poligono2D.Recorte(P(0), P(1)), Color.White)
            PoligonosAlgoritmoRecorte(P(0), P(1))
            'SubPoligonos = Poligono2D.PoligonosAlgoritmoRecorte(P(0), P(1))
            'Dibujar(SubPoligonos(0), Color.LightBlue)
            'Dibujar(SubPoligonos(1), Color.LightGreen)
        Else
            Me.Text = "Motor3D 2.0 - Prueba Figura2D (Dibujando)"
            G.Clear(Color.Black)

            If Vertices.Count > 2 Then
                G.DrawPolygon(Pens.Red, Punto2D.ToPoint(Vertices.ToArray))
            End If
        End If

        Pic.Image = BMP
        Pic.Refresh()
    End Sub

    Private Sub Dibujar(ByRef AABB As AABB2D, ByVal Color As Color)
        G.DrawRectangle(New Pen(Color), New Rectangle(AABB.Posicion.ToPoint, AABB.Dimensiones.ToPoint))
    End Sub

    Private Sub Dibujar(ByRef Arbol As ArbolOBB)
        Dibujar(Arbol.Raiz)
    End Sub

    Private Sub Dibujar(ByRef Nodo As NodoArbolOBB)
        Dibujar(Nodo.OBB)
        If Not Nodo.EsHoja Then
            Dibujar(Nodo.HijoA)
            Dibujar(Nodo.HijoB)
        End If
    End Sub

    Private Sub Dibujar(ByRef OBB As OBB2D)
        Dim Esquinas(3) As Point

        Esquinas(0) = OBB.EsquinaSuperiorIzquierda.ToPoint
        Esquinas(1) = OBB.EsquinaSuperiorDerecha.ToPoint
        Esquinas(2) = OBB.EsquinaInferiorDerecha.ToPoint
        Esquinas(3) = OBB.EsquinaInferiorIzquierda.ToPoint

        G.DrawLine(Pens.Red, CInt(OBB.Centro.X), CInt(OBB.Centro.Y), CInt(OBB.EjeX.X + OBB.Centro.X), CInt(OBB.EjeX.Y + OBB.Centro.Y))
        G.DrawLine(Pens.Green, CInt(OBB.Centro.X), CInt(OBB.Centro.Y), CInt(OBB.EjeY.X + OBB.Centro.X), CInt(OBB.EjeY.Y + OBB.Centro.Y))
        G.DrawPolygon(Pens.SkyBlue, Esquinas)
    End Sub

    Private Sub Dibujar(ByRef Poligono As Poligono2D, ByVal Color As Color)
        If Poligono.NumeroLados > 0 Then
            G.FillPolygon(New SolidBrush(Color.FromArgb(128, Color)), Poligono.VerticesToPoint)
            G.DrawPolygon(New Pen(Color), Poligono.VerticesToPoint)
        End If
    End Sub

    Private Sub Dibujar(ByRef Poligonos() As Poligono2D, ByVal Color As Color)
        For i As Integer = 0 To Poligonos.GetUpperBound(0)
            Dibujar(Poligonos(i), Color)
        Next
    End Sub

    Private Sub Dibujar(ByRef Puntos() As Punto2D, ByRef Color As Color)
        For i As Integer = 0 To Puntos.GetUpperBound(0)
            Dibujar(Puntos(i), Color)
        Next
    End Sub

    Private Sub Dibujar(ByRef Punto As Punto2D, ByRef Color As Color, Optional ByVal Relleno As Boolean = False)
        If Relleno Then
            G.FillEllipse(New SolidBrush(Color), New Rectangle(Punto.ToPoint.X - 3, Punto.ToPoint.Y - 3, 6, 6))
        Else
            G.DrawEllipse(New Pen(Color), New Rectangle(Punto.ToPoint.X - 3, Punto.ToPoint.Y - 3, 6, 6))
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        For i As Integer = 0 To P.GetUpperBound(0)
            If Agarrado Then
                If i = IndiceAgarre Then
                    P(i).Actualizar.AplicarTransformacion(PuntoAgarre)
                Else
                    P(i).Actualizar()
                End If
            Else
                P(i).Actualizar()
            End If
        Next

        If Agarrado Then
            P(IndiceAgarre).AplicarFuerza(New Vector2D(PuntoAgarre, Ancla) / 10, PuntoAgarre, 0)
        End If
        EvaluarMarco()
        Repintar()
    End Sub

    Private Sub Pic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseDown
        If Dibujando Then
            Vertices.Add(New Punto2D(e.Location.X, e.Location.Y))
        Else
            If Not Agarrado Then
                If P(0).Pertenece(New Punto2D(e.X, e.Y)) Then
                    PuntoAgarre = New Punto2D(e.X, e.Y)
                    Ancla = New Punto2D(e.X, e.Y)
                    Agarrado = True
                    IndiceAgarre = 0
                Else
                    If P(1).Pertenece(New Punto2D(e.X, e.Y)) Then
                        PuntoAgarre = New Punto2D(e.X, e.Y)
                        Ancla = New Punto2D(e.X, e.Y)
                        Agarrado = True
                        IndiceAgarre = 1
                    Else
                        Agarrado = False
                    End If
                End If
            End If
        End If
    End Sub

    Public Function ColorProporcional(ByVal Minimo As Double, ByVal Maximo As Double, ByVal Valor As Double) As Color
        Dim Rango As Double
        Maximo = Maximo + (Maximo / 6)
        Rango = Maximo - Minimo
        Dim r, g, b As Byte
        Dim m, n As Double
        Select Case Valor
            Case 0 To (Rango / 6)
                m = (Rango / 6)
                n = Math.Abs(Valor - m)
                r = 255
                g = 255 - ((255 / (Rango / 6)) * n)
                b = 0
                'OK
            Case (Rango / 6) + 1 To (Rango / 6) * 2
                m = (Rango / 6) * 2
                n = Math.Abs(Valor - m)
                r = (255 / (Rango / 6)) * n
                g = 255
                b = 0
                'OK
            Case ((Rango / 6) * 2) + 1 To (Rango / 6) * 3
                m = (Rango / 6) * 3
                n = Math.Abs(Valor - m)
                r = 0
                g = 255
                b = 255 - ((255 / (Rango / 6)) * n)
                'OK
            Case ((Rango / 6) * 3) + 1 To (Rango / 6) * 4
                m = (Rango / 6) * 4
                n = Math.Abs(Valor - m)
                r = 0
                g = (255 / (Rango / 6)) * n
                b = 255
                'OK
            Case ((Rango / 6) * 4) + 1 To (Rango / 6) * 5
                m = (Rango / 6) * 5
                n = Math.Abs(Valor - m)
                r = 255 - ((255 / (Rango / 6)) * n)
                g = 0
                b = 255
                'OK
            Case ((Rango / 6) * 5) + 1 To (Rango / 6) * 6
                m = (Rango / 6) * 6
                n = Math.Abs(Valor - m)
                r = 255
                g = 0
                b = (255 / (Rango / 6)) * n
                'OK
        End Select
        Return Color.FromArgb(r, g, b)
    End Function

    Private Sub Form1_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If Not (Pic.Width = 0 OrElse Pic.Height = 0) AndAlso Not Quadtree Is Nothing Then
            Quadtree.Espacio = New AABB2D(New Punto2D(), New Punto2D(Pic.Width, Pic.Height))
        End If
    End Sub

    Private Sub Pic_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        Pertenece = P(Indice).Arbol.Pertenece(New Punto2D(e.X, e.Y))
        If Agarrado Then Ancla = New Punto2D(e.X, e.Y)
    End Sub

    Public Function PuntoPertenece(ByRef Poligono As Poligono2D, ByRef Punto As Punto2D) As Boolean
        Dim Fase1 As New List(Of Integer)
        Dim Recta As New Recta2D(Punto, Poligono.Lados(0).PuntoMedio)
        Dim Interseccion As Punto2D
        Dim Total As Integer = 1
        Dim PosicionRelativa As PosicionRelativa2D

        RectaJordan = New Recta2D(Punto, Recta.VectorDirector)
        PuntosJordan.Clear()
        PuntosJordan.Add(Poligono.Lados(0).PuntoMedio)

        'NOTA: Por la definición de la recta, ya sabemos que Segmentos(0) interseca
        For i As Integer = 1 To Poligono.Lados.GetUpperBound(0)
            If New Vector2D(Punto, Poligono.Lados(i).PuntoMedio) * Recta.VectorDirector >= 0 AndAlso Poligono.Lados(i).AABB.Pertenece(Recta) Then
                Fase1.Add(i)
                PuntosJordan.Add(Poligono.Lados(i).PuntoMedio)
            End If
        Next

        For i As Integer = 0 To Fase1.Count - 1
            PosicionRelativa = Recta2D.PosicionRelativa(Recta, Poligono.Lados(Fase1(i)).Recta)

            If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante Then
                Interseccion = PosicionRelativa.Interseccion
                PuntosJordan.Add(Interseccion)
                'NOTA: Si los vectores ExtremoIniciañ -> Interseccion y ExtremoFinal -> Interseccion están enfrentados, la interseccion pertenece al segmento:
                If ((Interseccion.X - Poligono.Lados(Fase1(i)).ExtremoInicial.X) * (Interseccion.X - Poligono.Lados(Fase1(i)).ExtremoFinal.X)) + ((Interseccion.Y - Poligono.Lados(Fase1(i)).ExtremoInicial.Y) * (Interseccion.Y - Poligono.Lados(Fase1(i)).ExtremoFinal.Y)) < 0 Then
                    Total += 1
                End If
            End If
        Next

        Return Total Mod 2 <> 0
    End Function

    Private Sub Pic_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseUp
        If Agarrado Then
            Agarrado = False
        End If
    End Sub

    Private Sub EvaluarMarco()
        Dim AABB As AABB2D

        For i As Integer = 0 To P.GetUpperBound(0)
            AABB = P(i).AABB

            If AABB.MinX < Marco.MinX Then P(i).Velocidad = New Vector2D(Abs(P(i).Velocidad.X), P(i).Velocidad.Y)
            If AABB.MinY < Marco.MinY Then P(i).Velocidad = New Vector2D(P(i).Velocidad.X, Abs(P(i).Velocidad.Y))

            If AABB.MaxX > Marco.MaxX Then P(i).Velocidad = New Vector2D(-Abs(P(i).Velocidad.X), P(i).Velocidad.Y)
            If AABB.MaxY > Marco.MaxY Then P(i).Velocidad = New Vector2D(P(i).Velocidad.X, -Abs(P(i).Velocidad.Y))
        Next
    End Sub

    Public Sub PoligonosAlgoritmoRecorte(ByVal P1 As Poligono2D, ByVal P2 As Poligono2D)
        Dim Actual As VerticeRecorte
        Dim Interseccion As VerticeRecorte
        Dim L1 As VerticeRecorte
        Dim L2 As VerticeRecorte
        Dim PosicionRelativa As PosicionRelativa2D

        Dim A1(P1.NumeroLados - 1) As NodoArbolSegmentos
        Dim A2(P2.NumeroLados - 1) As NodoArbolSegmentos

        If AABB2D.Colision(P1.AABB, P2.AABB) Then
            L1 = New VerticeRecorte(P1.Lados(0).ExtremoInicial)
            L2 = New VerticeRecorte(P2.Lados(0).ExtremoInicial)

            Actual = L1
            For i As Integer = 0 To P1.NumeroLados - 1
                Actual.SiguienteEnRecorte = New VerticeRecorte(P1.Lados(i).ExtremoFinal)
                A1(i) = New NodoArbolSegmentos(P1.Lados(i), Actual, Actual.SiguienteEnRecorte)
                Actual = Actual.SiguienteEnRecorte
            Next

            Actual = L2
            For i As Integer = 0 To P2.NumeroLados - 1
                Actual.SiguienteEnRecortado = New VerticeRecorte(P2.Lados(i).ExtremoFinal)
                A2(i) = New NodoArbolSegmentos(P2.Lados(i), Actual, Actual.SiguienteEnRecortado)
                Actual = Actual.SiguienteEnRecortado
            Next

            For i As Integer = 0 To P1.NumeroLados - 1
                For j As Integer = 0 To P2.NumeroLados - 1
                    PosicionRelativa = Recta2D.PosicionRelativa(P1.Lados(i).Recta, P2.Lados(j).Recta)

                    If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante AndAlso P1.Lados(i).AABB.Pertenece(PosicionRelativa.Interseccion) AndAlso P2.Lados(j).AABB.Pertenece(PosicionRelativa.Interseccion) Then
                        Interseccion = New VerticeRecorte(PosicionRelativa.Interseccion, True)
                        A1(i).InsertarInterseccion(Interseccion, True)
                        A2(j).InsertarInterseccion(Interseccion, False)
                    End If
                Next
            Next

            Dim Pen As New Pen(Color.Red)
            Pen.EndCap = Drawing2D.LineCap.ArrowAnchor
            Pen.Width = 5

            Actual = L1
            While Not Actual Is Nothing
                If Not Actual.SiguienteEnRecorte Is Nothing Then G.DrawLine(Pen, Actual.Vertice.ToPoint, Actual.SiguienteEnRecorte.Vertice.ToPoint)
                Actual = Actual.SiguienteEnRecorte
            End While

            Pen = New Pen(Brushes.Green)
            Pen.EndCap = Drawing2D.LineCap.ArrowAnchor
            Pen.Width = 5

            Actual = L2
            While Not Actual Is Nothing
                If Not Actual.SiguienteEnRecortado Is Nothing Then G.DrawLine(Pen, Actual.Vertice.ToPoint, Actual.SiguienteEnRecortado.Vertice.ToPoint)
                Actual = Actual.SiguienteEnRecortado
            End While
        End If
    End Sub

    Private Sub PosicionarEnRecorte(ByRef Recta As Recta2D, ByRef Inicio As VerticeRecorte, ByRef Fin As VerticeRecorte, ByRef Vertice As VerticeRecorte)
        Dim Parametro As Double

        If Inicio.SiguienteEnRecorte Is Fin Then
            Vertice.SiguienteEnRecorte = Fin
            Inicio.SiguienteEnRecorte = Vertice
        Else
            Parametro = Recta.ObtenerParametro(Vertice.Vertice)

            If Parametro < Recta.ObtenerParametro(Inicio.SiguienteEnRecorte.Vertice) Then
                PosicionarEnRecorte(Recta, Inicio.SiguienteEnRecorte, Fin, Vertice)
            Else
                Vertice.SiguienteEnRecorte = Inicio.SiguienteEnRecorte.SiguienteEnRecorte
                Inicio.SiguienteEnRecorte.SiguienteEnRecorte = Vertice
            End If
        End If
    End Sub
End Class
