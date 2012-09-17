using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Algebra;
using System.Math;

namespace Motor3D.Espacio3D
{
	public class Recta3D : ObjetoGeometrico3D
	{

		private Plano3D PlanoA;
		private Plano3D PlanoB;
		private Vector3D mVector;
		private Punto3D mPunto;

		private Punto3D mPuntoMira;
		public Plano3D PrimerPlano {
			get { return PlanoA; }
		}

		public Plano3D SegundoPlano {
			get { return PlanoB; }
		}

		public Vector3D VectorDirector {
			get { return mVector; }
		}

		public Punto3D PuntoInicial {
			get { return mPunto; }
		}

		public Punto3D PuntoMira {
			get { return mPuntoMira; }
		}

		public Matriz[] Matrices {
			get { return RepresentacionMatricial(this); }
		}

		public Punto3D ObtenerPuntoParametrico(double Delta)
		{
			return new Punto3D(mPunto.X + (Delta * mVector.X), mPunto.Y + (Delta * mVector.Y), mPunto.Z + (Delta * mVector.Z));
		}

		public double ObtenerParametro(double Valor, EnumEjes Coordenada)
		{
			switch (Coordenada) {
				case EnumEjes.EjeX:
					if (mVector.X != 0) {
						return (Valor - mPunto.X) / mVector.X;
					} else {
						throw new ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." + Constants.vbNewLine + "Coordenada: " + Coordenada.ToString() + Constants.vbNewLine + "Vector direccion: " + mVector.ToString());
					}
					break;
				case EnumEjes.EjeY:
					if (mVector.Y != 0) {
						return (Valor - mPunto.Y) / mVector.Y;
					} else {
						throw new ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." + Constants.vbNewLine + "Coordenada: " + Coordenada.ToString() + Constants.vbNewLine + "Vector direccion: " + mVector.ToString());
					}
					break;
				case EnumEjes.EjeZ:
					if (mVector.Z != 0) {
						return (Valor - mPunto.Z) / mVector.Z;
					} else {
						throw new ExcepcionGeometrica3D("RECTA3D (OBTENERPARAMETRO): Division por cero." + Constants.vbNewLine + "Coordenada: " + Coordenada.ToString() + Constants.vbNewLine + "Vector direccion: " + mVector.ToString());
					}
					break;
			}
		}

		public Punto3D ObtenerPunto(double Valor, EnumEjes Coordenada)
		{
			return ObtenerPuntoParametrico(ObtenerParametro(Valor, Coordenada));
		}

		public Punto3D ObtenerPunto(double X)
		{
			Ecuacion ec1 = null;
			Ecuacion ec2 = null;
			SistemaEcuaciones sis = null;

			ec1 = new Ecuacion(PlanoA.B, PlanoA.C, -(PlanoA.D + (PlanoA.A * X)));
			ec2 = new Ecuacion(PlanoB.B, PlanoB.C, -(PlanoB.D + (PlanoB.A * X)));

			sis = new SistemaEcuaciones(ec1, ec2);

			if (sis.Solucion.TipoSolucion == TipoSolucionSistema.SistemaCompatibleDeterminado) {
				return new Punto3D(X, sis.Solucion.ValorSolucion[0], sis.Solucion.ValorSolucion[1]);
			} else {
				throw new ExcepcionGeometrica3D("RECTA3D (OBTENERPUNTO): No se ha podido calcular el punto. Es posible que los datos de los planos sean erroneos, o que el cálculo del sistema halla fallado." + Constants.vbNewLine + "Valor de la variable=" + X.ToString() + Constants.vbNewLine + "Primer plano: " + PlanoA.ToString() + Constants.vbNewLine + "Seundo plano: " + PlanoB.ToString() + Constants.vbNewLine + "Primera ecuación del sistema: " + ec1.ToString() + Constants.vbNewLine + "Segunda ecuación del sistema: " + ec2.ToString() + Constants.vbNewLine + "Solución obtenida: " + sis.Solucion.ToString());
			}
		}

		public Recta3D(Plano3D P1, Plano3D P2)
		{
			PlanoA = P1;
			PlanoB = P2;
			mVector = (P1.VectorNormal + P2.VectorNormal).VectorUnitario;
			mPunto = ObtenerPuntoParametrico(0);
			mPuntoMira = ObtenerPuntoParametrico(10);
		}

		public Recta3D(Punto3D P1, Punto3D P2) : this(P1, new Vector3D(P1, P2))
		{
		}

		public Recta3D(params Matriz[] Matrices)
		{
			if (Matrices.GetUpperBound(0) == 1) {
				Punto3D Punto = new Punto3D(Matrices[0]);
				Punto3D Punto2 = new Punto3D(Matrices[1]);

				mPunto = Punto;
				mPuntoMira = Punto2;
				mVector = new Vector3D(mPunto, mPuntoMira).VectorUnitario;

				//ÉSTOS VALORES SE OBTIENEN AL DESPEJAR LA ECUACIÓN PARAMÉTRICA DE UNA RECTA GENÉRICA:
				PlanoA = new Plano3D(mVector.Y, -mVector.X, 0, (-mVector.Y * Punto.X) + (mVector.X * Punto.Y));
				PlanoB = new Plano3D(0, mVector.Z, -mVector.Y, (-mVector.Z * Punto.Y) + (mVector.Y * Punto.Z));
			} else {
				throw new ExcepcionGeometrica3D("RECTA3D (NEW): La representación matricial de una recta corresponde a un array con dos matrices" + Constants.vbNewLine + "Tamaño del array=" + Matrices.GetUpperBound(0) + 1);
			}
		}

		public Recta3D(Punto3D Punto, Vector3D Vector)
		{
			mPunto = Punto;
			mPuntoMira = new Punto3D(mPunto.X + Vector.X, mPunto.Y + Vector.Y, mPunto.Z + Vector.Z);
			mVector = Vector.VectorUnitario;

			//ÉSTOS VALORES SE OBTIENEN AL DESPEJAR LA ECUACIÓN PARAMÉTRICA DE UNA RECTA GENÉRICA:
			PlanoA = new Plano3D(Vector.Y, -Vector.X, 0, (-Vector.Y * Punto.X) + (Vector.X * Punto.Y));
			PlanoB = new Plano3D(0, Vector.Z, -Vector.Y, (-Vector.Z * Punto.Y) + (Vector.Y * Punto.Z));
		}

		public bool Pertenece(Punto3D Punto)
		{
			return PlanoA.Pertenece(Punto) && PlanoB.Pertenece(Punto);
		}

		public override string ToString()
		{
			return "{Recta " + PlanoA.ToString() + " & " + PlanoB.ToString() + "}";
		}

		public static Matriz[] RepresentacionMatricial(Recta3D Recta)
		{
			Matriz[] Retorno = new Matriz[2];

			Retorno[0] = Recta.PuntoInicial.Matriz;
			Retorno[1] = Recta.PuntoMira.Matriz;

			return Retorno;
		}

		public static PosicionRelativa3D PosicionRelativa(Recta3D R1, Recta3D R2)
		{
			SistemaEcuaciones Sistema = new SistemaEcuaciones(new Ecuacion(R1.VectorDirector.X, R2.VectorDirector.X, R2.PuntoInicial.X - R1.PuntoInicial.Y), new Ecuacion(R1.VectorDirector.Y, R2.VectorDirector.Y, R2.PuntoInicial.Y - R1.PuntoInicial.Y), new Ecuacion(R1.VectorDirector.Z, R2.VectorDirector.Z, R2.PuntoInicial.Z - R1.PuntoInicial.Z));

			switch (Sistema.Solucion.TipoSolucion) {
				case TipoSolucionSistema.SistemaCompatibleDeterminado:
					return new PosicionRelativa3D(new Punto3D(Sistema.Solucion.ValorSolucion[0], Sistema.Solucion.ValorSolucion[1], Sistema.Solucion.ValorSolucion[2]));
				case TipoSolucionSistema.SistemaCompatibleIndeterminado:
					return new PosicionRelativa3D(TipoPosicionRelativa3D.Coincidente);
				case TipoSolucionSistema.SistemaIncompatible:
					if (Sistema.RangoMatrizPrincipal == 1) {
						return new PosicionRelativa3D(TipoPosicionRelativa3D.Paralelo);
					} else {
						return new PosicionRelativa3D(TipoPosicionRelativa3D.Cruce);
					}
					break;
			}
		}

		public static double Distancia(Recta3D Recta, Punto3D Punto)
		{
			return (new Vector3D(Recta.ObtenerPuntoParametrico(0), Punto) + Recta.VectorDirector).Modulo;
		}

		public static double Distancia(Recta3D Recta, Plano3D Plano)
		{
			if (Recta.VectorDirector * Plano.VectorNormal == 0) {
				return Distancia(Recta, Plano.ObtenerPunto(0, 0));
			} else {
				return 0;
			}
		}

		public static double Distancia(Recta3D R1, Recta3D R2)
		{
			switch (PosicionRelativa(R1, R2).Tipo) {
				case TipoPosicionRelativa3D.Coincidente:
					return 0;
				case TipoPosicionRelativa3D.Secante:
					return 0;
				case TipoPosicionRelativa3D.Paralelo:
					return Distancia(R1, R2.ObtenerPuntoParametrico(0));
				case TipoPosicionRelativa3D.Cruce:
					return Math.Abs(Vector3D.ProductoMixto(R1.VectorDirector, R2.VectorDirector, new Vector3D(R1.ObtenerPuntoParametrico(0), R2.ObtenerPuntoParametrico(0)))) / (R1.VectorDirector + R2.VectorDirector).Modulo;
			}
		}

		public static Punto3D Proyeccion(Punto3D Punto, Recta3D Recta)
		{
			int Landa = 0;
			double Vx = 0;
			double Vy = 0;
			double Vz = 0;
			double VVx = 0;
			double VVy = 0;
			double VVz = 0;
			double Qx = 0;
			double Qy = 0;
			double Qz = 0;
			double x = 0;
			double y = 0;
			double z = 0;

			Vx = Recta.VectorDirector.X;
			Vy = Recta.VectorDirector.Y;
			Vz = Recta.VectorDirector.Z;

			VVx = Vx * Vx;
			VVy = Vy * Vy;
			VVz = Vz * Vz;

			Qx = Recta.PuntoInicial.X;
			Qy = Recta.PuntoInicial.Y;
			Qz = Recta.PuntoInicial.Z;

			x = Punto.X;
			y = Punto.Y;
			z = Punto.Z;

			Landa = ((Vx * (x - Qx)) + (Vy * (y - Qy)) + (Vz * (z - Qz))) / (VVx + VVy + VVz);

			return Recta.ObtenerPuntoParametrico(Landa);
		}
	}
}

