using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor2D.Primitivas2D;

namespace Motor3D.Espacio2D
{
	public class SectorQuadtree : ObjetoGeometrico2D
	{

		private Caja2D mEspacio;
		private int mIndice;
		private int mNivel;

		private int mNiveles;
		private SectorQuadtree mPadre;

		private SectorQuadtree[] mSubSectores;

		private Figura2D[] mObjetos;
		public Caja2D Espacio {
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

		public SectorQuadtree Padre {
			get { return mPadre; }
		}

		public SectorQuadtree[] SubSectores {
			get { return mSubSectores; }
		}

		public bool EsRaiz {
			get { return mNivel == 0; }
		}

		public bool EsHoja {
			get { return (mNivel == mNiveles - 1); }
		}

		public SectorQuadtree SubSectores {
			get {
				if (Indice >= 0 && Indice <= 7) {
					return mSubSectores[Indice];
				} else {
					throw new ExcepcionGeometrica2D("SECTORQuadtree (NEW): El indice de un sector de Quadtree debe estar comprendido entre 0 y 3." + Constants.vbNewLine + "Indice=" + Indice.ToString());
				}
			}
		}

		public bool Vacio {
			get { return (mObjetos != null); }
		}

		public Figura2D[] Objetos {
			get { return mObjetos; }
		}

		public bool Colision {
			get {
				if (mObjetos == null) {
					return false;
				} else {
					return mObjetos.GetUpperBound(0) > 0;
				}
			}
		}

		//SOLO PARA EL NODO RAIZ:
		public SectorQuadtree(int Niveles, Caja2D Espacio)
		{
			if (Niveles > 1) {
				mNivel = 0;
				mNiveles = Niveles;
				mIndice = 0;
				mEspacio = Espacio;
				mPadre = null;

				if (mNivel < mNiveles - 1) {
					mSubSectores = ObtenerSubSectores(this);
				}
			} else {
				throw new ExcepcionGeometrica2D("SECTORQUADTREE (NEW_RAIZ): Un octree necesita al menos dos niveles." + Constants.vbNewLine + "Niveles=" + Niveles.ToString());
			}
		}

		public SectorQuadtree(ref SectorQuadtree Padre, int Indice, Caja2D Caja)
		{
			if (Padre.Niveles > 1) {
				if (Nivel >= 0 && Nivel < Padre.Niveles) {
					if (Indice >= 0 && Indice <= 3) {
						mNivel = Padre.Nivel + 1;
						mNiveles = Padre.Niveles;
						mIndice = Indice;
						mEspacio = Caja;
						mPadre = Padre;

						if (mNivel < mNiveles - 1) {
							mSubSectores = ObtenerSubSectores(this);
						}
					} else {
						throw new ExcepcionGeometrica2D("SECTORQUADTREE (NEW): El indice de un sector de Quadtree debe estar comprendido entre 0 y 3." + Constants.vbNewLine + "Indice=" + Indice.ToString());
					}

				} else {
					throw new ExcepcionGeometrica2D("SECTORQUADTREE (NEW): El nivel de un sector de Quadtree debe estar entre 0 y el número de niveles del Quadtree menos uno." + Constants.vbNewLine + "Niveles del Quadtree=" + Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Nivel.ToString());
				}

			} else {
				throw new ExcepcionGeometrica2D("SECTORQUADTREE (NEW): Un Quadtree debe tener al menos dos niveles." + Constants.vbNewLine + "Niveles del Quadtree=" + Niveles.ToString());
			}
		}

		public bool AñadirFigura2D(ref Figura2D Figura2D)
		{
			if (mObjetos == null) {
				mObjetos = new Figura2D[1];
				mObjetos[0] = Figura2D;
			} else {
				if (!mObjetos.Contains(Figura2D)) {
					Array.Resize(ref mObjetos, mObjetos.GetUpperBound(0) + 2);
					mObjetos[mObjetos.GetUpperBound(0)] = Figura2D;
				} else {
					return false;
				}
			}

			return true;
		}

		public void Refrescar()
		{
			if ((mSubSectores != null)) {
				foreach (SectorQuadtree Hijo in mSubSectores) {
					if ((Hijo != null))
						Hijo.Refrescar();
				}
			}
			mObjetos = null;
		}

		public int Pertenece(ref Punto2D Punto)
		{
			return Pertenece(ref this, ref Punto);
		}

		public int Pertenece(ref Recta2D Recta)
		{
			return Pertenece(ref this, ref Recta);
		}

		public int Pertenece(ref Caja2D Caja)
		{
			return Pertenece(ref this, ref Caja);
		}

		public int Pertenece(ref Figura2D Figura2D)
		{
			return Pertenece(ref this, ref Figura2D);
		}

		public static SectorQuadtree[] ObtenerSubSectores(SectorQuadtree Sector)
		{
			if (Sector.Nivel < Sector.Niveles - 1) {
				SectorQuadtree[] Retorno = new SectorQuadtree[4];
				Punto2D Tamaño = new Punto2D(Sector.Espacio.Ancho / 2, Sector.Espacio.Alto / 2);

				Retorno[0] = new SectorQuadtree(Sector, 0, new Caja2D(Sector.Espacio.Posicion, Tamaño));
				Retorno[1] = new SectorQuadtree(Sector, 1, new Caja2D(new Punto2D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top), Tamaño));
				Retorno[2] = new SectorQuadtree(Sector, 2, new Caja2D(new Punto2D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y), Tamaño));
				Retorno[3] = new SectorQuadtree(Sector, 3, new Caja2D(new Punto2D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y), Tamaño));

				return Retorno;
			} else {
				throw new ExcepcionGeometrica2D("SECTORQUADTREE (NEW): No se pueden generar subsectores de un sector cuyo nivel es máximo." + Constants.vbNewLine + "Niveles del Quadtree=" + Sector.Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Sector.Niveles);
			}
		}

		public static int Pertenece(ref SectorQuadtree Sector, ref Punto2D Punto)
		{
			if (!Sector.EsHoja) {
				for (int i = 0; i <= 3; i++) {
					if (Sector.SubSectores[i].Espacio.Pertenece(Punto)) {
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

		public static int Pertenece(ref SectorQuadtree Sector, ref Recta2D Recta)
		{
			if (!Sector.EsHoja) {
				for (int i = 0; i <= 3; i++) {
					if (Sector.SubSectores[i].Espacio.Pertenece(Recta)) {
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

		public static int Pertenece(ref SectorQuadtree Sector, ref Caja2D Caja)
		{
			if (!Sector.EsHoja && Caja.Dimensiones < Sector.Espacio.Dimensiones / 2) {
				//LA CAJA PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
				for (int i = 0; i <= 3; i++) {
					if (Sector.SubSectores[i].Espacio.Colisionan(Caja)) {
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

		public static int Pertenece(ref SectorQuadtree Sector, ref Figura2D Figura2D)
		{
			if (!Sector.EsHoja && Figura2D.Caja.Dimensiones < Sector.Espacio.Dimensiones / 2) {
				//LA CAJA PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
				for (int i = 0; i <= 3; i++) {
					if (Sector.SubSectores[i].Espacio.Colisionan(Figura2D.Caja)) {
						return i;
					}
				}
			} else {
				if (Figura2D.Caja.Dimensiones < Sector.Espacio.Dimensiones && Sector.Espacio.Colisionan(Figura2D.Caja)) {
					//LA CAJA PERTENECE AL SECTOR:
					Sector.AñadirFigura2D(ref Figura2D);
					return -1;
				} else {
					//LA CAJA NO PERTENECE AL SECTOR:
					return -2;
				}
			}
		}
	}
}
