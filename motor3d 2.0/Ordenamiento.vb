Namespace Utilidades
    Public Class Ordenamiento
        Public Sub New()
            MyBase.New()
        End Sub

        Public Shared Sub BubbleSort(Of Tipo As IComparable)(ByRef Array() As Tipo)
            Dim inter As Tipo
            For i As Integer = 0 To Array.GetUpperBound(0)
                For j As Integer = 0 To Array.GetUpperBound(0)
                    If Array(i).CompareTo(Array(j)) = 1 Then
                        inter = Array(i)
                        Array(i) = Array(j)
                        Array(j) = inter
                    End If
                Next
            Next
        End Sub

        Public Shared Sub Sort(Of Tipo As IComparable(Of Tipo))(ByRef Array() As Tipo)
            System.Array.Sort(Array)
        End Sub
    End Class
End Namespace
