Imports Motor3D.Escena.Renders.RenderXNA
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Primitivas3D
Imports Motor3D.Espacio3D
Imports System.Drawing

Public Class Game1
    Inherits JuegoBase

    Private WithEvents graphics As GraphicsDeviceManager
    Private WithEvents spriteBatch As SpriteBatch

    Private Cubos As List(Of Poliedro)
    Private RND As Random

    Public Sub New(ByRef Motor As Motor3D.Escena.Motor3D, ByRef Render As RenderXNA)
        MyBase.New(Motor, Render)
        graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
        Cubos = New List(Of Poliedro)
    End Sub

    ''' <summary>
    ''' Permite que el juego realice la inicialización que necesite para empezar a ejecutarse.
    ''' Aquí es donde puede solicitar cualquier servicio que se requiera y cargar todo tipo de contenido
    ''' no relacionado con los gráficos. Si se llama a MyBase.Initialize, todos los componentes se enumerarán
    ''' e inicializarán.
    ''' </summary>
    Protected Overrides Sub Initialize()
        ' TODO: agregue aquí su lógica de inicialización
        MyBase.Initialize()
    End Sub

    Private Sub CrearCubo()
        Dim t As Double
        RND = New Random

        t = RND.Next(1000, 1000)

        CrearCubo(New Vector3D(t, t, t))
    End Sub

    Private Sub CrearCubo(ByVal Posicion As Vector3D)
        Dim C As Poliedro = Poliedro.Cubo()

        C.AplicarTransformacion(New Escalado(100, 100, 100) + New Traslacion(Posicion))
        C.EstablecerColor(0, Color.Red)
        C.EstablecerColor(1, Color.Green)
        C.EstablecerColor(2, Color.Blue)
        C.EstablecerColor(3, Color.Orange)
        C.EstablecerColor(4, Color.White)
        C.EstablecerColor(5, Color.Purple)
        Cubos.Add(C)

        mMotor.AñadirPoliedro(C)
    End Sub

    Private Sub EliminarCubo()
        If Cubos.Count > 0 Then
            mMotor.QuitarPoliedro(Cubos(Cubos.Count - 1))
            Cubos.RemoveAt(Cubos.Count - 1)
        End If
    End Sub

    ''' <summary>
    ''' LoadContent se llama una vez por juego y permite cargar
    ''' todo el contenido.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        ' Crea un SpriteBatch nuevo para dibujar texturas.
        spriteBatch = New SpriteBatch(GraphicsDevice)

        ' TODO: use Me.Content para cargar el contenido del juego aquí
    End Sub

    ''' <summary>
    ''' UnloadContent se llama una vez por juego y permite descargar
    ''' todo el contenido.
    ''' </summary>
    Protected Overrides Sub UnloadContent()
        ' TODO: descargue aquí todo el contenido que no pertenezca a ContentManager
    End Sub

    ''' <summary>
    ''' Permite al juego ejecutar lógica para, por ejemplo, actualizar el mundo,
    ''' buscar colisiones, recopilar entradas y reproducir audio.
    ''' </summary>
    ''' <param name="gameTime">Proporciona una instantánea de los valores de tiempo.</param>
    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        Dim Pad As GamePadState = GamePad.GetState(PlayerIndex.One)
        ' Permite salir del juego
        If Pad.Buttons.Back = ButtonState.Pressed Then
            Me.Exit()
        End If

        If Pad.ThumbSticks.Left.X <> 0 OrElse Pad.ThumbSticks.Left.Y <> 0 Then
            Motor.CamaraSeleccionada.TrasladarSobreSRC(New Vector3D(Pad.ThumbSticks.Left.X, 0, Pad.ThumbSticks.Left.Y) * 100)
        End If

        If Pad.ThumbSticks.Right.X <> 0 OrElse Pad.ThumbSticks.Right.Y <> 0 OrElse Pad.Triggers.Right <> 0 OrElse Pad.Triggers.Left <> 0 Then
            Motor.CamaraSeleccionada.RotarSobreSRC(New Vector3D(Pad.ThumbSticks.Right.X, Pad.ThumbSticks.Right.Y, Pad.Triggers.Left - Pad.Triggers.Right), 0.01)
        End If

        If Pad.Buttons.A = ButtonState.Pressed Then
            CrearCubo()
        Else
            If Pad.Buttons.B = ButtonState.Pressed Then
                EliminarCubo()
            End If
        End If
        MyBase.Update(gameTime)
    End Sub
End Class
