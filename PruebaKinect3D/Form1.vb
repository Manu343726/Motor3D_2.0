Imports Microsoft.Kinect
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio2D
Imports Motor3D.Primitivas3D
Imports Motor3D.Escena
Imports System.Math

Public Class Form1
    Dim Kinect As KinectSensor

    Dim PosCabeza, PosCadera As Punto3D

    Dim Camara As Camara3D
    Dim Cubos As New List(Of Poliedro)
    Dim Foco As Foco3D

    Dim RND As New Random

    Dim BMP As Bitmap
    Dim g As Graphics

    Dim KinectActivado As Boolean = False
    Dim AnguloX, AnguloY, AnguloZ As Single

    Dim Motor As Motor3D.Escena.Motor3D

    Dim Debug As Boolean = False

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Interval = 1
        BMP = New Bitmap(Pic.Width, Pic.Height)
        g = Graphics.FromImage(BMP)
        g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)

        Camara = New Camara3D
        Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)
        Foco = New Foco3D(New Punto3D(), Color.White, 1)

        Motor = New Motor3D.Escena.Motor3D
        Motor.AñadirCamara(Camara)
        Motor.AñadirFoco(Foco)

        PosCabeza = New Punto3D
        PosCadera = New Punto3D
        AnguloX = 0
        AnguloY = 0
        AnguloZ = 0

        Repintar()

        ConectarKinect()
        Timer1.Start()
    End Sub

    Private Sub ConectarKinect()
        Kinect = New Runtime

        Try
            Kinect.Initialize(RuntimeOptions.UseSkeletalTracking)
            Kinect.NuiCamera.ElevationAngle = 0
        Catch ex As Exception
            MessageBox.Show("No se ha podido configurar el adaptador." & vbNewLine & "Asegurese de que su dispositivo Kinect esté correctamente conectado.", "Prueba Kinect 3D - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try
    End Sub

    Private Sub Seguimiento(ByVal sender As Object, ByVal e As SkeletonFrameReadyEventArgs)
        Dim ManoIzquierda As Joint
        For Each Skeleton As SkeletonData In e.SkeletonFrame.Skeletons
            If Skeleton.TrackingState = SkeletonTrackingState.Tracked Then
                Dim J As Joint = Skeleton.Joints(JointType.Head)
                Dim J2 As Joint = Skeleton.Joints(JointType.HipCenter)
                Dim J3 As Joint = Skeleton.Joints(JointType.HipRight)
                Dim J4 As Joint = Skeleton.Joints(JointType.HipLeft)
                ManoIzquierda = Skeleton.Joints(JointType.HandLeft)

                Dim x, y, z As Single

                'x = New Vector3D(New Punto3D(J2.Position.X, J2.Position.Y, J2.Position.Z), New Punto3D(J2.Position.X, 1000, J2.Position.Z)) ^ New Vector3D(New Punto3D(J2.Position.X, J2.Position.Y, J2.Position.Z), New Punto3D(J.Position.X, J.Position.Y, J2.Position.Z))
                y = New Vector3D(New Punto3D(J2.Position.X, J2.Position.Y, J2.Position.Z), New Punto3D(J2.Position.X, 1000, J2.Position.Z)) ^ New Vector3D(New Punto3D(J2.Position.X, J2.Position.Y, J2.Position.Z), New Punto3D(J.Position.X, J.Position.Y, J2.Position.Z))

                x = (PI / 2) * (J2.Position.Z - J.Position.Z)
                y = (PI / 2) * (J2.Position.X - J.Position.X)
                z = (PI / 2) * (J3.Position.Z - J4.Position.Z)

                Camara.RotarSobreSRC(EnumEjes.EjeX, x - AnguloX)
                AnguloX = x

                Camara.RotarSobreSRC(EnumEjes.EjeZ, y - AnguloY)
                AnguloY = y

                Camara.RotarSobreSRC(EnumEjes.EjeY, z - AnguloZ)
                AnguloZ = z

                PosCabeza = New Punto3D(J.Position.X, J.Position.Y, J.Position.Z)
                PosCadera = New Punto3D(J2.Position.X, J2.Position.Y, J2.Position.Z)

                Camara.TrasladarSobreSUR(New Punto3D(PosCabeza.X, -PosCabeza.Y, -PosCabeza.Z) * 10000)

                Foco.Coordenadas = New Punto3D(Camara.Posicion.X - (Camara.VectorDireccion.X * (20000 * (PosCadera.X - ManoIzquierda.Position.X))), Camara.Posicion.Y - (Camara.VectorDireccion.Y * (20000 * (PosCadera.Y - ManoIzquierda.Position.Y))), Camara.Posicion.Z - (Camara.VectorDireccion.Z * (20000 * (PosCadera.Z - ManoIzquierda.Position.Z))))

                Repintar()
            End If
        Next
    End Sub

    Private Sub RecalcularRepresentacions()
        For i As Integer = 0 To Cubos.Count - 1
            Cubos(i).RecalcularRepresentaciones(Camara)
        Next
    End Sub

    Private Sub CrearCubo()
        Dim C As Poliedro = Poliedro.Cubo
        Dim t, es As Double
        RND = New Random

        t = RND.Next(2000, 10000)
        es = RND.Next(100, 500)

        C.AplicarTransformacion(Transformacion3D.Escalado(es, es, es) + Transformacion3D.Traslacion(0, 0, t))
        Cubos.Add(C)

        Motor.AñadirPoliedro(C)
    End Sub

    Private Sub EliminarCubo()
        Motor.QuitarPoliedro(Cubos(Cubos.Count - 1))
        Cubos.RemoveAt(Cubos.Count - 1)
    End Sub


    Private Sub Repintar()
        Dim Poligono As Poligono2D

        g.Clear(Color.Black)

        Me.Text = "Motor3D 2.0 - Prueba Kinect 3D (Ángulo=" & AnguloX.ToString & ") [" & IIf(KinectActivado, "KINECT ACTIVADO]", "KINECT DESACTIVADO]")

        If Not Motor.ZBuffer.Vacio Then
            For Each Elemento As ElementoZBuffer In Motor.ZBuffer.Objetos
                Poligono = Motor.ZBuffer.Represenatciones(Elemento.Indices(0))

                g.FillPolygon(New SolidBrush(Poligono.Color), Poligono.VerticesToPoint)
            Next

            If Debug Then
                For i As Integer = 0 To Motor.NumeroPoliedros - 1
                    If Motor.ZBuffer.Pertenece(1, i) <> -1 Then
                        If Motor.Poliedros(i).AutoRecalcularCajas Then
                            For Each Pol As Poligono2D In Motor.Poliedros(i).CajaSUR.Representacion(Motor.CamaraSeleccionada)
                                g.DrawPolygon(Pens.Red, Pol.VerticesToPoint)
                            Next
                            For Each Pol As Poligono2D In Motor.Poliedros(i).CajaSRC.Representacion(Motor.CamaraSeleccionada, True)
                                g.DrawPolygon(Pens.GreenYellow, Pol.VerticesToPoint)
                            Next
                        Else
                            Motor.Poliedros(i).RecalcularCajaSUR()
                            Motor.Poliedros(i).RecalcularCajaSRC()

                            For Each Pol As Poligono2D In Motor.Poliedros(i).CajaSUR.Representacion(Motor.CamaraSeleccionada)
                                g.DrawPolygon(Pens.Red, Pol.VerticesToPoint)
                            Next
                            For Each Pol As Poligono2D In Motor.Poliedros(i).CajaSRC.Representacion(Motor.CamaraSeleccionada, True)
                                g.DrawPolygon(Pens.GreenYellow, Pol.VerticesToPoint)
                            Next
                        End If
                    End If
                Next
            End If
        End If

        If Debug Then
            g.DrawString("Número de cámaras=" & Motor.NumeroCamaras.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 10 - (Pic.Height / 2))
            g.DrawString("Número de poliedros=" & Motor.NumeroPoliedros.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 25 - (Pic.Height / 2))
            g.DrawString("Número de focos=" & Motor.NumeroFocos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 40 - (Pic.Height / 2))
            g.DrawString("Tamaño del buffer=" & Motor.ZBuffer.NumeroObjetos.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 55 - (Pic.Height / 2))
            g.DrawString("Posición de la cámara=" & Camara.Posicion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 70 - (Pic.Height / 2))
            g.DrawString("Dirección de la cámara=" & Camara.VectorDireccion.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 85 - (Pic.Height / 2))
            g.DrawString("Punto de mira de la cámara=" & Camara.PuntoDeMira.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 100 - (Pic.Height / 2))
            g.DrawString("Frustum=" & Camara.Frustum.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 115 - (Pic.Height / 2))
            g.DrawString("Transformación: " & vbNewLine & Camara.TransformacionSURtoSRC.Matriz.ToString, Me.Font, Brushes.White, 10 - (Pic.Width / 2), 130 - (Pic.Height / 2))
        End If

        Pic.Image = BMP
        Pic.Refresh()
    End Sub

    Private Sub Form1_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Control Then
            If e.Shift Then
                Select Case e.KeyCode
                    Case Keys.W
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeX, 0.02)
                    Case Keys.S
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeX, -0.02)
                    Case Keys.A
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeY, 0.02)
                    Case Keys.D
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeY, -0.02)
                    Case Keys.R
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeZ, 0.02)
                    Case Keys.F
                        Camara.RotarFijoSobreSUR(EnumEjes.EjeZ, -0.02)
                    Case Keys.Up
                        Kinect.NuiCamera.ElevationAngle += 5
                    Case Keys.Down
                        Kinect.NuiCamera.ElevationAngle -= 5
                End Select
            Else
                Select Case e.KeyCode
                    Case Keys.Up
                        Camara.TrasladarSobreSUR(New Vector3D(0, 0, 100))
                    Case Keys.Down
                        Camara.TrasladarSobreSUR(New Vector3D(0, 0, -100))
                    Case Keys.Right
                        Camara.TrasladarSobreSUR(New Vector3D(10, 0, 0))
                    Case Keys.Left
                        Camara.TrasladarSobreSUR(New Vector3D(-100, 0, 0))
                    Case Keys.NumPad8
                        Camara.TrasladarSobreSUR(New Vector3D(0, 100, 0))
                    Case Keys.NumPad2
                        Camara.TrasladarSobreSUR(New Vector3D(0, -100, 0))
                    Case Keys.W
                        Camara.RotarSobreSUR(EnumEjes.EjeX, 0.02)
                    Case Keys.S
                        Camara.RotarSobreSUR(EnumEjes.EjeX, -0.02)
                    Case Keys.A
                        Camara.RotarSobreSUR(EnumEjes.EjeY, 0.02)
                    Case Keys.D
                        Camara.RotarSobreSUR(EnumEjes.EjeY, -0.02)
                    Case Keys.R
                        Camara.RotarSobreSUR(EnumEjes.EjeZ, 0.02)
                    Case Keys.F
                        Camara.RotarSobreSUR(EnumEjes.EjeZ, -0.02)
                End Select
            End If

        Else
            Select Case e.KeyCode
                Case Keys.Up
                    Camara.TrasladarSobreSRC(New Vector3D(0, 0, 100))
                Case Keys.Down
                    Camara.TrasladarSobreSRC(New Vector3D(0, 0, -100))
                Case Keys.Right
                    Camara.TrasladarSobreSRC(New Vector3D(100, 0, 0))
                Case Keys.Left
                    Camara.TrasladarSobreSRC(New Vector3D(-100, 0, 0))
                Case Keys.NumPad8
                    Camara.TrasladarSobreSRC(New Vector3D(0, 100, 0))
                Case Keys.NumPad2
                    Camara.TrasladarSobreSRC(New Vector3D(0, -100, 0))
                Case Keys.W
                    Camara.RotarSobreSRC(EnumEjes.EjeX, 0.02)
                Case Keys.S
                    Camara.RotarSobreSRC(EnumEjes.EjeX, -0.02)
                Case Keys.A
                    Camara.RotarSobreSRC(EnumEjes.EjeY, 0.02)
                Case Keys.D
                    Camara.RotarSobreSRC(EnumEjes.EjeY, -0.02)
                Case Keys.R
                    Camara.RotarSobreSRC(EnumEjes.EjeZ, 0.02)
                Case Keys.F
                    Camara.RotarSobreSRC(EnumEjes.EjeZ, -0.02)
                Case Keys.Add
                    Camara.Distancia += 100
                Case Keys.Subtract
                    Camara.Distancia -= 100
                Case Keys.P
                    Timer1.Enabled = Not Timer1.Enabled
                Case Keys.Enter
                    CrearCubo()
                Case Keys.Back
                    If Cubos.Count > 0 Then EliminarCubo()
                Case Keys.Tab
                    Debug = Not Debug
                Case Keys.K
                    KinectActivado = Not KinectActivado
                    If KinectActivado Then
                        AddHandler Kinect.SkeletonFrameReady, AddressOf Seguimiento
                    Else
                        RemoveHandler Kinect.SkeletonFrameReady, AddressOf Seguimiento
                    End If
                Case Keys.L
                    Motor.ShadingActivado = Not Motor.ShadingActivado
            End Select
        End If

        RecalcularRepresentacions()

        Repintar()
    End Sub

    Private Sub Form1_SizeChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.SizeChanged
        If Not (Camara Is Nothing OrElse (Pic.Width = 0 OrElse Pic.Height = 0)) Then
            BMP = New Bitmap(Pic.Width, Pic.Height)
            g = Graphics.FromImage(BMP)
            g.TranslateTransform(Pic.Width / 2, Pic.Height / 2)

            Camara.ResolucionPantalla = New Punto2D(Pic.Width, Pic.Height)
            Repintar()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Motor.IniciarEscena()
        For i As Integer = 0 To Cubos.Count - 1
            Cubos(i).AplicarTransformacion(Transformacion3D.RotacionSobrePunto(Cubos(i).CentroSUR, CInt(RND.Next(0, 2)), RND.Next(-0.01, 0.01)) + Transformacion3D.Rotacion(IIf(i Mod 2 = 0, EnumEjes.EjeX, EnumEjes.EjeY), 0.01)) ' + Transformacion3D.RotacionSobrePunto(Cubo.Centro, EnumEjes.EjeX, 0.01))
            Cubos(i).RecalcularRepresentaciones(Camara)
        Next

        Motor.FinalizarEscena()

        Repintar()
    End Sub
End Class
