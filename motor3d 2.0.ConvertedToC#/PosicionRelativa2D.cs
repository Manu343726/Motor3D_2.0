using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;


namespace Motor3D.Espacio2D
{
	public enum TipoPosicionRelativa2D
	{
		Secante,
		Coincidente,
		Paralelo
	}

	public class PosicionRelativa2D : ObjetoGeometrico2D
	{

		private TipoPosicionRelativa2D mTipo;

		private Punto2D mInterseccion;
		public TipoPosicionRelativa2D Tipo {
			get { return mTipo; }
		}

		public Punto2D Interseccion {
			get {
				if (mTipo == TipoPosicionRelativa2D.Secante) {
					return mInterseccion;
				} else {
					throw new ExcepcionGeometrica2D("POSICIONRELATIVA2D (INTERSECCION_GET): La posición no es de tipo secante. No se puede obtener la intersección." + Constants.vbNewLine + "Tipo=" + mTipo.ToString());
				}
			}
		}

		public PosicionRelativa2D(Punto2D ValInterseccion)
		{
			mTipo = TipoPosicionRelativa2D.Secante;
			mInterseccion = ValInterseccion;
		}

		public PosicionRelativa2D(TipoPosicionRelativa2D ValTipoPosicion)
		{
			mTipo = ValTipoPosicion;
		}

		public override string ToString()
		{
			if (mTipo == TipoPosicionRelativa2D.Secante) {
				return "{" + mTipo.ToString() + ",Intersección=" + mInterseccion.ToString() + "}";
			} else {
				return "{" + mTipo.ToString() + "}";
			}
		}
	}
}

