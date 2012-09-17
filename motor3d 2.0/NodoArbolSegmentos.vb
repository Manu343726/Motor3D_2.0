Namespace Espacio2D
    Public Class NodoArbolSegmentos
        Public Inicio As VerticeRecorte
        Public Fin As VerticeRecorte
        Private Segmento As Segmento2D

        Public HijoIzquierda As NodoArbolSegmentos
        Public HijoDerecha As NodoArbolSegmentos

        Public ReadOnly Property EsHoja As Boolean
            Get
                Return HijoIzquierda Is Nothing
            End Get
        End Property

        Public Sub New(ByRef ValSegmento As Segmento2D, ByRef ValInicio As VerticeRecorte, ByRef ValFin As VerticeRecorte)
            Inicio = ValInicio
            Fin = ValFin
            Segmento = ValSegmento
            HijoIzquierda = Nothing
            HijoDerecha = Nothing
        End Sub

        Public Sub InsertarInterseccion(ByRef Interseccion As VerticeRecorte, ByVal EnRecorte As Boolean)
            If HijoIzquierda Is Nothing Then
                If EnRecorte Then
                    Inicio.SiguienteEnRecorte = Interseccion
                    Interseccion.SiguienteEnRecorte = Fin
                Else
                    Inicio.SiguienteEnRecortado = Interseccion
                    Interseccion.SiguienteEnRecortado = Fin
                End If

                HijoIzquierda = New NodoArbolSegmentos(Segmento, Inicio, Interseccion)
                HijoDerecha = New NodoArbolSegmentos(Segmento, Interseccion, Fin)
            Else
                If Segmento.Recta.VectorDirector * New Vector2D(HijoIzquierda.Fin.Vertice, Interseccion.Vertice) < 0 Then
                    HijoIzquierda.InsertarInterseccion(Interseccion, EnRecorte)
                Else
                    HijoDerecha.InsertarInterseccion(Interseccion, EnRecorte)
                End If
            End If
        End Sub
    End Class
End Namespace

