using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Escena;
using Motor3D.Escena.Shading;
using Motor3D.Espacio3D;
using Motor3D.Espacio3D.Transformaciones;
using Motor3D.Espacio2D;
using System.Drawing;
using System.Math;

namespace Motor3D.Primitivas3D
{
	public class Poliedro : ObjetoPrimitiva3D
	{

		protected Vertice[] mVertices;
		protected Cara[] mCaras;
		protected Punto3D mCentroSUR;
		protected Punto3D mCentroSRC;
		protected Vector3D mVertical;
		protected bool NormalesCentro;
		protected bool AutoReclcNorms;
		protected Caja3D mCajaSUR;
		protected Caja3D mCajaSRC;
		protected bool mAutoRecalcularCajas;
		protected PhongShader mConstantesShading;

		protected bool mShaded;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Poliedro Sender);
		public event TransformacionCompletadaEventHandler TransformacionCompletada;
		public delegate void TransformacionCompletadaEventHandler(ref Poliedro Sender);

		public Vertice[] Vertices {
			get { return mVertices; }
		}

		public Vertice Vertices {
			get { return mVertices[Indice]; }
		}

		public Cara[] Caras {
			get { return mCaras; }
		}

		public Cara Caras {
			get { return mCaras[Indice]; }
		}

		public Color ColorCara {
			get { return mCaras[Indice].Color; }
			set { mCaras[Indice].Color = value; }
		}

		public bool CaraVisible {
			get { return mCaras[Indice].EsVisible(Camara, mVertices); }
		}

		public bool AutoRecalcularCajas {
			get { return mAutoRecalcularCajas; }
			set { mAutoRecalcularCajas = value; }
		}

		public Caja3D CajaSUR {
			get { return mCajaSUR; }
		}

		public Caja3D CajaSRC {
			get { return mCajaSRC; }
		}

		public Punto3D CentroSUR {
			get { return mCentroSUR; }
		}

		public Punto3D CentroSRC {
			get { return mCentroSRC; }
		}

		public Vector3D Vertical {
			get { return mVertical; }

			set {
				if (value != mVertical) {
					AplicarTransformacion(new Rotacion(mVertical, value));
					mVertical = value;
				}
			}
		}

		public int NumeroVertices {
			get { return mVertices.GetUpperBound(0) + 1; }
		}

		public int NumeroCaras {
			get { return mCaras.GetUpperBound(0) + 1; }
		}

		public Poligono2D[] Representacion {
			get {
				Poligono2D[] Retorno = new Poligono2D[mCaras.GetUpperBound(0) + 1];
				Punto2D[] Repr = null;

				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					Repr = new Punto2D[mCaras[i].Vertices.GetUpperBound(0) + 1];
					for (int j = 0; j <= mCaras[i].Vertices.GetUpperBound(0); j++) {
						Repr[j] = mVertices[mCaras[i].Vertices[j]].Representacion;
					}
					Retorno[i] = new Poligono2D(Repr);
					Retorno[i].Color = mCaras[i].Color;
				}

				return Retorno;
			}
		}

		public Poligono2D Representacion {
			get {
				Poligono2D Retorno = null;
				Punto2D[] Repr = new Punto2D[mCaras[IndiceCara].NumeroVertices];

				for (int i = 0; i <= mCaras[IndiceCara].Vertices.GetUpperBound(0); i++) {
					Repr[i] = mVertices[mCaras[IndiceCara].Vertices[i]].Representacion;
				}
				Retorno = new Poligono2D(Repr);
				Retorno.Color = mCaras[IndiceCara].Color;

				return Retorno;
			}
		}

		public bool Shaded {
			get { return mShaded; }
			set { mShaded = value; }
		}

		public bool NormalesDesdeCentro {
			get { return NormalesCentro; }
			set {
				if (value != NormalesCentro) {
					NormalesCentro = value;
					RecalcularDatosCaras();
					if (AutoReclcNorms)
						RecalcularNormalesVertices();
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public bool AutoRecalcularNormalesVertices {
			get { return AutoReclcNorms; }
			set {
				if (AutoReclcNorms != value) {
					AutoReclcNorms = value;
				}
			}
		}

		public PhongShader ConstantesShading {
			get { return mConstantesShading; }
		}

		public void EstablecerColor(Color Color)
		{
			for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
				mCaras[i].Color = Color;
			}
		}

		public void EstablecerColor(int Cara, Color Color)
		{
			mCaras[Cara].Color = Color;
		}

		public void EstablecerConstantesShading(ref PhongShader Constantes)
		{
			mConstantesShading = Constantes;
		}

		public void RecalcularNormalesVertices()
		{
			Vector3D Normal = new Vector3D();

			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				Normal = new Vector3D();
				for (int j = 0; j <= mVertices[i].Caras.GetUpperBound(0); j++) {
					Normal += mCaras[mVertices[i].Caras[j]].NormalSUR;
				}
				mVertices[i].NormalSUR = Normal / (mVertices[i].Caras.GetUpperBound(0) + 1);
			}
		}

		public void RecalcularNormalesCarasVertice(int IndiceVertice)
		{
			if (IndiceVertice >= 0 && IndiceVertice <= mVertices.GetUpperBound(0)) {
				Vector3D Normal = new Vector3D();

				for (int i = 0; i <= mVertices[i].Caras.GetUpperBound(0); i++) {
					Normal += mCaras[mVertices[i].Caras[i]].NormalSUR;
				}
				mVertices[IndiceVertice].NormalSUR = Normal / (mVertices[IndiceVertice].Caras.GetUpperBound(0) + 1);
			}
		}

		public void AplicarTransformacion(Transformacion3D Transformacion)
		{
			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				mVertices[i].CoodenadasSUR = Transformacion * mVertices[i].CoodenadasSUR;
			}

			mVertical *= Transformacion;
			CalculoTransformacion();
		}

		public void AplicarTransformacion(Traslacion Traslacion)
		{
			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				mVertices[i].CoodenadasSUR = Traslacion * mVertices[i].CoodenadasSUR;
			}

			mVertical *= Traslacion;
			CalculoTransformacion();
		}

		public void AplicarTransformacion(Escalado Escalado)
		{
			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				mVertices[i].CoodenadasSUR = Escalado * mVertices[i].CoodenadasSUR;
			}

			mVertical *= Escalado;
			CalculoTransformacion();
		}

		public void AplicarTransformacion(Rotacion Rotacion)
		{
			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				mVertices[i].CoodenadasSUR = Rotacion * mVertices[i].CoodenadasSUR;
			}

			mVertical *= Rotacion;
			CalculoTransformacion();
		}

		private void CalculoTransformacion()
		{
			mVertical = mVertical.VectorUnitario;
			RecalcularCentro();
			RecalcularDatosCaras();
			if (mAutoRecalcularCajas)
				mCajaSUR = ObtenerCajaSUR();
			if (AutoReclcNorms)
				RecalcularNormalesVertices();

			if (Modificado != null) {
				Modificado(this);
			}
			if (TransformacionCompletada != null) {
				TransformacionCompletada(this);
			}
		}

		public Caja3D ObtenerCajaSUR()
		{
			double maxx = 0;
			double maxy = 0;
			double maxz = 0;
			double minx = 0;
			double miny = 0;
			double minz = 0;

			maxx = mVertices[0].CoodenadasSUR.X;
			maxy = mVertices[0].CoodenadasSUR.Y;
			maxz = mVertices[0].CoodenadasSUR.Z;
			minx = mVertices[0].CoodenadasSUR.X;
			miny = mVertices[0].CoodenadasSUR.Y;
			minz = mVertices[0].CoodenadasSUR.Z;

			for (int i = 1; i <= mVertices.GetUpperBound(0); i++) {
				if (mVertices[i].CoodenadasSUR.X > maxx)
					maxx = mVertices[i].CoodenadasSUR.X;
				if (mVertices[i].CoodenadasSUR.Y > maxy)
					maxy = mVertices[i].CoodenadasSUR.Y;
				if (mVertices[i].CoodenadasSUR.Z > maxz)
					maxz = mVertices[i].CoodenadasSUR.Z;
				if (mVertices[i].CoodenadasSUR.X < minx)
					minx = mVertices[i].CoodenadasSUR.X;
				if (mVertices[i].CoodenadasSUR.Y < miny)
					miny = mVertices[i].CoodenadasSUR.Y;
				if (mVertices[i].CoodenadasSUR.Z < minz)
					minz = mVertices[i].CoodenadasSUR.Z;
			}

			return new Caja3D(minx, miny, minz, Math.Abs(maxx - minx), Math.Abs(maxy - miny), Math.Abs(maxz - minz));
		}

		public Caja3D ObtenerCajaSRC()
		{
			double maxx = 0;
			double maxy = 0;
			double maxz = 0;
			double minx = 0;
			double miny = 0;
			double minz = 0;

			maxx = mVertices[0].CoodenadasSRC.X;
			maxy = mVertices[0].CoodenadasSRC.Y;
			maxz = mVertices[0].CoodenadasSRC.Z;
			minx = mVertices[0].CoodenadasSRC.X;
			miny = mVertices[0].CoodenadasSRC.Y;
			minz = mVertices[0].CoodenadasSRC.Z;

			for (int i = 1; i <= mVertices.GetUpperBound(0); i++) {
				if (mVertices[i].CoodenadasSRC.X > maxx)
					maxx = mVertices[i].CoodenadasSRC.X;
				if (mVertices[i].CoodenadasSRC.Y > maxy)
					maxy = mVertices[i].CoodenadasSRC.Y;
				if (mVertices[i].CoodenadasSRC.Z > maxz)
					maxz = mVertices[i].CoodenadasSRC.Z;
				if (mVertices[i].CoodenadasSRC.X < minx)
					minx = mVertices[i].CoodenadasSRC.X;
				if (mVertices[i].CoodenadasSRC.Y < miny)
					miny = mVertices[i].CoodenadasSRC.Y;
				if (mVertices[i].CoodenadasSRC.Z < minz)
					minz = mVertices[i].CoodenadasSRC.Z;
			}

			return new Caja3D(minx, miny, minz, Math.Abs(maxx - minx), Math.Abs(maxy - miny), Math.Abs(maxz - minz));
		}

		public void RecalcularCajaSUR()
		{
			mCajaSUR = ObtenerCajaSUR();
		}

		public void Shading(Foco3D[] Focos, Camara3D Camara)
		{
			if ((Focos != null)) {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].Shading(mConstantesShading, Focos, Camara);
				}
			} else {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].ColorShading = Color.Black;
				}
			}
		}

		public void RecalcularCajaSRC()
		{
			mCajaSRC = ObtenerCajaSRC();
		}

		public void RecalcularCentro()
		{
			mCentroSUR = Vertice.BaricentroSUR(mVertices);
		}

		private void RecalcularDatosCaras()
		{
			if (!NormalesCentro) {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].NormalSUR = Cara.VectorNormalSUR(mCaras[i], mVertices);
				}
			} else {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].NormalSUR = new Vector3D(mCentroSUR, mCaras[i].BaricentroSUR).VectorUnitario;
				}
			}

			for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
				mCaras[i].RecalcularBaricentroSUR(mVertices);
			}
		}

		public void RecalcularRepresentaciones(Camara3D Camara)
		{
			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				mVertices[i].CoodenadasSRC = Camara.TransformacionSURtoSRC * mVertices[i].CoodenadasSUR;
				mVertices[i].Representacion = Camara.Proyeccion(mVertices[i].CoodenadasSRC, true);
			}

			if (NormalesCentro) {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].RecalcularBaricentroSRC(mVertices);
					mCaras[i].NormalSRC = new Vector3D(mCentroSRC, mCaras[i].BaricentroSRC).VectorUnitario;
				}
			} else {
				for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
					mCaras[i].RecalcularBaricentroSRC(mVertices);
					mCaras[i].NormalSRC = Cara.VectorNormalSRC(mCaras[i], mVertices);
				}
			}

			mCentroSRC = Camara.TransformacionSURtoSRC * mCentroSUR;

			if (mAutoRecalcularCajas)
				mCajaSRC = ObtenerCajaSRC();
		}

		public void CalcularCarasVertices()
		{
			List<int> Caras = new List<int>();

			for (int i = 0; i <= mVertices.GetUpperBound(0); i++) {
				Caras.Clear();
				for (int j = 0; j <= mCaras.GetUpperBound(0); j++) {
					if (mCaras[j].Vertices.Contains(i))
						Caras.Add(j);
				}
				mVertices[i].EstablecerCaras(Caras.ToArray());
			}
		}

		public bool EsVisible(Camara3D Camara)
		{
			if (!Camara.EsVisible(mCentroSUR)) {
				foreach (Vertice Vertice in mVertices) {
					if (!Camara.EsVisible(Vertice.CoodenadasSUR)) {
						return false;
					}
				}

				return true;
			} else {
				return false;
			}
		}

		public Poliedro(Vertice[] Vertices, Cara[] Caras)
		{
			if (Vertices.GetUpperBound(0) >= 3) {
				if (Caras.GetUpperBound(0) >= 3) {
					mCaras = Caras;

					for (int i = 0; i <= mCaras.GetUpperBound(0); i++) {
						mCaras[i].Color = Color.White;
					}

					mVertices = Vertices;
					mAutoRecalcularCajas = false;
					AutoReclcNorms = true;
					RecalcularCentro();
					CalcularCarasVertices();
					RecalcularDatosCaras();
					RecalcularNormalesVertices();
					mConstantesShading = new PhongShader();
					mVertical = new Vector3D(0, 1, 0);
				} else {
					throw new ExcepcionPrimitiva3D("POLIEDRO (NEW): Un poliedro debe tener al menos 4 caras" + Constants.vbNewLine + "Numero de caras=" + Vertices.GetUpperBound(0) + 1);
				}
			} else {
				throw new ExcepcionPrimitiva3D("POLIEDRO (NEW): Un poliedro debe tener al menos 4 vertices" + Constants.vbNewLine + "Numero de vertices=" + Vertices.GetUpperBound(0) + 1);
			}
		}

		//SOLO PARA LAS CLASES HEREDADAS!!!
		public Poliedro()
		{
		}

		public override string ToString()
		{
			return "{Poliedro de " + mCaras.GetUpperBound(0) + 1 + " y " + mVertices.GetUpperBound(0) + 1 + " vertices}";
		}

		public static Poliedro Cubo()
		{
			Vertice[] Vertices = new Vertice[8];
			Cara[] Caras = new Cara[6];

			Vertices[0] = new Vertice(new Punto3D(-1, -1, -1));
			Vertices[1] = new Vertice(new Punto3D(1, -1, -1));
			Vertices[2] = new Vertice(new Punto3D(1, 1, -1));
			Vertices[3] = new Vertice(new Punto3D(-1, 1, -1));
			Vertices[4] = new Vertice(new Punto3D(-1, -1, 1));
			Vertices[5] = new Vertice(new Punto3D(1, -1, 1));
			Vertices[6] = new Vertice(new Punto3D(1, 1, 1));
			Vertices[7] = new Vertice(new Punto3D(-1, 1, 1));

			Caras[0] = new Cara(3, 2, 1, 0);
			Caras[1] = new Cara(4, 5, 6, 7);
			Caras[2] = new Cara(7, 6, 2, 3);
			Caras[3] = new Cara(4, 7, 3, 0);
			Caras[4] = new Cara(5, 4, 0, 1);
			Caras[5] = new Cara(6, 5, 1, 2);

			//For i As Integer = 0 To 5
			//    Caras(i).RevertirVertices()
			//Next

			return new Poliedro(Vertices, Caras);
		}

		public static Poliedro Esfera(int Pasos)
		{
			Vertice[] Vertices = new Vertice[(Math.Pow(Pasos, 2)) - Pasos + 2];
			Cara[] Caras = new Cara[(Math.Pow(Pasos, 2))];
			int cont = 0;
			int contc = 0;
			double Radio = 1;


			for (int i = 0; i <= Pasos - 1; i++) {
				Caras[i] = new Cara(3);
			}
			for (int i = Pasos; i <= (Math.Pow(Pasos, 2)) - Pasos - 1; i++) {
				Caras[i] = new Cara(4);
			}
			for (int i = (Math.Pow(Pasos, 2)) - Pasos; i <= (Math.Pow(Pasos, 2)) - 1; i++) {
				Caras[i] = new Cara(3);
			}

			cont = 1;
			contc = 0;

			Vertices[0] = new Vertice(new Punto3D(0, 0, 1));
			Vertices[Vertices.GetUpperBound(0)] = new Vertice(new Punto3D(0, 0, -1));

			for (double a = 0; a <= Math.PI; a += Math.PI / (Pasos / 1)) {
				if (a == 0 | a == Math.PI)
					continue;
				Radio = Math.Sin(a);
				for (double b = 0; b <= 2 * Math.PI; b += Math.PI / (Pasos / 2)) {
					if (b == 2 * Math.PI)
						continue;
					Vertices[cont] = new Vertice(new Punto3D(Radio * Math.Cos(b), Radio * Math.Sin(b), Math.Cos(a)));

					if (cont == Vertices.GetUpperBound(0))
						break; // TODO: might not be correct. Was : Exit For
					cont += 1;
				}
				if (cont == (Math.Pow(Pasos, 2)) - 1)
					break; // TODO: might not be correct. Was : Exit For
			}

			cont = 1;
			for (int i = 0; i <= Pasos - 1; i++) {
				Caras[i].Vertices[0] = 0;
				Caras[i].Vertices[1] = (cont + 1 <= Pasos - 0 ? cont + 1 : 1);
				Caras[i].Vertices[2] = cont;
				if (cont == Pasos)
					break; // TODO: might not be correct. Was : Exit For
				cont += 1;
			}

			cont = Vertices.GetUpperBound(0) - Pasos - 1;
			for (int i = Caras.GetUpperBound(0) - Pasos; i <= Caras.GetUpperBound(0); i++) {
				Caras[i].Vertices[0] = cont;
				Caras[i].Vertices[1] = (cont + 1 < Vertices.GetUpperBound(0) - 1 ? cont + 1 : Vertices.GetUpperBound(0) - Pasos - 1);
				Caras[i].Vertices[2] = Vertices.GetUpperBound(0);
				if (cont == Vertices.GetUpperBound(0) - 1)
					break; // TODO: might not be correct. Was : Exit For
				cont += 1;
			}

			for (int i = 1; i <= Pasos - 2; i++) {
				for (int j = 0; j <= Pasos - 1; j++) {
					Caras[(i * Pasos) + j].Vertices[0] = ((i - 1) * Pasos) + j + 1;
					Caras[(i * Pasos) + j].Vertices[1] = (j + 1 <= Pasos - 1 ? ((i - 1) * Pasos) + j + 2 : ((i - 1) * Pasos) + 1);
					Caras[(i * Pasos) + j].Vertices[2] = (j + 1 <= Pasos - 1 ? ((i) * Pasos) + j + 2 : ((i) * Pasos) + 1);
					Caras[(i * Pasos) + j].Vertices[3] = ((i) * Pasos) + j + 1;
				}
			}

			return new Poliedro(Vertices, Caras);
		}

		public static Poliedro Malla(int Dimensiones, int NumeroCeldas)
		{
			Vertice[] Vertices = null;
			Cara[] Caras = null;

			double p = -(Dimensiones / 2);
			double d = (Dimensiones / NumeroCeldas);
			int indice = 0;

			Vertices = new Vertice[(Math.Pow((NumeroCeldas + 1), 2))];
			Caras = new Cara[(Math.Pow(NumeroCeldas, 2))];

			for (int i = 0; i <= NumeroCeldas; i++) {
				for (int j = 0; j <= NumeroCeldas; j++) {
					Vertices[(i * (NumeroCeldas + 1)) + j] = new Vertice(new Punto3D(p + (d * (j)), 0, p + (d * (i))));
				}
			}

			for (int i = 0; i <= NumeroCeldas - 1; i++) {
				for (int j = 0; j <= NumeroCeldas - 1; j++) {
					indice = (i * (NumeroCeldas)) + j;
					Caras[indice] = new Cara(indice, indice + 1, indice + NumeroCeldas + 2, indice + NumeroCeldas + 1);
				}
			}

			return new Poliedro(Vertices, Caras);
		}

		public static bool operator ==(Poliedro P1, Poliedro P2)
		{
			return P1.Equals(P2);
		}

		public static bool operator !=(Poliedro P1, Poliedro P2)
		{
			return !(P1 == P2);
		}
	}
}

