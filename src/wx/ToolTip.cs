//-----------------------------------------------------------------------------
// wx.NET - ToolTip.cs
//
// The wxToolTip wrapper class.
//
// Written by Bryan Bulten (bryan@bulten.ca)
// (C) 2003 Bryan Bulten
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: ToolTip.cs,v 1.7 2004/11/24 20:47:45 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx {
   public class ToolTip : Object {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxToolTip_Enable(bool flag);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxToolTip_SetDelay(uint msecs);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxToolTip_ctor(string tip);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxToolTip_SetTip(IntPtr self, string tip);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxToolTip_GetTip(IntPtr self);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxToolTip_GetWindow(IntPtr self);

      //---------------------------------------------------------------------

      public ToolTip(IntPtr wxObject)
      : base(wxObject) { }

      public ToolTip(string tip)
      : base(wxToolTip_ctor(tip)) { }

      //---------------------------------------------------------------------

      public static bool Enabled {
         set {
            wxToolTip_Enable(value);
         }
      }

      //---------------------------------------------------------------------

      public static long Delay {
         set {
            wxToolTip_SetDelay((uint)value);
         }
      }

      //---------------------------------------------------------------------

      public string Tip {
         get {
            return new wxString(wxToolTip_GetTip(wxObject), true);
         }
         set {
            wxToolTip_SetTip(wxObject, value);
         }
      }

      //---------------------------------------------------------------------

      public Window Window {
         get {
            return (Window)FindObject(wxToolTip_GetWindow(wxObject));
         }
      }

      //---------------------------------------------------------------------
   }
}

