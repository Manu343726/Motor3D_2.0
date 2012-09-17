using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Espacio2D
{
	public abstract class ObjetoGeometrico2D : Motor3D.ObjetoGeometrico
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoGeometrico2D Sender);

		public ObjetoGeometrico2D() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
