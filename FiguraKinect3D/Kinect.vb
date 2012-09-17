Imports Microsoft.Kinect
Imports System.Runtime.InteropServices
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio2D
Imports Motor3D.Escena
Imports System.Drawing.Imaging

Public Class ExcepcionInicializacionKinect
    Inherits Exception

    Public Sub New()
        MyBase.New("No se ha podido configurar el adaptador." & vbNewLine & "Asegurese de que su dispositivo Kinect esté correctamente conectado.")
    End Sub
End Class

Public Class Hueso
    Private mExtremoInicial As Punto3D
    Private mExtremoFinal As Punto3D
    Private mPuntoMedio As Punto3D
    Private mSegmento As Segmento3D

    Public ReadOnly Property ExtremoInicial As Punto3D
        Get
            Return mExtremoInicial
        End Get
    End Property

    Public ReadOnly Property ExtremoFinal As Punto3D
        Get
            Return mExtremoFinal
        End Get
    End Property

    Public ReadOnly Property PuntoMedio As Punto3D
        Get
            Return mPuntoMedio
        End Get
    End Property

    Public ReadOnly Property Segmento As Segmento3D
        Get
            Return mSegmento
        End Get
    End Property

    Public Sub New(ByRef ExtremoInicial As Punto3D, ByRef ExtremoFinal As Punto3D)
        mExtremoInicial = ExtremoInicial
        mExtremoFinal = ExtremoFinal
        mPuntoMedio = New Punto3D((mExtremoInicial.X + mExtremoFinal.X) / 2, (mExtremoInicial.Y + mExtremoFinal.Y) / 2, (mExtremoInicial.Z + mExtremoFinal.Z) / 2)
        mSegmento = New Segmento3D(mExtremoInicial, mExtremoFinal)
    End Sub

    Public Sub Actualizar(ByRef Sender As Punto3D)
        mPuntoMedio = New Punto3D((mExtremoInicial.X + mExtremoFinal.X) / 2, (mExtremoInicial.Y + mExtremoFinal.Y) / 2, (mExtremoInicial.Z + mExtremoFinal.Z) / 2)
    End Sub

    Public Overrides Function ToString() As String
        Return "{Hueso: " & mExtremoInicial.ToString & " --> " & mExtremoFinal.ToString & ", Segmento: " & mSegmento.ToString & "}"
    End Function
End Class

Public Class Esqueleto
    Private mCabeza As Punto3D
    Private mBaseCuello As Punto3D
    Private mHombroDerecho As Punto3D
    Private mHombroIzquierdo As Punto3D
    Private mCodoDerecho As Punto3D
    Private mCodoIzquierdo As Punto3D
    Private mMuñecaDerecha As Punto3D
    Private mMuñecaIzquierda As Punto3D
    Private mPalmaDerecha As Punto3D
    Private mPalmaIzquierda As Punto3D
    Private mEspina As Punto3D
    Private mPelvis As Punto3D
    Private mCaderaDerecha As Punto3D
    Private mCaderaIzquierda As Punto3D
    Private mRodillaDerecha As Punto3D
    Private mRodillaIzquierda As Punto3D
    Private mTobilloDerecho As Punto3D
    Private mTobilloIzquierdo As Punto3D
    Private mPlantaDerecha As Punto3D
    Private mPlantaIzquierda As Punto3D

    Private mCuello As Hueso
    Private mEspalda As Hueso
    Private mClaviculaDerecha As Hueso
    Private mClaviculaIzquierda As Hueso
    Private mAntebrazoDerecho As Hueso
    Private mAntebrazoIzquierdo As Hueso
    Private mBrazoDerecho As Hueso
    Private mBrazoIzquierdo As Hueso
    Private mManoDerecha As Hueso
    Private mManoIzquierda As Hueso
    Private mMusloDerecho As Hueso
    Private mMusloIzquierdo As Hueso
    Private mEspinillaDerecha As Hueso
    Private mEspinillaIzquierda As Hueso
    Private mPieDerecho As Hueso
    Private mPieIzquierdo As Hueso

    Private mArticulaciones(19) As Punto3D
    Private mHuesos(15) As Hueso

    Public ReadOnly Property Cabeza As Punto3D
        Get
            Return mCabeza
        End Get
    End Property

    Public ReadOnly Property BaseCuello As Punto3D
        Get
            Return mBaseCuello
        End Get
    End Property

    Public ReadOnly Property HobroDerecho As Punto3D
        Get
            Return mHombroDerecho
        End Get
    End Property

    Public ReadOnly Property HobroIzquierdo As Punto3D
        Get
            Return mHombroIzquierdo
        End Get
    End Property

    Public ReadOnly Property CodoDerecho As Punto3D
        Get
            Return mCodoDerecho
        End Get
    End Property

    Public ReadOnly Property CodoIzquierdo As Punto3D
        Get
            Return mCodoIzquierdo
        End Get
    End Property

    Public ReadOnly Property MuñecaDerecha As Punto3D
        Get
            Return mMuñecaDerecha
        End Get
    End Property

    Public ReadOnly Property MuñecaIzquierda As Punto3D
        Get
            Return mMuñecaIzquierda
        End Get
    End Property

    Public ReadOnly Property PalmaDerecha As Punto3D
        Get
            Return mPalmaDerecha
        End Get
    End Property

    Public ReadOnly Property PalmaIzquierda As Punto3D
        Get
            Return mPalmaIzquierda
        End Get
    End Property

    Public ReadOnly Property Espina As Punto3D
        Get
            Return mEspina
        End Get
    End Property

    Public ReadOnly Property Pelvis As Punto3D
        Get
            Return mPelvis
        End Get
    End Property

    Public ReadOnly Property CaderaDerecha As Punto3D
        Get
            Return mCaderaDerecha
        End Get
    End Property

    Public ReadOnly Property CaderaIzquierda As Punto3D
        Get
            Return mCaderaIzquierda
        End Get
    End Property

    Public ReadOnly Property RodillaDerecha As Punto3D
        Get
            Return mRodillaDerecha
        End Get
    End Property

    Public ReadOnly Property RodillaIzquierda As Punto3D
        Get
            Return mRodillaIzquierda
        End Get
    End Property

    Public ReadOnly Property TobilloDerecho As Punto3D
        Get
            Return mTobilloDerecho
        End Get
    End Property

    Public ReadOnly Property TobilloIzquierdo As Punto3D
        Get
            Return mTobilloIzquierdo
        End Get
    End Property

    Public ReadOnly Property PlantaDerecha As Punto3D
        Get
            Return mPlantaDerecha
        End Get
    End Property

    Public ReadOnly Property PlantaIzquierda As Punto3D
        Get
            Return mPlantaIzquierda
        End Get
    End Property

    Public ReadOnly Property Cuello As Hueso
        Get
            Return mCuello
        End Get
    End Property

    Public ReadOnly Property Espalda As Hueso
        Get
            Return mEspalda
        End Get
    End Property

    Public ReadOnly Property ClaviculaDerecha As Hueso
        Get
            Return mClaviculaDerecha
        End Get
    End Property

    Public ReadOnly Property ClaviculaIzquierda As Hueso
        Get
            Return mClaviculaIzquierda
        End Get
    End Property

    Public ReadOnly Property AntebrazoDerecho As Hueso
        Get
            Return mAntebrazoDerecho
        End Get
    End Property

    Public ReadOnly Property AntebrazoIzquierdo As Hueso
        Get
            Return mAntebrazoIzquierdo
        End Get
    End Property

    Public ReadOnly Property BrazoDerecho As Hueso
        Get
            Return mBrazoDerecho
        End Get
    End Property

    Public ReadOnly Property BrazoIzquierdo As Hueso
        Get
            Return mBrazoIzquierdo
        End Get
    End Property

    Public ReadOnly Property ManoDerecha As Hueso
        Get
            Return mManoDerecha
        End Get
    End Property

    Public ReadOnly Property ManoIzquierda As Hueso
        Get
            Return mManoIzquierda
        End Get
    End Property

    Public ReadOnly Property MusloDerecho As Hueso
        Get
            Return mMusloDerecho
        End Get
    End Property

    Public ReadOnly Property MusloIzquierdo As Hueso
        Get
            Return mMusloIzquierdo
        End Get
    End Property

    Public ReadOnly Property EspinillaDerecha As Hueso
        Get
            Return mEspinillaDerecha
        End Get
    End Property

    Public ReadOnly Property EspinillaIzquierda As Hueso
        Get
            Return mEspinillaIzquierda
        End Get
    End Property

    Public ReadOnly Property PieDerecho As Hueso
        Get
            Return mPieDerecho
        End Get
    End Property

    Public ReadOnly Property PieIzquierdo As Hueso
        Get
            Return mPieIzquierdo
        End Get
    End Property

    Public ReadOnly Property Articulaciones As Punto3D()
        Get
            Return mArticulaciones
        End Get
    End Property

    Public ReadOnly Property Huesos As Hueso()
        Get
            Return mHuesos
        End Get
    End Property

    Public Sub New()
        mCabeza = New Punto3D
        mBaseCuello = New Punto3D
        mHombroDerecho = New Punto3D
        mHombroIzquierdo = New Punto3D
        mCodoDerecho = New Punto3D
        mCodoIzquierdo = New Punto3D
        mMuñecaDerecha = New Punto3D
        mMuñecaIzquierda = New Punto3D
        mPalmaDerecha = New Punto3D
        mPalmaIzquierda = New Punto3D
        mEspina = New Punto3D
        mPelvis = New Punto3D
        mCaderaDerecha = New Punto3D
        mCaderaIzquierda = New Punto3D
        mRodillaDerecha = New Punto3D
        mRodillaIzquierda = New Punto3D
        mTobilloDerecho = New Punto3D
        mTobilloIzquierdo = New Punto3D
        mPlantaDerecha = New Punto3D
        mPlantaIzquierda = New Punto3D

        mArticulaciones(0) = (mCabeza)
        mArticulaciones(1) = (mBaseCuello)
        mArticulaciones(2) = (mHombroDerecho)
        mArticulaciones(3) = (mHombroIzquierdo)
        mArticulaciones(4) = (mCodoDerecho)
        mArticulaciones(5) = (mCodoIzquierdo)
        mArticulaciones(6) = (mMuñecaDerecha)
        mArticulaciones(7) = (mMuñecaIzquierda)
        mArticulaciones(8) = (mPalmaDerecha)
        mArticulaciones(9) = (mPalmaIzquierda)
        mArticulaciones(10) = (mEspina)
        mArticulaciones(11) = (mPelvis)
        mArticulaciones(12) = (mCaderaDerecha)
        mArticulaciones(13) = (mCaderaIzquierda)
        mArticulaciones(14) = (mRodillaDerecha)
        mArticulaciones(15) = (mRodillaIzquierda)
        mArticulaciones(16) = (mTobilloDerecho)
        mArticulaciones(17) = (mTobilloIzquierdo)
        mArticulaciones(18) = (mPlantaDerecha)
        mArticulaciones(19) = (mPlantaIzquierda)

        mCuello = New Hueso(mBaseCuello, mCabeza)
        mEspalda = New Hueso(mPelvis, mBaseCuello)
        mClaviculaDerecha = New Hueso(mBaseCuello, mHombroDerecho)
        mClaviculaIzquierda = New Hueso(mBaseCuello, mHombroIzquierdo)
        mAntebrazoDerecho = New Hueso(mHombroDerecho, mCodoDerecho)
        mAntebrazoIzquierdo = New Hueso(mHombroIzquierdo, mCodoIzquierdo)
        mBrazoDerecho = New Hueso(mCodoDerecho, mMuñecaDerecha)
        mBrazoIzquierdo = New Hueso(mCodoIzquierdo, mMuñecaIzquierda)
        mManoDerecha = New Hueso(mMuñecaDerecha, mPalmaDerecha)
        mManoIzquierda = New Hueso(mMuñecaIzquierda, mPalmaIzquierda)
        mMusloDerecho = New Hueso(mCaderaDerecha, mRodillaDerecha)
        mMusloIzquierdo = New Hueso(mCaderaIzquierda, mRodillaIzquierda)
        mEspinillaDerecha = New Hueso(mRodillaDerecha, mTobilloDerecho)
        mEspinillaIzquierda = New Hueso(mRodillaIzquierda, mTobilloIzquierdo)
        mPieDerecho = New Hueso(mTobilloDerecho, mPlantaDerecha)
        mPieIzquierdo = New Hueso(mTobilloIzquierdo, mPlantaIzquierda)

        mHuesos(0) = (mCuello)
        mHuesos(1) = (mEspalda)
        mHuesos(2) = (mClaviculaDerecha)
        mHuesos(3) = (mClaviculaIzquierda)
        mHuesos(4) = (mAntebrazoDerecho)
        mHuesos(5) = (mAntebrazoIzquierdo)
        mHuesos(6) = (mBrazoDerecho)
        mHuesos(7) = (mBrazoIzquierdo)
        mHuesos(8) = (mManoDerecha)
        mHuesos(9) = (mManoIzquierda)
        mHuesos(10) = (mMusloDerecho)
        mHuesos(11) = (mMusloIzquierdo)
        mHuesos(12) = (mEspinillaDerecha)
        mHuesos(13) = (mEspinillaIzquierda)
        mHuesos(14) = (mPieDerecho)
        mHuesos(15) = (mPieIzquierdo)
    End Sub

    Public Sub Actualizar(ByVal Esqueleto As Skeleton)
        If Esqueleto.Joints(JointType.Head).TrackingState = JointTrackingState.Tracked Then
            mCabeza = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.Head))
        End If

        If Esqueleto.Joints(JointType.ShoulderCenter).TrackingState = JointTrackingState.Tracked Then
            mBaseCuello = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.ShoulderCenter))
        End If

        If Esqueleto.Joints(JointType.ShoulderRight).TrackingState = JointTrackingState.Tracked Then
            mHombroDerecho = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.ShoulderRight))
        End If

        If Esqueleto.Joints(JointType.ShoulderLeft).TrackingState = JointTrackingState.Tracked Then
            mHombroIzquierdo = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.ShoulderLeft))
        End If

        If Esqueleto.Joints(JointType.ElbowRight).TrackingState = JointTrackingState.Tracked Then
            mCodoDerecho = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.ElbowRight))
        End If

        If Esqueleto.Joints(JointType.ElbowLeft).TrackingState = JointTrackingState.Tracked Then
            mCodoIzquierdo = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.ElbowLeft))
        End If

        If Esqueleto.Joints(JointType.WristRight).TrackingState = JointTrackingState.Tracked Then
            mMuñecaDerecha = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.WristRight))
        End If

        If Esqueleto.Joints(JointType.WristLeft).TrackingState = JointTrackingState.Tracked Then
            mMuñecaIzquierda = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.WristLeft))
        End If

        If Esqueleto.Joints(JointType.Spine).TrackingState = JointTrackingState.Tracked Then
            mEspina = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.Spine))
        End If

        If Esqueleto.Joints(JointType.HipCenter).TrackingState = JointTrackingState.Tracked Then
            mPelvis = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.HipCenter))
        End If

        If Esqueleto.Joints(JointType.HipRight).TrackingState = JointTrackingState.Tracked Then
            mCaderaDerecha = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.HipRight))
        End If

        If Esqueleto.Joints(JointType.HipLeft).TrackingState = JointTrackingState.Tracked Then
            mCaderaIzquierda = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.HipLeft))
        End If

        If Esqueleto.Joints(JointType.KneeRight).TrackingState = JointTrackingState.Tracked Then
            mRodillaDerecha = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.KneeRight))
        End If

        If Esqueleto.Joints(JointType.KneeLeft).TrackingState = JointTrackingState.Tracked Then
            mRodillaIzquierda = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.KneeLeft))
        End If

        If Esqueleto.Joints(JointType.AnkleRight).TrackingState = JointTrackingState.Tracked Then
            mTobilloDerecho = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.AnkleRight))
        End If

        If Esqueleto.Joints(JointType.AnkleLeft).TrackingState = JointTrackingState.Tracked Then
            mTobilloIzquierdo = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.AnkleLeft))
        End If

        If Esqueleto.Joints(JointType.FootRight).TrackingState = JointTrackingState.Tracked Then
            mPlantaDerecha = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.FootRight))
        End If

        If Esqueleto.Joints(JointType.FootLeft).TrackingState = JointTrackingState.Tracked Then
            mPlantaIzquierda = Kinect.KinectToMotor3D(Esqueleto.Joints(JointType.FootLeft))
        End If

        mArticulaciones(0) = (mCabeza)
        mArticulaciones(1) = (mBaseCuello)
        mArticulaciones(2) = (mHombroDerecho)
        mArticulaciones(3) = (mHombroIzquierdo)
        mArticulaciones(4) = (mCodoDerecho)
        mArticulaciones(5) = (mCodoIzquierdo)
        mArticulaciones(6) = (mMuñecaDerecha)
        mArticulaciones(7) = (mMuñecaIzquierda)
        mArticulaciones(8) = (mPalmaDerecha)
        mArticulaciones(9) = (mPalmaIzquierda)
        mArticulaciones(10) = (mEspina)
        mArticulaciones(11) = (mPelvis)
        mArticulaciones(12) = (mCaderaDerecha)
        mArticulaciones(13) = (mCaderaIzquierda)
        mArticulaciones(14) = (mRodillaDerecha)
        mArticulaciones(15) = (mRodillaIzquierda)
        mArticulaciones(16) = (mTobilloDerecho)
        mArticulaciones(17) = (mTobilloIzquierdo)
        mArticulaciones(18) = (mPlantaDerecha)
        mArticulaciones(19) = (mPlantaIzquierda)

        mCuello = New Hueso(mBaseCuello, mCabeza)
        mEspalda = New Hueso(mPelvis, mBaseCuello)
        mClaviculaDerecha = New Hueso(mBaseCuello, mHombroDerecho)
        mClaviculaIzquierda = New Hueso(mBaseCuello, mHombroIzquierdo)
        mAntebrazoDerecho = New Hueso(mHombroDerecho, mCodoDerecho)
        mAntebrazoIzquierdo = New Hueso(mHombroIzquierdo, mCodoIzquierdo)
        mBrazoDerecho = New Hueso(mCodoDerecho, mMuñecaDerecha)
        mBrazoIzquierdo = New Hueso(mCodoIzquierdo, mMuñecaIzquierda)
        mManoDerecha = New Hueso(mMuñecaDerecha, mPalmaDerecha)
        mManoIzquierda = New Hueso(mMuñecaIzquierda, mPalmaIzquierda)
        mMusloDerecho = New Hueso(mCaderaDerecha, mRodillaDerecha)
        mMusloIzquierdo = New Hueso(mCaderaIzquierda, mRodillaIzquierda)
        mEspinillaDerecha = New Hueso(mRodillaDerecha, mTobilloDerecho)
        mEspinillaIzquierda = New Hueso(mRodillaIzquierda, mTobilloIzquierdo)
        mPieDerecho = New Hueso(mTobilloDerecho, mPlantaDerecha)
        mPieIzquierdo = New Hueso(mTobilloIzquierdo, mPlantaIzquierda)

        mHuesos(0) = (mCuello)
        mHuesos(1) = (mEspalda)
        mHuesos(2) = (mClaviculaDerecha)
        mHuesos(3) = (mClaviculaIzquierda)
        mHuesos(4) = (mAntebrazoDerecho)
        mHuesos(5) = (mAntebrazoIzquierdo)
        mHuesos(6) = (mBrazoDerecho)
        mHuesos(7) = (mBrazoIzquierdo)
        mHuesos(8) = (mManoDerecha)
        mHuesos(9) = (mManoIzquierda)
        mHuesos(10) = (mMusloDerecho)
        mHuesos(11) = (mMusloIzquierdo)
        mHuesos(12) = (mEspinillaDerecha)
        mHuesos(13) = (mEspinillaIzquierda)
        mHuesos(14) = (mPieDerecho)
        mHuesos(15) = (mPieIzquierdo)
    End Sub
End Class

Public Class Kinect
    Private mSensor As KinectSensor
    Private mEsqueleto As Esqueleto
    Private mSeguimiento As Boolean
    Private mCamara As Boolean
    Private mAngulo As Integer
    Private mLapso As Integer
    Private mTiempo As Date
    Private mTracked As Boolean
    Private mUltimoSkeletonFrameReadyEventArgs As SkeletonFrameReadyEventArgs
    Private mUltimosEsqueletos() As Skeleton
    Private mSkeletonTracked As Integer

    Private mImagenCamara As Bitmap

    Public Event EsqueletoActualizado(ByRef Esqueleto As Esqueleto)
    Public Event CamaraActualizada(ByRef Imagen As Bitmap)

    Public ReadOnly Property Sensor As KinectSensor
        Get
            Return mSensor
        End Get
    End Property

    Public ReadOnly Property Esqueleto As Esqueleto
        Get
            Return mEsqueleto
        End Get
    End Property

    Public Property Seguimiento As Boolean
        Get
            Return mSeguimiento
        End Get
        Set(value As Boolean)
            If value <> mSeguimiento Then
                mSeguimiento = value

                If mSeguimiento Then
                    AddHandler mSensor.SkeletonFrameReady, AddressOf SkeletonFrameReady
                Else
                    RemoveHandler mSensor.SkeletonFrameReady, AddressOf SkeletonFrameReady
                    mTracked = False
                End If
            End If
        End Set
    End Property

    Public Property Camara As Boolean
        Get
            Return mCamara
        End Get
        Set(value As Boolean)
            If value <> mCamara Then
                mCamara = value

                If mCamara Then
                    mSensor.ColorStream.Enable()
                Else
                    mSensor.ColorStream.Disable()
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property InstanteUltimoAngulo As Date
        Get
            Return mTiempo
        End Get
    End Property

    Public ReadOnly Property LapsoTiempoAngulo As Integer
        Get
            Return (Now - mTiempo).Seconds
        End Get
    End Property

    Public Property Angulo
        Get
            Return mAngulo
        End Get
        Set(value)
            If value <> mAngulo Then
                If (Now - mTiempo).Seconds >= 3 Then
                    mTiempo = Now
                    mAngulo = value
                    mSensor.ElevationAngle = mAngulo
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property SkeletonTracked As Integer
        Get
            Return mSkeletonTracked
        End Get
    End Property

    Public ReadOnly Property Tracked As Boolean
        Get
            Return mTracked
        End Get
    End Property

    Public ReadOnly Property ImagenCamara As Bitmap
        Get
            Return mImagenCamara
        End Get
    End Property

    Public ReadOnly Property UltimoSkeletonFrameReadyEventArgs As SkeletonFrameReadyEventArgs
        Get
            Return mUltimoSkeletonFrameReadyEventArgs
        End Get
    End Property

    Public ReadOnly Property UltimosEsqueletos As Skeleton()
        Get
            Return mUltimosEsqueletos
        End Get
    End Property


    Public Sub New()
        mSensor = KinectSensor.KinectSensors(0)

        Try
            mSensor.ColorStream.Enable()
            mSensor.SkeletonStream.Enable()

            mAngulo = 0
            mSeguimiento = True
            mCamara = True
            mTiempo = Now

            mEsqueleto = New Esqueleto
            mUltimoSkeletonFrameReadyEventArgs = Nothing
            mUltimosEsqueletos = Nothing
            mSkeletonTracked = -1

            AddHandler mSensor.SkeletonFrameReady, AddressOf SkeletonFrameReady

            mSensor.Start()
        Catch ex As Exception
            Throw New ExcepcionInicializacionKinect()
        End Try
    End Sub

    Public Sub Cerrar()
        mSensor.Stop()
        mSensor = Nothing
    End Sub

    Private Sub SkeletonFrameReady(ByVal Sender As Object, ByVal e As SkeletonFrameReadyEventArgs)
        Dim Frame As SkeletonFrame = e.OpenSkeletonFrame
        If Not Frame Is Nothing Then
            Dim esqs(Frame.SkeletonArrayLength - 1) As Skeleton

            Frame.CopySkeletonDataTo(esqs)
            mUltimosEsqueletos = esqs
            mUltimoSkeletonFrameReadyEventArgs = e

            For i As Integer = 0 To esqs.GetUpperBound(0)
                If esqs(i).TrackingState = SkeletonTrackingState.Tracked Then
                    mTracked = True
                    mSkeletonTracked = i
                    mEsqueleto.Actualizar(esqs(i))
                    RaiseEvent EsqueletoActualizado(mEsqueleto)
                    Exit For
                Else
                    mTracked = False
                End If
            Next

            mSkeletonTracked = -1
        End If
    End Sub

    Public Shared Function KinectToMotor3D(ByVal Posicion As SkeletonPoint, Optional ByVal Escala As Integer = 1000) As Punto3D
        Return New Punto3D(Posicion.X, Posicion.Y, Posicion.Z) * Escala
    End Function

    Public Shared Function KinectToMotor3D(ByVal Joint As Joint, Optional ByVal Escala As Integer = 1000) As Punto3D
        Return KinectToMotor3D(Joint.Position)
    End Function
End Class
