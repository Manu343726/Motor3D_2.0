Namespace Discreta
    Public Class Arista
        Private mPrimerVertice As Long
        Private mSegundoVertice As Long

        Public ReadOnly Property PrimerVertice As Long
            Get
                Return mPrimerVertice
            End Get
        End Property

        Public ReadOnly Property SegundoVertice As Long
            Get
                Return mSegundoVertice
            End Get
        End Property

        Public Sub New(ByVal PrimerVertice As Long, ByVal SegundoVertice As Long)
            If PrimerVertice >= 0 Then
                If SegundoVertice > 0 Then
                    mPrimerVertice = PrimerVertice
                    mSegundoVertice = SegundoVertice
                Else
                    Throw New ExcepcionDiscreta("ARISTA (NEW): El segundo vertice no puede ser menor que cero" & vbNewLine & _
                                                "Primer vertice=" & PrimerVertice.ToString & vbNewLine & _
                                                "Segundo vertice=" & SegundoVertice.ToString)
                End If
            Else
                Throw New ExcepcionDiscreta("ARISTA (NEW): El primer vertice no puede ser menor que cero" & vbNewLine & _
                                                "Primer vertice=" & PrimerVertice.ToString & vbNewLine & _
                                                "Segundo vertice=" & SegundoVertice.ToString)
            End If
        End Sub
    End Class
End Namespace

