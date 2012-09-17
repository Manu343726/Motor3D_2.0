Imports Motor3D.Espacio2D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Primitivas3D
Imports Motor3D.Escena

Public Class Form1

    Dim V1, V2, V3, V4 As Vector3D
    Dim P1, P2 As Punto2D

    Dim BMP As Bitmap
    Dim g As Graphics
    Dim pen1, pen2, pen3, pen4 As Pen

    Dim poli As Poligono2D

    Dim Punto As Punto3D
    Dim Trans As Transformacion3D

    Dim Cubo As Poliedro
    Dim Motor As Motor3D.Escena.Motor3D
    Dim Camara As Camara3D
    Dim Foco As Foco3D

    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter

            Case Keys.Escape
                End
        End Select
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        pen1 = New Pen(Brushes.Red, 5)
        pen2 = New Pen(Brushes.Green, 5)
        pen3 = New Pen(Brushes.Orange, 5)
        pen4 = New Pen(Brushes.Blue, 5)

        V1 = New Vector3D(0, 100, 0)
        V2 = New Vector3D(100, 0, 0)
        V3 = New Vector3D
        V4 = New Vector3D

        pen1.EndCap = Drawing2D.LineCap.ArrowAnchor
        pen2.EndCap = Drawing2D.LineCap.ArrowAnchor
        pen3.EndCap = Drawing2D.LineCap.ArrowAnchor
        pen4.EndCap = Drawing2D.LineCap.ArrowAnchor

        Camara = New Camara3D()
        Camara.TrasladarSobreSUR(New Punto3D(0, 0, -1000))

        Trans = New Transformacion3D(Motor3D.Algebra.Matriz.MatrizUnitaria(4))

        poli = New Poligono2D(New Punto2D(0, 0), New Punto2D(300, 300), New Punto2D(300, 0))
    End Sub

    Private Sub Pic_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                V1 = New Vector3D(e.Location.X - (Pic.Width / 2), e.Location.Y - (Pic.Height / 2), 1)
            Case Windows.Forms.MouseButtons.Right
                V2 = New Vector3D(e.Location.X - (Pic.Width / 2), e.Location.Y - (Pic.Height / 2), 1)
        End Select

        Trans = New Rotacion(V1, V2)
        V3 = V1 * Trans
        Redibujar()
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

    Private Sub Redibujar()
        Dim n As Integer = 0

        If Not g Is Nothing Then
            g.Clear(Color.Black)

            g.DrawLine(pen1, 0, 0, CLng(V1.X), CLng(V1.Y))
            g.DrawLine(pen2, 0, 0, CLng(V2.X), CLng(V2.Y))
            g.DrawLine(pen3, 0, 0, CLng(V3.X), CLng(V3.Y))
            g.DrawLine(pen4, 0, 0, CLng(V4.X), CLng(V4.Y))

            g.DrawString("V1: " & V1.ToString & vbNewLine & _
                         "V2: " & V2.ToString & vbNewLine & _
                         "V3: " & V3.ToString & vbNewLine & _
                         "V4: " & V4.ToString & vbNewLine & _
                         "Rotacion V1 --> V2: " & Trans.ToString _
                         , Me.Font, Brushes.White, 10 - (Pic.Width / 2), 20 - (Pic.Height / 2))

            Pic.Refresh()
        End If
    End Sub
End Class
