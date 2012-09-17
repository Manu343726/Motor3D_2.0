using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using Motor3D.Espacio2D;
using Motor3D.Escena;
using Motor3D.Escena.Shading;
using System.Drawing;
using System.Math;

namespace Motor3D.Primitivas3D
{
	public class Cara : ObjetoPrimitiva3D
	{

		private Vector3D mNormalSUR;
		private Vector3D mNormalSRC;
		private int[] mVertices;
		private Punto3D mBaricentroSUR;
		private Punto3D mBaricentroSRC;
		private Color mColor;
		private Color mColorShading;

		private bool mCargadaEnBuffer;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Cara Sebder);

		public bool CargadaEnBuffer {
			get { return mCargadaEnBuffer; }
			set { mCargadaEnBuffer = value; }
		}

		public Vector3D NormalSUR {
			get { return mNormalSUR; }
			set {
				mNormalSUR = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Vector3D NormalSRC {
			get { return mNormalSRC; }
			set {
				mNormalSRC = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Punto3D BaricentroSUR {
			get { return mBaricentroSUR; }
			set {
				mBaricentroSUR = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Punto3D BaricentroSRC {
			get { return mBaricentroSRC; }
			set {
				mBaricentroSRC = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public int[] Vertices {
			get { return mVertices; }
			set {
				if (value.GetUpperBound(0) == mVertices.GetUpperBound(0)) {
					mVertices = value;
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public int Vertices {
			get {
				if (Indice >= 0 && Indice <= mVertices.GetUpperBound(0)) {
					return mVertices[Indice];
				} else {
					return -1;
				}
			}
			set {
				if (Indice >= 0 && Indice <= mVertices.GetUpperBound(0)) {
					mVertices[Indice] = value;
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public Vertice[] Vertices {
			get {
				Vertice[] Retorno = new Vertice[mVertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					Retorno[i] = ArrayVertices[mVertices[i]];
				}

				return Retorno;
			}
		}

		public Punto3D[] PuntosSUR {
			get {
				Punto3D[] Retorno = new Punto3D[mVertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					Retorno[i] = Vertices[mVertices[i]].CoodenadasSUR;
				}

				return Retorno;
			}
		}

		public Punto3D[] PuntosSRC {
			get {
				Punto3D[] Retorno = new Punto3D[mVertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					Retorno[i] = Vertices[mVertices[i]].CoodenadasSRC;
				}

				return Retorno;
			}
		}

		public Punto2D[] Representacion {
			get {
				Punto2D[] Retorno = new Punto2D[mVertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					Retorno[i] = Vertices[mVertices[i]].Representacion;
				}

				return Retorno;
			}
		}

		public Point[] RepresentacionToPoint {
			get {
				Point[] Retorno = new Point[mVertices.GetUpperBound(0) + 1];

				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					Retorno[i] = Vertices[mVertices[i]].Representacion.ToPoint();
				}

				return Retorno;
			}
		}

		public int NumeroVertices {
			get { return mVertices.GetUpperBound(0) + 1; }
		}

		public Color Color {
			get { return mColor; }
			set {
				mColor = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public Color ColorShading {
			get { return mColorShading; }
			set { mColorShading = value; }
		}

		public Cara(params int[] Vertices)
		{
			if (Vertices.GetUpperBound(0) >= 2) {
				mVertices = Vertices;
				mNormalSUR = new Vector3D();
				mBaricentroSUR = new Punto3D();
				mColor = Color.White;
			} else {
				if (Vertices.GetUpperBound(0) == 0) {
					mVertices = new int[Vertices[0]];
					mNormalSUR = new Vector3D();
					mBaricentroSUR = new Punto3D();
					mColor = Color.White;
				} else {
					throw new ExcepcionPrimitiva3D("CARA (NEW): Una cara debe tener al menos tres vértices." + Constants.vbNewLine + "Número de vértices: " + Vertices.GetUpperBound(0) + 1);
				}

			}
		}

		public void RecalcularNormalSUR(Vertice[] Vertices)
		{
			mNormalSUR = Cara.VectorNormalSUR(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RecalcularBaricentroSUR(Vertice[] Vertices)
		{
			mBaricentroSUR = Cara.BaricentroCaraSUR(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RecalcularDatosSUR(Vertice[] Vertices)
		{
			mNormalSUR = Cara.VectorNormalSUR(this, Vertices);
			mBaricentroSUR = Cara.BaricentroCaraSUR(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RecalcularNormalSRC(Vertice[] Vertices)
		{
			mNormalSRC = Cara.VectorNormalSRC(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RecalcularBaricentroSRC(Vertice[] Vertices)
		{
			mBaricentroSRC = Cara.BaricentroCaraSRC(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RecalcularDatosSRC(Vertice[] Vertices)
		{
			mNormalSRC = Cara.VectorNormalSRC(this, Vertices);
			mBaricentroSRC = Cara.BaricentroCaraSRC(this, Vertices);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void Shading(PhongShader Constantes, Foco3D[] Focos, Camara3D Camara)
		{
			mColorShading = Constantes.EcuacionPhong(Focos, mNormalSUR, mBaricentroSUR, mColor, Camara);
		}

		public void RevertirVertices()
		{
			Array.Reverse(mVertices);
		}

		public bool EsVisible(Camara3D Camara, Vertice[] Vertices)
		{
			if (mNormalSRC.Z <= 0.01 && Camara.Frustum.Pertenece(mBaricentroSRC)) {
				for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
					if (Camara.Pantalla.Pertenece(Vertices[mVertices[i]].Representacion)) {
						return true;
					}
				}

				return false;
			} else {
				return false;
			}
		}

		public static Vector3D VectorNormalSUR(Cara Cara, Vertice[] Vertices)
		{
			return (new Vector3D(Vertices[Cara.Vertices[0]].CoodenadasSUR, Vertices[Cara.Vertices[1]].CoodenadasSUR) + new Vector3D(Vertices[Cara.Vertices[0]].CoodenadasSUR, Vertices[Cara.Vertices[2]].CoodenadasSUR)).VectorUnitario;
		}

		public static Vector3D VectorNormalSRC(Cara Cara, Vertice[] Vertices)
		{
			return (new Vector3D(Vertices[Cara.Vertices[0]].CoodenadasSRC, Vertices[Cara.Vertices[1]].CoodenadasSRC) + new Vector3D(Vertices[Cara.Vertices[0]].CoodenadasSRC, Vertices[Cara.Vertices[2]].CoodenadasSRC)).VectorUnitario;
		}

		public static Plano3D PlanoSUR(Cara Cara, Vertice[] Vertices)
		{
			return new Plano3D(Vertices[Cara.Vertices[0]].CoodenadasSUR, Vertices[Cara.Vertices[1]].CoodenadasSUR, Vertices[Cara.Vertices[2]].CoodenadasSUR);
		}

		public static Plano3D PlanoSRC(Cara Cara, Vertice[] Vertices)
		{
			return new Plano3D(Vertices[Cara.Vertices[0]].CoodenadasSRC, Vertices[Cara.Vertices[1]].CoodenadasSRC, Vertices[Cara.Vertices[2]].CoodenadasSRC);
		}

		public static Punto3D BaricentroCaraSUR(Cara Cara, Vertice[] Vertices)
		{
			return Punto3D.Baricentro(Cara.PuntosSUR[Vertices]);
		}

		public static Punto3D BaricentroCaraSRC(Cara Cara, Vertice[] Vertices)
		{
			return Punto3D.Baricentro(Cara.PuntosSRC[Vertices]);
		}

		public override string ToString()
		{
			string Retorno = "";

			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				if (i < mVertices.GetUpperBound(0)) {
					Retorno += mVertices[i].ToString() + ",";
				} else {
					Retorno += mVertices[i].ToString() + ",Baricentro=" + mBaricentroSUR.ToString() + ",Normal=" + mNormalSUR.ToString() + "}";
				}

			}

			return Retorno;
		}
	}
}

