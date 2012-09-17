Imports Motor3D.Primitivas3D

Namespace Espacio3D.Transformaciones
    Public Class Escalado
        Inherits Transformacion3D

        Private mEscalado As Vector3D

        Public ReadOnly Property Escalado As Vector3D
            Get
                Return mEscalado
            End Get
        End Property

        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            mEscalado = New Vector3D(X, Y, Z)
            EstablecerMatriz()
        End Sub

        Public Sub New(ByVal Escalado As Double)
            mEscalado = New Vector3D(Escalado, Escalado, Escalado)
            EstablecerMatriz()
        End Sub

        Public Sub New(ByVal Escalado As Vector3D)
            mEscalado = Escalado
            EstablecerMatriz()
        End Sub

        Private Sub EstablecerMatriz()
            mMatriz.EstablecerValoresPorFila(0, mEscalado.X, 0, 0, 0)
            mMatriz.EstablecerValoresPorFila(1, 0, mEscalado.Y, 0, 0)
            mMatriz.EstablecerValoresPorFila(2, 0, 0, mEscalado.Z, 0)
            mMatriz.EstablecerValoresPorFila(3, 0, 0, 0, 1)
        End Sub

        Public Overloads Shared Function EncadenarTransformaciones(ByVal E1 As Escalado, ByVal E2 As Escalado) As Escalado
            Return New Escalado(E1.Escalado + E2.Escalado)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Punto As Punto3D, ByVal Escalado As Escalado) As Punto3D
            Return New Punto3D(Punto.X * Escalado.Escalado.X, Punto.Y * Escalado.Escalado.Y, Punto.Z * Escalado.Escalado.Z)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vector As Vector3D, ByVal Escalado As Escalado) As Vector3D
            Return New Vector3D(Vector.X * Escalado.Escalado.X, Vector.Y * Escalado.Escalado.Y, Vector.Z * Escalado.Escalado.Z)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vertice As Vertice, ByVal Escalado As Escalado) As Vertice
            Return New Vertice(New Punto3D(Vertice.CoodenadasSUR.X * Escalado.Escalado.X, Vertice.CoodenadasSUR.Y * Escalado.Escalado.Y, Vertice.CoodenadasSUR.Z * Escalado.Escalado.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Recta As Recta3D, ByVal Escalado As Escalado) As Recta3D
            Return New Recta3D(New Punto3D(Recta.PuntoInicial.X * Escalado.Escalado.X, Recta.PuntoInicial.Y * Escalado.Escalado.Y, Recta.PuntoInicial.Z * Escalado.Escalado.Z), Recta.VectorDirector)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Plano As Plano3D, ByVal Escalado As Escalado) As Plano3D
            Dim P As Punto3D = Plano.ObtenerPunto(0, 0)
            Return New Plano3D(New Punto3D(P.X * Escalado.Escalado.X, P.Y * Escalado.Escalado.Y, P.Z * Escalado.Escalado.Z), Plano.VectorNormal)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal AABB As AABB3D, ByVal Escalado As Escalado) As AABB3D
            Return New AABB3D(New Punto3D(AABB.Left * Escalado.Escalado.X, AABB.Top * Escalado.Escalado.Y, AABB.Up * Escalado.Escalado.Z), New Vector3D(AABB.Ancho * Escalado.Escalado.X, AABB.Largo * Escalado.Escalado.Y, AABB.Alto * Escalado.Escalado.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Segmento As Segmento3D, ByVal Escalado As Escalado) As Segmento3D
            Return New Segmento3D(New Punto3D(Segmento.ExtremoInicial.X * Escalado.Escalado.X, Segmento.ExtremoInicial.Y * Escalado.Escalado.Y, Segmento.ExtremoInicial.Z * Escalado.Escalado.Z), New Punto3D(Segmento.ExtremoFinal.X * Escalado.Escalado.X, Segmento.ExtremoFinal.Y * Escalado.Escalado.Y, Segmento.ExtremoFinal.Z * Escalado.Escalado.Z))
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Poliedro As Poliedro, ByVal Escalado As Escalado) As Poliedro
            Poliedro.AplicarTransformacion(Escalado)
            Return Poliedro
        End Function

        Public Overloads Shared Operator +(ByVal E1 As Escalado, ByVal E2 As Escalado) As Escalado
            Return EncadenarTransformaciones(E1, E2)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Punto As Punto3D) As Punto3D
            Return AplicarTransformacion(Punto, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Punto As Punto3D, ByVal Escalado As Escalado) As Punto3D
            Return AplicarTransformacion(Punto, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Vector As Vector3D) As Vector3D
            Return AplicarTransformacion(Vector, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Vector As Vector3D, ByVal Escalado As Escalado) As Vector3D
            Return AplicarTransformacion(Vector, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Vertice As Vertice) As Vertice
            Return AplicarTransformacion(Vertice, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Vertice As Vertice, ByVal Escalado As Escalado) As Vertice
            Return AplicarTransformacion(Vertice, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Recta As Recta3D) As Recta3D
            Return AplicarTransformacion(Recta, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Recta As Recta3D, ByVal Escalado As Escalado) As Recta3D
            Return AplicarTransformacion(Recta, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Plano As Plano3D) As Plano3D
            Return AplicarTransformacion(Plano, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Plano As Plano3D, ByVal Escalado As Escalado) As Plano3D
            Return AplicarTransformacion(Plano, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal AABB As AABB3D) As AABB3D
            Return AplicarTransformacion(AABB, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal AABB As AABB3D, ByVal Escalado As Escalado) As AABB3D
            Return AplicarTransformacion(AABB, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Segmento As Segmento3D) As Segmento3D
            Return AplicarTransformacion(Segmento, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Segmento As Segmento3D, ByVal Escalado As Escalado) As Segmento3D
            Return AplicarTransformacion(Segmento, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Escalado As Escalado, ByVal Poliedro As Poliedro) As Poliedro
            Return AplicarTransformacion(Poliedro, Escalado)
        End Operator

        Public Overloads Shared Operator *(ByVal Poliedro As Poliedro, ByVal Escalado As Escalado) As Poliedro
            Return AplicarTransformacion(Poliedro, Escalado)
        End Operator
    End Class
End Namespace

