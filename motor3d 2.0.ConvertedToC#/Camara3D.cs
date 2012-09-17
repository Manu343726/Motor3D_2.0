using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;
using Motor3D.Espacio3D.Transformaciones;
using Motor3D.Espacio2D;
using Motor3D.Algebra;

namespace Motor3D.Escena
{
	public class Camara3D : ObjetoEscena
	{

		private Transformacion3D mTransformacion;
		private Transformacion3D mInversa;
		private Punto3D mPosicion;
		private Punto3D mPuntodeMira;
		private Vector3D mVectorDireccion;
		private Proyeccion mProyeccion;
		private Caja3D mFrustum;
		private Punto2D mResolucionPantalla;
		private double mDistancia;
		private Punto2D mRelacionAspecto;

		private Caja2D mPantalla;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref Camara3D Sebder);

		public Transformacion3D TransformacionSRCtoSUR {
			get { return mTransformacion; }
		}

		public Transformacion3D TransformacionSURtoSRC {
			get { return mInversa; }
		}

		public Punto3D Posicion {
			get { return mPosicion; }

			set {
				if (value != mPosicion) {
					mTransformacion += new Traslacion(mPosicion, value);
					RecalcularDatos();
				}
			}
		}

		public Vector3D VectorDireccion {
			get { return mVectorDireccion; }
		}

		public Punto3D PuntoDeMira {
			get { return mPuntodeMira; }
		}

		public Caja3D Frustum {
			get { return mFrustum; }
		}

		public Caja2D Pantalla {
			get { return mPantalla; }
		}

		public double Distancia {
			get { return mDistancia; }
			set {
				if (value > 0 && value != mDistancia) {
					mDistancia = value;
					mFrustum = new Caja3D(-(mFrustum.Ancho / 2), -(mFrustum.Largo / 2), mDistancia, mFrustum.Ancho, mFrustum.Largo, mFrustum.Alto);
					if (Modificado != null) {
						Modificado(this);
					}
				}
			}
		}

		public Punto2D ResolucionPantalla {
			get { return mResolucionPantalla; }
			set {
				if (mResolucionPantalla != value) {
					mResolucionPantalla = value;
					mRelacionAspecto = new Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo);
					mPantalla = new Caja2D(-ResolucionPantalla.X / 2, -ResolucionPantalla.Y / 2, ResolucionPantalla.X, ResolucionPantalla.Y);
				}
			}
		}

		public Poligono2D[] RepresentacionFrustum {
			get {
				Poligono2D[] Retorno = new Poligono2D[6];
				Punto2D[] Vertices = new Punto2D[8];

				Vertices[0] = Proyeccion(Frustum.Posicion);
				Vertices[1] = Proyeccion(new Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top, Frustum.Up));
				Vertices[2] = Proyeccion(new Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top + Frustum.Largo, Frustum.Up));
				Vertices[3] = Proyeccion(new Punto3D(Frustum.Left, Frustum.Top + Frustum.Largo, Frustum.Up));
				Vertices[4] = Proyeccion(new Punto3D(Frustum.Left, Frustum.Top, Frustum.Up + Frustum.Alto));
				Vertices[5] = Proyeccion(new Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top, Frustum.Up + Frustum.Alto));
				Vertices[6] = Proyeccion(new Punto3D(Frustum.Left + Frustum.Ancho, Frustum.Top + Frustum.Largo, Frustum.Up + Frustum.Alto));
				Vertices[7] = Proyeccion(new Punto3D(Frustum.Left, Frustum.Top + Frustum.Largo, Frustum.Up + Frustum.Alto));


				Retorno[0] = new Poligono2D(Vertices[3], Vertices[2], Vertices[1], Vertices[0]);
				Retorno[1] = new Poligono2D(Vertices[4], Vertices[5], Vertices[6], Vertices[7]);
				Retorno[2] = new Poligono2D(Vertices[7], Vertices[6], Vertices[2], Vertices[3]);
				Retorno[3] = new Poligono2D(Vertices[4], Vertices[7], Vertices[3], Vertices[0]);
				Retorno[4] = new Poligono2D(Vertices[5], Vertices[4], Vertices[0], Vertices[1]);
				Retorno[5] = new Poligono2D(Vertices[6], Vertices[5], Vertices[1], Vertices[2]);

				return Retorno;
			}
		}

		public void EstablecerDimensionesFrustum(double Ancho, double Largo, double Alto)
		{
			mFrustum = new Caja3D(-(Ancho / 2), -(Alto / 2), mDistancia, Ancho, Largo, Alto);
			mRelacionAspecto = new Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void EstablecerAnchoFrustum(double Ancho)
		{
			mFrustum = new Caja3D(-(Ancho / 2), -mFrustum.Largo / 2, mDistancia, Ancho, mFrustum.Largo, mFrustum.Alto);
			mRelacionAspecto = new Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void EstablecerLargoFrustum(double Largo)
		{
			mFrustum = new Caja3D(-(mFrustum.Ancho / 2), -Largo / 2, mDistancia, mFrustum.Ancho, Largo, mFrustum.Alto);
			mRelacionAspecto = new Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void EstablecerAltoFrustum(double Alto)
		{
			mFrustum = new Caja3D(-(mFrustum.Ancho / 2), -mFrustum.Largo / 2, mDistancia, mFrustum.Ancho, mFrustum.Largo, Alto);
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public Punto2D Proyeccion(Punto3D Punto, bool EstaTransformado = false)
		{
			Punto3D P = null;
			double k = 0;

			if (EstaTransformado) {
				k = mDistancia / Punto.Z;
				return new Punto2D(1 * (Punto.X * k), 1 * (Punto.Y * k));
			//Return New Punto2D(Punto.X, Punto.Y)
			} else {
				P = mInversa * Punto;
				k = mDistancia / P.Z;
				return new Punto2D(1 * (P.X * k), 1 * (P.Y * k));
				//Return New Punto2D(P.X, P.Y)
			}
		}

		private void RecalcularDatos()
		{
			mInversa = new Transformacion3D(Matriz.CalculoInversa(mTransformacion.Matriz));
			mPosicion = mTransformacion * new Punto3D();
			mPuntodeMira = mTransformacion * new Punto3D(0, 0, 1);
			mVectorDireccion = new Vector3D(mPosicion, mPuntodeMira);
		}

		public void TrasladarSobreSUR(Vector3D Traslacion)
		{
			mTransformacion = mTransformacion + new Traslacion(Traslacion);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void TrasladarSobreSUR(Punto3D Destino)
		{
			mTransformacion += new Traslacion(mPosicion, Destino);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void TrasladarSobreSRC(Vector3D Traslacion)
		{
			mTransformacion = new Traslacion(Traslacion) + mTransformacion;
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarSobreSUR(EnumEjes Eje, float Rotacion)
		{
			mTransformacion += new Rotacion(Rotacion, Eje);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarFijoSobreSUR(EnumEjes Eje, float Rotacion)
		{
			mTransformacion += new Rotacion(Rotacion, Eje, mPosicion);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarSobreSRC(EnumEjes Eje, float Rotacion)
		{
			mTransformacion = new Rotacion(Rotacion, Eje) + mTransformacion;
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarSobreSUR(Recta3D Eje, float Rotacion)
		{
			mTransformacion += new Rotacion(Rotacion, Eje);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarSobreSUR(Vector3D Eje, float Rotacion)
		{
			mTransformacion += new Rotacion(Rotacion, Eje);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarFijoSobreSUR(Vector3D Eje, float Rotacion)
		{
			mTransformacion += new Rotacion(Rotacion, Eje, mPosicion);
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public void RotarSobreSRC(Vector3D Eje, float Rotacion)
		{
			mTransformacion = new Rotacion(Rotacion, Eje) + mTransformacion;
			RecalcularDatos();
			if (Modificado != null) {
				Modificado(this);
			}
		}

		public bool EsVisible(Punto3D Punto)
		{
			return mFrustum.Pertenece(Punto);
		}

		public Camara3D()
		{
			mTransformacion = new Transformacion3D();
			mInversa = new Transformacion3D();
			mPosicion = new Punto3D();
			mPuntodeMira = new Punto3D(0, 0, 1);
			mVectorDireccion = new Vector3D(0, 0, 1);
			mDistancia = 1000;
			mFrustum = new Caja3D(-50000, -50000, 1000, 100000, 100000, 100000);
			mResolucionPantalla = new Punto2D(800, 600);
			mRelacionAspecto = new Punto2D(mResolucionPantalla.X / mFrustum.Ancho, mResolucionPantalla.Y / mFrustum.Largo);
			mPantalla = new Caja2D(-ResolucionPantalla.X / 2, -ResolucionPantalla.Y / 2, ResolucionPantalla.X, ResolucionPantalla.Y);
		}

		public override string ToString()
		{
			return "{Camara3D. Posicion=" + mPosicion.ToString() + " , Direccion=" + mVectorDireccion.ToString() + "}";
		}

		public static bool operator ==(Camara3D C1, Camara3D C2)
		{
			return C1.Equals(C2);
		}

		public static bool operator !=(Camara3D C1, Camara3D C2)
		{
			return !(C1 == C2);
		}
	}
}

