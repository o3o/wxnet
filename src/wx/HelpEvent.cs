//-----------------------------------------------------------------------------
// wx.NET - HelpEvent.cs
//
// The wxHelpEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: HelpEvent.cs,v 1.3 2004/11/28 18:17:22 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace wx {
   public class HelpEvent : CommandEvent {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxHelpEvent_ctor(int type);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxHelpEvent_GetPosition(IntPtr self, ref Point pos);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxHelpEvent_SetPosition(IntPtr self, ref Point pos);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxHelpEvent_GetLink(IntPtr self);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxHelpEvent_SetLink(IntPtr self, string link);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxHelpEvent_GetTarget(IntPtr self);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxHelpEvent_SetTarget(IntPtr self, string target);

      //-----------------------------------------------------------------------------

      public HelpEvent(IntPtr wxObject)
      : base(wxObject) { }

      public HelpEvent(int type)
      : this(wxHelpEvent_ctor(type)) { }

      //-----------------------------------------------------------------------------

      public Point Position {
         get {
            Point p = new Point();
            wxHelpEvent_GetPosition(wxObject, ref p);
            return p;
         }

         set {
            wxHelpEvent_SetPosition(wxObject, ref value);
         }
      }

      //-----------------------------------------------------------------------------

      public string Link {
         get {
            return new wxString(wxHelpEvent_GetLink(wxObject), true);
         }
         set {
            wxHelpEvent_SetLink(wxObject, value);
         }
      }

      //-----------------------------------------------------------------------------

      public string Target {
         get {
            return new wxString(wxHelpEvent_GetTarget(wxObject), true);
         }
         set {
            wxHelpEvent_SetTarget(wxObject, value);
         }
      }
   }
}
