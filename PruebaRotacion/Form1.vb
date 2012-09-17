Imports Motor3D.Espacio2D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Primitivas3D
Imports Motor3D.Escena

Public Class Form1
    Dim g As Graphics
    Dim BMP As Bitmap

    Dim Motor As Motor3D.Escena.Motor3D
    Dim Camara As Camara3D

    Dim C As Poliedro = Poliedro.Cubo

    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
                Case Keys.Escape
                    End
            End Select
        End If

        C.Vertical = Camara.VectorDireccion
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Camara = New Camara3D()
        Motor = New Motor3D.Escena.Motor3D

        C.AplicarTransformacion(New Escalado(100, 100, 100))

        C.EstablecerColor(0, Color.Red)
        C.EstablecerColor(1, Color.Green)
        C.EstablecerColor(2, Color.Blue)
        C.EstablecerColor(3, Color.Orange)
        C.EstablecerColor(4, Color.White)
        C.EstablecerColor(5, Color.Purple)

        Motor.AñadirCamara(Camara)
        Motor.AñadirPoliedro(C)

        AddHandler Motor.Actualizado, AddressOf Redibujar
    End Sub

    Private Sub Redibujar(ByRef Buffer As ZBuffer)
        If Not g Is Nothing Then
            g.Clear(Color.Black)

            If Not Buffer.Vacio Then
                For Each Pol As Poligono2D In Buffer.Represenatciones
                    g.FillPolygon(New SolidBrush(Pol.Color), Pol.VerticesToPoint(True))
                Next
            End If

            Pic.Refresh()
        End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Not Pic Is Nothing AndAlso Pic.Width > 0 AndAlso Pic.Height > 0 Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Graphics.FromImage(BMP)
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)
            Pic.Image = BMP
        End If
    End Sub
End Class
