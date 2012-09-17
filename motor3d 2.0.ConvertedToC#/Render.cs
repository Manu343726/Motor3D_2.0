using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Escena.Renders
{
	public abstract class Render : ObjetoEscena
	{

		protected Motor3D mMotor;
		protected int mAncho;

		protected int mAlto;
		public event ActualizadoEventHandler Actualizado;
		public delegate void ActualizadoEventHandler(ref Render Sender);

		public Motor3D Motor {
			get { return mMotor; }
		}

		public int Ancho {
			get { return mAncho; }
		}

		public int Alto {
			get { return mAlto; }
		}

		public Render(ref Motor3D Motor) : base()
		{
			mMotor = Motor;
			mAncho = 0;
			mAlto = 0;
		}

		public virtual void Redimensionar(int Ancho, int Alto)
		{
			mAncho = Ancho;
			mAlto = Alto;
		}

		public virtual void Iniciar()
		{
			mMotor.Actualizado += Actualizar;
		}

		public virtual void Finalizar()
		{
			mMotor.Actualizado -= Actualizar;
		}

		protected abstract void Actualizar(ref ZBuffer ZBuffer);
	}
}

