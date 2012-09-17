using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Math;

namespace Motor3D.Espacio2D
{
	public class Segmento2D : ObjetoGeometrico2D
	{

		private Punto2D Inicio;
		private Punto2D Fin;
		private Recta2D mRecta;
		private double Modulo;

		private Caja2D Rectangulo;
		public Punto2D ExtremoInicial {
			get { return Inicio; }
			set {
				Inicio = value;
				RecalcCaja();
			}
		}

		public Punto2D ExtremoFinal {
			get { return Fin; }
			set {
				Fin = value;
				RecalcCaja();
			}
		}

		public Recta2D Recta {
			get { return mRecta; }
		}

		public Caja2D Caja {
			get { return Rectangulo; }
		}

		public double Longitud {
			get { return Modulo; }
		}

		public int Pendiente {
			get {
				if (!YInvertida) {
					if (Inicio.X <= Fin.X && Inicio.Y >= Fin.Y)
						return 0;
					if (Inicio.X >= Fin.X && Inicio.Y >= Fin.Y)
						return -1;
					if (Inicio.X >= Fin.X && Inicio.Y <= Fin.Y)
						return 0;
					if (Inicio.X <= Fin.X && Inicio.Y <= Fin.Y)
						return -1;
				} else {
					if (Inicio.X <= Fin.X && Inicio.Y >= Fin.Y)
						return -1;
					if (Inicio.X >= Fin.X && Inicio.Y >= Fin.Y)
						return 0;
					if (Inicio.X >= Fin.X && Inicio.Y <= Fin.Y)
						return -1;
					if (Inicio.X <= Fin.X && Inicio.Y <= Fin.Y)
						return 0;
				}
			}
		}

		public Punto2D PuntoMedio {
			get { return new Punto2D((Inicio.X + Fin.X) / 2, (Inicio.Y + Fin.Y) / 2); }
		}

		public Segmento2D(Punto2D P1, Punto2D P2)
		{
			Inicio = P1;
			Fin = P2;

			Modulo = Math.Sqrt((Math.Pow((P1.X + P2.X), 2)) + (Math.Pow((P1.Y + P2.Y), 2)));
			mRecta = new Recta2D(P1, P2);

			Rectangulo = new Caja2D(P1, new Punto2D(P2.X - P1.X, P2.Y - P1.Y));
		}

		private void RecalcCaja()
		{
			mRecta = new Recta2D(Inicio, Fin);
			Rectangulo = new Caja2D(Inicio, new Punto2D(Fin.X - Inicio.X, Fin.Y - Inicio.Y));
			Modulo = Math.Sqrt((Math.Pow((Inicio.X + Fin.X), 2)) + (Math.Pow((Inicio.Y + Fin.Y), 2)));
		}

		public bool Pertenece(Punto2D Punto)
		{
			return Pertenece(this, Punto);
		}

		public bool Pertenece(Segmento2D Segmento, Punto2D Punto)
		{
			return Rectangulo.Pertenece(Punto) && mRecta.Pertenece(Punto);
		}

		public static bool Colisionan(Segmento2D S1, Segmento2D S2, int Niveles, bool BSP = false)
		{
			Caja2D[] CajasS1 = null;
			Caja2D[] CajasS2 = null;
			if (BSP) {
				int Nivel = 1;
				if (Niveles < 1)
					Niveles = 1;

				CajasS1 = Caja2D.SubCajasDiagonales(S1.Caja, S1.Pendiente);
				CajasS2 = Caja2D.SubCajasDiagonales(S2.Caja, S2.Pendiente);

				while (true) {
					for (int i = 0; i <= 1; i++) {
						for (int j = 0; j <= 1; j++) {
							if (Caja2D.Colisionan(CajasS1[i], CajasS2[j])) {
								if (Nivel < Niveles) {
									CajasS1 = Caja2D.SubCajasDiagonales(CajasS1[i], S1.Pendiente);
									CajasS2 = Caja2D.SubCajasDiagonales(CajasS2[j], S2.Pendiente);
									Nivel += 1;
									continue;
								} else {
									return true;
								}
							} else {
								if (Nivel == Niveles && Circunferencia2D.Colisionan(new Circunferencia2D(CajasS1[i]), new Circunferencia2D(CajasS2[j]))) {
									return true;
								}
							}
						}
					}

					return false;
				}
				return false;
			} else {
				CajasS1 = Caja2D.SubCajasDiagonales(S1.Caja, S1.Pendiente, Niveles);
				CajasS2 = Caja2D.SubCajasDiagonales(S2.Caja, S2.Pendiente, Niveles);

				for (int i = 0; i <= CajasS1.GetUpperBound(0); i++) {
					for (int j = 0; j <= CajasS2.GetUpperBound(0); j++) {
						if (Caja2D.Colisionan(CajasS1[i], CajasS2[j])) {
							return true;
						}
					}
				}

				return false;
			}

		}

		public static Caja2D[] UltimasCajasBSP(Segmento2D S1, Segmento2D S2, int NivelesBSP = 1)
		{
			int Nivel = 1;
			Caja2D[] CajasS1 = null;
			Caja2D[] CajasS2 = null;
			if (NivelesBSP < 1)
				NivelesBSP = 1;
			Caja2D[] Retorno = null;

			CajasS1 = Caja2D.SubCajasDiagonales(S1.Caja, S1.Pendiente);
			CajasS2 = Caja2D.SubCajasDiagonales(S2.Caja, S2.Pendiente);

			while (true) {
				for (int i = 0; i <= 1; i++) {
					for (int j = 0; j <= 1; j++) {
						if (Caja2D.Colisionan(CajasS1[i], CajasS2[j])) {
							if (Nivel < NivelesBSP) {
								CajasS1 = Caja2D.SubCajasDiagonales(CajasS1[i], S1.Pendiente);
								CajasS2 = Caja2D.SubCajasDiagonales(CajasS2[j], S2.Pendiente);
								Nivel += 1;
								continue;
							} else {
								Retorno = new Caja2D[2];

								Retorno[0] = CajasS1[i];
								Retorno[1] = CajasS2[j];

								return Retorno;
							}
						}
					}
				}

				Retorno = new Caja2D[4];

				Retorno[0] = CajasS1[0];
				Retorno[1] = CajasS1[1];
				Retorno[2] = CajasS2[0];
				Retorno[3] = CajasS2[1];

				return Retorno;
			}

			Retorno = new Caja2D[4];

			Retorno[0] = CajasS1[0];
			Retorno[1] = CajasS1[1];
			Retorno[2] = CajasS2[0];
			Retorno[3] = CajasS2[1];

			return Retorno;
		}

		public static Punto2D Interseccion(Segmento2D S1, Segmento2D S2)
		{
			try {
				Punto2D Pi = Recta2D.Interseccion(S1.Recta, S2.Recta);

				return Pi;
			} catch (ExcepcionGeometrica2D ex) {
				return null;
			}
		}

		public override string ToString()
		{
			return "{Segmento. Inicio=" + Inicio.ToString() + ",Fin=" + Fin.ToString() + "}";
		}
	}
}


