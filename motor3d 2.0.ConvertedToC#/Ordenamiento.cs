using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Utilidades
{
	public class Ordenamiento
	{
		public Ordenamiento() : base()
		{
		}

		public static void BubbleSort<Tipo>(ref Tipo[] Array) where Tipo : IComparable
		{
			Tipo inter = default(Tipo);
			for (int i = 0; i <= Array.GetUpperBound(0); i++) {
				for (int j = 0; j <= Array.GetUpperBound(0); j++) {
					if (Array[i].CompareTo(Array[j]) == 1) {
						inter = Array[i];
						Array[i] = Array[j];
						Array[j] = inter;
					}
				}
			}
		}

		public static void Sort<Tipo>(ref Tipo[] Array) where Tipo : IComparable<Tipo>
		{
			System.Array.Sort(Array);
		}
	}
}
