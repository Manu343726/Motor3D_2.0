using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Motor3D.Espacio2D
{
	public class Quadtree : ObjetoGeometrico2D
	{


		private int mNiveles;

		private SectorQuadtree mSectorRaiz;
		public Caja2D Espacio {
			get { return mSectorRaiz.Espacio; }
			set {
				if (mSectorRaiz.Espacio != value) {
					mSectorRaiz = new SectorQuadtree(mNiveles, value);
				}
			}
		}

		public int Niveles {
			get { return mNiveles; }
			set {
				if (value > 0) {
					mNiveles = value;
					mSectorRaiz = new SectorQuadtree(mNiveles, mSectorRaiz.Espacio);
				}
			}
		}

		public SectorQuadtree SectorRaiz {
			get { return mSectorRaiz; }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Punto); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Colisionan(Caja); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Colisionan(Figura2D.Caja); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Recta); }
		}

		public Quadtree(int Niveles, Caja2D Caja)
		{
			if (Niveles > 1) {
				mNiveles = Niveles;

				mSectorRaiz = new SectorQuadtree(Niveles, Caja);
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (NEW): Un octree debe tener al menos dos niveles:" + Constants.vbNewLine + "Niveles=" + Niveles);
			}
		}

		public void Refrescar()
		{
			mSectorRaiz.Refrescar();
		}

		public SectorQuadtree Sector(Punto2D Punto)
		{
			return Sector(this, Punto);
		}

		public SectorQuadtree Sector(Recta2D Recta)
		{
			return Sector(this, Recta);
		}

		public SectorQuadtree Sector(Caja2D Caja)
		{
			return Sector(this, Caja);
		}

		public SectorQuadtree Sector(Figura2D Figura2D)
		{
			return Sector(this, Figura2D);
		}

		public SectorQuadtree[] Sectores(Recta2D Recta)
		{
			return Sectores(this, Recta);
		}

		public static SectorQuadtree Sector(Quadtree Quadtree, Punto2D Punto)
		{
			int Resultado = Quadtree.SectorRaiz.Pertenece(ref Punto);
			SectorQuadtree S = Quadtree.SectorRaiz;

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
								S = S.SubSectores[Resultado];
							}
						} else {
							return S.Padre;
						}
					}

					return S;
				}
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Punto.ToString() + Constants.vbNewLine + "Espacio=" + Quadtree.Espacio.ToString());
			}
		}

		public static SectorQuadtree Sector(Quadtree Quadtree, Recta2D Recta)
		{
			int Resultado = 0;
			SectorQuadtree S = Quadtree.SectorRaiz;

			Resultado = Quadtree.SectorRaiz.Pertenece(ref Recta);

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
								S = S.SubSectores[Resultado];
							}
						} else {
							return S.Padre;
						}
					}

					return S;
				}
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + Quadtree.Espacio.ToString());
			}
		}

		public static SectorQuadtree[] Sectores(Quadtree Quadtree, Recta2D Recta)
		{
			SectorQuadtree S = Quadtree.SectorRaiz;
			List<SectorQuadtree> Retorno = new List<SectorQuadtree>();

			if (Quadtree.SectorRaiz.Espacio.Pertenece(Recta)) {
				InterseccionRecta(Recta, S, ref Retorno);

				return Retorno.ToArray();
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (PERTENECE): La recta especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Recta=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + Quadtree.Espacio.ToString());
			}
		}

		public static void InterseccionRecta(Recta2D Recta, SectorQuadtree Sector, ref List<SectorQuadtree> ListaRetorno)
		{
			if (!Sector.EsHoja) {
				foreach (SectorQuadtree Hijo in Sector.SubSectores) {
					if (Hijo.Espacio.Pertenece(Recta)) {
						InterseccionRecta(Recta, Hijo, ref ListaRetorno);
					}
				}
			} else {
				if (Sector.Espacio.Pertenece(Recta))
					ListaRetorno.Add(Sector);
			}
		}

		public static SectorQuadtree Sector(Quadtree Quadtree, Caja2D Caja)
		{
			int Resultado = Quadtree.SectorRaiz.Pertenece(ref Caja);
			SectorQuadtree S = Quadtree.SectorRaiz;

			//PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
			if (Resultado != -2) {
				if (Resultado == -1) {
					return Quadtree.SectorRaiz;
				} else {
					do {
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.SubSectores[Resultado];
							}
						} else {
							return S.Padre;
						}
						Resultado = S.Pertenece(ref Caja);
					} while (Resultado != -2 && Resultado != -1);

					return S;
				}
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (PERTENECE): La caja especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Caja=" + Caja.ToString() + Constants.vbNewLine + "Espacio=" + Quadtree.Espacio.ToString());
			}
		}

		public static SectorQuadtree Sector(Quadtree Quadtree, ref Figura2D Figura2D)
		{
			int Resultado = Quadtree.SectorRaiz.Pertenece(ref Figura2D);
			SectorQuadtree S = Quadtree.SectorRaiz;

			//PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
			if (Resultado != -2) {
				if (Resultado == -1) {
					return Quadtree.SectorRaiz;
				} else {
					do {
						if (Resultado != -2) {
							if (Resultado == -1) {
								return S;
							} else {
								S = S.SubSectores[Resultado];
							}
						} else {
							return S.Padre;
						}
						Resultado = S.Pertenece(ref Figura2D);
					} while (Resultado != -2 && Resultado != -1);

					return S;
				}
			} else {
				throw new ExcepcionGeometrica2D("OCTREE (PERTENECE): La figura especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Figura2D=" + Figura2D.ToString() + Constants.vbNewLine + "Espacio=" + Quadtree.Espacio.ToString());
			}
		}

		public override string ToString()
		{
			return "{Quadtree de espacio=" + mSectorRaiz.Espacio.ToString() + "}";
		}
	}
}


