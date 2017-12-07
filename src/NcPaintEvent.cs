//-----------------------------------------------------------------------------
// wx.NET - NCPaintEvent.cs
//
// The wxNCPaintEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: NcPaintEvent.cs,v 1.1 2004/10/17 10:39:12 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class NCPaintEvent : Event
	{
		[DllImport("wx-c")] static extern IntPtr wxNcPaintEvent_ctor(int Id);
		
		//-----------------------------------------------------------------------------

		public NCPaintEvent(IntPtr wxObject) 
			: base(wxObject) { }

		public NCPaintEvent(int Id)
			: this(wxNcPaintEvent_ctor(Id)) { }
	}
}
