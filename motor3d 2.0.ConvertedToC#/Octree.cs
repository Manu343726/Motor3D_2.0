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
	public class Octree : ObjetoGeometrico3D
	{


		private int mNiveles;

		private SectorOctree mSectorRaiz;
		public Caja3D Espacio {
			get { return mSectorRaiz.Espacio; }
			set {
				if (mSectorRaiz.Espacio != value) {
					mSectorRaiz = new SectorOctree(mNiveles, value);
				}
			}
		}

		public int Niveles {
			get { return mNiveles; }
			set {
				if (value > 0) {
					mNiveles = value;
					mSectorRaiz = new SectorOctree(mNiveles, mSectorRaiz.Espacio);
				}
			}
		}

		public SectorOctree SectorRaiz {
			get { return mSectorRaiz; }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Punto); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Colisionan(Caja); }
		}

		public bool Pertenece {
			get {
				if (CajaSUR) {
					return mSectorRaiz.Espacio.Colisionan(Poliedro.CajaSUR);
				} else {
					return mSectorRaiz.Espacio.Colisionan(Poliedro.CajaSRC);
				}
			}
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Recta); }
		}

		public Octree(int Niveles, Caja3D Caja)
		{
			if (Niveles > 1) {
				mNiveles = Niveles;

				mSectorRaiz = new SectorOctree(Niveles, Caja);
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (NEW): Un octree debe tener al menos dos niveles:" + Constants.vbNewLine + "Niveles=" + Niveles);
			}
		}

		public void Refrescar()
		{
			mSectorRaiz.Refrescar();
		}

		public SectorOctree Sector(Punto3D Punto)
		{
			return Sector(this, Punto);
		}

		public SectorOctree Sector(Recta3D Recta)
		{
			return Sector(this, Recta);
		}

		public SectorOctree Sector(Caja3D Caja)
		{
			return Sector(this, Caja);
		}

		public SectorOctree Sector(Poliedro Poliedro, bool CajaSUR = false)
		{
			return Sector(this, ref Poliedro, CajaSUR);
		}

		public SectorOctree[] Sectores(Recta3D Recta)
		{
			return Sectores(this, Recta);
		}

		public static SectorOctree Sector(Octree Octree, Punto3D Punto)
		{
			int Resultado = Octree.SectorRaiz.Pertenece(ref Punto);
			SectorOctree S = Octree.SectorRaiz;

			if (Resultado != -2) {
				if (Resultado == -1) {
					return S;
				} else {
					while (Resultado != -2 && Resultado != -1) {
						Resultado = S.Pertenece(ref Punto);
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.Hijos[Resultado];
							}
						} else {
							return S.Padre;
						}
					}

					return S;
				}
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Punto.ToString() + Constants.vbNewLine + "Espacio=" + Octree.Espacio.ToString());
			}
		}

		public static SectorOctree Sector(Octree Octree, Recta3D Recta)
		{
			int Resultado = Octree.SectorRaiz.Pertenece(ref Recta);
			SectorOctree S = Octree.SectorRaiz;

			if (Resultado != -2) {
				if (Resultado == -1) {
					return S;
				} else {
					while (Resultado != -2 && Resultado != -1) {
						Resultado = S.Pertenece(ref Recta);
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.Hijos[Resultado];
							}
						} else {
							return S.Padre;
						}
					}

					return S;
				}
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + Octree.Espacio.ToString());
			}
		}

		public static SectorOctree[] Sectores(Octree Octree, Recta3D Recta)
		{
			SectorOctree S = Octree.SectorRaiz;
			List<SectorOctree> Retorno = new List<SectorOctree>();

			if (Octree.SectorRaiz.Pertenece(ref Recta)) {
				InterseccionRecta(Recta, S, ref Retorno);

				return Retorno.ToArray();
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (PERTENECE): La recta especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Recta=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + Octree.Espacio.ToString());
			}
		}

		public static void InterseccionRecta(Recta3D Recta, SectorOctree Sector, ref List<SectorOctree> ListaRetorno)
		{
			if (!Sector.EsHoja) {
				foreach (SectorOctree Hijo in Sector.Hijos) {
					if (Hijo.Pertenece(ref Recta)) {
						InterseccionRecta(Recta, Hijo, ref ListaRetorno);
					}
				}
			} else {
				if (Sector.Pertenece(ref Recta))
					ListaRetorno.Add(Sector);
			}
		}

		public static SectorOctree Sector(Octree Octree, Caja3D Caja)
		{
			int Resultado = Octree.SectorRaiz.Pertenece(ref Caja);
			SectorOctree S = Octree.SectorRaiz;

			//PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
			if (Resultado != -2) {
				if (Resultado == -1) {
					return Octree.SectorRaiz;
				} else {
					do {
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.Hijos[Resultado];
							}
						} else {
							return S.Padre;
						}
						Resultado = S.Pertenece(ref Caja);
					} while (Resultado != -2 && Resultado != -1);

					return S;
				}
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (PERTENECE): La caja especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Caja=" + Caja.ToString() + Constants.vbNewLine + "Espacio=" + Octree.Espacio.ToString());
			}
		}

		public static SectorOctree Sector(Octree Octree, ref Poliedro Poliedro, bool CajaSUR = false)
		{
			int Resultado = Octree.SectorRaiz.Pertenece(ref Poliedro, CajaSUR);
			SectorOctree S = Octree.SectorRaiz;

			//PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
			if (Resultado != -2) {
				if (Resultado == -1) {
					return Octree.SectorRaiz;
				} else {
					do {
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.Hijos[Resultado];
							}
						} else {
							return S.Padre;
						}
						Resultado = S.Pertenece(ref Poliedro, CajaSUR);
					} while (Resultado != -2 && Resultado != -1);

					return S;
				}
			} else {
				throw new ExcepcionGeometrica3D("OCTREE (PERTENECE): El poliedro especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Poliedro=" + Poliedro.ToString() + Constants.vbNewLine + "Espacio=" + Octree.Espacio.ToString());
			}
		}

		public override string ToString()
		{
			return "{Octree de espacio=" + mSectorRaiz.Espacio.ToString() + "}";
		}
	}
}


