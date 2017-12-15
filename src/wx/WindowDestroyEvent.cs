//-----------------------------------------------------------------------------
// wx.NET - WindowDestroyEvent.cs
//
// The wxWindowDestroyEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: WindowDestroyEvent.cs,v 1.2 2004/11/28 18:17:22 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx {
   public class WindowDestroyEvent : CommandEvent {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxWindowDestroyEvent_ctor(IntPtr type);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxWindowDestroyEvent_GetWindow(IntPtr self);

      //-----------------------------------------------------------------------------

      public WindowDestroyEvent(IntPtr wxObject)
      : base(wxObject) { }

      public WindowDestroyEvent(Window win)
      : this(wxWindowDestroyEvent_ctor(Object.SafePtr(win))) { }

      //-----------------------------------------------------------------------------

      public Window Active {
         get {
            return (Window)FindObject(wxWindowDestroyEvent_GetWindow(wxObject), typeof(Window));
         }
      }
   }
}
