Imports Motor3D.Espacio3D
Imports System.Drawing
Imports System.Math

Namespace Escena.Shading
    Public Class PhongShader
        Inherits ObjetoShading

        Private mAmbiente As Single
        Private mDifusa As Single
        Private mEspecular As Single
        Private mExponenteEspecular As Single

        Public Event AmbienteModificada(ByVal Sender As PhongShader)
        Public Event DifusaModificada(ByVal Sender As PhongShader)
        Public Event EspecularModificada(ByVal Sender As PhongShader)
        Public Event ExponenteModificado(ByVal Sender As PhongShader)

        Public Shadows Event Modificado(ByRef Sender As PhongShader)

        Public Property Ambiente() As Single
            Get
                Return mAmbiente
            End Get
            Set(ByVal value As Single)
                If value >= 0 AndAlso value <= 1 Then
                    mAmbiente = value
                    RaiseEvent AmbienteModificada(Me)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property Difusa() As Single
            Get
                Return mDifusa
            End Get
            Set(ByVal value As Single)
                If value >= 0 AndAlso value <= 1 Then
                    mDifusa = value
                    RaiseEvent DifusaModificada(Me)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property Especular() As Single
            Get
                Return mEspecular
            End Get
            Set(ByVal value As Single)
                If value >= 0 AndAlso value <= 1 Then
                    mEspecular = value
                    RaiseEvent EspecularModificada(Me)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Property ExponenteEspecular() As Single
            Get
                Return mExponenteEspecular
            End Get
            Set(ByVal value As Single)
                If value >= 0 Then
                    mExponenteEspecular = value
                    RaiseEvent ExponenteModificado(Me)
                    RaiseEvent Modificado(Me)
                End If
            End Set
        End Property

        Public Sub New()
            mAmbiente = 1
            mDifusa = 1
            mEspecular = 1
            mExponenteEspecular = 50
        End Sub

        Public Sub New(ByVal Difusa As Single, ByVal Especular As Single, ByVal ExponenteEspecular As Integer)
            If Difusa >= 0 AndAlso Difusa <= 1 Then mDifusa = Difusa
            If Especular >= 0 AndAlso Especular <= 1 Then mEspecular = Especular
            If ExponenteEspecular >= 0 Then mExponenteEspecular = ExponenteEspecular
        End Sub

        Public Function EcuacionPhong(ByVal Focos() As Foco3D, ByVal NormalSUR As Vector3D, ByVal PuntoSUR As Punto3D, ByVal Color As Color, ByVal Camara As Camara3D) As Color
            Return EcuacionPhong(Focos, Me, NormalSUR, PuntoSUR, Color, Camara)
        End Function

        Public Shared Function EcuacionPhong(ByVal Focos() As Foco3D, ByVal Constantes As PhongShader, ByVal NormalSUR As Vector3D, ByVal PuntoSUR As Punto3D, ByVal Color As Color, ByVal Camara As Camara3D) As Color
            Dim Rayo, Salida, Vista As Vector3D
            Dim r, g, b As Byte
            Dim rr, gg, bb As Long
            Dim trr, tgg, tbb As Long
            Dim Escalar As Double
            Dim Ambiente, Difusa, Especular As Double

            Dim AmbienteR, DifusaR, EspecularR As Double
            Dim AmbienteG, DifusaG, EspecularG As Double
            Dim AmbienteB, DifusaB, EspecularB As Double

            trr = 0
            tgg = 0
            tbb = 0

            Vista = New Vector3D(Camara.Posicion, PuntoSUR)
            Vista.Normalizar()

            Ambiente = Constantes.Ambiente
            NormalSUR.Normalizar()

            For i As Long = 0 To Focos.GetUpperBound(0)
                Rayo = New Vector3D(Focos(i).Coordenadas, PuntoSUR)
                Rayo = Not Rayo.VectorUnitario
                Salida = (((2 * (NormalSUR * Rayo)) * NormalSUR) - Rayo).VectorUnitario

                Difusa = Focos(i).Intensidad * (Constantes.Difusa * (NormalSUR * Rayo))
                Escalar = (Salida * Vista)
                If Escalar < 0 Then
                    Especular = Abs((Constantes.Especular * Escalar ^ Constantes.ExponenteEspecular))
                Else
                    Especular = 0
                End If

                AmbienteR = Ambiente * (Focos(i).Color.R / 255)
                DifusaR = Difusa * (Focos(i).Color.R / 255)
                EspecularR = Especular * (Focos(i).Color.R / 255)

                AmbienteG = Ambiente * (Focos(i).Color.G / 255)
                DifusaG = Difusa * (Focos(i).Color.G / 255)
                EspecularG = Especular * (Focos(i).Color.G / 255)

                AmbienteB = Ambiente * (Focos(i).Color.B / 255)
                DifusaB = Difusa * (Focos(i).Color.B / 255)
                EspecularB = Especular * (Focos(i).Color.B / 255)

                rr = Color.R * (AmbienteR + DifusaR)
                rr = rr + ((Focos(i).Color.R - rr) * EspecularR)

                gg = Color.G * (AmbienteG + DifusaG)
                gg = gg + ((Focos(i).Color.G - gg) * EspecularG)

                bb = Color.B * (AmbienteB + DifusaB)
                bb = bb + ((Focos(i).Color.B - bb) * EspecularB)

                If rr < 0 Then rr = 0
                If gg < 0 Then gg = 0
                If bb < 0 Then bb = 0

                trr += rr
                tgg += gg
                tbb += bb
            Next

            If trr > 255 Then trr = 255
            If trr < 0 Then trr = 0

            If tgg > 255 Then tgg = 255
            If tgg < 0 Then tgg = 0

            If tbb > 255 Then tbb = 255
            If tbb < 0 Then tbb = 0

            r = trr
            g = tgg
            b = tbb

            Return Color.FromArgb(255, r, g, b)
        End Function
    End Class
End Namespace

