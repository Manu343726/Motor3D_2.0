using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Discreta
{
	public class Grafo
	{
		private Arista[] mAristas;
		private Vertice[] mVertices;

		private long mNumeroVertices;
		public Arista[] Aristas {
			get { return mAristas.ToArray(); }
		}

		public Vertice[] Vertices {
			get { return mVertices; }
		}

		public long NumeroVertices {
			get { return mNumeroVertices; }
		}

		public long NumeroAristas {
			get { return mAristas.GetUpperBound(0) + 1; }
		}

		public Grafo(long NumeroVertices, params Arista[] Aristas)
		{
			if (NumeroVertices > 0) {
				mNumeroVertices = NumeroVertices;
				mAristas = Aristas;
				mVertices = new Vertice[mNumeroVertices];

				for (int i = 0; i <= mNumeroVertices - 1; i++) {
					mVertices[i] = new Vertice(i);
					foreach (Arista Arista in mAristas) {
						if (mVertices[i].AristaValida[Arista]) {
							mVertices[i].AñadirArista(Arista);
						}
					}
				}
			} else {
				throw new ExcepcionDiscreta("GRAFO (NEW): Un grafo debe tener a menos un vertice" + Constants.vbNewLine + "NumeroVertices=" + NumeroVertices.ToString());
			}
		}

		public Grafo(Vertice[] Vertices, Arista[] Aristas, bool EstaEstructurado = false)
		{
			mVertices = Vertices;
			mAristas = Aristas;

			if (EstaEstructurado) {
				foreach (Vertice Vertice in mVertices) {
					foreach (Arista Arista in mAristas) {
						if (Vertice.AristaValida[Arista] && !Vertice.Aristas.Contains(Arista)) {
							Vertice.AñadirArista(Arista);
						}
					}
				}
			}
		}

		public static bool EsEuleriano(Grafo Grafo)
		{
			foreach (Vertice Vertice in Grafo.Vertices) {
				if (!Vertice.GradoPar) {
					return false;
				}
			}

			return true;
		}

		public static Grafo SubGrafo(Grafo Grafo, params Vertice[] Vertices)
		{
			List<Arista> Aristas = new List<Arista>();

			foreach (Arista Arista in Grafo.Aristas) {
				if (!Aristas.Contains(Arista)) {
					foreach (Vertice Vertice in Grafo.Vertices) {
						if (Vertice.AristaValida[Arista]) {
							Aristas.Add(Arista);
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				}
			}

			return new Grafo(Vertices, Aristas.ToArray());
		}

		public static Grafo SubGrafo(Grafo Grafo, Recorrido Recorrido)
		{
			return SubGrafo(Grafo, Recorrido.Vertices);
		}

		public static Grafo SubGrafo(Grafo G1, Grafo G2)
		{
			return SubGrafo(G1, G2.Vertices);
		}

		public static Recorrido RecorridoEuleriano(Grafo Grafo)
		{
			List<Vertice> Retorno = null;


		}

		public static bool operator ==(Grafo G1, Grafo G2)
		{
			return G1.Equals(G2);
		}

		public static bool operator !=(Grafo G1, Grafo G2)
		{
			return !G1.Equals(G2);
		}

		public static Grafo operator -(Grafo G1, Grafo G2)
		{
			return SubGrafo(G1, G2);
		}

		public static Grafo operator -(Grafo Grafo, Recorrido Recorrido)
		{
			return SubGrafo(Grafo, Recorrido);
		}
	}
}

