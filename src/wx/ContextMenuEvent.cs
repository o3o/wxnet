//-----------------------------------------------------------------------------
// wx.NET - ContextMenuEvent.cs
//
// The wxContextMenuEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: ContextMenuEvent.cs,v 1.2 2004/11/28 18:17:22 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace wx
{
	public class ContextMenuEvent : CommandEvent
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxContextMenuEvent_ctor(int type);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxContextMenuEvent_GetPosition(IntPtr self, ref Point pos);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxContextMenuEvent_SetPosition(IntPtr self, ref Point pos);
		
		//-----------------------------------------------------------------------------

		public ContextMenuEvent(IntPtr wxObject) 
			: base(wxObject) { }

		public ContextMenuEvent(int type)
			: this(wxContextMenuEvent_ctor(type)) { }

		//-----------------------------------------------------------------------------	
		
		public Point Position
		{
			get { 
				Point p = new Point();
				wxContextMenuEvent_GetPosition(wxObject, ref p); 
				return p;
			}
			
			set { wxContextMenuEvent_SetPosition(wxObject, ref value); }
		}
	}
}
