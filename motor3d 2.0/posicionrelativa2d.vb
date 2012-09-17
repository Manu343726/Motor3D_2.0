

Namespace Espacio2D
    Public Enum TipoPosicionRelativa2D
        Secante
        Coincidente
        Paralelo
    End Enum

    Public Class PosicionRelativa2D
        Inherits ObjetoGeometrico2D

        Private mTipo As TipoPosicionRelativa2D
        Private mInterseccion As Punto2D

        Public ReadOnly Property Tipo() As TipoPosicionRelativa2D
            Get
                Return mTipo
            End Get
        End Property

        Public ReadOnly Property Interseccion() As Punto2D
            Get
                If mTipo = TipoPosicionRelativa2D.Secante Then
                    Return mInterseccion
                Else
                    Throw New ExcepcionGeometrica2D("POSICIONRELATIVA2D (INTERSECCION_GET): La posición no es de tipo secante. No se puede obtener la intersección." & vbNewLine _
                                                    & "Tipo=" & mTipo.ToString)
                End If
            End Get
        End Property

        Public Sub New(ByVal ValInterseccion As Punto2D)
            mTipo = TipoPosicionRelativa2D.Secante
            mInterseccion = ValInterseccion
        End Sub

        Public Sub New(ByVal ValTipoPosicion As TipoPosicionRelativa2D)
            mTipo = ValTipoPosicion
        End Sub

        Public Overrides Function ToString() As String
            If mTipo = TipoPosicionRelativa2D.Secante Then
                Return "{" & mTipo.ToString & ",Intersección=" & mInterseccion.ToString & "}"
            Else
                Return "{" & mTipo.ToString & "}"
            End If
        End Function
    End Class
End Namespace

