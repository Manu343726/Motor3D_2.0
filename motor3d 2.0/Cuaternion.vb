Imports Motor3D.Espacio3D
Imports System.Math

Namespace Algebra
    Public Class Cuaternion
        Inherits ObjetoAlgebraico

        Private mA, mB, mC, mD As Double
        Private mVector As Vector3D
        Private mMatriz As Matriz

        Public ReadOnly Property A As Double
            Get
                Return mA
            End Get
        End Property

        Public ReadOnly Property B As Double
            Get
                Return mB
            End Get
        End Property

        Public ReadOnly Property C As Double
            Get
                Return mC
            End Get
        End Property

        Public ReadOnly Property D As Double
            Get
                Return mD
            End Get
        End Property

        Public ReadOnly Property Vector As Vector3D
            Get
                Return mVector
            End Get
        End Property

        Public ReadOnly Property Matriz As Matriz
            Get
                Return mMatriz
            End Get
        End Property

        Public ReadOnly Property Modulo As Double
            Get
                Return ObtenerModulo(Me)
            End Get
        End Property

        Public ReadOnly Property CuaternionUnitario As Cuaternion
            Get
                Dim m As Double = ObtenerModulo(Me)

                Return New Cuaternion(mA / m, mB / m, mC / m, mD / m)
            End Get
        End Property

        Public Sub New(Optional ByVal UsaMatriz As Boolean = True)
            mA = 0
            mB = 0
            mC = 0
            mD = 0

            EstablecerDatos(UsaMatriz)
        End Sub

        Public Sub New(ByVal A As Double, ByVal B As Double, ByVal C As Double, ByVal D As Double, Optional ByVal UsaMatriz As Boolean = True)
            mA = A
            mB = B
            mC = C
            mD = D

            EstablecerDatos(UsaMatriz)
        End Sub

        Public Sub New(ByVal Eje As Vector3D, ByVal Giro As Single, Optional ByVal UsaMatriz As Boolean = True)
            If Eje.Modulo > 1 Then
                Eje = Eje.VectorUnitario
            End If

            Eje *= Sin(Giro / 2)
            mA = Cos(Giro / 2)
            mB = Eje.X
            mC = Eje.Y
            mD = Eje.Z

            EstablecerDatos(UsaMatriz)
        End Sub

        Public Sub New(ByVal Punto As Punto3D, Optional ByVal UsaMatriz As Boolean = True)
            Me.New(0, Punto.X, Punto.Y, Punto.Z)
        End Sub

        Public Sub New(ByVal Cabeceo As Single, ByVal Alabeo As Single, ByVal Guiñada As Single, Optional ByVal UsaMatriz As Boolean = True)
            Dim sc, sg, sa, cc, cg, ca As Double
            Dim cccg, ccsg, scsg, sccg As Double

            sc = Sin(Cabeceo / 2)
            sa = Sin(Alabeo / 2)
            sg = Sin(Guiñada / 2)
            cc = Cos(Cabeceo / 2)
            ca = Cos(Alabeo / 2)
            cg = Cos(Guiñada / 2)

            cccg = cc * cg
            ccsg = cc * sg
            scsg = sc * sg
            sccg = sc * cg

            mA = (cccg * ca) + (scsg * sa)
            mB = (cccg * sa) - (scsg * ca)
            mC = (cccg * ca) + (ccsg * sa)
            mD = (ccsg * ca) - (sccg * sa)

            EstablecerDatos(UsaMatriz)
        End Sub

        Public Sub New(ByVal Angulos As AngulosEuler, Optional ByVal UsaMatriz As Boolean = True)
            Me.New(Angulos.Cabeceo, Angulos.Alabeo, Angulos.Guiñada, UsaMatriz)
        End Sub

        Protected Overridable Sub EstablecerDatos(Optional ByVal UsaMatriz As Boolean = True)
            Dim m, xx, yy, zz, xy, xz, yz, ax, ay, az As Double

            m = ObtenerModulo(Me)

            mA /= m
            mB /= m
            mC /= m
            mD /= m

            mVector = New Vector3D(mB, mC, mD)

            If UsaMatriz Then
                xx = mB * mB
                yy = mC * mC
                zz = mD * mD
                xy = mB * mC
                xz = mB * mD
                yz = mC * mD
                ax = mA * mB
                ay = mA * mC
                az = mA * mD

                mMatriz = New Matriz(4, 4)
                mMatriz.EstablecerValoresPorFila(0, 1 - (2 * yy) - (2 * zz), (2 * xy) - (2 * az), (2 * xz) + (2 * ay), 0)
                mMatriz.EstablecerValoresPorFila(1, (2 * xy) + (2 * az), 1 - (2 * xx) - (2 * zz), (2 * yz) - (2 * ax), 0)
                mMatriz.EstablecerValoresPorFila(2, (2 * xz) - (2 * ay), (2 * yz) + (2 * ax), 1 - (2 * xx) - (2 * yy), 0)
                mMatriz.EstablecerValoresPorFila(3, 0, 0, 0, 1)
            End If
        End Sub

        Public Shared Function Suma(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return New Cuaternion(Q1.A + Q2.A, Q1.B + Q2.B, Q1.C + Q2.C, Q1.D + Q2.D)
        End Function

        Public Shared Function Resta(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return New Cuaternion(Q1.A - Q2.A, Q1.B - Q2.B, Q1.C - Q2.C, Q1.D - Q2.D)
        End Function

        Public Shared Function Producto(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return New Cuaternion((Q1.A * Q2.A) - (Q1.B * Q2.B) - (Q1.C * Q2.C) - (Q1.D * Q2.D), _
                                  (Q1.A * Q2.B) + (Q1.B * Q2.A) + (Q1.C * Q2.D) - (Q1.D * Q2.C), _
                                  (Q1.A * Q2.C) - (Q1.B * Q2.D) + (Q1.C * Q2.A) + (Q1.D * Q2.B), _
                                  (Q1.A * Q2.D) + (Q1.B + Q2.C) - (Q1.C * Q2.B) + (Q1.D * Q2.A))
        End Function

        Public Shared Function Producto(ByVal Cuaternion As Cuaternion, ByVal Vector As Vector3D) As Cuaternion
            Return New Cuaternion(-(Cuaternion.B * Vector.X) - (Cuaternion.C * Vector.Y) - (Cuaternion.D * Vector.Z), _
                                   (Cuaternion.A * Vector.X) + (Cuaternion.C * Vector.Z) - (Cuaternion.D * Vector.Y), _
                                   (Cuaternion.A * Vector.Y) - (Cuaternion.B * Vector.Z) + (Cuaternion.D * Vector.X), _
                                   (Cuaternion.A * Vector.Z) + (Cuaternion.B + Vector.Y) - (Cuaternion.C * Vector.X))
        End Function

        Public Shared Function Producto(ByVal Cuaternion As Cuaternion, ByVal K As Double) As Cuaternion
            Return New Cuaternion(Cuaternion.A * K, Cuaternion.B * K, Cuaternion.C * K, Cuaternion.D * K)
        End Function

        Public Shared Function Conjugado(ByVal Cuaternion As Cuaternion) As Cuaternion
            Return New Cuaternion(Cuaternion.A, -Cuaternion.B, -Cuaternion.C, -Cuaternion.D)
        End Function

        Public Shared Function Inverso(ByVal Cuaternion As Cuaternion) As Cuaternion
            Return Cociente(Conjugado(Cuaternion), (Cuaternion.Modulo ^ 2))
        End Function

        Public Shared Function Cociente(ByVal Cuaternion As Cuaternion, ByVal K As Double)
            Return New Cuaternion(Cuaternion.A / K, Cuaternion.B / K, Cuaternion.C / K, Cuaternion.D / K)
        End Function

        Public Shared Function Cociente(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Producto(Q1, Inverso(Q2))
        End Function

        Public Shared Function ObtenerRotacion(ByVal Cuaternion As Cuaternion) As Single
            Return 2 * (Acos(Cuaternion.A))
        End Function

        Public Shared Function ObtenerPunto(ByVal Cuaternion As Cuaternion) As Punto3D
            Return New Punto3D(Cuaternion.Vector.Matriz)
        End Function

        Public Shared Function ObtenerVector(ByVal Cuaternion As Cuaternion) As Vector3D
            Return Cuaternion.Vector
        End Function

        Public Shared Function ObtenerModulo(ByVal Cuaternion As Cuaternion) As Double
            Return Sqrt((Cuaternion.A * Cuaternion.A) + (Cuaternion.B * Cuaternion.B) + (Cuaternion.C * Cuaternion.C) + (Cuaternion.D * Cuaternion.D))
        End Function

        Public Shared Function ObtenerCuadradoModulo(ByVal Cuaternion As Cuaternion) As Double
            Return (Cuaternion.A * Cuaternion.A) + (Cuaternion.B * Cuaternion.B) + (Cuaternion.C * Cuaternion.C) + (Cuaternion.D * Cuaternion.D)
        End Function

        Public Shared Operator Not(ByVal Cuaternion As Cuaternion) As Cuaternion
            Return Conjugado(Cuaternion)
        End Operator

        Public Shared Operator +(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Suma(Q1, Q2)
        End Operator

        Public Shared Operator -(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Resta(Q1, Q2)
        End Operator

        Public Shared Operator *(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Producto(Q1, Q2)
        End Operator

        Public Shared Operator *(ByVal Cuaternion As Cuaternion, ByVal K As Double) As Cuaternion
            Return Producto(Cuaternion, K)
        End Operator

        Public Shared Operator *(ByVal K As Double, ByVal Cuaternion As Cuaternion) As Cuaternion
            Return Producto(Cuaternion, K)
        End Operator

        Public Shared Operator /(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Cociente(Q1, Q2)
        End Operator

        Public Shared Operator /(ByVal Cuaternion As Cuaternion, ByVal K As Double) As Cuaternion
            Return Cociente(Cuaternion, K)
        End Operator

        Public Shared Function Rotacion(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion) As Cuaternion
            Return Q1 * Q2 * Not Q1
        End Function

        Public Shared Function Rotacion(ByVal Q1 As Cuaternion, ByVal Q2 As Cuaternion, ByVal Traslacion As Cuaternion) As Cuaternion
            Return (Q1 * (Q2 - Traslacion) * Not Q1) + Traslacion
        End Function

        Public Overrides Function ToString() As String
            Return "{Cuaternion: " & mA & IIf(mB >= 0, "+", "") & mB & "i" & IIf(mC >= 0, "+", "") & mC & "j" & IIf(mD >= 0, "+", "") & mD & "k}"
        End Function
    End Class
End Namespace

