Namespace Algebra
    Public MustInherit Class ObjetoAlgebraico
        Inherits ObjetoBase

        Public Shadows Event Modificado(ByRef Sender As ObjetoAlgebraico)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace