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
	public class Recorrido
	{
		private Vertice[] mVertices;

		private Grafo mGrafo;
		public Vertice[] Vertices {
			get { return mVertices; }
		}

		public Grafo Grafo {
			get { return mGrafo; }
		}

		public long Longitud {
			get { return mVertices.GetUpperBound(0) + 1; }
		}

		public Recorrido(ref Grafo Grafo, params Vertice[] Recorrido)
		{
			mGrafo = Grafo;
			mVertices = Recorrido;
		}

		public static Recorrido Concatenar(Recorrido R1, Recorrido R2)
		{
			Vertice[] Retorno = new Vertice[R1.Longitud + R2.Longitud - 1];

			if (R1.Grafo == R2.Grafo) {
				for (int i = 0; i <= Retorno.GetUpperBound(0); i++) {
					if (i <= R1.Vertices.GetUpperBound(0)) {
						Retorno[i] = R1.Vertices[i];
					} else {
						Retorno[i] = R2.Vertices[i - R1.Vertices.GetUpperBound(0)];
					}
				}

				return new Recorrido(R1.Grafo, Retorno);
			} else {
				throw new ExcepcionDiscreta("RECORRIDO(CONCATENAR): No se pueden concatenar dos recorridos de grafos diferentes");
			}
		}

		public static Recorrido operator +(Recorrido R1, Recorrido R2)
		{
			return Concatenar(R1, R2);
		}
	}
}

