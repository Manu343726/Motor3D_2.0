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
	public class Vertice
	{
		private List<Arista> mAristas;

		private long mIndice;
		public Arista[] Aristas {
			get { return mAristas.ToArray(); }
		}

		public long Indice {
			get { return mIndice; }
		}

		public long Grado {
			get { return mAristas.Count; }
		}

		public bool GradoPar {
			get { return mAristas.Count % 2 == 0; }
		}

		public bool AristaValida {
			get { return Arista.PrimerVertice == mIndice || Arista.SegundoVertice == mIndice; }
		}

		public Vertice(long Indice)
		{
			if (Indice >= 0) {
				mIndice = Indice;
			} else {
				throw new ExcepcionDiscreta("VERTICE (NEW): El indice no puede ser menor que cero" + Constants.vbNewLine + "Indice=" + Indice.ToString());
			}
		}

		public void EstablecerAristas(params Arista[] Aristas)
		{
			mAristas.Clear();

			foreach (Arista Arista in Aristas) {
				if (AristaValida[Arista])
					mAristas.Add(Arista);
			}
		}

		public void AñadirArista(Arista Arista)
		{
			if (AristaValida[Arista]) {
				mAristas.Add(Arista);
			}
		}
	}
}

