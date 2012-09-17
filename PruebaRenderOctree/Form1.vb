Imports Motor3D.Primitivas3D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Espacio2D
Imports Motor3D.Escena

Imports System.Math

Public Class Form1
    Dim Camara As Camara3D
    Dim Cubos As New List(Of Poliedro)
    Dim Foco As Foco3D

    Dim RND As New Random

    Dim BMP As Bitmap
    Dim g As Graphics

    Dim Motor As Motor3D.Escena.Motor3D

    Dim Debug As Boolean = False
    Dim Octree As OctreeGrafico

    Dim Recta As Recta3D
    Dim Pen As Pen

    Dim CuboSeleccionado As Integer = 0
    Dim Punto As Punto3D
    Dim SectorSeleccionado As SectorOctreeGrafico
    Dim ColorRelleno As Color
    Dim Sectores() As SectorOctreeGrafico

    Dim VerRecta As Boolean = False

    Dim Rastrear As Boolean = False

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
                        Punto.Y += (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
                    Case Keys.Down
                        Punto.Y -= (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
                    Case Keys.Right
                        Punto.X += (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
                    Case Keys.Left
                        Punto.X -= (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
                    Case Keys.O
                        Punto.Z += (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
                    Case Keys.L
                        Punto.Z -= (1000 / (2 ^ (Octree.Niveles - 2)))
                        SectorSeleccionado = Octree.Sector(Punto)
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
                    SectorSeleccionado.Rellenar(ColorRelleno)
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
                Case Keys.C
                    If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                        ColorRelleno = ColorDialog1.Color
                    End If
                Case Keys.Q
                    Rastrear = Not Rastrear
                Case Keys.V
                    VerRecta = Not VerRecta
            End Select
        End If
        Repintar()
    End Sub

    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Not (Camara Is Nothing OrElse (Pic.Width = 0 OrElse Pic.Height = 0)) Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Graphics.FromImage(BMP)
            g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)

            Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)
            Repintar()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Interval = 1
        BMP = New Bitmap(Pic.Width, Pic.Height)
        g = Graphics.FromImage(BMP)
        g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)
        Pen = New Pen(Color.Red)

        Camara = New Camara3D
        Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)
        Foco = New Foco3D(New Punto3D(), Color.White, 1)
        Octree = New OctreeGrafico(5, New AABB3D(-1000, -1000, -1000, 2000, 2000, 2000))
        Recta = New Recta3D(New Punto3D(-100, -100, -100), New Punto3D(100, 100, 100))
        'SectoresRecta = Octree.Sectores(Recta)

        Motor = New Motor3D.Escena.Motor3D
        Motor.AñadirCamara(Camara)
        Motor.AñadirFoco(Foco)

        Punto = New Punto3D((1000 / (2 ^ (Octree.Niveles - 1))), (1000 / (2 ^ (Octree.Niveles - 1))), (1000 / (2 ^ (Octree.Niveles - 1))))
        SectorSeleccionado = Octree.Sector(Punto)
        ColorRelleno = Color.Yellow

        Repintar()

        Timer1.Start()
    End Sub

    Private Sub CrearCubo()
        Dim C As Poliedro = Poliedro.Cubo
        Dim t As Double
        RND = New Random

        t = RND.Next(-1000, 1000)

        C.AplicarTransformacion(New Escalado(100, 100, 100) + New Traslacion(t, t, t))
        Cubos.Add(C)

        Motor.AñadirPoliedro(C)
    End Sub

    Private Sub EliminarCubo()
        Motor.QuitarPoliedro(Cubos(Cubos.Count - 1))
        Cubos.RemoveAt(Cubos.Count - 1)
    End Sub


    Private Sub Repintar()
        Dim Poligono As Poligono2D
        Dim Sector As SectorOctreeGrafico

        g.Clear(Color.Black)

        If Not Motor.ZBuffer.Vacio Then
            For Each Elemento As ElementoZBuffer In Motor.ZBuffer.Objetos
                Poligono = Motor.ZBuffer.Represenatciones(Elemento.Indices(0))

                g.FillPolygon(New SolidBrush(Poligono.Color), Poligono.VerticesToPoint)
            Next
        End If



        If Debug AndAlso Octree.Pertenece(Punto) Then
            Sector = SectorSeleccionado
            Pen.Color = Color.Green

            DibujarSectorOctree(Octree.SectorRaiz)

            Do
                For Each Pol As Poligono2D In Sector.Espacio.Representacion(Motor.CamaraSeleccionada)
                    g.DrawPolygon(Pen, Pol.VerticesToPoint)
                Next

                Sector = Sector.Padre
                Pen.Color = Color.Red
            Loop Until Sector Is Nothing
        End If


        If Debug Then
            Dim P1, P2, P3 As Point
            P1 = Camara.Proyeccion(Recta.ObtenerPuntoParametrico(-10000), False).ToPoint
            P2 = Camara.Proyeccion(Recta.ObtenerPuntoParametrico(10000), False).ToPoint
            P3 = Camara.Proyeccion(AABB3D.PuntoCorteRecta(Octree.Espacio, Recta), False).ToPoint

            'P1 = New Point(P1.X + (Pic.Width / 2), P1.Y - (Pic.Height / 2))
            'P2 = New Point(P2.X + (Pic.Width / 2), P2.Y - (Pic.Height / 2))

            g.DrawString("Número de cámaras=" & Motor.NumeroCamaras.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 10 - (Pic.Height / 2))
            g.DrawString("Número de poliedros=" & Motor.NumeroPoliedros.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 25 - (Pic.Height / 2))
            g.DrawString("Número de focos=" & Motor.NumeroFocos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 40 - (Pic.Height / 2))
            g.DrawString("Tamaño del buffer=" & Motor.ZBuffer.NumeroObjetos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 55 - (Pic.Height / 2))
            g.DrawString("Posición de la cámara=" & Camara.Posicion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 70 - (Pic.Height / 2))
            g.DrawString("Dirección de la cámara=" & Camara.VectorDireccion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 85 - (Pic.Height / 2))
            g.DrawString("Punto de mira de la cámara=" & Camara.PuntoDeMira.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 100 - (Pic.Height / 2))
            g.DrawString("Recta de señalado=" & Recta.ToString & "  Dirección: " & Recta.VectorDirector.ToString & "  (Extremos: " & P1.ToString & " ; " & P2.ToString & ")", Me.Font, Brushes.White, 10 - (Pic.Width / 2), 115 - (Pic.Height / 2))
            g.DrawString("Frustum=" & Camara.Frustum.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 130 - (Pic.Height / 2))
            g.DrawString("Transformación: " & vbNewLine & Camara.TransformacionSURtoSRC.Matriz.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 145 - (Pic.Height / 2))

            'g.DrawLine(Pens.White, P1, P2)
            'g.FillEllipse(Brushes.White, New Rectangle(P1.X - 2, P1.Y - 2, 4, 4))
            'g.FillEllipse(Brushes.White, New Rectangle(P2.X - 2, P2.Y - 2, 4, 4))
            'g.FillEllipse(Brushes.Orange, New Rectangle(P3.X - 2, P3.Y - 2, 4, 4))


            If Octree.SectorRaiz.Espacio.Pertenece(Recta) Then
                For Each Pol As Poligono2D In Octree.SectorRaiz.Espacio.Representacion(Motor.CamaraSeleccionada)
                    g.DrawPolygon(Pens.Purple, Pol.VerticesToPoint)
                Next
            Else
                For Each Pol As Poligono2D In Octree.SectorRaiz.Espacio.Representacion(Motor.CamaraSeleccionada)
                    g.DrawPolygon(Pens.Blue, Pol.VerticesToPoint)
                Next
            End If

            If VerRecta AndAlso Not Sectores Is Nothing Then
                For Each SectorGrafico As SectorOctreeGrafico In Sectores
                    For Each Pol As Poligono2D In SectorGrafico.Espacio.Representacion(Motor.CamaraSeleccionada)
                        g.DrawPolygon(Pens.White, Pol.VerticesToPoint)
                    Next
                Next
            End If
        End If

        Pic.Image = BMP
        Pic.Refresh()
    End Sub

    Private Sub DibujarSectorOctree(ByVal Sector As SectorOctreeGrafico)
        If Not Sector.Vacio Then
            For Each Pol As Poligono2D In Sector.Espacio.Representacion(Motor.CamaraSeleccionada)
                'g.FillPolygon(New SolidBrush(Sector.Color), Pol.VerticesToPoint)
                g.DrawPolygon(New Pen(Sector.Color), Pol.VerticesToPoint)
            Next
        End If

        If Not Sector.EsHoja AndAlso Sector.Vacio Then
            For i As Integer = 0 To 7
                DibujarSectorOctree(Sector.Hijos(i))
            Next
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        'Recta *= Transformacion3D.RotacionSobreSUR(EnumEjes.EjeY, 0.1)
        'Sectores = Octree.Sectores(Recta)
        Repintar()
    End Sub

    Private Sub Pic_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                SectorSeleccionado.Rellenar(ColorRelleno)
            Case Windows.Forms.MouseButtons.Right
                SectorSeleccionado.Vaciar()
        End Select
    End Sub

    Private Sub Pic_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        If Debug Then
            Recta = New Recta3D(New Punto3D, New Punto3D(e.Location.X - (Pic.Width / 2), e.Location.Y - (Pic.Height / 2), Camara.Distancia)) * Camara.TransformacionSRCtoSUR
            If Rastrear Then
                Try
                    SectorSeleccionado = Octree.Sector(Recta)
                    Sectores = Octree.Sectores(Recta)
                Catch ex As ExcepcionGeometrica3D
                    'Naaaaaaaaaaaa
                End Try
            End If
        End If
    End Sub
End Class
