Public MustInherit Class ObjetoGeometrico
    Inherits ObjetoBase

    Public Shadows Event Modificado(ByRef Sender As ObjetoGeometrico)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString()
    End Function
End Class