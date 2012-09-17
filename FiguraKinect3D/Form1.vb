Imports System.Math
Imports Motor3D.Espacio2D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Primitivas3D
Imports Motor3D.Escena
Imports Motor3D.Escena.Renders.GDI

Imports Microsoft.Kinect

Public Class Form1
    Dim Kinect As Kinect

    Dim Camara As Camara3D
    Dim Cubos(19) As Poliedro

    Dim MegaCubo As Poliedro

    Dim BMP As Bitmap
    Dim g As Graphics

    Dim Motor As Motor3D.Escena.Motor3D
    Dim Foco As New Foco3D(New Punto3D, Color.White)

    Dim p, p2 As Pen

    Dim Vector As New Vector3D(0, 1, 0)
    Dim VectorProducto As New Vector3D(0, 1, 0)
    Dim VectorTransformado As New Vector3D(0, 1, 0)

    Private Debugging As Boolean = False

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Kinect = New Kinect

            Camara = New Camara3D
            Camara.Posicion = New Punto3D(0, 0, -1000)

            Motor = New Motor3D.Escena.Motor3D()
            Motor.AñadirCamara(Camara)

            For i As Integer = 0 To 19
                Cubos(i) = Poliedro.Cubo
                Cubos(i).AplicarTransformacion(New Escalado(50, 50, 50))

                Cubos(i).EstablecerColor(0, Color.Red)
                Cubos(i).EstablecerColor(1, Color.White)
                Cubos(i).EstablecerColor(2, Color.Blue)
                Cubos(i).EstablecerColor(3, Color.Yellow)
                Cubos(i).EstablecerColor(4, Color.Green)
                Cubos(i).EstablecerColor(5, Color.Orange)

                Motor.AñadirPoliedro(Cubos(i))
            Next

            MegaCubo = Poliedro.Cubo
            MegaCubo.AplicarTransformacion(New Escalado(200, 200, 200))

            MegaCubo.EstablecerColor(0, Color.Red)
            MegaCubo.EstablecerColor(1, Color.White)
            MegaCubo.EstablecerColor(2, Color.Blue)
            MegaCubo.EstablecerColor(3, Color.Yellow)
            MegaCubo.EstablecerColor(4, Color.Green)
            MegaCubo.EstablecerColor(5, Color.Orange)

            'Motor.AñadirPoliedro(MegaCubo)
            Motor.AñadirFoco(Foco)
            Motor.ShadingActivado = True

            p = New Pen(Brushes.Red, 5)
            p.EndCap = Drawing2D.LineCap.ArrowAnchor
            p2 = New Pen(Brushes.Blue, 5)
            p2.EndCap = Drawing2D.LineCap.ArrowAnchor

            AddHandler Kinect.EsqueletoActualizado, AddressOf EsqueletoActualizado
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    Private Sub EsqueletoActualizado(ByRef e As Esqueleto)
        Foco.Coordenadas = e.PalmaIzquierda

        Motor.IniciarEscena()

        For i As Integer = 0 To 19
            Cubos(i).AplicarTransformacion(New Traslacion(Cubos(i).CentroSUR, e.Articulaciones(i)))
        Next

        VectorProducto = (Vector & e.BrazoDerecho.Segmento.Recta.VectorDirector).VectorUnitario

        'Vector *= Transformacion3D.Rotacion(Vector, Kinect.Esqueleto.Espalda.Segmento.Recta.VectorDirector)
        Vector = Vector.VectorUnitario

        VectorTransformado = Vector * New Rotacion(Vector, e.BrazoDerecho.Segmento.Recta.VectorDirector.VectorUnitario)

        Motor.FinalizarEscena()

        Redibujar()
    End Sub

    Private Sub Form1_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                Kinect.Angulo += 2
            Case Keys.Down
                Kinect.Angulo -= 2
            Case Keys.C
                Kinect.Camara = Not Kinect.Camara
            Case Keys.D
                Debugging = Not Debugging
            Case Keys.Escape
                Kinect.Cerrar()
                End
        End Select
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Kinect.Cerrar()
    End Sub


    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Pic.Width > 0 AndAlso Pic.Height > 0 AndAlso Not Kinect Is Nothing Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Graphics.FromImage(BMP)
            g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)

            Pic.Image = BMP
            Pic.Refresh()
        End If
    End Sub

    Private Sub Redibujar()
        Dim f As New Font("Times New Roman", 30)

        If Not g Is Nothing Then
            g.Clear(Color.Black)

            If Not Motor.ZBuffer.Vacio Then
                For Each Poligono As Poligono2D In Motor.ZBuffer.Represenatciones
                    g.FillPolygon(New SolidBrush(Poligono.Color), Poligono.VerticesToPoint(True))
                Next
            End If

            g.DrawLine(p, 0, 0, CInt(Vector.X) * 100, CInt(-Vector.Y) * 100)
            g.DrawLine(p2, 0, 0, CInt(VectorProducto.X) * 100, CInt(-VectorProducto.Y) * 100)
            g.DrawString("Vector=" & Vector.ToString & vbNewLine & _
                         "Brazo=" & Kinect.Esqueleto.BrazoDerecho.Segmento.Recta.VectorDirector.ToString & vbNewLine & _
                         "Producto=" & VectorProducto.ToString & vbNewLine & _
                         "Transformado=" & VectorTransformado.ToString, f, Brushes.White, 15, 15)

            Pic.Refresh()
        End If
    End Sub
End Class
