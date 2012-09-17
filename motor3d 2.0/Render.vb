Namespace Escena.Renders
    Public MustInherit Class Render
        Inherits ObjetoEscena

        Protected mMotor As Motor3D
        Protected mAncho As Integer
        Protected mAlto As Integer

        Public Event Actualizado(ByRef Sender As Render)

        Public ReadOnly Property Motor As Motor3D
            Get
                Return mMotor
            End Get
        End Property

        Public ReadOnly Property Ancho As Integer
            Get
                Return mAncho
            End Get
        End Property

        Public ReadOnly Property Alto As Integer
            Get
                Return mAlto
            End Get
        End Property

        Public Sub New(ByRef Motor As Motor3D)
            MyBase.New()
            mMotor = Motor
            mAncho = 0
            mAlto = 0
        End Sub

        Public Overridable Sub Redimensionar(ByVal Ancho As Integer, ByVal Alto As Integer)
            mAncho = Ancho
            mAlto = Alto
        End Sub

        Public Overridable Sub Iniciar()
            AddHandler mMotor.Actualizado, AddressOf Actualizar
        End Sub

        Public Overridable Sub Finalizar()
            RemoveHandler mMotor.Actualizado, AddressOf Actualizar
        End Sub

        Protected MustOverride Sub Actualizar(ByRef ZBuffer As ZBuffer)
    End Class
End Namespace

