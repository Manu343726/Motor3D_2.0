
Namespace Espacio2D
    Public Class Quadtree
        Inherits ObjetoGeometrico2D

        Private mNiveles As Integer

        Private mSectorRaiz As SectorQuadtree

        Public Property Espacio() As AABB2D
            Get
                Return mSectorRaiz.Espacio
            End Get
            Set(ByVal value As AABB2D)
                If mSectorRaiz.Espacio <> value Then
                    mSectorRaiz = New SectorQuadtree(mNiveles, value)
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
                    mSectorRaiz = New SectorQuadtree(mNiveles, mSectorRaiz.Espacio)
                End If
            End Set
        End Property

        Public ReadOnly Property SectorRaiz() As SectorQuadtree
            Get
                Return mSectorRaiz
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal Punto As Punto2D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Pertenece(Punto)
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal AABB As AABB2D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Colision(AABB)
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal Figura2D As Figura2D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Colision(Figura2D.AABB)
            End Get
        End Property

        Public ReadOnly Property Pertenece(ByVal Recta As Recta2D) As Boolean
            Get
                Return mSectorRaiz.Espacio.Pertenece(Recta)
            End Get
        End Property

        Public Sub New(ByVal Niveles As Integer, ByVal AABB As AABB2D)
            If Niveles > 1 Then
                mNiveles = Niveles

                mSectorRaiz = New SectorQuadtree(Niveles, AABB)
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (NEW): Un octree debe tener al menos dos niveles:" & vbNewLine _
                                                & "Niveles=" & Niveles)
            End If
        End Sub

        Public Sub Refrescar()
            mSectorRaiz.Refrescar()
        End Sub

        Public Function Sector(ByVal Punto As Punto2D) As SectorQuadtree
            Return Sector(Me, Punto)
        End Function

        Public Function Sector(ByVal Recta As Recta2D) As SectorQuadtree
            Return Sector(Me, Recta)
        End Function

        Public Function Sector(ByVal AABB As AABB2D) As SectorQuadtree
            Return Sector(Me, AABB)
        End Function

        Public Function Sector(ByVal Figura2D As Figura2D) As SectorQuadtree
            Return Sector(Me, Figura2D)
        End Function

        Public Function Sectores(ByVal Recta As Recta2D) As SectorQuadtree()
            Return Sectores(Me, Recta)
        End Function

        Public Shared Function Sector(ByVal Quadtree As Quadtree, ByVal Punto As Punto2D) As SectorQuadtree
            Dim Resultado As Integer = Quadtree.SectorRaiz.Pertenece(Punto)
            Dim S As SectorQuadtree = Quadtree.SectorRaiz

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
                                S = S.SubSectores(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                    Loop

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Punto.ToString & vbNewLine _
                                                & "Espacio=" & Quadtree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sector(ByVal Quadtree As Quadtree, ByVal Recta As Recta2D) As SectorQuadtree
            Dim Resultado As Integer
            Dim S As SectorQuadtree = Quadtree.SectorRaiz

            Resultado = Quadtree.SectorRaiz.Pertenece(Recta)

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
                                S = S.SubSectores(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                    Loop

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & Quadtree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sectores(ByVal Quadtree As Quadtree, ByVal Recta As Recta2D) As SectorQuadtree()
            Dim S As SectorQuadtree = Quadtree.SectorRaiz
            Dim Retorno As New List(Of SectorQuadtree)

            If Quadtree.SectorRaiz.Espacio.Pertenece(Recta) Then
                InterseccionRecta(Recta, S, Retorno)

                Return Retorno.ToArray
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (PERTENECE): La recta especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Recta=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & Quadtree.Espacio.ToString)
            End If
        End Function

        Public Shared Sub InterseccionRecta(ByVal Recta As Recta2D, ByVal Sector As SectorQuadtree, ByRef ListaRetorno As List(Of SectorQuadtree))
            If Not Sector.EsHoja Then
                For Each Hijo As SectorQuadtree In Sector.SubSectores
                    If Hijo.Espacio.Pertenece(Recta) Then
                        InterseccionRecta(Recta, Hijo, ListaRetorno)
                    End If
                Next
            Else
                If Sector.Espacio.Pertenece(Recta) Then ListaRetorno.Add(Sector)
            End If
        End Sub

        Public Shared Function Sector(ByVal Quadtree As Quadtree, ByVal AABB As AABB2D) As SectorQuadtree
            Dim Resultado As Integer = Quadtree.SectorRaiz.Pertenece(AABB)
            Dim S As SectorQuadtree = Quadtree.SectorRaiz

            'PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return Quadtree.SectorRaiz
                Else
                    Do
                        If Resultado <> -2 Then
                            If Resultado = -1 Then
                                Return S
                            Else
                                S = S.SubSectores(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                        Resultado = S.Pertenece(AABB)
                    Loop While Resultado <> -2 AndAlso Resultado <> -1

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (PERTENECE): La AABB especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "AABB=" & AABB.ToString & vbNewLine _
                                                & "Espacio=" & Quadtree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sector(ByVal Quadtree As Quadtree, ByRef Figura2D As Figura2D) As SectorQuadtree
            Dim Resultado As Integer = Quadtree.SectorRaiz.Pertenece(Figura2D)
            Dim S As SectorQuadtree = Quadtree.SectorRaiz

            'PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return Quadtree.SectorRaiz
                Else
                    Do
                        If Resultado <> -2 Then
                            If Resultado = -1 Then
                                Return S
                            Else
                                S = S.SubSectores(Resultado)
                            End If
                        Else
                            Return S.Padre
                        End If
                        Resultado = S.Pertenece(Figura2D)
                    Loop While Resultado <> -2 AndAlso Resultado <> -1

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica2D("OCTREE (PERTENECE): La figura especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Figura2D=" & Figura2D.ToString & vbNewLine _
                                                & "Espacio=" & Quadtree.Espacio.ToString)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return "{Quadtree de espacio=" & mSectorRaiz.Espacio.ToString & "}"
        End Function
    End Class
End Namespace


