Imports Motor3D.Espacio3D
Imports System.Drawing

Namespace Escena
    Public Class Foco3D
        Inherits ObjetoEscena

        Private mCoordenadasSUR As Punto3D
        Private mColor As Color
        Private mIntensidad As Single

        Public Shadows Event Modificado(ByRef Sender As Foco3D)

        Public Property Coordenadas() As Punto3D
            Get
                Return mCoordenadasSUR
            End Get
            Set(ByVal value As Punto3D)
                mCoordenadasSUR = value
                RaiseEvent Modificado(Me)
            End Set
        End Property

        Public Property Color() As Color
            Get
                Return mColor
            End Get
            Set(ByVal value As Color)
                If value <> Drawing.Color.Black AndAlso value.A = 255 Then
                    mColor = value
                    RaiseEvent Modificado(Me)
                Else
                    Throw New ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." & vbNewLine _
                                                  & "Color=ARGB(" & value.A & "," & value.R & "," & value.G & "," & value.B & ")")
                End If
            End Set
        End Property

        Public Property Intensidad() As Single
            Get
                Return mIntensidad
            End Get
            Set(ByVal value As Single)
                If value >= 0 AndAlso value <= 1 Then
                    mIntensidad = value
                Else
                    Throw New ExcepcionEscena("FOCO3D (INTENSIDAD_SET): La intensidad debe estar entre 0 y 1." & vbNewLine _
                                                  & "Intensidad=" & value)
                End If
            End Set
        End Property

        Public Sub New(ByVal Posicion As Punto3D, ByVal Color As Color)
            If Color <> Drawing.Color.Black AndAlso Color.A = 255 Then
                mCoordenadasSUR = Posicion
                mColor = Color
                mIntensidad = 1
            Else
                Throw New ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." & vbNewLine _
                                                  & "Color=ARGB(" & Color.A & "," & Color.R & "," & Color.G & "," & Color.B & ")")
            End If
        End Sub

        Public Sub New(ByVal Posicion As Punto3D, ByVal Color As Color, ByVal Intensidad As Single)
            If Intensidad >= 0 AndAlso Intensidad <= 1 Then
                If Color <> Drawing.Color.Black AndAlso Color.A = 255 Then
                    mCoordenadasSUR = Posicion
                    mColor = Color
                    mIntensidad = Intensidad
                Else
                    Throw New ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." & vbNewLine _
                                                      & "Color=ARGB(" & Color.A & "," & Color.R & "," & Color.G & "," & Color.B & ")")
                End If
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return "{Foco. Posicion=" & mCoordenadasSUR.ToString & " , Color=ARGB(" & mColor.A & "," & mColor.R & "," & mColor.G & "," & mColor.B & ") , Intensidad=" & mIntensidad.ToString & "}"
        End Function

        Public Shared Operator =(ByVal F1 As Foco3D, ByVal F2 As Foco3D) As Boolean
            Return F1.Equals(F2)
        End Operator

        Public Shared Operator <>(ByVal F1 As Foco3D, ByVal F2 As Foco3D) As Boolean
            Return Not (F1 = F2)
        End Operator
    End Class
End Namespace

