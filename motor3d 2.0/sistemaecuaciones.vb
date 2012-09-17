Namespace Algebra
    Public Class SistemaEcuaciones
        Inherits ObjetoAlgebraico

        Private mEcuaciones() As Ecuacion
        Private mNumeroIncognitas As Integer

        Private mSolucion As SolucionSistema

        Private mMatrizPrincipal As Matriz
        Private mMatrizAmpliada As Matriz

        Private mRangoMatrizAmpliada As Integer
        Private mRangoMatrizPrincipal As Integer

        Private mEsHomogeneo As Boolean

        Public ReadOnly Property Ecuaciones() As Ecuacion()
            Get
                Return mEcuaciones
            End Get
        End Property

        Public ReadOnly Property NumeroEcuaciones() As Integer
            Get
                Return mEcuaciones.GetUpperBound(0) + 1
            End Get
        End Property

        Public ReadOnly Property NumeroIncognitas() As Integer
            Get
                Return mNumeroIncognitas
            End Get
        End Property

        Public ReadOnly Property Solucion() As SolucionSistema
            Get
                Return mSolucion
            End Get
        End Property

        Public ReadOnly Property RangoMatrizAmpliada() As Integer
            Get
                Return mRangoMatrizAmpliada
            End Get
        End Property

        Public ReadOnly Property RangoMatrizPrincipal() As Integer
            Get
                Return mRangoMatrizPrincipal
            End Get
        End Property

        Public ReadOnly Property MatrizPrincipal() As Matriz
            Get
                Return mMatrizPrincipal
            End Get
        End Property

        Public ReadOnly Property MatrizAmpliada() As Matriz
            Get
                Return mMatrizAmpliada
            End Get
        End Property

        Public ReadOnly Property EsHomogeneo As Boolean
            Get
                Return mEsHomogeneo
            End Get
        End Property

        Public Sub New(ByVal ParamArray ValEcuaciones() As Ecuacion)
            Dim Sols() As Double
            Dim DetA As Double
            Dim Columnas() As Integer
            Dim Mat, MatOr As Matriz

            mEcuaciones = ValEcuaciones
            mNumeroIncognitas = 0
            mEsHomogeneo = TestHomogeneidad()

            For i As Integer = 0 To mEcuaciones.GetUpperBound(0)
                If mNumeroIncognitas < mEcuaciones(i).NumeroVariables Then mNumeroIncognitas = mEcuaciones(i).NumeroVariables
            Next

            If mEsHomogeneo Then
                mMatrizPrincipal = Matriz.MatrizUnitaria(mNumeroIncognitas + 1)
                mMatrizAmpliada = Matriz.MatrizUnitaria(mNumeroIncognitas + 1)
                mRangoMatrizPrincipal = 0
                mRangoMatrizAmpliada = 0

                ReDim Sols(mNumeroIncognitas - 1)

                mSolucion = New SolucionSistema(Sols)
            Else
                mMatrizAmpliada = New Matriz(mEcuaciones.GetUpperBound(0) + 1, mNumeroIncognitas + 1)

                For i As Integer = 0 To mEcuaciones.GetUpperBound(0)
                    mMatrizAmpliada.EstablecerValoresPorFila(i, mEcuaciones(i).Variables)
                Next

                mMatrizPrincipal = Matriz.SubMatrizPorColumna(mMatrizAmpliada, mNumeroIncognitas)

                mRangoMatrizAmpliada = Matriz.Rango(mMatrizAmpliada)
                mRangoMatrizPrincipal = Matriz.Rango(mMatrizPrincipal)

                If (mRangoMatrizPrincipal = mRangoMatrizAmpliada) Then
                    If mRangoMatrizAmpliada = mNumeroIncognitas Then
                        'RESOLUCION POR REGLA DE CRAMER:

                        ReDim Sols(mNumeroIncognitas - 1)
                        ReDim Columnas(mMatrizPrincipal.Columnas - 1)

                        If mMatrizPrincipal.EsCuadrada Then
                            DetA = Matriz.CalculoDeterminante(mMatrizPrincipal)
                            MatOr = mMatrizPrincipal.Copia
                        Else
                            MatOr = Matriz.SubMatrizPorTamaño(mMatrizPrincipal, mNumeroIncognitas, mNumeroIncognitas)
                            DetA = Matriz.CalculoDeterminante(MatOr)
                        End If

                        For i As Integer = 0 To mNumeroIncognitas - 1
                            Mat = MatOr.Copia

                            Mat.EstablecerValoresPorColumna(i, mMatrizAmpliada.ObtenerColumna(mNumeroIncognitas))

                            Sols(i) = Matriz.CalculoDeterminante(Mat) / DetA
                        Next

                        mSolucion = New SolucionSistema(Sols)
                    Else
                        mSolucion = New SolucionSistema(TipoSolucionSistema.SistemaCompatibleIndeterminado)
                    End If
                Else
                    mSolucion = New SolucionSistema(TipoSolucionSistema.SistemaIncompatible)
                End If
            End If
        End Sub

        Private Function TestHomogeneidad() As Boolean
            For Each Ecuacion As Ecuacion In mEcuaciones
                If Ecuacion.TerminoIndependiente <> 0 Then Return False
            Next

            Return True
        End Function

        Public Overrides Function ToString() As String
            Dim Retorno As String = ""

            For i As Integer = 0 To mEcuaciones.GetUpperBound(0)
                Retorno &= mEcuaciones(i).ToString & vbNewLine
            Next

            Return Retorno
        End Function
    End Class
End Namespace