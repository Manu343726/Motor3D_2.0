Imports Motor3D.Escena
Imports Motor3D.Espacio2D

Namespace Espacio3D
    Public Class AABB3D
        Inherits ObjetoGeometrico3D

        Private Pos As Punto3D
        Private Size As Vector3D

        Public ReadOnly Property Posicion() As Punto3D
            Get
                Return Pos
            End Get
        End Property

        Public ReadOnly Property Centro() As Punto3D
            Get
                Return New Punto3D(Pos.X + (Size.X / 2), Pos.Y + (Size.Y / 2), Pos.Z + (Size.Z / 2))
            End Get
        End Property

        Public ReadOnly Property Dimensiones() As Vector3D
            Get
                Return Size
            End Get
        End Property

        Public ReadOnly Property Vertices() As Punto3D()
            Get
                Dim Retorno(7) As Punto3D

                Retorno(0) = New Punto3D(Left, Top, Down)
                Retorno(1) = New Punto3D(Right, Top, Down)
                Retorno(2) = New Punto3D(Right, Bottom, Down)
                Retorno(3) = New Punto3D(Left, Bottom, Down)
                Retorno(4) = New Punto3D(Left, Top, Up)
                Retorno(5) = New Punto3D(Right, Top, Up)
                Retorno(6) = New Punto3D(Right, Bottom, Up)
                Retorno(7) = New Punto3D(Left, Bottom, Up)

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property Vertices(ByVal Indice As Integer) As Punto3D
            Get
                Select Case Indice
                    Case 0
                        Return New Punto3D(Left, Top, Down)
                    Case 1
                        Return New Punto3D(Right, Top, Down)
                    Case 2
                        Return New Punto3D(Right, Bottom, Down)
                    Case 3
                        Return New Punto3D(Left, Bottom, Down)
                    Case 4
                        Return New Punto3D(Left, Top, Up)
                    Case 5
                        Return New Punto3D(Right, Top, Up)
                    Case 6
                        Return New Punto3D(Right, Bottom, Up)
                    Case 7
                        Return New Punto3D(Left, Bottom, Up)
                    Case Else
                        Throw New ExcepcionGeometrica3D("AABB3D (VERTICES_GET): El índice debe estar entre 0 y 7." & vbNewLine & _
                                                        "Indice=" & Indice.ToString)
                End Select
            End Get
        End Property

        Public ReadOnly Property Vertices(ByVal Camara As Camara3D, Optional ByVal DefinidaEnSRC As Boolean = False) As Punto2D()
            Get
                Dim Retorno(7) As Punto2D

                Retorno(0) = Camara.Proyeccion(New Punto3D(Left, Top, Down), DefinidaEnSRC)
                Retorno(1) = Camara.Proyeccion(New Punto3D(Right, Top, Down), DefinidaEnSRC)
                Retorno(2) = Camara.Proyeccion(New Punto3D(Right, Bottom, Down), DefinidaEnSRC)
                Retorno(3) = Camara.Proyeccion(New Punto3D(Left, Bottom, Down), DefinidaEnSRC)
                Retorno(4) = Camara.Proyeccion(New Punto3D(Left, Top, Up), DefinidaEnSRC)
                Retorno(5) = Camara.Proyeccion(New Punto3D(Right, Top, Up), DefinidaEnSRC)
                Retorno(6) = Camara.Proyeccion(New Punto3D(Right, Bottom, Up), DefinidaEnSRC)
                Retorno(7) = Camara.Proyeccion(New Punto3D(Left, Bottom, Up), DefinidaEnSRC)

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property Representacion(ByVal Camara As Camara3D, Optional ByVal DefinidaEnSRC As Boolean = False) As Poligono2D()
            Get
                Dim Retorno(5) As Poligono2D
                Dim Vertices() As Punto2D = Me.Vertices(Camara, DefinidaEnSRC)

                Retorno(0) = New Poligono2D(Vertices(0), Vertices(1), Vertices(2), Vertices(3))
                Retorno(1) = New Poligono2D(Vertices(4), Vertices(7), Vertices(6), Vertices(5))
                Retorno(2) = New Poligono2D(Vertices(4), Vertices(5), Vertices(1), Vertices(0))
                Retorno(3) = New Poligono2D(Vertices(7), Vertices(4), Vertices(0), Vertices(3))
                Retorno(4) = New Poligono2D(Vertices(6), Vertices(7), Vertices(3), Vertices(2))
                Retorno(5) = New Poligono2D(Vertices(5), Vertices(6), Vertices(2), Vertices(1))

                Return Retorno
            End Get
        End Property

        Public ReadOnly Property Left() As Double
            Get
                Return Pos.X
            End Get
        End Property

        Public ReadOnly Property Top() As Double
            Get
                Return Pos.Y
            End Get
        End Property

        Public ReadOnly Property Ancho() As Double
            Get
                Return Size.X
            End Get
        End Property

        Public ReadOnly Property Largo() As Double
            Get
                Return Size.Y
            End Get
        End Property

        Public ReadOnly Property Alto() As Double
            Get
                Return Size.Z
            End Get
        End Property

        Public ReadOnly Property Right() As Double
            Get
                Return Size.X + Pos.X
            End Get
        End Property

        Public ReadOnly Property Bottom() As Double
            Get
                Return Size.Y + Pos.Y
            End Get
        End Property

        Public ReadOnly Property Up() As Double
            Get
                Return Pos.Z + Size.Z
            End Get
        End Property

        Public ReadOnly Property Down() As Double
            Get
                Return Pos.Z
            End Get
        End Property

        Public Sub New(ByVal Posicion As Punto3D, ByVal Tamaño As Vector3D)
            Pos = New Punto3D
            Size = New Vector3D

            If Tamaño.X < 0 Then
                Pos.X = Posicion.X + Tamaño.X
                Size.X = -Tamaño.X
            Else
                Pos.X = Posicion.X
                Size.X = Tamaño.X
            End If
            If Tamaño.Y < 0 Then
                Pos.Y = Posicion.Y + Tamaño.Y
                Size.Y = -Tamaño.Y
            Else
                Pos.Y = Posicion.Y
                Size.Y = Tamaño.Y
            End If
            If Tamaño.Z < 0 Then
                Pos.Z = Posicion.Z + Tamaño.Z
                Size.Z = -Tamaño.Z
            Else
                Pos.Z = Posicion.Z
                Size.Z = Tamaño.Z
            End If
        End Sub

        Public Sub New(ByVal PosX As Double, ByVal PosY As Double, ByVal PosZ As Double, ByVal Ancho As Double, ByVal Largo As Double, ByVal Alto As Double)
            Pos = New Punto3D
            Size = New Vector3D

            If Ancho < 0 Then
                Pos.X = PosX + Ancho
                Size.X = -Ancho
            Else
                Pos.X = PosX
                Size.X = Ancho
            End If
            If Largo < 0 Then
                Pos.Y = PosY + Largo
                Size.Y = -Largo
            Else
                Pos.Y = PosY
                Size.Y = Largo
            End If
            If Alto < 0 Then
                Pos.Z = PosZ + Alto
                Size.Z = -Alto
            Else
                Pos.Z = PosZ
                Size.Z = Alto
            End If
        End Sub

        Public Shared Function Pertenece(ByVal AABB As AABB3D, ByVal Punto As Punto3D) As Boolean
            Return (Punto.X >= AABB.Left AndAlso Punto.X <= AABB.Right AndAlso Punto.Y >= AABB.Top AndAlso Punto.Y <= AABB.Bottom AndAlso Punto.Z >= AABB.Down AndAlso Punto.Z <= AABB.Up)
        End Function

        Public Shared Function Colisionan(ByVal C1 As AABB3D, ByVal C2 As AABB3D) As Boolean
            Dim R As New AABB3D(C1.Posicion.X - C2.Ancho, C1.Posicion.Y - C2.Largo, C1.Posicion.Z - C2.Alto, C1.Ancho + C2.Ancho, C1.Largo + C2.Largo, C1.Alto + C2.Alto)

            Return Pertenece(R, C2.Posicion)
        End Function

        Public Shared Function Pertenece(ByVal AABB As AABB3D, ByVal Recta As Recta3D) As Boolean
            Dim PosicionA As Double = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(0))
            Dim PosicionB As Double
            Dim PlanoA As Boolean = False

            If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(1)) Then
                If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(2)) Then
                    If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(3)) Then
                        If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(4)) Then
                            If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(5)) Then
                                If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(6)) Then
                                    If PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(AABB.Vertices(7)) Then
                                        Return False
                                    Else
                                        PlanoA = True
                                    End If
                                Else
                                    PlanoA = True
                                End If
                            Else
                                PlanoA = True
                            End If
                        Else
                            PlanoA = True
                        End If
                    Else
                        PlanoA = True
                    End If
                Else
                    PlanoA = True
                End If
            Else
                PlanoA = True
            End If

            If PlanoA Then
                PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(0))

                If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(1)) Then
                    If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(2)) Then
                        If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(3)) Then
                            If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(4)) Then
                                If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(5)) Then
                                    If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(6)) Then
                                        If PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(AABB.Vertices(7)) Then
                                            Return False
                                        Else
                                            Return True
                                        End If
                                    Else
                                        Return True
                                    End If
                                Else
                                    Return True
                                End If
                            Else
                                Return True
                            End If
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Function PuntoCorteRecta(ByVal AABB As AABB3D, ByVal Recta As Recta3D) As Punto3D
            If Recta.VectorDirector.X = 0 Then
                If Recta.VectorDirector.Y = 0 Then
                    Return Recta.ObtenerPunto(AABB.Centro.Z, EnumEjes.EjeZ)
                Else
                    Return Recta.ObtenerPunto(AABB.Centro.Y, EnumEjes.EjeY)
                End If
            Else
                Return Recta.ObtenerPunto(AABB.Centro.X, EnumEjes.EjeX)
            End If
        End Function

        Public Function Pertenece(ByVal Recta As Recta3D) As Boolean
            Return Pertenece(Me, Recta)
        End Function

        Public Function Pertenece(ByVal Punto As Punto3D) As Boolean
            Return Pertenece(Me, Punto)
        End Function

        Public Function Colisionan(ByVal AABB As AABB3D) As Boolean
            Return Colisionan(Me, AABB)
        End Function

        Public Overrides Function ToString() As String
            Return "[Posición=" & Pos.ToString & ";" & "Dimensiones=" & Size.ToString & "]"
        End Function

        Public Shared Operator =(ByVal C1 As AABB3D, ByVal C2 As AABB3D) As Boolean
            Return (C1.Posicion = C2.Posicion) AndAlso (C1.Dimensiones = C2.Dimensiones)
        End Operator

        Public Shared Operator <>(ByVal C1 As AABB3D, ByVal C2 As AABB3D) As Boolean
            Return Not (C1 = C2)
        End Operator
    End Class
End Namespace
