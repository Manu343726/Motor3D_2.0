using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Motor3D.Espacio3D;

namespace Motor3D.Primitivas3D
{
	public class ObjetoPrimitiva3D : ObjetoGeometrico3D
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoPrimitiva3D Sender);

		public ObjetoPrimitiva3D() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

