using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Math;

namespace Motor3D.Espacio3D
{
	public class Segmento3D : ObjetoGeometrico3D
	{

		private Punto3D mExtremoInicial;
		private Punto3D mExtremoFinal;
		private Recta3D mRecta;
		private double mLongitud;

		private Caja3D mCaja;
		public Punto3D ExtremoInicial {
			get { return mExtremoInicial; }
			set {
				mExtremoInicial = value;
				RecalcularDatos();
			}
		}

		public Punto3D ExtremoFinal {
			get { return mExtremoFinal; }
			set {
				mExtremoFinal = value;
				RecalcularDatos();
			}
		}

		public Recta3D Recta {
			get { return mRecta; }
		}

		public Caja3D Caja {
			get { return mCaja; }
		}

		public double Longitud {
			get { return mLongitud; }
		}

		public Punto3D PuntoMedio {
			get { return new Punto3D((mExtremoInicial.X + mExtremoFinal.X) / 2, (mExtremoInicial.Y + mExtremoFinal.Y) / 2, (mExtremoInicial.Z + mExtremoFinal.Z) / 2); }
		}

		public Segmento3D(Punto3D P1, Punto3D P2)
		{
			mExtremoInicial = P1;
			mExtremoFinal = P2;

			mLongitud = Math.Sqrt((Math.Pow((P1.X + P2.X), 2)) + (Math.Pow((P1.Y + P2.Y), 2)));
			mRecta = new Recta3D(P1, P2);

			mCaja = new Caja3D(P1, new Vector3D(P2.X - P1.X, P2.Y - P1.Y, P2.Z - P1.Z));
		}

		private void RecalcularDatos()
		{
			mRecta = new Recta3D(mExtremoInicial, mExtremoFinal);
			mCaja = new Caja3D(mExtremoInicial, new Vector3D(mExtremoFinal.X - mExtremoInicial.X, mExtremoFinal.Y - mExtremoInicial.Y, mExtremoFinal.Z - mExtremoInicial.Z));
			mLongitud = Math.Sqrt((Math.Pow((mExtremoInicial.X + mExtremoFinal.X), 2)) + (Math.Pow((mExtremoInicial.Y + mExtremoFinal.Y), 2)));
		}

		public bool Pertenece(Punto3D Punto)
		{
			return Pertenece(this, Punto);
		}

		public bool Pertenece(Segmento3D Segmento, Punto3D Punto)
		{
			return mCaja.Pertenece(Punto) && mRecta.Pertenece(Punto);
		}

		public override string ToString()
		{
			return "{Segmento. Inicio=" + mExtremoInicial.ToString() + ",Fin=" + mExtremoFinal.ToString() + "}";
		}
	}
}
