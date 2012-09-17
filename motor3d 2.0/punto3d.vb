Imports Motor3D.Algebra

Namespace Espacio3D
    Public Class Punto3D
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

        Public Property Matriz() As Matriz
            Get
                Return mMatriz
            End Get
            Set(value As Matriz)
                mMatriz = value
                mMatriz(3, 0) = 1
            End Set
        End Property

        Public Property Array As Double(,)
            Get
                Return mArray
            End Get
            Set(value As Double(,))
                mArray = value
                mArray(3, 0) = 1
            End Set
        End Property

        Public ReadOnly Property EsNulo() As Boolean
            Get
                Return (mArray(0, 0) = 0 AndAlso mArray(1, 0) = 0 AndAlso mArray(2, 0) = 0)
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

        Public Sub New(ByVal Matriz As Matriz)
            mMatriz = Matriz
            mArray = Matriz.Array
            mArray(3, 0) = 1
        End Sub

        Public Shared Function RepresentacionMatricial(ByVal Punto As Punto3D) As Matriz
            Return Punto.Matriz
        End Function

        Public Function Copia() As Punto3D
            Return Copia(Me)
        End Function

        Public Overrides Function ToString() As String
            Return "{" & FormatNumber(mArray(0, 0), 2) & ";" & FormatNumber(mArray(1, 0), 2) & ";" & FormatNumber(mArray(2, 0), 2) & "}"
        End Function

        Public Shared Function Copia(ByVal Punto As Punto3D) As Punto3D
            Return New Punto3D(Punto.X, Punto.Y, Punto.Z)
        End Function

        Public Shared Function Incremento(ByVal Punto As Punto3D, ByVal IncrementoX As Double, ByVal IncrementoY As Double, ByVal IncrementoZ As Double) As Punto3D
            Return New Punto3D(Punto.X + IncrementoX, Punto.Y + IncrementoY, Punto.Z + IncrementoZ)
        End Function

        Public Shared Function Incremento(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Punto3D
            Return New Punto3D(P1.X + P2.X, P1.Y + P2.Y, P1.Z + P2.Z)
        End Function

        Public Shared Operator +(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Punto3D
            Return Incremento(P1, P2)
        End Operator

        Public Shared Operator -(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Punto3D
            Return Incremento(P1, -P2.X, -P2.Y, -P2.Z)
        End Operator

        Public Shared Operator *(ByVal Punto As Punto3D, ByVal K As Double) As Punto3D
            Return New Punto3D(Punto.X * K, Punto.Y * K, Punto.Z * K)
        End Operator

        Public Shared Operator *(ByVal K As Double, ByVal Punto As Punto3D) As Punto3D
            Return New Punto3D(Punto.X * K, Punto.Y * K, Punto.Z * K)
        End Operator

        Public Shared Operator /(ByVal Punto As Punto3D, ByVal K As Double) As Punto3D
            If K <> 0 Then
                Return New Punto3D(Punto.X / K, Punto.Y / K, Punto.Z / K)
            Else
                Throw New ExcepcionGeometrica3D("PUNTO3D (OPERADOR_DIVISION): División por cero")
            End If
        End Operator

        Public Shared Operator =(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Boolean
            Return (P1.X = P2.X AndAlso P1.Y = P2.Y AndAlso P1.Z = P2.Z)
        End Operator

        Public Shared Operator <>(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Boolean
            Return Not (P1.X = P2.X AndAlso P1.Y = P2.Y AndAlso P1.Z = P2.Z)
        End Operator

        Public Shared Operator >(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Boolean
            Return P1.X > P2.X OrElse P1.Y > P2.Y OrElse P1.Z > P2.Z
        End Operator

        Public Shared Operator <(ByVal P1 As Punto3D, ByVal P2 As Punto3D) As Boolean
            Return Not P1 > P2
        End Operator

        Public Shared Function Baricentro(ByVal ParamArray Puntos() As Punto3D) As Punto3D
            Dim x, y, z As Double

            x = 0
            y = 0
            z = 0

            For i As Integer = 0 To Puntos.GetUpperBound(0)
                x += Puntos(i).X
                y += Puntos(i).Y
                z += Puntos(i).Z
            Next

            Return New Punto3D(x / (Puntos.GetUpperBound(0) + 1), y / (Puntos.GetUpperBound(0) + 1), z / (Puntos.GetUpperBound(0) + 1))
        End Function
    End Class
End Namespace

