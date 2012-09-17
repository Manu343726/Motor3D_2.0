using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Drawing;
using System.Math;

namespace Motor3D.Espacio2D
{
	public class Poligono2D : ObjetoGeometrico2D
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Poligono2D Sender);

		protected Segmento2D[] Segmentos;
		protected Color mColor;

		protected Caja2D mCaja;
		public Segmento2D[] Lados {
			get { return Segmentos; }
		}

		public int NumeroLados {
			get { return Segmentos.GetUpperBound(0) + 1; }
		}

		public Punto2D[] Vertices {
			get {
				Punto2D[] Retorno = new Punto2D[Segmentos.GetUpperBound(0) + 1];

				for (int i = 0; i <= Segmentos.GetUpperBound(0); i++) {
					Retorno[i] = Segmentos[i].ExtremoInicial;
				}

				return Retorno;
			}
		}

		public Caja2D Caja {
			get {
				if ((mCaja != null)) {
					return mCaja;
				} else {
					mCaja = ObtenerCaja();
					return mCaja;
				}
			}
		}

		public Point[] VerticesToPoint {
			get {
				Point[] Retorno = new Point[Segmentos.GetUpperBound(0) + 1];

				for (int i = 0; i <= Segmentos.GetUpperBound(0); i++) {
					Retorno[i] = Segmentos[i].ExtremoInicial.ToPoint();
					if (YInvertida)
						Retorno[i].Y *= -1;
				}

				return Retorno;
			}
		}

		public Color Color {
			get { return mColor; }
			set { mColor = value; }
		}

		public Caja2D ObtenerCaja()
		{
			double maxx = 0;
			double maxy = 0;
			double minx = 0;
			double miny = 0;

			maxx = Segmentos[0].ExtremoInicial.X;
			maxy = Segmentos[0].ExtremoInicial.Y;
			minx = Segmentos[0].ExtremoInicial.X;
			miny = Segmentos[0].ExtremoInicial.Y;

			for (int i = 1; i <= Segmentos.GetUpperBound(0); i++) {
				if (maxx < Segmentos[i].ExtremoInicial.X)
					maxx = Segmentos[i].ExtremoInicial.X;
				if (maxy < Segmentos[i].ExtremoInicial.Y)
					maxy = Segmentos[i].ExtremoInicial.Y;
				if (minx > Segmentos[i].ExtremoInicial.X)
					minx = Segmentos[i].ExtremoInicial.X;
				if (miny > Segmentos[i].ExtremoInicial.Y)
					miny = Segmentos[i].ExtremoInicial.Y;
			}

			return new Caja2D(minx, miny, Math.Abs(maxx - minx), Math.Abs(maxy - miny));
		}

		public Poligono2D(params Punto2D[] Vertices)
		{
			if (Vertices.GetUpperBound(0) > 1) {
				Segmentos = new Segmento2D[Vertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= Vertices.GetUpperBound(0); i++) {
					if (i < Vertices.GetUpperBound(0)) {
						Segmentos[i] = new Segmento2D(Vertices[i], Vertices[i + 1]);
					} else {
						Segmentos[i] = new Segmento2D(Vertices[i], Vertices[0]);
					}
				}

				mColor = System.Drawing.Color.White;
			}
		}

		public virtual void EstablecerVertices(params Punto2D[] Vertices)
		{
			if (Vertices.GetUpperBound(0) > 1) {
				Segmentos = new Segmento2D[Vertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= Vertices.GetUpperBound(0); i++) {
					if (i < Vertices.GetUpperBound(0)) {
						Segmentos[i] = new Segmento2D(Vertices[i], Vertices[i + 1]);
					} else {
						Segmentos[i] = new Segmento2D(Vertices[i], Vertices[0]);
					}
				}
				mCaja = ObtenerCaja();

				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Poligono2D(params Segmento2D[] Lados)
		{
			if (Lados.GetUpperBound(0) > 1) {
				Segmentos = Lados;
				mColor = Color.Red;
			}
		}

		public override string ToString()
		{
			return "{Poligono bidimensional de " + Segmentos.GetUpperBound(0) + 1 + " lados}";
		}
	}
}

