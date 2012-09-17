Imports Motor3D.Espacio2D
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Escena
Imports Motor3D.Escena.Shading
Imports System.Drawing

Namespace Primitivas3D
    Public Class Vertice
        Inherits ObjetoPrimitiva3D

        Private mCoordenadasSUR As Punto3D
        Private mCoordenadasSRC As Punto3D
        Private mRepresentacion As Punto2D
        Private mNormalSUR As Vector3D
        Private mNormalSRC As Vector3D
        Private mColor As Color
        Private mColorShading As Color
        Private mCaras() As Integer
        Private mShaded As Boolean

        Public Property CoodenadasSUR() As Punto3D
            Get
                Return mCoordenadasSUR
            End Get
            Set(ByVal value As Punto3D)
                mCoordenadasSUR = value
                mShaded = False
            End Set
        End Property

        Public Property CoodenadasSRC() As Punto3D
            Get
                Return mCoordenadasSRC
            End Get
            Set(ByVal value As Punto3D)
                mCoordenadasSRC = value
                mShaded = False
            End Set
        End Property

        Public Property Representacion() As Punto2D
            Get
                Return mRepresentacion
            End Get
            Set(ByVal value As Punto2D)
                mRepresentacion = value
            End Set
        End Property

        Public Property NormalSUR() As Vector3D
            Get
                Return mNormalSUR
            End Get
            Set(ByVal value As Vector3D)
                mNormalSUR = value
                mShaded = False
            End Set
        End Property

        Public Property NormalSRC() As Vector3D
            Get
                Return mNormalSRC
            End Get
            Set(ByVal value As Vector3D)
                mNormalSRC = value
                mShaded = False
            End Set
        End Property

        Public Property Color() As Color
            Get
                Return mColor
            End Get
            Set(ByVal value As Color)
                mColor = value
                mShaded = False
            End Set
        End Property

        Public Property ColorShading() As Color
            Get
                Return mColor
            End Get
            Set(ByVal value As Color)
                mColorShading = value
                mShaded = True
            End Set
        End Property

        Public ReadOnly Property Caras() As Integer()
            Get
                Return mCaras
            End Get
        End Property

        Public ReadOnly Property Caras(ByVal Indice As Integer) As Integer
            Get
                If Indice >= 0 AndAlso Indice <= mCaras.GetUpperBound(0) Then
                    Return mCaras(Indice)
                Else
                    Return -1
                End If
            End Get
        End Property

        Public ReadOnly Property Shaded() As Boolean
            Get
                Return mShaded
            End Get
        End Property

        Public Sub AplicarTransformacion(ByVal Transformacion As Transformacion3D)
            mCoordenadasSUR *= Transformacion
            mShaded = False
        End Sub

        Public Sub EstablecerCaras(ByVal ParamArray Caras() As Integer)
            mCaras = Caras
        End Sub

        Public Sub Shading(ByVal Constantes As PhongShader, ByVal Focos() As Foco3D, ByVal Camara As Camara3D)
            mColorShading = Constantes.EcuacionPhong(Focos, mNormalSRC, mCoordenadasSRC, mColor, Camara)
        End Sub

        Public Sub New(ByVal Coordenadas As Punto3D)
            mCoordenadasSUR = Coordenadas
            mRepresentacion = New Punto2D
            mNormalSUR = New Vector3D
            mColor = Color.White
            mColorShading = Color.White
            mShaded = False
        End Sub

        Public Shared Function BaricentroSUR(ByVal ParamArray Vertices() As Vertice) As Punto3D
            Dim x, y, z As Double

            x = 0
            y = 0
            z = 0

            For i As Integer = 0 To Vertices.GetUpperBound(0)
                x += Vertices(i).CoodenadasSUR.X
                y += Vertices(i).CoodenadasSUR.Y
                z += Vertices(i).CoodenadasSUR.Z
            Next

            Return New Punto3D(x / (Vertices.GetUpperBound(0) + 1), y / (Vertices.GetUpperBound(0) + 1), z / (Vertices.GetUpperBound(0) + 1))
        End Function

        Public Shared Function BaricentroSRC(ByVal ParamArray Vertices() As Vertice) As Punto3D
            Dim x, y, z As Double

            x = 0
            y = 0
            z = 0

            For i As Integer = 0 To Vertices.GetUpperBound(0)
                x += Vertices(i).CoodenadasSRC.X
                y += Vertices(i).CoodenadasSRC.Y
                z += Vertices(i).CoodenadasSRC.Z
            Next

            Return New Punto3D(x / (Vertices.GetUpperBound(0) + 1), y / (Vertices.GetUpperBound(0) + 1), z / (Vertices.GetUpperBound(0) + 1))
        End Function

        Public Overrides Function ToString() As String
            Return "{Vertice de Coordenadas=" & mCoordenadasSUR.ToString & ", Normal=" & mNormalSUR.ToString & " y Representacion=" & mRepresentacion.ToString & "}"
        End Function
    End Class
End Namespace

