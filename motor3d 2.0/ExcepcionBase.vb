Public Class ExcepcionBase
    Inherits Exception

    Public Sub New(ByVal Mensaje As String)
        MyBase.New(Mensaje)
    End Sub
End Class
