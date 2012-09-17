Imports Motor3D.Algebra

Module Module1
    Dim M As Matriz
    Dim fils, cols As Integer

    Dim Fila() As Double

    Sub Main()
        Do While True
            Try
                Console.WriteLine("Motor3D Versión 2 - Prueba matrices 2")
                Console.WriteLine("=====================================")
                Console.WriteLine("")

                Console.Write("Especifique el número de filas de la matriz: ")
                fils = Console.ReadLine
                Console.Write("Especifique el número de columnas de la matriz: ")
                cols = Console.ReadLine

                M = New Matriz(fils, cols)

                For i As Integer = 0 To fils - 1
                    Console.WriteLine("FILA (" & i & "):")
                    ReDim Fila(cols)
                    For j As Integer = 0 To cols - 1
                        Console.Write(" - Especifique el término (" & j & ") de ésta fila: ")

                        Fila(j) = Console.ReadLine
                    Next
                    M.EstablecerValoresPorFila(i, Fila)
                Next

                Console.WriteLine("MATRIZ OBTENIDA:")
                Console.WriteLine(M.ToString)
                Console.WriteLine("")
                Console.WriteLine("RESULTADOS:")
                Try
                    Console.WriteLine(" - Determuinante=" & Matriz.CalculoDeterminante(M).ToString)
                Catch ex As ExcepcionMatriz
                    Console.WriteLine(" - Determinante=NO DISPONIBLE")
                End Try
                Console.WriteLine(" - Rango=" & Matriz.Rango(M).ToString)
                Console.WriteLine(" - Matriz traspuesta: ")
                Try
                    Console.WriteLine(Matriz.Transpuesta(M).ToString)
                Catch ex As ExcepcionMatriz
                    Console.WriteLine("NO DISPONIBLE")
                End Try
                Console.WriteLine(" - Matriz adjunta: ")
                Try
                    Console.WriteLine(Matriz.Adjunta(M).ToString)
                Catch ex As ExcepcionMatriz
                    Console.WriteLine("NO DISPONIBLE")
                End Try
                Console.WriteLine(" - Matriz inversa: ")
                Try
                    Console.WriteLine(M.Inversa.ToString)
                Catch ex As ExcepcionMatriz
                    Console.WriteLine("NO DISPONIBLE")
                End Try

                Console.WriteLine("")
                Console.WriteLine("TEST FINALIZADO. Presione cualquier tecla para continuar...")
                Console.ReadKey()
            Catch ex As Exception
                Console.WriteLine("ERROR: " & ex.Message)
                Console.WriteLine("Pulse cualquier tecla para continuar...")
                Console.ReadKey()
            End Try

            Console.Clear()
        Loop
    End Sub

End Module
