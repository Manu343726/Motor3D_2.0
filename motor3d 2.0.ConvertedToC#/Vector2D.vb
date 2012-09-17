Imports System.Math
Imports Motor3D.Algebra

Namespace Espacio2D
    Public Class Vector2D
        Inherits ObjetoGeometrico2D

        Private mX, mY As Double

        Public Property X() As Double
            Get
                Return mX
            End Get
            Set(ByVal value As Double)
                mX = value
            End Set
        End Property

        Public Property Y() As Double
            Get
                Return mY
            End Get
            Set(ByVal value As Double)
                mY = value
            End Set
        End Property

        Public ReadOnly Property Modulo() As Double
            Get
                Return Sqrt((mX ^ 2) + (mY ^ 2))
            End Get
        End Property

        Public ReadOnly Property Matriz As Matriz
            Get
                Return RepresentacionMatricial(Me)
            End Get
        End Property

        Public ReadOnly Property VectorUnitario() As Vector2D
            Get
                If Modulo > 0 Then
                    Return New Vector2D(mX / Modulo, mY / Modulo)
                Else
                    Return Me
                End If
            End Get
        End Property

        Public Sub New(ByVal Matriz As Matriz)
            If Matriz.Filas = 3 AndAlso Matriz.Columnas = 1 Then
                mX = Matriz.Valores(0, 0)
                mY = Matriz.Valores(1, 0)
            Else
                Throw New ExcepcionGeometrica2D("PUNTO2D (NEW): La representación matricial de un punto bidimensional" _
                                                & " corresponde a una matriz columna de tres elementos. " & vbNewLine _
                                                & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Sub

        Public Sub New(ByVal ValX As Double, ByVal ValY As Double)
            mX = ValX
            mY = ValY
        End Sub

        Public Sub New(ByVal P1 As Punto2D, ByVal P2 As Punto2D)
            mX = P2.X - P1.X
            mY = P2.Y - P1.Y
        End Sub

        Public Sub New(ByVal Punto As Punto2D)
            mX = Punto.X
            mY = Punto.Y
        End Sub

        Public Sub New()
            mX = 0
            mY = 0
        End Sub

        Public Sub Normalizar()
            Dim m As Double
            m = Modulo

            If m > 0 Then
                mX /= m
                mY /= m
            End If
        End Sub

        Public Shadows Function ToString() As String
            Return "{" & FormatNumber(mX, 8) & ";" & FormatNumber(mY, 8) & "}"
        End Function

        Public Shared Function RepresentacionMatricial(ByVal Vector As Vector2D) As Matriz
            Dim Retorno As New Matriz(3, 1)

            Retorno.EstablecerValoresPorColumna(0, Vector.X, Vector.Y, 1)

            Return Retorno
        End Function

        Public Shared Operator =(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Boolean
            Return (V1.X = V2.X AndAlso V1.Y = V2.Y)
        End Operator

        Public Shared Operator <>(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Boolean
            Return Not (V1.X = V2.X AndAlso V1.Y = V2.Y)
        End Operator

        Public Shared Shadows Operator +(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.X + V2.X, V1.Y + V2.Y)
        End Operator

        Public Shared Shadows Operator -(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.X - V2.X, V1.Y - V2.Y)
        End Operator

        Public Shared Operator *(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return (V1.X * V2.X) + (V1.Y * V2.Y)
        End Operator

        Public Shared Operator *(ByVal Vector As Vector2D, ByVal Factor As Double) As Vector2D
            Return New Vector2D(Vector.X * Factor, Vector.Y * Factor)
        End Operator

        Public Shared Operator *(ByVal Factor As Double, ByVal Vector As Vector2D) As Vector2D
            Return New Vector2D(Vector.X * Factor, Vector.Y * Factor)
        End Operator

        Public Shared Operator ^(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return Acos((V1 * V2) / (V1.Modulo * V2.Modulo))
        End Operator

        Public Shared Operator Not(ByVal Vector As Vector2D) As Vector2D
            Return New Vector2D(Vector.Y, -Vector.X)
        End Operator

        Public Shared Function Suma(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.X + V2.X, V1.Y + V2.Y)
        End Function

        Public Shared Function Resta(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Vector2D
            Return New Vector2D(V1.X - V2.X, V1.Y - V2.Y)
        End Function

        Public Shared Function ProductoEscalar(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return (V1.X * V2.X) + (V1.Y * V2.Y)
        End Function

        Public Shared Function ProductoCruzado(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return (V1.X * V2.Y) - (V1.Y * V2.X)
        End Function

        Public Shared Function ProductoCruzado(ByVal P1 As Punto2D, ByVal P2 As Punto2D, ByVal P3 As Punto2D) As Double
            Return ProductoCruzado(New Vector2D(P1, P2), New Vector2D(P2, P3))
        End Function

        Public Shared Function SignoProductoCruzado(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return Sign(ProductoCruzado(V1, V2))
        End Function

        Public Shared Function SignoProductoCruzado(ByVal P1 As Punto2D, ByVal P2 As Punto2D, ByVal P3 As Punto2D) As Double
            Return Sign(ProductoCruzado(P1, P2, P3))
        End Function

        Public Shared Function Angulo(ByVal V1 As Vector2D, ByVal V2 As Vector2D) As Double
            Return Acos((V1 * V2) / (V1.Modulo * V2.Modulo))
        End Function
    End Class
End Namespace

