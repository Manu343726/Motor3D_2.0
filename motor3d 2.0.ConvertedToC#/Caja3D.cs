using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Escena;
using Motor3D.Espacio2D;

namespace Motor3D.Espacio3D
{
	public class Caja3D : ObjetoGeometrico3D
	{

		private Punto3D Pos;

		private Vector3D Size;
		public Punto3D Posicion {
			get { return Pos; }
		}

		public Punto3D Centro {
			get { return new Punto3D(Pos.X + (Size.X / 2), Pos.Y + (Size.Y / 2), Pos.Z + (Size.Z / 2)); }
		}

		public Vector3D Dimensiones {
			get { return Size; }
		}

		public Punto3D[] Vertices {
			get {
				Punto3D[] Retorno = new Punto3D[8];

				Retorno[0] = new Punto3D(Left, Top, Down);
				Retorno[1] = new Punto3D(Right, Top, Down);
				Retorno[2] = new Punto3D(Right, Bottom, Down);
				Retorno[3] = new Punto3D(Left, Bottom, Down);
				Retorno[4] = new Punto3D(Left, Top, Up);
				Retorno[5] = new Punto3D(Right, Top, Up);
				Retorno[6] = new Punto3D(Right, Bottom, Up);
				Retorno[7] = new Punto3D(Left, Bottom, Up);

				return Retorno;
			}
		}

		public Punto3D Vertices {
			get {
				switch (Indice) {
					case 0:
						return new Punto3D(Left, Top, Down);
					case 1:
						return new Punto3D(Right, Top, Down);
					case 2:
						return new Punto3D(Right, Bottom, Down);
					case 3:
						return new Punto3D(Left, Bottom, Down);
					case 4:
						return new Punto3D(Left, Top, Up);
					case 5:
						return new Punto3D(Right, Top, Up);
					case 6:
						return new Punto3D(Right, Bottom, Up);
					case 7:
						return new Punto3D(Left, Bottom, Up);
					default:
						throw new ExcepcionGeometrica3D("CAJA3D (VERTICES_GET): El índice debe estar entre 0 y 7." + Constants.vbNewLine + "Indice=" + Indice.ToString());
				}
			}
		}

		public Punto2D[] Vertices {
			get {
				Punto2D[] Retorno = new Punto2D[8];

				Retorno[0] = Camara.Proyeccion(new Punto3D(Left, Top, Down), DefinidaEnSRC);
				Retorno[1] = Camara.Proyeccion(new Punto3D(Right, Top, Down), DefinidaEnSRC);
				Retorno[2] = Camara.Proyeccion(new Punto3D(Right, Bottom, Down), DefinidaEnSRC);
				Retorno[3] = Camara.Proyeccion(new Punto3D(Left, Bottom, Down), DefinidaEnSRC);
				Retorno[4] = Camara.Proyeccion(new Punto3D(Left, Top, Up), DefinidaEnSRC);
				Retorno[5] = Camara.Proyeccion(new Punto3D(Right, Top, Up), DefinidaEnSRC);
				Retorno[6] = Camara.Proyeccion(new Punto3D(Right, Bottom, Up), DefinidaEnSRC);
				Retorno[7] = Camara.Proyeccion(new Punto3D(Left, Bottom, Up), DefinidaEnSRC);

				return Retorno;
			}
		}

		public Poligono2D[] Representacion {
			get {
				Poligono2D[] Retorno = new Poligono2D[6];
				Punto2D[] Vertices = this.Vertices[Camara, DefinidaEnSRC];

				Retorno[0] = new Poligono2D(Vertices[0], Vertices[1], Vertices[2], Vertices[3]);
				Retorno[1] = new Poligono2D(Vertices[4], Vertices[7], Vertices[6], Vertices[5]);
				Retorno[2] = new Poligono2D(Vertices[4], Vertices[5], Vertices[1], Vertices[0]);
				Retorno[3] = new Poligono2D(Vertices[7], Vertices[4], Vertices[0], Vertices[3]);
				Retorno[4] = new Poligono2D(Vertices[6], Vertices[7], Vertices[3], Vertices[2]);
				Retorno[5] = new Poligono2D(Vertices[5], Vertices[6], Vertices[2], Vertices[1]);

				return Retorno;
			}
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

		public double Largo {
			get { return Size.Y; }
		}

		public double Alto {
			get { return Size.Z; }
		}

		public double Right {
			get { return Size.X + Pos.X; }
		}

		public double Bottom {
			get { return Size.Y + Pos.Y; }
		}

		public double Up {
			get { return Pos.Z + Size.Z; }
		}

		public double Down {
			get { return Pos.Z; }
		}

		public Caja3D(Punto3D Posicion, Vector3D Tamaño)
		{
			Pos = new Punto3D();
			Size = new Vector3D();

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
			if (Tamaño.Z < 0) {
				Pos.Z = Posicion.Z + Tamaño.Z;
				Size.Z = -Tamaño.Z;
			} else {
				Pos.Z = Posicion.Z;
				Size.Z = Tamaño.Z;
			}
		}

		public Caja3D(double PosX, double PosY, double PosZ, double Ancho, double Largo, double Alto)
		{
			Pos = new Punto3D();
			Size = new Vector3D();

			if (Ancho < 0) {
				Pos.X = PosX + Ancho;
				Size.X = -Ancho;
			} else {
				Pos.X = PosX;
				Size.X = Ancho;
			}
			if (Largo < 0) {
				Pos.Y = PosY + Largo;
				Size.Y = -Largo;
			} else {
				Pos.Y = PosY;
				Size.Y = Largo;
			}
			if (Alto < 0) {
				Pos.Z = PosZ + Alto;
				Size.Z = -Alto;
			} else {
				Pos.Z = PosZ;
				Size.Z = Alto;
			}
		}

		public static bool Pertenece(Caja3D Caja, Punto3D Punto)
		{
			return (Punto.X >= Caja.Left && Punto.X <= Caja.Right && Punto.Y >= Caja.Top && Punto.Y <= Caja.Bottom && Punto.Z >= Caja.Down && Punto.Z <= Caja.Up);
		}

		public static bool Colisionan(Caja3D C1, Caja3D C2)
		{
			Caja3D R = new Caja3D(C1.Posicion.X - C2.Ancho, C1.Posicion.Y - C2.Largo, C1.Posicion.Z - C2.Alto, C1.Ancho + C2.Ancho, C1.Largo + C2.Largo, C1.Alto + C2.Alto);

			return Pertenece(R, C2.Posicion);
		}

		public static bool Pertenece(Caja3D Caja, Recta3D Recta)
		{
			double PosicionA = Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[0]);
			double PosicionB = 0;
			bool PlanoA = false;

			if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[1])) {
				if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[2])) {
					if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[3])) {
						if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[4])) {
							if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[5])) {
								if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[6])) {
									if (PosicionA == Recta.PrimerPlano.SignoPosicionRelativa(Caja.Vertices[7])) {
										return false;
									} else {
										PlanoA = true;
									}
								} else {
									PlanoA = true;
								}
							} else {
								PlanoA = true;
							}
						} else {
							PlanoA = true;
						}
					} else {
						PlanoA = true;
					}
				} else {
					PlanoA = true;
				}
			} else {
				PlanoA = true;
			}

			if (PlanoA) {
				PosicionB = Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[0]);

				if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[1])) {
					if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[2])) {
						if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[3])) {
							if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[4])) {
								if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[5])) {
									if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[6])) {
										if (PosicionB == Recta.SegundoPlano.SignoPosicionRelativa(Caja.Vertices[7])) {
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
							} else {
								return true;
							}
						} else {
							return true;
						}
					} else {
						return true;
					}
				} else {
					return true;
				}
			} else {
				return false;
			}
		}

		public static Punto3D PuntoCorteRecta(Caja3D Caja, Recta3D Recta)
		{
			if (Recta.VectorDirector.X == 0) {
				if (Recta.VectorDirector.Y == 0) {
					return Recta.ObtenerPunto(Caja.Centro.Z, EnumEjes.EjeZ);
				} else {
					return Recta.ObtenerPunto(Caja.Centro.Y, EnumEjes.EjeY);
				}
			} else {
				return Recta.ObtenerPunto(Caja.Centro.X, EnumEjes.EjeX);
			}
		}

		public bool Pertenece(Recta3D Recta)
		{
			return Pertenece(this, Recta);
		}

		public bool Pertenece(Punto3D Punto)
		{
			return Pertenece(this, Punto);
		}

		public bool Colisionan(Caja3D Caja)
		{
			return Colisionan(this, Caja);
		}

		public override string ToString()
		{
			return "[Posición=" + Pos.ToString() + ";" + "Dimensiones=" + Size.ToString() + "]";
		}

		public static bool operator ==(Caja3D C1, Caja3D C2)
		{
			return (C1.Posicion == C2.Posicion) && (C1.Dimensiones == C2.Dimensiones);
		}

		public static bool operator !=(Caja3D C1, Caja3D C2)
		{
			return !(C1 == C2);
		}
	}
}
