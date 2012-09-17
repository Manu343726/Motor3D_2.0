Imports Motor3D.Algebra

Module Module1
    Dim sis As SistemaEcuaciones
    Dim necs, nincs As Integer

    Dim ec As Ecuacion
    Dim terms() As Double
    Dim Ecs() As Ecuacion

    Sub Main()
        Do While True
            Try
                Console.WriteLine("Motor3D Versión 2 - Prueba sistemas")
                Console.WriteLine("===================================")
                Console.WriteLine("")

                Console.Write("Especifique el número de incógnitas del sistema: ")
                nincs = Console.ReadLine
                Console.Write("Especifique el número de ecuaciones del sistema: ")
                necs = Console.ReadLine

                ReDim terms(nincs)
                ReDim Ecs(necs - 1)

                For i As Integer = 0 To necs - 1
                    Console.WriteLine("ECUACION (" & i & "):")
                    ReDim terms(nincs)
                    For j As Integer = 0 To nincs
                        If j < nincs Then
                            Console.Write(" - Especifique el término de la variable " & Ecuacion.LetraVariable(j) & " de ésta ecuación: ")
                        Else
                            Console.Write(" - Especifique el término independiente de ésta ecuación: ")
                        End If
                        terms(j) = Console.ReadLine
                    Next
                    Ecs(i) = New Ecuacion(terms)
                    Console.WriteLine("Ecuación obtenida: " & Ecs(i).ToString & vbNewLine)
                Next

                sis = New SistemaEcuaciones(Ecs)

                Console.WriteLine("SISTEMA OBTENIDO:")
                Console.WriteLine(sis.ToString)
                Console.WriteLine("")
                Console.WriteLine("RESULTADOS:")
                Console.WriteLine(" - Rango matriz principal=" & sis.RangoMatrizPrincipal.ToString)
                Console.WriteLine(" - Rango matriz ampliada=" & sis.RangoMatrizAmpliada.ToString)
                Console.WriteLine(" - Solución: " & sis.Solucion.ToString)

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
