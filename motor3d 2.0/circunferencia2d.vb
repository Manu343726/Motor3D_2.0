Imports System.Math

Namespace Espacio2D
    Public Class Circunferencia2D
        Inherits ObjetoGeometrico2D

        Private mRadio As Double
        Private CuadradoRadio As Double
        Private mCentro As Punto2D

        Private mA, mB, mC As Double

        Public Shadows Event Modificado(ByRef Sebder As Circunferencia2D)

        Public Property Radio() As Double
            Get
                Return mRadio
            End Get

            Set(ByVal value As Double)
                If value <> mRadio Then
                    mRadio = value
                    CuadradoRadio = mRadio ^ 2
                    mC = (mCentro.X ^ 2) + (mCentro.Y ^ 2) + (mRadio ^ 2)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property Centro() As Punto2D
            Get
                Return mCentro
            End Get

            Set(ByVal value As Punto2D)
                If value <> mCentro Then
                    mCentro = value
                    CalculoCoeficientes()
                    RaiseEvent Modificado(Me)
                End If
            End Set
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

        Public Sub New(ByVal ValCentro As Punto2D, ByVal ValRadio As Double)
            mRadio = ValRadio
            mCentro = ValCentro
            CalculoCoeficientes()
        End Sub

        Public Sub New(ByVal P1 As Punto2D, ByVal P2 As Punto2D, ByVal P3 As Punto2D)
            mCentro = New Punto2D((P1.X + P2.X + P3.X) / 3, (P1.Y + P2.Y + P3.Y) / 3)
            CuadradoRadio = ((P1.X - mCentro.X) ^ 2) + ((P1.Y - mCentro.Y) ^ 2)
            mRadio = Sqrt(CuadradoRadio)
            CalculoCoeficientes()
        End Sub

        Public Sub New(ByVal AABB As AABB2D)
            mCentro = AABB.Centro
            mRadio = IIf(AABB.LongitudX >= AABB.LongitudY, AABB.LongitudX / 2, AABB.LongitudY / 2)
            CuadradoRadio = mRadio ^ 2
            CalculoCoeficientes()
        End Sub

        Private Sub CalculoCoeficientes()
            mA = -2 * mCentro.X
            mB = -2 * mCentro.Y
            mC = (mCentro.X ^ 2) + (mCentro.Y ^ 2) - (mRadio ^ 2)
            CuadradoRadio = mRadio ^ 2
        End Sub

        Public Function Pertenece(ByVal Punto As Punto2D) As Boolean
            Return (Punto.X ^ 2) + (Punto.Y ^ 2) + (mA * Punto.X) + (mB * Punto.Y) + mC = 0
        End Function

        Public Function ContenidoEnCirculo(ByVal Punto As Punto2D) As Boolean
            Return (Punto.X ^ 2) + (Punto.Y ^ 2) + (mA * Punto.X) + (mB * Punto.Y) + mC < 0
        End Function

        Public Function PosicionRelativa(ByVal Punto As Punto2D) As Double
            Return (Punto.X ^ 2) + (Punto.Y ^ 2) + (mA * Punto.X) + (mB * Punto.Y) + mC
        End Function

        Public Shared Function PosicionRelativa(ByVal C1 As Circunferencia2D, ByVal C2 As Circunferencia2D) As Double
            Return Sqrt(((C2.Centro.X - C1.Centro.X) ^ 2) + ((C2.Centro.Y - C1.Centro.Y) ^ 2)) - (C1.Radio + C2.Radio)
        End Function

        Public Shared Function EjeRadical(ByVal C1 As Circunferencia2D, ByVal C2 As Circunferencia2D) As Recta2D
            Return New Recta2D(C1.A - C2.A, C1.B - C2.B, C1.C - C2.C)
        End Function

        Public Overrides Function ToString() As String
            Return "[Centro=" & mCentro.ToString & "," & "Radio=" & mRadio.ToString & "]"
        End Function

        Public Overloads Function ToString(ByVal Algebraica As Boolean) As String
            If Algebraica Then
                Return "X^2+Y^2" & IIf(mA > 0, "+", "") & FormatNumber(mA, 2) & "X" & IIf(mB >= 0, "+" & FormatNumber(mB, 2), FormatNumber(mB, 2)) & "Y" & IIf(mC >= 0, "+" & FormatNumber(mC, 2), FormatNumber(mC, 2)) & "=0"
            Else
                Return "[Centro=" & mCentro.ToString & "," & "Radio=" & mRadio.ToString & "]"
            End If
        End Function

        Public Shared Function Colisionan(ByVal C1 As Circunferencia2D, ByVal C2 As Circunferencia2D) As Boolean
            Return (PosicionRelativa(C1, C2) <= 0)
        End Function
    End Class
End Namespace

