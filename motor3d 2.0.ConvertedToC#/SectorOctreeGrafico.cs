using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using System.Drawing;

namespace Motor3D.Primitivas3D
{
	public class SectorOctreeGrafico : ObjetoGeometrico3D
	{

		private Caja3D mEspacio;
		private int mIndice;
		private int mNivel;

		private int mNiveles;
		private SectorOctreeGrafico mPadre;

		private SectorOctreeGrafico[] mHijos;
		private Color mColor;

		private bool mVacio;
		public Caja3D Espacio {
			get { return mEspacio; }
		}

		public int Indice {
			get { return mIndice; }
		}

		public int Nivel {
			get { return mNivel; }
		}

		public int Niveles {
			get { return mNiveles; }
		}

		public SectorOctreeGrafico Padre {
			get { return mPadre; }
		}

		public SectorOctreeGrafico[] Hijos {
			get { return mHijos; }
		}

		public bool EsRaiz {
			get { return mNivel == 0; }
		}

		public bool EsHoja {
			get { return (mNivel == mNiveles - 1); }
		}

		public SectorOctreeGrafico Hijos {
			get {
				if (Indice >= 0 && Indice <= 7) {
					return mHijos[Indice];
				} else {
					throw new ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (HIJOS_GET): El indice de un sector de Octree debe estar comprendido entre 0 y 7." + Constants.vbNewLine + "Indice=" + Indice.ToString());
				}
			}
		}

		public bool Vacio {
			get { return mVacio; }
		}

		public Color Color {
			get { return mColor; }
		}

		//SOLO PARA EL NODO RAIZ:
		public SectorOctreeGrafico(int Niveles, Caja3D Espacio)
		{
			if (Niveles > 1) {
				mNivel = 0;
				mNiveles = Niveles;
				mIndice = 0;
				mEspacio = Espacio;
				mPadre = null;
				mVacio = true;

				if (mNivel < mNiveles - 1) {
					mHijos = ObtenerSubSectores(this);
				}
			} else {
				throw new ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW_RAIZ): Un octree necesita al menos dos niveles." + Constants.vbNewLine + "Niveles=" + Niveles.ToString());
			}
		}

		public SectorOctreeGrafico(ref SectorOctreeGrafico Padre, int Indice, ref Caja3D Caja)
		{
			if (Padre.Niveles > 1) {
				if (Nivel >= 0 && Nivel < Padre.Niveles) {
					if (Indice >= 0 && Indice <= 7) {
						mNivel = Padre.Nivel + 1;
						mNiveles = Padre.Niveles;
						mIndice = Indice;
						mEspacio = Caja;
						mPadre = Padre;
						mVacio = true;

						if (mNivel < mNiveles - 1) {
							mHijos = ObtenerSubSectores(this);
						}
					} else {
						throw new ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): El indice de un sector de Octree debe estar comprendido entre 0 y 7." + Constants.vbNewLine + "Indice=" + Indice.ToString());
					}

				} else {
					throw new ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): El nivel de un sector de Octree debe estar entre 0 y el número de niveles del Octree menos uno." + Constants.vbNewLine + "Niveles del Octree=" + Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Nivel.ToString());
				}

			} else {
				throw new ExcepcionPrimitiva3D("SECTOROCTREEGRAFICO (NEW): Un Octree debe tener al menos dos niveles." + Constants.vbNewLine + "Niveles del Octree=" + Niveles.ToString());
			}
		}

		public void Vaciar()
		{
			if (mHijos == null) {
				mColor = System.Drawing.Color.Black;
				mVacio = true;
			}
		}

		public void Rellenar(Color Color)
		{
			if (mHijos == null) {
				mColor = Color;
				mVacio = false;
				if ((mPadre != null)) {
					mPadre.EvaluarFusion();
				}
			}
		}

		public void EvaluarFusion(int HijoFusionado = -1)
		{
			bool Salir = false;
			for (int i = 0; i <= 7; i++) {
				if (mHijos[i].Vacio) {
					if (HijoFusionado == i) {
						mHijos[i].Fusion();
						return;
					}

					Salir = true;
				}
			}
			if (!Salir) {
				if (mPadre == null) {
					mColor = Fusionar();
				} else {
					mPadre.EvaluarFusion(mIndice);
				}
			}
		}

		public void Fusion()
		{
			mVacio = false;
			mColor = Fusionar();
		}

		public Color Fusionar()
		{
			if (mHijos == null) {
				return mColor;
			} else {
				long r = 0;
				long g = 0;
				long b = 0;
				Color Color = default(Color);

				r = 0;
				g = 0;
				b = 0;

				for (int i = 0; i <= 7; i++) {
					Color = mHijos[i].Fusionar();
					r += Color.R;
					g += Color.G;
					b += Color.B;
				}

				return Color.FromArgb(255, r / 8, g / 8, b / 8);
			}
		}

		public int Pertenece(ref Punto3D Punto)
		{
			return Pertenece(ref this, ref Punto);
		}

		public int Pertenece(ref Recta3D Recta)
		{
			return Pertenece(ref this, ref Recta);
		}

		public int Pertenece(ref Caja3D Caja)
		{
			return Pertenece(ref this, ref Caja);
		}

		public static SectorOctreeGrafico[] ObtenerSubSectores(SectorOctreeGrafico Sector)
		{
			if (Sector.Nivel < Sector.Niveles - 1) {
				SectorOctreeGrafico[] Retorno = new SectorOctreeGrafico[8];
				Vector3D Tamaño = new Vector3D(Sector.Espacio.Ancho / 2, Sector.Espacio.Largo / 2, Sector.Espacio.Alto / 2);

				//ABAJO:
				Retorno[0] = new SectorOctreeGrafico(Sector, 0, new Caja3D(Sector.Espacio.Posicion, Tamaño));
				Retorno[1] = new SectorOctreeGrafico(Sector, 1, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down), Tamaño));
				Retorno[2] = new SectorOctreeGrafico(Sector, 2, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño));
				Retorno[3] = new SectorOctreeGrafico(Sector, 3, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño));
				//ARRIBA:
				Retorno[4] = new SectorOctreeGrafico(Sector, 4, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[5] = new SectorOctreeGrafico(Sector, 5, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[6] = new SectorOctreeGrafico(Sector, 6, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[7] = new SectorOctreeGrafico(Sector, 7, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño));

				return Retorno;
			} else {
				throw new ExcepcionGeometrica3D("SECTOROCTREEGRAFICO (NEW): No se pueden generar subsectores de un sector cuyo nivel es máximo." + Constants.vbNewLine + "Niveles del Octree=" + Sector.Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Sector.Niveles);
			}
		}

		public static int Pertenece(ref SectorOctreeGrafico Sector, ref Punto3D Punto)
		{
			if (!Sector.EsHoja) {
				for (int i = 0; i <= 7; i++) {
					if (Sector.Hijos[i].Espacio.Pertenece(Punto)) {
						return i;
					}
				}
			} else {
				if (Sector.Espacio.Pertenece(Punto)) {
					return -1;
				} else {
					return -2;
				}
			}
		}

		public static int Pertenece(ref SectorOctreeGrafico Sector, ref Recta3D Recta)
		{
			if (!Sector.EsHoja) {
				for (int i = 0; i <= 7; i++) {
					if (Sector.Hijos[i].Espacio.Pertenece(Recta)) {
						return i;
					}
				}
			} else {
				if (Sector.Espacio.Pertenece(Recta)) {
					return -1;
				} else {
					return -2;
				}
			}
		}

		public static int Pertenece(ref SectorOctreeGrafico Sector, ref Caja3D Caja)
		{
			if (!Sector.EsHoja && Caja.Dimensiones < Sector.Espacio.Dimensiones / 2) {
				//LA CAJA PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
				for (int i = 0; i <= 7; i++) {
					if (Sector.Hijos[i].Espacio.Colisionan(Caja)) {
						return i;
					}
				}
			} else {
				if (Caja.Dimensiones < Sector.Espacio.Dimensiones && Sector.Espacio.Colisionan(Caja)) {
					//LA CAJA PERTENECE AL SECTOR:
					return -1;
				} else {
					//LA CAJA NO PERTENECE AL SECTOR:
					return -2;
				}
			}
		}
	}
}

