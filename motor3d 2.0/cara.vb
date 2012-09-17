Imports Motor3D.Espacio3D
Imports Motor3D.Espacio2D
Imports Motor3D.Escena
Imports Motor3D.Escena.Shading
Imports System.Drawing
Imports System.Math

Namespace Primitivas3D
    Public Class Cara
        Inherits ObjetoPrimitiva3D

        Private mNormalSUR As Vector3D
        Private mNormalSRC As Vector3D
        Private mVertices() As Integer
        Private mBaricentroSUR As Punto3D
        Private mBaricentroSRC As Punto3D
        Private mColor As Color
        Private mColorShading As Color
        Private mCargadaEnBuffer As Boolean

        Public Shadows Event Modificado(ByRef Sebder As Cara)

        Public Property CargadaEnBuffer As Boolean
            Get
                Return mCargadaEnBuffer
            End Get
            Set(value As Boolean)
                mCargadaEnBuffer = value
            End Set
        End Property

        Public Property NormalSUR() As Vector3D
            Get
                Return mNormalSUR
            End Get
            Set(ByVal value As Vector3D)
                mNormalSUR = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property NormalSRC() As Vector3D
            Get
                Return mNormalSRC
            End Get
            Set(ByVal value As Vector3D)
                mNormalSRC = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property BaricentroSUR() As Punto3D
            Get
                Return mBaricentroSUR
            End Get
            Set(ByVal value As Punto3D)
                mBaricentroSUR = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property BaricentroSRC() As Punto3D
            Get
                Return mBaricentroSRC
            End Get
            Set(ByVal value As Punto3D)
                mBaricentroSRC = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property Vertices() As Integer()
            Get
                Return mVertices
            End Get
            Set(ByVal value As Integer())
                If value.GetUpperBound(0) = mVertices.GetUpperBound(0) Then
                    mVertices = value
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property Vertices(ByVal Indice As Integer) As Integer
            Get
                If Indice >= 0 AndAlso Indice <= mVertices.GetUpperBound(0) Then
                    Return mVertices(Indice)
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Integer)
                If Indice >= 0 AndAlso Indice <= mVertices.GetUpperBound(0) Then
                    mVertices(Indice) = value
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public ReadOnly Property Vertices(ByVal ArrayVertices() As Vertice) As Vertice()
            Get
                Dim Retorno(mVertices.GetUpperBound(0)) As Vertice

                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    Retorno(i) = ArrayVertices(mVertices(i))
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property PuntosSUR(ByVal Vertices() As Vertice) As Punto3D()
            Get
                Dim Retorno(mVertices.GetUpperBound(0)) As Punto3D

                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    Retorno(i) = Vertices(mVertices(i)).CoodenadasSUR
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property PuntosSRC(ByVal Vertices() As Vertice) As Punto3D()
            Get
                Dim Retorno(mVertices.GetUpperBound(0)) As Punto3D

                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    Retorno(i) = Vertices(mVertices(i)).CoodenadasSRC
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property Representacion(ByVal Vertices() As Vertice) As Punto2D()
            Get
                Dim Retorno(mVertices.GetUpperBound(0)) As Punto2D

                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    Retorno(i) = Vertices(mVertices(i)).Representacion
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property RepresentacionToPoint(ByVal Vertices() As Vertice) As Point()
            Get
                Dim Retorno(mVertices.GetUpperBound(0)) As Point

                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    Retorno(i) = Vertices(mVertices(i)).Representacion.ToPoint
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property NumeroVertices() As Integer
            Get
                Return mVertices.GetUpperBound(0) + 1
            End Get
        End Property

        Public Property Color() As Color
            Get
                Return mColor
            End Get
            Set(ByVal value As Color)
                mColor = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property ColorShading() As Color
            Get
                Return mColorShading
            End Get
            Set(ByVal value As Color)
                mColorShading = value
            End Set
        End Property

        Public Sub New(ByVal ParamArray Vertices() As Integer)
            If Vertices.GetUpperBound(0) >= 2 Then
                mVertices = Vertices
                mNormalSUR = New Vector3D
                mBaricentroSUR = New Punto3D
                mColor = Color.White
            Else
                If Vertices.GetUpperBound(0) = 0 Then
                    ReDim mVertices(Vertices(0) - 1)
                    mNormalSUR = New Vector3D
                    mBaricentroSUR = New Punto3D
                    mColor = Color.White
                Else
                    Throw New ExcepcionPrimitiva3D("CARA (NEW): Una cara debe tener al menos tres vértices." & vbNewLine _
                                                  & "Número de vértices: " & Vertices.GetUpperBound(0) + 1)
                End If

            End If
        End Sub

        Public Sub RecalcularNormalSUR(ByVal Vertices() As Vertice)
            mNormalSUR = Cara.VectorNormalSUR(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RecalcularBaricentroSUR(ByVal Vertices() As Vertice)
            mBaricentroSUR = Cara.BaricentroCaraSUR(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RecalcularDatosSUR(ByVal Vertices() As Vertice)
            mNormalSUR = Cara.VectorNormalSUR(Me, Vertices)
            mBaricentroSUR = Cara.BaricentroCaraSUR(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RecalcularNormalSRC(ByVal Vertices() As Vertice)
            mNormalSRC = Cara.VectorNormalSRC(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RecalcularBaricentroSRC(ByVal Vertices() As Vertice)
            mBaricentroSRC = Cara.BaricentroCaraSRC(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub RecalcularDatosSRC(ByVal Vertices() As Vertice)
            mNormalSRC = Cara.VectorNormalSRC(Me, Vertices)
            mBaricentroSRC = Cara.BaricentroCaraSRC(Me, Vertices)
            RaiseEvent Modificado(Me)
        End Sub

        Public Sub Shading(ByVal Constantes As PhongShader, ByVal Focos() As Foco3D, ByVal Camara As Camara3D)
            mColorShading = Constantes.EcuacionPhong(Focos, mNormalSUR, mBaricentroSUR, mColor, Camara)
        End Sub

        Public Sub RevertirVertices()
            Array.Reverse(mVertices)
        End Sub

        Public Function EsVisible(ByVal Camara As Camara3D, ByVal Vertices() As Vertice) As Boolean
            If mNormalSRC.Z <= 0.01 AndAlso Camara.Frustum.Pertenece(mBaricentroSRC) Then
                For i As Integer = 0 To mVertices.GetUpperBound(0)
                    If Camara.Pantalla.Pertenece(Vertices(mVertices(i)).Representacion) Then
                        Return True
                    End If
                Next

                Return False
            Else
                Return False
            End If
        End Function

        Public Shared Function VectorNormalSUR(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Vector3D
            Return (New Vector3D(Vertices(Cara.Vertices(0)).CoodenadasSUR, Vertices(Cara.Vertices(1)).CoodenadasSUR) & New Vector3D(Vertices(Cara.Vertices(0)).CoodenadasSUR, Vertices(Cara.Vertices(2)).CoodenadasSUR)).VectorUnitario
        End Function

        Public Shared Function VectorNormalSRC(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Vector3D
            Return (New Vector3D(Vertices(Cara.Vertices(0)).CoodenadasSRC, Vertices(Cara.Vertices(1)).CoodenadasSRC) & New Vector3D(Vertices(Cara.Vertices(0)).CoodenadasSRC, Vertices(Cara.Vertices(2)).CoodenadasSRC)).VectorUnitario
        End Function

        Public Shared Function PlanoSUR(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Plano3D
            Return New Plano3D(Vertices(Cara.Vertices(0)).CoodenadasSUR, Vertices(Cara.Vertices(1)).CoodenadasSUR, Vertices(Cara.Vertices(2)).CoodenadasSUR)
        End Function

        Public Shared Function PlanoSRC(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Plano3D
            Return New Plano3D(Vertices(Cara.Vertices(0)).CoodenadasSRC, Vertices(Cara.Vertices(1)).CoodenadasSRC, Vertices(Cara.Vertices(2)).CoodenadasSRC)
        End Function

        Public Shared Function BaricentroCaraSUR(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Punto3D
            Return Punto3D.Baricentro(Cara.PuntosSUR(Vertices))
        End Function

        Public Shared Function BaricentroCaraSRC(ByVal Cara As Cara, ByVal Vertices() As Vertice) As Punto3D
            Return Punto3D.Baricentro(Cara.PuntosSRC(Vertices))
        End Function

        Public Overrides Function ToString() As String
            Dim Retorno As String = ""

            For i As Integer = 0 To mVertices.GetUpperBound(0)
                If i < mVertices.GetUpperBound(0) Then
                    Retorno &= mVertices(i).ToString & ","
                Else
                    Retorno &= mVertices(i).ToString & ",Baricentro=" & mBaricentroSUR.ToString & ",Normal=" & mNormalSUR.ToString & "}"
                End If

            Next

            Return Retorno
        End Function
    End Class
End Namespace

