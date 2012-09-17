Namespace Discreta
    Public Class Vertice
        Private mAristas As List(Of Arista)
        Private mIndice As Long

        Public ReadOnly Property Aristas As Arista()
            Get
                Return mAristas.ToArray
            End Get
        End Property

        Public ReadOnly Property Indice As Long
            Get
                Return mIndice
            End Get
        End Property

        Public ReadOnly Property Grado As Long
            Get
                Return mAristas.Count
            End Get
        End Property

        Public ReadOnly Property GradoPar As Boolean
            Get
                Return mAristas.Count Mod 2 = 0
            End Get
        End Property

        Public ReadOnly Property AristaValida(ByVal Arista As Arista) As Boolean
            Get
                Return Arista.PrimerVertice = mIndice OrElse Arista.SegundoVertice = mIndice
            End Get
        End Property

        Public Sub New(ByVal Indice As Long)
            If Indice >= 0 Then
                mIndice = Indice
            Else
                Throw New ExcepcionDiscreta("VERTICE (NEW): El indice no puede ser menor que cero" & vbNewLine & _
                                            "Indice=" & Indice.ToString)
            End If
        End Sub

        Public Sub EstablecerAristas(ByVal ParamArray Aristas() As Arista)
            mAristas.Clear()

            For Each Arista As Arista In Aristas
                If AristaValida(Arista) Then mAristas.Add(Arista)
            Next
        End Sub

        Public Sub AñadirArista(ByVal Arista As Arista)
            If AristaValida(Arista) Then
                mAristas.Add(Arista)
            End If
        End Sub
    End Class
End Namespace

