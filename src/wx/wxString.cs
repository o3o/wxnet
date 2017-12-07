//-----------------------------------------------------------------------------
// wx.NET - wxString.cs
//
// The wxString wrapper class.
//
// Written by Bryan Bulten (bryan@bulten.ca)
// (C) 2003 Bryan Bulten
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: wxString.cs,v 1.16 2005/01/07 22:55:47 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public class wxString : Object
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxString_ctor(string str);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxString_dtor(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern string wxString_mb_str(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern uint   wxString_Length(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern char   wxString_CharAt(IntPtr self, uint i);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern int    wxString_CharAtInt(IntPtr self, uint i);
		
		//---------------------------------------------------------------------

		public wxString(IntPtr wxObject)
			: base(wxObject)
		{ 
			this.wxObject = wxObject;
		}
			
		internal wxString(IntPtr wxObject, bool memOwn)
			: base(wxObject)
		{ 
			this.memOwn = memOwn;
			this.wxObject = wxObject;
		}

		public wxString()
			: this("") { }

		public wxString(string str)
			: this(wxString_ctor(str), true) { }
		
		//---------------------------------------------------------------------
				
		public override void Dispose()
		{
			if (!disposed)
			{
				if (wxObject != IntPtr.Zero)
				{
					if (memOwn)
					{
						wxString_dtor(wxObject);
						memOwn = false;
					}
				}
				RemoveObject(wxObject);
				wxObject = IntPtr.Zero;
				disposed= true;
			}
			
			base.Dispose();
			GC.SuppressFinalize(this);
		}
		
		//---------------------------------------------------------------------
		
		~wxString() 
		{
			Dispose();
		}

		//---------------------------------------------------------------------

		public static implicit operator string (wxString str)
		{
			long len = str.Length;
			char[] t = new char[len];
			
			for(long i = 0; i < len; i++) {
				t[i] = str[i];
			}
			
			return new String(t);
		}
		
		public static implicit operator wxString (string str) 
		{
			return new wxString(str);
		}
		
		//---------------------------------------------------------------------

		public long Length
		{
			get { return wxString_Length(wxObject); }
		}

		public char this[long i]
		{
#if __WXMSW__
			get { return wxString_CharAt(wxObject, (uint)i); }
#else
			get {return System.Convert.ToChar(wxString_CharAtInt(wxObject, (uint)i)); }
#endif			
		}
		
		//---------------------------------------------------------------------

		internal static IntPtr SafePtr(wxString obj)
		{
			return (obj == null) ? IntPtr.Zero : obj.wxObject;
		}
	}
}
