Imports Motor3D.Espacio2D
Imports dx_lib32

Namespace Motor3D.Escena.Renders.RenderDx_lib32
    Public Class RenderDx_lib32
        Inherits Render

        Dim g As dx_GFX_Class
        Private mRender As Boolean

        Private mHandle As IntPtr

        Private mPoligonos As List(Of PoligonoDx_lib32)

        Public Shadows Event Actualizado(ByRef sender As RenderDx_lib32)

        Public ReadOnly Property Graphics As dx_GFX_Class
            Get
                Return g
            End Get
        End Property

        Public ReadOnly Property Handle As IntPtr
            Get
                Return mHandle
            End Get
        End Property

        Public ReadOnly Property Poligonos As PoligonoDx_lib32()
            Get
                Return mPoligonos.ToArray
            End Get
        End Property

        Public ReadOnly Property Rendering As Boolean
            Get
                Return mRender
            End Get
        End Property

        Public ReadOnly Property FPS As Integer
            Get
                Return g.FPS
            End Get
        End Property

        Public Sub New(ByRef Motor As Motor3D, ByVal Handle As IntPtr, ByVal Ancho As Integer, ByVal Alto As Integer)
            MyBase.New(Motor)
            g = New dx_GFX_Class

            mPoligonos = New List(Of PoligonoDx_lib32)

            mHandle = Handle
            mAncho = Ancho
            mAlto = Alto

            g.Init(mHandle, Ancho, Alto, 32, True, True, True, 60)
            g.DEVICE_SetDrawCenter(Ancho / 1, Alto / 1)
            mRender = False
        End Sub

        Public Overrides Sub Redimensionar(ByVal Ancho As Integer, ByVal Alto As Integer)
            MyBase.Redimensionar(Ancho, Alto)
            g.DEVICE_SetDisplayMode(Ancho, Alto, 32, True, True, True, 60)
            g.DEVICE_SetDrawCenter(Ancho / 1, Alto / 1)
        End Sub

        Protected Overrides Sub Actualizar(ByRef ZBuffer As ZBuffer)
            mPoligonos.Clear()

            If Not ZBuffer.Vacio Then
                For Each Poligono As Poligono2D In ZBuffer.Represenatciones
                    mPoligonos.Add(New PoligonoDx_lib32(Poligono))
                    mPoligonos(mPoligonos.Count - 1).Redibujar(g)
                Next
            End If

            g.Frame()

            RaiseEvent Actualizado(Me)
        End Sub

        Public Overrides Sub Iniciar()
            MyBase.Iniciar()
            mRender = True
            AddHandler mMotor.Actualizado, AddressOf Actualizar
            RenderLoop()
        End Sub

        Private Sub RenderLoop()
            Do
                If Not mPoligonos.Count = 0 Then
                    For Each Poligono As PoligonoDx_lib32 In mPoligonos
                        Poligono.Redibujar(g)
                    Next
                End If

                g.Frame()
            Loop While mRender
        End Sub

        Public Overrides Sub Finalizar()
            MyBase.Finalizar()
            mRender = False
            RemoveHandler mMotor.Actualizado, AddressOf Actualizar
            g.Terminate()
        End Sub
    End Class
End Namespace

