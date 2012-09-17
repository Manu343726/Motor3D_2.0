using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Primitivas3D;

namespace Motor3D.Espacio3D.Transformaciones
{
	public sealed class Traslacion : Transformacion3D
	{


		private Vector3D mTraslacion;
		public Vector3D Traslacion {
			get { return mTraslacion; }
		}

		public Traslacion(double X, double Y, double Z)
		{
			mTraslacion = new Vector3D(X, Y, Z);
			EstablecerMatriz();
		}

		public Traslacion(Vector3D Traslacion)
		{
			mTraslacion = Traslacion;
			EstablecerMatriz();
		}

		public Traslacion(Punto3D Inicio, Punto3D Fin)
		{
			mTraslacion = new Vector3D(Inicio, Fin);
			EstablecerMatriz();
		}

		private void EstablecerMatriz()
		{
			mMatriz.EstablecerValoresPorFila(0, 1, 0, 0, mTraslacion.X);
			mMatriz.EstablecerValoresPorFila(1, 0, 1, 0, mTraslacion.Y);
			mMatriz.EstablecerValoresPorFila(2, 0, 0, 1, mTraslacion.Z);
			mMatriz.EstablecerValoresPorFila(3, 0, 0, 0, 1);
		}

		public static Traslacion EncadenarTransformaciones(Traslacion E1, Traslacion E2)
		{
			return new Traslacion(E1.Traslacion + E2.Traslacion);
		}

		public static Punto3D AplicarTransformacion(Punto3D Punto, Traslacion Traslacion)
		{
			return new Punto3D(Punto.X + Traslacion.Traslacion.X, Punto.Y + Traslacion.Traslacion.Y, Punto.Z + Traslacion.Traslacion.Z);
		}

		public static Vector3D AplicarTransformacion(Vector3D Vector, Traslacion Traslacion)
		{
			return new Vector3D(Vector.X + Traslacion.Traslacion.X, Vector.Y + Traslacion.Traslacion.Y, Vector.Z + Traslacion.Traslacion.Z);
		}

		public static Vertice AplicarTransformacion(Vertice Vertice, Traslacion Traslacion)
		{
			return new Vertice(new Punto3D(Vertice.CoodenadasSUR.X + Traslacion.Traslacion.X, Vertice.CoodenadasSUR.Y + Traslacion.Traslacion.Y, Vertice.CoodenadasSUR.Z + Traslacion.Traslacion.Z));
		}

		public static Recta3D AplicarTransformacion(Recta3D Recta, Traslacion Traslacion)
		{
			return new Recta3D(new Punto3D(Recta.PuntoInicial.X + Traslacion.Traslacion.X, Recta.PuntoInicial.Y + Traslacion.Traslacion.Y, Recta.PuntoInicial.Z + Traslacion.Traslacion.Z), Recta.VectorDirector);
		}

		public static Plano3D AplicarTransformacion(Plano3D Plano, Traslacion Traslacion)
		{
			Punto3D P = Plano.ObtenerPunto(0, 0);
			return new Plano3D(new Punto3D(P.X + Traslacion.Traslacion.X, P.Y + Traslacion.Traslacion.Y, P.Z + Traslacion.Traslacion.Z), Plano.VectorNormal);
		}

		public static Caja3D AplicarTransformacion(Caja3D Caja, Traslacion Traslacion)
		{
			return new Caja3D(new Punto3D(Caja.Left + Traslacion.Traslacion.X, Caja.Top + Traslacion.Traslacion.Y, Caja.Up + Traslacion.Traslacion.Z), new Vector3D(Caja.Ancho + Traslacion.Traslacion.X, Caja.Largo + Traslacion.Traslacion.Y, Caja.Alto + Traslacion.Traslacion.Z));
		}

		public static Segmento3D AplicarTransformacion(Segmento3D Segmento, Traslacion Traslacion)
		{
			return new Segmento3D(new Punto3D(Segmento.ExtremoInicial.X + Traslacion.Traslacion.X, Segmento.ExtremoInicial.Y + Traslacion.Traslacion.Y, Segmento.ExtremoInicial.Z + Traslacion.Traslacion.Z), new Punto3D(Segmento.ExtremoFinal.X + Traslacion.Traslacion.X, Segmento.ExtremoFinal.Y + Traslacion.Traslacion.Y, Segmento.ExtremoFinal.Z + Traslacion.Traslacion.Z));
		}

		public static Poliedro AplicarTransformacion(Poliedro Poliedro, Traslacion Traslacion)
		{
			Poliedro.AplicarTransformacion(Traslacion);
			return Poliedro;
		}

		public static Traslacion operator +(Traslacion E1, Traslacion E2)
		{
			return EncadenarTransformaciones(E1, E2);
		}

		public static Punto3D operator *(Traslacion Traslacion, Punto3D Punto)
		{
			return AplicarTransformacion(Punto, Traslacion);
		}

		public static Punto3D operator *(Punto3D Punto, Traslacion Traslacion)
		{
			return AplicarTransformacion(Punto, Traslacion);
		}

		public static Vector3D operator *(Traslacion Traslacion, Vector3D Vector)
		{
			return AplicarTransformacion(Vector, Traslacion);
		}

		public static Vector3D operator *(Vector3D Vector, Traslacion Traslacion)
		{
			return AplicarTransformacion(Vector, Traslacion);
		}

		public static Vertice operator *(Traslacion Traslacion, Vertice Vertice)
		{
			return AplicarTransformacion(Vertice, Traslacion);
		}

		public static Vertice operator *(Vertice Vertice, Traslacion Traslacion)
		{
			return AplicarTransformacion(Vertice, Traslacion);
		}

		public static Recta3D operator *(Traslacion Traslacion, Recta3D Recta)
		{
			return AplicarTransformacion(Recta, Traslacion);
		}

		public static Recta3D operator *(Recta3D Recta, Traslacion Traslacion)
		{
			return AplicarTransformacion(Recta, Traslacion);
		}

		public static Plano3D operator *(Traslacion Traslacion, Plano3D Plano)
		{
			return AplicarTransformacion(Plano, Traslacion);
		}

		public static Plano3D operator *(Plano3D Plano, Traslacion Traslacion)
		{
			return AplicarTransformacion(Plano, Traslacion);
		}

		public static Caja3D operator *(Traslacion Traslacion, Caja3D Caja)
		{
			return AplicarTransformacion(Caja, Traslacion);
		}

		public static Caja3D operator *(Caja3D Caja, Traslacion Traslacion)
		{
			return AplicarTransformacion(Caja, Traslacion);
		}

		public static Segmento3D operator *(Traslacion Traslacion, Segmento3D Segmento)
		{
			return AplicarTransformacion(Segmento, Traslacion);
		}

		public static Segmento3D operator *(Segmento3D Segmento, Traslacion Traslacion)
		{
			return AplicarTransformacion(Segmento, Traslacion);
		}

		public static Poliedro operator *(Traslacion Traslacion, Poliedro Poliedro)
		{
			return AplicarTransformacion(Poliedro, Traslacion);
		}

		public static Poliedro operator *(Poliedro Poliedro, Traslacion Traslacion)
		{
			return AplicarTransformacion(Poliedro, Traslacion);
		}
	}
}

