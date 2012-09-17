Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Motor3D.Espacio2D

Namespace Motor3D.Escena.Renders.RenderXNA
    Public Class JuegoBase
        Inherits Game

        Protected mMotor As Motor3D
        Protected mRender As RenderXNA

        Protected mVertexDeclaration As VertexDeclaration
        Protected mSpritebatch As SpriteBatch
        Protected mEffect As BasicEffect

        Public ReadOnly Property Motor As Motor3D
            Get
                Return mMotor
            End Get
        End Property

        Public ReadOnly Property Render As RenderXNA
            Get
                Return mRender
            End Get
        End Property

        Public Sub New(ByRef Motor As Motor3D, ByRef Render As RenderXNA)
            mMotor = Motor
            mRender = Render
        End Sub

        Protected Overrides Sub Initialize()
            MyBase.Initialize()
            mVertexDeclaration = New VertexDeclaration(New VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), _
                                                       New VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0))

            mSpritebatch = New SpriteBatch(GraphicsDevice)
            mEffect = New BasicEffect(GraphicsDevice)
            mEffect.VertexColorEnabled = True

            mEffect.View = Matrix.Identity
            mEffect.Projection = Matrix.Identity

            mRender.Redimensionar(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height)
            mMotor.CamaraSeleccionada.ResolucionPantalla = New Punto2D(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height)
        End Sub

        Protected Overrides Sub Draw(gameTime As Microsoft.Xna.Framework.GameTime)
            GraphicsDevice.Clear(Color.Black)

            For Each Pass As EffectPass In mEffect.CurrentTechnique.Passes
                Pass.Apply()
                For Each Poligono As PoligonoXNA In mRender.Poligonos
                    GraphicsDevice.DrawUserIndexedPrimitives(Of VertexPositionColor)(PrimitiveType.TriangleList, Poligono.Vertices, 0, Poligono.NumeroVertices, Poligono.Indices, 0, Poligono.NumeroVertices - 2)
                Next
            Next

            MyBase.Draw(gameTime)
        End Sub
    End Class
End Namespace

