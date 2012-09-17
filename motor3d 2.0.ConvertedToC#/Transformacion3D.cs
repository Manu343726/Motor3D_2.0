using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;
using Motor3D.Primitivas3D;
using System.Math;

namespace Motor3D.Espacio3D.Transformaciones
{
	public class Transformacion3D : ObjetoGeometrico3D
	{


		protected Matriz mMatriz;
		public Matriz Matriz {
			get { return mMatriz; }
		}

		public Transformacion3D()
		{
			mMatriz = Motor3D.Algebra.Matriz.MatrizUnitaria(4);
		}

		public Transformacion3D(Matriz Matriz)
		{
			if (Matriz.EsCuadrada && Matriz.Filas == 4) {
				mMatriz = Matriz;
			} else {
				throw new ExcepcionGeometrica3D("TRANSFORMACION3D (NEW): Las matrices de transformación tridimensional solo se pueden implementar con matrices cuadradas de 4x4" + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static Transformacion3D EncadenarTransformaciones(Transformacion3D PrimeraTransformacion, Transformacion3D SegundaTransformacion)
		{
			return new Transformacion3D(SegundaTransformacion.Matriz * PrimeraTransformacion.Matriz);
		}

		public static Punto3D AplicarTransformacion(Punto3D Punto, Transformacion3D Transformacion)
		{
			return new Punto3D(Transformacion.Matriz * Punto.Matriz);
		}

		public static Vector3D AplicarTransformacion(Vector3D Vector, Transformacion3D Transformacion)
		{
			return new Vector3D(Transformacion.Matriz * Vector.Matriz);
		}

		public static Vertice AplicarTransformacion(Vertice Vertice, Transformacion3D Transformacion)
		{
			return new Vertice(new Punto3D(Transformacion.Matriz * Vertice.CoodenadasSUR.Matriz));
		}

		public static Recta3D AplicarTransformacion(Recta3D Recta, Transformacion3D Transformacion)
		{
			return new Recta3D(Transformacion.Matriz * Recta.Matrices[0], Transformacion.Matriz * Recta.Matrices[1]);
		}

		public static Plano3D AplicarTransformacion(Plano3D Plano, Transformacion3D Transformacion)
		{
			return new Plano3D(new Punto3D(Transformacion.Matriz * Plano.ObtenerPunto(0, 0).Matriz), new Vector3D(Transformacion.Matriz * Plano.VectorNormal.Matriz));
		}

		public static Caja3D AplicarTransformacion(Caja3D Caja, Transformacion3D Transformacion)
		{
			return new Caja3D(new Punto3D(Transformacion.Matriz * Caja.Posicion.Matriz), Caja.Dimensiones);
		}

		public static Segmento3D AplicarTransformacion(Segmento3D Segmento, Transformacion3D Transformacion)
		{
			return new Segmento3D(new Punto3D(Transformacion.Matriz * Segmento.ExtremoInicial.Matriz), new Punto3D(Transformacion.Matriz * Segmento.ExtremoFinal.Matriz));
		}

		public static Transformacion3D operator +(Transformacion3D PrimeraTransformacion, Transformacion3D SegundaTransformacion)
		{
			return EncadenarTransformaciones(PrimeraTransformacion, SegundaTransformacion);
		}

		public static Punto3D operator *(Punto3D Punto, Transformacion3D Transformacion)
		{
			return AplicarTransformacion(Punto, Transformacion);
		}

		public static Punto3D operator *(Transformacion3D Transformacion, Punto3D Punto)
		{
			return AplicarTransformacion(Punto, Transformacion);
		}

		public static Vector3D operator *(Vector3D Vector, Transformacion3D Transformacion)
		{
			return AplicarTransformacion(Vector, Transformacion);
		}

		public static Vector3D operator *(Transformacion3D Transformacion, Vector3D Vector)
		{
			return AplicarTransformacion(Vector, Transformacion);
		}

		public static Recta3D operator *(Recta3D Recta, Transformacion3D Transformacion)
		{
			return AplicarTransformacion(Recta, Transformacion);
		}

		public static Recta3D operator *(Transformacion3D Transformacion, Recta3D Recta)
		{
			return AplicarTransformacion(Recta, Transformacion);
		}

		public override string ToString()
		{
			return "{Transformacion3D: Matriz (" + mMatriz.ToString() + "}";
		}
	}
}

