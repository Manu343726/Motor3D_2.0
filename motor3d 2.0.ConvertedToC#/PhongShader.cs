using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using System.Drawing;
using System.Math;

namespace Motor3D.Escena.Shading
{
	public class PhongShader : ObjetoShading
	{

		private float mAmbiente;
		private float mDifusa;
		private float mEspecular;

		private float mExponenteEspecular;
		public event AmbienteModificadaEventHandler AmbienteModificada;
		public delegate void AmbienteModificadaEventHandler(PhongShader Sender);
		public event DifusaModificadaEventHandler DifusaModificada;
		public delegate void DifusaModificadaEventHandler(PhongShader Sender);
		public event EspecularModificadaEventHandler EspecularModificada;
		public delegate void EspecularModificadaEventHandler(PhongShader Sender);
		public event ExponenteModificadoEventHandler ExponenteModificado;
		public delegate void ExponenteModificadoEventHandler(PhongShader Sender);

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref PhongShader Sender);

		public float Ambiente {
			get { return mAmbiente; }
			set {
				if (value >= 0 && value <= 1) {
					mAmbiente = value;
					if (AmbienteModificada != null) {
						AmbienteModificada(this);
					}
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public float Difusa {
			get { return mDifusa; }
			set {
				if (value >= 0 && value <= 1) {
					mDifusa = value;
					if (DifusaModificada != null) {
						DifusaModificada(this);
					}
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public float Especular {
			get { return mEspecular; }
			set {
				if (value >= 0 && value <= 1) {
					mEspecular = value;
					if (EspecularModificada != null) {
						EspecularModificada(this);
					}
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public float ExponenteEspecular {
			get { return mExponenteEspecular; }
			set {
				if (value >= 0) {
					mExponenteEspecular = value;
					if (ExponenteModificado != null) {
						ExponenteModificado(this);
					}
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public PhongShader()
		{
			mAmbiente = 1;
			mDifusa = 1;
			mEspecular = 1;
			mExponenteEspecular = 50;
		}

		public PhongShader(float Difusa, float Especular, int ExponenteEspecular)
		{
			if (Difusa >= 0 && Difusa <= 1)
				mDifusa = Difusa;
			if (Especular >= 0 && Especular <= 1)
				mEspecular = Especular;
			if (ExponenteEspecular >= 0)
				mExponenteEspecular = ExponenteEspecular;
		}

		public Color EcuacionPhong(Foco3D[] Focos, Vector3D NormalSUR, Punto3D PuntoSUR, Color Color, Camara3D Camara)
		{
			return EcuacionPhong(Focos, this, NormalSUR, PuntoSUR, Color, Camara);
		}

		public static Color EcuacionPhong(Foco3D[] Focos, PhongShader Constantes, Vector3D NormalSUR, Punto3D PuntoSUR, Color Color, Camara3D Camara)
		{
			Vector3D Rayo = null;
			Vector3D Salida = null;
			Vector3D Vista = null;
			byte r = 0;
			byte g = 0;
			byte b = 0;
			long rr = 0;
			long gg = 0;
			long bb = 0;
			long trr = 0;
			long tgg = 0;
			long tbb = 0;
			double Escalar = 0;
			double Ambiente = 0;
			double Difusa = 0;
			double Especular = 0;

			double AmbienteR = 0;
			double DifusaR = 0;
			double EspecularR = 0;
			double AmbienteG = 0;
			double DifusaG = 0;
			double EspecularG = 0;
			double AmbienteB = 0;
			double DifusaB = 0;
			double EspecularB = 0;

			trr = 0;
			tgg = 0;
			tbb = 0;

			Vista = new Vector3D(Camara.Posicion, PuntoSUR);
			Vista.Normalizar();

			Ambiente = Constantes.Ambiente;
			NormalSUR.Normalizar();

			for (long i = 0; i <= Focos.GetUpperBound(0); i++) {
				Rayo = new Vector3D(Focos[i].Coordenadas, PuntoSUR);
				Rayo = !Rayo.VectorUnitario;
				Salida = (((2 * (NormalSUR * Rayo)) * NormalSUR) - Rayo).VectorUnitario;

				Difusa = Focos[i].Intensidad * (Constantes.Difusa * (NormalSUR * Rayo));
				Escalar = (Salida * Vista);
				if (Escalar < 0) {
					Especular = Math.Abs((Constantes.Especular * Math.Pow(Escalar, Constantes.ExponenteEspecular)));
				} else {
					Especular = 0;
				}

				AmbienteR = Ambiente * (Focos[i].Color.R / 255);
				DifusaR = Difusa * (Focos[i].Color.R / 255);
				EspecularR = Especular * (Focos[i].Color.R / 255);

				AmbienteG = Ambiente * (Focos[i].Color.G / 255);
				DifusaG = Difusa * (Focos[i].Color.G / 255);
				EspecularG = Especular * (Focos[i].Color.G / 255);

				AmbienteB = Ambiente * (Focos[i].Color.B / 255);
				DifusaB = Difusa * (Focos[i].Color.B / 255);
				EspecularB = Especular * (Focos[i].Color.B / 255);

				rr = Color.R * (AmbienteR + DifusaR);
				rr = rr + ((Focos[i].Color.R - rr) * EspecularR);

				gg = Color.G * (AmbienteG + DifusaG);
				gg = gg + ((Focos[i].Color.G - gg) * EspecularG);

				bb = Color.B * (AmbienteB + DifusaB);
				bb = bb + ((Focos[i].Color.B - bb) * EspecularB);

				if (rr < 0)
					rr = 0;
				if (gg < 0)
					gg = 0;
				if (bb < 0)
					bb = 0;

				trr += rr;
				tgg += gg;
				tbb += bb;
			}

			if (trr > 255)
				trr = 255;
			if (trr < 0)
				trr = 0;

			if (tgg > 255)
				tgg = 255;
			if (tgg < 0)
				tgg = 0;

			if (tbb > 255)
				tbb = 255;
			if (tbb < 0)
				tbb = 0;

			r = trr;
			g = tgg;
			b = tbb;

			return Color.FromArgb(255, r, g, b);
		}
	}
}

