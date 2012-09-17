using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;

namespace Motor3D.Espacio2D
{
	public class Punto2D : ObjetoGeometrico2D
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Punto2D Sender);

		private double mX;

		private double mY;
		public double X {
			get { return mX; }
			set {
				mX = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public double Y {
			get { return mY; }
			set {
				mY = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public bool EsCero {
			get { return (mX == 0 && mY == 0); }
		}

		public Matriz Matriz {
			get { return RepresentacionMatricial(this); }
		}

		public Punto2D()
		{
			mX = 0;
			mY = 0;
		}

		public Punto2D(double ValX, double ValY)
		{
			mX = ValX;
			mY = ValY;
		}

		public Punto2D(Matriz Matriz)
		{
			if (Matriz.Filas == 3 && Matriz.Columnas == 1) {
				mX = Matriz.Valores[0, 0];
				mY = Matriz.Valores[1, 0];
			} else {
				throw new ExcepcionGeometrica2D("PUNTO2D (NEW): La representación matricial de un punto bidimensional" + " corresponde a una matriz columna de tres elementos. " + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public System.Drawing.Point ToPoint()
		{
			return ToPoint(this);
		}

		public new string ToString()
		{
			return "{" + Strings.FormatNumber(mX, 2) + ";" + Strings.FormatNumber(mY, 2) + "}";
		}

		public static bool operator ==(Punto2D P1, Punto2D P2)
		{
			return (P1.X == P2.X && P1.Y == P2.Y);
		}

		public static bool operator !=(Punto2D P1, Punto2D P2)
		{
			return !(P1.X == P2.X && P1.Y == P2.Y);
		}

		public static bool operator >(Punto2D P1, Punto2D P2)
		{
			return P1.X > P2.X && P1.Y > P2.Y;
		}

		public static bool operator <(Punto2D P1, Punto2D P2)
		{
			return !(P1 > P2);
		}

		public static Punto2D operator +(Punto2D P1, Punto2D P2)
		{
			return new Punto2D(P1.X + P2.X, P1.Y + P2.Y);
		}

		public static Punto2D operator -(Punto2D P1, Punto2D P2)
		{
			return new Punto2D(P1.X - P2.X, P1.Y - P2.Y);
		}

		public static Punto2D operator *(Punto2D Punto, double K)
		{
			return new Punto2D(Punto.X * K, Punto.Y * K);
		}

		public static Punto2D operator *(double K, Punto2D Punto)
		{
			return new Punto2D(Punto.X * K, Punto.Y * K);
		}

		public static Punto2D operator /(Punto2D Punto, double K)
		{
			if (K != 0) {
				return new Punto2D(Punto.X / K, Punto.Y / K);
			} else {
				throw new ExcepcionGeometrica2D("PUNTO2D (OPERACION_DIVISION): Division por cero");
			}
		}

		public static double ProductoCruzado(Punto2D P1, Punto2D P2, Punto2D P3)
		{
			return ((P2.X - P1.X) * (P2.Y - P2.Y)) - ((P2.Y - P1.Y) * (P3.X - P2.X));
		}

		public static Punto2D[] Envolvente(params Punto2D[] Puntos)
		{
			return Puntos;
		}

		public static Poligono2D PoligonoEnvolvente(Punto2D[] Puntos)
		{
			return new Poligono2D(Puntos);
		}

		public static Matriz RepresentacionMatricial(Punto2D Punto)
		{
			Matriz Retorno = new Matriz(3, 1);

			Retorno.EstablecerValoresPorColumna(0, Punto.X, Punto.Y, 1);

			return Retorno;
		}

		public static System.Drawing.Point ToPoint(Punto2D Punto)
		{
			int x = 0;
			int y = 0;
			if (Punto.X >= int.MinValue) {
				if (Punto.X <= int.MaxValue) {
					x = Punto.X;
				} else {
					x = int.MaxValue - 1;
				}
			} else {
				x = int.MinValue + 1;
			}
			if (Punto.Y >= int.MinValue) {
				if (Punto.Y <= int.MaxValue) {
					y = Punto.Y;
				} else {
					y = int.MaxValue - 1;
				}
			} else {
				y = int.MinValue + 1;
			}

			return new System.Drawing.Point(x, y);
		}

		public static System.Drawing.Point[] ToPoint(Punto2D[] Puntos)
		{
			System.Drawing.Point[] Retorno = new System.Drawing.Point[Puntos.GetUpperBound(0) + 1];

			for (int i = 0; i <= Puntos.GetUpperBound(0); i++) {
				Retorno[i] = ToPoint(Puntos[i]);
			}

			return Retorno;
		}

		public static Punto2D Baricentro(Punto2D[] Puntos)
		{
			double x = 0;
			double y = 0;

			x = 0;
			y = 0;

			for (int i = 0; i <= Puntos.GetUpperBound(0); i++) {
				x += Puntos[i].X;
				y += Puntos[i].Y;
			}

			return new Punto2D(x / (Puntos.GetUpperBound(0) + 1), y / (Puntos.GetUpperBound(0) + 1));
		}
	}
}
