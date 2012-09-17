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
	public class Ecuacion : ObjetoAlgebraico
	{


		private double[] mVariables;
		public double[] Variables {
			get { return mVariables; }
		}

		public int NumeroVariables {
			get { return mVariables.GetUpperBound(0); }
		}

		public double TerminoIndependiente {
			get { return mVariables[mVariables.GetUpperBound(0)]; }
		}

		public Ecuacion(params double[] ValVariables)
		{
			mVariables = ValVariables;
		}

		public override string ToString()
		{
			string Retorno = "";
			for (int i = 0; i <= mVariables.GetUpperBound(0); i++) {
				if (i == 0) {
					Retorno += mVariables[i].ToString() + LetraVariable(i);
				} else {
					if (i < mVariables.GetUpperBound(0)) {
						Retorno += (mVariables[i] >= 0 ? "+" : "") + mVariables[i].ToString() + LetraVariable(i);
					} else {
						Retorno += "=" + mVariables[i].ToString();
					}
				}
			}

			return Retorno;
		}

		public Ecuacion Copia()
		{
			return Copia(this);
		}

		public static Ecuacion Copia(Ecuacion Ecuacion)
		{
			return new Ecuacion(Ecuacion.Variables);
		}

		public static string LetraVariable(int IndiceVariable)
		{
			switch (IndiceVariable) {
				case 0:
					return "X";
				case 1:
					return "Y";
				case 2:
					return "Z";
				case 3:
					return "W";
				default:
					return "[VAR" + IndiceVariable + "]";
			}
		}
	}

}
