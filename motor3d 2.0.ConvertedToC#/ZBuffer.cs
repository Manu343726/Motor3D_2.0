using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Primitivas3D;
using Motor3D.Utilidades;
using Motor3D.Espacio2D;
using System.Drawing;

namespace Motor3D.Escena
{
	public class ZBuffer : ObjetoEscena
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ZBuffer Sender);

		private ElementoZBuffer[] mObjetos;
		private Poligono2D[] mRepresentaciones;
		private int mNumeroIndices;

		private double mTiempo;
		public ElementoZBuffer[] Objetos {
			get { return mObjetos; }
		}

		public int NumeroObjetos {
			get {
				if ((mObjetos != null)) {
					return mObjetos.GetUpperBound(0) + 1;
				} else {
					return 0;
				}
			}
		}

		public Poligono2D[] Represenatciones {
			get { return mRepresentaciones; }
		}

		public bool Vacio {
			get { return mObjetos == null; }
		}

		public double TiempoUltimoCalculo {
			get { return mTiempo; }
		}

		public void Actualizar(ref Poliedro[] Poliedros, ref Camara3D Camara)
		{
			List<ElementoZBuffer> Objetos = new List<ElementoZBuffer>();
			List<Poligono2D> Representaciones = new List<Poligono2D>();
			System.DateTime T = DateAndTime.Now;
			int n = 0;
			if ((Poliedros != null)) {
				if (!Vacio) {
					for (int i = 0; i <= mObjetos.GetUpperBound(0); i++) {
						if (mObjetos[i].Indices[1] <= Poliedros.GetUpperBound(0) && mObjetos[i].Indices[2] < Poliedros[mObjetos[i].Indices[1]].NumeroCaras) {
							if (Poliedros[mObjetos[i].Indices[1]].CaraVisible[mObjetos[i].Indices[2], Camara]) {
								Objetos.Add(new ElementoZBuffer(Poliedros[mObjetos[i].Indices[1]].Caras[mObjetos[i].Indices[2]].BaricentroSRC.Z, n, mObjetos[i].Indices[1], mObjetos[i].Indices[2]));
								Representaciones.Add(new Poligono2D(Poliedros[mObjetos[i].Indices[1]].Caras[mObjetos[i].Indices[2]].Representacion[Poliedros[mObjetos[i].Indices[1]].Vertices]));
								Representaciones[Representaciones.Count - 1].Color = Poliedros[mObjetos[i].Indices[1]].Caras[mObjetos[i].Indices[2]].Color;
								Poliedros[mObjetos[i].Indices[1]].Caras[mObjetos[i].Indices[2]].CargadaEnBuffer = true;

								n += 1;
							} else {
								Poliedros[mObjetos[i].Indices[1]].Caras[mObjetos[i].Indices[2]].CargadaEnBuffer = false;
							}
						}
					}
				}

				for (int i = 0; i <= Poliedros.GetUpperBound(0); i++) {
					for (int j = 0; j <= Poliedros[i].NumeroCaras - 1; j++) {
						if (!Poliedros[i].Caras[j].CargadaEnBuffer) {
							if (Poliedros[i].CaraVisible[j, Camara]) {
								Objetos.Add(new ElementoZBuffer(Poliedros[i].Caras[j].BaricentroSRC.Z, n, i, j));
								Representaciones.Add(Poliedros[i].Representacion[j]);
								Poliedros[i].Caras[j].CargadaEnBuffer = true;

								n += 1;
							}
						}

					}
				}

				mObjetos = Objetos.ToArray();
				mRepresentaciones = Representaciones.ToArray();
				Reordenar();
			} else {
				mObjetos = null;
				mRepresentaciones = null;
			}

			mTiempo = (DateAndTime.Now - T).TotalMilliseconds;

			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void Shading(int IndiceRepresentacion, Color ColorShading)
		{
			if (IndiceRepresentacion >= 0 && IndiceRepresentacion <= mRepresentaciones.GetUpperBound(0)) {
				mRepresentaciones[IndiceRepresentacion].Color = ColorShading;
			}
		}

		public void Reordenar()
		{
			if (!Vacio) {
				Ordenamiento.Sort<ElementoZBuffer>(ref mObjetos);
			}
		}

		public int Pertenece(params double[] Indices)
		{
			if ((mObjetos != null)) {
				for (int i = 0; i <= mObjetos.GetUpperBound(0); i++) {
					if (mObjetos[i].EsEquivalente(Indices))
						return i;
				}

				return -1;
			} else {
				return -1;
			}
		}

		public int Pertenece(int Indice, double ValorIndice)
		{
			if ((mObjetos != null)) {
				for (int i = 0; i <= mObjetos.GetUpperBound(0); i++) {
					if (Indice < mObjetos[i].NumeroIndices) {
						if (mObjetos[i].Indices[Indice] == ValorIndice)
							return i;
					}
				}

				return -1;
			} else {
				return -1;
			}
		}

		public ZBuffer() : base()
		{

			mTiempo = 0;

		}
	}
}

