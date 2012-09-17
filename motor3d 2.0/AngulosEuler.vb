Imports System.Math

Namespace Algebra
    Public Structure AngulosEuler
        Private mCabeceo As Single
        Private mAlabeo As Single
        Private mGuiñada As Single

        Public Property Cabeceo As Single
            Get
                Return mCabeceo
            End Get
            Set(value As Single)
                mCabeceo = value
            End Set
        End Property

        Public Property Alabeo As Single
            Get
                Return mAlabeo
            End Get
            Set(value As Single)
                mAlabeo = value
            End Set
        End Property

        Public Property Guiñada As Single
            Get
                Return mGuiñada
            End Get
            Set(value As Single)
                mGuiñada = value
            End Set
        End Property

        Public ReadOnly Property Cuaternion As Cuaternion
            Get
                Return ObtenerCuaternion(Me)
            End Get
        End Property

        Public Sub New(ByVal Cabeceo As Single, ByVal Alabeo As Single, ByVal Guiñada As Single)
            mCabeceo = Cabeceo
            mAlabeo = Alabeo
            mGuiñada = Guiñada
        End Sub

        Public Shared Function ObtenerCuaternion(ByVal Angulos As AngulosEuler) As Cuaternion
            Return New Cuaternion(Angulos.Cabeceo, Angulos.Alabeo, Angulos.Guiñada)
        End Function

        Public Overrides Function ToString() As String
            Return "{AngulosEuler: " & mCabeceo.ToString & "," & mAlabeo.ToString & "," & mGuiñada.ToString & "}"
        End Function
    End Structure
End Namespace

