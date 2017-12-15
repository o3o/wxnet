//-----------------------------------------------------------------------------
// wx.NET - MouseCaptureChangedEvent.cs
//
// The wxMouseCaptureChangedEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: MouseCaptureChangedEvent.cs,v 1.1 2004/10/17 17:47:42 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx {
   public class MouseCaptureChangedEvent : Event {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxMouseCaptureChangedEvent_ctor(int type);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxMouseCaptureChangedEvent_GetCapturedWindow(IntPtr self);

      //-----------------------------------------------------------------------------

      public MouseCaptureChangedEvent(IntPtr wxObject)
      : base(wxObject) { }

      public MouseCaptureChangedEvent(int type)
      : this(wxMouseCaptureChangedEvent_ctor(type)) { }

      //-----------------------------------------------------------------------------

      public Window CapturedWindow {
         get {
            return (Window)FindObject(wxMouseCaptureChangedEvent_GetCapturedWindow(wxObject), typeof(Window));
         }
      }
   }
}
