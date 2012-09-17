using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using System.Drawing;

namespace Motor3D.Escena
{
	public class Foco3D : ObjetoEscena
	{

		private Punto3D mCoordenadasSUR;
		private Color mColor;

		private float mIntensidad;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Foco3D Sender);

		public Punto3D Coordenadas {
			get { return mCoordenadasSUR; }
			set {
				mCoordenadasSUR = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Color Color {
			get { return mColor; }
			set {
				if (value != System.Drawing.Color.Black && value.A == 255) {
					mColor = value;
					if (Modificado != null) {
						Modificado(this);
					}
				} else {
					throw new ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." + Constants.vbNewLine + "Color=ARGB(" + value.A + "," + value.R + "," + value.G + "," + value.B + ")");
				}
			}
		}

		public float Intensidad {
			get { return mIntensidad; }
			set {
				if (value >= 0 && value <= 1) {
					mIntensidad = value;
				} else {
					throw new ExcepcionEscena("FOCO3D (INTENSIDAD_SET): La intensidad debe estar entre 0 y 1." + Constants.vbNewLine + "Intensidad=" + value);
				}
			}
		}

		public Foco3D(Punto3D Posicion, Color Color)
		{
			if (Color != System.Drawing.Color.Black && Color.A == 255) {
				mCoordenadasSUR = Posicion;
				mColor = Color;
				mIntensidad = 1;
			} else {
				throw new ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." + Constants.vbNewLine + "Color=ARGB(" + Color.A + "," + Color.R + "," + Color.G + "," + Color.B + ")");
			}
		}

		public Foco3D(Punto3D Posicion, Color Color, float Intensidad)
		{
			if (Intensidad >= 0 && Intensidad <= 1) {
				if (Color != System.Drawing.Color.Black && Color.A == 255) {
					mCoordenadasSUR = Posicion;
					mColor = Color;
					mIntensidad = Intensidad;
				} else {
					throw new ExcepcionEscena("FOCO3D (COLOR_SET): No se puede asignar un color negro o semitransparente." + Constants.vbNewLine + "Color=ARGB(" + Color.A + "," + Color.R + "," + Color.G + "," + Color.B + ")");
				}
			}
		}

		public override string ToString()
		{
			return "{Foco. Posicion=" + mCoordenadasSUR.ToString() + " , Color=ARGB(" + mColor.A + "," + mColor.R + "," + mColor.G + "," + mColor.B + ") , Intensidad=" + mIntensidad.ToString() + "}";
		}

		public static bool operator ==(Foco3D F1, Foco3D F2)
		{
			return F1.Equals(F2);
		}

		public static bool operator !=(Foco3D F1, Foco3D F2)
		{
			return !(F1 == F2);
		}
	}
}

