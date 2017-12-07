//-----------------------------------------------------------------------------
// wx.NET - InitDialogEvent.cs
//
// The wxInitDialogEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: InitDialogEvent.cs,v 1.1 2004/10/16 18:23:46 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class InitDialogEvent : Event
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxInitDialogEvent_ctor(int Id);
		
		//-----------------------------------------------------------------------------

		public InitDialogEvent(IntPtr wxObject) 
			: base(wxObject) { }
			
		public InitDialogEvent()
			: base(wxInitDialogEvent_ctor(0)) {}

		public InitDialogEvent(int Id)
			: this(wxInitDialogEvent_ctor(Id)) { }
	}
}
