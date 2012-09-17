Imports Motor3D.Espacio2D
Imports System.Math

Public Class Form1
    Dim BMP As Bitmap
    Dim g As Graphics

    Dim c1, c2 As Circunferencia2D
    Dim r As Recta2D

    Dim r1, r2 As AABB2D

    Dim PosRaton As Point

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(0, 0)
        Me.Size = Screen.PrimaryScreen.WorkingArea.Size

        BMP = New Bitmap(Pic.Width, Pic.Height)
        g = Graphics.FromImage(BMP)

        c1 = New Circunferencia2D(New Punto2D(200, 200), 100)
        c2 = New Circunferencia2D(New Punto2D(300, 300), 50)
        r = Circunferencia2D.EjeRadical(c1, c2)

        r1 = New AABB2D(New Punto2D(150, 150), New Punto2D(50, 200))
        r2 = New AABB2D(New Punto2D(250, 300), New Punto2D(100, 150))

        RedibujarEscena()
    End Sub

    Private Sub Pic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        PosRaton = e.Location

        If e.Button = Windows.Forms.MouseButtons.Left Then
            If c1.ContenidoEnCirculo(New Punto2D(e.Location.X, e.Location.Y)) Then
                c1 = New Circunferencia2D(New Punto2D(e.Location.X, e.Location.Y), c1.Radio)
                r = Circunferencia2D.EjeRadical(c1, c2)
            Else
                If c2.ContenidoEnCirculo(New Punto2D(e.Location.X, e.Location.Y)) Then
                    c2 = New Circunferencia2D(New Punto2D(e.Location.X, e.Location.Y), c2.Radio)
                    r = Circunferencia2D.EjeRadical(c1, c2)
                End If
            End If

            If r1.Pertenece(New Punto2D(e.Location.X, e.Location.Y)) Then
                r1 = New AABB2D(New Punto2D(e.Location.X - (r1.LongitudX / 2), e.Location.Y - (r1.LongitudY / 2)), r1.Dimensiones)
            Else
                If r2.Pertenece(New Punto2D(e.Location.X, e.Location.Y)) Then
                    r2 = New AABB2D(New Punto2D(e.Location.X - (r2.LongitudX / 2), e.Location.Y - (r2.LongitudY / 2)), r2.Dimensiones)
                End If
            End If
        End If

        RedibujarEscena()
    End Sub

    Private Sub Pic_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseWheel

    End Sub

    Private Sub Pic_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Pic.Resize
        If Pic.Width > 0 AndAlso Pic.Height > 0 Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Graphics.FromImage(BMP)
        End If
    End Sub

    Private Sub DibujarCircunferencia(ByVal Circunferencia As Circunferencia2D, ByVal Color As Color)
        g.DrawEllipse(New Pen(Color), New Rectangle(Circunferencia.Centro.X - Circunferencia.Radio, Circunferencia.Centro.Y - Circunferencia.Radio, Circunferencia.Radio * 2, Circunferencia.Radio * 2))
        g.DrawString(Circunferencia.ToString, Me.Font, New SolidBrush(Color), CSng(Circunferencia.Centro.X), CSng(Circunferencia.Centro.Y))
    End Sub

    Private Sub DibujarRecta(ByVal Recta As Recta2D, ByVal Color As Color)
        Dim p1 As Punto2D = Recta.ObtenerPunto(c1.Centro.X)
        Dim p2 As Punto2D = Recta.ObtenerPunto(c2.Centro.X)

        g.DrawLine(New Pen(Color), CSng(p1.X), CSng(p1.Y), CSng(p2.X), CSng(p2.Y))
        g.DrawString(r.ToString, Me.Font, New SolidBrush(Color), CSng(p2.X), CSng(p2.Y))
    End Sub

    Private Sub DibujarRectangulo(ByVal Rectangulo As AABB2D, ByVal Color As Color)
        g.DrawRectangle(New Pen(Color), New Rectangle(CSng(Rectangulo.MinX), CSng(Rectangulo.MinY), CSng(Rectangulo.LongitudX), CSng(Rectangulo.LongitudY)))
        g.DrawString(Rectangulo.ToString, Me.Font, New SolidBrush(Color), CSng(Rectangulo.Centro.X), CSng(Rectangulo.Centro.Y))
    End Sub

    Private Sub RedibujarEscena()
        Dim Intcs As Boolean = (Circunferencia2D.PosicionRelativa(c1, c2) <= 0)
        Dim Intrs As Boolean = AABB2D.Colision(r1, r2)

        g.Clear(Color.Black)

        DibujarCircunferencia(c1, IIf(Intcs, Color.Green, Color.Red))
        DibujarCircunferencia(c2, IIf(Intcs, Color.Green, Color.Red))

        DibujarRectangulo(r1, IIf(Intrs, Color.Green, Color.Red))
        DibujarRectangulo(r2, IIf(Intrs, Color.Green, Color.Red))

        DibujarRecta(r, Color.Blue)

        g.DrawString("Ecuación C1: " & c1.ToString(True), Me.Font, Brushes.White, 10, 10)
        g.DrawString("Ecuación C2: " & c2.ToString(True), Me.Font, Brushes.White, 10, 25)
        g.DrawString("Eje radical: " & r.ToString, Me.Font, Brushes.White, 10, 40)
        g.DrawString("Dirección eje radical: " & r.VectorDirector.ToString, Me.Font, Brushes.White, 10, 55)

        Pic.Image = BMP
        Pic.Refresh()
    End Sub

    Private Sub Form1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If c1.ContenidoEnCirculo(New Punto2D(e.Location.X, e.Location.Y)) Then
            c1 = New Circunferencia2D(c1.Centro, c1.Radio + (Sign(e.Delta) * 5))
            r = Circunferencia2D.EjeRadical(c1, c2)
            RedibujarEscena()
        Else
            If c2.ContenidoEnCirculo(New Punto2D(e.Location.X, e.Location.Y)) Then
                c2 = New Circunferencia2D(c2.Centro, c2.Radio + (Sign(e.Delta) * 5))
                r = Circunferencia2D.EjeRadical(c1, c2)
                RedibujarEscena()
            End If
        End If
    End Sub
End Class
