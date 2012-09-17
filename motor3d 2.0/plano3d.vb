Imports Motor3D.Algebra
Imports System.Math

Namespace Espacio3D
    Public Class Plano3D
        Inherits ObjetoGeometrico3D

        Private mA, mB, mC, mD As Double
        Private Normal As Vector3D

        Public ReadOnly Property A() As Double
            Get
                Return mA
            End Get
        End Property

        Public ReadOnly Property B() As Double
            Get
                Return mB
            End Get
        End Property

        Public ReadOnly Property C() As Double
            Get
                Return mC
            End Get
        End Property

        Public ReadOnly Property D() As Double
            Get
                Return mD
            End Get
        End Property

        Public ReadOnly Property VectorNormal() As Vector3D
            Get
                Return Normal
            End Get
        End Property

        Public Sub New(ByVal A As Double, ByVal B As Double, ByVal C As Double, ByVal D As Double)
            mA = A
            mB = B
            mC = C
            mD = D
            Normal = New Vector3D(A, B, C)
        End Sub

        Public Sub New(ByVal V1 As Vector3D, ByVal V2 As Vector3D, ByVal Punto As Punto3D)
            Normal = V1 & V2

            mA = Normal.X
            mB = Normal.Y
            mC = Normal.Z

            mD = -((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z))
        End Sub

        Public Sub New(ByVal P1 As Punto3D, ByVal P2 As Punto3D, ByVal P3 As Punto3D)
            Normal = New Vector3D(P1, P2) & New Vector3D(P1, P3)

            mA = Normal.X
            mB = Normal.Y
            mC = Normal.Z

            mD = -((mA * P1.X) + (mB * P1.Y) + (mC * P1.Z))
        End Sub

        Public Sub New(ByVal Punto As Punto3D, ByVal Vector As Vector3D)
            Normal = Vector.VectorUnitario

            mA = Normal.X
            mB = Normal.Y
            mC = Normal.Z

            mD = -((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z))
        End Sub

        Public Function ObtenerPunto(ByVal X As Double, ByVal Y As Double) As Punto3D
            Return New Punto3D(X, Y, ((mA * X) + (mB * Y) + mD) / (-mC))
        End Function

        Public Function Pertenece(ByVal Punto As Punto3D) As Boolean
            Return ((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD = 0)
        End Function

        Public Function PosicionRelativa(ByVal Punto As Punto3D) As Double
            Return (mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD
        End Function

        Public Function SignoPosicionRelativa(ByVal Punto As Punto3D) As Double
            Return Sign((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD)
        End Function

        Public Function PosicionRelativa(ByVal Plano As Plano3D) As PosicionRelativa3D
            Return PosicionRelativa(Me, Plano)
        End Function

        Public Function ObtenerEcuacion() As Ecuacion
            Return New Ecuacion(mA, mB, mC, -mD)
        End Function

        Public Overrides Function ToString() As String
            Return FormatNumber(mA, 2) & "X" & IIf(mB >= 0, "+" & FormatNumber(mB, 2), FormatNumber(mB, 2)) & "Y" & IIf(mC >= 0, "+" & FormatNumber(mC, 2), FormatNumber(mC, 2)) & "Z" & IIf(mD >= 0, "+" & FormatNumber(mD, 2), FormatNumber(mD, 2)) & "=0"
        End Function

        Public Shared Function PosicionRelativa(ByVal P1 As Plano3D, ByVal P2 As Plano3D) As PosicionRelativa3D
            Dim Retorno As PosicionRelativa3D
            Dim Sis As SistemaEcuaciones

            Sis = New SistemaEcuaciones(P1.ObtenerEcuacion, P2.ObtenerEcuacion)

            Select Case Sis.Solucion.TipoSolucion
                Case TipoSolucionSistema.SistemaCompatibleDeterminado
                    Retorno = New PosicionRelativa3D(New Punto3D(Sis.Solucion.ValorSolucion(0), Sis.Solucion.ValorSolucion(1), Sis.Solucion.ValorSolucion(2)))
                Case TipoSolucionSistema.SistemaCompatibleIndeterminado
                    Retorno = New PosicionRelativa3D(TipoPosicionRelativa3D.Coincidente)
                Case Else
                    Retorno = New PosicionRelativa3D(TipoPosicionRelativa3D.Paralelo)
            End Select

            Return Retorno
        End Function

        Public Shared Function PosicionRelativa(ByVal Plano As Plano3D, ByVal Punto As Punto3D) As Double
            Return (Plano.A * Punto.X) + (Plano.B * Punto.Y) + (Plano.C * Punto.Z) + Plano.C
        End Function

        Public Shared Function SignoPosicionRelativa(ByVal Plano As Plano3D, ByVal Punto As Punto3D) As Double
            Return Sign((Plano.A * Punto.X) + (Plano.B * Punto.Y) + (Plano.C * Punto.Z) + Plano.D)
        End Function

        Public Shared Function Interseccion(ByVal Plano As Plano3D, ByVal Recta As Recta3D) As Punto3D
            Dim sis As SistemaEcuaciones

            If Plano.VectorNormal * Recta.VectorDirector <> 0 Then
                sis = New SistemaEcuaciones(Plano.ObtenerEcuacion, Recta.PrimerPlano.ObtenerEcuacion, Recta.SegundoPlano.ObtenerEcuacion)

                If sis.Solucion.TipoSolucion = TipoSolucionSistema.SistemaCompatibleDeterminado Then
                    Return New Punto3D(sis.Solucion.ValorSolucion(0), sis.Solucion.ValorSolucion(1), sis.Solucion.ValorSolucion(2))
                Else
                    If sis.Solucion.TipoSolucion = TipoSolucionSistema.SistemaCompatibleIndeterminado Then
                        Return Plano.ObtenerPunto(0, 0)
                    Else
                        Throw New ExcepcionGeometrica3D("PLANO3D (INTERSECCION): No se ha podido calcular la interseccion. Es posible que los datos de los planos sean erroneos, o que el cálculo del sistema halla fallado." & vbNewLine _
                                                                            & "Recta=" & Recta.ToString & vbNewLine _
                                                                            & "Plano: " & Plano.ToString & vbNewLine _
                                                                            & "Primer plano: " & Recta.PrimerPlano.ToString & vbNewLine _
                                                                            & "Seundo plano: " & Recta.SegundoPlano.ToString & vbNewLine _
                                                                            & "Primera ecuación del sistema: " & Plano.ObtenerEcuacion.ToString & vbNewLine _
                                                                            & "Segunda ecuación del sistema: " & Recta.PrimerPlano.ObtenerEcuacion.ToString & vbNewLine _
                                                                            & "Tercera ecuación del sistema: " & Recta.SegundoPlano.ObtenerEcuacion.ToString & vbNewLine _
                                                                            & "Solución obtenida: " & sis.Solucion.ToString)
                    End If
                End If
            Else
                If Recta3D.Distancia(Recta, Plano) = 0 Then
                    Return Recta.PuntoInicial
                Else
                    Throw New ExcepcionGeometrica3D("PLANO3D (INTERSECCION): La recta y el plano son paralelos" & vbNewLine _
                                                    & "Recta: " & Recta.ToString & vbNewLine _
                                                    & "Plano: " & Plano.ToString)
                End If
            End If
        End Function
    End Class
End Namespace

