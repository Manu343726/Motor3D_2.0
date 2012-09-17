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
	public class ExcepcionMatrizNoCuadrada : ExcepcionMatriz
	{

		public ExcepcionMatrizNoCuadrada(string Mensaje) : base(Mensaje)
		{
		}
	}
}
