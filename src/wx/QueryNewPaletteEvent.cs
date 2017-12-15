//-----------------------------------------------------------------------------
// wx.NET - QueryNewPaletteEvent.cs
//
// The wxQueryNewPaletteEvent wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: QueryNewPaletteEvent.cs,v 1.1 2004/10/17 17:47:42 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx {
   public class QueryNewPaletteEvent : Event {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxQueryNewPaletteEvent_ctor(int type);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool wxQueryNewPaletteEvent_GetPaletteRealized(IntPtr self);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void wxQueryNewPaletteEvent_SetPaletteRealized(IntPtr self, bool realized);

      //-----------------------------------------------------------------------------

      public QueryNewPaletteEvent(IntPtr wxObject)
      : base(wxObject) { }

      public QueryNewPaletteEvent(int type)
      : this(wxQueryNewPaletteEvent_ctor(type)) { }

      //-----------------------------------------------------------------------------

      public bool Realized {
         get {
            return wxQueryNewPaletteEvent_GetPaletteRealized(wxObject);
         }
         set {
            wxQueryNewPaletteEvent_SetPaletteRealized(wxObject, value);
         }
      }
   }
}
