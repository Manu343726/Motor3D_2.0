Imports System.Math

Namespace Algebra
    Public Class Matriz
        Inherits ObjetoAlgebraico

        Private mMatriz(,) As Double

        Public Shadows Event Modificado(ByRef Sender As Matriz)

        Public ReadOnly Property Matriz() As Double(,)
            Get
                Return mMatriz
            End Get
        End Property

        Default Public Property Valor(ByVal X As Integer, ByVal Y As Integer) As Double
            Get
                Return mMatriz(X, Y)
            End Get
            Set(value As Double)
                mMatriz(X, Y) = value
            End Set
        End Property

        Public ReadOnly Property Array As Double(,)
            Get
                Return mMatriz
            End Get
        End Property

        Public Property Valores(ByVal X As Integer, ByVal Y As Integer) As Double
            Get
                If X >= 0 AndAlso X <= mMatriz.GetUpperBound(0) AndAlso Y >= 0 AndAlso Y <= mMatriz.GetUpperBound(1) Then
                    Return mMatriz(X, Y)
                Else
                    Throw New ExcepcionMatriz("MATRIZ (VALORES_GET): El índice está fuera de los límites de la matriz." & vbNewLine _
                                            & "Dimensiones de la matriz: " & Filas.ToString & "x" & Columnas.ToString & vbNewLine _
                                            & "Primer índice: " & X.ToString & vbNewLine _
                                            & "Segundo índice: " & Y.ToString)
                End If
            End Get
            Set(ByVal value As Double)
                If X >= 0 AndAlso X <= mMatriz.GetUpperBound(0) AndAlso Y >= 0 AndAlso Y <= mMatriz.GetUpperBound(1) Then
                    mMatriz(X, Y) = value
                    RaiseEvent Modificado(Me)
                Else
                    Throw New ExcepcionMatriz("MATRIZ (VALORES_SET): El índice está fuera de los límites de la matriz." & vbNewLine _
                                            & "Dimensiones de la matriz: " & Filas.ToString & "x" & Columnas.ToString & vbNewLine _
                                            & "Primer índice: " & X.ToString & vbNewLine _
                                            & "Segundo índice: " & Y.ToString)
                End If
            End Set
        End Property

        Public ReadOnly Property Filas() As Integer
            Get
                Return mMatriz.GetUpperBound(0) + 1
            End Get
        End Property

        Public ReadOnly Property Columnas() As Integer
            Get
                Return mMatriz.GetUpperBound(1) + 1
            End Get
        End Property

        Public ReadOnly Property EsCuadrada() As Boolean
            Get
                Return (mMatriz.GetUpperBound(0) = mMatriz.GetUpperBound(1))
            End Get
        End Property

        Public ReadOnly Property Inversa As Matriz
            Get
                Return CalculoInversa(Me)
            End Get
        End Property

        Public ReadOnly Property Determinante As Double
            Get
                Return CalculoDeterminante(Me)
            End Get
        End Property

        Public Sub New(ByVal ValFilas As Integer, ByVal ValColumnas As Integer)
            If ValFilas < 1 Then ValFilas = 1
            If ValColumnas < 1 Then ValColumnas = 1

            ReDim mMatriz(ValFilas - 1, ValColumnas - 1)
        End Sub

        Public Sub New(ByRef ValMatriz(,) As Double)
            mMatriz = ValMatriz
        End Sub

        Public Sub EstablecerValor(ByVal Fila As Integer, ByVal Columna As Integer, ByVal Valor As Double)
            If Fila >= 0 AndAlso Columna >= 0 AndAlso Fila <= mMatriz.GetUpperBound(0) AndAlso Columna <= mMatriz.GetUpperBound(1) Then
                mMatriz(Fila, Columna) = Valor
                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Sub EstablecerValoresPorFila(ByVal Fila As Integer, ByVal ParamArray Valores() As Double)
            If Fila >= 0 AndAlso Fila <= mMatriz.GetUpperBound(0) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(1)
                    If i <= Valores.GetUpperBound(0) Then
                        EstablecerValor(Fila, i, Valores(i))
                    Else
                        EstablecerValor(Fila, i, 0)
                    End If
                Next
                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Sub EstablecerValoresPorColumna(ByVal Columna As Integer, ByVal ParamArray Valores() As Double)
            If Columna >= 0 AndAlso Columna <= mMatriz.GetUpperBound(1) Then
                For j As Integer = 0 To mMatriz.GetUpperBound(0)
                    If j <= Valores.GetUpperBound(0) Then
                        EstablecerValor(j, Columna, Valores(j))
                    Else
                        EstablecerValor(j, Columna, 0)
                    End If
                Next
                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Function ObtenerValor(ByVal Fila As Integer, ByVal Columna As Integer) As Double
            If Fila >= 0 AndAlso Columna >= 0 AndAlso Fila <= mMatriz.GetUpperBound(0) AndAlso Columna <= mMatriz.GetUpperBound(1) Then
                Return mMatriz(Fila, Columna)
            End If
        End Function

        Public Function ObtenerFila(ByVal Fila As Integer) As Double()
            Dim Retorno(mMatriz.GetUpperBound(1)) As Double

            If Fila >= 0 AndAlso Fila < Filas Then
                For i As Integer = 0 To Retorno.GetUpperBound(0)
                    Retorno(i) = mMatriz(Fila, i)
                Next

                Return Retorno
            Else
                Throw New ExcepcionMatriz("MATRIZ (OBTENER FILA): La fila especificada está fuera del rango." & vbNewLine _
                                          & "Dimensiones de la matriz: " & Filas.ToString & "x" & Columnas.ToString & vbNewLine _
                                          & "Fila especificada: " & Fila.ToString)
            End If
        End Function

        Public Function ObtenerColumna(ByVal Columna As Integer) As Double()
            Dim Retorno(mMatriz.GetUpperBound(0)) As Double

            If Columna >= 0 AndAlso Columna < Columnas Then
                For i As Integer = 0 To Retorno.GetUpperBound(0)
                    Retorno(i) = mMatriz(i, Columna)
                Next

                Return Retorno
            Else
                Throw New ExcepcionMatriz("MATRIZ (OBTENER COLUMNA): La columna especificada está fuera del rango." & vbNewLine _
                                          & "Dimensiones de la matriz: " & Columnas.ToString & "x" & Columnas.ToString & vbNewLine _
                                          & "Columna especificada: " & Columna.ToString)
            End If
        End Function

        Public Function IntercambiarFilas(ByVal F1 As Integer, ByVal F2 As Integer) As Boolean
            Dim aux As Double

            If F1 <> F2 AndAlso F1 >= 0 AndAlso F1 <= mMatriz.GetUpperBound(0) AndAlso F2 >= 0 AndAlso F2 <= mMatriz.GetUpperBound(0) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(1)
                    aux = mMatriz(F1, i)
                    mMatriz(F1, i) = mMatriz(F2, i)
                    mMatriz(F2, i) = aux
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function IntercambiarColumnas(ByVal C1 As Integer, ByVal C2 As Integer) As Boolean
            Dim aux As Double

            If C1 <> C2 AndAlso C1 >= 0 AndAlso C1 <= mMatriz.GetUpperBound(1) AndAlso C2 >= 0 AndAlso C2 <= mMatriz.GetUpperBound(1) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(0)
                    aux = mMatriz(i, C1)
                    mMatriz(i, C1) = mMatriz(i, C2)
                    mMatriz(i, C2) = aux
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function ProductoFila(ByVal Fila As Integer, ByVal Valor As Double) As Boolean
            If Fila >= 0 AndAlso Fila <= mMatriz.GetUpperBound(0) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(1)
                    mMatriz(Fila, i) *= Valor
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function ProductoColumna(ByVal Columna As Integer, ByVal Valor As Double) As Boolean
            If Columna >= 0 AndAlso Columna <= mMatriz.GetUpperBound(1) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(0)
                    mMatriz(i, Columna) *= Valor
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function ProductoFila(ByVal FilaBase As Integer, ByVal Fila As Integer, ByVal Valor As Double) As Boolean
            If FilaBase <> Fila AndAlso FilaBase >= 0 AndAlso FilaBase <= mMatriz.GetUpperBound(0) AndAlso Fila >= 0 AndAlso Fila <= mMatriz.GetUpperBound(0) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(1)
                    mMatriz(Fila, i) += Valor * mMatriz(FilaBase, i)
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function ProductoColumna(ByVal ColumnaBase As Integer, ByVal Columna As Integer, ByVal Valor As Double) As Boolean
            If ColumnaBase <> Columna AndAlso ColumnaBase >= 0 AndAlso ColumnaBase <= mMatriz.GetUpperBound(1) AndAlso Columna >= 0 AndAlso Columna <= mMatriz.GetUpperBound(1) Then
                For i As Integer = 0 To mMatriz.GetUpperBound(0)
                    mMatriz(i, Columna) += Valor * mMatriz(i, ColumnaBase)
                Next

                Return True
            Else
                Return False
            End If
        End Function

        Public Function Gauss() As Matriz
            Dim Retorno As Matriz = Copia(Me)
            Dim ConPivote As Boolean
            Dim DimensionMenor As Integer = Min(mMatriz.GetUpperBound(0) + 1, mMatriz.GetUpperBound(1) + 1)
            Dim i, j, k As Integer
            i = 0
            j = 0
            k = 0

            While ConPivote AndAlso i < DimensionMenor
                j = i
                k = i
                ConPivote = False

                ' Fase 1: Búsqueda de pivote
                While Not ConPivote AndAlso j < mMatriz.GetUpperBound(0)

                    k = i

                    While Not ConPivote AndAlso k < mMatriz.GetUpperBound(1)

                        If Retorno(j, k) <> 0 Then
                            ConPivote = True
                            Retorno.IntercambiarFilas(j, i)
                            Retorno.IntercambiarColumnas(k, i)
                            Retorno.ProductoFila(i, (1 / Retorno(i, i))) 'Nota: es 1/retorno(i,i) porque primero se mueve el pivote a su posición que es (i,i)
                        End If

                        k += 1
                    End While

                    j += 1
                End While

                If ConPivote Then
                    'Fase 2: Reducción de columna:
                    For l As Integer = i + 1 To DimensionMenor - 1
                        Retorno.ProductoFila(i, l, -Retorno(l, i))
                    Next
                End If

                i += 1
            End While

            Return Retorno
        End Function

        Public Function Rango() As Integer
            Dim Retorno As Integer = 0
            Dim Matriz As Matriz = Copia(Me)
            Dim ConPivote As Boolean
            Dim DimensionMenor As Integer = Min(mMatriz.GetUpperBound(0) + 1, mMatriz.GetUpperBound(1) + 1)
            Dim i, j, k As Integer
            i = 0
            j = 0
            k = 0

            While ConPivote AndAlso i < DimensionMenor
                j = i
                k = i
                ConPivote = False

                ' Fase 1: Búsqueda de pivote
                While Not ConPivote AndAlso j <= mMatriz.GetUpperBound(0)

                    k = i

                    While Not ConPivote AndAlso k <= mMatriz.GetUpperBound(1)

                        If Matriz(j, k) <> 0 Then
                            ConPivote = True
                            Matriz.IntercambiarFilas(j, i)
                            Matriz.IntercambiarColumnas(k, i)
                            Matriz.ProductoFila(i, (1 / Matriz(i, i))) 'Nota: es 1/retorno(i,i) porque primero se mueve el pivote a su posición que es (i,i)
                        End If

                        k += 1
                    End While

                    j += 1
                End While

                If ConPivote Then
                    Retorno += 1
                    'Fase 2: Reducción de columna:
                    For l As Integer = i + 1 To DimensionMenor - 1
                        Matriz.ProductoFila(i, l, -Matriz(l, i))
                    Next
                End If

                i += 1
            End While

            Return Retorno
        End Function

        Public Shared Function Rango(ByVal Matriz As Matriz) As Integer
            Dim Retorno As Integer = 0
            Dim ConPivote As Boolean = True
            Dim DimensionMenor As Integer = Min(Matriz.Filas, Matriz.Columnas)
            Dim i, j, k As Integer
            i = 0
            j = 0
            k = 0

            While ConPivote AndAlso i < DimensionMenor
                j = i
                k = i
                ConPivote = False

                ' Fase 1: Búsqueda de pivote
                While Not ConPivote AndAlso j < Matriz.Filas

                    k = i

                    While Not ConPivote AndAlso k < Matriz.Columnas

                        If Matriz(j, k) <> 0 Then
                            ConPivote = True
                            Matriz.IntercambiarFilas(j, i)
                            Matriz.IntercambiarColumnas(k, i)
                            Matriz.ProductoFila(i, (1 / Matriz(i, i))) 'Nota: es 1/retorno(i,i) porque primero se mueve el pivote a su posición que es (i,i)
                        End If

                        k += 1
                    End While

                    j += 1
                End While

                If ConPivote Then
                    Retorno += 1
                    'Fase 2: Reducción de columna:
                    For l As Integer = i + 1 To DimensionMenor - 1
                        Matriz.ProductoFila(i, l, -Matriz(l, i))
                    Next
                End If

                i += 1
            End While

            Return Retorno
        End Function

        Public Function Copia() As Matriz
            Return Copia(Me)
        End Function

        Public Overrides Function ToString() As String
            Dim Retorno As String = ""

            For i As Integer = 0 To mMatriz.GetUpperBound(0)
                For j As Integer = 0 To mMatriz.GetUpperBound(1)
                    Retorno &= FormatNumber(mMatriz(i, j), 2) & " "
                Next
                Retorno &= vbNewLine
            Next

            Return Retorno
        End Function

        Public Overloads Function ToString(ByVal Multiline As Boolean) As String
            Dim Retorno As String = ""

            If Multiline Then
                For i As Integer = 0 To mMatriz.GetUpperBound(0)
                    For j As Integer = 0 To mMatriz.GetUpperBound(1)
                        Retorno &= FormatNumber(mMatriz(i, j), 2) & " "
                    Next
                    Retorno &= vbNewLine
                Next
            Else
                For i As Integer = 0 To mMatriz.GetUpperBound(0)
                    For j As Integer = 0 To mMatriz.GetUpperBound(1)
                        Retorno &= FormatNumber(mMatriz(i, j), 2) & " "
                    Next
                    Retorno &= "/"
                Next
            End If

            Return Retorno
        End Function

        Public Shared Function Suma(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Dim Retorno As Matriz

            If (M1.Filas = M2.Filas AndAlso M1.Columnas = M2.Columnas) Then
                Retorno = New Matriz(M1.Filas, M1.Columnas)

                For i As Integer = 0 To M1.Filas - 1
                    For j As Integer = 0 To M1.Columnas - 1
                        Retorno.EstablecerValor(i, j, M1.ObtenerValor(i, j) + M2.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionOperacionMatricial("MATRIZ (SUMA): Los sumandos deben tener las mismas dimensiones" & vbNewLine _
                                              & "Primar sumando: " & M1.Filas & "x" & M1.Columnas & vbNewLine _
                                              & "Segundo sumando: " & M2.Filas & "x" & M2.Columnas)
            End If
        End Function

        Public Shared Function Resta(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Dim Retorno As Matriz

            If (M1.Filas = M2.Filas AndAlso M1.Columnas = M2.Columnas) Then
                Retorno = New Matriz(M1.Filas, M1.Columnas)

                For i As Integer = 0 To M1.Filas - 1
                    For j As Integer = 0 To M1.Columnas - 1
                        Retorno.EstablecerValor(i, j, M1.ObtenerValor(i, j) - M2.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionOperacionMatricial("MATRIZ (RESTA): Minuendo y sustraendo deben tener las mismas dimensiones" & vbNewLine _
                                              & "Minuendo: " & M1.Filas & "x" & M1.Columnas & vbNewLine _
                                              & "Sustraendo: " & M2.Filas & "x" & M2.Columnas)
            End If
        End Function

        Public Shared Function Producto(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            If (M1.Columnas = M2.Filas) Then
                Dim ValorElemento As Double
                Dim Retorno As New Matriz(M1.Filas, M2.Columnas)

                For i As Integer = 0 To M1.Filas - 1
                    For j As Integer = 0 To M2.Columnas - 1
                        ValorElemento = 0
                        For k As Integer = 0 To M1.Columnas - 1
                            ValorElemento += M1.ObtenerValor(i, k) * M2.ObtenerValor(k, j)
                        Next
                        Retorno.EstablecerValor(i, j, ValorElemento)
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionOperacionMatricial("MATRIZ (PRODUCTO): El número de columnas del primer factor debe ser igual al número de filas del segundo" & vbNewLine _
                                              & "Primer factor: " & M1.Filas & "x" & M1.Columnas & vbNewLine _
                                              & "Segundo factor: " & M2.Filas & "x" & M2.Columnas)
            End If
        End Function

        Public Shared Function Producto(ByVal Matriz As Matriz, ByVal Factor As Double) As Matriz
            For i As Integer = 0 To Matriz.Filas - 1
                For j As Integer = 0 To Matriz.Columnas - 1
                    Matriz.EstablecerValor(i, j, Matriz.ObtenerValor(i, j) * Factor)
                Next
            Next

            Return Matriz
        End Function

        Public Shared Function Transpuesta(ByVal Matriz As Matriz) As Matriz
            Dim Retorno = New Matriz(Matriz.Columnas, Matriz.Filas)

            For i As Integer = 0 To Matriz.Filas - 1
                For j As Integer = 0 To Matriz.Columnas - 1
                    Retorno.EstablecerValor(j, i, Matriz.ObtenerValor(i, j))
                Next
            Next

            Return Retorno
        End Function

        Public Shared Function SubMatriz(ByVal Matriz As Matriz, ByVal Fila As Integer, ByVal Columna As Integer)
            If Matriz.Filas >= 2 AndAlso Matriz.Columnas >= 2 Then
                Dim Retorno As New Matriz(Matriz.Filas - 1, Matriz.Columnas - 1)

                Dim x, y As Integer

                For i As Integer = 0 To Matriz.Filas - 1
                    Select Case i
                        Case Is < Fila
                            x = i
                        Case Is = Fila
                            Continue For
                        Case Is > Fila
                            x = i - 1
                    End Select

                    For j As Integer = 0 To Matriz.Columnas - 1
                        Select Case j
                            Case Is < Columna
                                y = j
                            Case Is = Columna
                                Continue For
                            Case Is > Columna
                                y = j - 1
                        End Select

                        Retorno.EstablecerValor(x, y, Matriz.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." & vbNewLine _
                                              & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Function

        Public Shared Function SubMatrizPorTamaño(ByVal Matriz As Matriz, ByVal Filas As Integer, ByVal Columnas As Integer) As Matriz
            Dim Retorno As Matriz

            If Filas <= Matriz.Filas AndAlso Columnas <= Matriz.Columnas AndAlso Filas > 0 AndAlso Columnas > 0 Then
                Retorno = New Matriz(Filas, Columnas)

                For i As Integer = 0 To Filas - 1
                    For j As Integer = 0 To Columnas - 1
                        Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): El número de filas y/o columnas es menor que uno o superior a los originales." & vbNewLine _
                                          & "Dimensiones originales: " & Matriz.Filas.ToString & "x" & Matriz.Columnas.ToString & vbNewLine _
                                          & "Dimensiones especificadas: " & Filas.ToString & "x" & Columnas.ToString)
            End If
        End Function

        Public Shared Function SubMatrizPorFila(ByVal Matriz As Matriz, ByVal Fila As Integer) As Matriz
            If Matriz.Filas >= 2 Then
                Dim Retorno As New Matriz(Matriz.Filas - 1, Matriz.Columnas)

                Dim x As Integer

                For i As Integer = 0 To Matriz.Filas - 1
                    Select Case i
                        Case Is < Fila
                            x = i
                        Case Is = Fila
                            Continue For
                        Case Is > Fila
                            x = i - 1
                    End Select

                    For j As Integer = 0 To Matriz.Columnas - 1
                        Retorno.EstablecerValor(x, j, Matriz.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." & vbNewLine _
                                              & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Function

        Public Shared Function SubMatrizPorColumna(ByVal Matriz As Matriz, ByVal Columna As Integer) As Matriz
            If Matriz.Columnas >= 2 Then
                Dim Retorno As New Matriz(Matriz.Filas, Matriz.Columnas - 1)

                Dim y As Integer

                For j As Integer = 0 To Matriz.Columnas - 1
                    Select Case j
                        Case Is < Columna
                            y = j
                        Case Is = Columna
                            Continue For
                        Case Is > Columna
                            y = j - 1
                    End Select

                    For i As Integer = 0 To Matriz.Filas - 1
                        Retorno.EstablecerValor(i, y, Matriz.ObtenerValor(i, j))
                    Next
                Next

                Return Retorno
            Else
                Throw New ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." & vbNewLine _
                                              & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Function

        Public Shared Function SubMatrizCuadrada(ByVal Matriz As Matriz, ByVal Fila As Integer, ByVal Columna As Integer, ByVal Dimensiones As Integer)
            If Not ((Fila + Dimensiones <= Matriz.Filas) AndAlso (Columna + Dimensiones <= Matriz.Columnas)) Then
                Throw New ExcepcionMatriz("MATRIZ (SUBMATRIZ CUADRADA): No se puede obtener una submatriz de las dimensiones especificadas desde el elemento especificado.")
                Exit Function
            End If

            Dim Retorno As New Matriz(Dimensiones, Dimensiones)

            For i As Integer = 0 To Retorno.Filas - 1
                For j As Integer = 0 To Retorno.Columnas - 1
                    Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(i + Fila, j + Columna))
                Next
            Next

            Return Retorno
        End Function

        Public Shared Function CalculoDeterminante(ByVal Matriz As Matriz) As Double
            If Matriz.Filas = Matriz.Columnas Then
                Dim Positivos, Negativos As Double

                Select Case Matriz.Filas
                    Case 1
                        Return Matriz.ObtenerValor(0, 0)
                    Case 2
                        Positivos = Matriz.ObtenerValor(0, 0) * Matriz.ObtenerValor(1, 1)
                        Negativos = Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 0)

                        Return Positivos - Negativos
                    Case 3
                        Positivos = (Matriz.ObtenerValor(0, 0) * Matriz.ObtenerValor(1, 1) * Matriz.ObtenerValor(2, 2)) + _
                                    (Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 2) * Matriz.ObtenerValor(2, 0)) + _
                                    (Matriz.ObtenerValor(1, 0) * Matriz.ObtenerValor(2, 1) * Matriz.ObtenerValor(0, 2))

                        Negativos = (Matriz.ObtenerValor(0, 2) * Matriz.ObtenerValor(1, 1) * Matriz.ObtenerValor(2, 0)) + _
                                    (Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 0) * Matriz.ObtenerValor(2, 2)) + _
                                    (Matriz.ObtenerValor(1, 2) * Matriz.ObtenerValor(2, 1) * Matriz.ObtenerValor(0, 0))

                        Return Positivos - Negativos

                    Case Else
                        Dim Retorno As Double = 0
                        For i As Integer = 0 To Matriz.Filas - 1
                            Retorno += (Adjunto(Matriz, i, 0) * Matriz.ObtenerValor(i, 0))
                        Next

                        Return Retorno
                End Select

            Else
                Throw New ExcepcionMatrizNoCuadrada("MATRIZ (DETERMINANTE): Solo se puede obtener el determinante de matrices cuadradas." & vbNewLine _
                                              & "Dimensiones de la matriz: " & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Function

        Public Shared Function Menor(ByVal Matriz As Matriz, ByVal Fila As Integer, ByVal Columna As Integer) As Double
            Dim SubMatriz As Matriz = Matriz.SubMatriz(Matriz, Fila, Columna)

            If SubMatriz.EsCuadrada Then
                Return CalculoDeterminante(SubMatriz)
            Else
                Throw New ExcepcionMatrizNoCuadrada("MATRIZ (MENOR): La submatriz obtenida no es cuadrada. No se puede calcular el determinante." & vbNewLine _
                                              & "Dimensiones de la submatriz: " & SubMatriz.Filas & "x" & SubMatriz.Columnas)
            End If
        End Function

        Public Shared Function Adjunto(ByVal Matriz As Matriz, ByVal Fila As Integer, ByVal Columna As Integer) As Double
            Dim SubMatriz As Matriz = Matriz.SubMatriz(Matriz, Fila, Columna)

            If SubMatriz.EsCuadrada Then
                Return ((-1) ^ (Fila + Columna + 2)) * CalculoDeterminante(SubMatriz)
            Else
                Throw New ExcepcionMatrizNoCuadrada("MATRIZ (ADJUNTO): La submatriz obtenida no es cuadrada. No se puede calcular el determinante." & vbNewLine _
                                              & "Dimensiones de la submatriz: " & SubMatriz.Filas & "x" & SubMatriz.Columnas)
            End If
        End Function

        Public Shared Function Adjunta(ByVal Matriz As Matriz) As Matriz
            Dim Retorno As New Matriz(Matriz.Filas, Matriz.Columnas)

            For i As Integer = 0 To Matriz.Filas - 1
                For j As Integer = 0 To Matriz.Columnas - 1
                    Try
                        Retorno.EstablecerValor(i, j, Adjunto(Matriz, i, j))
                    Catch ex As ExcepcionAlgebraica
                        Throw New ExcepcionMatrizNoCuadrada("MATRIZ (ADJUNTA): La submatriz obtenida no es cuadrada. No se puede calcular el determinante.")
                        Exit Function
                    End Try
                Next
            Next

            Return Retorno
        End Function

        Public Shared Function CalculoInversa(ByVal Matriz As Matriz) As Matriz
            If Matriz.EsCuadrada Then
                Dim Det As Double = CalculoDeterminante(Matriz)
                If Det <> 0 Then
                    Return Producto(Transpuesta(Adjunta(Matriz)), (1 / Det))
                Else
                    Throw New ExcepcionOperacionMatricial("MATRIZ (CALCULOINVERSA): El determinante de la matriz es cero. No se puede calcular la inversa.")
                End If
            Else
                Throw New ExcepcionMatrizNoCuadrada("MATRIZ (CALCULOINVERSA): La matriz no es cuadrada. No se puede calcular el determinante.")
            End If
        End Function

        Public Shared Function Copia(ByVal Matriz As Matriz) As Matriz
            Dim Retorno As New Matriz(Matriz.Filas, Matriz.Columnas)

            For i As Integer = 0 To Matriz.Filas - 1
                For j As Integer = 0 To Matriz.Columnas - 1
                    Retorno.EstablecerValor(i, j, Matriz.Matriz(i, j))
                Next
            Next

            Return Retorno
        End Function

        Public Shared Operator +(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Return Suma(M1, M2)
        End Operator

        Public Shared Operator -(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Return Resta(M1, M2)
        End Operator

        Public Shared Operator *(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Return Producto(M1, M2)
        End Operator

        Public Shared Operator *(ByVal M1 As Matriz, ByVal Factor As Double) As Matriz
            Return Producto(M1, Factor)
        End Operator

        Public Shared Operator /(ByVal M1 As Matriz, ByVal M2 As Matriz) As Matriz
            Return (M2.Inversa * M1)
        End Operator

        Public Shared Operator /(ByVal M1 As Matriz, ByVal Factor As Double) As Matriz
            If Factor <> 0 Then
                Return Producto(M1, 1 / Factor)
            Else
                Throw New ExcepcionOperacionMatricial("MATRIZ (OPERADOR DE DIVISION): El factor de división es cero.")
            End If
        End Operator

        Public Shared Function MatrizUnitaria(ByVal Dimensiones As Integer)
            If Dimensiones <= 0 Then Dimensiones = 1

            Dim Retorno As New Matriz(Dimensiones, Dimensiones)

            For i As Integer = 0 To Dimensiones - 1
                For j As Integer = 0 To Dimensiones - 1
                    Retorno.EstablecerValor(i, j, IIf(i = j, 1, 0))
                Next
            Next

            Return Retorno
        End Function

        Public Shared Function MatrizPorColumnas(ByVal Matriz As Matriz, ByVal ParamArray Columnas() As Integer)
            Dim Retorno As New Matriz(Matriz.Filas, Columnas.GetUpperBound(0) + 1)

            For i As Integer = 0 To Columnas.GetUpperBound(0)
                For j As Integer = 0 To Matriz.Filas - 1
                    Retorno.EstablecerValor(j, i, Matriz.ObtenerValor(j, Columnas(i)))
                Next
            Next

            Return Retorno
        End Function

        Public Shared Function MatrizPorFilas(ByVal Matriz As Matriz, ByVal ParamArray Filas() As Integer)
            Dim Retorno As New Matriz(Filas.GetUpperBound(0) + 1, Matriz.Columnas)

            For i As Integer = 0 To Filas.GetUpperBound(0)
                For j As Integer = 0 To Matriz.Columnas - 1
                    Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(Filas(i), j))
                Next
            Next

            Return Retorno
        End Function
    End Class
End Namespace
