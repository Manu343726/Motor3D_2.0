Imports System.Math
Imports Motor3D.Algebra

Namespace Espacio2D
    Public Enum Orientacion2D
        NORTE
        SUR
        ESTE
        OESTE
        NORESTE
        SURESTE
        SUROESTE
        NOROESTE
        DENTRO
        FUERA
    End Enum

    Public Class AABB2D
        Inherits ObjetoGeometrico2D

        Private Pos As Punto2D
        Private Size As Punto2D

        Public ReadOnly Property Posicion() As Punto2D
            Get
                Return Pos
            End Get
        End Property

        Public Property Centro() As Punto2D
            Get
                Return New Punto2D(Pos.X + (Size.X / 2), Pos.Y + (Size.Y / 2))
            End Get
            Set(ByVal value As Punto2D)
                Pos = New Punto2D(value.X - (Size.X / 2), value.Y - (Size.Y / 2))
            End Set
        End Property

        Public Property Dimensiones() As Punto2D
            Get
                Return Size
            End Get
            Set(ByVal value As Punto2D)
                Size = value
            End Set
        End Property

        Public ReadOnly Property MinX() As Double
            Get
                Return Pos.X
            End Get
        End Property

        Public ReadOnly Property MinY() As Double
            Get
                Return Pos.Y
            End Get
        End Property

        Public ReadOnly Property LongitudX() As Double
            Get
                Return Size.X
            End Get
        End Property

        Public ReadOnly Property LongitudY() As Double
            Get
                Return Size.Y
            End Get
        End Property

        Public ReadOnly Property MaxX() As Double
            Get
                Return Size.X + Pos.X
            End Get
        End Property

        Public ReadOnly Property MaxY() As Double
            Get
                Return Size.Y + Pos.Y
            End Get
        End Property

        Public ReadOnly Property Area As Double
            Get
                Return Size.X * Size.Y
            End Get
        End Property

        Public ReadOnly Property Esquinas As Punto2D()
            Get
                Dim Retorno(3) As Punto2D

                Retorno(0) = EsquinaSuperiorIzquierda
                Retorno(1) = EsquinaSuperiorDerecha
                Retorno(2) = EsquinaInferiorDerecha
                Retorno(3) = EsquinaInferiorIzquierda

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property EsquinaSuperiorIzquierda() As Punto2D
            Get
                Return Pos
            End Get
        End Property

        Public ReadOnly Property EsquinaSuperiorDerecha() As Punto2D
            Get
                Return New Punto2D(Pos.X + Size.X, Pos.Y)
            End Get
        End Property

        Public ReadOnly Property EsquinaInferiorIzquierda() As Punto2D
            Get
                Return New Punto2D(Pos.X, Pos.Y + Size.Y)
            End Get
        End Property

        Public ReadOnly Property EsquinaInferiorDerecha() As Punto2D
            Get
                Return New Punto2D(Pos.X + Size.X, Pos.Y + Size.Y)
            End Get
        End Property

        Public ReadOnly Property Poligono As Poligono2D
            Get
                Return New Poligono2D(EsquinaSuperiorIzquierda, EsquinaSuperiorDerecha, EsquinaInferiorDerecha, EsquinaInferiorIzquierda)
            End Get
        End Property

        Public Sub New(ByVal Posicion As Punto2D, ByVal Tamaño As Punto2D)
            Pos = New Punto2D
            Size = New Punto2D

            If Tamaño.X < 0 Then
                Pos.X = Posicion.X + Tamaño.X
                Size.X = -Tamaño.X
            Else
                Pos.X = Posicion.X
                Size.X = Tamaño.X
            End If
            If Tamaño.Y < 0 Then
                Pos.Y = Posicion.Y + Tamaño.Y
                Size.Y = -Tamaño.Y
            Else
                Pos.Y = Posicion.Y
                Size.Y = Tamaño.Y
            End If
        End Sub

        Public Sub New(ByVal PosX As Double, ByVal PosY As Double, ByVal Width As Double, ByVal Height As Double)
            Pos = New Punto2D
            Size = New Punto2D

            If Width < 0 Then
                Pos.X = PosX + Width
                Size.X = -Width
            Else
                Pos.X = PosX
                Size.X = Width
            End If
            If Height < 0 Then
                Pos.Y = PosY + Height
                Size.Y = -Height
            Else
                Pos.Y = PosY
                Size.Y = Height
            End If
        End Sub

        Public Sub New(ByVal ParamArray Puntos() As Punto2D)
            Dim maxx, maxy, minx, miny As Double

            maxx = Puntos(0).X
            maxy = Puntos(0).Y
            minx = Puntos(0).X
            miny = Puntos(0).Y

            For i As Integer = 1 To Puntos.GetUpperBound(0)
                If maxx < Puntos(i).X Then maxx = Puntos(i).X
                If maxy < Puntos(i).Y Then maxy = Puntos(i).Y
                If minx > Puntos(i).X Then minx = Puntos(i).X
                If miny > Puntos(i).Y Then miny = Puntos(i).Y
            Next

            Pos = New Punto2D(minx, miny)
            Size = New Punto2D(Abs(maxx - minx), Abs(maxy - miny))
        End Sub

        Public Shared Function Pertenece(ByVal AABB As AABB2D, ByVal Punto As Punto2D) As Boolean
            Return (Punto.X >= AABB.MinX AndAlso Punto.X <= AABB.MaxX AndAlso Punto.Y >= AABB.MinY AndAlso Punto.Y <= AABB.MaxY)
        End Function

        Public Shared Function Pertenece(ByVal AABB As AABB2D, ByVal Punto As Matriz) As Boolean
            Return (Punto(0, 0) >= AABB.MinX AndAlso Punto(0, 0) <= AABB.MaxX AndAlso Punto(1, 0) >= AABB.MinY AndAlso Punto(1, 0) <= AABB.MaxY)
        End Function

        Public Shared Function Colision(ByVal C1 As AABB2D, ByVal C2 As AABB2D) As Boolean
            Dim R As New AABB2D(C1.Posicion.X - C2.LongitudX, C1.Posicion.Y - C2.LongitudY, C1.LongitudX + C2.LongitudX, C1.LongitudY + C2.LongitudY)

            Return Pertenece(R, C2.Posicion)
        End Function

        Public Shared Function PosicionRelativa(ByRef A1 As AABB2D, ByRef A2 As AABB2D) As Orientacion2D
            Dim R As New AABB2D(A1.Posicion.X - A2.LongitudX, A1.Posicion.Y - A2.LongitudY, A1.LongitudX + A2.LongitudX, A1.LongitudY + A2.LongitudY)

            If R.Pertenece(A2.Posicion) Then
                Dim SuperiorIzquierda As Boolean = A1.Pertenece(A2.EsquinaSuperiorIzquierda) OrElse A2.Pertenece(A1.EsquinaInferiorDerecha)
                Dim SuperiorDerecha As Boolean = A1.Pertenece(A2.EsquinaSuperiorDerecha) OrElse A2.Pertenece(A1.EsquinaInferiorIzquierda)
                Dim InferiorDerecha As Boolean = A1.Pertenece(A2.EsquinaInferiorDerecha) OrElse A2.Pertenece(A1.EsquinaSuperiorIzquierda)
                Dim InferiorIzquierda As Boolean = A1.Pertenece(A2.EsquinaInferiorIzquierda) OrElse A2.Pertenece(A1.EsquinaSuperiorDerecha)

                If SuperiorIzquierda AndAlso InferiorDerecha Then
                    Return Orientacion2D.DENTRO
                ElseIf SuperiorIzquierda AndAlso SuperiorDerecha Then
                    Return Orientacion2D.NORTE
                ElseIf InferiorIzquierda AndAlso InferiorDerecha Then
                    Return Orientacion2D.SUR
                ElseIf SuperiorIzquierda AndAlso InferiorIzquierda Then
                    Return Orientacion2D.ESTE
                ElseIf SuperiorDerecha AndAlso InferiorDerecha Then
                    Return Orientacion2D.OESTE
                ElseIf SuperiorIzquierda Then
                    Return Orientacion2D.NOROESTE
                ElseIf SuperiorDerecha Then
                    Return Orientacion2D.NORESTE
                ElseIf InferiorDerecha Then
                    Return Orientacion2D.SURESTE
                ElseIf InferiorIzquierda Then
                    Return Orientacion2D.SUROESTE
                Else
                    Return Orientacion2D.FUERA
                End If
            Else
                Return Orientacion2D.FUERA
            End If
        End Function

        Public Shared Function DatosColision(ByRef A1 As AABB2D, ByRef A2 As AABB2D) As DatosColision2D
            Dim Interseccion As AABB2D = AABB2D.Interseccion(A1, A2)

            Return New DatosColision2D(Interseccion.Centro, New Vector2D(A1.Centro, A2.Centro), Interseccion)
        End Function

        Public Shared Function SubAABBsDiagonales(ByVal AABB As AABB2D, ByVal PendienteDiagonal As Integer) As AABB2D()
            Return SubAABBsDiagonales(AABB, PendienteDiagonal, 1)
        End Function

        Public Shared Function SubAABBsDiagonales(ByVal AABB As AABB2D, ByVal PendienteDiagonal As Integer, ByVal Nivel As Integer) As AABB2D()
            Dim AABBs() As AABB2D

            ReDim AABBs((2 ^ Nivel) - 1)
            If PendienteDiagonal >= 0 Then
                For i As Integer = 0 To (2 ^ Nivel) - 1
                    AABBs(i) = New AABB2D(New Punto2D(AABB.Posicion.X + i * (AABB.LongitudX / (2 ^ Nivel)), AABB.Posicion.Y + i * (AABB.LongitudY / (2 ^ Nivel))), New Punto2D((AABB.LongitudX / (2 ^ Nivel)), (AABB.LongitudY / (2 ^ Nivel))))
                Next
            Else
                For i As Integer = 0 To (2 ^ Nivel) - 1
                    AABBs(i) = New AABB2D(New Punto2D(AABB.Posicion.X + AABB.LongitudX - i * (AABB.LongitudX / (2 ^ Nivel)) - (AABB.LongitudX / (2 ^ Nivel)), AABB.Posicion.Y + i * (AABB.LongitudY / (2 ^ Nivel))), New Punto2D((AABB.LongitudX / (2 ^ Nivel)), (AABB.LongitudY / (2 ^ Nivel))))
                Next
            End If

            Return AABBs
        End Function

        Public Shared Function Interseccion(ByRef A1 As AABB2D, ByRef A2 As AABB2D) As AABB2D
            Dim px, py, dx, dy As Double

            If A2.MinX > A1.MinX Then
                dx = A1.MinX + A1.LongitudX - A2.MinX
                px = A2.MinX
            Else
                dx = A2.MinX + A2.LongitudX - A1.MinX
                px = A1.MinX
            End If

            If A2.MinY > A1.MinY Then
                dy = A1.MinY + A1.LongitudY - A2.MinY
                py = A2.MinY
            Else
                dy = A2.MinY + A2.LongitudY - A1.MinY
                py = A1.MinY
            End If

            Return New AABB2D(px, py, dx, dy)
        End Function

        Public Shared Function Union(ByRef A1 As AABB2D, ByRef A2 As AABB2D) As AABB2D
            Return New AABB2D(A1.Posicion.X - A2.LongitudX, A1.Posicion.Y - A2.LongitudY, A1.LongitudX + A2.LongitudX, A1.LongitudY + A2.LongitudY)
        End Function

        Public Function PosicionRelativa(ByRef AABB As AABB2D) As Orientacion2D
            Return PosicionRelativa(Me, AABB)
        End Function

        Public Function Pertenece(ByRef Punto As Punto2D) As Boolean
            Return (Punto.X >= Pos.X AndAlso Punto.X <= (Pos.X + Size.X) AndAlso Punto.Y >= Pos.Y AndAlso Punto.Y <= (Pos.Y + Size.Y))
        End Function

        Public Function Pertenece(ByRef Punto As Matriz) As Boolean
            Return (Punto(0, 0) >= Pos.X AndAlso Punto(0, 0) <= (Pos.X + Size.X) AndAlso Punto(1, 0) >= Pos.Y AndAlso Punto(1, 0) <= (Pos.Y + Size.Y))
        End Function

        Public Function Pertenece(ByRef Segmento As Segmento2D) As Boolean
            Return (Colision(Segmento.AABB) AndAlso Pertenece(Segmento.Recta))
        End Function

        Public Function Pertenece(ByRef Recta As Recta2D) As Boolean
            Dim Posicion As Double = Recta.SignoPosicionRelativa(EsquinaSuperiorIzquierda)
            Dim P As Double

            P = Recta.SignoPosicionRelativa(EsquinaSuperiorDerecha)
            If P = Posicion Then
                P = Recta.SignoPosicionRelativa(EsquinaInferiorDerecha)
                If P = Posicion Then
                    P = Recta.SignoPosicionRelativa(EsquinaInferiorIzquierda)
                    If P = Posicion Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return True
            End If
        End Function

        Public Function Colision(ByRef AABB As AABB2D) As Boolean
            Dim R As New AABB2D(Pos.X - AABB.LongitudX, Pos.Y - AABB.LongitudY, Size.X + AABB.LongitudX, Size.Y + AABB.LongitudY)

            Return Pertenece(R, AABB.Posicion)
        End Function

        Public Overrides Function ToString() As String
            Return "[Posición=" & Pos.ToString & ";" & "Dimensiones=" & Size.ToString & "]"
        End Function

        Public Shared Operator +(ByVal A1 As AABB2D, ByVal A2 As AABB2D) As AABB2D
            Return AABB2D.Union(A1, A2)
        End Operator

        Public Shared Operator -(ByVal A1 As AABB2D, ByVal A2 As AABB2D) As AABB2D
            Return AABB2D.Interseccion(A1, A2)
        End Operator

        Public Shared Operator =(ByVal C1 As AABB2D, ByVal C2 As AABB2D) As Boolean
            Return (C1.Posicion = C2.Posicion) AndAlso (C1.Dimensiones = C2.Dimensiones)
        End Operator

        Public Shared Operator <>(ByVal C1 As AABB2D, ByVal C2 As AABB2D) As Boolean
            Return Not (C1 = C2)
        End Operator
    End Class
End Namespace


