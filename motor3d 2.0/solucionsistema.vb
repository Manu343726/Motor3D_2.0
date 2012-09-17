Namespace Algebra
    Public Enum TipoSolucionSistema
        SistemaCompatibleDeterminado
        SistemaCompatibleIndeterminado
        SistemaIncompatible
    End Enum

    Public Class SolucionSistema
        Inherits ObjetoAlgebraico

        Private mSolucionCompatibleDeterminado() As Double
        Private mTipoSolucion As TipoSolucionSistema

        Public ReadOnly Property TipoSolucion() As TipoSolucionSistema
            Get
                Return mTipoSolucion
            End Get
        End Property

        Public ReadOnly Property ValorSolucion() As Double()
            Get
                Return mSolucionCompatibleDeterminado
            End Get
        End Property

        Public Sub New(ByVal ParamArray ValSolucion() As Double)
            mTipoSolucion = TipoSolucionSistema.SistemaCompatibleDeterminado
            mSolucionCompatibleDeterminado = ValSolucion
        End Sub

        Public Sub New(ByVal ValTipoSolucion As TipoSolucionSistema)
            mTipoSolucion = ValTipoSolucion
        End Sub

        Public Overrides Function ToString() As String
            Dim Retorno As String = ""

            If mTipoSolucion = TipoSolucionSistema.SistemaCompatibleDeterminado Then
                Retorno = "{Sistema compatible determinado.("
                For i As Integer = 0 To mSolucionCompatibleDeterminado.GetUpperBound(0)
                    If i < mSolucionCompatibleDeterminado.GetUpperBound(0) Then
                        Retorno &= mSolucionCompatibleDeterminado(i).ToString & ","
                    Else
                        Retorno &= mSolucionCompatibleDeterminado(i).ToString & ")}"
                    End If
                Next

                Return Retorno
            Else
                Return "{" & mTipoSolucion.ToString & "}"
            End If
        End Function
    End Class
End Namespace