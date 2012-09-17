Namespace Escena
    Public Class ElementoZBuffer
        Inherits ObjetoEscena
        Implements IComparable(Of ElementoZBuffer)

        Private mIndices() As Double
        Private mNumeroIndices As Integer
        Private mZ As Double

        Public Shadows Event Modificado(ByRef Sender As Object)

        Public ReadOnly Property NumeroIndices As Integer
            Get
                Return mNumeroIndices
            End Get
        End Property

        Public Property Indices() As Double()
            Get
                Return mIndices
            End Get
            Set(value As Double())
                If value.GetUpperBound(0) = mIndices.GetUpperBound(0) Then
                    mIndices = value
                End If
            End Set
        End Property

        Public Property Indices(ByVal Index As Integer) As Double
            Get
                If Index >= 0 AndAlso Index < mNumeroIndices Then
                    Return mIndices(Index)
                Else
                    Throw New ExcepcionEscena("ELEMENTOZBUFFER (INDICE_GET): El índice está fuera del intervalo." & vbNewLine _
                                              & "Index=" & Index.ToString & vbNewLine _
                                              & "Intervalo=(0," & mNumeroIndices - 1 & ")")
                End If
            End Get
            Set(value As Double)
                If Index >= 0 AndAlso Index < mNumeroIndices Then
                    mIndices(Index) = value
                Else
                    Throw New ExcepcionEscena("ELEMENTOZBUFFER (INDICE_SET): El índice está fuera del intervalo." & vbNewLine _
                          & "Index=" & Index.ToString & vbNewLine _
                          & "Intervalo=(0," & mNumeroIndices - 1 & ")")
                End If
            End Set
        End Property

        Public Property Z As Double
            Get
                Return mZ
            End Get
            Set(value As Double)
                mZ = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Function EsEquivalente(ByVal ParamArray Indices() As Double)
            Return mIndices.Equals(Indices)
        End Function

        Public Sub New(ByVal NumeroIndices As Integer)
            If NumeroIndices > 0 Then
                ReDim mIndices(NumeroIndices - 1)
                mNumeroIndices = NumeroIndices
            Else
                Throw New ExcepcionEscena("ELEMENTOZBUFFER (NEW): El número de índices debe ser mayor o igual que 1." & vbNewLine _
                                          & "Numero de índices=" & NumeroIndices.ToString)
            End If
        End Sub

        Public Sub New(ByVal Z As Double, ByVal ParamArray Indices() As Double)
            mIndices = Indices
            mNumeroIndices = Indices.GetUpperBound(0) + 1
            mZ = Z
        End Sub

        Public Shared Operator >(ByVal X As ElementoZBuffer, ByVal Y As ElementoZBuffer) As Boolean
            Return X.Z > Y.Z
        End Operator

        Public Shared Operator <(ByVal X As ElementoZBuffer, ByVal Y As ElementoZBuffer) As Boolean
            Return X.Z < Y.Z
        End Operator

        Public Shared Operator =(ByVal X As ElementoZBuffer, ByVal Y As ElementoZBuffer) As Boolean
            Return X.Z = Y.Z
        End Operator

        Public Shared Operator <>(ByVal X As ElementoZBuffer, ByVal Y As ElementoZBuffer) As Boolean
            Return X.Z <> Y.Z
        End Operator

        Public Function CompareTo(other As ElementoZBuffer) As Integer Implements System.IComparable(Of ElementoZBuffer).CompareTo
            Select Case other.Z
                Case Is > mZ
                    Return 1
                Case Is = mZ
                    Return 0
                Case Is < mZ
                    Return -1
            End Select
        End Function
    End Class
End Namespace

