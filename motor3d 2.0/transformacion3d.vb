Imports Motor3D.Algebra
Imports Motor3D.Primitivas3D
Imports System.Math

Namespace Espacio3D.Transformaciones
    Public Class Transformacion3D
        Inherits ObjetoGeometrico3D

        Protected mMatriz As Matriz

        Public ReadOnly Property Matriz() As Matriz
            Get
                Return mMatriz
            End Get
        End Property

        Public Sub New()
            mMatriz = Algebra.Matriz.MatrizUnitaria(4)
        End Sub

        Public Sub New(ByVal Matriz As Matriz)
            If Matriz.EsCuadrada AndAlso Matriz.Filas = 4 Then
                mMatriz = Matriz
            Else
                Throw New ExcepcionGeometrica3D("TRANSFORMACION3D (NEW): Las matrices de transformación tridimensional solo se pueden implementar con matrices cuadradas de 4x4" & vbNewLine _
                                                & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Sub

        Public Shared Function EncadenarTransformaciones(ByVal PrimeraTransformacion As Transformacion3D, ByVal SegundaTransformacion As Transformacion3D) As Transformacion3D
            Return New Transformacion3D(SegundaTransformacion.Matriz * PrimeraTransformacion.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Punto As Punto3D, ByVal Transformacion As Transformacion3D) As Punto3D
            Return New Punto3D(Transformacion.Matriz * Punto.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Vector As Vector3D, ByVal Transformacion As Transformacion3D) As Vector3D
            Return New Vector3D(Transformacion.Matriz * Vector.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Vertice As Vertice, ByVal Transformacion As Transformacion3D) As Vertice
            Return New Vertice(New Punto3D(Transformacion.Matriz * Vertice.CoodenadasSUR.Matriz))
        End Function

        Public Shared Function AplicarTransformacion(ByVal Recta As Recta3D, ByVal Transformacion As Transformacion3D) As Recta3D
            Return New Recta3D(Transformacion.Matriz * Recta.Matrices(0), Transformacion.Matriz * Recta.Matrices(1))
        End Function

        Public Shared Function AplicarTransformacion(ByVal Plano As Plano3D, ByVal Transformacion As Transformacion3D) As Plano3D
            Return New Plano3D(New Punto3D(Transformacion.Matriz * Plano.ObtenerPunto(0, 0).Matriz), New Vector3D(Transformacion.Matriz * Plano.VectorNormal.Matriz))
        End Function

        Public Shared Function AplicarTransformacion(ByVal AABB As AABB3D, ByVal Transformacion As Transformacion3D) As AABB3D
            Return New AABB3D(New Punto3D(Transformacion.Matriz * AABB.Posicion.Matriz), AABB.Dimensiones)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Segmento As Segmento3D, ByVal Transformacion As Transformacion3D) As Segmento3D
            Return New Segmento3D(New Punto3D(Transformacion.Matriz * Segmento.ExtremoInicial.Matriz), New Punto3D(Transformacion.Matriz * Segmento.ExtremoFinal.Matriz))
        End Function

        Public Shared Operator +(ByVal PrimeraTransformacion As Transformacion3D, ByVal SegundaTransformacion As Transformacion3D) As Transformacion3D
            Return EncadenarTransformaciones(PrimeraTransformacion, SegundaTransformacion)
        End Operator

        Public Shared Operator *(ByVal Punto As Punto3D, ByVal Transformacion As Transformacion3D) As Punto3D
            Return AplicarTransformacion(Punto, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion3D, ByVal Punto As Punto3D) As Punto3D
            Return AplicarTransformacion(Punto, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Vector As Vector3D, ByVal Transformacion As Transformacion3D) As Vector3D
            Return AplicarTransformacion(Vector, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion3D, ByVal Vector As Vector3D) As Vector3D
            Return AplicarTransformacion(Vector, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Recta As Recta3D, ByVal Transformacion As Transformacion3D) As Recta3D
            Return AplicarTransformacion(Recta, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion3D, ByVal Recta As Recta3D) As Recta3D
            Return AplicarTransformacion(Recta, Transformacion)
        End Operator

        Public Overrides Function ToString() As String
            Return "{Transformacion3D: Matriz (" & mMatriz.ToString & "}"
        End Function
    End Class
End Namespace

