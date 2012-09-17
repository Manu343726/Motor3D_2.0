using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Discreta
{
	public class Arista
	{
		private long mPrimerVertice;

		private long mSegundoVertice;
		public long PrimerVertice {
			get { return mPrimerVertice; }
		}

		public long SegundoVertice {
			get { return mSegundoVertice; }
		}

		public Arista(long PrimerVertice, long SegundoVertice)
		{
			if (PrimerVertice >= 0) {
				if (SegundoVertice > 0) {
					mPrimerVertice = PrimerVertice;
					mSegundoVertice = SegundoVertice;
				} else {
					throw new ExcepcionDiscreta("ARISTA (NEW): El segundo vertice no puede ser menor que cero" + Constants.vbNewLine + "Primer vertice=" + PrimerVertice.ToString() + Constants.vbNewLine + "Segundo vertice=" + SegundoVertice.ToString());
				}
			} else {
				throw new ExcepcionDiscreta("ARISTA (NEW): El primer vertice no puede ser menor que cero" + Constants.vbNewLine + "Primer vertice=" + PrimerVertice.ToString() + Constants.vbNewLine + "Segundo vertice=" + SegundoVertice.ToString());
			}
		}
	}
}

