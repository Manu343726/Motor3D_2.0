Imports Motor3D.Espacio3D
Imports Motor3D.Espacio2D
Imports Motor3D.Primitivas3D
Imports System.Threading

Namespace Escena
    Public Class Motor3D
        Inherits ObjetoEscena

        Private mPoliedros() As Poliedro
        Private mFocos() As Foco3D
        Private mCamaras() As Camara3D
        Private mCamaraSeleccionada As Camara3D

        Private mShading As Boolean

        Private Buffer As ZBuffer

        Private mModificandoEscena As Boolean

        Public Shadows Event Modificado(ByRef Sender As Motor3D)
        Public Event Actualizado(ByRef ZBuffer As ZBuffer)

        Public Property ShadingActivado As Boolean
            Get
                Return mShading
            End Get
            Set(value As Boolean)
                If mShading <> value Then
                    mShading = value
                End If
            End Set
        End Property

        Public ReadOnly Property ModificandoEscena As Boolean
            Get
                Return mModificandoEscena
            End Get
        End Property

        Public Property CamaraSeleccionada As Camara3D
            Get
                Return mCamaraSeleccionada
            End Get
            Set(value As Camara3D)
                If mCamaras.Contains(value) Then
                    If Not mCamaraSeleccionada Is Nothing Then RemoveHandler mCamaraSeleccionada.Modificado, AddressOf CamaraModificada
                    mCamaraSeleccionada = value
                    AddHandler mCamaraSeleccionada.Modificado, AddressOf CamaraModificada
                    CamaraModificada(mCamaraSeleccionada)
                Else
                    AñadirCamara(value)
                End If
            End Set
        End Property

        Public ReadOnly Property NumeroCamaras As Integer
            Get
                If Not mCamaras Is Nothing Then
                    Return mCamaras.GetUpperBound(0) + 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property NumeroPoliedros As Integer
            Get
                If Not mPoliedros Is Nothing Then
                    Return mPoliedros.GetUpperBound(0) + 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property NumeroFocos As Integer
            Get
                If Not mFocos Is Nothing Then
                    Return mFocos.GetUpperBound(0) + 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property SinCamaras As Boolean
            Get
                Return mCamaras Is Nothing
            End Get
        End Property

        Public ReadOnly Property SinPoliedros As Boolean
            Get
                Return mPoliedros Is Nothing
            End Get
        End Property

        Public ReadOnly Property ZBuffer As ZBuffer
            Get
                Return Buffer
            End Get
        End Property

        Public ReadOnly Property Poliedros As Poliedro()
            Get
                Return mPoliedros
            End Get
        End Property

        Public ReadOnly Property Focos As Foco3D()
            Get
                Return mFocos
            End Get
        End Property

        Public ReadOnly Property Camaras As Camara3D()
            Get
                Return mCamaras
            End Get
        End Property

        Private Sub CamaraModificada(ByRef Sender As Camara3D)
            If Not mPoliedros Is Nothing AndAlso Sender = mCamaraSeleccionada Then
                For i As Integer = 0 To mPoliedros.GetUpperBound(0)
                    mPoliedros(i).RecalcularRepresentaciones(Sender)
                    mPoliedros(i).Shaded = False
                Next

                Buffer.Actualizar(mPoliedros, Sender)
                Shading()

                RaiseEvent Modificado(Me)
            End If
        End Sub

        Private Sub PoliedroModificado(ByRef Sender As Poliedro)
            If Not mModificandoEscena Then
                Sender.RecalcularRepresentaciones(mCamaraSeleccionada)
                'Sender.RecalcularCoordenadasSRC(mCamaraSeleccionada)
                Sender.Shaded = False

                Buffer.Actualizar(mPoliedros, mCamaraSeleccionada)
                Shading(Array.IndexOf(mPoliedros, Sender))

                RaiseEvent Modificado(Me)
            End If
        End Sub

        Private Sub FocoModificado(ByRef Sender As Foco3D)
            Shading()
        End Sub

        Public Sub Shading()
            If mShading Then
                If Not Buffer.Vacio Then
                    For i As Integer = 0 To Buffer.Objetos.GetUpperBound(0)
                        If Not mPoliedros(Buffer.Objetos(i).Indices(1)).Shaded Then
                            mPoliedros(Buffer.Objetos(i).Indices(1)).Shading(mFocos, mCamaraSeleccionada)
                        End If

                        Buffer.Shading(Buffer.Objetos(i).Indices(0), mPoliedros(Buffer.Objetos(i).Indices(1)).Caras(Buffer.Objetos(i).Indices(2)).ColorShading)
                    Next
                End If
            End If

            RaiseEvent Actualizado(Buffer)
        End Sub

        Private Sub ReestablecerColoresBuffer()
            If Not Buffer.Vacio Then
                For i As Integer = 0 To Buffer.Objetos.GetUpperBound(0)
                    Buffer.Shading(Buffer.Objetos(i).Indices(0), mPoliedros(Buffer.Objetos(i).Indices(1)).Caras(Buffer.Objetos(i).Indices(2)).Color)
                Next
            End If
        End Sub

        Private Sub Shading(ByVal IndicePoliedro As Integer)
            If mShading Then
                If IndicePoliedro >= 0 AndAlso IndicePoliedro <= mPoliedros.GetUpperBound(0) Then
                    If Not Buffer.Vacio Then
                        For i As Integer = 0 To Buffer.Objetos.GetUpperBound(0)
                            If Buffer.Objetos(i).Indices(1) = IndicePoliedro Then
                                If Not mPoliedros(Buffer.Objetos(i).Indices(1)).Shaded Then
                                    mPoliedros(Buffer.Objetos(i).Indices(1)).Shading(mFocos, mCamaraSeleccionada)
                                End If

                                Buffer.Shading(Buffer.Objetos(i).Indices(0), mPoliedros(IndicePoliedro).Caras(Buffer.Objetos(i).Indices(2)).ColorShading)
                            End If
                        Next
                    End If
                End If
            End If

            RaiseEvent Actualizado(Buffer)
        End Sub

        Public Sub AñadirPoliedro(ByRef Poliedro As Poliedro)
            If Not mPoliedros Is Nothing Then
                ReDim Preserve mPoliedros(mPoliedros.GetUpperBound(0) + 1)
            Else
                ReDim mPoliedros(0)
            End If

            mPoliedros(mPoliedros.GetUpperBound(0)) = Poliedro

            AddHandler Poliedro.Modificado, AddressOf PoliedroModificado

            PoliedroModificado(Poliedro)
        End Sub

        Public Sub QuitarPoliedro(ByRef Poliedro As Poliedro)
            If Not mPoliedros Is Nothing AndAlso mPoliedros.Contains(Poliedro) Then
                If mPoliedros.GetUpperBound(0) > 0 Then
                    Dim Copia(mPoliedros.GetUpperBound(0)) As Poliedro
                    mPoliedros.CopyTo(Copia, 0)

                    ReDim mPoliedros(mPoliedros.GetUpperBound(0) - 1)

                    For i As Integer = 0 To Copia.GetUpperBound(0)
                        If Copia(i) <> Poliedro Then
                            If i <= mPoliedros.GetUpperBound(0) Then
                                mPoliedros(i) = Copia(i)
                            Else
                                mPoliedros(i - 1) = Copia(i)
                            End If
                        End If
                    Next

                    RemoveHandler Poliedro.Modificado, AddressOf PoliedroModificado
                Else
                    mPoliedros = Nothing
                End If

                Buffer.Actualizar(mPoliedros, mCamaraSeleccionada)
                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Sub AñadirFoco(ByRef Foco As Foco3D)
            If Not mFocos Is Nothing Then
                ReDim Preserve mFocos(mFocos.GetUpperBound(0) + 1)
            Else
                ReDim mFocos(0)
            End If

            mFocos(mFocos.GetUpperBound(0)) = Foco

            AddHandler Foco.Modificado, AddressOf FocoModificado

            FocoModificado(Foco)
        End Sub

        Public Sub QuitarFoco(ByRef Foco As Foco3D)
            If Not mFocos Is Nothing AndAlso mFocos.Contains(Foco) Then
                If mFocos.GetUpperBound(0) > 0 Then
                    Dim Copia(mFocos.GetUpperBound(0)) As Foco3D
                    mFocos.CopyTo(Copia, 0)

                    ReDim mFocos(mFocos.GetUpperBound(0) - 1)

                    For i As Integer = 0 To Copia.GetUpperBound(0)
                        If Copia(i) <> Foco Then
                            If i <= mFocos.GetUpperBound(0) Then
                                mFocos(i) = Copia(i)
                            Else
                                mFocos(i - 1) = Copia(i)
                            End If
                        End If
                    Next

                    RemoveHandler Foco.Modificado, AddressOf FocoModificado
                Else
                    mFocos = Nothing
                End If
                RaiseEvent Modificado(Me)
            End If
        End Sub

        Public Sub AñadirCamara(ByRef Camara As Camara3D)
            If Not mCamaras Is Nothing Then
                ReDim Preserve mCamaras(mCamaras.GetUpperBound(0) + 1)
            Else
                ReDim mCamaras(0)
            End If

            mCamaras(mCamaras.GetUpperBound(0)) = Camara
            If Not mCamaraSeleccionada Is Nothing Then RemoveHandler mCamaraSeleccionada.Modificado, AddressOf CamaraModificada
            mCamaraSeleccionada = Camara
            AddHandler mCamaraSeleccionada.Modificado, AddressOf CamaraModificada
            CamaraModificada(mCamaraSeleccionada)
        End Sub

        Public Sub QuitarCamara(ByRef Camara As Camara3D)
            If Not mCamaras Is Nothing AndAlso mCamaras.Contains(Camara) Then
                If mCamaras.GetUpperBound(0) > 0 Then
                    Dim Copia(mCamaras.GetUpperBound(0)) As Camara3D
                    mCamaras.CopyTo(Copia, 0)

                    ReDim mCamaras(mCamaras.GetUpperBound(0) - 1)

                    For i As Integer = 0 To Copia.GetUpperBound(0)
                        If Copia(i) <> Camara Then
                            If i <= mCamaras.GetUpperBound(0) Then
                                mCamaras(i) = Copia(i)
                            Else
                                mCamaras(i - 1) = Copia(i)
                            End If
                        Else
                            If mCamaraSeleccionada = Camara Then
                                mCamaraSeleccionada = Copia(i + 1)
                            End If
                        End If
                    Next

                    RemoveHandler Camara.Modificado, AddressOf CamaraModificada
                Else
                    mCamaras = Nothing
                    If Camara = mCamaraSeleccionada Then mCamaraSeleccionada = Nothing
                End If
            End If
        End Sub

        Public Sub ObtenerReferenciaPoliedro(ByRef Poliedro As Poliedro, ByVal Indice As Integer)
            If Indice >= 0 AndAlso Indice <= mPoliedros.GetUpperBound(0) Then
                Poliedro = mPoliedros(Indice)
            End If
        End Sub

        Public Sub ObtenerReferenciaFoco(ByRef Foco As Foco3D, ByVal Indice As Integer)
            If Indice >= 0 AndAlso Indice <= mFocos.GetUpperBound(0) Then
                Foco = mFocos(Indice)
            End If
        End Sub

        Public Sub ObtenerReferenciaCamara(ByRef Camara As Camara3D, ByVal Indice As Integer)
            If Indice >= 0 AndAlso Indice <= mCamaras.GetUpperBound(0) Then
                Camara = mCamaras(Indice)
            End If
        End Sub

        Public Sub IniciarEscena()
            mModificandoEscena = True
        End Sub

        Public Sub FinalizarEscena()
            If Not mPoliedros Is Nothing Then
                For i As Integer = 0 To mPoliedros.GetUpperBound(0)
                    mPoliedros(i).RecalcularRepresentaciones(mCamaraSeleccionada)
                Next
            End If

            Buffer.Actualizar(mPoliedros, mCamaraSeleccionada)
            Shading()

            mModificandoEscena = False

            RaiseEvent Modificado(Me)
        End Sub

        Public Sub New()
            MyBase.New()
            Buffer = New ZBuffer
        End Sub
    End Class
End Namespace

