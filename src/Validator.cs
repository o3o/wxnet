//-----------------------------------------------------------------------------
// wx.NET - Validator.cs
//
// The wxValidator wrapper class.
//
// Written by Jason Perkins (jason@379.com)
// (C) 2003 by 379, Inc.
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: Validator.cs,v 1.3 2004/06/10 13:59:57 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class Validator : EvtHandler
	{
		[DllImport("wx-c")] static extern IntPtr wxValidator_ctor();
		
		//---------------------------------------------------------------------
		
		public Validator()
			: base(wxValidator_ctor()) {}

		public Validator(IntPtr wxObject) 
			: base(wxObject) {}

		//---------------------------------------------------------------------
	}
}
