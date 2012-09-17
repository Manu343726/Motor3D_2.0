Imports Motor3D.Espacio3D

Namespace Primitivas3D
    Public Class OctreeGrafico
        Inherits ObjetoGeometrico3D

        Private mNiveles As Integer

        Private mSectorRaiz As SectorOctreeGrafico

        Public Property Espacio() As AABB3D
            Get
                Return mSectorRaiz.Espacio
            End Get
            Set(ByVal value As AABB3D)
                If mSectorRaiz.Espacio <> value Then
                    mSectorRaiz = New SectorOctreeGrafico(mNiveles, value)
                End If
            End Set
        End Property

        Public Property Niveles() As Integer
            Get
                Return mNiveles
            End Get
            Set(ByVal value As Integer)
                If value > 0 Then
                    mNiveles = value
                    mSectorRaiz = New SectorOctreeGrafico(mNiveles, mSectorRaiz.Espacio)
                End If
            End Set
        End Property

        Public ReadOnly Property SectorRaiz() As SectorOctreeGrafico
            Get
                Return mSectorRaiz
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal Punto As Punto3D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Pertenece(Punto)
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal AABB As AABB3D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Colisionan(AABB)
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal Recta As Recta3D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Pertenece(Recta)
            End Get
        End Property

        Public Sub New(ByVal Niveles As Integer, ByVal AABB As AABB3D)
            If Niveles > 1 Then
                mNiveles = Niveles

                mSectorRaiz = New SectorOctreeGrafico(Niveles, AABB)
            Else
                Throw New ExcepcionGeometrica3D("OctreeGrafico (NEW): Un OctreeGrafico debe tener al menos dos niveles:" & vbNewLine _
                                                & "Niveles=" & Niveles)
            End If
        End Sub

        Public Function Sector(ByVal Punto As Punto3D) As SectorOctreeGrafico
            Return Sector(Me, Punto)
        End Function

        Public Function Sector(ByVal Recta As Recta3D) As SectorOctreeGrafico
            Return Sector(Me, Recta)
        End Function

        Public Function Sector(ByVal AABB As AABB3D) As SectorOctreeGrafico
            Return Sector(Me, AABB)
        End Function

        Public Function Sectores(ByVal Recta As Recta3D) As SectorOctreeGrafico()
            Return Sectores(Me, Recta)
        End Function

        Public Shared Function Sector(ByVal OctreeGrafico As OctreeGrafico, ByVal Punto As Punto3D) As SectorOctreeGrafico
            Dim Resultado As Integer = OctreeGrafico.SectorRaiz.Pertenece(Punto)
            Dim S As SectorOctreeGrafico = OctreeGrafico.SectorRaiz

            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return S
                Else
                    Do While Resultado <> -2 AndAlso Resultado <> -1
                        Resultado = S.Pertenece(Punto)
                        If Resultado <> -2 Then
                            If Resultado = -1 Then
                                Return S
                            Else
                                S = S.Hijos(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                    Loop

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Punto.ToString & vbNewLine _
                                                & "Espacio=" & OctreeGrafico.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sector(ByVal OctreeGrafico As OctreeGrafico, ByVal Recta As Recta3D) As SectorOctreeGrafico
            Dim Resultado As Integer = OctreeGrafico.SectorRaiz.Pertenece(Recta)
            Dim S As SectorOctreeGrafico = OctreeGrafico.SectorRaiz

            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return S
                Else
                    Do While Resultado <> -2 AndAlso Resultado <> -1
                        Resultado = S.Pertenece(Recta)
                        If Resultado <> -2 Then
                            If Resultado = -1 Then
                                Return S
                            Else
                                S = S.Hijos(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                    Loop

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): ELa recta especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & OctreeGrafico.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sectores(ByVal OctreeGrafico As OctreeGrafico, ByVal Recta As Recta3D) As SectorOctreeGrafico()
            Dim S As SectorOctreeGrafico = OctreeGrafico.SectorRaiz
            Dim Retorno As New List(Of SectorOctreeGrafico)

            If OctreeGrafico.SectorRaiz.Espacio.Pertenece(Recta) Then
                InterseccionRecta(Recta, S, Retorno)

                Return Retorno.ToArray
            Else
                Throw New ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Recta=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & OctreeGrafico.Espacio.ToString)
            End If
        End Function

        Public Shared Sub InterseccionRecta(ByVal Recta As Recta3D, ByVal Sector As SectorOctreeGrafico, ByRef ListaRetorno As List(Of SectorOctreeGrafico))
            If Not Sector.EsHoja Then
                For Each Hijo As SectorOctreeGrafico In Sector.Hijos
                    If Hijo.Espacio.Pertenece(Recta) Then
                        InterseccionRecta(Recta, Hijo, ListaRetorno)
                    End If
                Next
            Else
                If Sector.Espacio.Pertenece(Recta) Then ListaRetorno.Add(Sector)
            End If
        End Sub

        Public Shared Function Sector(ByVal OctreeGrafico As OctreeGrafico, ByVal AABB As AABB3D) As SectorOctreeGrafico
            Dim Resultado As Integer = OctreeGrafico.SectorRaiz.Pertenece(AABB)
            Dim S As SectorOctreeGrafico = OctreeGrafico.SectorRaiz

            'PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return OctreeGrafico.SectorRaiz
                Else
                    Do
                        If Resultado <> -2 Then
                            If Resultado = -1 Then
                                Return S
                            Else
                                S = S.Hijos(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                        Resultado = S.Pertenece(AABB)
                    Loop While Resultado <> -2 AndAlso Resultado <> -1

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): La AABB especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "AABB=" & AABB.ToString & vbNewLine _
                                                & "Espacio=" & OctreeGrafico.Espacio.ToString)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return "{OctreeGrafico de espacio=" & mSectorRaiz.Espacio.ToString & "}"
        End Function
    End Class
End Namespace
