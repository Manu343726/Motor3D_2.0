using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;
using System.Math;

namespace Motor3D.Espacio3D
{
	public class Plano3D : ObjetoGeometrico3D
	{

		private double mA;
		private double mB;
		private double mC;
		private double mD;

		private Vector3D Normal;
		public double A {
			get { return mA; }
		}

		public double B {
			get { return mB; }
		}

		public double C {
			get { return mC; }
		}

		public double D {
			get { return mD; }
		}

		public Vector3D VectorNormal {
			get { return Normal; }
		}

		public Plano3D(double A, double B, double C, double D)
		{
			mA = A;
			mB = B;
			mC = C;
			mD = D;
			Normal = new Vector3D(A, B, C);
		}

		public Plano3D(Vector3D V1, Vector3D V2, Punto3D Punto)
		{
			Normal = V1 + V2;

			mA = Normal.X;
			mB = Normal.Y;
			mC = Normal.Z;

			mD = -((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z));
		}

		public Plano3D(Punto3D P1, Punto3D P2, Punto3D P3)
		{
			Normal = new Vector3D(P1, P2) + new Vector3D(P1, P3);

			mA = Normal.X;
			mB = Normal.Y;
			mC = Normal.Z;

			mD = -((mA * P1.X) + (mB * P1.Y) + (mC * P1.Z));
		}

		public Plano3D(Punto3D Punto, Vector3D Vector)
		{
			Normal = Vector.VectorUnitario;

			mA = Normal.X;
			mB = Normal.Y;
			mC = Normal.Z;

			mD = -((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z));
		}

		public Punto3D ObtenerPunto(double X, double Y)
		{
			return new Punto3D(X, Y, ((mA * X) + (mB * Y) + mD) / (-mC));
		}

		public bool Pertenece(Punto3D Punto)
		{
			return ((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD == 0);
		}

		public double PosicionRelativa(Punto3D Punto)
		{
			return (mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD;
		}

		public double SignoPosicionRelativa(Punto3D Punto)
		{
			return Math.Sign((mA * Punto.X) + (mB * Punto.Y) + (mC * Punto.Z) + mD);
		}

		public PosicionRelativa3D PosicionRelativa(Plano3D Plano)
		{
			return PosicionRelativa(this, Plano);
		}

		public Ecuacion ObtenerEcuacion()
		{
			return new Ecuacion(mA, mB, mC, -mD);
		}

		public override string ToString()
		{
			return Strings.FormatNumber(mA, 2) + "X" + (mB >= 0 ? "+" + Strings.FormatNumber(mB, 2) : Strings.FormatNumber(mB, 2)) + "Y" + (mC >= 0 ? "+" + Strings.FormatNumber(mC, 2) : Strings.FormatNumber(mC, 2)) + "Z" + (mD >= 0 ? "+" + Strings.FormatNumber(mD, 2) : Strings.FormatNumber(mD, 2)) + "=0";
		}

		public static PosicionRelativa3D PosicionRelativa(Plano3D P1, Plano3D P2)
		{
			PosicionRelativa3D Retorno = null;
			SistemaEcuaciones Sis = null;

			Sis = new SistemaEcuaciones(P1.ObtenerEcuacion(), P2.ObtenerEcuacion());

			switch (Sis.Solucion.TipoSolucion) {
				case TipoSolucionSistema.SistemaCompatibleDeterminado:
					Retorno = new PosicionRelativa3D(new Punto3D(Sis.Solucion.ValorSolucion[0], Sis.Solucion.ValorSolucion[1], Sis.Solucion.ValorSolucion[2]));
					break;
				case TipoSolucionSistema.SistemaCompatibleIndeterminado:
					Retorno = new PosicionRelativa3D(TipoPosicionRelativa3D.Coincidente);
					break;
				default:
					Retorno = new PosicionRelativa3D(TipoPosicionRelativa3D.Paralelo);
					break;
			}

			return Retorno;
		}

		public static double PosicionRelativa(Plano3D Plano, Punto3D Punto)
		{
			return (Plano.A * Punto.X) + (Plano.B * Punto.Y) + (Plano.C * Punto.Z) + Plano.C;
		}

		public static double SignoPosicionRelativa(Plano3D Plano, Punto3D Punto)
		{
			return Math.Sign((Plano.A * Punto.X) + (Plano.B * Punto.Y) + (Plano.C * Punto.Z) + Plano.D);
		}

		public static Punto3D Interseccion(Plano3D Plano, Recta3D Recta)
		{
			SistemaEcuaciones sis = null;

			if (Plano.VectorNormal * Recta.VectorDirector != 0) {
				sis = new SistemaEcuaciones(Plano.ObtenerEcuacion(), Recta.PrimerPlano.ObtenerEcuacion(), Recta.SegundoPlano.ObtenerEcuacion());

				if (sis.Solucion.TipoSolucion == TipoSolucionSistema.SistemaCompatibleDeterminado) {
					return new Punto3D(sis.Solucion.ValorSolucion[0], sis.Solucion.ValorSolucion[1], sis.Solucion.ValorSolucion[2]);
				} else {
					if (sis.Solucion.TipoSolucion == TipoSolucionSistema.SistemaCompatibleIndeterminado) {
						return Plano.ObtenerPunto(0, 0);
					} else {
						throw new ExcepcionGeometrica3D("PLANO3D (INTERSECCION): No se ha podido calcular la interseccion. Es posible que los datos de los planos sean erroneos, o que el cálculo del sistema halla fallado." + Constants.vbNewLine + "Recta=" + Recta.ToString() + Constants.vbNewLine + "Plano: " + Plano.ToString() + Constants.vbNewLine + "Primer plano: " + Recta.PrimerPlano.ToString() + Constants.vbNewLine + "Seundo plano: " + Recta.SegundoPlano.ToString() + Constants.vbNewLine + "Primera ecuación del sistema: " + Plano.ObtenerEcuacion().ToString() + Constants.vbNewLine + "Segunda ecuación del sistema: " + Recta.PrimerPlano.ObtenerEcuacion().ToString() + Constants.vbNewLine + "Tercera ecuación del sistema: " + Recta.SegundoPlano.ObtenerEcuacion().ToString() + Constants.vbNewLine + "Solución obtenida: " + sis.Solucion.ToString());
					}
				}
			} else {
				if (Recta3D.Distancia(Recta, Plano) == 0) {
					return Recta.PuntoInicial;
				} else {
					throw new ExcepcionGeometrica3D("PLANO3D (INTERSECCION): La recta y el plano son paralelos" + Constants.vbNewLine + "Recta: " + Recta.ToString() + Constants.vbNewLine + "Plano: " + Plano.ToString());
				}
			}
		}
	}
}

