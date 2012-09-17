Imports System.Math

Namespace Espacio2D
    Public Class Segmento2D
        Inherits ObjetoGeometrico2D

        Private Inicio, Fin As Punto2D
        Private mRecta As Recta2D
        Private Modulo As Double
        Private Rectangulo As AABB2D

        Public Property ExtremoInicial() As Punto2D
            Get
                Return Inicio
            End Get
            Set(ByVal value As Punto2D)
                Inicio = value
                RecalcAABB()
            End Set
        End Property

        Public Property ExtremoFinal() As Punto2D
            Get
                Return Fin
            End Get
            Set(ByVal value As Punto2D)
                Fin = value
                RecalcAABB()
            End Set
        End Property

        Public ReadOnly Property ParametroExtremoInicial As Double
            Get
                Return mRecta.ObtenerParametro(Inicio)
            End Get
        End Property

        Public ReadOnly Property ParametroExtremoFinal As Double
            Get
                Return mRecta.ObtenerParametro(Fin)
            End Get
        End Property

        Public ReadOnly Property Recta() As Recta2D
            Get
                Return mRecta
            End Get
        End Property

        Public ReadOnly Property AABB() As AABB2D
            Get
                Return Rectangulo
            End Get
        End Property

        Public ReadOnly Property Longitud() As Double
            Get
                Return Modulo
            End Get
        End Property

        Public ReadOnly Property Pendiente(Optional ByVal YInvertida As Boolean = True) As Integer
            Get
                If Not YInvertida Then
                    If Inicio.X <= Fin.X AndAlso Inicio.Y >= Fin.Y Then Return 0
                    If Inicio.X >= Fin.X AndAlso Inicio.Y >= Fin.Y Then Return -1
                    If Inicio.X >= Fin.X AndAlso Inicio.Y <= Fin.Y Then Return 0
                    If Inicio.X <= Fin.X AndAlso Inicio.Y <= Fin.Y Then Return -1
                Else
                    If Inicio.X <= Fin.X AndAlso Inicio.Y >= Fin.Y Then Return -1
                    If Inicio.X >= Fin.X AndAlso Inicio.Y >= Fin.Y Then Return 0
                    If Inicio.X >= Fin.X AndAlso Inicio.Y <= Fin.Y Then Return -1
                    If Inicio.X <= Fin.X AndAlso Inicio.Y <= Fin.Y Then Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property PuntoMedio() As Punto2D
            Get
                Return New Punto2D((Inicio.X + Fin.X) / 2, (Inicio.Y + Fin.Y) / 2)
            End Get
        End Property

        Public Sub New(ByVal P1 As Punto2D, ByVal P2 As Punto2D)
            Inicio = P1
            Fin = P2

            Modulo = Sqrt(((P1.X + P2.X) ^ 2) + ((P1.Y + P2.Y) ^ 2))
            mRecta = New Recta2D(P1, P2)

            Rectangulo = New AABB2D(P1, New Punto2D(P2.X - P1.X, P2.Y - P1.Y))
        End Sub

        Private Sub RecalcAABB()
            mRecta = New Recta2D(Inicio, Fin)
            Rectangulo = New AABB2D(Inicio, New Punto2D(Fin.X - Inicio.X, Fin.Y - Inicio.Y))
            Modulo = Sqrt(((Inicio.X + Fin.X) ^ 2) + ((Inicio.Y + Fin.Y) ^ 2))
        End Sub

        Public Function Pertenece(ByVal Punto As Punto2D) As Boolean
            Return Pertenece(Me, Punto)
        End Function

        Public Function Pertenece(ByVal Segmento As Segmento2D, ByVal Punto As Punto2D) As Boolean
            Return Rectangulo.Pertenece(Punto) AndAlso mRecta.Pertenece(Punto)
        End Function

        Public Shared Function Colisionan(ByVal S1 As Segmento2D, ByVal S2 As Segmento2D, ByVal Niveles As Integer, Optional ByVal BSP As Boolean = False) As Boolean
            Dim AABBsS1(), AABBsS2() As AABB2D
            If BSP Then
                Dim Nivel As Integer = 1
                If Niveles < 1 Then Niveles = 1

                AABBsS1 = AABB2D.SubAABBsDiagonales(S1.AABB, S1.Pendiente)
                AABBsS2 = AABB2D.SubAABBsDiagonales(S2.AABB, S2.Pendiente)

                Do While True
                    For i As Integer = 0 To 1
                        For j As Integer = 0 To 1
                            If AABB2D.Colision(AABBsS1(i), AABBsS2(j)) Then
                                If Nivel < Niveles Then
                                    AABBsS1 = AABB2D.SubAABBsDiagonales(AABBsS1(i), S1.Pendiente)
                                    AABBsS2 = AABB2D.SubAABBsDiagonales(AABBsS2(j), S2.Pendiente)
                                    Nivel += 1
                                    Continue Do
                                Else
                                    Return True
                                End If
                            Else
                                If Nivel = Niveles AndAlso Circunferencia2D.Colisionan(New Circunferencia2D(AABBsS1(i)), New Circunferencia2D(AABBsS2(j))) Then
                                    Return True
                                End If
                            End If
                        Next
                    Next

                    Return False
                Loop
                Return False
            Else
                AABBsS1 = AABB2D.SubAABBsDiagonales(S1.AABB, S1.Pendiente, Niveles)
                AABBsS2 = AABB2D.SubAABBsDiagonales(S2.AABB, S2.Pendiente, Niveles)

                For i As Integer = 0 To AABBsS1.GetUpperBound(0)
                    For j As Integer = 0 To AABBsS2.GetUpperBound(0)
                        If AABB2D.Colision(AABBsS1(i), AABBsS2(j)) Then
                            Return True
                        End If
                    Next
                Next

                Return False
            End If

        End Function

        Public Shared Function UltimasAABBsBSP(ByVal S1 As Segmento2D, ByVal S2 As Segmento2D, Optional ByVal NivelesBSP As Integer = 1) As AABB2D()
            Dim Nivel As Integer = 1
            Dim AABBsS1(), AABBsS2() As AABB2D
            If NivelesBSP < 1 Then NivelesBSP = 1
            Dim Retorno() As AABB2D

            AABBsS1 = AABB2D.SubAABBsDiagonales(S1.AABB, S1.Pendiente)
            AABBsS2 = AABB2D.SubAABBsDiagonales(S2.AABB, S2.Pendiente)

            Do While True
                For i As Integer = 0 To 1
                    For j As Integer = 0 To 1
                        If AABB2D.Colision(AABBsS1(i), AABBsS2(j)) Then
                            If Nivel < NivelesBSP Then
                                AABBsS1 = AABB2D.SubAABBsDiagonales(AABBsS1(i), S1.Pendiente)
                                AABBsS2 = AABB2D.SubAABBsDiagonales(AABBsS2(j), S2.Pendiente)
                                Nivel += 1
                                Continue Do
                            Else
                                ReDim Retorno(1)

                                Retorno(0) = AABBsS1(i)
                                Retorno(1) = AABBsS2(j)

                                Return Retorno
                            End If
                        End If
                    Next
                Next

                ReDim Retorno(3)

                Retorno(0) = AABBsS1(0)
                Retorno(1) = AABBsS1(1)
                Retorno(2) = AABBsS2(0)
                Retorno(3) = AABBsS2(1)

                Return Retorno
            Loop

            ReDim Retorno(3)

            Retorno(0) = AABBsS1(0)
            Retorno(1) = AABBsS1(1)
            Retorno(2) = AABBsS2(0)
            Retorno(3) = AABBsS2(1)

            Return Retorno
        End Function

        Public Shared Function Interseccion(ByVal S1 As Segmento2D, ByVal S2 As Segmento2D) As Punto2D
            Try
                Dim Pi As Punto2D = Recta2D.Interseccion(S1.Recta, S2.Recta)

                Return Pi
            Catch ex As ExcepcionGeometrica2D
                Return Nothing
            End Try
        End Function

        Public Overrides Function ToString() As String
            Return "{Segmento. Inicio=" & Inicio.ToString & ",Fin=" & Fin.ToString & "}"
        End Function
    End Class
End Namespace


