Namespace Discreta
    Public Class Grafo
        Private mAristas() As Arista
        Private mVertices() As Vertice
        Private mNumeroVertices As Long

        Public ReadOnly Property Aristas As Arista()
            Get
                Return mAristas.ToArray
            End Get
        End Property

        Public ReadOnly Property Vertices As Vertice()
            Get
                Return mVertices
            End Get
        End Property

        Public ReadOnly Property NumeroVertices As Long
            Get
                Return mNumeroVertices
            End Get
        End Property

        Public ReadOnly Property NumeroAristas As Long
            Get
                Return mAristas.GetUpperBound(0) + 1
            End Get
        End Property

        Public Sub New(ByVal NumeroVertices As Long, ByVal ParamArray Aristas() As Arista)
            If NumeroVertices > 0 Then
                mNumeroVertices = NumeroVertices
                mAristas = Aristas
                ReDim mVertices(mNumeroVertices - 1)

                For i As Integer = 0 To mNumeroVertices - 1
                    mVertices(i) = New Vertice(i)
                    For Each Arista As Arista In mAristas
                        If mVertices(i).AristaValida(Arista) Then
                            mVertices(i).AñadirArista(Arista)
                        End If
                    Next
                Next
            Else
                Throw New ExcepcionDiscreta("GRAFO (NEW): Un grafo debe tener a menos un vertice" & vbNewLine & _
                                            "NumeroVertices=" & NumeroVertices.ToString)
            End If
        End Sub

        Public Sub New(ByVal Vertices() As Vertice, ByVal Aristas() As Arista, Optional ByVal EstaEstructurado As Boolean = False)
            mVertices = Vertices
            mAristas = Aristas

            If EstaEstructurado Then
                For Each Vertice As Vertice In mVertices
                    For Each Arista As Arista In mAristas
                        If Vertice.AristaValida(Arista) AndAlso Not Vertice.Aristas.Contains(Arista) Then
                            Vertice.AñadirArista(Arista)
                        End If
                    Next
                Next
            End If
        End Sub

        Public Shared Function EsEuleriano(ByVal Grafo As Grafo) As Boolean
            For Each Vertice As Vertice In Grafo.Vertices
                If Not Vertice.GradoPar Then
                    Return False
                End If
            Next

            Return True
        End Function

        Public Shared Function SubGrafo(ByVal Grafo As Grafo, ByVal ParamArray Vertices() As Vertice) As Grafo
            Dim Aristas As New List(Of Arista)

            For Each Arista As Arista In Grafo.Aristas
                If Not Aristas.Contains(Arista) Then
                    For Each Vertice As Vertice In Grafo.Vertices
                        If Vertice.AristaValida(Arista) Then
                            Aristas.Add(Arista)
                            Exit For
                        End If
                    Next
                End If
            Next

            Return New Grafo(Vertices, Aristas.ToArray)
        End Function

        Public Shared Function SubGrafo(ByVal Grafo As Grafo, ByVal Recorrido As Recorrido) As Grafo
            Return SubGrafo(Grafo, Recorrido.Vertices)
        End Function

        Public Shared Function SubGrafo(ByVal G1 As Grafo, ByVal G2 As Grafo) As Grafo
            Return SubGrafo(G1, G2.Vertices)
        End Function

        Public Shared Function RecorridoEuleriano(ByVal Grafo As Grafo) As Recorrido
            Dim Retorno As List(Of Vertice)


        End Function

        Public Shared Operator =(ByVal G1 As Grafo, ByVal G2 As Grafo) As Boolean
            Return G1.Equals(G2)
        End Operator

        Public Shared Operator <>(ByVal G1 As Grafo, ByVal G2 As Grafo) As Boolean
            Return Not G1.Equals(G2)
        End Operator

        Public Shared Operator -(ByVal G1 As Grafo, ByVal G2 As Grafo) As Grafo
            Return SubGrafo(G1, G2)
        End Operator

        Public Shared Operator -(ByVal Grafo As Grafo, ByVal Recorrido As Recorrido) As Grafo
            Return SubGrafo(Grafo, Recorrido)
        End Operator
    End Class
End Namespace

