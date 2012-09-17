
Namespace Espacio2D
    Public Class DatosColision2D
        Private mPenetracion As Double
        Private mRecta As Recta2D
        Private mInterseccion As AABB2D

        Public ReadOnly Property Penetracion As Double
            Get
                Return mPenetracion
            End Get
        End Property

        Public ReadOnly Property RectaColision As Recta2D
            Get
                Return mRecta
            End Get
        End Property

        Public ReadOnly Property Direccion As Vector2D
            Get
                Return mRecta.VectorDirector
            End Get
        End Property

        Public ReadOnly Property PuntoImpacto As Punto2D
            Get
                Return mRecta.PuntoDiretor
            End Get
        End Property

        Public ReadOnly Property Interseccion As AABB2D
            Get
                Return mInterseccion
            End Get
        End Property

        Public Sub New(ByRef PuntoImpacto As Punto2D, ByRef DireccionImpacto As Vector2D, ByRef Interseccion As AABB2D)
            mRecta = New Recta2D(PuntoImpacto, DireccionImpacto)
            mPenetracion = Math.Sqrt((Interseccion.LongitudX * Interseccion.LongitudX) + (Interseccion.LongitudY * Interseccion.LongitudY))
            mInterseccion = Interseccion
        End Sub

        Public Shared Operator +(ByVal D1 As DatosColision2D, ByVal D2 As DatosColision2D) As DatosColision2D
            Return New DatosColision2D(D1.PuntoImpacto + D2.PuntoImpacto, (D1.Direccion * D1.Penetracion) + (D2.Direccion * D2.Penetracion), D1.Interseccion + D2.Interseccion)
        End Operator

        Public Shared Operator -(ByVal D1 As DatosColision2D, ByVal D2 As DatosColision2D) As DatosColision2D
            Return New DatosColision2D(D1.PuntoImpacto - D2.PuntoImpacto, (D1.Direccion * D1.Penetracion) - (D2.Direccion * D2.Penetracion), D1.Interseccion - D2.Interseccion)
        End Operator

        Public Shared Operator *(ByVal Datos As DatosColision2D, ByVal Valor As Double) As DatosColision2D
            Return New DatosColision2D(Datos.PuntoImpacto * Valor, Datos.Direccion * Datos.Penetracion, New AABB2D(Datos.Interseccion.Posicion * Valor, Datos.Interseccion.Dimensiones))
        End Operator

        Public Shared Operator *(ByVal Valor As Double, ByVal Datos As DatosColision2D) As DatosColision2D
            Return New DatosColision2D(Datos.PuntoImpacto * Valor, Datos.Direccion * Datos.Penetracion, New AABB2D(Datos.Interseccion.Posicion * Valor, Datos.Interseccion.Dimensiones))
        End Operator

        Public Shared Operator /(ByVal Datos As DatosColision2D, ByVal Valor As Double) As DatosColision2D
            Return New DatosColision2D(Datos.PuntoImpacto / Valor, Datos.Direccion * Datos.Penetracion, New AABB2D(Datos.Interseccion.Posicion / Valor, Datos.Interseccion.Dimensiones))
        End Operator
    End Class
End Namespace
