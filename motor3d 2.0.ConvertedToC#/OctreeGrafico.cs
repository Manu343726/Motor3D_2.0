using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;

namespace Motor3D.Primitivas3D
{
	public class OctreeGrafico : ObjetoGeometrico3D
	{


		private int mNiveles;

		private SectorOctreeGrafico mSectorRaiz;
		public Caja3D Espacio {
			get { return mSectorRaiz.Espacio; }
			set {
				if (mSectorRaiz.Espacio != value) {
					mSectorRaiz = new SectorOctreeGrafico(mNiveles, value);
				}
			}
		}

		public int Niveles {
			get { return mNiveles; }
			set {
				if (value > 0) {
					mNiveles = value;
					mSectorRaiz = new SectorOctreeGrafico(mNiveles, mSectorRaiz.Espacio);
				}
			}
		}

		public SectorOctreeGrafico SectorRaiz {
			get { return mSectorRaiz; }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Punto); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Colisionan(Caja); }
		}

		public bool Pertenece {
			get { return mSectorRaiz.Espacio.Pertenece(Recta); }
		}

		public OctreeGrafico(int Niveles, Caja3D Caja)
		{
			if (Niveles > 1) {
				mNiveles = Niveles;

				mSectorRaiz = new SectorOctreeGrafico(Niveles, Caja);
			} else {
				throw new ExcepcionGeometrica3D("OctreeGrafico (NEW): Un OctreeGrafico debe tener al menos dos niveles:" + Constants.vbNewLine + "Niveles=" + Niveles);
			}
		}

		public SectorOctreeGrafico Sector(Punto3D Punto)
		{
			return Sector(this, Punto);
		}

		public SectorOctreeGrafico Sector(Recta3D Recta)
		{
			return Sector(this, Recta);
		}

		public SectorOctreeGrafico Sector(Caja3D Caja)
		{
			return Sector(this, Caja);
		}

		public SectorOctreeGrafico[] Sectores(Recta3D Recta)
		{
			return Sectores(this, Recta);
		}

		public static SectorOctreeGrafico Sector(OctreeGrafico OctreeGrafico, Punto3D Punto)
		{
			int Resultado = OctreeGrafico.SectorRaiz.Pertenece(ref Punto);
			SectorOctreeGrafico S = OctreeGrafico.SectorRaiz;

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
				throw new ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): El punto especificado no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Punto.ToString() + Constants.vbNewLine + "Espacio=" + OctreeGrafico.Espacio.ToString());
			}
		}

		public static SectorOctreeGrafico Sector(OctreeGrafico OctreeGrafico, Recta3D Recta)
		{
			int Resultado = OctreeGrafico.SectorRaiz.Pertenece(ref Recta);
			SectorOctreeGrafico S = OctreeGrafico.SectorRaiz;

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
				throw new ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): ELa recta especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Punto=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + OctreeGrafico.Espacio.ToString());
			}
		}

		public static SectorOctreeGrafico[] Sectores(OctreeGrafico OctreeGrafico, Recta3D Recta)
		{
			SectorOctreeGrafico S = OctreeGrafico.SectorRaiz;
			List<SectorOctreeGrafico> Retorno = new List<SectorOctreeGrafico>();

			if (OctreeGrafico.SectorRaiz.Espacio.Pertenece(Recta)) {
				InterseccionRecta(Recta, S, ref Retorno);

				return Retorno.ToArray();
			} else {
				throw new ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): La recta especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Recta=" + Recta.ToString() + Constants.vbNewLine + "Espacio=" + OctreeGrafico.Espacio.ToString());
			}
		}

		public static void InterseccionRecta(Recta3D Recta, SectorOctreeGrafico Sector, ref List<SectorOctreeGrafico> ListaRetorno)
		{
			if (!Sector.EsHoja) {
				foreach (SectorOctreeGrafico Hijo in Sector.Hijos) {
					if (Hijo.Espacio.Pertenece(Recta)) {
						InterseccionRecta(Recta, Hijo, ref ListaRetorno);
					}
				}
			} else {
				if (Sector.Espacio.Pertenece(Recta))
					ListaRetorno.Add(Sector);
			}
		}

		public static SectorOctreeGrafico Sector(OctreeGrafico OctreeGrafico, Caja3D Caja)
		{
			int Resultado = OctreeGrafico.SectorRaiz.Pertenece(ref Caja);
			SectorOctreeGrafico S = OctreeGrafico.SectorRaiz;

			//PARA ENTENDER EL ALGORITMO, IR A FUNCION PERTENECE DE SECTORQUADTREE.
			if (Resultado != -2) {
				if (Resultado == -1) {
					return OctreeGrafico.SectorRaiz;
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
				throw new ExcepcionGeometrica3D("OctreeGrafico (PERTENECE): La caja especificada no pertenece al espacio dominado por el quadtree." + Constants.vbNewLine + "Caja=" + Caja.ToString() + Constants.vbNewLine + "Espacio=" + OctreeGrafico.Espacio.ToString());
			}
		}

		public override string ToString()
		{
			return "{OctreeGrafico de espacio=" + mSectorRaiz.Espacio.ToString() + "}";
		}
	}
}
