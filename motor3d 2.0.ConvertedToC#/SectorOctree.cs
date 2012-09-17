using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Primitivas3D;

namespace Motor3D.Espacio3D
{
	public class SectorOctree : ObjetoGeometrico3D
	{

		private Caja3D mEspacio;
		private int mIndice;
		private int mNivel;

		private int mNiveles;
		private SectorOctree mPadre;

		private SectorOctree[] mSubSectores;

		private Poliedro[] mObjetos;
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

		public SectorOctree Padre {
			get { return mPadre; }
		}

		public SectorOctree[] Hijos {
			get { return mSubSectores; }
		}

		public bool EsRaiz {
			get { return mNivel == 0; }
		}

		public bool EsHoja {
			get { return (mNivel == mNiveles - 1); }
		}

		public SectorOctree Hijos {
			get {
				if (Indice >= 0 && Indice <= 7) {
					return mSubSectores[Indice];
				} else {
					throw new ExcepcionGeometrica3D("SECTOROCTREE (HIJOS_GET): El indice de un sector de Octree debe estar comprendido entre 0 y 3." + Constants.vbNewLine + "Indice=" + Indice.ToString());
				}
			}
		}

		public bool Vacio {
			get { return (mObjetos != null); }
		}

		public Poliedro[] Objetos {
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
		public SectorOctree(int Niveles, Caja3D Espacio)
		{
			if (Niveles > 1) {
				mNivel = 0;
				mNiveles = Niveles;
				mIndice = 0;
				mEspacio = Espacio;
				mPadre = null;

				if (mNivel < mNiveles - 1) {
					mSubSectores = ObtenerHijos(this);
				}
			} else {
				throw new ExcepcionGeometrica3D("SECTOROCTREE (NEW_RAIZ): Un octree necesita al menos dos niveles." + Constants.vbNewLine + "Niveles=" + Niveles.ToString());
			}
		}

		public SectorOctree(ref SectorOctree Padre, int Indice, Caja3D Caja)
		{
			if (Padre.Niveles > 1) {
				if (Nivel >= 0 && Nivel < Padre.Niveles) {
					if (Indice >= 0 && Indice <= 7) {
						mNivel = Padre.Nivel + 1;
						mNiveles = Padre.Niveles;
						mIndice = Indice;
						mEspacio = Caja;
						mPadre = Padre;

						if (mNivel < mNiveles - 1) {
							mSubSectores = ObtenerHijos(this);
						}
					} else {
						throw new ExcepcionGeometrica3D("SECTOROCTREE (NEW): El indice de un sector de Octree debe estar comprendido entre 0 y 3." + Constants.vbNewLine + "Indice=" + Indice.ToString());
					}

				} else {
					throw new ExcepcionGeometrica3D("SECTOROCTREE (NEW): El nivel de un sector de Octree debe estar entre 0 y el número de niveles del Octree menos uno." + Constants.vbNewLine + "Niveles del Octree=" + Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Nivel.ToString());
				}

			} else {
				throw new ExcepcionGeometrica3D("SECTOROCTREE (NEW): Un Octree debe tener al menos dos niveles." + Constants.vbNewLine + "Niveles del Octree=" + Niveles.ToString());
			}
		}

		public bool AñadirPoliedro(ref Poliedro Poliedro)
		{
			if (mObjetos == null) {
				mObjetos = new Poliedro[1];
				mObjetos[0] = Poliedro;
			} else {
				if (!mObjetos.Contains(Poliedro)) {
					Array.Resize(ref mObjetos, mObjetos.GetUpperBound(0) + 2);
					mObjetos[mObjetos.GetUpperBound(0)] = Poliedro;
				} else {
					return false;
				}
			}

			return true;
		}

		public void Refrescar()
		{
			if ((mSubSectores != null)) {
				foreach (SectorOctree Hijo in mSubSectores) {
					if ((Hijo != null))
						Hijo.Refrescar();
				}
			}
			mObjetos = null;
		}

		public int Pertenece(ref Punto3D Punto)
		{
			return Pertenece(ref this, Punto);
		}

		public bool Pertenece(ref Recta3D Recta)
		{
			return Pertenece(ref this, Recta);
		}

		public int Pertenece(ref Caja3D Caja)
		{
			return Pertenece(ref this, Caja);
		}

		public int Pertenece(ref Poliedro Poliedro, bool CajaSUR = false)
		{
			return Pertenece(ref this, ref Poliedro, CajaSUR);
		}

		public static SectorOctree[] ObtenerHijos(SectorOctree Sector)
		{
			if (Sector.Nivel < Sector.Niveles - 1) {
				SectorOctree[] Retorno = new SectorOctree[8];
				Vector3D Tamaño = new Vector3D(Sector.Espacio.Ancho / 2, Sector.Espacio.Largo / 2, Sector.Espacio.Alto / 2);

				//ABAJO:
				Retorno[0] = new SectorOctree(Sector, 0, new Caja3D(Sector.Espacio.Posicion, Tamaño));
				Retorno[1] = new SectorOctree(Sector, 1, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down), Tamaño));
				Retorno[2] = new SectorOctree(Sector, 2, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño));
				Retorno[3] = new SectorOctree(Sector, 3, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down), Tamaño));
				//ARRIBA:
				Retorno[4] = new SectorOctree(Sector, 4, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[5] = new SectorOctree(Sector, 5, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[6] = new SectorOctree(Sector, 6, new Caja3D(new Punto3D(Sector.Espacio.Left + Tamaño.X, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño));
				Retorno[7] = new SectorOctree(Sector, 7, new Caja3D(new Punto3D(Sector.Espacio.Left, Sector.Espacio.Top + Tamaño.Y, Sector.Espacio.Down + Tamaño.Z), Tamaño));

				return Retorno;
			} else {
				throw new ExcepcionGeometrica3D("SECTOROCTREE (OBTENERHIJOS): No se pueden generar subsectores de un sector cuyo nivel es máximo." + Constants.vbNewLine + "Niveles del Octree=" + Sector.Niveles.ToString() + Constants.vbNewLine + "Nivel del sector=" + Sector.Niveles);
			}
		}

		public static int Pertenece(ref SectorOctree Sector, ref Punto3D Punto)
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

		public static bool Pertenece(ref SectorOctree Sector, ref Recta3D Recta)
		{
			return Sector.Espacio.Pertenece(Recta);
		}

		public static int Pertenece(ref SectorOctree Sector, ref Caja3D Caja)
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

		public static int Pertenece(ref SectorOctree Sector, ref Poliedro Poliedro, bool CajaSUR = false)
		{
			if (CajaSUR) {
				if (!Sector.EsHoja && Poliedro.CajaSUR.Dimensiones < Sector.Espacio.Dimensiones / 2) {
					//LA CAJA PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
					for (int i = 0; i <= 7; i++) {
						if (Sector.Hijos[i].Espacio.Colisionan(Poliedro.CajaSUR)) {
							return i;
						}
					}
				} else {
					if (Poliedro.CajaSUR.Dimensiones < Sector.Espacio.Dimensiones && Sector.Espacio.Colisionan(Poliedro.CajaSUR)) {
						//LA CAJA PERTENECE AL SECTOR:
						Sector.AñadirPoliedro(ref Poliedro);
						return -1;
					} else {
						//LA CAJA NO PERTENECE AL SECTOR:
						return -2;
					}
				}
			} else {
				if (!Sector.EsHoja && Poliedro.CajaSRC.Dimensiones < Sector.Espacio.Dimensiones / 2) {
					//LA CAJA PERTENECE A ALGUNO DE LOS HIJOS DEL SECTOR:
					for (int i = 0; i <= 7; i++) {
						if (Sector.Hijos[i].Espacio.Colisionan(Poliedro.CajaSRC)) {
							return i;
						}
					}
				} else {
					if (Poliedro.CajaSRC.Dimensiones < Sector.Espacio.Dimensiones && Sector.Espacio.Colisionan(Poliedro.CajaSRC)) {
						//LA CAJA PERTENECE AL SECTOR:
						Sector.AñadirPoliedro(ref Poliedro);
						return -1;
					} else {
						//LA CAJA NO PERTENECE AL SECTOR:
						return -2;
					}
				}
			}

		}
	}
}
