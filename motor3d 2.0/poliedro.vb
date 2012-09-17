Imports Motor3D.Escena
Imports Motor3D.Escena.Shading
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio3D.Transformaciones
Imports Motor3D.Espacio2D
Imports System.Drawing
Imports System.Math

Namespace Primitivas3D
    Public Class Poliedro
        Inherits ObjetoPrimitiva3D

        Protected mVertices() As Vertice
        Protected mCaras() As Cara
        Protected mCentroSUR As Punto3D
        Protected mCentroSRC As Punto3D
        Protected mVertical As Vector3D
        Protected NormalesCentro As Boolean
        Protected AutoReclcNorms As Boolean
        Protected mAABBSUR As AABB3D
        Protected mAABBSRC As AABB3D
        Protected mAutoRecalcularAABBs As Boolean
        Protected mConstantesShading As PhongShader
        Protected mShaded As Boolean

        Public Shadows Event Modificado(ByRef Sender As Poliedro)
        Public Event TransformacionCompletada(ByRef Sender As Poliedro)

        Public ReadOnly Property Vertices() As Vertice()
            Get
                Return mVertices
            End Get
        End Property

        Public ReadOnly Property Vertices(ByVal Indice As Integer) As Vertice
            Get
                Return mVertices(Indice)
            End Get
        End Property

        Public ReadOnly Property Caras() As Cara()
            Get
                Return mCaras
            End Get
        End Property

        Public ReadOnly Property Caras(ByVal Indice As Integer) As Cara
            Get
                Return mCaras(Indice)
            End Get
        End Property

        Public Property ColorCara(ByVal Indice As Integer) As Color
            Get
                Return mCaras(Indice).Color
            End Get
            Set(ByVal value As Color)
                mCaras(Indice).Color = value
            End Set
        End Property

        Public ReadOnly Property CaraVisible(ByVal Indice As Integer, ByVal Camara As Camara3D) As Boolean
            Get
                Return mCaras(Indice).EsVisible(Camara, mVertices)
            End Get
        End Property

        Public Property AutoRecalcularAABBs As Boolean
            Get
                Return mAutoRecalcularAABBs
            End Get
            Set(ByVal value As Boolean)
                mAutoRecalcularAABBs = value
            End Set
        End Property

        Public ReadOnly Property AABBSUR As AABB3D
            Get
                Return mAABBSUR
            End Get
        End Property

        Public ReadOnly Property AABBSRC As AABB3D
            Get
                Return mAABBSRC
            End Get
        End Property

        Public ReadOnly Property CentroSUR() As Punto3D
            Get
                Return mCentroSUR
            End Get
        End Property

        Public ReadOnly Property CentroSRC() As Punto3D
            Get
                Return mCentroSRC
            End Get
        End Property

        Public Property Vertical As Vector3D
            Get
                Return mVertical
            End Get

            Set(ByVal value As Vector3D)
                If value <> mVertical Then
                    AplicarTransformacion(New Rotacion(mVertical, value))
                    mVertical = value
                End If
            End Set
        End Property

        Public ReadOnly Property NumeroVertices() As Integer
            Get
                Return mVertices.GetUpperBound(0) + 1
            End Get
        End Property

        Public ReadOnly Property NumeroCaras() As Integer
            Get
                Return mCaras.GetUpperBound(0) + 1
            End Get
        End Property

        Public ReadOnly Property Representacion() As Poligono2D()
            Get
                Dim Retorno(mCaras.GetUpperBound(0)) As Poligono2D
                Dim Repr() As Punto2D

                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    ReDim Repr(mCaras(i).Vertices.GetUpperBound(0))
                    For j As Integer = 0 To mCaras(i).Vertices.GetUpperBound(0)
                        Repr(j) = mVertices(mCaras(i).Vertices(j)).Representacion
                    Next
                    Retorno(i) = New Poligono2D(Repr)
                    Retorno(i).Color = mCaras(i).Color
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property Representacion(ByVal IndiceCara As Integer) As Poligono2D
            Get
                Dim Retorno As Poligono2D
                Dim Repr(mCaras(IndiceCara).NumeroVertices - 1) As Punto2D

                For i As Integer = 0 To mCaras(IndiceCara).Vertices.GetUpperBound(0)
                    Repr(i) = mVertices(mCaras(IndiceCara).Vertices(i)).Representacion
                Next
                Retorno = New Poligono2D(Repr)
                Retorno.Color = mCaras(IndiceCara).Color

                Return Retorno
            End Get
        End Property

        Public Property Shaded As Boolean
            Get
                Return mShaded
            End Get
            Set(ByVal value As Boolean)
                mShaded = value
            End Set
        End Property

        Public Property NormalesDesdeCentro() As Boolean
            Get
                Return NormalesCentro
            End Get
            Set(ByVal value As Boolean)
                If value <> NormalesCentro Then
                    NormalesCentro = value
                    RecalcularDatosCaras()
                    If AutoReclcNorms Then RecalcularNormalesVertices()
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property AutoRecalcularNormalesVertices() As Boolean
            Get
                Return AutoReclcNorms
            End Get
            Set(ByVal value As Boolean)
                If AutoReclcNorms <> value Then
                    AutoReclcNorms = value
                End If
            End Set
        End Property

        Public ReadOnly Property ConstantesShading As PhongShader
            Get
                Return mConstantesShading
            End Get
        End Property

        Public Sub EstablecerColor(ByVal Color As Color)
            For i As Integer = 0 To mCaras.GetUpperBound(0)
                mCaras(i).Color = Color
            Next
        End Sub

        Public Sub EstablecerColor(ByVal Cara As Integer, ByVal Color As Color)
            mCaras(Cara).Color = Color
        End Sub

        Public Sub EstablecerConstantesShading(ByRef Constantes As PhongShader)
            mConstantesShading = Constantes
        End Sub

        Public Sub RecalcularNormalesVertices()
            Dim Normal As New Vector3D

            For i As Integer = 0 To mVertices.GetUpperBound(0)
                Normal = New Vector3D
                For j As Integer = 0 To mVertices(i).Caras.GetUpperBound(0)
                    Normal += mCaras(mVertices(i).Caras(j)).NormalSUR
                Next
                mVertices(i).NormalSUR = Normal / (mVertices(i).Caras.GetUpperBound(0) + 1)
            Next
        End Sub

        Public Sub RecalcularNormalesCarasVertice(ByVal IndiceVertice As Integer)
            If IndiceVertice >= 0 AndAlso IndiceVertice <= mVertices.GetUpperBound(0) Then
                Dim Normal As New Vector3D

                For i As Integer = 0 To mVertices(i).Caras.GetUpperBound(0)
                    Normal += mCaras(mVertices(i).Caras(i)).NormalSUR
                Next
                mVertices(IndiceVertice).NormalSUR = Normal / (mVertices(IndiceVertice).Caras.GetUpperBound(0) + 1)
            End If
        End Sub

        Public Sub AplicarTransformacion(ByVal Transformacion As Transformacion3D)
            For i As Integer = 0 To mVertices.GetUpperBound(0)
                mVertices(i).CoodenadasSUR = Transformacion * mVertices(i).CoodenadasSUR
            Next

            mVertical *= Transformacion
            CalculoTransformacion()
        End Sub

        Public Sub AplicarTransformacion(ByVal Traslacion As Traslacion)
            For i As Integer = 0 To mVertices.GetUpperBound(0)
                mVertices(i).CoodenadasSUR = Traslacion * mVertices(i).CoodenadasSUR
            Next

            mVertical *= Traslacion
            CalculoTransformacion()
        End Sub

        Public Sub AplicarTransformacion(ByVal Escalado As Escalado)
            For i As Integer = 0 To mVertices.GetUpperBound(0)
                mVertices(i).CoodenadasSUR = Escalado * mVertices(i).CoodenadasSUR
            Next

            mVertical *= Escalado
            CalculoTransformacion()
        End Sub

        Public Sub AplicarTransformacion(ByVal Rotacion As Rotacion)
            For i As Integer = 0 To mVertices.GetUpperBound(0)
                mVertices(i).CoodenadasSUR = Rotacion * mVertices(i).CoodenadasSUR
            Next

            mVertical *= Rotacion
            CalculoTransformacion()
        End Sub

        Private Sub CalculoTransformacion()
            mVertical = mVertical.VectorUnitario
            RecalcularCentro()
            RecalcularDatosCaras()
            If mAutoRecalcularAABBs Then mAABBSUR = ObtenerAABBSUR()
            If AutoReclcNorms Then RecalcularNormalesVertices()

            RaiseEvent Modificado(Me)
            RaiseEvent TransformacionCompletada(Me)
        End Sub

        Public Function ObtenerAABBSUR() As AABB3D
            Dim maxx, maxy, maxz, minx, miny, minz As Double

            maxx = mVertices(0).CoodenadasSUR.X
            maxy = mVertices(0).CoodenadasSUR.Y
            maxz = mVertices(0).CoodenadasSUR.Z
            minx = mVertices(0).CoodenadasSUR.X
            miny = mVertices(0).CoodenadasSUR.Y
            minz = mVertices(0).CoodenadasSUR.Z

            For i As Integer = 1 To mVertices.GetUpperBound(0)
                If mVertices(i).CoodenadasSUR.X > maxx Then maxx = mVertices(i).CoodenadasSUR.X
                If mVertices(i).CoodenadasSUR.Y > maxy Then maxy = mVertices(i).CoodenadasSUR.Y
                If mVertices(i).CoodenadasSUR.Z > maxz Then maxz = mVertices(i).CoodenadasSUR.Z
                If mVertices(i).CoodenadasSUR.X < minx Then minx = mVertices(i).CoodenadasSUR.X
                If mVertices(i).CoodenadasSUR.Y < miny Then miny = mVertices(i).CoodenadasSUR.Y
                If mVertices(i).CoodenadasSUR.Z < minz Then minz = mVertices(i).CoodenadasSUR.Z
            Next

            Return New AABB3D(minx, miny, minz, Abs(maxx - minx), Abs(maxy - miny), Abs(maxz - minz))
        End Function

        Public Function ObtenerAABBSRC() As AABB3D
            Dim maxx, maxy, maxz, minx, miny, minz As Double

            maxx = mVertices(0).CoodenadasSRC.X
            maxy = mVertices(0).CoodenadasSRC.Y
            maxz = mVertices(0).CoodenadasSRC.Z
            minx = mVertices(0).CoodenadasSRC.X
            miny = mVertices(0).CoodenadasSRC.Y
            minz = mVertices(0).CoodenadasSRC.Z

            For i As Integer = 1 To mVertices.GetUpperBound(0)
                If mVertices(i).CoodenadasSRC.X > maxx Then maxx = mVertices(i).CoodenadasSRC.X
                If mVertices(i).CoodenadasSRC.Y > maxy Then maxy = mVertices(i).CoodenadasSRC.Y
                If mVertices(i).CoodenadasSRC.Z > maxz Then maxz = mVertices(i).CoodenadasSRC.Z
                If mVertices(i).CoodenadasSRC.X < minx Then minx = mVertices(i).CoodenadasSRC.X
                If mVertices(i).CoodenadasSRC.Y < miny Then miny = mVertices(i).CoodenadasSRC.Y
                If mVertices(i).CoodenadasSRC.Z < minz Then minz = mVertices(i).CoodenadasSRC.Z
            Next

            Return New AABB3D(minx, miny, minz, Abs(maxx - minx), Abs(maxy - miny), Abs(maxz - minz))
        End Function

        Public Sub RecalcularAABBSUR()
            mAABBSUR = ObtenerAABBSUR()
        End Sub

        Public Sub Shading(ByVal Focos() As Foco3D, ByVal Camara As Camara3D)
            If Not Focos Is Nothing Then
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).Shading(mConstantesShading, Focos, Camara)
                Next
            Else
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).ColorShading = Color.Black
                Next
            End If
        End Sub

        Public Sub RecalcularAABBSRC()
            mAABBSRC = ObtenerAABBSRC()
        End Sub

        Public Sub RecalcularCentro()
            mCentroSUR = Vertice.BaricentroSUR(mVertices)
        End Sub

        Private Sub RecalcularDatosCaras()
            If Not NormalesCentro Then
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).NormalSUR = Cara.VectorNormalSUR(mCaras(i), mVertices)
                Next
            Else
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).NormalSUR = New Vector3D(mCentroSUR, mCaras(i).BaricentroSUR).VectorUnitario
                Next
            End If

            For i As Integer = 0 To mCaras.GetUpperBound(0)
                mCaras(i).RecalcularBaricentroSUR(mVertices)
            Next
        End Sub

        Public Sub RecalcularRepresentaciones(ByVal Camara As Camara3D)
            For i As Integer = 0 To mVertices.GetUpperBound(0)
                mVertices(i).CoodenadasSRC = Camara.TransformacionSURtoSRC * mVertices(i).CoodenadasSUR
                mVertices(i).Representacion = Camara.Proyeccion(mVertices(i).CoodenadasSRC, True)
            Next

            If NormalesCentro Then
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).RecalcularBaricentroSRC(mVertices)
                    mCaras(i).NormalSRC = New Vector3D(mCentroSRC, mCaras(i).BaricentroSRC).VectorUnitario
                Next
            Else
                For i As Integer = 0 To mCaras.GetUpperBound(0)
                    mCaras(i).RecalcularBaricentroSRC(mVertices)
                    mCaras(i).NormalSRC = Cara.VectorNormalSRC(mCaras(i), mVertices)
                Next
            End If

            mCentroSRC = Camara.TransformacionSURtoSRC * mCentroSUR

            If mAutoRecalcularAABBs Then mAABBSRC = ObtenerAABBSRC()
        End Sub

        Public Sub CalcularCarasVertices()
            Dim Caras As New List(Of Integer)

            For i As Integer = 0 To mVertices.GetUpperBound(0)
                Caras.Clear()
                For j As Integer = 0 To mCaras.GetUpperBound(0)
                    If mCaras(j).Vertices.Contains(i) Then Caras.Add(j)
                Next
                mVertices(i).EstablecerCaras(Caras.ToArray)
            Next
        End Sub

        Public Function EsVisible(ByVal Camara As Camara3D) As Boolean
            If Not Camara.EsVisible(mCentroSUR) Then
                For Each Vertice As Vertice In mVertices
                    If Not Camara.EsVisible(Vertice.CoodenadasSUR) Then
                        Return False
                    End If
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Sub New(ByVal Vertices() As Vertice, ByVal Caras() As Cara)
            If Vertices.GetUpperBound(0) >= 3 Then
                If Caras.GetUpperBound(0) >= 3 Then
                    mCaras = Caras

                    For i As Integer = 0 To mCaras.GetUpperBound(0)
                        mCaras(i).Color = Color.White
                    Next

                    mVertices = Vertices
                    mAutoRecalcularAABBs = False
                    AutoReclcNorms = True
                    RecalcularCentro()
                    CalcularCarasVertices()
                    RecalcularDatosCaras()
                    RecalcularNormalesVertices()
                    mConstantesShading = New PhongShader
                    mVertical = New Vector3D(0, 1, 0)
                Else
                    Throw New ExcepcionPrimitiva3D("POLIEDRO (NEW): Un poliedro debe tener al menos 4 caras" & vbNewLine & _
                                                   "Numero de caras=" & Vertices.GetUpperBound(0) + 1)
                End If
            Else
                Throw New ExcepcionPrimitiva3D("POLIEDRO (NEW): Un poliedro debe tener al menos 4 vertices" & vbNewLine & _
                                               "Numero de vertices=" & Vertices.GetUpperBound(0) + 1)
            End If
        End Sub

        Public Sub New() 'SOLO PARA LAS CLASES HEREDADAS!!!
        End Sub

        Public Overrides Function ToString() As String
            Return "{Poliedro de " & mCaras.GetUpperBound(0) + 1 & " y " & mVertices.GetUpperBound(0) + 1 & " vertices}"
        End Function

        Public Shared Function Cubo() As Poliedro
            Dim Vertices(7) As Vertice
            Dim Caras(5) As Cara

            Vertices(0) = New Vertice(New Punto3D(-1, -1, -1))
            Vertices(1) = New Vertice(New Punto3D(1, -1, -1))
            Vertices(2) = New Vertice(New Punto3D(1, 1, -1))
            Vertices(3) = New Vertice(New Punto3D(-1, 1, -1))
            Vertices(4) = New Vertice(New Punto3D(-1, -1, 1))
            Vertices(5) = New Vertice(New Punto3D(1, -1, 1))
            Vertices(6) = New Vertice(New Punto3D(1, 1, 1))
            Vertices(7) = New Vertice(New Punto3D(-1, 1, 1))

            Caras(0) = New Cara(3, 2, 1, 0)
            Caras(1) = New Cara(4, 5, 6, 7)
            Caras(2) = New Cara(7, 6, 2, 3)
            Caras(3) = New Cara(4, 7, 3, 0)
            Caras(4) = New Cara(5, 4, 0, 1)
            Caras(5) = New Cara(6, 5, 1, 2)

            'For i As Integer = 0 To 5
            '    Caras(i).RevertirVertices()
            'Next

            Return New Poliedro(Vertices, Caras)
        End Function

        Public Shared Function Esfera(ByVal Pasos As Integer) As Poliedro
            Dim Vertices((Pasos ^ 2) - Pasos + 1) As Vertice
            Dim Caras((Pasos ^ 2) - 1) As Cara
            Dim cont, contc As Integer
            Dim Radio As Double = 1


            For i As Integer = 0 To Pasos - 1
                Caras(i) = New Cara(3)
            Next
            For i As Integer = Pasos To (Pasos ^ 2) - Pasos - 1
                Caras(i) = New Cara(4)
            Next
            For i As Integer = (Pasos ^ 2) - Pasos To (Pasos ^ 2) - 1
                Caras(i) = New Cara(3)
            Next

            cont = 1
            contc = 0

            Vertices(0) = New Vertice(New Punto3D(0, 0, 1))
            Vertices(Vertices.GetUpperBound(0)) = New Vertice(New Punto3D(0, 0, -1))

            For a As Double = 0 To PI Step PI / (Pasos / 1)
                If a = 0 Or a = PI Then Continue For
                Radio = Sin(a)
                For b As Double = 0 To 2 * PI Step PI / (Pasos / 2)
                    If b = 2 * PI Then Continue For
                    Vertices(cont) = New Vertice(New Punto3D(Radio * Cos(b), Radio * Sin(b), Cos(a)))

                    If cont = Vertices.GetUpperBound(0) Then Exit For
                    cont += 1
                Next
                If cont = (Pasos ^ 2) - 1 Then Exit For
            Next

            cont = 1
            For i As Integer = 0 To Pasos - 1
                Caras(i).Vertices(0) = 0
                Caras(i).Vertices(1) = IIf(cont + 1 <= Pasos - 0, cont + 1, 1)
                Caras(i).Vertices(2) = cont
                If cont = Pasos Then Exit For
                cont += 1
            Next

            cont = Vertices.GetUpperBound(0) - Pasos - 1
            For i As Integer = Caras.GetUpperBound(0) - Pasos To Caras.GetUpperBound(0)
                Caras(i).Vertices(0) = cont
                Caras(i).Vertices(1) = IIf(cont + 1 < Vertices.GetUpperBound(0) - 1, cont + 1, Vertices.GetUpperBound(0) - Pasos - 1)
                Caras(i).Vertices(2) = Vertices.GetUpperBound(0)
                If cont = Vertices.GetUpperBound(0) - 1 Then Exit For
                cont += 1
            Next

            For i As Integer = 1 To Pasos - 2
                For j As Integer = 0 To Pasos - 1
                    Caras((i * Pasos) + j).Vertices(0) = ((i - 1) * Pasos) + j + 1
                    Caras((i * Pasos) + j).Vertices(1) = IIf(j + 1 <= Pasos - 1, ((i - 1) * Pasos) + j + 2, ((i - 1) * Pasos) + 1)
                    Caras((i * Pasos) + j).Vertices(2) = IIf(j + 1 <= Pasos - 1, ((i) * Pasos) + j + 2, ((i) * Pasos) + 1)
                    Caras((i * Pasos) + j).Vertices(3) = ((i) * Pasos) + j + 1
                Next
            Next

            Return New Poliedro(Vertices, Caras)
        End Function

        Public Shared Function Malla(ByVal Dimensiones As Integer, ByVal NumeroCeldas As Integer) As Poliedro
            Dim Vertices() As Vertice
            Dim Caras() As Cara

            Dim p As Double = -(Dimensiones / 2)
            Dim d As Double = (Dimensiones / NumeroCeldas)
            Dim indice As Integer

            ReDim Vertices(((NumeroCeldas + 1) ^ 2) - 1)
            ReDim Caras((NumeroCeldas ^ 2) - 1)

            For i As Integer = 0 To NumeroCeldas
                For j As Integer = 0 To NumeroCeldas
                    Vertices((i * (NumeroCeldas + 1)) + j) = New Vertice(New Punto3D(p + (d * (j)), 0, p + (d * (i))))
                Next
            Next

            For i As Integer = 0 To NumeroCeldas - 1
                For j As Integer = 0 To NumeroCeldas - 1
                    indice = (i * (NumeroCeldas)) + j
                    Caras(indice) = New Cara(indice, indice + 1, indice + NumeroCeldas + 2, indice + NumeroCeldas + 1)
                Next
            Next

            Return New Poliedro(Vertices, Caras)
        End Function

        Public Shared Operator =(ByVal P1 As Poliedro, ByVal P2 As Poliedro) As Boolean
            Return P1.Equals(P2)
        End Operator

        Public Shared Operator <>(ByVal P1 As Poliedro, ByVal P2 As Poliedro) As Boolean
            Return Not (P1 = P2)
        End Operator
    End Class
End Namespace

