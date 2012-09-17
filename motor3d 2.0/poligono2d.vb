Imports System.Drawing
Imports System.Math

Namespace Espacio2D
    Public Class VerticeRecorte
        Public Vertice As Punto2D
        Public EsInterseccion As Boolean
        Public Visitado As Boolean

        Public IndiceP1 As Integer
        Public IndiceP2 As Integer

        Public SiguienteEnRecorte As VerticeRecorte
        Public SiguienteEnRecortado As VerticeRecorte

        Public ReadOnly Property ElementoEnRecorte(ByVal Indice As Integer) As VerticeRecorte
            Get
                Return ObtenerElementoEnRecorte(Indice, 0)
            End Get
        End Property

        Public ReadOnly Property ElementoEnRecortado(ByVal Indice As Integer) As VerticeRecorte
            Get
                Return ObtenerElementoEnRecortado(Indice, 0)
            End Get
        End Property

        Public Sub New(ByRef ValVertice As Punto2D, Optional ByVal ValEsInterseccion As Boolean = False, Optional ByVal ValIndiceP1 As Integer = -1, Optional ByVal ValIndiceP2 As Integer = -1)
            Vertice = ValVertice
            EsInterseccion = ValEsInterseccion
            Visitado = False
            SiguienteEnRecortado = Nothing
            SiguienteEnRecorte = Nothing
            IndiceP1 = ValIndiceP1
            IndiceP2 = ValIndiceP2
        End Sub

        Protected Friend Function ObtenerElementoEnRecorte(ByVal Indice As Integer, ByVal Procesados As Integer) As VerticeRecorte
            Dim Actual As VerticeRecorte = Me

            If Procesados < Indice AndAlso Not Me.SiguienteEnRecorte Is Nothing Then
                While Actual.EsInterseccion AndAlso Not Actual.SiguienteEnRecorte Is Nothing
                    Actual = Actual.SiguienteEnRecorte
                End While

                Return ObtenerElementoEnRecorte(Indice, Procesados + 1)
            Else
                Return Me
            End If
        End Function

        Protected Friend Function ObtenerElementoEnRecortado(ByVal Indice As Integer, ByVal Procesados As Integer) As VerticeRecorte
            Dim Actual As VerticeRecorte = Me

            If Procesados < Indice AndAlso Not Me.SiguienteEnRecortado Is Nothing Then
                While Actual.EsInterseccion AndAlso Not Actual.SiguienteEnRecortado Is Nothing
                    Actual = Actual.SiguienteEnRecortado
                End While

                Return ObtenerElementoEnRecortado(Indice, Procesados + 1)
            Else
                Return Me
            End If
        End Function

        Public Shared Sub Visitar(ByRef Vertice As VerticeRecorte)
            Vertice.Visitado = True
        End Sub

        Public Overrides Function ToString() As String
            Return "(" & Vertice.ToString & " " & IIf(EsInterseccion, "INTERSECCION", "VERTICE") & "," & IIf(Visitado, "VISITADO", "NO_VISITADO") & ")"
        End Function
    End Class

    Public Class Poligono2D
        Inherits ObjetoGeometrico2D

        Public Shadows Event Modificado(ByRef Sender As Poligono2D)

        Protected Segmentos As List(Of Segmento2D)
        Protected mColor As Color
        Protected mAABB As AABB2D

        Public ReadOnly Property Lados() As Segmento2D()
            Get
                Return Segmentos.ToArray
            End Get
        End Property

        Public ReadOnly Property NumeroLados As Integer
            Get
                Return Segmentos.Count
            End Get
        End Property

        Public ReadOnly Property Vertices() As Punto2D()
            Get
                Dim Retorno(Segmentos.Count - 1) As Punto2D

                For i As Integer = 0 To Segmentos.Count - 1
                    Retorno(i) = Segmentos(i).ExtremoInicial
                Next

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property AABB() As AABB2D
            Get
                If Not mAABB Is Nothing Then
                    Return mAABB
                Else
                    mAABB = ObtenerAABB()
                    Return mAABB
                End If
            End Get
        End Property

        Public ReadOnly Property VerticesToPoint(Optional ByVal YInvertida As Boolean = False) As Point()
            Get
                Dim Retorno(Segmentos.Count - 1) As Point

                For i As Integer = 0 To Segmentos.Count - 1
                    Retorno(i) = Segmentos(i).ExtremoInicial.ToPoint
                    If YInvertida Then Retorno(i).Y *= -1
                Next

                Return Retorno
            End Get
        End Property

        Public Property Color() As Color
            Get
                Return mColor
            End Get
            Set(ByVal value As Color)
                mColor = value
            End Set
        End Property

        Public Function ObtenerAABB() As AABB2D
            Dim maxx, maxy, minx, miny As Double

            maxx = Segmentos(0).ExtremoInicial.X
            maxy = Segmentos(0).ExtremoInicial.Y
            minx = Segmentos(0).ExtremoInicial.X
            miny = Segmentos(0).ExtremoInicial.Y

            For i As Integer = 1 To Segmentos.Count - 1
                If maxx < Segmentos(i).ExtremoInicial.X Then maxx = Segmentos(i).ExtremoInicial.X
                If maxy < Segmentos(i).ExtremoInicial.Y Then maxy = Segmentos(i).ExtremoInicial.Y
                If minx > Segmentos(i).ExtremoInicial.X Then minx = Segmentos(i).ExtremoInicial.X
                If miny > Segmentos(i).ExtremoInicial.Y Then miny = Segmentos(i).ExtremoInicial.Y
            Next

            Return New AABB2D(minx, miny, Abs(maxx - minx), Abs(maxy - miny))
        End Function

        Public Sub New(ByVal ParamArray Vertices() As Punto2D)
            Segmentos = New List(Of Segmento2D)

            If Vertices.GetUpperBound(0) > 1 Then
                For i As Integer = 0 To Vertices.GetUpperBound(0)
                    If i < Vertices.GetUpperBound(0) Then
                        Segmentos.Add(New Segmento2D(Vertices(i), Vertices(i + 1)))
                    Else
                        Segmentos.Add(New Segmento2D(Vertices(i), Vertices(0)))
                    End If
                Next

                mColor = Drawing.Color.White
            End If
        End Sub

        Public Sub New(ByVal ParamArray Lados() As Segmento2D)
            Segmentos = New List(Of Segmento2D)

            If Lados.GetUpperBound(0) > 1 Then
                Segmentos.AddRange(Lados)
                mColor = Color.Red
            End If
        End Sub

        Public Overridable Sub DividirLado(ByVal Lado As Integer, ByRef Vertice As Punto2D)
            If Lado >= 0 AndAlso Lado <= Segmentos.Count - 1 Then
                Segmentos(Lado).ExtremoFinal = Vertice
                If Lado < Segmentos.Count - 1 Then
                    Segmentos.Insert(Lado + 1, New Segmento2D(Vertice, Segmentos(Lado + 1).ExtremoInicial))
                Else
                    Segmentos.Insert(Lado + 1, New Segmento2D(Vertice, Segmentos(0).ExtremoInicial))
                End If

            End If
        End Sub

        Public Overridable Sub EstablecerVertices(ByVal ParamArray Vertices() As Punto2D)
            If Vertices.GetUpperBound(0) > 1 Then
                Segmentos = New List(Of Segmento2D)

                For i As Integer = 0 To Vertices.GetUpperBound(0)
                    If i < Vertices.GetUpperBound(0) Then
                        Segmentos.Add(New Segmento2D(Vertices(i), Vertices(i + 1)))
                    Else
                        Segmentos.Add(New Segmento2D(Vertices(i), Vertices(0)))
                    End If
                Next
                mAABB = ObtenerAABB()

                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Function Pertenece(ByRef Punto As Punto2D) As Boolean
            If mAABB.Pertenece(Punto) Then
                Dim Fase1 As New List(Of Integer)
                Dim Recta As New Recta2D(Punto, Segmentos(0).PuntoMedio)
                Dim Interseccion As Punto2D
                Dim Total As Integer = 1
                Dim PosicionRelativa As PosicionRelativa2D

                'NOTA: Por la definición de la recta, ya sabemos que Segmentos(0) interseca
                For i As Integer = 1 To Segmentos.Count - 1
                    If New Vector2D(Punto, Segmentos(i).PuntoMedio) * Recta.VectorDirector >= 0 AndAlso Segmentos(i).AABB.Pertenece(Recta) Then
                        Fase1.Add(i)
                    End If
                Next

                For i As Integer = 0 To Fase1.Count - 1
                    PosicionRelativa = Recta2D.PosicionRelativa(Recta, Segmentos(Fase1(i)).Recta)

                    If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante Then
                        Interseccion = PosicionRelativa.Interseccion

                        'NOTA: Si los vectores ExtremoIniciañ -> Interseccion y ExtremoFinal -> Interseccion están enfrentados, la interseccion pertenece al segmento:
                        If ((Interseccion.X - Segmentos(Fase1(i)).ExtremoInicial.X) * (Interseccion.X - Segmentos(Fase1(i)).ExtremoFinal.X)) + ((Interseccion.Y - Segmentos(Fase1(i)).ExtremoInicial.Y) * (Interseccion.Y - Segmentos(Fase1(i)).ExtremoFinal.Y)) < 0 Then
                            Total += 1
                        End If
                    End If
                Next

                Return Total Mod 2 <> 0
            Else
                Return False
            End If
        End Function

        Public Shared Function Particion(ByRef Poligono As Poligono2D, ByRef Recta As Recta2D) As Poligono2D()
            Dim Retono(1) As Poligono2D
            Dim VertsA As New List(Of Punto2D)
            Dim VertsB As New List(Of Punto2D)
            Dim Verts As New List(Of Punto2D)
            Dim PosicionRelativa As PosicionRelativa2D
            Dim Intersecciones As New List(Of Integer)
            Dim PosicionRelativaCentroide As Double = Recta.PosicionRelativa(Punto2D.Baricentro(Poligono.Vertices))

            For i As Integer = 0 To Poligono.NumeroLados - 1
                Verts.Add(Poligono.Lados(i).ExtremoInicial)

                If Poligono.Lados(i).AABB.Pertenece(Recta) Then
                    PosicionRelativa = Recta2D.PosicionRelativa(Poligono.Lados(i).Recta, Recta)
                    If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante Then
                        If Poligono.Lados(i).AABB.Pertenece(PosicionRelativa.Interseccion) Then
                            Verts.Add(PosicionRelativa.Interseccion)
                            Intersecciones.Add(Verts.Count - 1)
                        End If
                    End If
                End If
            Next

            For i As Integer = 0 To Verts.Count - 1
                If Not Intersecciones.Contains(i) Then
                    If PosicionRelativaCentroide > 0 Then
                        Select Case Recta.PosicionRelativa(Verts(i))
                            Case Is > 0
                                VertsA.Add(Verts(i))
                            Case Is = 0
                                VertsA.Add(Verts(i))
                                VertsB.Add(Verts(i))
                            Case Else
                                VertsB.Add(Verts(i))
                        End Select
                    Else
                        Select Case Recta.PosicionRelativa(Verts(i))
                            Case Is < 0
                                VertsA.Add(Verts(i))
                            Case Is = 0
                                VertsA.Add(Verts(i))
                                VertsB.Add(Verts(i))
                            Case Else
                                VertsB.Add(Verts(i))
                        End Select
                    End If

                Else
                    VertsA.Add(Verts(i))
                    VertsB.Add(Verts(i))
                End If
            Next

            If VertsA.Count >= 3 Then Retono(0) = New Poligono2D(VertsA.ToArray) Else Retono(0) = Nothing
            If VertsB.Count >= 3 Then Retono(1) = New Poligono2D(VertsB.ToArray) Else Retono(1) = Nothing

            Return Retono
        End Function

        Public Shared Function PuntosInterseccion(ByRef P1 As Poligono2D, ByRef P2 As Poligono2D) As Punto2D()
            Dim Retorno As New List(Of Punto2D)
            Dim PosicionRelativa As PosicionRelativa2D

            If AABB2D.Colision(P1.AABB, P2.AABB) Then
                For i As Integer = 0 To P1.NumeroLados - 1
                    For j As Integer = 0 To P2.NumeroLados - 1
                        PosicionRelativa = Recta2D.PosicionRelativa(P1.Lados(i).Recta, P2.Lados(j).Recta)
                        If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante AndAlso P1.Lados(i).AABB.Pertenece(PosicionRelativa.Interseccion) AndAlso P2.Lados(j).AABB.Pertenece(PosicionRelativa.Interseccion) Then
                            Retorno.Add(PosicionRelativa.Interseccion)
                        End If
                    Next
                Next
            End If

            Return Retorno.ToArray
        End Function

        Public Shared Function Recorte(ByRef P1 As Poligono2D, ByRef P2 As Poligono2D) As Poligono2D
            Dim Retorno As New List(Of Punto2D)
            Dim Actual As VerticeRecorte
            Dim Interseccion As VerticeRecorte
            Dim L1 As VerticeRecorte
            Dim L2 As VerticeRecorte
            Dim PosicionRelativa As PosicionRelativa2D
            Dim SobreRecorte As Boolean
            Dim SinIntersecciones As Boolean = True

            Dim A1(P1.NumeroLados - 1) As NodoArbolSegmentos
            Dim A2(P2.NumeroLados - 1) As NodoArbolSegmentos

            If AABB2D.Colision(P1.AABB, P2.AABB) Then
                L1 = New VerticeRecorte(P1.Lados(0).ExtremoInicial)
                L2 = New VerticeRecorte(P2.Lados(0).ExtremoInicial)

                Actual = L1
                For i As Integer = 0 To P1.NumeroLados - 1
                    Actual.SiguienteEnRecorte = New VerticeRecorte(P1.Lados(i).ExtremoFinal)
                    A1(i) = New NodoArbolSegmentos(P1.Lados(i), Actual, Actual.SiguienteEnRecorte)
                    Actual = Actual.SiguienteEnRecorte
                Next
                Actual.SiguienteEnRecorte = L1

                Actual = L2
                For i As Integer = 0 To P2.NumeroLados - 1
                    Actual.SiguienteEnRecortado = New VerticeRecorte(P2.Lados(i).ExtremoFinal)
                    A2(i) = New NodoArbolSegmentos(P2.Lados(i), Actual, Actual.SiguienteEnRecortado)
                    Actual = Actual.SiguienteEnRecortado
                Next
                Actual.SiguienteEnRecortado = L2

                For i As Integer = 0 To P1.NumeroLados - 1
                    For j As Integer = 0 To P2.NumeroLados - 1
                        PosicionRelativa = Recta2D.PosicionRelativa(P1.Lados(i).Recta, P2.Lados(j).Recta)

                        If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante AndAlso P1.Lados(i).AABB.Pertenece(PosicionRelativa.Interseccion) AndAlso P2.Lados(j).AABB.Pertenece(PosicionRelativa.Interseccion) Then
                            Interseccion = New VerticeRecorte(PosicionRelativa.Interseccion, True, i, j)
                            A1(i).InsertarInterseccion(Interseccion, True)
                            A2(j).InsertarInterseccion(Interseccion, False)
                            SinIntersecciones = False
                        End If
                    Next
                Next

                Actual = L2
                If Not SinIntersecciones Then
                    While Not Actual Is Nothing AndAlso Not Actual.EsInterseccion
                        Actual = Actual.SiguienteEnRecortado
                    End While

                    If Not Actual Is Nothing Then SobreRecorte = Not P1.Pertenece(P2.Lados(0).ExtremoInicial)

                    While Not Actual Is Nothing AndAlso Not Actual.Visitado
                        Retorno.Add(Actual.Vertice)
                        Actual.Visitado = True
                        If SobreRecorte Then
                            If Actual.EsInterseccion Then
                                Actual = Actual.SiguienteEnRecortado
                                SobreRecorte = False
                            Else
                                Actual = Actual.SiguienteEnRecorte
                            End If
                        Else
                            If Actual.EsInterseccion Then
                                Actual = Actual.SiguienteEnRecorte
                                SobreRecorte = True
                            Else
                                Actual = Actual.SiguienteEnRecortado
                            End If
                        End If
                    End While
                End If
            End If

            Return New Poligono2D(Retorno.ToArray)
        End Function

        Public Shared Function PoligonosAlgoritmoRecorte(ByRef P1 As Poligono2D, ByRef P2 As Poligono2D) As Poligono2D()
            Dim Retorno(1) As List(Of Punto2D)
            Dim PoliRetorno(1) As Poligono2D
            Dim Actual As VerticeRecorte
            Dim Interseccion As VerticeRecorte
            Dim LV1(P1.NumeroLados - 1) As VerticeRecorte
            Dim LV2(P2.NumeroLados - 1) As VerticeRecorte
            Dim L1 As VerticeRecorte
            Dim L2 As VerticeRecorte
            Dim PosicionRelativa As PosicionRelativa2D

            Retorno(0) = New List(Of Punto2D)
            Retorno(1) = New List(Of Punto2D)

            If AABB2D.Colision(P1.AABB, P2.AABB) Then
                L1 = New VerticeRecorte(P1.Lados(0).ExtremoInicial)
                L2 = New VerticeRecorte(P2.Lados(0).ExtremoInicial)

                Actual = L1
                LV1(0) = L1
                For i As Integer = 1 To P1.NumeroLados - 1
                    Actual.SiguienteEnRecorte = New VerticeRecorte(P1.Lados(i).ExtremoInicial)
                    LV1(i) = Actual.SiguienteEnRecorte
                    Actual = Actual.SiguienteEnRecorte
                Next

                Actual = L2
                LV2(0) = L2
                For i As Integer = 1 To P2.NumeroLados - 1
                    Actual.SiguienteEnRecortado = New VerticeRecorte(P2.Lados(i).ExtremoInicial)
                    LV2(i) = Actual.SiguienteEnRecortado
                    Actual = Actual.SiguienteEnRecortado
                Next

                For i As Integer = 0 To P1.NumeroLados - 1
                    For j As Integer = 0 To P2.NumeroLados - 1
                        PosicionRelativa = Recta2D.PosicionRelativa(P1.Lados(i).Recta, P2.Lados(j).Recta)

                        If PosicionRelativa.Tipo = TipoPosicionRelativa2D.Secante AndAlso P1.Lados(i).AABB.Pertenece(PosicionRelativa.Interseccion) AndAlso P2.Lados(j).AABB.Pertenece(PosicionRelativa.Interseccion) Then
                            Interseccion = New VerticeRecorte(PosicionRelativa.Interseccion, True)

                            Interseccion.SiguienteEnRecorte = LV1(i).SiguienteEnRecorte
                            Interseccion.SiguienteEnRecortado = LV2(j).SiguienteEnRecortado

                            LV1(i).SiguienteEnRecorte = Interseccion
                            LV2(j).SiguienteEnRecortado = Interseccion
                        End If
                    Next
                Next

                Actual = L1
                While Not Actual Is Nothing
                    Retorno(0).Add(Actual.Vertice)
                    Actual = Actual.SiguienteEnRecorte
                End While

                Actual = L2
                While Not Actual Is Nothing
                    Retorno(1).Add(Actual.Vertice)
                    Actual = Actual.SiguienteEnRecortado
                End While
            End If

            PoliRetorno(0) = New Poligono2D(Retorno(0).ToArray)
            PoliRetorno(1) = New Poligono2D(Retorno(1).ToArray)

            Return PoliRetorno
        End Function

        Private Function Insertar(ByRef Poligono As Poligono2D, ByRef Intersecciones() As Punto2D, ByVal EsRecorte As Boolean) As VerticeRecorte()
            Dim Retorno As New List(Of VerticeRecorte)

            If EsRecorte Then

            End If
        End Function

        Public Overrides Function ToString() As String
            Return "{Poligono bidimensional de " & Segmentos.Count & " lados}"
        End Function
    End Class
End Namespace

