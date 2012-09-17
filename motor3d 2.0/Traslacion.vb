Imports Motor3D.Primitivas3D

Namespace Espacio3D.Transformaciones
    Public NotInheritable Class Traslacion
        Inherits Transformacion3D

        Private mTraslacion As Vector3D

        Public ReadOnly Property Traslacion As Vector3D
            Get
                Return mTraslacion
            End Get
        End Property

        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            mTraslacion = New Vector3D(X, Y, Z)
            EstablecerMatriz()
        End Sub

        Public Sub New(ByVal Traslacion As Vector3D)
            mTraslacion = Traslacion
            EstablecerMatriz()
        End Sub

        Public Sub New(ByVal Inicio As Punto3D, ByVal Fin As Punto3D)
            mTraslacion = New Vector3D(Inicio, Fin)
            EstablecerMatriz()
        End Sub

        Private Sub EstablecerMatriz()
            mMatriz.EstablecerValoresPorFila(0, 1, 0, 0, mTraslacion.X)
            mMatriz.EstablecerValoresPorFila(1, 0, 1, 0, mTraslacion.Y)
            mMatriz.EstablecerValoresPorFila(2, 0, 0, 1, mTraslacion.Z)
            mMatriz.EstablecerValoresPorFila(3, 0, 0, 0, 1)
        End Sub

        Public Overloads Shared Function EncadenarTransformaciones(ByVal E1 As Traslacion, ByVal E2 As Traslacion) As Traslacion
            Return New Traslacion(E1.Traslacion + E2.Traslacion)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Punto As Punto3D, ByVal Traslacion As Traslacion) As Punto3D
            Return New Punto3D(Punto.X + Traslacion.Traslacion.X, Punto.Y + Traslacion.Traslacion.Y, Punto.Z + Traslacion.Traslacion.Z)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vector As Vector3D, ByVal Traslacion As Traslacion) As Vector3D
            Return New Vector3D(Vector.X + Traslacion.Traslacion.X, Vector.Y + Traslacion.Traslacion.Y, Vector.Z + Traslacion.Traslacion.Z)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vertice As Vertice, ByVal Traslacion As Traslacion) As Vertice
            Return New Vertice(New Punto3D(Vertice.CoodenadasSUR.X + Traslacion.Traslacion.X, Vertice.CoodenadasSUR.Y + Traslacion.Traslacion.Y, Vertice.CoodenadasSUR.Z + Traslacion.Traslacion.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Recta As Recta3D, ByVal Traslacion As Traslacion) As Recta3D
            Return New Recta3D(New Punto3D(Recta.PuntoInicial.X + Traslacion.Traslacion.X, Recta.PuntoInicial.Y + Traslacion.Traslacion.Y, Recta.PuntoInicial.Z + Traslacion.Traslacion.Z), Recta.VectorDirector)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Plano As Plano3D, ByVal Traslacion As Traslacion) As Plano3D
            Dim P As Punto3D = Plano.ObtenerPunto(0, 0)
            Return New Plano3D(New Punto3D(P.X + Traslacion.Traslacion.X, P.Y + Traslacion.Traslacion.Y, P.Z + Traslacion.Traslacion.Z), Plano.VectorNormal)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal AABB As AABB3D, ByVal Traslacion As Traslacion) As AABB3D
            Return New AABB3D(New Punto3D(AABB.Left + Traslacion.Traslacion.X, AABB.Top + Traslacion.Traslacion.Y, AABB.Up + Traslacion.Traslacion.Z), New Vector3D(AABB.Ancho + Traslacion.Traslacion.X, AABB.Largo + Traslacion.Traslacion.Y, AABB.Alto + Traslacion.Traslacion.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Segmento As Segmento3D, ByVal Traslacion As Traslacion) As Segmento3D
            Return New Segmento3D(New Punto3D(Segmento.ExtremoInicial.X + Traslacion.Traslacion.X, Segmento.ExtremoInicial.Y + Traslacion.Traslacion.Y, Segmento.ExtremoInicial.Z + Traslacion.Traslacion.Z), New Punto3D(Segmento.ExtremoFinal.X + Traslacion.Traslacion.X, Segmento.ExtremoFinal.Y + Traslacion.Traslacion.Y, Segmento.ExtremoFinal.Z + Traslacion.Traslacion.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Poliedro As Poliedro, ByVal Traslacion As Traslacion) As Poliedro
            Poliedro.AplicarTransformacion(Traslacion)
            Return Poliedro
        End Function

        Public Overloads Shared Operator +(ByVal E1 As Traslacion, ByVal E2 As Traslacion) As Traslacion
            Return EncadenarTransformaciones(E1, E2)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Punto As Punto3D) As Punto3D
            Return AplicarTransformacion(Punto, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Punto As Punto3D, ByVal Traslacion As Traslacion) As Punto3D
            Return AplicarTransformacion(Punto, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Vector As Vector3D) As Vector3D
            Return AplicarTransformacion(Vector, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Vector As Vector3D, ByVal Traslacion As Traslacion) As Vector3D
            Return AplicarTransformacion(Vector, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Vertice As Vertice) As Vertice
            Return AplicarTransformacion(Vertice, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Vertice As Vertice, ByVal Traslacion As Traslacion) As Vertice
            Return AplicarTransformacion(Vertice, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Recta As Recta3D) As Recta3D
            Return AplicarTransformacion(Recta, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Recta As Recta3D, ByVal Traslacion As Traslacion) As Recta3D
            Return AplicarTransformacion(Recta, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Plano As Plano3D) As Plano3D
            Return AplicarTransformacion(Plano, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Plano As Plano3D, ByVal Traslacion As Traslacion) As Plano3D
            Return AplicarTransformacion(Plano, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal AABB As AABB3D) As AABB3D
            Return AplicarTransformacion(AABB, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal AABB As AABB3D, ByVal Traslacion As Traslacion) As AABB3D
            Return AplicarTransformacion(AABB, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Segmento As Segmento3D) As Segmento3D
            Return AplicarTransformacion(Segmento, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Segmento As Segmento3D, ByVal Traslacion As Traslacion) As Segmento3D
            Return AplicarTransformacion(Segmento, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Traslacion As Traslacion, ByVal Poliedro As Poliedro) As Poliedro
            Return AplicarTransformacion(Poliedro, Traslacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Poliedro As Poliedro, ByVal Traslacion As Traslacion) As Poliedro
            Return AplicarTransformacion(Poliedro, Traslacion)
        End Operator
    End Class
End Namespace

