Imports Motor3D.Primitivas3D

Namespace Espacio3D
    Public Class SectorOctree
        Inherits ObjetoGeometrico3D

        Private mEspacio As AABB3D
        Private mIndice As Integer
        Private mNivel As Integer
        Private mNiveles As Integer

        Private mPadre As SectorOctree
        Private mSubSectores() As SectorOctree

        Private mObjetos() As Poliedro

        Public ReadOnly Property Espacio() As AABB3D
            Get
                Return mEspacio
            End Get
        End Property

        Public ReadOnly Property Indice() As Integer
            Get
                Return mIndice
            End Get
        End Property

        Public ReadOnly Property Nivel() As Integer
            Get
                Return mNivel
            End Get
        End Property

        Public ReadOnly Property Niveles() As Integer
            Get
                Return mNiveles
            End Get
        End Property

        Public ReadOnly Property Padre As SectorOctree
            Get
                Return mPadre
            End Get
        End Property

        Public ReadOnly Property Hijos() As SectorOctree()
            Get
                Return mSubSectores
            End Get
        End Property

        Public ReadOnly Property EsRaiz As Boolean
            Get
                Return mNivel = 0
            End Get
        End Property

        Public ReadOnly Property EsHoja() As Boolean
            Get
                Return (mNivel = mNiveles - 1)
            End Get
        End Property

        Public ReadOnly Property Hijos(ByVal Indice As Integer) As SectorOctree
            Get
                If Indice >= 0 AndAlso Indice <= 7 Then
                    Return mSubSectores(Indice)
                Else
                    Throw New ExcepcionGeometrica3D("SECTOROCTREE (HIJOS_GET): El indice de un sector de Octree debe estar comprendido entre 0 y 3." & vbNewLine _
                                                    & "Indice=" & Indice.ToString)
                End If
            End Get
        End Property

        Public ReadOnly Property Vacio As Boolean
            Get
                Return Not mObjetos Is Nothing
            End Get
        End Property

        Public ReadOnly Property Objetos As Poliedro()
            Get
                Return mObjetos
            End Get
        End Property

        Public ReadOnly Property Colision As Boolean
            Get
                If mObjetos Is Nothing Then
                    Return False
                Else
                    Return mObjetos.GetUpperBound(0) > 0
                End If
            End Get
        End Property

        'SOLO PARA EL NODO RAIZ:
        Public Sub New(ByVal Niveles As Integer, ByVal Espacio As AABB3D)
            If Niveles > 1 Then
                mNivel = 0
                mNiveles = Niveles
                mIndice = 0
                mEspacio = Espacio
                mPadre = Nothing

                If mNivel < mNiveles - 1 Then
                    mSubSectores = ObtenerHijos(Me)
                End If
            Else
                Throw New ExcepcionGeometrica3D("SECTOROCTREE (NEW_RAIZ): Un octree necesita al menos dos niveles." & vbNewLine _
                                                & "Niveles=" & Niveles.ToString)
            End If
        End Sub

        Public Sub New(ByRef Padre As SectorOctree, ByVal Indice As Integer, ByVal AABB As AABB3D)
            If Padre.Niveles > 1 Then
                If Nivel >= 0 AndAlso Nivel < Padre.Niveles Then
                    If Indice >= 0 AndAlso Indice <= 7 Then
                        mNivel = Padre.Nivel + 1
                        mNiveles = Padre.Niveles
                        mIndice = Indice
                        mEspacio = AABB
                        mPadre = Padre

                        If mNivel < mNiveles - 1 Then
                            mSubSectores = ObtenerHijos(Me)
                        End If
                    Else
                        Throw New ExcepcionGeometrica3D("SECTOROCTREE (NEW): El indice de un sector de Octree debe estar comprendido entre 0 y 3." & vbNewLine _
                                                        & "Indice=" & Indice.ToString)
                    End If

                Else
                    Throw New ExcepcionGeometrica3D("SECTOROCTREE (NEW): El nivel de un sector de Octree debe estar entre 0 y el número de niveles del Octree menos uno." & vbNewLine _
                                                    & "Niveles del Octree=" & Niveles.ToString & vbNewLine _
                                                    & "Nivel del sector=" & Nivel.ToString)
                End If

            Else
                Throw New ExcepcionGeometrica3D("SECTOROCTREE (NEW): Un Octree debe tener al menos dos niveles." & vbNewLine _
                                                & "Niveles del Octree=" & Niveles.ToString)
            End If
        End Sub

        Public Function AñadirPoliedro(ByRef Poliedro As Poliedro) As Boolean
            If mObjetos Is Nothing Then
                ReDim mObjetos(0)
                mObjetos(0) = Poliedro
            Else
                If Not mObjetos.Contains(Poliedro) Then
                    ReDim Preserve mObjetos(mObjetos.GetUpperBound(0) + 1)
                    mObjetos(mObjetos.GetUpperBound(0)) = Poliedro
                Else
                    Return False
                End If
            End If

            Return True
        End Function

        Public Sub Refrescar()
            If Not mSubSectores Is Nothing Then
                For Each Hijo As SectorOctree In mSubSectores
                    If Not Hijo Is Nothing Then Hijo.Refrescar()
                Next
            End If
            mObjetos = Nothing
        End Sub

        Public Function Pertenece(ByRef Punto As Punto3D) As Integer
            Return Pertenece(Me, Punto)
        End Function

        Public Function Pertenece(ByRef Recta As Recta3D) As Boolean
            Return Pertenece(Me, Recta)
        End Function

        Public Function Pertenece(ByRef AABB As AABB3D) As Integer
            Return Pertenece(Me, AABB)
        End Function

        Public Function Pertenece(ByRef Poliedro As Poliedro, Optional ByVal AABBSUR As Boolean = False) As Integer
            Return Pertenece(Me, Poliedro, AABBSUR)
        End Function

        Public Shared Function ObtenerHijos(ByVal Sector As SectorOctree) As SectorOctree()
            If Sector.Nivel < Sector.Niveles - 1 Then
                Dim Retorno(7) As SectorOctree
                Dim Tamaño As New Vector3D(Sector.Espacio.Ancho / 2, Sector.Espacio.Largo / 2, Sector.Espacio.Alto / 2)

                'ABAJO:
                Retorno(0) = New SectorOctree(Sector, 0, New AABB3D(Sector.Espacio.Posicion, Tamaño))
                Retorno(1) = New SectorOctree(Sector, 1, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down), Tamaño))
                Retorno(2) = New SectorOctree(Sector, 2, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño))
                Retorno(3) = New SectorOctree(Sector, 3, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño))
                'ARRIBA:
                Retorno(4) = New SectorOctree(Sector, 4, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(5) = New SectorOctree(Sector, 5, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(6) = New SectorOctree(Sector, 6, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(7) = New SectorOctree(Sector, 7, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño))

                Return Retorno
            Else
                Throw New ExcepcionGeometrica3D("SECTOROCTREE (OBTENERHIJOS): No se pueden generar subsectores de un sector cuyo nivel es máximo." & vbNewLine _
                                                & "Niveles del Octree=" & Sector.Niveles.ToString & vbNewLine _
                                                & "Nivel del sector=" & Sector.Niveles)
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorOctree, ByRef Punto As Punto3D) As Integer
            If Not Sector.EsHoja Then
                For i As Integer = 0 To 7
                    If Sector.Hijos(i).Espacio.Pertenece(Punto) Then
                        Return i
                    End If
                Next
            Else
                If Sector.Espacio.Pertenece(Punto) Then
                    Return -1
                Else
                    Return -2
                End If
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorOctree, ByRef Recta As Recta3D) As Boolean
            Return Sector.Espacio.Pertenece(Recta)
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorOctree, ByRef AABB As AABB3D) As Integer
            If Not Sector.EsHoja AndAlso AABB.Dimensiones < Sector.Espacio.Dimensiones / 2 Then
                'LA AABB PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
                For i As Integer = 0 To 7
                    If Sector.Hijos(i).Espacio.Colisionan(AABB) Then
                        Return i
                    End If
                Next
            Else
                If AABB.Dimensiones < Sector.Espacio.Dimensiones AndAlso Sector.Espacio.Colisionan(AABB) Then
                    'LA AABB PERTENECE AL SECTOR:
                    Return -1
                Else
                    'LA AABB NO PERTENECE AL SECTOR:
                    Return -2
                End If
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorOctree, ByRef Poliedro As Poliedro, Optional AABBSUR As Boolean = False) As Integer
            If AABBSUR Then
                If Not Sector.EsHoja AndAlso Poliedro.AABBSUR.Dimensiones < Sector.Espacio.Dimensiones / 2 Then
                    'LA AABB PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
                    For i As Integer = 0 To 7
                        If Sector.Hijos(i).Espacio.Colisionan(Poliedro.AABBSUR) Then
                            Return i
                        End If
                    Next
                Else
                    If Poliedro.AABBSUR.Dimensiones < Sector.Espacio.Dimensiones AndAlso Sector.Espacio.Colisionan(Poliedro.AABBSUR) Then
                        'LA AABB PERTENECE AL SECTOR:
                        Sector.AñadirPoliedro(Poliedro)
                        Return -1
                    Else
                        'LA AABB NO PERTENECE AL SECTOR:
                        Return -2
                    End If
                End If
            Else
                If Not Sector.EsHoja AndAlso Poliedro.AABBSRC.Dimensiones < Sector.Espacio.Dimensiones / 2 Then
                    'LA AABB PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
                    For i As Integer = 0 To 7
                        If Sector.Hijos(i).Espacio.Colisionan(Poliedro.AABBSRC) Then
                            Return i
                        End If
                    Next
                Else
                    If Poliedro.AABBSRC.Dimensiones < Sector.Espacio.Dimensiones AndAlso Sector.Espacio.Colisionan(Poliedro.AABBSRC) Then
                        'LA AABB PERTENECE AL SECTOR:
                        Sector.AñadirPoliedro(Poliedro)
                        Return -1
                    Else
                        'LA AABB NO PERTENECE AL SECTOR:
                        Return -2
                    End If
                End If
            End If

        End Function
    End Class
End Namespace
