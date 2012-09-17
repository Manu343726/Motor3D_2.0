using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;

namespace Motor3D.Espacio3D
{
	public class Punto3D : ObjetoGeometrico3D
	{

		private double[,] mArray = new double[4, 1];

		private Matriz mMatriz;
		public double X {
			get { return mArray[0, 0]; }
			set { mArray[0, 0] = value; }
		}

		public double Y {
			get { return mArray[1, 0]; }
			set { mArray[1, 0] = value; }
		}

		public double Z {
			get { return mArray[2, 0]; }
			set { mArray[2, 0] = value; }
		}

		public Matriz Matriz {
			get { return mMatriz; }
			set {
				mMatriz = value;
				mMatriz[3, 0] = 1;
			}
		}

		public double[,] Array {
			get { return mArray; }
			set {
				mArray = value;
				mArray[3, 0] = 1;
			}
		}

		public bool EsNulo {
			get { return (mArray[0, 0] == 0 && mArray[1, 0] == 0 && mArray[2, 0] == 0); }
		}

		public Punto3D()
		{
			mArray = new double[4, 1];
			mMatriz = new Matriz(mArray);
			mArray[0, 0] = 0;
			mArray[1, 0] = 0;
			mArray[2, 0] = 0;
			mArray[3, 0] = 1;
		}

		public Punto3D(double ValX, double ValY, double ValZ)
		{
			mArray = new double[4, 1];
			mMatriz = new Matriz(mArray);
			mArray[0, 0] = ValX;
			mArray[1, 0] = ValY;
			mArray[2, 0] = ValZ;
			mArray[3, 0] = 1;
		}

		public Punto3D(Matriz Matriz)
		{
			mMatriz = Matriz;
			mArray = Matriz.Array;
			mArray[3, 0] = 1;
		}

		public static Matriz RepresentacionMatricial(Punto3D Punto)
		{
			return Punto.Matriz;
		}

		public Punto3D Copia()
		{
			return Copia(this);
		}

		public override string ToString()
		{
			return "{" + Strings.FormatNumber(mArray[0, 0], 2) + ";" + Strings.FormatNumber(mArray[1, 0], 2) + ";" + Strings.FormatNumber(mArray[2, 0], 2) + "}";
		}

		public static Punto3D Copia(Punto3D Punto)
		{
			return new Punto3D(Punto.X, Punto.Y, Punto.Z);
		}

		public static Punto3D Incremento(Punto3D Punto, double IncrementoX, double IncrementoY, double IncrementoZ)
		{
			return new Punto3D(Punto.X + IncrementoX, Punto.Y + IncrementoY, Punto.Z + IncrementoZ);
		}

		public static Punto3D Incremento(Punto3D P1, Punto3D P2)
		{
			return new Punto3D(P1.X + P2.X, P1.Y + P2.Y, P1.Z + P2.Z);
		}

		public static Punto3D operator +(Punto3D P1, Punto3D P2)
		{
			return Incremento(P1, P2);
		}

		public static Punto3D operator -(Punto3D P1, Punto3D P2)
		{
			return Incremento(P1, -P2.X, -P2.Y, -P2.Z);
		}

		public static Punto3D operator *(Punto3D Punto, double K)
		{
			return new Punto3D(Punto.X * K, Punto.Y * K, Punto.Z * K);
		}

		public static Punto3D operator *(double K, Punto3D Punto)
		{
			return new Punto3D(Punto.X * K, Punto.Y * K, Punto.Z * K);
		}

		public static Punto3D operator /(Punto3D Punto, double K)
		{
			if (K != 0) {
				return new Punto3D(Punto.X / K, Punto.Y / K, Punto.Z / K);
			} else {
				throw new ExcepcionGeometrica3D("PUNTO3D (OPERADOR_DIVISION): División por cero");
			}
		}

		public static bool operator ==(Punto3D P1, Punto3D P2)
		{
			return (P1.X == P2.X && P1.Y == P2.Y && P1.Z == P2.Z);
		}

		public static bool operator !=(Punto3D P1, Punto3D P2)
		{
			return !(P1.X == P2.X && P1.Y == P2.Y && P1.Z == P2.Z);
		}

		public static bool operator >(Punto3D P1, Punto3D P2)
		{
			return P1.X > P2.X || P1.Y > P2.Y || P1.Z > P2.Z;
		}

		public static bool operator <(Punto3D P1, Punto3D P2)
		{
			return !(P1 > P2);
		}

		public static Punto3D Baricentro(params Punto3D[] Puntos)
		{
			double x = 0;
			double y = 0;
			double z = 0;

			x = 0;
			y = 0;
			z = 0;

			for (int i = 0; i <= Puntos.GetUpperBound(0); i++) {
				x += Puntos[i].X;
				y += Puntos[i].Y;
				z += Puntos[i].Z;
			}

			return new Punto3D(x / (Puntos.GetUpperBound(0) + 1), y / (Puntos.GetUpperBound(0) + 1), z / (Puntos.GetUpperBound(0) + 1));
		}
	}
}

