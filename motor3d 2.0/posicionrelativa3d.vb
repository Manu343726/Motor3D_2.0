Namespace Espacio3D
    Public Enum TipoPosicionRelativa3D
        Secante
        Coincidente
        Paralelo
        Cruce
    End Enum

    Public Class PosicionRelativa3D
        Inherits ObjetoGeometrico3D

        Private mTipo As TipoPosicionRelativa3D
        Private mInterseccion As Punto3D

        Public ReadOnly Property Tipo() As TipoPosicionRelativa3D
            Get
                Return mTipo
            End Get
        End Property

        Public ReadOnly Property Interseccion() As Punto3D
            Get
                If mTipo = TipoPosicionRelativa3D.Secante Then
                    Return mInterseccion
                Else
                    Throw New ExcepcionGeometrica3D("POSICIONRELATIVA3D (INTERSECCION_GET): La posición no es de tipo secante. No se puede obtener la intersección." & vbNewLine _
                                                    & "Tipo=" & mTipo.ToString)
                End If
            End Get
        End Property

        Public Sub New(ByVal ValInterseccion As Punto3D)
            mTipo = TipoPosicionRelativa3D.Secante
            mInterseccion = ValInterseccion
        End Sub

        Public Sub New(ByVal ValTipoPosicion As TipoPosicionRelativa3D)
            mTipo = ValTipoPosicion
        End Sub

        Public Overrides Function ToString() As String
            If mTipo = TipoPosicionRelativa3D.Secante Then
                Return "{" & mTipo.ToString & ",Intersección=" & mInterseccion.ToString & "}"
            Else
                Return "{" & mTipo.ToString & "}"
            End If
        End Function
    End Class
End Namespace
