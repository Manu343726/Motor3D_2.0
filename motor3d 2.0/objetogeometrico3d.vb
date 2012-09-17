Namespace Espacio3D
    Public MustInherit Class ObjetoGeometrico3D
        Inherits ObjetoGeometrico

        Public Shadows Event Modificado(ByRef Sender As ObjetoGeometrico3D)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace