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
	public class Caja2D : ObjetoGeometrico2D
	{

		private Punto2D Pos;

		private Punto2D Size;
		public Punto2D Posicion {
			get { return Pos; }
		}

		public Punto2D Centro {
			get { return new Punto2D(Pos.X + (Size.X / 2), Pos.Y + (Size.Y / 2)); }
			set { Pos = new Punto2D(value.X - (Size.X / 2), value.Y - (Size.Y / 2)); }
		}

		public Punto2D Dimensiones {
			get { return Size; }
			set { Size = value; }
		}

		public double Left {
			get { return Pos.X; }
		}

		public double Top {
			get { return Pos.Y; }
		}

		public double Ancho {
			get { return Size.X; }
		}

		public double Alto {
			get { return Size.Y; }
		}

		public double Right {
			get { return Size.X + Pos.X; }
		}

		public double Bottom {
			get { return Size.Y + Pos.Y; }
		}

		public Punto2D EsquinaArribaIzquierda {
			get { return Pos; }
		}

		public Punto2D EsquinaArribaDerecha {
			get { return new Punto2D(Pos.X + Size.X, Pos.Y); }
		}

		public Punto2D EsquinaAbajoIzquierda {
			get { return new Punto2D(Pos.X, Pos.Y + Size.Y); }
		}

		public Punto2D EsquinaAbajoDerecha {
			get { return new Punto2D(Pos.X + Size.X, Pos.Y + Size.Y); }
		}

		public Poligono2D Poligono {
			get { return new Poligono2D(EsquinaArribaIzquierda, EsquinaArribaDerecha, EsquinaAbajoDerecha, EsquinaAbajoIzquierda); }
		}

		public Caja2D(Punto2D Posicion, Punto2D Tamaño)
		{
			Pos = new Punto2D();
			Size = new Punto2D();

			if (Tamaño.X < 0) {
				Pos.X = Posicion.X + Tamaño.X;
				Size.X = -Tamaño.X;
			} else {
				Pos.X = Posicion.X;
				Size.X = Tamaño.X;
			}
			if (Tamaño.Y < 0) {
				Pos.Y = Posicion.Y + Tamaño.Y;
				Size.Y = -Tamaño.Y;
			} else {
				Pos.Y = Posicion.Y;
				Size.Y = Tamaño.Y;
			}
		}

		public Caja2D(double PosX, double PosY, double Width, double Height)
		{
			Pos = new Punto2D();
			Size = new Punto2D();

			if (Width < 0) {
				Pos.X = PosX + Width;
				Size.X = -Width;
			} else {
				Pos.X = PosX;
				Size.X = Width;
			}
			if (Height < 0) {
				Pos.Y = PosY + Height;
				Size.Y = -Height;
			} else {
				Pos.Y = PosY;
				Size.Y = Height;
			}
		}

		public static bool Pertenece(Caja2D Caja, Punto2D Punto)
		{
			return (Punto.X >= Caja.Left && Punto.X <= Caja.Right && Punto.Y >= Caja.Top && Punto.Y <= Caja.Bottom);
		}

		public static bool Colisionan(Caja2D C1, Caja2D C2)
		{
			Caja2D R = new Caja2D(C1.Posicion.X - C2.Ancho, C1.Posicion.Y - C2.Alto, C1.Ancho + C2.Ancho, C1.Alto + C2.Alto);

			return Pertenece(R, C2.Posicion);
		}

		public static Caja2D[] SubCajasDiagonales(Caja2D Caja, int PendienteDiagonal)
		{
			return SubCajasDiagonales(Caja, PendienteDiagonal, 1);
		}

		public static Caja2D[] SubCajasDiagonales(Caja2D Caja, int PendienteDiagonal, int Nivel)
		{
			Caja2D[] Cajas = null;

			Cajas = new Caja2D[(Math.Pow(2, Nivel))];
			if (PendienteDiagonal >= 0) {
				for (int i = 0; i <= (Math.Pow(2, Nivel)) - 1; i++) {
					Cajas[i] = new Caja2D(new Punto2D(Caja.Posicion.X + i * (Caja.Ancho / (Math.Pow(2, Nivel))), Caja.Posicion.Y + i * (Caja.Alto / (Math.Pow(2, Nivel)))), new Punto2D((Caja.Ancho / (Math.Pow(2, Nivel))), (Caja.Alto / (Math.Pow(2, Nivel)))));
				}
			} else {
				for (int i = 0; i <= (Math.Pow(2, Nivel)) - 1; i++) {
					Cajas[i] = new Caja2D(new Punto2D(Caja.Posicion.X + Caja.Ancho - i * (Caja.Ancho / (Math.Pow(2, Nivel))) - (Caja.Ancho / (Math.Pow(2, Nivel))), Caja.Posicion.Y + i * (Caja.Alto / (Math.Pow(2, Nivel)))), new Punto2D((Caja.Ancho / (Math.Pow(2, Nivel))), (Caja.Alto / (Math.Pow(2, Nivel)))));
				}
			}


			return Cajas;
		}

		public bool Pertenece(Punto2D Punto)
		{
			return Pertenece(this, Punto);
		}

		public bool Pertenece(Segmento2D Segmento)
		{
			return (Colisionan(Segmento.Caja) && Pertenece(Segmento.Recta));
		}

		public bool Pertenece(Recta2D Recta)
		{
			double Posicion = Recta.SignoPosicionRelativa(EsquinaArribaIzquierda);
			double P = 0;

			P = Recta.SignoPosicionRelativa(EsquinaArribaDerecha);
			if (P == Posicion) {
				P = Recta.SignoPosicionRelativa(EsquinaAbajoDerecha);
				if (P == Posicion) {
					P = Recta.SignoPosicionRelativa(EsquinaAbajoIzquierda);
					if (P == Posicion) {
						return false;
					} else {
						return true;
					}
				} else {
					return true;
				}
			} else {
				return true;
			}
		}

		public bool Colisionan(Caja2D Caja)
		{
			return Colisionan(this, Caja);
		}

		public override string ToString()
		{
			return "[Posición=" + Pos.ToString() + ";" + "Dimensiones=" + Size.ToString() + "]";
		}

		public static bool operator ==(Caja2D C1, Caja2D C2)
		{
			return (C1.Posicion == C2.Posicion) && (C1.Dimensiones == C2.Dimensiones);
		}

		public static bool operator !=(Caja2D C1, Caja2D C2)
		{
			return !(C1 == C2);
		}
	}
}


