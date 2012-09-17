Namespace Escena.Shading
    Public MustInherit Class ObjetoShading
        Inherits ObjetoEscena

        Public Shadows Event Modificado(ByRef Sender As ObjetoEscena)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace

