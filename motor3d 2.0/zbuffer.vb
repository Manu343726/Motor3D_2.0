Imports Motor3D.Primitivas3D
Imports Motor3D.Utilidades
Imports Motor3D.Espacio2D
Imports System.Drawing

Namespace Escena
    Public Class ZBuffer
        Inherits ObjetoEscena

        Public Shadows Event Modificado(ByRef Sender As ZBuffer)

        Private mObjetos() As ElementoZBuffer
        Private mRepresentaciones() As Poligono2D
        Private mNumeroIndices As Integer
        Private mTiempo As Double

        Public ReadOnly Property Objetos As ElementoZBuffer()
            Get
                Return mObjetos
            End Get
        End Property

        Public ReadOnly Property NumeroObjetos As Integer
            Get
                If Not mObjetos Is Nothing Then
                    Return mObjetos.GetUpperBound(0) + 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property Represenatciones As Poligono2D()
            Get
                Return mRepresentaciones
            End Get
        End Property

        Public ReadOnly Property Vacio As Boolean
            Get
                Return mObjetos Is Nothing
            End Get
        End Property

        Public ReadOnly Property TiempoUltimoCalculo As Double
            Get
                Return mTiempo
            End Get
        End Property

        Public Sub Actualizar(ByRef Poliedros() As Poliedro, ByRef Camara As Camara3D)
            Dim Objetos As New List(Of ElementoZBuffer)
            Dim Representaciones As New List(Of Poligono2D)
            Dim T As Date = Now
            Dim n As Integer = 0
            If Not Poliedros Is Nothing Then
                If Not Vacio Then
                    For i As Integer = 0 To mObjetos.GetUpperBound(0)
                        If mObjetos(i).Indices(1) <= Poliedros.GetUpperBound(0) AndAlso mObjetos(i).Indices(2) < Poliedros(mObjetos(i).Indices(1)).NumeroCaras Then
                            If Poliedros(mObjetos(i).Indices(1)).CaraVisible(mObjetos(i).Indices(2), Camara) Then
                                Objetos.Add(New ElementoZBuffer(Poliedros(mObjetos(i).Indices(1)).Caras(mObjetos(i).Indices(2)).BaricentroSRC.Z, n, mObjetos(i).Indices(1), mObjetos(i).Indices(2)))
                                Representaciones.Add(New Poligono2D(Poliedros(mObjetos(i).Indices(1)).Caras(mObjetos(i).Indices(2)).Representacion(Poliedros(mObjetos(i).Indices(1)).Vertices)))
                                Representaciones(Representaciones.Count - 1).Color = Poliedros(mObjetos(i).Indices(1)).Caras(mObjetos(i).Indices(2)).Color
                                Poliedros(mObjetos(i).Indices(1)).Caras(mObjetos(i).Indices(2)).CargadaEnBuffer = True

                                n += 1
                            Else
                                Poliedros(mObjetos(i).Indices(1)).Caras(mObjetos(i).Indices(2)).CargadaEnBuffer = False
                            End If
                        End If
                    Next
                End If

                For i As Integer = 0 To Poliedros.GetUpperBound(0)
                    For j As Integer = 0 To Poliedros(i).NumeroCaras - 1
                        If Not Poliedros(i).Caras(j).CargadaEnBuffer Then
                            If Poliedros(i).CaraVisible(j, Camara) Then
                                Objetos.Add(New ElementoZBuffer(Poliedros(i).Caras(j).BaricentroSRC.Z, n, i, j))
                                Representaciones.Add(Poliedros(i).Representacion(j))
                                Poliedros(i).Caras(j).CargadaEnBuffer = True

                                n += 1
                            End If
                        End If

                    Next
                Next

                mObjetos = Objetos.ToArray
                mRepresentaciones = Representaciones.ToArray
                Reordenar()
            Else
                mObjetos = Nothing
                mRepresentaciones = Nothing
            End If

            mTiempo = (Now - T).TotalMilliseconds

            RaiseEvent Modificado(Me)
        End Sub

        Public Sub Shading(ByVal IndiceRepresentacion As Integer, ByVal ColorShading As Color)
            If IndiceRepresentacion >= 0 AndAlso IndiceRepresentacion <= mRepresentaciones.GetUpperBound(0) Then
                mRepresentaciones(IndiceRepresentacion).Color = ColorShading
            End If
        End Sub

        Public Sub Reordenar()
            If Not Vacio Then
                Ordenamiento.Sort(Of ElementoZBuffer)(mObjetos)
            End If
        End Sub

        Public Function Pertenece(ByVal ParamArray Indices() As Double) As Integer
            If Not mObjetos Is Nothing Then
                For i As Integer = 0 To mObjetos.GetUpperBound(0)
                    If mObjetos(i).EsEquivalente(Indices) Then Return i
                Next

                Return -1
            Else
                Return -1
            End If
        End Function

        Public Function Pertenece(ByVal Indice As Integer, ByVal ValorIndice As Double) As Integer
            If Not mObjetos Is Nothing Then
                For i As Integer = 0 To mObjetos.GetUpperBound(0)
                    If Indice < mObjetos(i).NumeroIndices Then
                        If mObjetos(i).Indices(Indice) = ValorIndice Then Return i
                    End If
                Next

                Return -1
            Else
                Return -1
            End If
        End Function

        Public Sub New()
            MyBase.New()

            mTiempo = 0

        End Sub
    End Class
End Namespace

