using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Espacio3D
{
	public enum TipoPosicionRelativa3D
	{
		Secante,
		Coincidente,
		Paralelo,
		Cruce
	}

	public class PosicionRelativa3D : ObjetoGeometrico3D
	{

		private TipoPosicionRelativa3D mTipo;

		private Punto3D mInterseccion;
		public TipoPosicionRelativa3D Tipo {
			get { return mTipo; }
		}

		public Punto3D Interseccion {
			get {
				if (mTipo == TipoPosicionRelativa3D.Secante) {
					return mInterseccion;
				} else {
					throw new ExcepcionGeometrica3D("POSICIONRELATIVA3D (INTERSECCION_GET): La posición no es de tipo secante. No se puede obtener la intersección." + Constants.vbNewLine + "Tipo=" + mTipo.ToString());
				}
			}
		}

		public PosicionRelativa3D(Punto3D ValInterseccion)
		{
			mTipo = TipoPosicionRelativa3D.Secante;
			mInterseccion = ValInterseccion;
		}

		public PosicionRelativa3D(TipoPosicionRelativa3D ValTipoPosicion)
		{
			mTipo = ValTipoPosicion;
		}

		public override string ToString()
		{
			if (mTipo == TipoPosicionRelativa3D.Secante) {
				return "{" + mTipo.ToString() + ",Intersección=" + mInterseccion.ToString() + "}";
			} else {
				return "{" + mTipo.ToString() + "}";
			}
		}
	}
}
