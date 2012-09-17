
Namespace Espacio2D
    Public Class ArbolOBB
        Private mRaiz As NodoArbolOBB

        Public ReadOnly Property Raiz As NodoArbolOBB
            Get
                Return mRaiz
            End Get
        End Property

        Public ReadOnly Property OBB As OBB2D
            Get
                Return mRaiz.OBB
            End Get
        End Property

        Public Sub New(ByVal ParamArray Puntos() As Punto2D)
            mRaiz = New NodoArbolOBB(Puntos)
        End Sub

        Public Function Pertenece(ByRef Punto As Punto2D) As Boolean
            Return mRaiz.Pertenece(Punto)
        End Function

        Public Sub AplicarTransformacion(ByRef Transformacion As Transformacion2D)
            mRaiz.AplicarTransformacion(Transformacion)
        End Sub
    End Class
End Namespace

