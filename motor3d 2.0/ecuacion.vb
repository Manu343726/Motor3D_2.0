Namespace Algebra
    Public Class Ecuacion
        Inherits ObjetoAlgebraico

        Private mVariables() As Double

        Public ReadOnly Property Variables() As Double()
            Get
                Return mVariables
            End Get
        End Property

        Public ReadOnly Property NumeroVariables() As Integer
            Get
                Return mVariables.GetUpperBound(0)
            End Get
        End Property

        Public ReadOnly Property TerminoIndependiente As Double
            Get
                Return mVariables(mVariables.GetUpperBound(0))
            End Get
        End Property

        Public Sub New(ByVal ParamArray ValVariables() As Double)
            mVariables = ValVariables
        End Sub

        Public Overrides Function ToString() As String
            Dim Retorno As String = ""
            For i As Integer = 0 To mVariables.GetUpperBound(0)
                If i = 0 Then
                    Retorno &= mVariables(i).ToString & LetraVariable(i)
                Else
                    If i < mVariables.GetUpperBound(0) Then
                        Retorno &= IIf(mVariables(i) >= 0, "+", "") & mVariables(i).ToString & LetraVariable(i)
                    Else
                        Retorno &= "=" & mVariables(i).ToString
                    End If
                End If
            Next

            Return Retorno
        End Function

        Public Function Copia() As Ecuacion
            Return Copia(Me)
        End Function

        Public Shared Function Copia(ByVal Ecuacion As Ecuacion) As Ecuacion
            Return New Ecuacion(Ecuacion.Variables)
        End Function

        Public Shared Function LetraVariable(ByVal IndiceVariable As Integer) As String
            Select Case IndiceVariable
                Case 0
                    Return "X"
                Case 1
                    Return "Y"
                Case 2
                    Return "Z"
                Case 3
                    Return "W"
                Case Else
                    Return "[VAR" & IndiceVariable & "]"
            End Select
        End Function
    End Class

End Namespace
