using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D
{
	public abstract class ObjetoBase : object
	{

		public event ModificadoEventHandler Modificado;
		public delegate void ModificadoEventHandler(ref object Sender);

		public ObjetoBase() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

