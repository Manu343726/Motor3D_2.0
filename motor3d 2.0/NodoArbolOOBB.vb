Imports System.Math

Namespace Espacio2D
    Public Class NodoArbolOBB
        Private mOBB As OBB2D
        Private mPadre As NodoArbolOBB
        Private mHijoA, mHijoB As NodoArbolOBB

        Private Recta As Recta2D

        Public Const MARGEN_POSICION_RELATIVA As Double = 0.001
        Private Margen As Double

        Public ReadOnly Property OBB As OBB2D
            Get
                Return mOBB
            End Get
        End Property

        Public ReadOnly Property Padre As NodoArbolOBB
            Get
                Return mPadre
            End Get
        End Property

        Public ReadOnly Property HijoA As NodoArbolOBB
            Get
                Return mHijoA
            End Get
        End Property

        Public ReadOnly Property HijoB As NodoArbolOBB
            Get
                Return mHijoB
            End Get
        End Property

        Public ReadOnly Property EsRaiz As Boolean
            Get
                Return mPadre Is Nothing
            End Get
        End Property

        Public ReadOnly Property EsHoja As Boolean
            Get
                Return mHijoA Is Nothing
            End Get
        End Property

        'Constructor del nodo padre:
        Public Sub New(ByVal ParamArray Puntos() As Punto2D)
            mPadre = Nothing
            mOBB = New OBB2D(Puntos)
            If mOBB.LongitudEjeX < mOBB.LongitudEjeY Then
                Recta = New Recta2D(mOBB.Centro, mOBB.EjeX)
                Margen = MARGEN_POSICION_RELATIVA * mOBB.LongitudEjeY
            Else
                Recta = New Recta2D(mOBB.Centro, mOBB.EjeY)
                Margen = MARGEN_POSICION_RELATIVA * mOBB.LongitudEjeX
            End If

            GenerarHijos(Puntos)
            'NOTA: El vertice final del HijoA as el mismo que el vertice inicial del HijoB, se repiten para prevenir desbordamiento
        End Sub

        Public Sub New(ByRef Padre As NodoArbolOBB, ByVal Indices() As Integer, ByVal ParamArray Puntos() As Punto2D)
            mPadre = Padre
            mOBB = New OBB2D(Indices, Puntos)
            If mOBB.LongitudEjeX < mOBB.LongitudEjeY Then
                Recta = New Recta2D(mOBB.Centro, mOBB.EjeX)
                Margen = MARGEN_POSICION_RELATIVA * mOBB.LongitudEjeY
            Else
                Recta = New Recta2D(mOBB.Centro, mOBB.EjeY)
                Margen = MARGEN_POSICION_RELATIVA * mOBB.LongitudEjeX
            End If

            GenerarHijos(Indices, Puntos)
        End Sub

        Private Sub GenerarHijos(ByVal ParamArray Puntos() As Punto2D)
            If Puntos.GetUpperBound(0) > 3 Then
                Dim ListA As New List(Of Integer)
                Dim ListB As New List(Of Integer)
                Dim PosicionRelativa As Double

                For i As Integer = 0 To Puntos.GetUpperBound(0)
                    PosicionRelativa = Recta.PosicionRelativa(Puntos(i))

                    If abs(PosicionRelativa) <= Margen Then
                        ListA.Add(i)
                        ListB.Add(i)
                    Else
                        If PosicionRelativa > 0 Then
                            ListA.Add(i)
                        Else
                            ListB.Add(i)
                        End If

                    End If
                Next

                MejorarListas(ListA, ListB, Puntos.GetUpperBound(0) + 1)

                mHijoA = New NodoArbolOBB(Me, ListA.ToArray, Puntos)
                mHijoB = New NodoArbolOBB(Me, ListB.ToArray, Puntos)
            Else
                mHijoA = Nothing
                mHijoB = Nothing
            End If
        End Sub

        Private Sub GenerarHijos(ByVal Indices() As Integer, ByVal ParamArray Puntos() As Punto2D)
            If Indices.GetUpperBound(0) > 3 Then
                Dim ListA As New List(Of Integer)
                Dim ListB As New List(Of Integer)
                Dim PosicionRelativa As Double

                For i As Integer = 0 To Indices.GetUpperBound(0)
                    PosicionRelativa = Recta.PosicionRelativa(Puntos(Indices(i)))

                    If Abs(PosicionRelativa) <= Margen Then
                        ListA.Add(i)
                        ListB.Add(i)
                    Else
                        If PosicionRelativa > 0 Then
                            ListA.Add(i)
                        Else
                            ListB.Add(i)
                        End If
                    End If
                Next

                MejorarListas(ListA, ListB, Puntos.GetUpperBound(0) + 1)

                mHijoA = New NodoArbolOBB(Me, ListA.ToArray, Puntos)
                mHijoB = New NodoArbolOBB(Me, ListB.ToArray, Puntos)
            Else
                mHijoA = Nothing
                mHijoB = Nothing
            End If
        End Sub

        Public Sub MejorarListas(ByRef ListA As List(Of Integer), ByRef ListB As List(Of Integer), ByVal TotalPuntos As Integer)
            If ListA.Count < 3 Then
                If ListA(ListA.Count - 1) < (TotalPuntos - 1) Then
                    ListA.Add(ListA(ListA.Count - 1) + 1)
                Else
                    ListA.Add(0)
                End If
            Else
                If ListB.Count < 3 Then
                    If ListB(ListB.Count - 1) < (TotalPuntos - 1) Then
                        ListB.Add(ListB(ListB.Count - 1) + 1)
                    Else
                        ListB.Add(0)
                    End If
                End If
            End If
        End Sub

        Public Function Pertenece(ByRef Punto As Punto2D) As Boolean
            If mOBB.Pertenece(Punto) Then
                If mHijoA Is Nothing Then
                    Return True
                Else
                    If Recta.PosicionRelativa(Punto) > 0 Then
                        Return mHijoA.Pertenece(Punto)
                    Else
                        Return mHijoB.Pertenece(Punto)
                    End If
                End If
            Else
                Return False
            End If
        End Function

        Public Sub AplicarTransformacion(ByRef Transformacion As Transformacion2D)
            mOBB.AplicarTransformacion(Transformacion)

            If Not mHijoA Is Nothing Then
                mHijoA.AplicarTransformacion(Transformacion)
                mHijoB.AplicarTransformacion(Transformacion)
            End If
        End Sub
    End Class
End Namespace

