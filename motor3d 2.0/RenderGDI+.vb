Imports Motor3D.Espacio2D
Imports System.Drawing
Imports System.Windows.Forms

Namespace Escena.Renders.GDI
    Public Class RenderGDI
        Inherits Render

        Protected g As Graphics
        Protected BMP As Bitmap
        Protected b As SolidBrush
        Protected p As Pen

        Protected mHandle As IntPtr
        Protected mCanvas As PictureBox

        Protected mAutoRedimensionar As Boolean

        Protected mRendering As Boolean

        Public Shadows Event Actualizado(ByRef sender As RenderGDI)

        Public ReadOnly Property Bitmap As Bitmap
            Get
                Return BMP
            End Get
        End Property

        Public ReadOnly Property CanvasHandle As IntPtr
            Get
                Return mHandle
            End Get
        End Property

        Public ReadOnly Property Canvas As Control
            Get
                Return mCanvas
            End Get
        End Property

        Public Property AutoRedimensionar As Boolean
            Get
                Return mAutoRedimensionar
            End Get
            Set(value As Boolean)
                If value <> mAutoRedimensionar Then
                    mAutoRedimensionar = value

                    If value Then
                        AddHandler mCanvas.Resize, AddressOf ControladorRedimension
                    Else
                        RemoveHandler mCanvas.Resize, AddressOf ControladorRedimension
                    End If
                End If
            End Set
        End Property

        Public ReadOnly Property Rendering As Boolean
            Get
                Return mRendering
            End Get
        End Property

        Public Sub New(ByRef Motor As Motor3D, ByRef Handle As IntPtr, Optional ByVal AutoRedimensionar As Boolean = False)
            MyBase.New(Motor)

            mHandle = Handle
            mCanvas = Control.FromHandle(mHandle)
            mAncho = mCanvas.Width
            mAlto = mCanvas.Height
            mAutoRedimensionar = AutoRedimensionar
            mRendering = False
            p = New Pen(Brushes.White, 1)
            b = New SolidBrush(Color.White)
        End Sub

        Protected Overrides Sub Actualizar(ByRef ZBuffer As ZBuffer)
            If Not ZBuffer.Vacio Then
                g.Clear(Color.Black)

                For Each Poligono As Poligono2D In ZBuffer.Represenatciones
                    b.Color = Poligono.Color

                    g.FillPolygon(b, Poligono.VerticesToPoint)
                Next

                mCanvas.Refresh()
                RaiseEvent Actualizado(Me)
            End If
        End Sub

        Private Sub ControladorRedimension(ByVal sender As Object, ByVal e As EventArgs)
            Redimensionar(mCanvas.Width, mCanvas.Height)
        End Sub

        Public Overrides Sub Redimensionar(Ancho As Integer, Alto As Integer)
            MyBase.Redimensionar(Ancho, Alto)
            If mRendering Then ResetGraficos()
        End Sub

        Private Sub ResetGraficos()
            BMP = New Bitmap(mAncho, mAlto)
            g = Graphics.FromImage(BMP)
            g.TranslateTransform(mAncho / 2, mAlto / 2)
            mCanvas.Image = BMP
        End Sub

        Public Overrides Sub Iniciar()
            mRendering = True
            ResetGraficos()
            MyBase.Iniciar()
        End Sub

        Public Overrides Sub Finalizar()
            MyBase.Finalizar()
            BMP = Nothing
            g = Nothing
            mRendering = False
        End Sub
    End Class
End Namespace

