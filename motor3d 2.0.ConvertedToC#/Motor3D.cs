using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using Motor3D.Espacio2D;
using Motor3D.Primitivas3D;
using System.Threading;

namespace Motor3D.Escena
{
	public class Motor3D : ObjetoEscena
	{

		private Poliedro[] mPoliedros;
		private Foco3D[] mFocos;
		private Camara3D[] mCamaras;

		private Camara3D mCamaraSeleccionada;

		private bool mShading;

		private ZBuffer Buffer;

		private bool mModificandoEscena;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Motor3D Sender);
		public event ActualizadoEventHandler Actualizado;
		public delegate void ActualizadoEventHandler(ref ZBuffer ZBuffer);

		public bool ShadingActivado {
			get { return mShading; }
			set {
				if (mShading != value) {
					mShading = value;
				}
			}
		}

		public bool ModificandoEscena {
			get { return mModificandoEscena; }
		}

		public Camara3D CamaraSeleccionada {
			get { return mCamaraSeleccionada; }
			set {
				if (mCamaras.Contains(value)) {
					if ((mCamaraSeleccionada != null))
						mCamaraSeleccionada.Modificado -= CamaraModificada;
					mCamaraSeleccionada = value;
					mCamaraSeleccionada.Modificado += CamaraModificada;
					CamaraModificada(ref mCamaraSeleccionada);
				} else {
					AñadirCamara(ref value);
				}
			}
		}

		public int NumeroCamaras {
			get {
				if ((mCamaras != null)) {
					return mCamaras.GetUpperBound(0) + 1;
				} else {
					return 0;
				}
			}
		}

		public int NumeroPoliedros {
			get {
				if ((mPoliedros != null)) {
					return mPoliedros.GetUpperBound(0) + 1;
				} else {
					return 0;
				}
			}
		}

		public int NumeroFocos {
			get {
				if ((mFocos != null)) {
					return mFocos.GetUpperBound(0) + 1;
				} else {
					return 0;
				}
			}
		}

		public bool SinCamaras {
			get { return mCamaras == null; }
		}

		public bool SinPoliedros {
			get { return mPoliedros == null; }
		}

		public ZBuffer ZBuffer {
			get { return Buffer; }
		}

		public Poliedro[] Poliedros {
			get { return mPoliedros; }
		}

		public Foco3D[] Focos {
			get { return mFocos; }
		}

		public Camara3D[] Camaras {
			get { return mCamaras; }
		}

		private void CamaraModificada(ref Camara3D Sender)
		{
			if ((mPoliedros != null) && Sender == mCamaraSeleccionada) {
				for (int i = 0; i <= mPoliedros.GetUpperBound(0); i++) {
					mPoliedros[i].RecalcularRepresentaciones(Sender);
					mPoliedros[i].Shaded = false;
				}

				Buffer.Actualizar(ref mPoliedros, ref Sender);
				Shading();

				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		private void PoliedroModificado(ref Poliedro Sender)
		{
			if (!mModificandoEscena) {
				Sender.RecalcularRepresentaciones(mCamaraSeleccionada);
				//Sender.RecalcularCoordenadasSRC(mCamaraSeleccionada)
				Sender.Shaded = false;

				Buffer.Actualizar(ref mPoliedros, ref mCamaraSeleccionada);
				Shading(Array.IndexOf(mPoliedros, Sender));

				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		private void FocoModificado(ref Foco3D Sender)
		{
			Shading();
		}

		public void Shading()
		{
			if (mShading) {
				if (!Buffer.Vacio) {
					for (int i = 0; i <= Buffer.Objetos.GetUpperBound(0); i++) {
						if (!mPoliedros[Buffer.Objetos[i].Indices[1]].Shaded) {
							mPoliedros[Buffer.Objetos[i].Indices[1]].Shading(mFocos, mCamaraSeleccionada);
						}

						Buffer.Shading(Buffer.Objetos[i].Indices[0], mPoliedros[Buffer.Objetos[i].Indices[1]].Caras[Buffer.Objetos[i].Indices[2]].ColorShading);
					}
				}
			}

			if (Actualizado != null) {
				Actualizado(Buffer);
			}
		}

		private void ReestablecerColoresBuffer()
		{
			if (!Buffer.Vacio) {
				for (int i = 0; i <= Buffer.Objetos.GetUpperBound(0); i++) {
					Buffer.Shading(Buffer.Objetos[i].Indices[0], mPoliedros[Buffer.Objetos[i].Indices[1]].Caras[Buffer.Objetos[i].Indices[2]].Color);
				}
			}
		}

		private void Shading(int IndicePoliedro)
		{
			if (mShading) {
				if (IndicePoliedro >= 0 && IndicePoliedro <= mPoliedros.GetUpperBound(0)) {
					if (!Buffer.Vacio) {
						for (int i = 0; i <= Buffer.Objetos.GetUpperBound(0); i++) {
							if (Buffer.Objetos[i].Indices[1] == IndicePoliedro) {
								if (!mPoliedros[Buffer.Objetos[i].Indices[1]].Shaded) {
									mPoliedros[Buffer.Objetos[i].Indices[1]].Shading(mFocos, mCamaraSeleccionada);
								}

								Buffer.Shading(Buffer.Objetos[i].Indices[0], mPoliedros[IndicePoliedro].Caras[Buffer.Objetos[i].Indices[2]].ColorShading);
							}
						}
					}
				}
			}

			if (Actualizado != null) {
				Actualizado(Buffer);
			}
		}

		public void AñadirPoliedro(ref Poliedro Poliedro)
		{
			if ((mPoliedros != null)) {
				Array.Resize(ref mPoliedros, mPoliedros.GetUpperBound(0) + 2);
			} else {
				mPoliedros = new Poliedro[1];
			}

			mPoliedros[mPoliedros.GetUpperBound(0)] = Poliedro;

			Poliedro.Modificado += PoliedroModificado;

			PoliedroModificado(ref Poliedro);
		}

		public void QuitarPoliedro(ref Poliedro Poliedro)
		{
			if ((mPoliedros != null) && mPoliedros.Contains(Poliedro)) {
				if (mPoliedros.GetUpperBound(0) > 0) {
					Poliedro[] Copia = new Poliedro[mPoliedros.GetUpperBound(0) + 1];
					mPoliedros.CopyTo(Copia, 0);

					mPoliedros = new Poliedro[mPoliedros.GetUpperBound(0)];

					for (int i = 0; i <= Copia.GetUpperBound(0); i++) {
						if (Copia[i] != Poliedro) {
							if (i <= mPoliedros.GetUpperBound(0)) {
								mPoliedros[i] = Copia[i];
							} else {
								mPoliedros[i - 1] = Copia[i];
							}
						}
					}

					Poliedro.Modificado -= PoliedroModificado;
				} else {
					mPoliedros = null;
				}

				Buffer.Actualizar(ref mPoliedros, ref mCamaraSeleccionada);
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public void AñadirFoco(ref Foco3D Foco)
		{
			if ((mFocos != null)) {
				Array.Resize(ref mFocos, mFocos.GetUpperBound(0) + 2);
			} else {
				mFocos = new Foco3D[1];
			}

			mFocos[mFocos.GetUpperBound(0)] = Foco;

			Foco.Modificado += FocoModificado;

			FocoModificado(ref Foco);
		}

		public void QuitarFoco(ref Foco3D Foco)
		{
			if ((mFocos != null) && mFocos.Contains(Foco)) {
				if (mFocos.GetUpperBound(0) > 0) {
					Foco3D[] Copia = new Foco3D[mFocos.GetUpperBound(0) + 1];
					mFocos.CopyTo(Copia, 0);

					mFocos = new Foco3D[mFocos.GetUpperBound(0)];

					for (int i = 0; i <= Copia.GetUpperBound(0); i++) {
						if (Copia[i] != Foco) {
							if (i <= mFocos.GetUpperBound(0)) {
								mFocos[i] = Copia[i];
							} else {
								mFocos[i - 1] = Copia[i];
							}
						}
					}

					Foco.Modificado -= FocoModificado;
				} else {
					mFocos = null;
				}
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public void AñadirCamara(ref Camara3D Camara)
		{
			if ((mCamaras != null)) {
				Array.Resize(ref mCamaras, mCamaras.GetUpperBound(0) + 2);
			} else {
				mCamaras = new Camara3D[1];
			}

			mCamaras[mCamaras.GetUpperBound(0)] = Camara;
			if ((mCamaraSeleccionada != null))
				mCamaraSeleccionada.Modificado -= CamaraModificada;
			mCamaraSeleccionada = Camara;
			mCamaraSeleccionada.Modificado += CamaraModificada;
			CamaraModificada(ref mCamaraSeleccionada);
		}

		public void QuitarCamara(ref Camara3D Camara)
		{
			if ((mCamaras != null) && mCamaras.Contains(Camara)) {
				if (mCamaras.GetUpperBound(0) > 0) {
					Camara3D[] Copia = new Camara3D[mCamaras.GetUpperBound(0) + 1];
					mCamaras.CopyTo(Copia, 0);

					mCamaras = new Camara3D[mCamaras.GetUpperBound(0)];

					for (int i = 0; i <= Copia.GetUpperBound(0); i++) {
						if (Copia[i] != Camara) {
							if (i <= mCamaras.GetUpperBound(0)) {
								mCamaras[i] = Copia[i];
							} else {
								mCamaras[i - 1] = Copia[i];
							}
						} else {
							if (mCamaraSeleccionada == Camara) {
								mCamaraSeleccionada = Copia[i + 1];
							}
						}
					}

					Camara.Modificado -= CamaraModificada;
				} else {
					mCamaras = null;
					if (Camara == mCamaraSeleccionada)
						mCamaraSeleccionada = null;
				}
			}
		}

		public void ObtenerReferenciaPoliedro(ref Poliedro Poliedro, int Indice)
		{
			if (Indice >= 0 && Indice <= mPoliedros.GetUpperBound(0)) {
				Poliedro = mPoliedros[Indice];
			}
		}

		public void ObtenerReferenciaFoco(ref Foco3D Foco, int Indice)
		{
			if (Indice >= 0 && Indice <= mFocos.GetUpperBound(0)) {
				Foco = mFocos[Indice];
			}
		}

		public void ObtenerReferenciaCamara(ref Camara3D Camara, int Indice)
		{
			if (Indice >= 0 && Indice <= mCamaras.GetUpperBound(0)) {
				Camara = mCamaras[Indice];
			}
		}

		public void IniciarEscena()
		{
			mModificandoEscena = true;
		}

		public void FinalizarEscena()
		{
			if ((mPoliedros != null)) {
				for (int i = 0; i <= mPoliedros.GetUpperBound(0); i++) {
					mPoliedros[i].RecalcularRepresentaciones(mCamaraSeleccionada);
				}
			}

			Buffer.Actualizar(ref mPoliedros, ref mCamaraSeleccionada);
			Shading();

			mModificandoEscena = false;

			if (Modificado != null) {
				Modificado(this);
			}
		}

		public Motor3D() : base()
		{
			Buffer = new ZBuffer();
		}
	}
}

