Public MustInherit Class ObjetoBase
    Public Event Modificado(ByRef Sender As ObjetoBase)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString()
    End Function
End Class
