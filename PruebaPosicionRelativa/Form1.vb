Imports Motor3D.Espacio2D

Public Class Form1
    Dim g As Graphics
    Dim BMP As Bitmap
    Dim Recta As Recta2D
    Dim Puntos As New List(Of Punto2D)
    Dim Puntero As Punto2D
    Dim AABB As AABB2D

    Dim Quadtree As Quadtree
    Dim Sectores() As SectorQuadtree
    Dim Tamaño As Integer = 6

    Dim DibujarRecta As Boolean = False

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.Up
                    AABB = New AABB2D(AABB.Posicion.X, AABB.Posicion.Y - 10, AABB.LongitudX, AABB.LongitudY)
                Case Keys.Down
                    AABB = New AABB2D(AABB.Posicion.X, AABB.Posicion.Y + 10, AABB.LongitudX, AABB.LongitudY)
                Case Keys.Right
                    AABB = New AABB2D(AABB.Posicion.X + 10, AABB.Posicion.Y, AABB.LongitudX, AABB.LongitudY)
                Case Keys.Left
                    AABB = New AABB2D(AABB.Posicion.X - 10, AABB.Posicion.Y, AABB.LongitudX, AABB.LongitudY)
            End Select
        Else
            Select Case e.KeyCode
                Case Keys.Up
                    Recta = Recta * Transformacion2D.Traslacion(0, 10)
                Case Keys.Down
                    Recta = Recta * Transformacion2D.Traslacion(0, -10)
                Case Keys.Right
                    Recta = Recta * Transformacion2D.Traslacion(-10, 0)
                Case Keys.Left
                    Recta = Recta * Transformacion2D.Traslacion(10, 0)
                Case Keys.Z
                    Recta = Recta * Transformacion2D.Rotacion(-0.1)
                Case Keys.X
                    Recta = Recta * Transformacion2D.Rotacion(0.1)
                Case Keys.D
                    DibujarRecta = Not DibujarRecta
                Case Keys.Escape
                    End
            End Select

            Try
                Sectores = Quadtree.Sectores(Recta)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Excepcion no controlada", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        Redibujar()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BMP = New Bitmap(Pic.Width, Pic.Height)
        Pic.Image = BMP
        g = Graphics.FromImage(BMP)
        Recta = New Recta2D(New Punto2D(0, Pic.Height / 2), New Punto2D(Pic.Width, Pic.Height / 2))
        Puntero = New Punto2D
        AABB = New AABB2D(0, 0, 100, 100)
        Quadtree = New Quadtree(Tamaño, New AABB2D(0, 0, Pic.Width, Pic.Height))
        Puntos.Clear()
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        BMP = New Bitmap(Pic.Width, Pic.Height)
        g = Graphics.FromImage(BMP)
        Pic.Image = BMP
        Quadtree = New Quadtree(Tamaño, New AABB2D(0, 0, Pic.Width, Pic.Height))
    End Sub

    Private Sub Pic_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseDown
        If Not DibujarRecta Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Left
                    Puntos.Add(Puntero)
            End Select
        End If
    End Sub

    Private Sub Pic_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Pic.MouseMove
        Puntero = New Punto2D(e.X, e.Y)

        If DibujarRecta Then
            'Recta = New Recta2D(New Punto2D, Puntero)
            Recta = New Recta2D(New Punto2D(Pic.Width - Puntero.X, Pic.Height - Puntero.X), Puntero)
            Sectores = Quadtree.Sectores(Recta)
        End If

        Redibujar()
    End Sub

    Private Sub Redibujar()
        Dim SectorActual As SectorQuadtree
        g.Clear(Color.Black)

        Try
            If Not DibujarRecta Then
                g.FillEllipse(Brushes.Black, Puntero.ToPoint.X - 5, Puntero.ToPoint.Y - 5, 10, 10)

                If Recta.SignoPosicionRelativa(Puntero) > 0 Then
                    g.DrawEllipse(Pens.Blue, Puntero.ToPoint.X - 5, Puntero.ToPoint.Y - 5, 10, 10)
                Else
                    g.DrawEllipse(Pens.Orange, Puntero.ToPoint.X - 5, Puntero.ToPoint.Y - 5, 10, 10)
                End If
            End If

            For Each Punto As Punto2D In Puntos
                SectorActual = Quadtree.Sector(Punto)
                Do
                    g.DrawRectangle(Pens.White, New Rectangle(SectorActual.Espacio.Posicion.ToPoint, SectorActual.Espacio.Dimensiones.ToPoint))
                    SectorActual = SectorActual.Padre
                Loop While Not SectorActual Is Nothing

                g.FillEllipse(Brushes.Black, Punto.ToPoint.X - 5, Punto.ToPoint.Y - 5, 10, 10)

                If Recta.SignoPosicionRelativa(Punto) > 0 Then
                    g.DrawEllipse(Pens.Green, Punto.ToPoint.X - 5, Punto.ToPoint.Y - 5, 10, 10)
                Else
                    g.DrawEllipse(Pens.Red, Punto.ToPoint.X - 5, Punto.ToPoint.Y - 5, 10, 10)
                End If
            Next

            'SectorActual = Quadtree.Sector(AABB)
            'Do
            '    g.DrawRectangle(Pens.DeepPink, New Rectangle(SectorActual.Espacio.Posicion.ToPoint, SectorActual.Espacio.Dimensiones.ToPoint))
            '    SectorActual = SectorActual.Padre
            'Loop While Not SectorActual Is Nothing

            If AABB.Pertenece(Recta) Then
                g.DrawRectangle(Pens.Green, New Rectangle(AABB.Posicion.ToPoint, AABB.Dimensiones.ToPoint))
            Else
                g.DrawRectangle(Pens.Red, New Rectangle(AABB.Posicion.ToPoint, AABB.Dimensiones.ToPoint))
            End If

            'If Not Sectores Is Nothing Then
            '    For Each Sector As SectorQuadtree In Sectores
            '        g.DrawRectangle(Pens.DarkGray, New Rectangle(Sector.Espacio.Posicion.ToPoint, Sector.Espacio.Dimensiones.ToPoint))
            '    Next
            'End If

            g.DrawLine(Pens.Purple, Recta.ObtenerPunto(-100).ToPoint, Recta.ObtenerPunto(100).ToPoint)
        Catch ex As Exception
            MessageBox.Show(ex.InnerException.ToString & ": " & ex.Message, "Excepcion no controlada", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try



        Pic.Refresh()
    End Sub

End Class
