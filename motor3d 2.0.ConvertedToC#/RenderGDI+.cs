using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio2D;
using System.Drawing;
using System.Windows.Forms;

namespace Motor3D.Escena.Renders.GDI
{
	public class RenderGDI : Render
	{

		protected Graphics g;
		protected Bitmap BMP;
		protected SolidBrush b;

		protected Pen p;
		protected IntPtr mHandle;

		protected PictureBox mCanvas;

		protected bool mAutoRedimensionar;

		protected bool mRendering;
		public new event ActualizadoEventHandler Actualizado;
		public new delegate void ActualizadoEventHandler(ref RenderGDI sender);

		public Bitmap Bitmap {
			get { return BMP; }
		}

		public IntPtr CanvasHandle {
			get { return mHandle; }
		}

		public Control Canvas {
			get { return mCanvas; }
		}

		public bool AutoRedimensionar {
			get { return mAutoRedimensionar; }
			set {
				if (value != mAutoRedimensionar) {
					mAutoRedimensionar = value;

					if (value) {
						mCanvas.Resize += ControladorRedimension;
					} else {
						mCanvas.Resize -= ControladorRedimension;
					}
				}
			}
		}

		public bool Rendering {
			get { return mRendering; }
		}

		public RenderGDI(ref Motor3D Motor, ref IntPtr Handle, bool AutoRedimensionar = false) : base(Motor)
		{

			mHandle = Handle;
			mCanvas = Control.FromHandle(mHandle);
			mAncho = mCanvas.Width;
			mAlto = mCanvas.Height;
			mAutoRedimensionar = AutoRedimensionar;
			mRendering = false;
			p = new Pen(Brushes.White, 1);
			b = new SolidBrush(Color.White);
		}

		protected override void Actualizar(ref ZBuffer ZBuffer)
		{
			if (!ZBuffer.Vacio) {
				g.Clear(Color.Black);

				foreach (Poligono2D Poligono in ZBuffer.Represenatciones) {
					b.Color = Poligono.Color;

					g.FillPolygon(b, Poligono.VerticesToPoint);
				}

				mCanvas.Refresh();
				if (Actualizado != null) {
					Actualizado(this);
				}
			}
		}

		private void ControladorRedimension(object sender, EventArgs e)
		{
			Redimensionar(mCanvas.Width, mCanvas.Height);
		}

		public override void Redimensionar(int Ancho, int Alto)
		{
			base.Redimensionar(Ancho, Alto);
			if (mRendering)
				ResetGraficos();
		}

		private void ResetGraficos()
		{
			BMP = new Bitmap(mAncho, mAlto);
			g = Graphics.FromImage(BMP);
			g.TranslateTransform(mAncho / 2, mAlto / 2);
			mCanvas.Image = BMP;
		}

		public override void Iniciar()
		{
			mRendering = true;
			ResetGraficos();
			base.Iniciar();
		}

		public override void Finalizar()
		{
			base.Finalizar();
			BMP = null;
			g = null;
			mRendering = false;
		}
	}
}

