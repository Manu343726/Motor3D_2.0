Imports Motor3D.Espacio3D
Imports System.Drawing

Namespace Primitivas3D
    Public Class SectorOctreeGrafico
        Inherits ObjetoGeometrico3D

        Private mEspacio As AABB3D
        Private mIndice As Integer
        Private mNivel As Integer
        Private mNiveles As Integer

        Private mPadre As SectorOctreeGrafico
        Private mHijos() As SectorOctreeGrafico

        Private mColor As Color
        Private mVacio As Boolean

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

        Public ReadOnly Property Padre As SectorOctreeGrafico
            Get
                Return mPadre
            End Get
        End Property

        Public ReadOnly Property Hijos() As SectorOctreeGrafico()
            Get
                Return mHijos
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

        Public ReadOnly Property Hijos(ByVal Indice As Integer) As SectorOctreeGrafico
            Get
                If Indice >= 0 AndAlso Indice <= 7 Then
                    Return mHijos(Indice)
                Else
                    Throw New ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (HIJOS_GET): El indice de un sector de Octree debe estar comprendido entre 0 y 7." & vbNewLine _
                                                    & "Indice=" & Indice.ToString)
                End If
            End Get
        End Property

        Public ReadOnly Property Vacio As Boolean
            Get
                Return mVacio
            End Get
        End Property

        Public ReadOnly Property Color As Color
            Get
                Return mColor
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
                mVacio = True

                If mNivel < mNiveles - 1 Then
                    mHijos = ObtenerSubSectores(Me)
                End If
            Else
                Throw New ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW_RAIZ): Un octree necesita al menos dos niveles." & vbNewLine _
                                                & "Niveles=" & Niveles.ToString)
            End If
        End Sub

        Public Sub New(ByRef Padre As SectorOctreeGrafico, ByVal Indice As Integer, ByRef AABB As AABB3D)
            If Padre.Niveles > 1 Then
                If Nivel >= 0 AndAlso Nivel < Padre.Niveles Then
                    If Indice >= 0 AndAlso Indice <= 7 Then
                        mNivel = Padre.Nivel + 1
                        mNiveles = Padre.Niveles
                        mIndice = Indice
                        mEspacio = AABB
                        mPadre = Padre
                        mVacio = True

                        If mNivel < mNiveles - 1 Then
                            mHijos = ObtenerSubSectores(Me)
                        End If
                    Else
                        Throw New ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): El indice de un sector de Octree debe estar comprendido entre 0 y 7." & vbNewLine _
                                                        & "Indice=" & Indice.ToString)
                    End If

                Else
                    Throw New ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): El nivel de un sector de Octree debe estar entre 0 y el número de niveles del Octree menos uno." & vbNewLine _
                                                    & "Niveles del Octree=" & Niveles.ToString & vbNewLine _
                                                    & "Nivel del sector=" & Nivel.ToString)
                End If

            Else
                Throw New ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): Un Octree debe tener al menos dos niveles." & vbNewLine _
                                                & "Niveles del Octree=" & Niveles.ToString)
            End If
        End Sub

        Public Sub Vaciar()
            If mHijos Is Nothing Then
                mColor = Drawing.Color.Black
                mVacio = True
            End If
        End Sub

        Public Sub Rellenar(ByVal Color As Color)
            If mHijos Is Nothing Then
                mColor = Color
                mVacio = False
                If Not mPadre Is Nothing Then
                    mPadre.EvaluarFusion()
                End If
            End If
        End Sub

        Public Sub EvaluarFusion(Optional ByVal HijoFusionado As Integer = -1)
            Dim Salir As Boolean = False
            For i As Integer = 0 To 7
                If mHijos(i).Vacio Then
                    If HijoFusionado = i Then
                        mHijos(i).Fusion()
                        Exit Sub
                    End If

                    Salir = True
                End If
            Next
            If Not Salir Then
                If mPadre Is Nothing Then
                    mColor = Fusionar()
                Else
                    mPadre.EvaluarFusion(mIndice)
                End If
            End If
        End Sub

        Public Sub Fusion()
            mVacio = False
            mColor = Fusionar()
        End Sub

        Public Function Fusionar() As Color
            If mHijos Is Nothing Then
                Return mColor
            Else
                Dim r, g, b As Long
                Dim Color As Color

                r = 0
                g = 0
                b = 0

                For i As Integer = 0 To 7
                    Color = mHijos(i).Fusionar
                    r += Color.R
                    g += Color.G
                    b += Color.B
                Next

                Return Color.FromArgb(255, r / 8, g / 8, b / 8)
            End If
        End Function

        Public Function Pertenece(ByRef Punto As Punto3D) As Integer
            Return Pertenece(Me, Punto)
        End Function

        Public Function Pertenece(ByRef Recta As Recta3D) As Integer
            Return Pertenece(Me, Recta)
        End Function

        Public Function Pertenece(ByRef AABB As AABB3D) As Integer
            Return Pertenece(Me, AABB)
        End Function

        Public Shared Function ObtenerSubSectores(ByVal Sector As SectorOctreeGrafico) As SectorOctreeGrafico()
            If Sector.Nivel < Sector.Niveles - 1 Then
                Dim Retorno(7) As SectorOctreeGrafico
                Dim Tamaño As New Vector3D(Sector.Espacio.Ancho / 2, Sector.Espacio.Largo / 2, Sector.Espacio.Alto / 2)

                'ABAJO:
                Retorno(0) = New SectorOctreeGrafico(Sector, 0, New AABB3D(Sector.Espacio.Posicion, Tamaño))
                Retorno(1) = New SectorOctreeGrafico(Sector, 1, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down), Tamaño))
                Retorno(2) = New SectorOctreeGrafico(Sector, 2, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño))
                Retorno(3) = New SectorOctreeGrafico(Sector, 3, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño))
                'ARRIBA:
                Retorno(4) = New SectorOctreeGrafico(Sector, 4, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(5) = New SectorOctreeGrafico(Sector, 5, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(6) = New SectorOctreeGrafico(Sector, 6, New AABB3D(New Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño))
                Retorno(7) = New SectorOctreeGrafico(Sector, 7, New AABB3D(New Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño))

                Return Retorno
            Else
                Throw New ExcepcionGeometrica3D("SECTOROCTREEGRAFICO (NEW): No se pueden generar subsectores de un sector cuyo nivel es máximo." & vbNewLine _
                                                & "Niveles del Octree=" & Sector.Niveles.ToString & vbNewLine _
                                                & "Nivel del sector=" & Sector.Niveles)
            End If
        End Function

        Public Shared Function Pertenece(ByRef Sector As SectorOctreeGrafico, ByRef Punto As Punto3D) As Integer
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

        Public Shared Function Pertenece(ByRef Sector As SectorOctreeGrafico, ByRef Recta As Recta3D) As Integer
            If Not Sector.EsHoja Then
                For i As Integer = 0 To 7
                    If Sector.Hijos(i).Espacio.Pertenece(Recta) Then
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

        Public Shared Function Pertenece(ByRef Sector As SectorOctreeGrafico, ByRef AABB As AABB3D) As Integer
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
    End Class
End Namespace

