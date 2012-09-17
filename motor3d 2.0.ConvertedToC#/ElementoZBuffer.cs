using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Escena
{
	public class ElementoZBuffer : ObjetoEscena, IComparable<ElementoZBuffer>
	{

		private double[] mIndices;
		private int mNumeroIndices;

		private double mZ;
		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref object Sender);

		public int NumeroIndices {
			get { return mNumeroIndices; }
		}

		public double[] Indices {
			get { return mIndices; }
			set {
				if (value.GetUpperBound(0) == mIndices.GetUpperBound(0)) {
					mIndices = value;
				}
			}
		}

		public double Indices {
			get {
				if (Index >= 0 && Index < mNumeroIndices) {
					return mIndices[Index];
				} else {
					throw new ExcepcionEscena("ELEMENTOZBUFFER (INDICE_GET): El índice está fuera del intervalo." + Constants.vbNewLine + "Index=" + Index.ToString() + Constants.vbNewLine + "Intervalo=(0," + mNumeroIndices - 1 + ")");
				}
			}
			set {
				if (Index >= 0 && Index < mNumeroIndices) {
					mIndices[Index] = value;
				} else {
					throw new ExcepcionEscena("ELEMENTOZBUFFER (INDICE_SET): El índice está fuera del intervalo." + Constants.vbNewLine + "Index=" + Index.ToString() + Constants.vbNewLine + "Intervalo=(0," + mNumeroIndices - 1 + ")");
				}
			}
		}

		public double Z {
			get { return mZ; }
			set {
				mZ = value;
				if (Modificado != null) {
					Modificado(this);
				}
			}
		}

		public object EsEquivalente(params double[] Indices)
		{
			return mIndices.Equals(Indices);
		}

		public ElementoZBuffer(int NumeroIndices)
		{
			if (NumeroIndices > 0) {
				mIndices = new double[NumeroIndices];
				mNumeroIndices = NumeroIndices;
			} else {
				throw new ExcepcionEscena("ELEMENTOZBUFFER (NEW): El número de índices debe ser mayor o igual que 1." + Constants.vbNewLine + "Numero de índices=" + NumeroIndices.ToString());
			}
		}

		public ElementoZBuffer(double Z, params double[] Indices)
		{
			mIndices = Indices;
			mNumeroIndices = Indices.GetUpperBound(0) + 1;
			mZ = Z;
		}

		public static bool operator >(ElementoZBuffer X, ElementoZBuffer Y)
		{
			return X.Z > Y.Z;
		}

		public static bool operator <(ElementoZBuffer X, ElementoZBuffer Y)
		{
			return X.Z < Y.Z;
		}

		public static bool operator ==(ElementoZBuffer X, ElementoZBuffer Y)
		{
			return X.Z == Y.Z;
		}

		public static bool operator !=(ElementoZBuffer X, ElementoZBuffer Y)
		{
			return X.Z != Y.Z;
		}

		public int CompareTo(ElementoZBuffer other)
		{
			switch (other.Z) {
				case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
mZ:
					return 1;
				case  // ERROR: Case labels with binary operators are unsupported : Equality
mZ:
					return 0;
				case  // ERROR: Case labels with binary operators are unsupported : LessThan
mZ:
					return -1;
			}
		}
	}
}

