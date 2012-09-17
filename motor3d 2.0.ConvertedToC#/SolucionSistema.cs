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
	public enum TipoSolucionSistema
	{
		SistemaCompatibleDeterminado,
		SistemaCompatibleIndeterminado,
		SistemaIncompatible
	}

	public class SolucionSistema : ObjetoAlgebraico
	{

		private double[] mSolucionCompatibleDeterminado;

		private TipoSolucionSistema mTipoSolucion;
		public TipoSolucionSistema TipoSolucion {
			get { return mTipoSolucion; }
		}

		public double[] ValorSolucion {
			get { return mSolucionCompatibleDeterminado; }
		}

		public SolucionSistema(params double[] ValSolucion)
		{
			mTipoSolucion = TipoSolucionSistema.SistemaCompatibleDeterminado;
			mSolucionCompatibleDeterminado = ValSolucion;
		}

		public SolucionSistema(TipoSolucionSistema ValTipoSolucion)
		{
			mTipoSolucion = ValTipoSolucion;
		}

		public override string ToString()
		{
			string Retorno = "";

			if (mTipoSolucion == TipoSolucionSistema.SistemaCompatibleDeterminado) {
				Retorno = "{Sistema compatible determinado.(";
				for (int i = 0; i <= mSolucionCompatibleDeterminado.GetUpperBound(0); i++) {
					if (i < mSolucionCompatibleDeterminado.GetUpperBound(0)) {
						Retorno += mSolucionCompatibleDeterminado[i].ToString() + ",";
					} else {
						Retorno += mSolucionCompatibleDeterminado[i].ToString() + ")}";
					}
				}

				return Retorno;
			} else {
				return "{" + mTipoSolucion.ToString() + "}";
			}
		}
	}
}
