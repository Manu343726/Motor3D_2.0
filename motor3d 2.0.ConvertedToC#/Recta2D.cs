using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Math;
using Motor3D.Algebra;

namespace Motor3D.Espacio2D
{
	public class Recta2D : ObjetoGeometrico2D
	{

		private double mA;
		private double mB;
		private double mC;
		private Vector2D mVector;
		private Punto2D mPunto;
		private Punto2D mPuntoMira;

		private double mPendiente;
		public Vector2D VectorDirector {
			get { return mVector; }
		}

		public Punto2D PuntoDiretor {
			get { return mPunto; }
		}

		public Punto2D PuntoDeMira {
			get { return mPuntoMira; }
		}

		public double Pendiente {
			get {
				if (mB != 0) {
					return mPendiente;
				} else {
					throw new ExcepcionGeometrica2D("RECTA2D (PENDIENTE_GET): Pendiente infinita (Recta perpendicular al eje X)" + Constants.vbNewLine + "Ecuación de la recta: " + this.ToString());
				}
			}
		}

		public double A {
			get { return mA; }
		}

		public double B {
			get { return mB; }
		}

		public double C {
			get { return mC; }
		}

		public Matriz[] Matrices {
			get { return RepresentacionMatricial(this); }
		}

		public Recta2D(params Matriz[] Matrices)
		{
			if (Matrices.GetUpperBound(0) == 1) {
				Punto2D Punto = new Punto2D(Matrices[0]);
				Punto2D Punto2 = new Punto2D(Matrices[1]);

				mPunto = Punto;
				mPuntoMira = Punto2;
				mVector = new Vector2D(mPunto, mPuntoMira).VectorUnitario;

				mA = -mVector.Y;
				mB = mVector.X;
				mC = -((mA * mPunto.X) + (mB * mPunto.Y));
				mPendiente = (mB != 0 ? -(mA / mB) : 0);
			} else {
				throw new ExcepcionGeometrica2D("RECTA2D (NEW): La representación matricial de una recta corresponde a un array con dos matrices" + Constants.vbNewLine + "Tamaño del array=" + Matrices.GetUpperBound(0) + 1);
			}
		}

		public Recta2D(double ValA, double ValB, double ValC)
		{
			mA = ValA / (Math.Sqrt((Math.Pow(ValA, 2)) + (Math.Pow(ValB, 2)) + (Math.Pow(ValC, 2))));
			mB = ValB / (Math.Sqrt((Math.Pow(ValA, 2)) + (Math.Pow(ValB, 2)) + (Math.Pow(ValC, 2))));
			mC = ValC / (Math.Sqrt((Math.Pow(ValA, 2)) + (Math.Pow(ValB, 2)) + (Math.Pow(ValC, 2))));

			mVector = new Vector2D(mB, -mA);
			mPunto = new Punto2D(0, -(mC / mB));
			mPuntoMira = new Punto2D(mPunto.X + mVector.X, mPunto.Y + mVector.Y);
			mPendiente = (mB != 0 ? -(mA / mB) : 0);
		}

		public Recta2D(Punto2D P1, Punto2D P2)
		{
			mVector = new Vector2D(P1, P2).VectorUnitario;
			mPunto = P1;
			mPuntoMira = P2;

			mA = -mVector.Y;
			mB = mVector.X;
			mC = -((mA * P1.X) + (mB * P1.Y));
			mPendiente = (mB != 0 ? -(mA / mB) : 0);
		}

		public Recta2D(Punto2D ValPunto, Vector2D ValVector)
		{
			mVector = ValVector.VectorUnitario;
			mPunto = ValPunto;
			mPuntoMira = new Punto2D(mPunto.X + mVector.X, mPunto.Y + mVector.Y);

			mA = -mVector.Y;
			mB = mVector.X;
			mC = -((mA * mPunto.X) + (mB * mPunto.Y));
			mPendiente = (mB != 0 ? -(mA / mB) : 0);
		}

		public virtual bool Pertenece(Punto2D Punto)
		{
			return (((mA * Punto.X) + (mB * Punto.Y) + mC) == 0);
		}

		public double PosicionRelativa(Punto2D Punto)
		{
			double Retorno = ((mA * Punto.X) + (mB * Punto.Y) + mC);

			if (mPendiente > 0) {
				return Retorno;
			} else {
				return -Retorno;
			}
		}

		public double SignoPosicionRelativa(Punto2D Punto)
		{
			double Retorno = Math.Sign((mA * Punto.X) + (mB * Punto.Y) + mC);

			if (mPendiente >= 0) {
				return Retorno;
			} else {
				return -Retorno;
			}
		}

		public Ecuacion ObtenerEcuacion()
		{
			return new Ecuacion(mA, mB, -mC);
		}

		public double Funcion(double X)
		{
			if (mB != 0) {
				return (-((mA * X) + mC) / mB);
			} else {
				return 0;
			}
		}

		public Punto2D ObtenerPunto(double X)
		{
			return new Punto2D(X, Funcion(X));
		}

		public PosicionRelativa2D PosicionRelativa(Recta2D Recta)
		{
			return PosicionRelativa(this, Recta);
		}

		public override string ToString()
		{
			return Strings.FormatNumber(mA, 2) + "X" + (mB >= 0 ? "+" + Strings.FormatNumber(mB, 2) : Strings.FormatNumber(mB, 2)) + "Y" + (mC >= 0 ? "+" + Strings.FormatNumber(mC, 2) : Strings.FormatNumber(mC, 2)) + "=0";
		}

		public static PosicionRelativa2D PosicionRelativa(Recta2D R1, Recta2D R2)
		{
			PosicionRelativa2D Retorno = null;
			SistemaEcuaciones Sis = null;

			Sis = new SistemaEcuaciones(R1.ObtenerEcuacion(), R2.ObtenerEcuacion());

			switch (Sis.Solucion.TipoSolucion) {
				case TipoSolucionSistema.SistemaCompatibleDeterminado:
					Retorno = new PosicionRelativa2D(new Punto2D(Sis.Solucion.ValorSolucion[0], Sis.Solucion.ValorSolucion[1]));
					break;
				case TipoSolucionSistema.SistemaCompatibleIndeterminado:
					Retorno = new PosicionRelativa2D(TipoPosicionRelativa2D.Coincidente);
					break;
				default:
					Retorno = new PosicionRelativa2D(TipoPosicionRelativa2D.Paralelo);
					break;
			}

			return Retorno;
		}

		public static Punto2D Interseccion(Recta2D R1, Recta2D R2)
		{
			PosicionRelativa2D m = PosicionRelativa(R1, R2);

			if (m.Tipo == TipoPosicionRelativa2D.Secante) {
				return m.Interseccion;
			} else {
				throw new ExcepcionGeometrica2D("RECTA2D (INTERSECCION): No se puede obtener la intersección de dos rectas que no son secantes." + Constants.vbNewLine + "Posición relativa: " + m.ToString());
			}
		}

		public static Matriz[] RepresentacionMatricial(Recta2D Recta)
		{
			Matriz[] Retorno = new Matriz[2];

			Retorno[0] = Recta.PuntoDiretor.Matriz;
			Retorno[1] = Recta.PuntoDeMira.Matriz;

			return Retorno;
		}
	}
}

