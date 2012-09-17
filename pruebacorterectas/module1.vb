Imports Motor3D.Espacio2D

Module Module1
    Dim r, s As Recta2D
    Dim P1, P2, P3 As Punto2D

    Sub Main()
        Do While True
            Try
                Console.WriteLine("Motor3D Versión 2 - Prueba Recta2D")
                Console.WriteLine("==================================")
                Console.WriteLine("")

                P1 = New Punto2D
                P2 = New Punto2D

                Console.Write("Especifique la coordenada X de un punto: ")
                P1.X = Console.ReadLine()
                Console.Write("Especifique la coordenada Y de un punto: ")
                P1.Y = Console.ReadLine()
                Console.Write("Especifique la coordenada X de un punto: ")
                P2.X = Console.ReadLine()
                Console.Write("Especifique la coordenada Y de un punto: ")
                P2.Y = Console.ReadLine()

                r = New Recta2D(P1, P2)
                Console.WriteLine("")
                Console.WriteLine("Puntos: " & P1.ToString & " , " & P2.ToString & ". Recta obtenida: " & r.ToString & " (Pendiente=" & r.Pendiente & ")")


                Console.Write("Especifique la coordenada X de un punto: ")
                P1.X = Console.ReadLine()
                Console.Write("Especifique la coordenada Y de un punto: ")
                P1.Y = Console.ReadLine()
                Console.Write("Especifique la coordenada X de un punto: ")
                P2.X = Console.ReadLine()
                Console.Write("Especifique la coordenada Y de un punto: ")
                P2.Y = Console.ReadLine()

                s = New Recta2D(P1, P2)
                Console.WriteLine("")
                Console.WriteLine("Puntos: " & P1.ToString & " , " & P2.ToString & ". Recta obtenida: " & s.ToString & " (Pendiente=" & s.Pendiente.ToString & ")")

                Console.WriteLine("")
                Console.WriteLine("Posición relativa: " & Recta2D.PosicionRelativa(r, s).ToString)

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
