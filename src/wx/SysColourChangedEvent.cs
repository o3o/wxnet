//-----------------------------------------------------------------------------
// wx.NET - SysColourChangedEvent.cs
//
// The wxSysColourChangedEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: SysColourChangedEvent.cs,v 1.1 2004/10/17 17:47:42 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class SysColourChangedEvent : Event
	{
		[DllImport("wx-c")] static extern IntPtr wxSysColourChangedEvent_ctor();
		
		//-----------------------------------------------------------------------------

		public SysColourChangedEvent(IntPtr wxObject) 
			: base(wxObject) { }

		public SysColourChangedEvent()
			: this(wxSysColourChangedEvent_ctor()) { }
	}
}
