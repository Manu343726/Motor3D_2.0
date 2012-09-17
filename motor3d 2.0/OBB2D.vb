Imports Motor3D.Algebra
Imports System.Math

Namespace Espacio2D
    Public Class OBB2D
        Private mCentro As Punto2D
        Private mEjeX As Vector2D
        Private mEjeY As Vector2D

        Private mAABB As AABB2D

        Private l As Double
        Private l_2 As Double

        Private supiz, supder, infder, infiz As Punto2D
        Private dnorte, dsur, deste, doeste As Double

        Private mMundoALocal As Transformacion2D
        Private mLocalAMundo As Transformacion2D

        Public ReadOnly Property Centro As Punto2D
            Get
                Return mCentro
            End Get
        End Property

        Public ReadOnly Property EjeX As Vector2D
            Get
                Return mEjeX
            End Get
        End Property

        Public ReadOnly Property EjeY As Vector2D
            Get
                Return mEjeY
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

        Public ReadOnly Property Esquinas(ByVal Indice As Integer) As Punto2D
            Get
                Select Case Indice
                    Case 0
                        Return supiz
                    Case 1
                        Return supder
                    Case 2
                        Return infder
                    Case 3
                        Return infiz
                End Select
            End Get
        End Property

        Public ReadOnly Property EsquinaSuperiorIzquierda As Punto2D
            Get
                Return supiz
            End Get
        End Property

        Public ReadOnly Property EsquinaSuperiorDerecha As Punto2D
            Get
                Return supder
            End Get
        End Property

        Public ReadOnly Property EsquinaInferiorDerecha As Punto2D
            Get
                Return infder
            End Get
        End Property

        Public ReadOnly Property EsquinaInferiorIzquierda As Punto2D
            Get
                Return infiz
            End Get
        End Property

        Public ReadOnly Property Ancho As Double
            Get
                Return doeste + deste
            End Get
        End Property

        Public ReadOnly Property Largo As Double
            Get
                Return dnorte + dsur
            End Get
        End Property

        Public ReadOnly Property LongitudEjeX As Double
            Get
                Return deste
            End Get
        End Property

        Public ReadOnly Property LongitudEjeY As Double
            Get
                Return dsur
            End Get
        End Property

        Public ReadOnly Property MundoALocal As Transformacion2D
            Get
                Return mMundoALocal
            End Get
        End Property

        Public ReadOnly Property LocalAMundo As Transformacion2D
            Get
                Return mLocalAMundo
            End Get
        End Property

        Public ReadOnly Property AABB As AABB2D
            Get
                Return mAABB
            End Get
        End Property

        Public ReadOnly Property Area As Double
            Get
                Return mAABB.Area
            End Get
        End Property

        Public Sub New(ByVal Indices() As Integer, ByVal ParamArray Puntos() As Punto2D)
            Dim norte, sur, este, oeste, p As Punto2D
            Dim aux As Vector2D

            mCentro = Punto2D.Baricentro(Indices, Puntos)
            mEjeX = New Vector2D

            norte = New Punto2D(0, Double.MaxValue)
            sur = New Punto2D(0, Double.MinValue)
            este = New Punto2D(Double.MinValue, 0)
            oeste = New Punto2D(Double.MaxValue, 0)

            For i As Integer = 0 To Indices.GetUpperBound(0)
                aux = New Vector2D(mCentro, Puntos(Indices(i)))

                If aux * mEjeX > 0 Then
                    mEjeX += aux
                Else
                    mEjeX -= aux
                End If
            Next
            mEjeX.Normalizar()

            mLocalAMundo = Transformacion2D.EncadenarTransformaciones(Transformacion2D.Rotacion(-mEjeX.Y, mEjeX.X), Transformacion2D.Traslacion(mCentro.X, mCentro.Y))
            mMundoALocal = Transformacion2D.TransformacionInversa(mLocalAMundo)

            For i As Integer = 0 To Indices.GetUpperBound(0)
                p = Puntos(Indices(i)) * mMundoALocal

                If p.X > este.X Then
                    este = p
                End If
                If p.X < oeste.X Then
                    oeste = p
                End If
                If p.Y > sur.Y Then
                    sur = p
                End If
                If p.Y < norte.Y Then
                    norte = p
                End If
            Next

            dnorte = Abs(norte.Y)
            dsur = Abs(sur.Y)
            deste = Abs(este.X)
            doeste = Abs(oeste.X)

            mEjeY = New Vector2D(-mEjeX.Y, mEjeX.X) 'EjeY es perpendicular a EjeX, y con modulo dnorte o dsu
            RecalcularEsquinas()

            mAABB = New AABB2D(-doeste, -dnorte, doeste + deste, dnorte + dsur)
        End Sub

        Public Sub New(ByVal ParamArray Puntos() As Punto2D)
            Dim norte, sur, este, oeste, p As Punto2D
            Dim aux As Vector2D

            mCentro = Punto2D.Baricentro(Puntos)
            mEjeX = New Vector2D

            norte = New Punto2D(0, Double.MaxValue)
            sur = New Punto2D(0, Double.MinValue)
            este = New Punto2D(Double.MinValue, 0)
            oeste = New Punto2D(Double.MaxValue, 0)

            For i As Integer = 0 To Puntos.GetUpperBound(0)
                aux = New Vector2D(mCentro, Puntos(i))

                If aux * mEjeX > 0 Then
                    mEjeX += aux
                Else
                    mEjeX -= aux
                End If
            Next
            mEjeX.Normalizar()

            mLocalAMundo = Transformacion2D.EncadenarTransformaciones(Transformacion2D.Rotacion(-mEjeX.Y, mEjeX.X), Transformacion2D.Traslacion(mCentro.X, mCentro.Y))
            mMundoALocal = Transformacion2D.TransformacionInversa(mLocalAMundo)

            For i As Integer = 0 To Puntos.GetUpperBound(0)
                p = Puntos(i) * mMundoALocal

                If p.X > este.X Then
                    este = p
                End If
                If p.X < oeste.X Then
                    oeste = p
                End If
                If p.Y > sur.Y Then
                    sur = p
                End If
                If p.Y < norte.Y Then
                    norte = p
                End If
            Next

            dnorte = Abs(norte.Y)
            dsur = Abs(sur.Y)
            deste = Abs(este.X)
            doeste = Abs(oeste.X)

            mEjeY = New Vector2D(-mEjeX.Y, mEjeX.X) 'EjeY es perpendicular a EjeX, y con modulo dnorte o dsu
            RecalcularEsquinas()

            mAABB = New AABB2D(-doeste, -dnorte, doeste + deste, dnorte + dsur)
        End Sub

        Private Sub RecalcularEsquinas()
            Dim MatrizEsquina As New Matriz(3, 1)

            MatrizEsquina(0, 0) = -doeste
            MatrizEsquina(1, 0) = -dnorte
            MatrizEsquina(2, 0) = 1
            supiz = New Punto2D(Matriz.Producto(mLocalAMundo.Matriz, MatrizEsquina))
            MatrizEsquina(0, 0) = deste
            'MatrizEsquina(1, 0) = -dnorte
            'MatrizEsquina(2, 0) = 1
            supder = New Punto2D(Matriz.Producto(mLocalAMundo.Matriz, MatrizEsquina))
            'MatrizEsquina(0, 0) = deste
            MatrizEsquina(1, 0) = dsur
            'MatrizEsquina(2, 0) = 1
            infder = New Punto2D(Matriz.Producto(mLocalAMundo.Matriz, MatrizEsquina))
            MatrizEsquina(0, 0) = -doeste
            'MatrizEsquina(1, 0) = -dsur
            'MatrizEsquina(2, 0) = 1
            infiz = New Punto2D(Matriz.Producto(mLocalAMundo.Matriz, MatrizEsquina))
        End Sub

        Public Sub AplicarTransformacion(ByVal Transformacion As Transformacion2D)
            mLocalAMundo = Transformacion2D.EncadenarTransformaciones(mLocalAMundo, Transformacion)
            mMundoALocal = Transformacion2D.TransformacionInversa(mLocalAMundo)
            Transformacion.AplicarTransformacion(mCentro)

            RecalcularEsquinas()
            mEjeX = New Vector2D((supder.X + infder.X - mCentro.X - mCentro.X) / 2, (supder.Y + infder.Y - mCentro.Y - mCentro.Y) / 2)
            mEjeX.Normalizar()
            mEjeY = New Vector2D(-mEjeX.Y, mEjeX.X)
        End Sub

        Public Function Pertenece(ByRef Punto As Punto2D) As Boolean
            Return mAABB.Pertenece(Matriz.Producto(mMundoALocal.Matriz, Punto.Matriz))
        End Function

        Public Shared Function Colision(ByRef O1 As OBB2D, ByRef O2 As OBB2D) As Boolean
            Dim A1, A2 As AABB2D

            If AABB2D.Colision(O1.AABB, O2.AABB) Then
                A2 = New AABB2D(O1.MundoALocal.Aplicar(O2.EsquinaSuperiorIzquierda), _
                                O1.MundoALocal.Aplicar(O2.EsquinaSuperiorDerecha), _
                                O1.MundoALocal.Aplicar(O2.EsquinaInferiorDerecha), _
                                O1.MundoALocal.Aplicar(O2.EsquinaInferiorIzquierda))

                If AABB2D.Colision(O1.AABB, A2) Then
                    A1 = New AABB2D(O2.MundoALocal.Aplicar(O1.EsquinaSuperiorIzquierda), _
                    O2.MundoALocal.Aplicar(O1.EsquinaSuperiorDerecha), _
                    O2.MundoALocal.Aplicar(O1.EsquinaInferiorDerecha), _
                    O2.MundoALocal.Aplicar(O1.EsquinaInferiorIzquierda))

                    Return AABB2D.Colision(O2.AABB, A1)
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Function Union(ByRef O1 As OBB2D, ByRef O2 As OBB2D) As OBB2D
            Dim V1(3), V2(3) As Punto2D

            V1 = Transformacion2D.AplicarTransformacion(O1.Esquinas, O1.LocalAMundo)
            V2 = Transformacion2D.AplicarTransformacion(O2.Esquinas, O2.LocalAMundo)

            Return New OBB2D(V1.Concat(V2).ToArray)
        End Function

        Public Shared Function PuntosPenetracion(ByRef O1 As OBB2D, ByRef O2 As OBB2D) As Punto2D()
            Dim Retorno As New List(Of Punto2D)

            For i As Integer = 0 To 3
                If O2.Pertenece(O1.Esquinas(i)) Then
                    Retorno.Add(O1.Esquinas(i))
                End If
            Next

            Return Retorno.ToArray
        End Function

        Public Shared Function PuntoImpacto(ByRef O1 As OBB2D, ByRef O2 As OBB2D) As Punto2D
            Return Punto2D.Baricentro(PuntosPenetracion(O1, O2).Concat(PuntosPenetracion(O2, O1)).ToArray)
        End Function

        Public Shared Function DatosColision(ByRef O1 As OBB2D, ByRef O2 As OBB2D) As DatosColision2D
            Dim A1, A2 As AABB2D
            Dim D1, D2 As DatosColision2D
            Dim Retorno As DatosColision2D
            Dim PS1() As Punto2D = PuntosPenetracion(O1, O2)
            Dim PS2() As Punto2D = PuntosPenetracion(O2, O1)
            Dim C1 As Punto2D = Punto2D.Baricentro(PS1)
            Dim C2 As Punto2D = Punto2D.Baricentro(PS2)

            If True Then C1 = O1.LocalAMundo.Aplicar(O1.AABB.Centro)
            If True Then C2 = O2.LocalAMundo.Aplicar(O2.AABB.Centro)

            A2 = New AABB2D(O1.MundoALocal.Aplicar(O2.EsquinaSuperiorIzquierda), _
                    O1.MundoALocal.Aplicar(O2.EsquinaSuperiorDerecha), _
                    O1.MundoALocal.Aplicar(O2.EsquinaInferiorDerecha), _
                    O1.MundoALocal.Aplicar(O2.EsquinaInferiorIzquierda))
            A1 = New AABB2D(O2.MundoALocal.Aplicar(O1.EsquinaSuperiorIzquierda), _
                    O2.MundoALocal.Aplicar(O1.EsquinaSuperiorDerecha), _
                    O2.MundoALocal.Aplicar(O1.EsquinaInferiorDerecha), _
                    O2.MundoALocal.Aplicar(O1.EsquinaInferiorIzquierda))

            D1 = O1.LocalAMundo.Aplicar(AABB2D.DatosColision(O1.AABB, A2))
            D2 = O2.LocalAMundo.Aplicar(AABB2D.DatosColision(A1, O2.AABB))
            Retorno = (D1 + D2) / 2

            Retorno = New DatosColision2D(PuntoImpacto(O1, O2), New Vector2D(C1, C2), Retorno.Interseccion)

            Return Retorno
        End Function
    End Class
End Namespace

