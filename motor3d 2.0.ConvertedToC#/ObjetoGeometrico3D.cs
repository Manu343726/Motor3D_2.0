using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Espacio3D
{
	public abstract class ObjetoGeometrico3D : ObjetoGeometrico
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoGeometrico3D Sender);

		public ObjetoGeometrico3D() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
