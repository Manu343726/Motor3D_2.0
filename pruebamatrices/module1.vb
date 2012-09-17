Imports Motor3D
Imports Motor3D.Algebra
Imports Motor3D.Algebra.Matriz

Module Module1
    Dim M As Matriz = MatrizUnitaria(3)
    Dim Sis As SistemaEcuaciones

    Sub Main()
        M = New Matriz(4, 5) 'LIBRO MATES, PÁGINA 90, EJERCICIO RESUELTO 1 (RESULTADO: RANGO=2)

        M.EstablecerValoresPorFila(0, -1, 3, 0, 1, 2)
        M.EstablecerValoresPorFila(1, 0, 5, 1, 2, 3)
        M.EstablecerValoresPorFila(2, -3, -1, -2, -1, 0)
        M.EstablecerValoresPorFila(3, 3, 11, 4, 5, 6)

        Console.WriteLine("Motor3D Versión 2 - Prueba Matrices")
        Console.WriteLine("===================================")

        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("MATES 2º, PAG 114, EJERCICIO RESUELTO 1,apartado a). (SOLUCION: SCD, (1,-5))")
        Console.WriteLine("////////////////////////////////////////////////////////////////////////////")
        Console.WriteLine("")
        Sis = New SistemaEcuaciones(New Ecuacion(1, -1, 6), New Ecuacion(4, 1, -1), New Ecuacion(5, 2, -5))
        Console.WriteLine(Sis.ToString)
        Console.WriteLine("")
        Console.WriteLine("Matriz principal: " & vbNewLine & Sis.MatrizPrincipal.ToString)
        Console.WriteLine("Matriz ampliada: " & vbNewLine & Sis.MatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("Número de ecuaciones=" & Sis.NumeroEcuaciones.ToString)
        Console.WriteLine("Número de incógnitas=" & Sis.NumeroIncognitas.ToString)
        Console.WriteLine("Rango matriz principal=" & Sis.RangoMatrizPrincipal.ToString)
        Console.WriteLine("Rango matriz ampliada=" & Sis.RangoMatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("SOLUCION: " & Sis.Solucion.ToString)

        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("MATES 2º, PAG 117, EJERCICIO RESUELTO 4,apartado a). (SOLUCION: SCD, (1,1,1))")
        Console.WriteLine("/////////////////////////////////////////////////////////////////////////////")
        Console.WriteLine("")
        Sis = New SistemaEcuaciones(New Ecuacion(1, 1, 1, 3), New Ecuacion(2, -1, 1, 2), New Ecuacion(1, -1, 1, 1))
        Console.WriteLine(Sis.ToString)
        Console.WriteLine("")
        Console.WriteLine("Matriz principal: " & vbNewLine & Sis.MatrizPrincipal.ToString)
        Console.WriteLine("Matriz ampliada: " & vbNewLine & Sis.MatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("Número de ecuaciones=" & Sis.NumeroEcuaciones.ToString)
        Console.WriteLine("Número de incógnitas=" & Sis.NumeroIncognitas.ToString)
        Console.WriteLine("Rango matriz principal=" & Sis.RangoMatrizPrincipal.ToString)
        Console.WriteLine("Rango matriz ampliada=" & Sis.RangoMatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("SOLUCION: " & Sis.Solucion.ToString)

        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("PROBLEMAS ALGEBRA Y ANALITICA,PAG 461,EJERCICIO 3. (SOLUCION: SCD, (-1,-2,6))")
        Console.WriteLine("/////////////////////////////////////////////////////////////////////////////")
        Console.WriteLine("")
        Sis = New SistemaEcuaciones(New Ecuacion(1, 1, 1, 3), New Ecuacion(2, 1, 1, 2), New Ecuacion(1, 2, 1, 1))
        Console.WriteLine(Sis.ToString)
        Console.WriteLine("")
        Console.WriteLine("Matriz principal: " & vbNewLine & Sis.MatrizPrincipal.ToString)
        Console.WriteLine("Matriz ampliada: " & vbNewLine & Sis.MatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("Número de ecuaciones=" & Sis.NumeroEcuaciones.ToString)
        Console.WriteLine("Número de incógnitas=" & Sis.NumeroIncognitas.ToString)
        Console.WriteLine("Rango matriz principal=" & Sis.RangoMatrizPrincipal.ToString)
        Console.WriteLine("Rango matriz ampliada=" & Sis.RangoMatrizAmpliada.ToString)
        Console.WriteLine("")
        Console.WriteLine("SOLUCION: " & Sis.Solucion.ToString)

        Console.WriteLine("")
        Console.WriteLine("TEST FINALIZADO. Presione cualquier tecla para continuar...")
        Console.ReadKey()
    End Sub
End Module
