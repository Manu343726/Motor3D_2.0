Public MustInherit Class ObjetoBase
    Inherits Object

    Public Event Modificado(ByRef Sender As Object)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString
    End Function
End Class

