using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Math;

namespace Motor3D.Algebra
{
	public struct AngulosEuler
	{
		private float mCabeceo;
		private float mAlabeo;

		private float mGuiñada;
		public float Cabeceo {
			get { return mCabeceo; }
			set { mCabeceo = value; }
		}

		public float Alabeo {
			get { return mAlabeo; }
			set { mAlabeo = value; }
		}

		public float Guiñada {
			get { return mGuiñada; }
			set { mGuiñada = value; }
		}

		public Cuaternion Cuaternion {
			get { return ObtenerCuaternion(this); }
		}

		public AngulosEuler(float Cabeceo, float Alabeo, float Guiñada)
		{
			mCabeceo = Cabeceo;
			mAlabeo = Alabeo;
			mGuiñada = Guiñada;
		}

		public static Cuaternion ObtenerCuaternion(AngulosEuler Angulos)
		{
			return new Cuaternion(Angulos.Cabeceo, Angulos.Alabeo, Angulos.Guiñada);
		}

		public override string ToString()
		{
			return "{AngulosEuler: " + mCabeceo.ToString() + "," + mAlabeo.ToString() + "," + mGuiñada.ToString() + "}";
		}
	}
}

