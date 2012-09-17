using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio2D;
using Motor3D.Espacio3D;
using Motor3D.Espacio3D.Transformaciones;
using Motor3D.Escena;
using Motor3D.Escena.Shading;
using System.Drawing;

namespace Motor3D.Primitivas3D
{
	public class Vertice : ObjetoPrimitiva3D
	{

		private Punto3D mCoordenadasSUR;
		private Punto3D mCoordenadasSRC;
		private Punto2D mRepresentacion;
		private Vector3D mNormalSUR;
		private Vector3D mNormalSRC;
		private Color mColor;
		private Color mColorShading;
		private int[] mCaras;

		private bool mShaded;
		public Punto3D CoodenadasSUR {
			get { return mCoordenadasSUR; }
			set {
				mCoordenadasSUR = value;
				mShaded = false;
			}
		}

		public Punto3D CoodenadasSRC {
			get { return mCoordenadasSRC; }
			set {
				mCoordenadasSRC = value;
				mShaded = false;
			}
		}

		public Punto2D Representacion {
			get { return mRepresentacion; }
			set { mRepresentacion = value; }
		}

		public Vector3D NormalSUR {
			get { return mNormalSUR; }
			set {
				mNormalSUR = value;
				mShaded = false;
			}
		}

		public Vector3D NormalSRC {
			get { return mNormalSRC; }
			set {
				mNormalSRC = value;
				mShaded = false;
			}
		}

		public Color Color {
			get { return mColor; }
			set {
				mColor = value;
				mShaded = false;
			}
		}

		public Color ColorShading {
			get { return mColor; }
			set {
				mColorShading = value;
				mShaded = true;
			}
		}

		public int[] Caras {
			get { return mCaras; }
		}

		public int Caras {
			get {
				if (Indice >= 0 && Indice <= mCaras.GetUpperBound(0)) {
					return mCaras[Indice];
				} else {
					return -1;
				}
			}
		}

		public bool Shaded {
			get { return mShaded; }
		}

		public void AplicarTransformacion(Transformacion3D Transformacion)
		{
			mCoordenadasSUR *= Transformacion;
			mShaded = false;
		}

		public void EstablecerCaras(params int[] Caras)
		{
			mCaras = Caras;
		}

		public void Shading(PhongShader Constantes, Foco3D[] Focos, Camara3D Camara)
		{
			mColorShading = Constantes.EcuacionPhong(Focos, mNormalSRC, mCoordenadasSRC, mColor, Camara);
		}

		public Vertice(Punto3D Coordenadas)
		{
			mCoordenadasSUR = Coordenadas;
			mRepresentacion = new Punto2D();
			mNormalSUR = new Vector3D();
			mColor = Color.White;
			mColorShading = Color.White;
			mShaded = false;
		}

		public static Punto3D BaricentroSUR(params Vertice[] Vertices)
		{
			double x = 0;
			double y = 0;
			double z = 0;

			x = 0;
			y = 0;
			z = 0;

			for (int i = 0; i <= Vertices.GetUpperBound(0); i++) {
				x += Vertices[i].CoodenadasSUR.X;
				y += Vertices[i].CoodenadasSUR.Y;
				z += Vertices[i].CoodenadasSUR.Z;
			}

			return new Punto3D(x / (Vertices.GetUpperBound(0) + 1), y / (Vertices.GetUpperBound(0) + 1), z / (Vertices.GetUpperBound(0) + 1));
		}

		public static Punto3D BaricentroSRC(params Vertice[] Vertices)
		{
			double x = 0;
			double y = 0;
			double z = 0;

			x = 0;
			y = 0;
			z = 0;

			for (int i = 0; i <= Vertices.GetUpperBound(0); i++) {
				x += Vertices[i].CoodenadasSRC.X;
				y += Vertices[i].CoodenadasSRC.Y;
				z += Vertices[i].CoodenadasSRC.Z;
			}

			return new Punto3D(x / (Vertices.GetUpperBound(0) + 1), y / (Vertices.GetUpperBound(0) + 1), z / (Vertices.GetUpperBound(0) + 1));
		}

		public override string ToString()
		{
			return "{Vertice de Coordenadas=" + mCoordenadasSUR.ToString() + ", Normal=" + mNormalSUR.ToString() + " y Representacion=" + mRepresentacion.ToString() + "}";
		}
	}
}

