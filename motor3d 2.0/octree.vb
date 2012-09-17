Imports Motor3D.Primitivas3D

Namespace Espacio3D
    Public Class Octree
        Inherits ObjetoGeometrico3D

        Private mNiveles As Integer

        Private mSectorRaiz As SectorOctree

        Public Property Espacio() As AABB3D
            Get
                Return mSectorRaiz.Espacio
            End Get
            Set(ByVal value As AABB3D)
                If mSectorRaiz.Espacio <> value Then
                    mSectorRaiz = New SectorOctree(mNiveles, value)
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
                    mSectorRaiz = New SectorOctree(mNiveles, mSectorRaiz.Espacio)
                End If
            End Set
        End Property

        Public ReadOnly Property SectorRaiz() As SectorOctree
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

        Public ReadOnly Property Pertenece(ByVal Poliedro As Poliedro, Optional ByVal AABBSUR As Boolean = False) As Boolean
            Get
                If AABBSUR Then
                    Return mSectorRaiz.Espacio.Colisionan(Poliedro.AABBSUR)
                Else
                    Return mSectorRaiz.Espacio.Colisionan(Poliedro.AABBSRC)
                End If
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

                mSectorRaiz = New SectorOctree(Niveles, AABB)
            Else
                Throw New ExcepcionGeometrica3D("OCTREE (NEW): Un octree debe tener al menos dos niveles:" & vbNewLine _
                                                & "Niveles=" & Niveles)
            End If
        End Sub

        Public Sub Refrescar()
            mSectorRaiz.Refrescar()
        End Sub

        Public Function Sector(ByVal Punto As Punto3D) As SectorOctree
            Return Sector(Me, Punto)
        End Function

        Public Function Sector(ByVal Recta As Recta3D) As SectorOctree
            Return Sector(Me, Recta)
        End Function

        Public Function Sector(ByVal AABB As AABB3D) As SectorOctree
            Return Sector(Me, AABB)
        End Function

        Public Function Sector(ByVal Poliedro As Poliedro, Optional ByVal AABBSUR As Boolean = False) As SectorOctree
            Return Sector(Me, Poliedro, AABBSUR)
        End Function

        Public Function Sectores(ByVal Recta As Recta3D) As SectorOctree()
            Return Sectores(Me, Recta)
        End Function

        Public Shared Function Sector(ByVal Octree As Octree, ByVal Punto As Punto3D) As SectorOctree
            Dim Resultado As Integer = Octree.SectorRaiz.Pertenece(Punto)
            Dim S As SectorOctree = Octree.SectorRaiz

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
                Throw New ExcepcionGeometrica3D("OCTREE (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Punto.ToString & vbNewLine _
                                                & "Espacio=" & Octree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sector(ByVal Octree As Octree, ByVal Recta As Recta3D) As SectorOctree
            Dim Resultado As Integer = Octree.SectorRaiz.Pertenece(Recta)
            Dim S As SectorOctree = Octree.SectorRaiz

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
                Throw New ExcepcionGeometrica3D("OCTREE (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Punto=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & Octree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sectores(ByVal Octree As Octree, ByVal Recta As Recta3D) As SectorOctree()
            Dim S As SectorOctree = Octree.SectorRaiz
            Dim Retorno As New List(Of SectorOctree)

            If Octree.SectorRaiz.Pertenece(Recta) Then
                InterseccionRecta(Recta, S, Retorno)

                Return Retorno.ToArray
            Else
                Throw New ExcepcionGeometrica3D("OCTREE (PERTENECE): La recta especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Recta=" & Recta.ToString & vbNewLine _
                                                & "Espacio=" & Octree.Espacio.ToString)
            End If
        End Function

        Public Shared Sub InterseccionRecta(ByVal Recta As Recta3D, ByVal Sector As SectorOctree, ByRef ListaRetorno As List(Of SectorOctree))
            If Not Sector.EsHoja Then
                For Each Hijo As SectorOctree In Sector.Hijos
                    If Hijo.Pertenece(Recta) Then
                        InterseccionRecta(Recta, Hijo, ListaRetorno)
                    End If
                Next
            Else
                If Sector.Pertenece(Recta) Then ListaRetorno.Add(Sector)
            End If
        End Sub

        Public Shared Function Sector(ByVal Octree As Octree, ByVal AABB As AABB3D) As SectorOctree
            Dim Resultado As Integer = Octree.SectorRaiz.Pertenece(AABB)
            Dim S As SectorOctree = Octree.SectorRaiz

            'PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return Octree.SectorRaiz
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
                Throw New ExcepcionGeometrica3D("OCTREE (PERTENECE): La AABB especificada no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "AABB=" & AABB.ToString & vbNewLine _
                                                & "Espacio=" & Octree.Espacio.ToString)
            End If
        End Function

        Public Shared Function Sector(ByVal Octree As Octree, ByRef Poliedro As Poliedro, Optional ByVal AABBSUR As Boolean = False) As SectorOctree
            Dim Resultado As Integer = Octree.SectorRaiz.Pertenece(Poliedro, AABBSUR)
            Dim S As SectorOctree = Octree.SectorRaiz

            'PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
            If Resultado <> -2 Then
                If Resultado = -1 Then
                    Return Octree.SectorRaiz
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
                        Resultado = S.Pertenece(Poliedro, AABBSUR)
                    Loop While Resultado <> -2 AndAlso Resultado <> -1

                    Return S
                End If
            Else
                Throw New ExcepcionGeometrica3D("OCTREE (PERTENECE): El poliedro especificado no pertenece al espacio dominado por el quadtree." & vbNewLine _
                                                & "Poliedro=" & Poliedro.ToString & vbNewLine _
                                                & "Espacio=" & Octree.Espacio.ToString)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return "{Octree de espacio=" & mSectorRaiz.Espacio.ToString & "}"
        End Function
    End Class
End Namespace


