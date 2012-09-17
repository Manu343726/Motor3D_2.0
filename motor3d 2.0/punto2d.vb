Imports Motor3D.Algebra
Imports System.Math

Namespace Espacio2D
    Public Class Punto2D
        Inherits ObjetoGeometrico2D

        Public Shadows Event Modificado(ByRef Sender As Punto2D)

        Private mX, mY As Double

        Public Property X() As Double
            Get
                Return mX
            End Get
            Set(ByVal value As Double)
                mX = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property Y() As Double
            Get
                Return mY
            End Get
            Set(ByVal value As Double)
                mY = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public ReadOnly Property EsCero() As Boolean
            Get
                Return (mX = 0 AndAlso mY = 0)
            End Get
        End Property

        Public ReadOnly Property Matriz() As Matriz
            Get
                Return RepresentacionMatricial(Me)
            End Get
        End Property

        Public Sub New()
            mX = 0
            mY = 0
        End Sub

        Public Sub New(ByVal ValX As Double, ByVal ValY As Double)
            mX = ValX
            mY = ValY
        End Sub

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

        Public Function ToPoint() As System.Drawing.Point
            Return ToPoint(Me)
        End Function

        Public Shadows Function ToString() As String
            Return "{" & FormatNumber(mX, 2) & ";" & FormatNumber(mY, 2) & "}"
        End Function

        Public Shared Operator =(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Boolean
            Return (P1.X = P2.X AndAlso P1.Y = P2.Y)
        End Operator

        Public Shared Operator <>(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Boolean
            Return Not (P1.X = P2.X AndAlso P1.Y = P2.Y)
        End Operator

        Public Shared Operator >(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Boolean
            Return P1.X > P2.X AndAlso P1.Y > P2.Y
        End Operator

        Public Shared Operator <(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Boolean
            Return Not P1 > P2
        End Operator

        Public Shared Operator +(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Punto2D
            Return New Punto2D(P1.X + P2.X, P1.Y + P2.Y)
        End Operator

        Public Shared Operator -(ByVal P1 As Punto2D, ByVal P2 As Punto2D) As Punto2D
            Return New Punto2D(P1.X - P2.X, P1.Y - P2.Y)
        End Operator

        Public Shared Operator *(ByVal Punto As Punto2D, ByVal K As Double) As Punto2D
            Return New Punto2D(Punto.X * K, Punto.Y * K)
        End Operator

        Public Shared Operator *(ByVal K As Double, ByVal Punto As Punto2D) As Punto2D
            Return New Punto2D(Punto.X * K, Punto.Y * K)
        End Operator

        Public Shared Operator /(ByVal Punto As Punto2D, ByVal K As Double) As Punto2D
            If K <> 0 Then
                Return New Punto2D(Punto.X / K, Punto.Y / K)
            Else
                Throw New ExcepcionGeometrica2D("PUNTO2D (OPERACION_DIVISION): Division por cero")
            End If
        End Operator

        Public Shared Function ProductoCruzado(ByVal P1 As Punto2D, ByVal P2 As Punto2D, ByVal P3 As Punto2D) As Double
            Return ((P2.X - P1.X) * (P2.Y - P2.Y)) - ((P2.Y - P1.Y) * (P3.X - P2.X))
        End Function

        Public Shared Function Envolvente(ByVal ParamArray Puntos() As Punto2D) As Punto2D()
            Return Puntos
        End Function

        Public Shared Function PoligonoEnvolvente(ByVal Puntos() As Punto2D) As Poligono2D
            Return New Poligono2D(Puntos)
        End Function

        Public Shared Function RepresentacionMatricial(ByVal Punto As Punto2D) As Matriz
            Dim Retorno As New Matriz(3, 1)

            Retorno.EstablecerValoresPorColumna(0, Punto.X, Punto.Y, 1)

            Return Retorno
        End Function

        Public Shared Function ToPoint(ByVal Punto As Punto2D) As System.Drawing.Point
            Dim x, y As Integer
            If Punto.X >= Integer.MinValue Then
                If Punto.X <= Integer.MaxValue Then
                    x = Punto.X
                Else
                    x = Integer.MaxValue - 1
                End If
            Else
                x = Integer.MinValue + 1
            End If
            If Punto.Y >= Integer.MinValue Then
                If Punto.Y <= Integer.MaxValue Then
                    y = Punto.Y
                Else
                    y = Integer.MaxValue - 1
                End If
            Else
                y = Integer.MinValue + 1
            End If

            Return New System.Drawing.Point(x, y)
        End Function

        Public Shared Function ToPoint(ByVal ParamArray Puntos() As Punto2D) As System.Drawing.Point()
            Dim Retorno(Puntos.GetUpperBound(0)) As System.Drawing.Point

            For i As Integer = 0 To Puntos.GetUpperBound(0)
                Retorno(i) = ToPoint(Puntos(i))
            Next

            Return Retorno
        End Function

        Public Shared Function Baricentro(ByVal ParamArray Puntos() As Punto2D) As Punto2D
            If Puntos.GetUpperBound(0) >= 0 Then
                Dim x, y As Double

                x = 0
                y = 0

                For i As Integer = 0 To Puntos.GetUpperBound(0)
                    x += Puntos(i).X
                    y += Puntos(i).Y
                Next

                Return New Punto2D(x / (Puntos.GetUpperBound(0) + 1), y / (Puntos.GetUpperBound(0) + 1))
            Else
                Return New Punto2D()
            End If
        End Function

        Public Shared Function Baricentro(ByVal Indices() As Integer, ByVal ParamArray Puntos() As Punto2D) As Punto2D
            Dim x, y As Double

            x = 0
            y = 0

            For i As Integer = 0 To Indices.GetUpperBound(0)
                x += Puntos(Indices(i)).X
                y += Puntos(Indices(i)).Y
            Next

            Return New Punto2D(x / (Indices.GetUpperBound(0) + 1), y / (Indices.GetUpperBound(0) + 1))
        End Function
    End Class
End Namespace
