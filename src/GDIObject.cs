//-----------------------------------------------------------------------------
// wx.NET - GDIObject.cs
//
// The wxGDIObject wrapper class.
//
// Written by Jason Perkins (jason@379.com)
// (C) 2003 by 379, Inc.
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: GDIObject.cs,v 1.10 2004/11/26 21:39:52 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class GDIObject : Object
	{
		[DllImport("wx-c")] static extern void wxGDIObj_dtor(IntPtr self);

		//---------------------------------------------------------------------

		public GDIObject(IntPtr wxObject) 
			: base(wxObject) {}

		/*public override void Dispose()
		{
			wxObject = IntPtr.Zero;
			Dispose(false);
		}*/		

		//---------------------------------------------------------------------
	}
}
