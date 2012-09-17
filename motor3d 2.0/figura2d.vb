Imports System.Timers
Imports System.Drawing
Imports System.Math
Imports Motor3D.Espacio3D

Namespace Espacio2D
    Public Class Figura2D
        Inherits Poligono2D

        Protected Transformacion, Giro, Movimiento, Centro, VueltaCentro As Transformacion2D
        Protected Timer As Timer
        Protected Frecuencia As Integer
        Protected mVelocidad As Vector2D
        Protected mVelocidadAngular As Single
        Protected mRozamiento As Double

        Protected mRozamientoActivado As Boolean

        Protected mOBB As OBB2D
        Protected mArbol As ArbolOBB
        Protected mCalcularOBB As Boolean

        Protected mMasa As Double = 1
        Protected mMomentoInercia As Double
        Protected mMomentoAngular As Vector3D
        Protected mMomentoLineal As Vector2D

        Public Const ROZAMIENTO_VELOCIDAD = 100

        Public Shadows Event Modificado(ByRef Sebder As Figura2D)

        Public ReadOnly Property Baricentro() As Punto2D
            Get
                Return Punto2D.Baricentro(MyBase.Vertices)
            End Get
        End Property

        Public Property FrecuenciaActualizacion() As Integer
            Get
                Return Frecuencia
            End Get
            Set(ByVal value As Integer)
                If value > 0 AndAlso value <= 1000 Then
                    Frecuencia = value
                    Timer.Interval = 1000 / value
                End If

            End Set
        End Property

        Public Property Velocidad() As Vector2D
            Get
                Return mVelocidad
            End Get
            Set(ByVal value As Vector2D)
                mVelocidad = value
                Movimiento = Transformacion2D.Traslacion(value)
            End Set
        End Property

        Public Property VelocidadAngular() As Single
            Get
                Return mVelocidadAngular
            End Get
            Set(ByVal value As Single)
                mVelocidadAngular = value
                Giro = Transformacion2D.Rotacion(value)
            End Set
        End Property

        Public Property Rozamiento() As Double
            Get
                Return mRozamiento
            End Get

            Set(ByVal value As Double)
                If value >= 0 Then mRozamiento = value Else mRozamiento = 0
            End Set
        End Property

        Public Property RozamientoActivado() As Boolean
            Get
                Return mRozamientoActivado
            End Get
            Set(ByVal value As Boolean)
                mRozamientoActivado = value
            End Set
        End Property

        Public Property AutoActualizar() As Boolean
            Get
                Return Timer.Enabled
            End Get
            Set(ByVal value As Boolean)
                Timer.Enabled = value
            End Set
        End Property

        Public ReadOnly Property OBB As OBB2D
            Get
                Return mOBB
            End Get
        End Property

        Public ReadOnly Property Arbol As ArbolOBB
            Get
                Return mArbol
            End Get
        End Property

        Public ReadOnly Property Masa As Double
            Get
                Return mMasa
            End Get
        End Property

        Public Property CalcularOBB As Boolean
            Get
                Return mCalcularOBB
            End Get
            Set(ByVal value As Boolean)
                mCalcularOBB = value
            End Set
        End Property

        Public Sub New(ByVal ParamArray Vertices As Punto2D())
            MyBase.New(Vertices)
            Transformacion = New Transformacion2D()
            Movimiento = New Transformacion2D
            Giro = New Transformacion2D
            Centro = New Transformacion2D
            Timer = New Timer()
            Timer.Interval = 10
            Timer.Enabled = False
            Frecuencia = 100
            mVelocidad = New Vector2D()
            mVelocidadAngular = 0
            mRozamiento = 0
            mRozamientoActivado = False
            mOBB = Nothing
            mCalcularOBB = True
            Actualizar()

            AddHandler Timer.Elapsed, AddressOf TimerTick
        End Sub

        Public Sub New(ByVal ParamArray Lados() As Segmento2D)
            MyBase.New(Lados)
            Transformacion = New Transformacion2D()
            Movimiento = New Transformacion2D
            Giro = New Transformacion2D
            Centro = New Transformacion2D
            Timer = New Timer()
            Timer.Interval = 10
            Timer.Enabled = False
            Frecuencia = 100
            mVelocidad = New Vector2D()
            mVelocidadAngular = 0
            mRozamiento = 0
            mRozamientoActivado = False
            mOBB = Nothing
            mCalcularOBB = True
            Actualizar()

            AddHandler Timer.Elapsed, AddressOf TimerTick
        End Sub

        Private Sub CalcularInercia()
            mMomentoInercia = 0
            For i As Integer = 0 To MyBase.Segmentos.Count - 1
                mMomentoInercia += (mMasa / (Segmentos.Count) * ((Segmentos(i).ExtremoInicial.X * Segmentos(i).ExtremoInicial.X) + (Segmentos(i).ExtremoInicial.Y * Segmentos(i).ExtremoInicial.Y)))
            Next

            mMomentoInercia /= (Segmentos.Count)
            mMomentoAngular = New Vector3D
            mMomentoLineal = New Vector2D
        End Sub

        Private Sub TimerTick()
            Actualizar()
        End Sub

        Public Function Actualizar() As Transformacion2D
            Dim Bar As Punto2D = Baricentro
            Dim Punto As Punto2D
            Dim maxX, MaxY, minX, minY As Double

            If mRozamientoActivado Then
                If Abs(mVelocidad.X) - (mRozamiento * ROZAMIENTO_VELOCIDAD) >= 0 Then mVelocidad.X += (-Sign(mVelocidad.X) * mRozamiento * ROZAMIENTO_VELOCIDAD) Else mVelocidad.X = 0
                If Abs(mVelocidad.Y) - (mRozamiento * ROZAMIENTO_VELOCIDAD) >= 0 Then mVelocidad.Y += (-Sign(mVelocidad.Y) * mRozamiento * ROZAMIENTO_VELOCIDAD) Else mVelocidad.Y = 0
                If Abs(mVelocidadAngular) - mRozamiento >= 0 Then mVelocidadAngular += (-Sign(mVelocidadAngular) * mRozamiento) Else mVelocidadAngular = 0

                Movimiento = Transformacion2D.Traslacion(mVelocidad)
                Giro = Transformacion2D.Rotacion(mVelocidadAngular)
            End If

            Centro = Transformacion2D.Traslacion(-Bar.X, -Bar.Y)
            VueltaCentro = Transformacion2D.Traslacion(Bar.X, Bar.Y)

            Transformacion = Centro + Giro + VueltaCentro + Movimiento

            For i As Integer = 0 To MyBase.Segmentos.Count - 1
                Punto = Transformacion * MyBase.Segmentos(i).ExtremoInicial

                If i = 0 Then
                    minX = Punto.X
                    minY = Punto.Y
                    maxX = Punto.X
                    MaxY = Punto.Y

                    MyBase.Segmentos(i).ExtremoInicial = Punto
                Else
                    If i < MyBase.Segmentos.Count - 1 Then
                        MyBase.Segmentos(i - 1).ExtremoFinal = Punto
                        MyBase.Segmentos(i).ExtremoInicial = Punto
                    Else
                        MyBase.Segmentos(i - 1).ExtremoFinal = Punto
                        MyBase.Segmentos(i).ExtremoInicial = Punto
                        MyBase.Segmentos(i).ExtremoFinal = MyBase.Segmentos(0).ExtremoInicial
                    End If
                End If

                If Punto.X < minX Then minX = Punto.X
                If Punto.X > maxX Then maxX = Punto.X
                If Punto.Y < minY Then minY = Punto.Y
                If Punto.Y > MaxY Then MaxY = Punto.Y
            Next

            mAABB = New AABB2D(minX, minY, Abs(maxX - minX), Abs(MaxY - minY))
            If mCalcularOBB Then
                If mOBB Is Nothing Then
                    mOBB = New OBB2D(Vertices)
                    mArbol = New ArbolOBB(Vertices)
                    mMasa = mOBB.Area / 10
                    CalcularInercia()
                Else
                    mOBB.AplicarTransformacion(Transformacion)
                    mArbol.AplicarTransformacion(Transformacion)
                End If
            End If

            RaiseEvent Modificado(Me)

            Return Transformacion
        End Function

        Public Shared Function Colisionan(ByVal F1 As Figura2D, ByVal F2 As Figura2D, ByVal NivelesBSP As Integer, Optional ByVal BSP As Boolean = False) As Boolean
            If AABB2D.Colision(F1.AABB, F2.AABB) Then
                For i As Integer = 0 To F1.Lados.GetUpperBound(0)
                    If AABB2D.Colision(F1.Lados(i).AABB, F2.AABB) Then
                        For j As Integer = 0 To F2.Lados.GetUpperBound(0)
                            If AABB2D.Colision(F1.AABB, F2.Lados(j).AABB) Then
                                If Segmento2D.Colisionan(F1.Lados(i), F2.Lados(j), NivelesBSP, BSP) Then
                                    Return True
                                End If
                            End If
                        Next
                    End If
                Next
            End If

            Return False
        End Function

        Public Sub Desplazar(ByRef Desplazamiento As Vector2D)
            For i As Integer = 0 To Segmentos.Count - 1
                Segmentos(i).ExtremoInicial += New Punto2D(Desplazamiento.X, Desplazamiento.Y)
            Next

            mAABB = New AABB2D(Vertices)
            mOBB.AplicarTransformacion(Transformacion2D.Traslacion(Desplazamiento))
        End Sub

        Public Overrides Sub EstablecerVertices(ParamArray Vertices() As Punto2D)
            MyBase.EstablecerVertices(Vertices)
            mOBB = New OBB2D(Vertices)
            mArbol = New ArbolOBB(Vertices)
        End Sub

        Public Function Fuerza(ByRef Punto As Punto2D) As Vector2D
            Dim vector As Vector2D
            If mVelocidadAngular >= 0 Then
                vector = Not New Vector2D(Punto2D.Baricentro(Vertices), Punto).VectorNormal
            Else
                vector = New Vector2D(Punto2D.Baricentro(Vertices), Punto).VectorNormal
            End If
            Return (mVelocidad) + (vector * mVelocidadAngular)
        End Function

        Public Sub AplicarFuerza(ByRef Fuerza As Vector2D, ByRef Punto As Punto2D, Optional ByVal MagnitudFuerza As Integer = 0)
            Dim Radio As Vector2D = New Vector2D(Punto2D.Baricentro(Vertices), Punto)
            Dim par As Double = Vector2D.ProductoCruzado(Radio, Fuerza)
            mVelocidadAngular -= par / (Radio.CuadradoModulo * mMasa)
            mVelocidad += Fuerza * Abs(Radio * Fuerza) / ((10 ^ MagnitudFuerza) * (mMasa))
        End Sub

        Public Shared Sub ReaccionColision(ByRef F1 As Figura2D, ByRef F2 As Figura2D)
            Dim Datos As DatosColision2D
            Dim Fuerza1, Fuerza2, F As Vector2D

            If OBB2D.Colision(F1.OBB, F2.OBB) Then
                Datos = OBB2D.DatosColision(F1.OBB, F2.OBB)

                Fuerza1 = ((Datos.Direccion) * Datos.Penetracion / (100 + 10 ^ (F1.Velocidad.Modulo + 0.00001)))
                Fuerza2 = ((Datos.Direccion) * Datos.Penetracion / (100 + 10 ^ (F2.Velocidad.Modulo + 0.00001)))

                F1.AplicarFuerza(Fuerza2, Datos.PuntoImpacto)
                F2.AplicarFuerza(Fuerza1, Datos.PuntoImpacto)
            End If
        End Sub
    End Class
End Namespace


