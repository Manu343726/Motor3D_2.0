Imports Motor3D.Algebra
Imports Motor3D.Primitivas3D

Namespace Espacio3D.Transformaciones
    Public Class Rotacion
        Inherits Transformacion3D

        Private mCuaternion As Cuaternion
        Private mRotacion As Single
        Private mSobreOrigen As Boolean
        Private mTraslacion As Cuaternion
        Private mVectorTraslacion As Vector3D

        Public ReadOnly Property Cuaternion As Cuaternion
            Get
                Return mCuaternion
            End Get
        End Property

        Public ReadOnly Property Rotacion As Single
            Get
                Return mRotacion
            End Get
        End Property

        Public ReadOnly Property SobreOrigen As Boolean
            Get
                Return mSobreOrigen
            End Get
        End Property

        Public ReadOnly Property Traslacion As Cuaternion
            Get
                Return mTraslacion
            End Get
        End Property

        Public ReadOnly Property VectorTraslacion As Vector3D
            Get
                Return mVectorTraslacion
            End Get
        End Property

        Public ReadOnly Property Eje As Vector3D
            Get
                Return mCuaternion.Vector
            End Get
        End Property

        Public Sub New(ByVal Cuaternion As Cuaternion)
            mCuaternion = Cuaternion
            mMatriz = Cuaternion.Matriz
            mRotacion = Cuaternion.ObtenerRotacion(Cuaternion)
            mVectorTraslacion = New Vector3D
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
            mSobreOrigen = True
        End Sub

        Public Sub New(ByVal Rotacion As Single, ByVal Eje As EnumEjes)
            Select Case Eje
                Case EnumEjes.EjeX
                    mCuaternion = New Cuaternion(New Vector3D(1, 0, 0), Rotacion)
                Case EnumEjes.EjeY
                    mCuaternion = New Cuaternion(New Vector3D(0, 1, 0), Rotacion)
                Case EnumEjes.EjeZ
                    mCuaternion = New Cuaternion(New Vector3D(0, 0, 1), Rotacion)
            End Select

            mRotacion = Rotacion
            mMatriz = mCuaternion.Matriz
            mVectorTraslacion = New Vector3D
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
            mSobreOrigen = True
        End Sub

        Public Sub New(ByVal Rotacion As Single, ByVal Eje As EnumEjes, ByVal CentroRotacion As Punto3D)
            Select Case Eje
                Case EnumEjes.EjeX
                    mCuaternion = New Cuaternion(New Vector3D(1, 0, 0), Rotacion)
                Case EnumEjes.EjeY
                    mCuaternion = New Cuaternion(New Vector3D(0, 1, 0), Rotacion)
                Case EnumEjes.EjeZ
                    mCuaternion = New Cuaternion(New Vector3D(0, 0, 1), Rotacion)
            End Select

            mVectorTraslacion = New Vector3D(New Punto3D, CentroRotacion)
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)

            'RECUERDA: El producto matricial al encadenar transformaciones es en el orden inverso:
            mMatriz = New Traslacion(mVectorTraslacion).Matriz * mCuaternion.Matriz * New Traslacion(Not mVectorTraslacion).Matriz
            mSobreOrigen = False
            mRotacion = Rotacion
        End Sub

        Public Sub New(ByVal Cabeceo As Single, ByVal Alabeo As Single, ByVal Guiñada As Single)
            mCuaternion = New Cuaternion(Cabeceo, Alabeo, Guiñada)
            mMatriz = mCuaternion.Matriz
            mVectorTraslacion = New Vector3D
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
            mSobreOrigen = True
            mRotacion = 0
        End Sub

        Public Sub New(ByVal Angulos As AngulosEuler)
            mCuaternion = New Cuaternion(Angulos)
            mMatriz = mCuaternion.Matriz
            mVectorTraslacion = New Vector3D
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
            mSobreOrigen = True
            mRotacion = 0
        End Sub

        Public Sub New(ByVal Rotacion As Single, ByVal Eje As Vector3D)
            mCuaternion = New Cuaternion(Eje, Rotacion)
            mMatriz = mCuaternion.Matriz
            mVectorTraslacion = New Vector3D
            mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
            mSobreOrigen = True
            mRotacion = Rotacion
        End Sub

        Public Sub New(ByVal Rotacion As Single, ByVal Eje As Vector3D, ByVal CentroGiro As Punto3D)
            mCuaternion = New Cuaternion(Eje, Rotacion)
            mMatriz = mCuaternion.Matriz
            mVectorTraslacion = New Vector3D(New Punto3D, CentroGiro)
            mMatriz = New Traslacion(mVectorTraslacion).Matriz * mCuaternion.Matriz * New Traslacion(Not mVectorTraslacion).Matriz
            mSobreOrigen = False
            mRotacion = Rotacion
        End Sub

        Public Sub New(ByVal V1 As Vector3D, ByVal V2 As Vector3D)
            Me.New(V1 ^ V2, V1 & V2)
        End Sub

        Public Sub New(ByVal V1 As Vector3D, ByVal V2 As Vector3D, ByVal CentroGiro As Punto3D)
            Me.New(V1 ^ V2, V1 & V2, CentroGiro)
        End Sub

        Public Sub New(ByVal Rotacion As Single, ByVal Eje As Recta3D)
            mCuaternion = New Cuaternion(Eje.VectorDirector, Rotacion)

            If Eje.Pertenece(New Punto3D) Then
                mVectorTraslacion = New Vector3D
                mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)
                mMatriz = mCuaternion.Matriz
                mSobreOrigen = True
            Else
                mVectorTraslacion = New Vector3D(New Punto3D, Recta3D.Proyeccion(New Punto3D, Eje))

                mTraslacion = New Cuaternion(mVectorTraslacion, 0, False)

                'RECUERDA: El producto matricial al encadenar transformaciones es en el orden inverso:
                mMatriz = New Traslacion(mVectorTraslacion).Matriz * mCuaternion.Matriz * New Traslacion(Not mVectorTraslacion).Matriz
                mSobreOrigen = False
            End If

            mRotacion = Rotacion
        End Sub

#Region "AplicarTransformacion (DEBUGGING)"
        Public Overloads Shared Function EncadenarTransformaciones(ByVal E1 As Rotacion, ByVal E2 As Rotacion) As Rotacion
            Return New Rotacion(E1.Cuaternion * E2.Cuaternion)
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Punto As Punto3D, ByVal Rotacion As Rotacion) As Punto3D
            If Rotacion.SobreOrigen Then
                Return Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(Punto, False) * Not Rotacion.Cuaternion)
            Else
                Return Transformacion3D.AplicarTransformacion(Punto, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vector As Vector3D, ByVal Rotacion As Rotacion) As Vector3D
            If Rotacion.SobreOrigen Then
                Return Cuaternion.ObtenerVector(Rotacion.Cuaternion * New Cuaternion(Vector, 0, False) * Not Rotacion.Cuaternion)
            Else
                Return Transformacion3D.AplicarTransformacion(Vector, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Vertice As Vertice, ByVal Rotacion As Rotacion) As Vertice
            If Rotacion.SobreOrigen Then
                Return New Vertice(Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(Vertice.CoodenadasSUR, False) * Not Rotacion.Cuaternion))
            Else
                Return Transformacion3D.AplicarTransformacion(Vertice, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Recta As Recta3D, ByVal Rotacion As Rotacion) As Recta3D
            If Rotacion.SobreOrigen Then
                Return New Recta3D(Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(Recta.PuntoInicial, False) * Not Rotacion.Cuaternion), Cuaternion.ObtenerVector(Rotacion.Cuaternion * New Cuaternion(Recta.VectorDirector, 0, False) * Not Rotacion.Cuaternion))
            Else
                Return Transformacion3D.AplicarTransformacion(Recta, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Plano As Plano3D, ByVal Rotacion As Rotacion) As Plano3D
            Dim P As Punto3D = Plano.ObtenerPunto(0, 0)
            If Rotacion.SobreOrigen Then
                Return New Plano3D(Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(P, False) * Not Rotacion.Cuaternion), Cuaternion.ObtenerVector(Rotacion.Cuaternion * New Cuaternion(Plano.VectorNormal, 0, False) * Not Rotacion.Cuaternion))
            Else
                Transformacion3D.AplicarTransformacion(Plano, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal AABB As AABB3D, ByVal Rotacion As Rotacion) As AABB3D
            If Rotacion.SobreOrigen Then
                Return New AABB3D(Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(AABB.Posicion, False) * Not Rotacion.Cuaternion), AABB.Dimensiones)
            Else
                Return Transformacion3D.AplicarTransformacion(AABB, Rotacion)
            End If
        End Function

        Public Overloads Shared Function AplicarTransformacion(ByVal Segmento As Segmento3D, ByVal Rotacion As Rotacion) As Segmento3D
            If Rotacion.SobreOrigen Then
                Return New Segmento3D(Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(Segmento.ExtremoInicial, False) * Not Rotacion.Cuaternion), Cuaternion.ObtenerPunto(Rotacion.Cuaternion * New Cuaternion(Segmento.ExtremoFinal, False) * Not Rotacion.Cuaternion))
            Else
                Return Transformacion3D.AplicarTransformacion(Segmento, Rotacion)
            End If
        End Function
#End Region

        Public Overloads Shared Function AplicarTransformacion(ByVal Poliedro As Poliedro, ByVal Rotacion As Rotacion) As Poliedro
            Poliedro.AplicarTransformacion(Rotacion)
            Return Poliedro
        End Function

        Public Overloads Shared Operator +(ByVal E1 As Rotacion, ByVal E2 As Rotacion) As Rotacion
            Return EncadenarTransformaciones(E1, E2)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Punto As Punto3D) As Punto3D
            Return AplicarTransformacion(Punto, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Punto As Punto3D, ByVal Rotacion As Rotacion) As Punto3D
            Return AplicarTransformacion(Punto, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Vector As Vector3D) As Vector3D
            Return AplicarTransformacion(Vector, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Vector As Vector3D, ByVal Rotacion As Rotacion) As Vector3D
            Return AplicarTransformacion(Vector, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Vertice As Vertice) As Vertice
            Return AplicarTransformacion(Vertice, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Vertice As Vertice, ByVal Rotacion As Rotacion) As Vertice
            Return AplicarTransformacion(Vertice, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Recta As Recta3D) As Recta3D
            Return AplicarTransformacion(Recta, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Recta As Recta3D, ByVal Rotacion As Rotacion) As Recta3D
            Return AplicarTransformacion(Recta, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Plano As Plano3D) As Plano3D
            Return AplicarTransformacion(Plano, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Plano As Plano3D, ByVal Rotacion As Rotacion) As Plano3D
            Return AplicarTransformacion(Plano, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal AABB As AABB3D) As AABB3D
            Return AplicarTransformacion(AABB, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal AABB As AABB3D, ByVal Rotacion As Rotacion) As AABB3D
            Return AplicarTransformacion(AABB, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Segmento As Segmento3D) As Segmento3D
            Return AplicarTransformacion(Segmento, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Segmento As Segmento3D, ByVal Rotacion As Rotacion) As Segmento3D
            Return AplicarTransformacion(Segmento, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Rotacion As Rotacion, ByVal Poliedro As Poliedro) As Poliedro
            Return AplicarTransformacion(Poliedro, Rotacion)
        End Operator

        Public Overloads Shared Operator *(ByVal Poliedro As Poliedro, ByVal Rotacion As Rotacion) As Poliedro
            Return AplicarTransformacion(Poliedro, Rotacion)
        End Operator
    End Class
End Namespace

