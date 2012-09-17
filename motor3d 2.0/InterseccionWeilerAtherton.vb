Namespace Espacio2D
    Public Enum TipoInterseccionWeilerAtherton
        ENTRANTE
        SALIENTE
        DESCONOCIDO
    End Enum

    Public Structure InterseccionWeilerAtherton
        Implements IComparable(Of InterseccionWeilerAtherton)

        Private Punto As Punto2D
        Private Parametro As Double
        Private Tipo As TipoInterseccionWeilerAtherton

        Public Sub New(ByVal ValPunto As Punto2D, ByVal ValParametro As Double, ByVal ValTipo As TipoInterseccionWeilerAtherton)
            Punto = ValPunto
            Parametro = ValParametro
            Tipo = ValTipo
        End Sub

        Public Sub New(ByVal ValPunto As Punto2D, ByVal ValParametro As Double)
            Punto = ValPunto
            Parametro = ValParametro
            Tipo = TipoInterseccionWeilerAtherton.DESCONOCIDO
        End Sub

        Public Function CompareTo(other As InterseccionWeilerAtherton) As Integer Implements System.IComparable(Of InterseccionWeilerAtherton).CompareTo
            Select Case other.Parametro
                Case Is < Parametro
                    Return -1
                Case Is = Parametro
                    Return 0
                Case Else
                    Return 1
            End Select
        End Function


    End Structure
End Namespace

