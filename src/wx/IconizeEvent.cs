//-----------------------------------------------------------------------------
// wx.NET - IconizeEvent.cs
//
// The wxIconizeEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: IconizeEvent.cs,v 1.1 2004/10/16 18:23:46 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class IconizeEvent : Event
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxIconizeEvent_ctor(int type);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool wxIconizeEvent_Iconized(IntPtr self);
		
		//-----------------------------------------------------------------------------

		public IconizeEvent(IntPtr wxObject) 
			: base(wxObject) { }

		public IconizeEvent(int type)
			: this(wxIconizeEvent_ctor(type)) { }

		//-----------------------------------------------------------------------------	
		
		public bool Iconized
		{
			get { return wxIconizeEvent_Iconized(wxObject); }
		}
	}
}
