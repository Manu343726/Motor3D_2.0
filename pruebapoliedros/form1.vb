Imports Motor3D.Primitivas3D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Espacio2D
Imports Motor3D.Escena
Imports Motor3D.Escena.Renders.GDI

Imports System.Math

Public Class Form1
    Dim Camara As Camara3D
    Dim Cubos As New List(Of Poliedro)
    Dim Foco As Foco3D

    Dim RND As New Random

    Dim BMP As Bitmap
    Dim g As Drawing.Graphics

    Dim Motor As Motor3D.Escena.Motor3D

    Dim Debug As Boolean = False
    Dim Octree As Octree

    Dim Recta As Recta3D
    Dim Rotacion As Rotacion
    Dim SectoresRecta() As SectorOctree
    Dim Pen As Pen

    Dim CuboSeleccionado As Integer = 0

    Dim PosRaton As New Punto2D

    Dim Eje As Recta3D

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            If e.Shift Then
                Select Case e.KeyCode
                    Case Keys.W
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeX, 0.02)
                    Case Keys.S
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeX, -0.02)
                    Case Keys.A
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeY, 0.02)
                    Case Keys.D
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeY, -0.02)
                    Case Keys.R
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeZ, 0.02)
                    Case Keys.F
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeZ, -0.02)
                    Case Keys.Up
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(10, 0, 0))
                    Case Keys.Down
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(-10, 0, 0))
                    Case Keys.Right
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(0, 10, 0))
                    Case Keys.Left
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(0, -10, 0))
                    Case Keys.O
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(0, 0, 10))
                    Case Keys.L
                        Cubos(CuboSeleccionado).AplicarTransformacion(New Traslacion(0, 0, -10))
                    Case Keys.Enter
                End Select
            Else
                Select Case e.KeyCode
                    Case Keys.Up
                        Camara.TrasladarSobreSUR(New Vector3D(0, 0, 100))
                    Case Keys.Down
                        Camara.TrasladarSobreSUR(New Vector3D(0, 0, -100))
                    Case Keys.Right
                        Camara.TrasladarSobreSUR(New Vector3D(10, 0, 0))
                    Case Keys.Left
                        Camara.TrasladarSobreSUR(New Vector3D(-100, 0, 0))
                    Case Keys.O
                        Camara.TrasladarSobreSUR(New Vector3D(0, 100, 0))
                    Case Keys.L
                        Camara.TrasladarSobreSUR(New Vector3D(0, -100, 0))
                    Case Keys.W
                        Camara.RotarSobreSUR(EnumEjes.EjeX, 0.02)
                    Case Keys.S
                        Camara.RotarSobreSUR(EnumEjes.EjeX, -0.02)
                    Case Keys.A
                        Camara.RotarSobreSUR(EnumEjes.EjeY, 0.02)
                    Case Keys.D
                        Camara.RotarSobreSUR(EnumEjes.EjeY, -0.02)
                    Case Keys.R
                        Camara.RotarSobreSUR(EnumEjes.EjeZ, 0.02)
                    Case Keys.F
                        Camara.RotarSobreSUR(EnumEjes.EjeZ, -0.02)
                    Case Windows.Forms.Keys.Enter

                End Select
            End If

        Else
            Select Case e.KeyCode
                Case Keys.Up
                    Camara.TrasladarSobreSRC(New Vector3D(0, 0, 100))
                Case Keys.Down
                    Camara.TrasladarSobreSRC(New Vector3D(0, 0, -100))
                Case Keys.Right
                    Camara.TrasladarSobreSRC(New Vector3D(100, 0, 0))
                Case Keys.Left
                    Camara.TrasladarSobreSRC(New Vector3D(-100, 0, 0))
                Case Keys.NumPad8
                    Camara.TrasladarSobreSRC(New Vector3D(0, 100, 0))
                Case Keys.NumPad2
                    Camara.TrasladarSobreSRC(New Vector3D(0, -100, 0))
                Case Keys.W
                    Camara.RotarSobreSRC(EnumEjes.EjeX, 0.02)
                Case Keys.S
                    Camara.RotarSobreSRC(EnumEjes.EjeX, -0.02)
                Case Keys.A
                    Camara.RotarSobreSRC(EnumEjes.EjeY, 0.02)
                Case Keys.D
                    Camara.RotarSobreSRC(EnumEjes.EjeY, -0.02)
                Case Keys.R
                    Camara.RotarSobreSRC(EnumEjes.EjeZ, 0.02)
                Case Keys.F
                    Camara.RotarSobreSRC(EnumEjes.EjeZ, -0.02)
                Case Keys.Add
                    Camara.Distancia += 100
                Case Keys.Subtract
                    Camara.Distancia -= 100
                Case Keys.P
                    Timer1.Enabled = Not Timer1.Enabled
                Case Keys.Enter
                    CrearCubo()
                Case Keys.Back
                    If Cubos.Count > 0 Then EliminarCubo()
                Case Keys.Tab
                    Debug = Not Debug
                Case Keys.Escape
                    End
                Case Keys.Z
                    If CuboSeleccionado >= 1 Then CuboSeleccionado -= 1
                Case Keys.X
                    If CuboSeleccionado < Cubos.Count - 1 Then CuboSeleccionado += 1
            End Select
        End If

        Repintar()
    End Sub

    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        If Not (Camara Is Nothing OrElse (Pic.Width = 0 OrElse Pic.Height = 0)) Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Drawing.Graphics.FromImage(BMP)
            g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)

            Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)

            'Render.Redimensionar(Pic.Width, Pic.Height)
            Repintar()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Motor.IniciarEscena()
        Octree.Refrescar()
        For i As Integer = 0 To Motor.NumeroPoliedros - 1
            Motor.Poliedros(i) *= Rotacion '+ Transformacion3D.RotacionSobreSUR(EnumEjes.EjeY, 0.01) + Transformacion3D.RotacionSobreSUR(EnumEjes.EjeX, 0.01))
        Next

        Motor.FinalizarEscena()

        Me.Text = "Motor3D 2.0 - Prueba de renderizado (" & Motor.NumeroPoliedros & " poliedros, " & Motor.ZBuffer.NumeroObjetos & " caras visibles [Tiempo calculo ZBuffer=" & Motor.ZBuffer.TiempoUltimoCalculo.ToString & "ms])"

        Repintar()
    End Sub

    Private Sub RecalcularRepresentacions()
        For i As Integer = 0 To Cubos.Count - 1
            Cubos(i).RecalcularRepresentaciones(Camara)
        Next
    End Sub

    Private Sub MoverCamara(ByVal DeltaX As Integer, ByVal DeltaY As Integer)
        Dim Pos As Punto3D = Camara.Posicion
        Camara.TrasladarSobreSUR(New Punto3D)
        Camara.RotarSobreSRC(EnumEjes.EjeX, DeltaX / 1)
        'Camara.RotarSobreSRC(EnumEjes.EjeY, DeltaY / 1)
        Camara.TrasladarSobreSUR(Pos)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Interval = 1
        BMP = New Bitmap(Pic.Width, Pic.Height)
        g = Drawing.Graphics.FromImage(BMP)
        g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)
        Pen = New Drawing.Pen(Drawing.Color.Red)

        Eje = New Recta3D(New Punto3D(100, 100, 100), New Vector3D(0, 0, 1))
        Rotacion = New Rotacion(0.02, Eje)

        Camara = New Camara3D
        Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)
        Camara.EstablecerDimensionesFrustum(50000, 50000, 50000)
        Foco = New Foco3D(New Punto3D(), Drawing.Color.White, 1)
        Octree = New Octree(5, New AABB3D(-1000, -1000, -1000, 2000, 2000, 2000))
        Recta = New Recta3D(New Punto3D(-100, -100, -100), New Vector3D(0, 0, 1))
        'SectoresRecta = Octree.Sectores(Recta)

        Motor = New Motor3D.Escena.Motor3D
        Motor.AñadirCamara(Camara)
        Motor.AñadirFoco(Foco)
        'Motor.AñadirFoco(New Foco3D(New Punto3D(), Color.Red, 1))
        'Motor.AñadirFoco(New Foco3D(New Punto3D(0, 1000, 0), Color.Green))
        'Motor.AñadirFoco(New Foco3D(New Punto3D(0, 0, 1000), Color.Blue))
        Motor.ShadingActivado = True

        For i As Integer = -1000 To 1000 Step 600
            For j As Integer = -1000 To 1000 Step 600
                CrearCubo(New Vector3D(i, j, 0))
            Next
        Next

        'CrearCubo(New Vector3D())

        'Render = New RenderDx_lib32(Motor, Pic.Handle, Pic.Width, Pic.Height)

        'Dim Esfera As Poliedro = Poliedro.Esfera(20)
        'Esfera *= New Escalado(1000)
        'Esfera.EstablecerColor(Color.White)

        'Motor.AñadirPoliedro(Esfera)
    End Sub

    Private Sub CrearCubo()
        Dim t As Double
        RND = New Random

        t = RND.Next(800, 800)

        CrearCubo(New Vector3D(t, t, t))
    End Sub

    Private Sub CrearCubo(ByVal Posicion As Vector3D)
        Dim C As Poliedro = Poliedro.Cubo()

        C.AplicarTransformacion(New Escalado(100, 100, 100) + New Traslacion(Posicion))
        C.EstablecerColor(0, Drawing.Color.Red)
        C.EstablecerColor(1, Drawing.Color.Green)
        C.EstablecerColor(2, Drawing.Color.Blue)
        C.EstablecerColor(3, Drawing.Color.Orange)
        C.EstablecerColor(4, Drawing.Color.White)
        C.EstablecerColor(5, Drawing.Color.Purple)
        Cubos.Add(C)

        Motor.AñadirPoliedro(C)
    End Sub

    Private Sub EliminarCubo()
        Motor.QuitarPoliedro(Cubos(Cubos.Count - 1))
        Cubos.RemoveAt(Cubos.Count - 1)
    End Sub


    Private Sub Repintar()
        Dim Poligono As Poligono2D
        Dim Sector As SectorOctree

        g.Clear(Drawing.Color.Black)

        If Not Motor.ZBuffer.Vacio Then
            For Each Elemento As ElementoZBuffer In Motor.ZBuffer.Objetos
                Poligono = Motor.ZBuffer.Represenatciones(Elemento.Indices(0))

                g.FillPolygon(New SolidBrush(Poligono.Color), Poligono.VerticesToPoint)
            Next

            If Debug Then
                For i As Integer = 0 To Motor.NumeroPoliedros - 1
                    If Motor.ZBuffer.Pertenece(1, i) <> -1 Then
                        If Motor.Poliedros(i).AutoRecalcularAABBs Then
                            For Each Pol As Poligono2D In Motor.Poliedros(i).AABBSUR.Representacion(Motor.CamaraSeleccionada)
                                g.DrawPolygon(Pens.Blue, Pol.VerticesToPoint)
                            Next
                            For Each Pol As Poligono2D In Motor.Poliedros(i).AABBSRC.Representacion(Motor.CamaraSeleccionada, True)
                                g.DrawPolygon(Pens.GreenYellow, Pol.VerticesToPoint)
                            Next

                            If Octree.Pertenece(Motor.Poliedros(i), True) Then
                                Sector = Octree.Sector(Motor.Poliedros(i), True)

                                Do While True
                                    If Sector.Colision Then
                                        Pen.Color = Drawing.Color.Green
                                    Else
                                        Pen.Color = Drawing.Color.Red
                                    End If

                                    For Each Pol As Poligono2D In Sector.Espacio.Representacion(Motor.CamaraSeleccionada)
                                        g.DrawPolygon(Pen, Pol.VerticesToPoint)
                                    Next
                                    Sector = Sector.Padre
                                    If Sector Is Nothing Then Exit Do
                                Loop
                            End If

                            'DibujarSectorOctree(Octree.SectorRaiz)
                        Else
                            Motor.Poliedros(i).RecalcularAABBSUR()
                            'Motor.Poliedros(i).RecalcularAABBSRC()
                            If Octree.Pertenece(Motor.Poliedros(i), True) Then
                                Sector = Octree.Sector(Motor.Poliedros(i), True)

                                Do While True
                                    If Sector.Colision Then
                                        Pen.Color = Drawing.Color.Green
                                    Else
                                        Pen.Color = Drawing.Color.Red
                                    End If

                                    For Each Pol As Poligono2D In Sector.Espacio.Representacion(Motor.CamaraSeleccionada)
                                        g.DrawPolygon(Pen, Pol.VerticesToPoint)
                                    Next
                                    Sector = Sector.Padre
                                    If Sector Is Nothing Then Exit Do
                                Loop

                                'For Each SectorRecta As SectorOctree In SectoresRecta
                                '    For Each Pol As Poligono2D In SectorRecta.AABB.Representacion(Motor.CamaraSeleccionada)
                                '        g.DrawPolygon(Pens.Gray, Pol.VerticesToPoint)
                                '    Next
                                'Next
                            End If

                            'DibujarSectorOctree(Octree.SectorRaiz)

                            For Each Pol As Poligono2D In Motor.Poliedros(i).AABBSUR.Representacion(Motor.CamaraSeleccionada)
                                g.DrawPolygon(Pens.Blue, Pol.VerticesToPoint)
                            Next
                            'For Each Pol As Poligono2D In Motor.Poliedros(i).AABBSRC.Representacion(Motor.CamaraSeleccionada, True)
                            '    g.DrawPolygon(Pens.GreenYellow, Pol.VerticesToPoint)
                            'Next
                        End If
                    End If
                Next
            End If
        End If

        If Debug Then
            g.DrawString("Número de cámaras=" & Motor.NumeroCamaras.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 10 - (Pic.Height / 2))
            g.DrawString("Número de poliedros=" & Motor.NumeroPoliedros.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 25 - (Pic.Height / 2))
            g.DrawString("Número de focos=" & Motor.NumeroFocos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 40 - (Pic.Height / 2))
            g.DrawString("Tamaño del buffer=" & Motor.ZBuffer.NumeroObjetos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 55 - (Pic.Height / 2))
            g.DrawString("Posición de la cámara=" & Camara.Posicion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 70 - (Pic.Height / 2))
            g.DrawString("Dirección de la cámara=" & Camara.VectorDireccion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 85 - (Pic.Height / 2))
            g.DrawString("Punto de mira de la cámara=" & Camara.PuntoDeMira.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 100 - (Pic.Height / 2))
            g.DrawString("Frustum=" & Camara.Frustum.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 115 - (Pic.Height / 2))
            g.DrawString("Transformación: " & vbNewLine & Camara.TransformacionSURtoSRC.Matriz.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 130 - (Pic.Height / 2))
        End If

        Pic.Image = BMP
        Pic.Refresh()
    End Sub

    Private Sub DibujarSectorOctree(ByVal Sector As SectorOctree)
        For Each Pol As Poligono2D In Sector.Espacio.Representacion(Motor.CamaraSeleccionada)
            g.DrawPolygon(Pens.Red, Pol.VerticesToPoint)
        Next

        If Not Sector.EsHoja Then
            For i As Integer = 0 To 7
                DibujarSectorOctree(Sector.Hijos(i))
            Next
        End If
    End Sub

    Private Sub Pic_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            Eje = New Recta3D(New Punto3D, New Punto3D(e.Location.X - (Pic.Width / 2), e.Location.Y - (Pic.Height / 2), Camara.Distancia)) * Camara.TransformacionSRCtoSUR
            Rotacion = New Rotacion(0.02, Eje)

            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    Motor.Focos(0).Coordenadas = New Plano3D(Motor.CamaraSeleccionada.Posicion, Motor.CamaraSeleccionada.VectorDireccion).ObtenerPunto(e.Location.X, e.Location.Y)
                    'Repintar()
            End Select
        End If
    End Sub

    Public Function CaraSeñalada(ByVal Buffer As ZBuffer, ByVal Recta As Recta3D) As Integer

    End Function
End Class
