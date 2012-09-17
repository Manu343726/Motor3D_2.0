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
	public class ExcepcionPrimitiva3D : ExcepcionGeometrica3D
	{

		public ExcepcionPrimitiva3D(string Mensaje) : base(Mensaje)
		{
		}
	}
}

