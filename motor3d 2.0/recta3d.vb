Imports Motor3D.Algebra
Imports System.Math

Namespace Espacio3D
    Public Class Recta3D
        Inherits ObjetoGeometrico3D

        Private PlanoA, PlanoB As Plano3D
        Private mVector As Vector3D
        Private mPunto As Punto3D
        Private mPuntoMira As Punto3D

        Public ReadOnly Property PrimerPlano() As Plano3D
            Get
                Return PlanoA
            End Get
        End Property

        Public ReadOnly Property SegundoPlano() As Plano3D
            Get
                Return PlanoB
            End Get
        End Property

        Public ReadOnly Property VectorDirector() As Vector3D
            Get
                Return mVector
            End Get
        End Property

        Public ReadOnly Property PuntoInicial As Punto3D
            Get
                Return mPunto
            End Get
        End Property

        Public ReadOnly Property PuntoMira As Punto3D
            Get
                Return mPuntoMira
            End Get
        End Property

        Public ReadOnly Property Matrices As Matriz()
            Get
                Return RepresentacionMatricial(Me)
            End Get
        End Property

        Public Function ObtenerPuntoParametrico(ByVal Delta As Double) As Punto3D
            Return New Punto3D(mPunto.X + (Delta * mVector.X), mPunto.Y + (Delta * mVector.Y), mPunto.Z + (Delta * mVector.Z))
        End Function

        Public Function ObtenerParametro(ByVal Valor As Double, ByVal Coordenada As EnumEjes) As Double
            Select Case Coordenada
                Case EnumEjes.EjeX
                    If mVector.X <> 0 Then
                        Return (Valor - mPunto.X) / mVector.X
                    Else
                        Throw New ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." & vbNewLine & _
                                                        "Coordenada: " & Coordenada.ToString & vbNewLine & _
                                                        "Vector direccion: " & mVector.ToString)
                    End If
                Case EnumEjes.EjeY
                    If mVector.Y <> 0 Then
                        Return (Valor - mPunto.Y) / mVector.Y
                    Else
                        Throw New ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." & vbNewLine & _
                                                        "Coordenada: " & Coordenada.ToString & vbNewLine & _
                                                        "Vector direccion: " & mVector.ToString)
                    End If
                Case EnumEjes.EjeZ
                    If mVector.Z <> 0 Then
                        Return (Valor - mPunto.Z) / mVector.Z
                    Else
                        Throw New ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." & vbNewLine & _
                                                        "Coordenada: " & Coordenada.ToString & vbNewLine & _
                                                        "Vector direccion: " & mVector.ToString)
                    End If
            End Select
        End Function

        Public Function ObtenerPunto(ByVal Valor As Double, ByVal Coordenada As EnumEjes) As Punto3D
            Return ObtenerPuntoParametrico(ObtenerParametro(Valor, Coordenada))
        End Function

        Public Function ObtenerPunto(ByVal X As Double) As Punto3D
            Dim ec1, ec2 As Ecuacion
            Dim sis As SistemaEcuaciones

            ec1 = New Ecuacion(PlanoA.B, PlanoA.C, -(PlanoA.D + (PlanoA.A * X)))
            ec2 = New Ecuacion(PlanoB.B, PlanoB.C, -(PlanoB.D + (PlanoB.A * X)))

            sis = New SistemaEcuaciones(ec1, ec2)

            If sis.Solucion.TipoSolucion = TipoSolucionSistema.SistemaCompatibleDeterminado Then
                Return New Punto3D(X, sis.Solucion.ValorSolucion(0), sis.Solucion.ValorSolucion(1))
            Else
                Throw New ExcepcionGeometrica3D("RECTA3D (OBTENERPUNTO): No se ha podido calcular el punto. Es posible que los datos de los planos sean erroneos, o que el cálculo del sistema halla fallado." & vbNewLine _
                                                & "Valor de la variable=" & X.ToString & vbNewLine _
                                                & "Primer plano: " & PlanoA.ToString & vbNewLine _
                                                & "Seundo plano: " & PlanoB.ToString & vbNewLine _
                                                & "Primera ecuación del sistema: " & ec1.ToString & vbNewLine _
                                                & "Segunda ecuación del sistema: " & ec2.ToString & vbNewLine _
                                                & "Solución obtenida: " & sis.Solucion.ToString)
            End If
        End Function

        Public Sub New(ByVal P1 As Plano3D, ByVal P2 As Plano3D)
            PlanoA = P1
            PlanoB = P2
            mVector = (P1.VectorNormal & P2.VectorNormal).VectorUnitario
            mPunto = ObtenerPuntoParametrico(0)
            mPuntoMira = ObtenerPuntoParametrico(10)
        End Sub

        Public Sub New(ByVal P1 As Punto3D, ByVal P2 As Punto3D)
            Me.New(P1, New Vector3D(P1, P2))
        End Sub

        Public Sub New(ParamArray Matrices() As Matriz)
            If Matrices.GetUpperBound(0) = 1 Then
                Dim Punto As New Punto3D(Matrices(0))
                Dim Punto2 As New Punto3D(Matrices(1))

                mPunto = Punto
                mPuntoMira = Punto2
                mVector = New Vector3D(mPunto, mPuntoMira).VectorUnitario

                'ÉSTOS VALORES SE OBTIENEN AL DESPEJAR LA ECUACIÓN PARAMÉTRICA DE UNA RECTA GENÉRICA:
                PlanoA = New Plano3D(mVector.Y, -mVector.X, 0, (-mVector.Y * Punto.X) + (mVector.X * Punto.Y))
                PlanoB = New Plano3D(0, mVector.Z, -mVector.Y, (-mVector.Z * Punto.Y) + (mVector.Y * Punto.Z))
            Else
                Throw New ExcepcionGeometrica3D("RECTA3D (NEW): La representación matricial de una recta corresponde a un array con dos matrices" & vbNewLine & _
                                                "Tamaño del array=" & Matrices.GetUpperBound(0) + 1)
            End If
        End Sub

        Public Sub New(ByVal Punto As Punto3D, ByVal Vector As Vector3D)
            mPunto = Punto
            mPuntoMira = New Punto3D(mPunto.X + Vector.X, mPunto.Y + Vector.Y, mPunto.Z + Vector.Z)
            mVector = Vector.VectorUnitario

            'ÉSTOS VALORES SE OBTIENEN AL DESPEJAR LA ECUACIÓN PARAMÉTRICA DE UNA RECTA GENÉRICA:
            PlanoA = New Plano3D(Vector.Y, -Vector.X, 0, (-Vector.Y * Punto.X) + (Vector.X * Punto.Y))
            PlanoB = New Plano3D(0, Vector.Z, -Vector.Y, (-Vector.Z * Punto.Y) + (Vector.Y * Punto.Z))
        End Sub

        Public Function Pertenece(ByVal Punto As Punto3D) As Boolean
            Return PlanoA.Pertenece(Punto) AndAlso PlanoB.Pertenece(Punto)
        End Function

        Public Overrides Function ToString() As String
            Return "{Recta " & PlanoA.ToString & " & " & PlanoB.ToString & "}"
        End Function

        Public Shared Function RepresentacionMatricial(ByVal Recta As Recta3D) As Matriz()
            Dim Retorno(1) As Matriz

            Retorno(0) = Recta.PuntoInicial.Matriz
            Retorno(1) = Recta.PuntoMira.Matriz

            Return Retorno
        End Function

        Public Shared Function PosicionRelativa(ByVal R1 As Recta3D, ByVal R2 As Recta3D) As PosicionRelativa3D
            Dim Sistema As New SistemaEcuaciones(New Ecuacion(R1.VectorDirector.X, R2.VectorDirector.X, R2.PuntoInicial.X - R1.PuntoInicial.Y), _
                                                 New Ecuacion(R1.VectorDirector.Y, R2.VectorDirector.Y, R2.PuntoInicial.Y - R1.PuntoInicial.Y), _
                                                 New Ecuacion(R1.VectorDirector.Z, R2.VectorDirector.Z, R2.PuntoInicial.Z - R1.PuntoInicial.Z))

            Select Case Sistema.Solucion.TipoSolucion
                Case TipoSolucionSistema.SistemaCompatibleDeterminado
                    Return New PosicionRelativa3D(New Punto3D(Sistema.Solucion.ValorSolucion(0), Sistema.Solucion.ValorSolucion(1), Sistema.Solucion.ValorSolucion(2)))
                Case TipoSolucionSistema.SistemaCompatibleIndeterminado
                    Return New PosicionRelativa3D(TipoPosicionRelativa3D.Coincidente)
                Case TipoSolucionSistema.SistemaIncompatible
                    If Sistema.RangoMatrizPrincipal = 1 Then
                        Return New PosicionRelativa3D(TipoPosicionRelativa3D.Paralelo)
                    Else
                        Return New PosicionRelativa3D(TipoPosicionRelativa3D.Cruce)
                    End If
            End Select
        End Function

        Public Shared Function Distancia(ByVal Recta As Recta3D, ByVal Punto As Punto3D) As Double
            Return (New Vector3D(Recta.ObtenerPuntoParametrico(0), Punto) & Recta.VectorDirector).Modulo
        End Function

        Public Shared Function Distancia(ByVal Recta As Recta3D, ByVal Plano As Plano3D) As Double
            If Recta.VectorDirector * Plano.VectorNormal = 0 Then
                Return Distancia(Recta, Plano.ObtenerPunto(0, 0))
            Else
                Return 0
            End If
        End Function

        Public Shared Function Distancia(ByVal R1 As Recta3D, ByVal R2 As Recta3D) As Double
            Select Case PosicionRelativa(R1, R2).Tipo
                Case TipoPosicionRelativa3D.Coincidente
                    Return 0
                Case TipoPosicionRelativa3D.Secante
                    Return 0
                Case TipoPosicionRelativa3D.Paralelo
                    Return Distancia(R1, R2.ObtenerPuntoParametrico(0))
                Case TipoPosicionRelativa3D.Cruce
                    Return Abs(Vector3D.ProductoMixto(R1.VectorDirector, R2.VectorDirector, New Vector3D(R1.ObtenerPuntoParametrico(0), R2.ObtenerPuntoParametrico(0)))) / (R1.VectorDirector & R2.VectorDirector).Modulo
            End Select
        End Function

        Public Shared Function Proyeccion(ByVal Punto As Punto3D, ByVal Recta As Recta3D) As Punto3D
            Dim Landa As Integer
            Dim Vx, Vy, Vz, VVx, VVy, VVz, Qx, Qy, Qz, x, y, z As Double

            Vx = Recta.VectorDirector.X
            Vy = Recta.VectorDirector.Y
            Vz = Recta.VectorDirector.Z

            VVx = Vx * Vx
            VVy = Vy * Vy
            VVz = Vz * Vz

            Qx = Recta.PuntoInicial.X
            Qy = Recta.PuntoInicial.Y
            Qz = Recta.PuntoInicial.Z

            x = Punto.X
            y = Punto.Y
            z = Punto.Z

            Landa = ((Vx * (x - Qx)) + (Vy * (y - Qy)) + (Vz * (z - Qz))) / (VVx + VVy + VVz)

            Return Recta.ObtenerPuntoParametrico(Landa)
        End Function
    End Class
End Namespace

