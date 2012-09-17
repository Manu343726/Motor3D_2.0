Imports System.Math
Imports Motor3D.Algebra

Namespace Espacio3D
    Public Class Vector3D
        Inherits ObjetoGeometrico3D

        Private mArray(3, 0) As Double
        Private mMatriz As Matriz

        Public Property X() As Double
            Get
                Return mArray(0, 0)
            End Get
            Set(ByVal value As Double)
                mArray(0, 0) = value
            End Set
        End Property

        Public Property Y() As Double
            Get
                Return mArray(1, 0)
            End Get
            Set(ByVal value As Double)
                mArray(1, 0) = value
            End Set
        End Property

        Public Property Z() As Double
            Get
                Return mArray(2, 0)
            End Get
            Set(ByVal value As Double)
                mArray(2, 0) = value
            End Set
        End Property

        Public ReadOnly Property Matriz() As Matriz
            Get
                Return mMatriz
            End Get
        End Property

        Public ReadOnly Property EsNulo() As Boolean
            Get
                Return (mArray(0, 0) = 0 AndAlso mArray(1, 0) = 0 AndAlso mArray(2, 0) = 0)
            End Get
        End Property

        Public ReadOnly Property Modulo() As Double
            Get
                Return Sqrt((mArray(0, 0) ^ 2) + (mArray(1, 0) ^ 2) + (mArray(2, 0) ^ 2))
            End Get
        End Property

        Public ReadOnly Property VectorUnitario() As Vector3D
            Get
                If Modulo > 0 Then
                    Dim m As Double = Modulo
                    Return New Vector3D(mArray(0, 0) / m, mArray(1, 0) / m, mArray(2, 0) / m)
                Else
                    Return Me
                End If
            End Get
        End Property

        Public Sub New()
            ReDim mArray(3, 0)
            mMatriz = New Matriz(mArray)
            mArray(0, 0) = 0
            mArray(1, 0) = 0
            mArray(2, 0) = 0
            mArray(3, 0) = 1
        End Sub

        Public Sub New(ByVal ValX As Double, ByVal ValY As Double, ByVal ValZ As Double)
            ReDim mArray(3, 0)
            mMatriz = New Matriz(mArray)
            mArray(0, 0) = ValX
            mArray(1, 0) = ValY
            mArray(2, 0) = ValZ
            mArray(3, 0) = 1
        End Sub

        Public Sub New(ByVal P1 As Punto3D, ByVal P2 As Punto3D)
            ReDim mArray(3, 0)
            mMatriz = New Matriz(mArray)
            mArray(0, 0) = P2.X - P1.X
            mArray(1, 0) = P2.Y - P1.Y
            mArray(2, 0) = P2.Z - P1.Z
            mArray(3, 0) = 1
        End Sub

        Public Sub New(ByVal Punto As Punto3D)
            ReDim mArray(3, 0)
            mMatriz = New Matriz(mArray)
            mArray(0, 0) = Punto.X
            mArray(1, 0) = Punto.Y
            mArray(2, 0) = Punto.Z
            mArray(3, 0) = 1
        End Sub

        Public Sub New(ByVal Matriz As Matriz)
            mMatriz = Matriz
            mArray = Matriz.Array
            mArray(3, 0) = 1
        End Sub

        Public Sub Normalizar()
            If Modulo > 0 Then
                Dim m As Double
                m = Modulo
                mArray(0, 0) /= m
                mArray(1, 0) /= m
                mArray(2, 0) /= m
                mArray(3, 0) = 1
            End If
        End Sub

        Public Shared Function RepresentacionMatricial(ByVal Vector As Vector3D) As Matriz
            Return Vector.Matriz
        End Function

        Public Function Copia() As Vector3D
            Return Copia(Me)
        End Function

        Public Overrides Function ToString() As String
            Return "{" & FormatNumber(mArray(0, 0), 2) & ";" & FormatNumber(mArray(1, 0), 2) & ";" & FormatNumber(mArray(2, 0), 2) & "}"
        End Function

        Public Shared Function Copia(ByVal Vector As Vector3D) As Vector3D
            Return New Vector3D(Vector.X, Vector.Y, Vector.Z)
        End Function

        Public Shared Function Incremento(ByVal Vector As Vector3D, ByVal IncrementoX As Double, ByVal IncrementoY As Double, ByVal IncrementoZ As Double) As Vector3D
            Return New Vector3D(Vector.X + IncrementoX, Vector.Y + IncrementoY, Vector.Z + IncrementoZ)
        End Function

        Public Shared Function Suma(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return New Vector3D(V1.X + V2.X, V1.Y + V2.Y, V1.Z + V2.Z)
        End Function

        Public Shared Function Resta(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return New Vector3D(V1.X - V2.X, V1.Y - V2.Y, V1.Z - V2.Z)
        End Function

        Public Shared Function ProductoVectorial(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return New Vector3D((V1.Y * V2.Z) - (V2.Y - V1.Z), (V1.Z * V2.X) - (V2.Z - V1.X), (V1.X * V2.Y) - (V2.X * V1.Y))
        End Function

        Public Shared Function ProductoEscalar(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Double
            Return (V1.X * V2.X) + (V1.Y * V2.Y) + (V1.Z * V2.Z)
        End Function

        Public Shared Function ProductoMixto(ByVal V1 As Vector3D, ByVal V2 As Vector3D, ByVal V3 As Vector3D) As Double
            Dim Matriz As New Matriz(3, 3)

            Matriz.EstablecerValoresPorFila(0, V1.X, V1.Y, V1.Z)
            Matriz.EstablecerValoresPorFila(1, V2.X, V2.Y, V2.Z)
            Matriz.EstablecerValoresPorFila(2, V3.X, V3.Y, V3.Z)

            Return Matriz.CalculoDeterminante(Matriz)
        End Function

        Public Shared Function Producto(ByVal Vector As Vector3D, ByVal Factor As Double) As Vector3D
            Return New Vector3D(Vector.X * Factor, Vector.Y * Factor, Vector.Z * Factor)
        End Function

        Public Shared Function Producto(ByVal Factor As Double, ByVal Vector As Vector3D) As Vector3D
            Return New Vector3D(Vector.X * Factor, Vector.Y * Factor, Vector.Z * Factor)
        End Function

        Public Shared Function Division(ByVal Vector As Vector3D, ByVal Divisor As Double) As Vector3D
            If Divisor <> 0 Then
                Return New Vector3D(Vector.X / Divisor, Vector.Y / Divisor, Vector.Z / Divisor)
            Else
                Return Vector
            End If
        End Function

        Public Shared Function Angulo(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Single
            Return Acos((ProductoEscalar(V1, V2)) / (V1.Modulo * V2.Modulo))
        End Function

        Public Shared Function Inverso(ByVal Vector As Vector3D) As Vector3D
            Return New Vector3D(-Vector.X, -Vector.Y, -Vector.Z)
        End Function

        Public Shared Operator +(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return Suma(V1, V2)
        End Operator

        Public Shared Operator -(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return Incremento(V1, -V2.X, -V2.Y, -V2.Z)
        End Operator

        Public Shared Operator =(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Boolean
            Return (V1.X = V2.X AndAlso V1.Y = V2.Y AndAlso V1.Z = V2.Z)
        End Operator

        Public Shared Operator <>(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Boolean
            Return Not (V1 = V2)
        End Operator

        Public Shared Operator >(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Boolean
            Return V1.X > V2.X OrElse V1.Y > V2.Y OrElse V1.Z > V2.Z
        End Operator

        Public Shared Operator <(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Boolean
            Return Not V1 > V2
        End Operator

        Public Shared Operator Not(ByVal Vector As Vector3D) As Vector3D
            Return Inverso(Vector)
        End Operator

        Public Shared Operator *(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Double
            Return ProductoEscalar(V1, V2)
        End Operator

        Public Shared Operator *(ByVal Vector As Vector3D, ByVal Factor As Double) As Vector3D
            Return Producto(Vector, Factor)
        End Operator

        Public Shared Operator *(ByVal Factor As Double, ByVal Vector As Vector3D) As Vector3D
            Return Producto(Vector, Factor)
        End Operator

        Public Shared Operator /(ByVal Vector As Vector3D, ByVal Divisor As Double) As Vector3D
            Return Division(Vector, Divisor)
        End Operator

        Public Shared Operator &(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Vector3D
            Return ProductoVectorial(V1, V2)
        End Operator

        Public Shared Operator ^(ByVal V1 As Vector3D, ByVal V2 As Vector3D) As Single
            Return Angulo(V1, V2)
        End Operator
    End Class
End Namespace

