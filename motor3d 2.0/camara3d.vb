Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Espacio2D
Imports Motor3D.Algebra

Namespace Escena
    Public Class Camara3D
        Inherits ObjetoEscena

        Private mTransformacion As Transformacion3D
        Private mInversa As Transformacion3D
        Private mPosicion As Punto3D
        Private mPuntodeMira As Punto3D
        Private mVectorDireccion As Vector3D
        Private mProyeccion As Proyeccion
        Private mFrustum As AABB3D
        Private mResolucionPantalla As Punto2D
        Private mDistancia As Double
        Private mRelacionAspecto As Punto2D
        Private mPantalla As AABB2D

        Public Shadows Event Modificado(ByRef Sebder As Camara3D)

        Public ReadOnly Property TransformacionSRCtoSUR() As Transformacion3D
            Get
                Return mTransformacion
            End Get
        End Property

        Public ReadOnly Property TransformacionSURtoSRC() As Transformacion3D
            Get
                Return mInversa
            End Get
        End Property

        Public Property Posicion() As Punto3D
            Get
                Return mPosicion
            End Get

            Set(ByVal value As Punto3D)
                If value <> mPosicion Then
                    mTransformacion += New Traslacion(mPosicion, value)
                    RecalcularDatos()
                End If
            End Set
        End Property

        Public ReadOnly Property VectorDireccion() As Vector3D
            Get
                Return mVectorDireccion
            End Get
        End Property

        Public ReadOnly Property PuntoDeMira() As Punto3D
            Get
                Return mPuntodeMira
            End Get
        End Property

        Public ReadOnly Property Frustum() As AABB3D
            Get
                Return mFrustum
            End Get
        End Property

        Public ReadOnly Property Pantalla() As AABB2D
            Get
                Return mPantalla
            End Get
        End Property

        Public Property Distancia() As Double
            Get
                Return mDistancia
            End Get
            Set(ByVal value As Double)
                If value > 0 AndAlso value <> mDistancia Then
                    mDistancia = value
                    mFrustum = New AABB3D(-(mFrustum.Ancho / 2), -(mFrustum.Largo / 2), mDistancia, mFrustum.Ancho, mFrustum.Largo, mFrustum.Alto)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property ResolucionPantalla() As Punto2D
            Get
                Return mResolucionPantalla
            End Get
            Set(ByVal value As Punto2D)
                If mResolucionPantalla <> value Then
                    mResolucionPantalla = value
                    mRelacionAspecto = New Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo)
                    mPantalla = New AABB2D(-ResolucionPantalla.X / 2, -ResolucionPantalla.Y / 2, ResolucionPantalla.X, ResolucionPantalla.Y)
                End If
            End Set
        End Property

        Public ReadOnly Property RepresentacionFrustum() As Poligono2D()
            Get
                Dim Retorno(5) As Poligono2D
                Dim Vertices(7) As Punto2D

                Vertices(0) = Proyeccion(Frustum.Posicion)
                Vertices(1) = Proyeccion(New Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top, Frustum.Up))
                Vertices(2) = Proyeccion(New Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top + Frustum.Largo, Frustum.Up))
                Vertices(3) = Proyeccion(New Punto3D(Frustum.Left, Frustum.Top + Frustum.Largo, Frustum.Up))
                Vertices(4) = Proyeccion(New Punto3D(Frustum.Left, Frustum.Top, Frustum.Up + Frustum.Alto))
                Vertices(5) = Proyeccion(New Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top, Frustum.Up + Frustum.Alto))
                Vertices(6) = Proyeccion(New Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top + Frustum.Largo, Frustum.Up + Frustum.Alto))
                Vertices(7) = Proyeccion(New Punto3D(Frustum.Left, Frustum.Top + Frustum.Largo, Frustum.Up + Frustum.Alto))


                Retorno(0) = New Poligono2D(Vertices(3), Vertices(2), Vertices(1), Vertices(0))
                Retorno(1) = New Poligono2D(Vertices(4), Vertices(5), Vertices(6), Vertices(7))
                Retorno(2) = New Poligono2D(Vertices(7), Vertices(6), Vertices(2), Vertices(3))
                Retorno(3) = New Poligono2D(Vertices(4), Vertices(7), Vertices(3), Vertices(0))
                Retorno(4) = New Poligono2D(Vertices(5), Vertices(4), Vertices(0), Vertices(1))
                Retorno(5) = New Poligono2D(Vertices(6), Vertices(5), Vertices(1), Vertices(2))

                Return Retorno
            End Get
        End Property

        Public Sub EstablecerDimensionesFrustum(ByVal Ancho As Double, ByVal Largo As Double, ByVal Alto As Double)
            mFrustum = New AABB3D(-(Ancho / 2), -(Alto / 2), mDistancia, Ancho, Largo, Alto)
            mRelacionAspecto = New Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub EstablecerAnchoFrustum(ByVal Ancho As Double)
            mFrustum = New AABB3D(-(Ancho / 2), -mFrustum.Largo / 2, mDistancia, Ancho, mFrustum.Largo, mFrustum.Alto)
            mRelacionAspecto = New Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub EstablecerLargoFrustum(ByVal Largo As Double)
            mFrustum = New AABB3D(-(mFrustum.Ancho / 2), -Largo / 2, mDistancia, mFrustum.Ancho, Largo, mFrustum.Alto)
            mRelacionAspecto = New Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub EstablecerAltoFrustum(ByVal Alto As Double)
            mFrustum = New AABB3D(-(mFrustum.Ancho / 2), -mFrustum.Largo / 2, mDistancia, mFrustum.Ancho, mFrustum.Largo, Alto)
            RaiseEvent Modificado(Me)
        End Sub

        Public Function Proyeccion(ByVal Punto As Punto3D, Optional ByVal EstaTransformado As Boolean = False) As Punto2D
            Dim P As Punto3D
            Dim k As Double

            If EstaTransformado Then
                k = mDistancia / Punto.Z
                Return New Punto2D(1 * (Punto.X * k), 1 * (Punto.Y * k))
                'Return New Punto2D(Punto.X, Punto.Y)
            Else
                P = mInversa * Punto
                k = mDistancia / P.Z
                Return New Punto2D(1 * (P.X * k), 1 * (P.Y * k))
                'Return New Punto2D(P.X, P.Y)
            End If
        End Function

        Private Sub RecalcularDatos()
            mInversa = New Transformacion3D(Matriz.CalculoInversa(mTransformacion.Matriz))
            mPosicion = mTransformacion * New Punto3D
            mPuntodeMira = mTransformacion * New Punto3D(0, 0, 1)
            mVectorDireccion = New Vector3D(mPosicion, mPuntodeMira)
        End Sub

        Public Sub TrasladarSobreSUR(ByVal Traslacion As Vector3D)
            mTransformacion = mTransformacion + New Traslacion(Traslacion)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub TrasladarSobreSUR(ByVal Destino As Punto3D)
            mTransformacion += New Traslacion(mPosicion, Destino)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub TrasladarSobreSRC(ByVal Traslacion As Vector3D)
            mTransformacion = New Traslacion(Traslacion) + mTransformacion
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarSobreSUR(ByVal Eje As EnumEjes, ByVal Rotacion As Single)
            mTransformacion += New Rotacion(Rotacion, Eje)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarFijoSobreSUR(ByVal Eje As EnumEjes, ByVal Rotacion As Single)
            mTransformacion += New Rotacion(Rotacion, Eje, mPosicion)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarSobreSRC(ByVal Eje As EnumEjes, ByVal Rotacion As Single)
            mTransformacion = New Rotacion(Rotacion, Eje) + mTransformacion
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarSobreSUR(ByVal Eje As Recta3D, ByVal Rotacion As Single)
            mTransformacion += New Rotacion(Rotacion, Eje)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarSobreSUR(ByVal Eje As Vector3D, ByVal Rotacion As Single)
            mTransformacion += New Rotacion(Rotacion, Eje)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarFijoSobreSUR(ByVal Eje As Vector3D, ByVal Rotacion As Single)
            mTransformacion += New Rotacion(Rotacion, Eje, mPosicion)
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RotarSobreSRC(ByVal Eje As Vector3D, ByVal Rotacion As Single)
            mTransformacion = New Rotacion(Rotacion, Eje) + mTransformacion
            RecalcularDatos()
            RaiseEvent Modificado(Me)
        End Sub

        Public Function EsVisible(ByVal Punto As Punto3D) As Boolean
            Return mFrustum.Pertenece(Punto)
        End Function

        Public Sub New()
            mTransformacion = New Transformacion3D
            mInversa = New Transformacion3D
            mPosicion = New Punto3D
            mPuntodeMira = New Punto3D(0, 0, 1)
            mVectorDireccion = New Vector3D(0, 0, 1)
            mDistancia = 1000
            mFrustum = New AABB3D(-50000, -50000, 1000, 100000, 100000, 100000)
            mResolucionPantalla = New Punto2D(800, 600)
            mRelacionAspecto = New Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo)
            mPantalla = New AABB2D(-ResolucionPantalla.X / 2, -ResolucionPantalla.Y / 2, ResolucionPantalla.X, ResolucionPantalla.Y)
        End Sub

        Public Overrides Function ToString() As String
            Return "{Camara3D. Posicion=" & mPosicion.ToString & " , Direccion=" & mVectorDireccion.ToString & "}"
        End Function

        Public Shared Operator =(ByVal C1 As Camara3D, ByVal C2 As Camara3D) As Boolean
            Return C1.Equals(C2)
        End Operator

        Public Shared Operator <>(ByVal C1 As Camara3D, ByVal C2 As Camara3D) As Boolean
            Return Not (C1 = C2)
        End Operator
    End Class
End Namespace

