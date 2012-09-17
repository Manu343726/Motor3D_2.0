using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Motor3D.Algebra
{

	public class Matriz : ObjetoAlgebraico
	{


		private double[,] mMatriz;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Matriz Sender);

		public double[,] Matriz {
			get { return mMatriz; }
		}

		public double this[int X, int Y] {
			get { return mMatriz[X, Y]; }
			set { mMatriz[X, Y] = value; }
		}

		public double[,] Array {
			get { return mMatriz; }
		}

		public double Valores {
			get {
				if (X >= 0 && X <= mMatriz.GetUpperBound(0) && Y >= 0 && Y <= mMatriz.GetUpperBound(1)) {
					return mMatriz[X, Y];
				} else {
					throw new ExcepcionMatriz("MATRIZ (VALORES_GET): El índice está fuera de los límites de la matriz." + Constants.vbNewLine + "Dimensiones de la matriz: " + Filas.ToString() + "x" + Columnas.ToString() + Constants.vbNewLine + "Primer índice: " + X.ToString() + Constants.vbNewLine + "Segundo índice: " + Y.ToString());
				}
			}
			set {
				if (X >= 0 && X <= mMatriz.GetUpperBound(0) && Y >= 0 && Y <= mMatriz.GetUpperBound(1)) {
					mMatriz[X, Y] = value;
					if (Modificado != null) {
						Modificado(this);
					}
				} else {
					throw new ExcepcionMatriz("MATRIZ (VALORES_SET): El índice está fuera de los límites de la matriz." + Constants.vbNewLine + "Dimensiones de la matriz: " + Filas.ToString() + "x" + Columnas.ToString() + Constants.vbNewLine + "Primer índice: " + X.ToString() + Constants.vbNewLine + "Segundo índice: " + Y.ToString());
				}
			}
		}

		public int Filas {
			get { return mMatriz.GetUpperBound(0) + 1; }
		}

		public int Columnas {
			get { return mMatriz.GetUpperBound(1) + 1; }
		}

		public bool EsCuadrada {
			get { return (mMatriz.GetUpperBound(0) == mMatriz.GetUpperBound(1)); }
		}

		public Matriz Inversa {
			get { return CalculoInversa(this); }
		}

		public double Determinante {
			get { return CalculoDeterminante(this); }
		}

		public Matriz(int ValFilas, int ValColumnas)
		{
			if (ValFilas < 1)
				ValFilas = 1;
			if (ValColumnas < 1)
				ValColumnas = 1;

			mMatriz = new double[ValFilas, ValColumnas];
		}

		public Matriz(ref double[,] ValMatriz)
		{
			mMatriz = ValMatriz;
		}

		public void EstablecerValor(int Fila, int Columna, double Valor)
		{
			if (Fila >= 0 && Columna >= 0 && Fila <= mMatriz.GetUpperBound(0) && Columna <= mMatriz.GetUpperBound(1)) {
				mMatriz[Fila, Columna] = Valor;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public void EstablecerValoresPorFila(int Fila, params double[] Valores)
		{
			if (Fila >= 0 && Fila <= mMatriz.GetUpperBound(0)) {
				for (int i = 0; i <= mMatriz.GetUpperBound(1); i++) {
					if (i <= Valores.GetUpperBound(0)) {
						EstablecerValor(Fila, i, Valores[i]);
					} else {
						EstablecerValor(Fila, i, 0);
					}
				}
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public void EstablecerValoresPorColumna(int Columna, params double[] Valores)
		{
			if (Columna >= 0 && Columna <= mMatriz.GetUpperBound(1)) {
				for (int j = 0; j <= mMatriz.GetUpperBound(0); j++) {
					if (j <= Valores.GetUpperBound(0)) {
						EstablecerValor(j, Columna, Valores[j]);
					} else {
						EstablecerValor(j, Columna, 0);
					}
				}
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public double ObtenerValor(int Fila, int Columna)
		{
			if (Fila >= 0 && Columna >= 0 && Fila <= mMatriz.GetUpperBound(0) && Columna <= mMatriz.GetUpperBound(1)) {
				return mMatriz[Fila, Columna];
			}
		}

		public double[] ObtenerFila(int Fila)
		{
			double[] Retorno = new double[mMatriz.GetUpperBound(1) + 1];

			if (Fila >= 0 && Fila < Filas) {
				for (int i = 0; i <= Retorno.GetUpperBound(0); i++) {
					Retorno[i] = mMatriz[Fila, i];
				}

				return Retorno;
			} else {
				throw new ExcepcionMatriz("MATRIZ (OBTENER FILA): La fila especificada está fuera del rango." + Constants.vbNewLine + "Dimensiones de la matriz: " + Filas.ToString() + "x" + Columnas.ToString() + Constants.vbNewLine + "Fila especificada: " + Fila.ToString());
			}
		}

		public double[] ObtenerColumna(int Columna)
		{
			double[] Retorno = new double[mMatriz.GetUpperBound(0) + 1];

			if (Columna >= 0 && Columna < Columnas) {
				for (int i = 0; i <= Retorno.GetUpperBound(0); i++) {
					Retorno[i] = mMatriz[i, Columna];
				}

				return Retorno;
			} else {
				throw new ExcepcionMatriz("MATRIZ (OBTENER COLUMNA): La columna especificada está fuera del rango." + Constants.vbNewLine + "Dimensiones de la matriz: " + Columnas.ToString() + "x" + Columnas.ToString() + Constants.vbNewLine + "Columna especificada: " + Columna.ToString());
			}
		}

		public Matriz Copia()
		{
			return Copia(this);
		}

		public override string ToString()
		{
			string Retorno = "";

			for (int i = 0; i <= mMatriz.GetUpperBound(0); i++) {
				for (int j = 0; j <= mMatriz.GetUpperBound(1); j++) {
					Retorno += Strings.FormatNumber(mMatriz[i, j], 2) + " ";
				}
				Retorno += Constants.vbNewLine;
			}

			return Retorno;
		}

		public string ToString(bool Multiline)
		{
			string Retorno = "";

			if (Multiline) {
				for (int i = 0; i <= mMatriz.GetUpperBound(0); i++) {
					for (int j = 0; j <= mMatriz.GetUpperBound(1); j++) {
						Retorno += Strings.FormatNumber(mMatriz[i, j], 2) + " ";
					}
					Retorno += Constants.vbNewLine;
				}
			} else {
				for (int i = 0; i <= mMatriz.GetUpperBound(0); i++) {
					for (int j = 0; j <= mMatriz.GetUpperBound(1); j++) {
						Retorno += Strings.FormatNumber(mMatriz[i, j], 2) + " ";
					}
					Retorno += "/";
				}
			}

			return Retorno;
		}

		public static Matriz Suma(Matriz M1, Matriz M2)
		{
			Matriz Retorno = null;

			if ((M1.Filas == M2.Filas && M1.Columnas == M2.Columnas)) {
				Retorno = new Matriz(M1.Filas, M1.Columnas);

				for (int i = 0; i <= M1.Filas - 1; i++) {
					for (int j = 0; j <= M1.Columnas - 1; j++) {
						Retorno.EstablecerValor(i, j, M1.ObtenerValor(i, j) + M2.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionOperacionMatricial("MATRIZ (SUMA): Los sumandos deben tener las mismas dimensiones" + Constants.vbNewLine + "Primar sumando: " + M1.Filas + "x" + M1.Columnas + Constants.vbNewLine + "Segundo sumando: " + M2.Filas + "x" + M2.Columnas);
			}
		}

		public static Matriz Resta(Matriz M1, Matriz M2)
		{
			Matriz Retorno = null;

			if ((M1.Filas == M2.Filas && M1.Columnas == M2.Columnas)) {
				Retorno = new Matriz(M1.Filas, M1.Columnas);

				for (int i = 0; i <= M1.Filas - 1; i++) {
					for (int j = 0; j <= M1.Columnas - 1; j++) {
						Retorno.EstablecerValor(i, j, M1.ObtenerValor(i, j) - M2.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionOperacionMatricial("MATRIZ (RESTA): Minuendo y sustraendo deben tener las mismas dimensiones" + Constants.vbNewLine + "Minuendo: " + M1.Filas + "x" + M1.Columnas + Constants.vbNewLine + "Sustraendo: " + M2.Filas + "x" + M2.Columnas);
			}
		}

		public static Matriz Producto(Matriz M1, Matriz M2)
		{
			if ((M1.Columnas == M2.Filas)) {
				double ValorElemento = 0;
				Matriz Retorno = new Matriz(M1.Filas, M2.Columnas);

				for (int i = 0; i <= M1.Filas - 1; i++) {
					for (int j = 0; j <= M2.Columnas - 1; j++) {
						ValorElemento = 0;
						for (int k = 0; k <= M1.Columnas - 1; k++) {
							ValorElemento += M1.ObtenerValor(i, k) * M2.ObtenerValor(k, j);
						}
						Retorno.EstablecerValor(i, j, ValorElemento);
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionOperacionMatricial("MATRIZ (PRODUCTO): El número de columnas del primer factor debe ser igual al número de filas del segundo" + Constants.vbNewLine + "Primer factor: " + M1.Filas + "x" + M1.Columnas + Constants.vbNewLine + "Segundo factor: " + M2.Filas + "x" + M2.Columnas);
			}
		}

		public static Matriz Producto(Matriz Matriz, double Factor)
		{
			for (int i = 0; i <= Matriz.Filas - 1; i++) {
				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					Matriz.EstablecerValor(i, j, Matriz.ObtenerValor(i, j) * Factor);
				}
			}

			return Matriz;
		}

		public static Matriz Transpuesta(Matriz Matriz)
		{
			var Retorno = new Matriz(Matriz.Columnas, Matriz.Filas);

			for (int i = 0; i <= Matriz.Filas - 1; i++) {
				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					Retorno.EstablecerValor(j, i, Matriz.ObtenerValor(i, j));
				}
			}

			return Retorno;
		}

		public static object SubMatriz(Matriz Matriz, int Fila, int Columna)
		{
			if (Matriz.Filas >= 2 && Matriz.Columnas >= 2) {
				Matriz Retorno = new Matriz(Matriz.Filas - 1, Matriz.Columnas - 1);

				int x = 0;
				int y = 0;

				for (int i = 0; i <= Matriz.Filas - 1; i++) {
					switch (i) {
						case  // ERROR: Case labels with binary operators are unsupported : LessThan
Fila:
							x = i;
							break;
						case  // ERROR: Case labels with binary operators are unsupported : Equality
Fila:
							continue;
						case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
Fila:
							x = i - 1;
							break;
					}

					for (int j = 0; j <= Matriz.Columnas - 1; j++) {
						switch (j) {
							case  // ERROR: Case labels with binary operators are unsupported : LessThan
Columna:
								y = j;
								break;
							case  // ERROR: Case labels with binary operators are unsupported : Equality
Columna:
								continue;
							case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
Columna:
								y = j - 1;
								break;
						}

						Retorno.EstablecerValor(x, y, Matriz.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static Matriz SubMatrizPorTamaño(Matriz Matriz, int Filas, int Columnas)
		{
			Matriz Retorno = null;

			if (Filas <= Matriz.Filas && Columnas <= Matriz.Columnas && Filas > 0 && Columnas > 0) {
				Retorno = new Matriz(Filas, Columnas);

				for (int i = 0; i <= Filas - 1; i++) {
					for (int j = 0; j <= Columnas - 1; j++) {
						Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): El número de filas y/o columnas es menor que uno o superior a los originales." + Constants.vbNewLine + "Dimensiones originales: " + Matriz.Filas.ToString() + "x" + Matriz.Columnas.ToString() + Constants.vbNewLine + "Dimensiones especificadas: " + Filas.ToString() + "x" + Columnas.ToString());
			}
		}

		public static Matriz SubMatrizPorFila(Matriz Matriz, int Fila)
		{
			if (Matriz.Filas >= 2) {
				Matriz Retorno = new Matriz(Matriz.Filas - 1, Matriz.Columnas);

				int x = 0;

				for (int i = 0; i <= Matriz.Filas - 1; i++) {
					switch (i) {
						case  // ERROR: Case labels with binary operators are unsupported : LessThan
Fila:
							x = i;
							break;
						case  // ERROR: Case labels with binary operators are unsupported : Equality
Fila:
							continue;
						case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
Fila:
							x = i - 1;
							break;
					}

					for (int j = 0; j <= Matriz.Columnas - 1; j++) {
						Retorno.EstablecerValor(x, j, Matriz.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static Matriz SubMatrizPorColumna(Matriz Matriz, int Columna)
		{
			if (Matriz.Columnas >= 2) {
				Matriz Retorno = new Matriz(Matriz.Filas, Matriz.Columnas - 1);

				int y = 0;

				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					switch (j) {
						case  // ERROR: Case labels with binary operators are unsupported : LessThan
Columna:
							y = j;
							break;
						case  // ERROR: Case labels with binary operators are unsupported : Equality
Columna:
							continue;
						case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
Columna:
							y = j - 1;
							break;
					}

					for (int i = 0; i <= Matriz.Filas - 1; i++) {
						Retorno.EstablecerValor(i, y, Matriz.ObtenerValor(i, j));
					}
				}

				return Retorno;
			} else {
				throw new ExcepcionSubMatriz("MATRIZ (SUBMATRIZ): No se puede obtener una submatriz de una matriz columna, una matriz fila, o una matriz de un solo elemento." + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static object SubMatrizCuadrada(Matriz Matriz, int Fila, int Columna, int Dimensiones)
		{
			object functionReturnValue = null;
			if (!((Fila + Dimensiones <= Matriz.Filas) && (Columna + Dimensiones <= Matriz.Columnas))) {
				throw new ExcepcionMatriz("MATRIZ (SUBMATRIZ CUADRADA): No se puede obtener una submatriz de las dimensiones especificadas desde el elemento especificado.");
				return functionReturnValue;
			}

			Matriz Retorno = new Matriz(Dimensiones, Dimensiones);

			for (int i = 0; i <= Retorno.Filas - 1; i++) {
				for (int j = 0; j <= Retorno.Columnas - 1; j++) {
					Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(i + Fila, j + Columna));
				}
			}

			return Retorno;
			return functionReturnValue;
		}

		public static double CalculoDeterminante(Matriz Matriz)
		{
			if (Matriz.Filas == Matriz.Columnas) {
				double Positivos = 0;
				double Negativos = 0;

				switch (Matriz.Filas) {
					case 1:
						return Matriz.ObtenerValor(0, 0);
					case 2:
						Positivos = Matriz.ObtenerValor(0, 0) * Matriz.ObtenerValor(1, 1);
						Negativos = Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 0);

						return Positivos - Negativos;
					case 3:
						Positivos = (Matriz.ObtenerValor(0, 0) * Matriz.ObtenerValor(1, 1) * Matriz.ObtenerValor(2, 2)) + (Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 2) * Matriz.ObtenerValor(2, 0)) + (Matriz.ObtenerValor(1, 0) * Matriz.ObtenerValor(2, 1) * Matriz.ObtenerValor(0, 2));

						Negativos = (Matriz.ObtenerValor(0, 2) * Matriz.ObtenerValor(1, 1) * Matriz.ObtenerValor(2, 0)) + (Matriz.ObtenerValor(0, 1) * Matriz.ObtenerValor(1, 0) * Matriz.ObtenerValor(2, 2)) + (Matriz.ObtenerValor(1, 2) * Matriz.ObtenerValor(2, 1) * Matriz.ObtenerValor(0, 0));


						return Positivos - Negativos;
					default:
						double Retorno = 0;
						for (int i = 0; i <= Matriz.Filas - 1; i++) {
							Retorno += (Adjunto(Matriz, i, 0) * Matriz.ObtenerValor(i, 0));
						}


						return Retorno;
				}

			} else {
				throw new ExcepcionMatrizNoCuadrada("MATRIZ (DETERMINANTE): Solo se puede obtener el determinante de matrices cuadradas." + Constants.vbNewLine + "Dimensiones de la matriz: " + Matriz.Filas + "x" + Matriz.Columnas);
			}
		}

		public static double Menor(Matriz Matriz, int Fila, int Columna)
		{
			Matriz SubMatriz = Matriz.SubMatriz(Matriz, Fila, Columna);

			if (SubMatriz.EsCuadrada) {
				return CalculoDeterminante(SubMatriz);
			} else {
				throw new ExcepcionMatrizNoCuadrada("MATRIZ (MENOR): La submatriz obtenida no es cuadrada. No se puede calcular el determinante." + Constants.vbNewLine + "Dimensiones de la submatriz: " + SubMatriz.Filas + "x" + SubMatriz.Columnas);
			}
		}

		public static double Adjunto(Matriz Matriz, int Fila, int Columna)
		{
			Matriz SubMatriz = Matriz.SubMatriz(Matriz, Fila, Columna);

			if (SubMatriz.EsCuadrada) {
				return (Math.Pow((-1), (Fila + Columna + 2))) * CalculoDeterminante(SubMatriz);
			} else {
				throw new ExcepcionMatrizNoCuadrada("MATRIZ (ADJUNTO): La submatriz obtenida no es cuadrada. No se puede calcular el determinante." + Constants.vbNewLine + "Dimensiones de la submatriz: " + SubMatriz.Filas + "x" + SubMatriz.Columnas);
			}
		}

		public static Matriz Adjunta(Matriz Matriz)
		{
			Matriz functionReturnValue = null;
			Matriz Retorno = new Matriz(Matriz.Filas, Matriz.Columnas);

			for (int i = 0; i <= Matriz.Filas - 1; i++) {
				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					try {
						Retorno.EstablecerValor(i, j, Adjunto(Matriz, i, j));
					} catch (ExcepcionAlgebraica ex) {
						throw new ExcepcionMatrizNoCuadrada("MATRIZ (ADJUNTA): La submatriz obtenida no es cuadrada. No se puede calcular el determinante.");
						return functionReturnValue;
					}
				}
			}

			return Retorno;
			return functionReturnValue;
		}

		public static Matriz CalculoInversa(Matriz Matriz)
		{
			if (Matriz.EsCuadrada) {
				double Det = CalculoDeterminante(Matriz);
				if (Det != 0) {
					return Producto(Transpuesta(Adjunta(Matriz)), (1 / Det));
				} else {
					throw new ExcepcionOperacionMatricial("MATRIZ (CALCULOINVERSA): El determinante de la matriz es cero. No se puede calcular la inversa.");
				}
			} else {
				throw new ExcepcionMatrizNoCuadrada("MATRIZ (CALCULOINVERSA): La matriz no es cuadrada. No se puede calcular el determinante.");
			}
		}

		public static int Rango(Matriz Matriz)
		{
			Matriz SubMat = null;
			bool NoCeros = true;
			bool SinSubMat = false;
			int Tamaño = (Matriz.Filas > Matriz.Columnas ? Matriz.Columnas : Matriz.Filas);
			int Det = 0;

			if (Matriz.EsCuadrada) {

				if (CalculoDeterminante(Matriz) == 0) {
					while (Tamaño > 1) {
						SinSubMat = true;

						for (int i = 0; i <= Matriz.Filas - 1; i++) {
							for (int j = 0; j <= Matriz.Columnas - 1; j++) {
								try {
									SubMat = SubMatrizCuadrada(Matriz, i, j, Tamaño);
									Det = CalculoDeterminante(SubMat);

									if (Det == 0) {
										Tamaño -= 1;
										continue;
									}

									SinSubMat = false;
								} catch (ExcepcionMatriz ex) {
								}
							}
						}

						if (!SinSubMat) {
							if (Tamaño > 1) {
								return Tamaño;
							}
						}

						Tamaño -= 1;
					}

					return 1;
				} else {
					return Matriz.Filas;
				}
			} else {
				while (Tamaño > 1) {
					SinSubMat = true;

					for (int i = 0; i <= Matriz.Filas - 1; i++) {
						for (int j = 0; j <= Matriz.Columnas - 1; j++) {
							try {
								SubMat = SubMatrizCuadrada(Matriz, i, j, Tamaño);
								Det = CalculoDeterminante(SubMat);

								if (Det == 0) {
									Tamaño -= 1;
									continue;
								}

								SinSubMat = false;
							} catch (ExcepcionMatriz ex) {
							}
						}
					}

					if (!SinSubMat) {
						if (Tamaño > 1) {
							return Tamaño;
						}
					}

					Tamaño -= 1;
				}

				return 1;
			}

		}

		public static Matriz Copia(Matriz Matriz)
		{
			Matriz Retorno = new Matriz(Matriz.Filas, Matriz.Columnas);

			for (int i = 0; i <= Matriz.Filas - 1; i++) {
				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					Retorno.EstablecerValor(i, j, Matriz.Matriz[i, j]);
				}
			}

			return Retorno;
		}

		public static Matriz operator +(Matriz M1, Matriz M2)
		{
			return Suma(M1, M2);
		}

		public static Matriz operator -(Matriz M1, Matriz M2)
		{
			return Resta(M1, M2);
		}

		public static Matriz operator *(Matriz M1, Matriz M2)
		{
			return Producto(M1, M2);
		}

		public static Matriz operator *(Matriz M1, double Factor)
		{
			return Producto(M1, Factor);
		}

		public static Matriz operator /(Matriz M1, Matriz M2)
		{
			return (M2.Inversa * M1);
		}

		public static Matriz operator /(Matriz M1, double Factor)
		{
			if (Factor != 0) {
				return Producto(M1, 1 / Factor);
			} else {
				throw new ExcepcionOperacionMatricial("MATRIZ (OPERADOR DE DIVISION): El factor de división es cero.");
			}
		}

		public static object MatrizUnitaria(int Dimensiones)
		{
			if (Dimensiones <= 0)
				Dimensiones = 1;

			Matriz Retorno = new Matriz(Dimensiones, Dimensiones);

			for (int i = 0; i <= Dimensiones - 1; i++) {
				for (int j = 0; j <= Dimensiones - 1; j++) {
					Retorno.EstablecerValor(i, j, (i == j ? 1 : 0));
				}
			}

			return Retorno;
		}

		public static object MatrizPorColumnas(Matriz Matriz, params int[] Columnas)
		{
			Matriz Retorno = new Matriz(Matriz.Filas, Columnas.GetUpperBound(0) + 1);

			for (int i = 0; i <= Columnas.GetUpperBound(0); i++) {
				for (int j = 0; j <= Matriz.Filas - 1; j++) {
					Retorno.EstablecerValor(j, i, Matriz.ObtenerValor(j, Columnas[i]));
				}
			}

			return Retorno;
		}

		public static object MatrizPorFilas(Matriz Matriz, params int[] Filas)
		{
			Matriz Retorno = new Matriz(Filas.GetUpperBound(0) + 1, Matriz.Columnas);

			for (int i = 0; i <= Filas.GetUpperBound(0); i++) {
				for (int j = 0; j <= Matriz.Columnas - 1; j++) {
					Retorno.EstablecerValor(i, j, Matriz.ObtenerValor(Filas[i], j));
				}
			}

			return Retorno;
		}
	}
}
