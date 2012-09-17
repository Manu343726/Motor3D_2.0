Namespace Discreta
    Public Class Recorrido
        Private mVertices() As Vertice
        Private mGrafo As Grafo

        Public ReadOnly Property Vertices As Vertice()
            Get
                Return mVertices
            End Get
        End Property

        Public ReadOnly Property Grafo As Grafo
            Get
                Return mGrafo
            End Get
        End Property

        Public ReadOnly Property Longitud As Long
            Get
                Return mVertices.GetUpperBound(0) + 1
            End Get
        End Property

        Public Sub New(ByRef Grafo As Grafo, ByVal ParamArray Recorrido() As Vertice)
            mGrafo = Grafo
            mVertices = Recorrido
        End Sub

        Public Shared Function Concatenar(ByVal R1 As Recorrido, ByVal R2 As Recorrido) As Recorrido
            Dim Retorno(R1.Longitud + R2.Longitud - 2) As Vertice

            If R1.Grafo = R2.Grafo Then
                For i As Integer = 0 To Retorno.GetUpperBound(0)
                    If i <= R1.Vertices.GetUpperBound(0) Then
                        Retorno(i) = R1.Vertices(i)
                    Else
                        Retorno(i) = R2.Vertices(i - R1.Vertices.GetUpperBound(0))
                    End If
                Next

                Return New Recorrido(R1.Grafo, Retorno)
            Else
                Throw New ExcepcionDiscreta("RECORRIDO(CONCATENAR): No se pueden concatenar dos recorridos de grafos diferentes")
            End If
        End Function

        Public Shared Operator +(ByVal R1 As Recorrido, ByVal R2 As Recorrido) As Recorrido
            Return Concatenar(R1, R2)
        End Operator
    End Class
End Namespace

