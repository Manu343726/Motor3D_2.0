#If WINDOWS Or XBOX Then
Imports System.Drawing
Imports Motor3D.Espacio3D
Imports Motor3D.Escena
Imports Motor3D.Escena.Renders.RenderXNA
Module Program
    ''' <summary>
    ''' Punto de entrada principal de la aplicación.
    ''' </summary>
    ''' 
    Private Motor As Motor3D.Escena.Motor3D
    Private Render As RenderXNA
    Private Camara As New Camara3D()
    Private mFoco As New Foco3D(New Punto3D(1000, 1000, 1000), Color.White)

    Sub Main(ByVal args As String())
        Motor = New Motor3D.Escena.Motor3D
        Motor.AñadirFoco(mFoco)
        Motor.AñadirCamara(Camara)
        Motor.ShadingActivado = True
        Camara.Distancia = 100
        Render = New RenderXNA(Motor)

        Using game As New Game1(Motor, Render)
            game.Run()
        End Using
    End Sub
End Module

#End If
