using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Escena.Shading
{
	public abstract class ObjetoShading : ObjetoEscena
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoEscena Sender);

		public ObjetoShading() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

