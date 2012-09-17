using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Timers;
using System.Drawing;
using System.Math;

namespace Motor3D.Espacio2D
{
	public class Figura2D : Poligono2D
	{

		protected Transformacion2D Transformacion;
		protected Transformacion2D Giro;
		protected Transformacion2D Movimiento;
		protected Transformacion2D Centro;
		protected Transformacion2D VueltaCentro;
		protected Timer Timer;
		protected int Frecuencia;
		protected Vector2D mVelocidad;
		protected float mVelocidadAngular;

		protected double mRozamiento;

		protected bool mRozamientoActivado;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Figura2D Sebder);

		public Punto2D Baricentro {
			get { return Punto2D.Baricentro(base.Vertices); }
		}

		public int FrecuenciaActualizacion {
			get { return Frecuencia; }
			set {
				if (value > 0 && value <= 1000) {
					Frecuencia = value;
					Timer.Interval = 1000 / value;
				}

			}
		}

		public Vector2D Velocidad {
			get { return mVelocidad; }
			set {
				mVelocidad = value;
				Movimiento = Transformacion2D.Traslacion(value);
			}
		}

		public float VelocidadAngular {
			get { return mVelocidadAngular; }
			set {
				mVelocidadAngular = value;
				Giro = Transformacion2D.Rotacion(value);
			}
		}

		public double Rozamiento {
			get { return mRozamiento; }

			set {
				if (value >= 0)
					mRozamiento = value;
				else
					mRozamiento = 0;
			}
		}

		public bool RozamientoActivado {
			get { return mRozamientoActivado; }
			set { mRozamientoActivado = value; }
		}

		public bool AutoActualizar {
			get { return Timer.Enabled; }
			set { Timer.Enabled = value; }
		}

		public Figura2D(params Punto2D[] Vertices) : base(Vertices)
		{
			Transformacion = new Transformacion2D();
			Movimiento = new Transformacion2D();
			Giro = new Transformacion2D();
			Centro = new Transformacion2D();
			Timer = new Timer();
			Timer.Interval = 10;
			Timer.Enabled = false;
			Frecuencia = 100;
			mVelocidad = new Vector2D();
			mVelocidadAngular = 0;
			mRozamiento = 0;
			mRozamientoActivado = false;
			Actualizar();

			Timer.Elapsed += TimerTick;
		}

		public Figura2D(params Segmento2D[] Lados) : base(Lados)
		{
			Transformacion = new Transformacion2D();
			Movimiento = new Transformacion2D();
			Giro = new Transformacion2D();
			Centro = new Transformacion2D();
			Timer = new Timer();
			Timer.Interval = 10;
			Timer.Enabled = false;
			Frecuencia = 100;
			mVelocidad = new Vector2D();
			mVelocidadAngular = 0;
			mRozamiento = 0;
			mRozamientoActivado = false;
			Actualizar();

			Timer.Elapsed += TimerTick;
		}

		private void TimerTick()
		{
			Actualizar();
		}

		public void Actualizar()
		{
			Punto2D Bar = Baricentro;
			Punto2D Punto = null;
			double maxX = 0;
			double MaxY = 0;
			double minX = 0;
			double minY = 0;

			if (mRozamientoActivado) {
				if (Math.Abs(mVelocidad.X) - mRozamiento >= 0)
					mVelocidad.X += (-Math.Sign(mVelocidad.X) * mRozamiento);
				else
					mVelocidad.X = 0;
				if (Math.Abs(mVelocidad.Y) - mRozamiento >= 0)
					mVelocidad.Y += (-Math.Sign(mVelocidad.Y) * mRozamiento);
				else
					mVelocidad.Y = 0;
				if (Math.Abs(mVelocidadAngular) - mRozamiento >= 0)
					mVelocidadAngular += (-Math.Sign(mVelocidadAngular) * mRozamiento);
				else
					mVelocidadAngular = 0;

				Movimiento = Transformacion2D.Traslacion(mVelocidad);
				Giro = Transformacion2D.Rotacion(mVelocidadAngular);
			}

			Centro = Transformacion2D.Traslacion(-Bar.X, -Bar.Y);
			VueltaCentro = Transformacion2D.Traslacion(Bar.X, Bar.Y);

			Transformacion = Centro + Giro + VueltaCentro + Movimiento;

			for (int i = 0; i <= base.Segmentos.GetUpperBound(0); i++) {
				Punto = Transformacion * base.Segmentos[i].ExtremoInicial;

				if (i == 0) {
					minX = Punto.X;
					minY = Punto.Y;
					maxX = Punto.X;
					MaxY = Punto.Y;

					base.Segmentos[i].ExtremoInicial = Punto;
				} else {
					if (i < base.Segmentos.GetUpperBound(0)) {
						base.Segmentos[i - 1].ExtremoFinal = Punto;
						base.Segmentos[i].ExtremoInicial = Punto;
					} else {
						base.Segmentos[i - 1].ExtremoFinal = Punto;
						base.Segmentos[i].ExtremoInicial = Punto;
						base.Segmentos[i].ExtremoFinal = base.Segmentos[0].ExtremoInicial;
					}
				}

				if (Punto.X < minX)
					minX = Punto.X;
				if (Punto.X > maxX)
					maxX = Punto.X;
				if (Punto.Y < minY)
					minY = Punto.Y;
				if (Punto.Y > MaxY)
					MaxY = Punto.Y;
			}

			mCaja = new Caja2D(minX, minY, Math.Abs(maxX - minX), Math.Abs(MaxY - minY));

			if (Modificado != null) {
				Modificado(this);
			}
		}

		public static bool Colisionan(Figura2D F1, Figura2D F2, int NivelesBSP, bool BSP = false)
		{
			if (Caja2D.Colisionan(F1.Caja, F2.Caja)) {
				for (int i = 0; i <= F1.Lados.GetUpperBound(0); i++) {
					if (Caja2D.Colisionan(F1.Lados[i].Caja, F2.Caja)) {
						for (int j = 0; j <= F2.Lados.GetUpperBound(0); j++) {
							if (Caja2D.Colisionan(F1.Caja, F2.Lados[j].Caja)) {
								if (Segmento2D.Colisionan(F1.Lados[i], F2.Lados[j], NivelesBSP, BSP)) {
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}
	}
}


