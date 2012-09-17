Imports System.Math

Namespace Espacio3D
    Public Class Segmento3D
        Inherits ObjetoGeometrico3D

        Private mExtremoInicial As Punto3D
        Private mExtremoFinal As Punto3D
        Private mRecta As Recta3D
        Private mLongitud As Double
        Private mAABB As AABB3D

        Public Property ExtremoInicial() As Punto3D
            Get
                Return mExtremoInicial
            End Get
            Set(ByVal value As Punto3D)
                mExtremoInicial = value
                RecalcularDatos()
            End Set
        End Property

        Public Property ExtremoFinal() As Punto3D
            Get
                Return mExtremoFinal
            End Get
            Set(ByVal value As Punto3D)
                mExtremoFinal = value
                RecalcularDatos()
            End Set
        End Property

        Public ReadOnly Property Recta() As Recta3D
            Get
                Return mRecta
            End Get
        End Property

        Public ReadOnly Property AABB() As AABB3D
            Get
                Return mAABB
            End Get
        End Property

        Public ReadOnly Property Longitud() As Double
            Get
                Return mLongitud
            End Get
        End Property

        Public ReadOnly Property PuntoMedio() As Punto3D
            Get
                Return New Punto3D((mExtremoInicial.X + mExtremoFinal.X) / 2, (mExtremoInicial.Y + mExtremoFinal.Y) / 2, (mExtremoInicial.Z + mExtremoFinal.Z) / 2)
            End Get
        End Property

        Public Sub New(ByVal P1 As Punto3D, ByVal P2 As Punto3D)
            mExtremoInicial = P1
            mExtremoFinal = P2

            mLongitud = Sqrt(((P1.X + P2.X) ^ 2) + ((P1.Y + P2.Y) ^ 2))
            mRecta = New Recta3D(P1, P2)

            mAABB = New AABB3D(P1, New Vector3D(P2.X - P1.X, P2.Y - P1.Y, P2.Z - P1.Z))
        End Sub

        Private Sub RecalcularDatos()
            mRecta = New Recta3D(mExtremoInicial, mExtremoFinal)
            mAABB = New AABB3D(mExtremoInicial, New Vector3D(mExtremoFinal.X - mExtremoInicial.X, mExtremoFinal.Y - mExtremoInicial.Y, mExtremoFinal.Z - mExtremoInicial.Z))
            mLongitud = Sqrt(((mExtremoInicial.X + mExtremoFinal.X) ^ 2) + ((mExtremoInicial.Y + mExtremoFinal.Y) ^ 2))
        End Sub

        Public Function Pertenece(ByVal Punto As Punto3D) As Boolean
            Return Pertenece(Me, Punto)
        End Function

        Public Function Pertenece(ByVal Segmento As Segmento3D, ByVal Punto As Punto3D) As Boolean
            Return mAABB.Pertenece(Punto) AndAlso mRecta.Pertenece(Punto)
        End Function

        Public Overrides Function ToString() As String
            Return "{Segmento. Inicio=" & mExtremoInicial.ToString & ",Fin=" & mExtremoFinal.ToString & "}"
        End Function
    End Class
End Namespace