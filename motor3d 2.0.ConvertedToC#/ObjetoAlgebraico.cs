using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace Motor3D.Algebra
{
	public abstract class ObjetoAlgebraico : ObjetoBase
	{

		public new event ModificadoEventHandler Modificado;
		public new delegate void ModificadoEventHandler(ref ObjetoAlgebraico Sender);

		public ObjetoAlgebraico() : base()
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
