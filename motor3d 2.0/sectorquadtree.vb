Imports Motor2D.Primitivas2D

Namespace Espacio2D
    Public Class SectorQuadtree
        Inherits ObjetoGeometrico2D

        Private mEspacio As AABB2D
        Private mIndice As Integer
        Private mNivel As Integer
        Private mNiveles As Integer

        Private mPadre As SectorQuadtree
        Private mSubSectores() As SectorQuadtree

        Private mObjetos() As Figura2D

        Public ReadOnly Property Espacio() As AABB2D
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

        Public ReadOnly Property Padre As SectorQuadtree
            Get
                Return mPadre
            End Get
        End Property

        Public ReadOnly Property SubSectores() As SectorQuadtree()
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

        Public ReadOnly Property SubSectores(ByVal Indice As Integer) As SectorQuadtree
            Get
                If Indice >= 0 AndAlso Indice <= 7 Then
                    Return mSubSectores(Indice)
                Else
                    Throw New ExcepcionGeometrica2D("SECTORQuadtree (NEW): El indice de un sector de Quadtree debe estar comprendido entre 0 y 3." & vbNewLine _
                                                    & "Indice=" & Indice.ToString)
                End If
            End Get
        End Property

        Public ReadOnly Property Vacio As Boolean
            Get
                Return Not mObjetos Is Nothing
            End Get
        End Property

        Public ReadOnly Property Objetos As Figura2D()
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
        Public Sub New(ByVal Niveles As Integer, ByVal Espacio As AABB2D)
            If Niveles > 1 Then
                mNivel = 0
                mNiveles = Niveles
                mIndice = 0
                mEspacio = Espacio
                mPadre = Nothing

                If mNivel < mNiveles - 1 Then
                    mSubSectores = ObtenerSubSectores(Me)
                End If
            Else
                Throw New ExcepcionGeometrica2D("SECTORQUADTREE (NEW_RAIZ): Un octree necesita al menos dos niveles." & vbNewLine _
                                                & "Niveles=" & Niveles.ToString)
            End If
        End Sub

        Public Sub New(ByRef Padre As SectorQuadtree, ByVal Indice As Integer, ByVal AABB As AABB2D)
            If Padre.Niveles > 1 Then
                If Nivel >= 0 AndAlso Nivel < Padre.Niveles Then
                    If Indice >= 0 AndAlso Indice <= 3 Then
                        mNivel = Padre.Nivel + 1
                        mNiveles = Padre.Niveles
                        mIndice = Indice
                        mEspacio = AABB
                        mPadre = Padre

                        If mNivel < mNiveles - 1 Then
                            mSubSectores = ObtenerSubSectores(Me)
                        End If
                    Else
                        Throw New ExcepcionGeometrica2D("SECTORQUADTREE (NEW): El indice de un sector de Quadtree debe estar comprendido entre 0 y 3." & vbNewLine _
                                                        & "Indice=" & Indice.ToString)
                    End If

                Else
                    Throw New ExcepcionGeometrica2D("SECTORQUADTREE (NEW): El nivel de un sector de Quadtree debe estar entre 0 y el número de niveles del Quadtree menos uno." & vbNewLine _
                                                    & "Niveles del Quadtree=" & Niveles.ToString & vbNewLine _
                                                    & "Nivel del sector=" & Nivel.ToString)
                End If

            Else
                Throw New ExcepcionGeometrica2D("SECTORQUADTREE (NEW): Un Quadtree debe tener al menos dos niveles." & vbNewLine _
                                                & "Niveles del Quadtree=" & Niveles.ToString)
            End If
        End Sub

        Public Function AñadirFigura2D(ByRef Figura2D As Figura2D) As Boolean
            If mObjetos Is Nothing Then
                ReDim mObjetos(0)
                mObjetos(0) = Figura2D
            Else
                If Not mObjetos.Contains(Figura2D) Then
                    ReDim Preserve mObjetos(mObjetos.GetUpperBound(0) + 1)
                    mObjetos(mObjetos.GetUpperBound(0)) = Figura2D
                Else
                    Return False
                End If
            End If

            Return True
        End Function

        Public Sub Refrescar()
            If Not mSubSectores Is Nothing Then
                For Each Hijo As SectorQuadtree In mSubSectores
                    If Not Hijo Is Nothing Then Hijo.Refrescar()
                Next
            End If
            mObjetos = Nothing
        End Sub

        Public Function Pertenece(ByRef Punto As Punto2D) As Integer
            Return Pertenece(Me, Punto)
        End Function

        Public Function Pertenece(ByRef Recta As Recta2D) As Integer
            Return Pertenece(Me, Recta)
        End Function

        Public Function Pertenece(ByRef AABB As AABB2D) As Integer
            Return Pertenece(Me, AABB)
        End Function

        Public Function Pertenece(ByRef Figura2D As Figura2D) As Integer
            Return Pertenece(Me, Figura2D)
        End Function

        Public Shared Function ObtenerSubSectores(ByVal Sector As SectorQuadtree) As SectorQuadtree()
            If Sector.Nivel < Sector.Niveles - 1 Then
                Dim Retorno(3) As SectorQuadtree
                Dim Tamaño As New Punto2D(Sector.Espacio.LongitudX / 2, Sector.Espacio.LongitudY / 2)

                Retorno(0) = New SectorQuadtree(Sector, 0, New AABB2D(Sector.Espacio.Posicion, Tamaño))
                Retorno(1) = New SectorQuadtree(Sector, 1, New AABB2D(New Punto2D(Sector.Espacio.MinX + Tamaño.X, Sector.Espacio.MinY), Tamaño))
                Retorno(2) = New SectorQuadtree(Sector, 2, New AABB2D(New Punto2D(Sector.Espacio.MinX + Tamaño.X, Sector.Espacio.MinY + Tamaño.Y), Tamaño))
                Retorno(3) = New SectorQuadtree(Sector, 3, New AABB2D(New Punto2D(Sector.Espacio.MinX, Sector.Espacio.MinY + Tamaño.Y), Tamaño))

                Return Retorno
            Else
                Throw New ExcepcionGeometrica2D("SECTORQUADTREE (NEW): No se pueden generar subsectores de un sector cuyo nivel es máximo." & vbNewLine _
                                                & "Niveles del Quadtree=" & Sector.Niveles.ToString & vbNewLine _
                                                & "Nivel del sector=" & Sector.Niveles)
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorQuadtree, ByRef Punto As Punto2D) As Integer
            If Not Sector.EsHoja Then
                For i As Integer = 0 To 3
                    If Sector.SubSectores(i).Espacio.Pertenece(Punto) Then
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

        Public Shared Function Pertenece(ByRef Sector As SectorQuadtree, ByRef Recta As Recta2D) As Integer
            If Not Sector.EsHoja Then
                For i As Integer = 0 To 3
                    If Sector.SubSectores(i).Espacio.Pertenece(Recta) Then
                        Return i
                    End If
                Next
            Else
                If Sector.Espacio.Pertenece(Recta) Then
                    Return -1
                Else
                    Return -2
                End If
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorQuadtree, ByRef AABB As AABB2D) As Integer
            If Not Sector.EsHoja AndAlso AABB.Dimensiones < Sector.Espacio.Dimensiones / 2 Then
                'LA AABB PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
                For i As Integer = 0 To 3
                    If Sector.SubSectores(i).Espacio.Colision(AABB) Then
                        Return i
                    End If
                Next
            Else
                If AABB.Dimensiones < Sector.Espacio.Dimensiones AndAlso Sector.Espacio.Colision(AABB) Then
                    'LA AABB PERTENECE AL SECTOR:
                    Return -1
                Else
                    'LA AABB NO PERTENECE AL SECTOR:
                    Return -2
                End If
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorQuadtree, ByRef Figura2D As Figura2D) As Integer
            If Not Sector.EsHoja AndAlso Figura2D.AABB.Dimensiones < Sector.Espacio.Dimensiones / 2 Then
                'LA AABB PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
                For i As Integer = 0 To 3
                    If Sector.SubSectores(i).Espacio.Colision(Figura2D.AABB) Then
                        Return i
                    End If
                Next
            Else
                If Figura2D.AABB.Dimensiones < Sector.Espacio.Dimensiones AndAlso Sector.Espacio.Colision(Figura2D.AABB) Then
                    'LA AABB PERTENECE AL SECTOR:
                    Sector.AñadirFigura2D(Figura2D)
                    Return -1
                Else
                    'LA AABB NO PERTENECE AL SECTOR:
                    Return -2
                End If
            End If
        End Function
    End Class
End Namespace
