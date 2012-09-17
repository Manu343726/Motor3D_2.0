Imports Motor3D.Algebra
Imports Motor3D.Espacio3D
Imports Motor3D.Espacio2D
Imports System.Math

Namespace Escena
    ' La matriz proyeccion está formada por columnas que corresponden a la proyección de cada base canónica
    ' Las bases canónicas para el espacio 3D corresponden a:
    ' - Bx=(1,0,0)
    ' - By=(0,1,0)
    ' - Bz=(0,0,1)

    'La proyeccion isometrica, por ejemplo:
    'Proyeccion(Bx)=(cos(30),-sen(30))
    'Proyeccion(By)=(-cos(30),-sen(30))
    'Proyeccion(Bz)=(0,1)
    'Por tanto la matriz proyeccion será:
    ' cos(30) -cos(30) 0
    ' -sen(30) -sen(30) 1
    'Y en coordenadas homogéneas:
    ' cos(30) -cos(30) 0 0
    ' -sen(30) -sen(30) 1 0
    '   0         0     0 1

    Public Class Proyeccion
        Inherits ObjetoEscena

        Private mMatriz As Matriz

        Public ReadOnly Property Matriz() As Matriz
            Get
                Return mMatriz
            End Get
        End Property

        Public Sub New()
            mMatriz = New Matriz(3, 4)

            mMatriz.EstablecerValoresPorFila(0, 1, 0, 0, 0)
            mMatriz.EstablecerValoresPorFila(1, 0, 1, 0, 0)
            mMatriz.EstablecerValoresPorFila(2, 0, 0, 0, 1)
        End Sub

        Public Sub New(ByVal Matriz As Matriz)
            If Matriz.Filas = 3 AndAlso Matriz.Columnas = 4 Then
                mMatriz = Matriz
            Else
                Throw New ExcepcionEscena("PROYECCION (NEW): Una proyección solo se puede definir mediante una matriz de dimensiones 3x4." & vbNewLine _
                                              & "Dimensiones=" & Matriz.Filas & "x" & Matriz.Columnas)
            End If
        End Sub

        Public Shared Function ProyeccionParalela() As Proyeccion
            Dim Matriz As New Matriz(3, 4)

            Matriz.EstablecerValoresPorFila(0, 1, 0, 0, 0)
            Matriz.EstablecerValoresPorFila(1, 0, 1, 0, 0)
            Matriz.EstablecerValoresPorFila(2, 0, 0, 0, 1)

            Return New Proyeccion(Matriz)
        End Function

        Public Shared Function ProyeccionIsometrica() As Proyeccion
            Dim Matriz As New Matriz(3, 4)

            Matriz.EstablecerValoresPorFila(0, Cos(PI / 6), -Cos(PI / 6), 0, 0)
            Matriz.EstablecerValoresPorFila(1, -Sin(PI / 6), -Sin(PI / 6), 1, 0)
            Matriz.EstablecerValoresPorFila(2, 0, 0, 0, 1)

            Return New Proyeccion(Matriz)
        End Function

        Public Shared Function Proyectar(ByVal Proyeccion As Proyeccion, ByVal Punto As Punto3D) As Punto2D
            Return New Punto2D(Proyeccion.Matriz * Punto.Matriz)
        End Function
    End Class
End Namespace

