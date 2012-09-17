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
	public class SistemaEcuaciones : ObjetoAlgebraico
	{

		private Ecuacion[] mEcuaciones;

		private int mNumeroIncognitas;

		private SolucionSistema mSolucion;
		private Matriz mMatrizPrincipal;

		private Matriz mMatrizAmpliada;
		private int mRangoMatrizAmpliada;

		private int mRangoMatrizPrincipal;

		private bool mEsHomogeneo;
		public Ecuacion[] Ecuaciones {
			get { return mEcuaciones; }
		}

		public int NumeroEcuaciones {
			get { return mEcuaciones.GetUpperBound(0) + 1; }
		}

		public int NumeroIncognitas {
			get { return mNumeroIncognitas; }
		}

		public SolucionSistema Solucion {
			get { return mSolucion; }
		}

		public int RangoMatrizAmpliada {
			get { return mRangoMatrizAmpliada; }
		}

		public int RangoMatrizPrincipal {
			get { return mRangoMatrizPrincipal; }
		}

		public Matriz MatrizPrincipal {
			get { return mMatrizPrincipal; }
		}

		public Matriz MatrizAmpliada {
			get { return mMatrizAmpliada; }
		}

		public bool EsHomogeneo {
			get { return mEsHomogeneo; }
		}

		public SistemaEcuaciones(params Ecuacion[] ValEcuaciones)
		{
			double[] Sols = null;
			double DetA = 0;
			int[] Columnas = null;
			Matriz Mat = null;
			Matriz MatOr = null;

			mEcuaciones = ValEcuaciones;
			mNumeroIncognitas = 0;
			mEsHomogeneo = TestHomogeneidad();

			for (int i = 0; i <= mEcuaciones.GetUpperBound(0); i++) {
				if (mNumeroIncognitas < mEcuaciones[i].NumeroVariables)
					mNumeroIncognitas = mEcuaciones[i].NumeroVariables;
			}

			if (mEsHomogeneo) {
				mMatrizPrincipal = Matriz.MatrizUnitaria(mNumeroIncognitas + 1);
				mMatrizAmpliada = Matriz.MatrizUnitaria(mNumeroIncognitas + 1);
				mRangoMatrizPrincipal = 0;
				mRangoMatrizAmpliada = 0;

				Sols = new double[mNumeroIncognitas];

				mSolucion = new SolucionSistema(Sols);
			} else {
				mMatrizAmpliada = new Matriz(mEcuaciones.GetUpperBound(0) + 1, mNumeroIncognitas + 1);

				for (int i = 0; i <= mEcuaciones.GetUpperBound(0); i++) {
					mMatrizAmpliada.EstablecerValoresPorFila(i, mEcuaciones[i].Variables);
				}

				mMatrizPrincipal = Matriz.SubMatrizPorColumna(mMatrizAmpliada, mNumeroIncognitas);

				mRangoMatrizAmpliada = Matriz.Rango(mMatrizAmpliada);
				mRangoMatrizPrincipal = Matriz.Rango(mMatrizPrincipal);

				if ((mRangoMatrizPrincipal == mRangoMatrizAmpliada)) {
					if (mRangoMatrizAmpliada == mNumeroIncognitas) {
						//RESOLUCION POR REGLA DE CRAMER:

						Sols = new double[mNumeroIncognitas];
						Columnas = new int[mMatrizPrincipal.Columnas];

						if (mMatrizPrincipal.EsCuadrada) {
							DetA = Matriz.CalculoDeterminante(mMatrizPrincipal);
							MatOr = mMatrizPrincipal.Copia();
						} else {
							MatOr = Matriz.SubMatrizPorTamaño(mMatrizPrincipal, mNumeroIncognitas, mNumeroIncognitas);
							DetA = Matriz.CalculoDeterminante(MatOr);
						}

						for (int i = 0; i <= mNumeroIncognitas - 1; i++) {
							Mat = MatOr.Copia();

							Mat.EstablecerValoresPorColumna(i, mMatrizAmpliada.ObtenerColumna(mNumeroIncognitas));

							Sols[i] = Matriz.CalculoDeterminante(Mat) / DetA;
						}

						mSolucion = new SolucionSistema(Sols);
					} else {
						mSolucion = new SolucionSistema(TipoSolucionSistema.SistemaCompatibleIndeterminado);
					}
				} else {
					mSolucion = new SolucionSistema(TipoSolucionSistema.SistemaIncompatible);
				}
			}
		}

		private bool TestHomogeneidad()
		{
			foreach (Ecuacion Ecuacion in mEcuaciones) {
				if (Ecuacion.TerminoIndependiente != 0)
					return false;
			}

			return true;
		}

		public override string ToString()
		{
			string Retorno = "";

			for (int i = 0; i <= mEcuaciones.GetUpperBound(0); i++) {
				Retorno += mEcuaciones[i].ToString() + Constants.vbNewLine;
			}

			return Retorno;
		}
	}
}
