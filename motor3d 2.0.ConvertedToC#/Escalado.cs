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
	public class Escalado : Transformacion3D
	{


		private Vector3D mEscalado;
		public Vector3D Escalado {
			get { return mEscalado; }
		}

		public Escalado(double X, double Y, double Z)
		{
			mEscalado = new Vector3D(X, Y, Z);
			EstablecerMatriz();
		}

		public Escalado(double Escalado)
		{
			mEscalado = new Vector3D(Escalado, Escalado, Escalado);
			EstablecerMatriz();
		}

		public Escalado(Vector3D Escalado)
		{
			mEscalado = Escalado;
			EstablecerMatriz();
		}

		private void EstablecerMatriz()
		{
			mMatriz.EstablecerValoresPorFila(0, mEscalado.X, 0, 0, 0);
			mMatriz.EstablecerValoresPorFila(1, 0, mEscalado.Y, 0, 0);
			mMatriz.EstablecerValoresPorFila(2, 0, 0, mEscalado.Z, 0);
			mMatriz.EstablecerValoresPorFila(3, 0, 0, 0, 1);
		}

		public static Escalado EncadenarTransformaciones(Escalado E1, Escalado E2)
		{
			return new Escalado(E1.Escalado + E2.Escalado);
		}

		public static Punto3D AplicarTransformacion(Punto3D Punto, Escalado Escalado)
		{
			return new Punto3D(Punto.X * Escalado.Escalado.X, Punto.Y * Escalado.Escalado.Y, Punto.Z * Escalado.Escalado.Z);
		}

		public static Vector3D AplicarTransformacion(Vector3D Vector, Escalado Escalado)
		{
			return new Vector3D(Vector.X * Escalado.Escalado.X, Vector.Y * Escalado.Escalado.Y, Vector.Z * Escalado.Escalado.Z);
		}

		public static Vertice AplicarTransformacion(Vertice Vertice, Escalado Escalado)
		{
			return new Vertice(new Punto3D(Vertice.CoodenadasSUR.X * Escalado.Escalado.X, Vertice.CoodenadasSUR.Y * Escalado.Escalado.Y, Vertice.CoodenadasSUR.Z * Escalado.Escalado.Z));
		}

		public static Recta3D AplicarTransformacion(Recta3D Recta, Escalado Escalado)
		{
			return new Recta3D(new Punto3D(Recta.PuntoInicial.X * Escalado.Escalado.X, Recta.PuntoInicial.Y * Escalado.Escalado.Y, Recta.PuntoInicial.Z * Escalado.Escalado.Z), Recta.VectorDirector);
		}

		public static Plano3D AplicarTransformacion(Plano3D Plano, Escalado Escalado)
		{
			Punto3D P = Plano.ObtenerPunto(0, 0);
			return new Plano3D(new Punto3D(P.X * Escalado.Escalado.X, P.Y * Escalado.Escalado.Y, P.Z * Escalado.Escalado.Z), Plano.VectorNormal);
		}

		public static Caja3D AplicarTransformacion(Caja3D Caja, Escalado Escalado)
		{
			return new Caja3D(new Punto3D(Caja.Left * Escalado.Escalado.X, Caja.Top * Escalado.Escalado.Y, Caja.Up * Escalado.Escalado.Z), new Vector3D(Caja.Ancho * Escalado.Escalado.X, Caja.Largo * Escalado.Escalado.Y, Caja.Alto * Escalado.Escalado.Z));
		}

		public static Segmento3D AplicarTransformacion(Segmento3D Segmento, Escalado Escalado)
		{
			return new Segmento3D(new Punto3D(Segmento.ExtremoInicial.X * Escalado.Escalado.X, Segmento.ExtremoInicial.Y * Escalado.Escalado.Y, Segmento.ExtremoInicial.Z * Escalado.Escalado.Z), new Punto3D(Segmento.ExtremoFinal.X * Escalado.Escalado.X, Segmento.ExtremoFinal.Y * Escalado.Escalado.Y, Segmento.ExtremoFinal.Z * Escalado.Escalado.Z));
		}

		public static Poliedro AplicarTransformacion(Poliedro Poliedro, Escalado Escalado)
		{
			Poliedro.AplicarTransformacion(Escalado);
			return Poliedro;
		}

		public static Escalado operator +(Escalado E1, Escalado E2)
		{
			return EncadenarTransformaciones(E1, E2);
		}

		public static Punto3D operator *(Escalado Escalado, Punto3D Punto)
		{
			return AplicarTransformacion(Punto, Escalado);
		}

		public static Punto3D operator *(Punto3D Punto, Escalado Escalado)
		{
			return AplicarTransformacion(Punto, Escalado);
		}

		public static Vector3D operator *(Escalado Escalado, Vector3D Vector)
		{
			return AplicarTransformacion(Vector, Escalado);
		}

		public static Vector3D operator *(Vector3D Vector, Escalado Escalado)
		{
			return AplicarTransformacion(Vector, Escalado);
		}

		public static Vertice operator *(Escalado Escalado, Vertice Vertice)
		{
			return AplicarTransformacion(Vertice, Escalado);
		}

		public static Vertice operator *(Vertice Vertice, Escalado Escalado)
		{
			return AplicarTransformacion(Vertice, Escalado);
		}

		public static Recta3D operator *(Escalado Escalado, Recta3D Recta)
		{
			return AplicarTransformacion(Recta, Escalado);
		}

		public static Recta3D operator *(Recta3D Recta, Escalado Escalado)
		{
			return AplicarTransformacion(Recta, Escalado);
		}

		public static Plano3D operator *(Escalado Escalado, Plano3D Plano)
		{
			return AplicarTransformacion(Plano, Escalado);
		}

		public static Plano3D operator *(Plano3D Plano, Escalado Escalado)
		{
			return AplicarTransformacion(Plano, Escalado);
		}

		public static Caja3D operator *(Escalado Escalado, Caja3D Caja)
		{
			return AplicarTransformacion(Caja, Escalado);
		}

		public static Caja3D operator *(Caja3D Caja, Escalado Escalado)
		{
			return AplicarTransformacion(Caja, Escalado);
		}

		public static Segmento3D operator *(Escalado Escalado, Segmento3D Segmento)
		{
			return AplicarTransformacion(Segmento, Escalado);
		}

		public static Segmento3D operator *(Segmento3D Segmento, Escalado Escalado)
		{
			return AplicarTransformacion(Segmento, Escalado);
		}

		public static Poliedro operator *(Escalado Escalado, Poliedro Poliedro)
		{
			return AplicarTransformacion(Poliedro, Escalado);
		}

		public static Poliedro operator *(Poliedro Poliedro, Escalado Escalado)
		{
			return AplicarTransformacion(Poliedro, Escalado);
		}
	}
}

