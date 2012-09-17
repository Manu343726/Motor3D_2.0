Imports System.Math
Imports Motor3D.Algebra

Namespace Espacio2D
    Public Class Recta2D
        Inherits ObjetoGeometrico2D

        Private mA, mB, mC As Double
        Private mVector As Vector2D
        Private mPunto As Punto2D
        Private mPuntoMira As Punto2D
        Private mPendiente As Double

        Public ReadOnly Property VectorDirector() As Vector2D
            Get
                Return mVector
            End Get
        End Property

        Public ReadOnly Property PuntoDiretor As Punto2D
            Get
                Return mPunto
            End Get
        End Property

        Public ReadOnly Property PuntoDeMira As Punto2D
            Get
                Return mPuntoMira
            End Get
        End Property

        Public ReadOnly Property Pendiente() As Double
            Get
                If mB <> 0 Then
                    Return mPendiente
                Else
                    Throw New ExcepcionGeometrica2D("RECTA2D (PENDIENTE_GET): Pendiente infinita (Recta perpendicular al eje X)" & vbNewLine _
                                                 & "Ecuación de la recta: " & Me.ToString)
                End If
            End Get
        End Property

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

        Public ReadOnly Property Matrices As Matriz()
            Get
                Return RepresentacionMatricial(Me)
            End Get
        End Property

        Public Sub New(ParamArray Matrices() As Matriz)
            If Matrices.GetUpperBound(0) = 1 Then
                Dim Punto As New Punto2D(Matrices(0))
                Dim Punto2 As New Punto2D(Matrices(1))

                mPunto = Punto
                mPuntoMira = Punto2
                mVector = New Vector2D(mPunto, mPuntoMira).VectorUnitario

                mA = -mVector.Y
                mB = mVector.X
                mC = -((mA * mPunto.X) + (mB * mPunto.Y))
                mPendiente = IIf(mB <> 0, -(mA / mB), 0)
            Else
                Throw New ExcepcionGeometrica2D("RECTA2D (NEW): La representación matricial de una recta corresponde a un array con dos matrices" & vbNewLine & _
                                                "Tamaño del array=" & Matrices.GetUpperBound(0) + 1)
            End If
        End Sub

        Public Sub New(ByVal ValA As Double, ByVal ValB As Double, ByVal ValC As Double)
            mA = ValA / (Sqrt((ValA ^ 2) + (ValB ^ 2) + (ValC ^ 2)))
            mB = ValB / (Sqrt((ValA ^ 2) + (ValB ^ 2) + (ValC ^ 2)))
            mC = ValC / (Sqrt((ValA ^ 2) + (ValB ^ 2) + (ValC ^ 2)))

            mVector = New Vector2D(mB, -mA)
            mPunto = New Punto2D(0, -(mC / mB))
            mPuntoMira = New Punto2D(mPunto.X + mVector.X, mPunto.Y + mVector.Y)
            mPendiente = IIf(mB <> 0, -(mA / mB), 0)
        End Sub

        Public Sub New(ByVal P1 As Punto2D, ByVal P2 As Punto2D)
            mVector = New Vector2D(P1, P2).VectorUnitario
            mPunto = P1
            mPuntoMira = P2

            mA = -mVector.Y
            mB = mVector.X
            mC = -((mA * P1.X) + (mB * P1.Y))
            mPendiente = IIf(mB <> 0, -(mA / mB), 0)
        End Sub

        Public Sub New(ByVal ValPunto As Punto2D, ByVal ValVector As Vector2D)
            mVector = ValVector.VectorUnitario
            mPunto = ValPunto
            mPuntoMira = New Punto2D(mPunto.X + mVector.X, mPunto.Y + mVector.Y)

            mA = -mVector.Y
            mB = mVector.X
            mC = -((mA * mPunto.X) + (mB * mPunto.Y))
            mPendiente = IIf(mB <> 0, -(mA / mB), 0)
        End Sub

        Public Overridable Function Pertenece(ByVal Punto As Punto2D) As Boolean
            Return (((mA * Punto.X) + (mB * Punto.Y) + mC) = 0)
        End Function

        Public Function PosicionRelativa(ByVal Punto As Punto2D) As Double
            Dim Retorno As Double = ((mA * Punto.X) + (mB * Punto.Y) + mC)

            If mPendiente > 0 Then
                Return Retorno
            Else
                Return -Retorno
            End If
        End Function

        Public Function SignoPosicionRelativa(ByVal Punto As Punto2D) As Double
            Dim Retorno As Double = Sign((mA * Punto.X) + (mB * Punto.Y) + mC)

            If mPendiente >= 0 Then
                Return Retorno
            Else
                Return -Retorno
            End If
        End Function

        Public Function ObtenerEcuacion() As Ecuacion
            Return New Ecuacion(mA, mB, -mC)
        End Function

        Public Function Funcion(ByVal X As Double) As Double
            If mB <> 0 Then
                Return (-((mA * X) + mC) / mB)
            Else
                Return 0
            End If
        End Function

        Public Function ObtenerParametro(ByRef Punto As Punto2D) As Double
            If mVector.X <> 0 Then
                Return (Punto.X - mPunto.X) / mVector.X
            Else
                Return (Punto.Y - mPunto.Y) / mVector.Y
            End If
        End Function

        Public Function ObtenerPunto(ByVal X As Double) As Punto2D
            Return New Punto2D(X, Funcion(X))
        End Function

        Public Function PosicionRelativa(ByVal Recta As Recta2D) As PosicionRelativa2D
            Return PosicionRelativa(Me, Recta)
        End Function

        Public Overrides Function ToString() As String
            Return FormatNumber(mA, 2) & "X" & IIf(mB >= 0, "+" & FormatNumber(mB, 2), FormatNumber(mB, 2)) & "Y" & IIf(mC >= 0, "+" & FormatNumber(mC, 2), FormatNumber(mC, 2)) & "=0"
        End Function

        Public Shared Function PosicionRelativa(ByVal R1 As Recta2D, ByVal R2 As Recta2D) As PosicionRelativa2D
            Dim Retorno As PosicionRelativa2D
            Dim Sis As SistemaEcuaciones

            Sis = New SistemaEcuaciones(R1.ObtenerEcuacion, R2.ObtenerEcuacion)

            Select Case Sis.Solucion.TipoSolucion
                Case TipoSolucionSistema.SistemaCompatibleDeterminado
                    Retorno = New PosicionRelativa2D(New Punto2D(Sis.Solucion.ValorSolucion(0), Sis.Solucion.ValorSolucion(1)))
                Case TipoSolucionSistema.SistemaCompatibleIndeterminado
                    Retorno = New PosicionRelativa2D(TipoPosicionRelativa2D.Coincidente)
                Case Else
                    Retorno = New PosicionRelativa2D(TipoPosicionRelativa2D.Paralelo)
            End Select

            Return Retorno
        End Function

        Public Shared Function Interseccion(ByVal R1 As Recta2D, ByVal R2 As Recta2D) As Punto2D
            Dim m As PosicionRelativa2D = PosicionRelativa(R1, R2)

            If m.Tipo = TipoPosicionRelativa2D.Secante Then
                Return m.Interseccion
            Else
                Throw New ExcepcionGeometrica2D("RECTA2D (INTERSECCION): No se puede obtener la intersección de dos rectas que no son secantes." & vbNewLine _
                                                & "Posición relativa: " & m.ToString)
            End If
        End Function

        Public Shared Function RepresentacionMatricial(ByVal Recta As Recta2D) As Matriz()
            Dim Retorno(1) As Matriz

            Retorno(0) = Recta.PuntoDiretor.Matriz
            Retorno(1) = Recta.PuntoDeMira.Matriz

            Return Retorno
        End Function

        Public Shared Operator +(ByVal R1 As Recta2D, ByVal R2 As Recta2D) As Recta2D
            Return New Recta2D(R1.PuntoDiretor + R2.PuntoDiretor, R1.VectorDirector + R2.VectorDirector)
        End Operator

        Public Shared Operator -(ByVal R1 As Recta2D, ByVal R2 As Recta2D) As Recta2D
            Return New Recta2D(R1.PuntoDiretor - R2.PuntoDiretor, R1.VectorDirector - R2.VectorDirector)
        End Operator

        Public Shared Operator *(ByVal Recta As Recta2D, ByVal Valor As Double) As Recta2D
            Return New Recta2D(Recta.PuntoDiretor * Valor, Recta.VectorDirector)
        End Operator

        Public Shared Operator *(ByVal Valor As Double, ByVal Recta As Recta2D) As Recta2D
            Return New Recta2D(Recta.PuntoDiretor * Valor, Recta.VectorDirector)
        End Operator

        Public Shared Operator /(ByVal Recta As Recta2D, ByVal Valor As Double) As Recta2D
            Return New Recta2D(Recta.PuntoDiretor / Valor, Recta.VectorDirector)
        End Operator
    End Class
End Namespace

