//-----------------------------------------------------------------------------
// wx.NET - Icon.cs
//
// The wxIcon wrapper class.
//
// Written by Jason Perkins (jason@379.com)
// (C) 2003 by 379, Inc.
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: Icon.cs,v 1.6 2005/04/23 17:45:49 nummish Exp $
//-----------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace wx {
   public class Icon : Bitmap {
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxIcon_ctor();
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxIcon_CopyFromBitmap(IntPtr self, IntPtr bitmap);
      [DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxIcon_LoadFile(IntPtr self, string name, BitmapType type);

      //---------------------------------------------------------------------

      public Icon(string name)
      : this() {
         Image img = new Image();
         if (!img.LoadFile(name)) {
            throw new ArgumentException("file '" + name + "' not found");
         }

         Bitmap bmp = new Bitmap(img);
         wxIcon_CopyFromBitmap(wxObject, bmp.wxObject);
      }

      public Icon(string name, BitmapType type)
      : this() {
         if (type == BitmapType.wxBITMAP_TYPE_RESOURCE) {
            Assembly ResourceAssembly = Assembly.GetCallingAssembly();

            Image img = new Image(name, ResourceAssembly);

            Bitmap bmp = new Bitmap(img);
            wxIcon_CopyFromBitmap(wxObject, bmp.wxObject);
         } else {
            if (!wxIcon_LoadFile(wxObject, name, type)) {
               throw new ArgumentException();
            }
         }
      }

      public Icon()
      : base(wxIcon_ctor()) {
      }

      public Icon(IntPtr wxObject)
      : base(wxObject) { }

      //---------------------------------------------------------------------

      public void CopyFromBitmap(Bitmap bitmap) {
         wxIcon_CopyFromBitmap(wxObject, Object.SafePtr(bitmap));
      }

      //---------------------------------------------------------------------
   }
}
