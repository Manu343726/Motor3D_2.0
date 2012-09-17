Imports Motor3D.Algebra
Imports System.Math

Namespace Espacio2D
    Public Class Transformacion2D
        Inherits ObjetoGeometrico2D

        Private mMatriz As Matriz

        Public ReadOnly Property Matriz() As Matriz
            Get
                Return mMatriz
            End Get
        End Property

        Public Sub New()
            mMatriz = Algebra.Matriz.MatrizUnitaria(3)
        End Sub

        Public Sub New(ByVal Matriz As Matriz)
            If Matriz.EsCuadrada AndAlso Matriz.Filas = 3 Then
                mMatriz = Matriz
            Else
                Throw New ExcepcionGeometrica2D("TRANSFORMACION2D (NEW): Las matrices de transformación bidimensional solo se pueden implementar con matrices cuadradas de 3x3" & vbNewLine _
                                                & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Sub

        Public Sub AplicarTransformacion(ByRef Punto As Punto2D)
            Punto = AplicarTransformacion(Punto, Me)
        End Sub

        Public Function Transformacion(ByVal Punto As Punto2D) As Punto2D
            Return AplicarTransformacion(Punto, Me)
        End Function

        Public Shared Function Rotacion(ByVal Giro As Single) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, Cos(Giro), Sin(Giro), 0)
            Retorno.EstablecerValoresPorFila(1, -Sin(Giro), Cos(Giro), 0)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function Rotacion(ByVal Seno As Double, ByVal Coseno As Double) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, Coseno, Seno, 0)
            Retorno.EstablecerValoresPorFila(1, -Seno, Coseno, 0)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function Rotacion(ByVal Vector As Vector2D) As Transformacion2D
            Return Rotacion(-Vector.Y, Vector.X)
        End Function

        Public Shared Function Rotacion(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Transformacion2D
            Try
                Return New Transformacion2D((Rotacion(V2).Matriz * Rotacion(V1).Matriz.Inversa))
            Catch ex As ExcepcionMatriz
                Return New Transformacion2D(Algebra.Matriz.MatrizUnitaria(3))
            End Try
        End Function

        Public Shared Function Traslacion(ByVal VectorTraslacion As Vector2D) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, 1, 0, VectorTraslacion.X)
            Retorno.EstablecerValoresPorFila(1, 0, 1, VectorTraslacion.Y)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function Traslacion(ByVal X As Double, ByVal Y As Double) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, 1, 0, X)
            Retorno.EstablecerValoresPorFila(1, 0, 1, Y)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function Escalado(ByVal VectorEscalado As Vector2D) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, VectorEscalado.X, 0, 0)
            Retorno.EstablecerValoresPorFila(1, 0, VectorEscalado.Y, 0)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function Escalado(ByVal X As Double, ByVal Y As Double) As Transformacion2D
            Dim Retorno As New Matriz(3, 3)

            Retorno.EstablecerValoresPorFila(0, X, 0, 0)
            Retorno.EstablecerValoresPorFila(1, 0, Y, 0)
            Retorno.EstablecerValoresPorFila(2, 0, 0, 1)

            Return New Transformacion2D(Retorno)
        End Function

        Public Shared Function EncadenarTransformaciones(ByVal PrimeraTransformacion As Transformacion2D, ByVal SegundaTransformacion As Transformacion2D) As Transformacion2D
            Return New Transformacion2D(SegundaTransformacion.Matriz * PrimeraTransformacion.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Punto As Punto2D, ByVal Transformacion As Transformacion2D) As Punto2D
            Return New Punto2D(Transformacion.Matriz * Punto.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Vector As Vector2D, ByVal Transformacion As Transformacion2D) As Vector2D
            Return New Vector2D(Transformacion.Matriz * Vector.Matriz)
        End Function

        Public Shared Function AplicarTransformacion(ByVal Recta As Recta2D, ByVal Transformacion As Transformacion2D) As Recta2D
            Return New Recta2D(Transformacion.Matriz * Recta.Matrices(0), Transformacion.Matriz * Recta.Matrices(1))
        End Function

        Public Shared Function AplicarTransformacion(ByVal Poligono As Poligono2D, ByVal Transformacion As Transformacion2D) As Poligono2D
            Dim Vertices() As Punto2D = Poligono.Vertices

            For i As Integer = 0 To Vertices.GetUpperBound(0)
                Vertices(i) = Vertices(i) * Transformacion
            Next

            Return New Poligono2D(Vertices)
        End Function

        Public Shared Function TransformacionInversa(ByVal Transformacion As Transformacion2D) As Transformacion2D
            Return New Transformacion2D(Algebra.Matriz.CalculoInversa(Transformacion.Matriz))
        End Function

        Public Shared Operator +(ByVal PrimeraTransformacion As Transformacion2D, ByVal SegundaTransformacion As Transformacion2D) As Transformacion2D
            Return EncadenarTransformaciones(PrimeraTransformacion, SegundaTransformacion)
        End Operator

        Public Shared Operator *(ByVal Punto As Punto2D, ByVal Transformacion As Transformacion2D) As Punto2D
            Return AplicarTransformacion(Punto, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion2D, ByVal Punto As Punto2D) As Punto2D
            Return AplicarTransformacion(Punto, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Vector As Vector2D, ByVal Transformacion As Transformacion2D) As Vector2D
            Return AplicarTransformacion(Vector, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion2D, ByVal Vector As Vector2D) As Vector2D
            Return AplicarTransformacion(Vector, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Recta As Recta2D, ByVal Transformacion As Transformacion2D) As Recta2D
            Return AplicarTransformacion(Recta, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion2D, ByVal Recta As Recta2D) As Recta2D
            Return AplicarTransformacion(Recta, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Transformacion As Transformacion2D, ByVal Poligono As Poligono2D) As Poligono2D
            Return AplicarTransformacion(Poligono, Transformacion)
        End Operator

        Public Shared Operator *(ByVal Poligono As Poligono2D, ByVal Transformacion As Transformacion2D) As Poligono2D
            Return AplicarTransformacion(Poligono, Transformacion)
        End Operator

        Public Shared Operator Not(ByVal Transformacion As Transformacion2D) As Transformacion2D
            Return TransformacionInversa(Transformacion)
        End Operator

        Public Overrides Function ToString() As String
            Return "{Transformacion2D: Matriz (" & mMatriz.ToString & "}"
        End Function
    End Class
End Namespace

