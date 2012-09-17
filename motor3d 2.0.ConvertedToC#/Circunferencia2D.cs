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
	public class Circunferencia2D : ObjetoGeometrico2D
	{

		private double mRadio;
		private double CuadradoRadio;

		private Punto2D mCentro;
		private double mA;
		private double mB;

		private double mC;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Circunferencia2D Sebder);

		public double Radio {
			get { return mRadio; }

			set {
				if (value != mRadio) {
					mRadio = value;
					CuadradoRadio = Math.Pow(mRadio, 2);
					mC = (Math.Pow(mCentro.X, 2)) + (Math.Pow(mCentro.Y, 2)) + (Math.Pow(mRadio, 2));
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public Punto2D Centro {
			get { return mCentro; }

			set {
				if (value != mCentro) {
					mCentro = value;
					CalculoCoeficientes();
					if (Modificado != null) {
						Modificado(this);
					}
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

		public Circunferencia2D(Punto2D ValCentro, double ValRadio)
		{
			mRadio = ValRadio;
			mCentro = ValCentro;
			CalculoCoeficientes();
		}

		public Circunferencia2D(Punto2D P1, Punto2D P2, Punto2D P3)
		{
			mCentro = new Punto2D((P1.X + P2.X + P3.X) / 3, (P1.Y + P2.Y + P3.Y) / 3);
			CuadradoRadio = (Math.Pow((P1.X - mCentro.X), 2)) + (Math.Pow((P1.Y - mCentro.Y), 2));
			mRadio = Math.Sqrt(CuadradoRadio);
			CalculoCoeficientes();
		}

		public Circunferencia2D(Caja2D Caja)
		{
			mCentro = Caja.Centro;
			mRadio = (Caja.Ancho >= Caja.Alto ? Caja.Ancho / 2 : Caja.Alto / 2);
			CuadradoRadio = Math.Pow(mRadio, 2);
			CalculoCoeficientes();
		}

		private void CalculoCoeficientes()
		{
			mA = -2 * mCentro.X;
			mB = -2 * mCentro.Y;
			mC = (Math.Pow(mCentro.X, 2)) + (Math.Pow(mCentro.Y, 2)) - (Math.Pow(mRadio, 2));
			CuadradoRadio = Math.Pow(mRadio, 2);
		}

		public bool Pertenece(Punto2D Punto)
		{
			return (Math.Pow(Punto.X, 2)) + (Math.Pow(Punto.Y, 2)) + (mA * Punto.X) + (mB * Punto.Y) + mC == 0;
		}

		public bool ContenidoEnCirculo(Punto2D Punto)
		{
			return (Math.Pow(Punto.X, 2)) + (Math.Pow(Punto.Y, 2)) + (mA * Punto.X) + (mB * Punto.Y) + mC < 0;
		}

		public double PosicionRelativa(Punto2D Punto)
		{
			return (Math.Pow(Punto.X, 2)) + (Math.Pow(Punto.Y, 2)) + (mA * Punto.X) + (mB * Punto.Y) + mC;
		}

		public static double PosicionRelativa(Circunferencia2D C1, Circunferencia2D C2)
		{
			return Math.Sqrt((Math.Pow((C2.Centro.X - C1.Centro.X), 2)) + (Math.Pow((C2.Centro.Y - C1.Centro.Y), 2))) - (C1.Radio + C2.Radio);
		}

		public static Recta2D EjeRadical(Circunferencia2D C1, Circunferencia2D C2)
		{
			return new Recta2D(C1.A - C2.A, C1.B - C2.B, C1.C - C2.C);
		}

		public override string ToString()
		{
			return "[Centro=" + mCentro.ToString() + "," + "Radio=" + mRadio.ToString() + "]";
		}

		public string ToString(bool Algebraica)
		{
			if (Algebraica) {
				return "X^2+Y^2" + (mA > 0 ? "+" : "") + Strings.FormatNumber(mA, 2) + "X" + (mB >= 0 ? "+" + Strings.FormatNumber(mB, 2) : Strings.FormatNumber(mB, 2)) + "Y" + (mC >= 0 ? "+" + Strings.FormatNumber(mC, 2) : Strings.FormatNumber(mC, 2)) + "=0";
			} else {
				return "[Centro=" + mCentro.ToString() + "," + "Radio=" + mRadio.ToString() + "]";
			}
		}

		public static bool Colisionan(Circunferencia2D C1, Circunferencia2D C2)
		{
			return (PosicionRelativa(C1, C2) <= 0);
		}
	}
}

