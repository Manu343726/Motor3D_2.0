using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;
using Motor3D.Espacio3D;
using Motor3D.Espacio2D;
using System.Math;

namespace Motor3D.Escena
{
	// La matriz proyeccion está formada por columnas que corresponden a la proyección de cada base canónica
	// Las bases canónicas para el espacio 3D corresponden a:
	// - Bx=(1,0,0)
	// - By=(0,1,0)
	// - Bz=(0,0,1)

	//La proyeccion isometrica, por ejemplo:
	//Proyeccion(Bx)=(cos(30),-sen(30))
	//Proyeccion(By)=(-cos(30),-sen(30))
	//Proyeccion(Bz)=(0,1)
	//Por tanto la matriz proyeccion será:
	// cos(30) -cos(30) 0
	// -sen(30) -sen(30) 1
	//Y en coordenadas homogéneas:
	// cos(30) -cos(30) 0 0
	// -sen(30) -sen(30) 1 0
	//   0         0     0 1

	public class Proyeccion : ObjetoEscena
	{


		private Matriz mMatriz;
		public Matriz Matriz {
			get { return mMatriz; }
		}

		public Proyeccion()
		{
			mMatriz = new Matriz(3, 4);

			mMatriz.EstablecerValoresPorFila(0, 1, 0, 0, 0);
			mMatriz.EstablecerValoresPorFila(1, 0, 1, 0, 0);
			mMatriz.EstablecerValoresPorFila(2, 0, 0, 0, 1);
		}

		public Proyeccion(Matriz Matriz)
		{
			if (Matriz.Filas == 3 && Matriz.Columnas == 4) {
				mMatriz = Matriz;
			} else {
				throw new ExcepcionEscena("PROYECCION (NEW): Una proyección solo se puede definir mediante una matriz de dimensiones 3x4." + Constants.vbNewLine + "Dimensiones=" + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static Proyeccion ProyeccionParalela()
		{
			Matriz Matriz = new Matriz(3, 4);

			Matriz.EstablecerValoresPorFila(0, 1, 0, 0, 0);
			Matriz.EstablecerValoresPorFila(1, 0, 1, 0, 0);
			Matriz.EstablecerValoresPorFila(2, 0, 0, 0, 1);

			return new Proyeccion(Matriz);
		}

		public static Proyeccion ProyeccionIsometrica()
		{
			Matriz Matriz = new Matriz(3, 4);

			Matriz.EstablecerValoresPorFila(0, Math.Cos(Math.PI / 6), -Math.Cos(Math.PI / 6), 0, 0);
			Matriz.EstablecerValoresPorFila(1, -Math.Sin(Math.PI / 6), -Math.Sin(Math.PI / 6), 1, 0);
			Matriz.EstablecerValoresPorFila(2, 0, 0, 0, 1);

			return new Proyeccion(Matriz);
		}

		public static Punto2D Proyectar(Proyeccion Proyeccion, Punto3D Punto)
		{
			return new Punto2D(Proyeccion.Matriz * Punto.Matriz);
		}
	}
}

