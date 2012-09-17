Namespace Escena
    Public MustInherit Class ObjetoEscena
        Inherits Global.Motor3D.ObjetoBase

        Public Shadows Event Modificado(ByRef Sender As ObjetoEscena)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace

