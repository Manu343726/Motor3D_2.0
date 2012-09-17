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
	public abstract class ObjetoEscena : global::Motor3D.ObjetoBase
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoEscena Sender);

		public ObjetoEscena() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

