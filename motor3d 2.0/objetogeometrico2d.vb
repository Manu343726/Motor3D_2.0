Namespace Espacio2D
    Public MustInherit Class ObjetoGeometrico2D
        Inherits Motor3D.ObjetoGeometrico

        Public Shadows Event Modificado(ByRef Sender As ObjetoGeometrico2D)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function
    End Class
End Namespace