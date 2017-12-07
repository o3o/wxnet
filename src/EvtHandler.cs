//-----------------------------------------------------------------------------
// wx.NET - EvtHandler.cs
//
// The wxEvtHandler wrapper class.
//
// Written by Jason Perkins (jason@379.com)
// (C) 2003 by 379, Inc.
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: EvtHandler.cs,v 1.46 2005/04/30 16:02:09 olkalex Exp $
//-----------------------------------------------------------------------------

// TODO:	Change handling of removing EventListeners. If a listener gets
//		removed and then readded it will not be added to the end but at
//		the same position of listeners. This is because of the clientdata
//		that the event proxy gets...

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace wx
{
	public delegate void EventListener(object sender, Event e);
	
	//---------------------------------------------------------------------
    
	public struct SListener
	{
		public EventListener listener;
		public Object owner;
		public int eventType;
		public bool active;
		
		public SListener( EventListener listener, Object owner, int eventType )
		{
			this.listener = listener;
			this.owner = owner;
			this.eventType = eventType;
			active = true;
		}
	}
	
	//---------------------------------------------------------------------

	public class EvtHandler : Object
	{
		private delegate void EvtMarshalDelegate(IntPtr wxEvent, int iListener);
		private EvtMarshalDelegate eventMarshal;
		
		private ArrayList listeners;
		
		public delegate void ObjectDeletedHandler();
		
		public ObjectDeletedHandler ObjectDeleted;
		
		//---------------------------------------------------------------------
		
		// We store hard references to event handlers, since wxWidgets will
		// clean them up.
		private static Hashtable evtHandlers = new Hashtable();
		
		//---------------------------------------------------------------------
			
		[DllImport("wx-c")] static extern void wxEvtHandler_proxy(IntPtr self, EvtMarshalDelegate proxy);
		[DllImport("wx-c")] static extern void wxEvtHandler_Connect(IntPtr self, int evtType, int id, int lastId, int iListener);
		
		[DllImport("wx-c")] static extern bool wxEvtHandler_ProcessEvent(IntPtr self, IntPtr evt);
		
		[DllImport("wx-c")] static extern void wxEvtHandler_AddPendingEvent(IntPtr self, IntPtr evt); 
		
		//---------------------------------------------------------------------

		internal EvtHandler(IntPtr wxObject) 
			: base(wxObject)
		{
			lock (typeof(EvtHandler))
			{
				eventMarshal = new EvtMarshalDelegate(MarshalEvent);
				wxEvtHandler_proxy(wxObject, eventMarshal);
				
				listeners = new ArrayList();
				
				AddEventListener(Event.wxEVT_OBJECTDELETED, new EventListener(OnObjectDeleted));
			
				AddEvtHander(this);
			}
        	}
	
		//---------------------------------------------------------------------
        
		~EvtHandler()
		{
			RemoveEvtHandler(wxObject);
		}
	
		//---------------------------------------------------------------------
	
		public void AddCommandListener(int eventType, int id, EventListener listener)
		{
			AddCommandRangeListener(eventType, id, -1, listener);
		}
		
		public void AddCommandListener(int eventType, int id, EventListener listener, Object owner)
		{
			AddCommandRangeListener(eventType, id, -1, listener, owner);
		}
		
		//---------------------------------------------------------------------
	
		public void AddCommandRangeListener(int eventType, int id, int lastId, EventListener listener)
		{
			// I must keep a reference to the listener to prevent it from
			// being garbage collected. I had trouble passing the listener
			// delegate into C and back (.NET threw a runtime error, Mono
			// crashed) so I pass the index into the listeners array instead.
			// Works like a charm so far.
			listeners.Add( new SListener(listener, null, eventType) );
			wxEvtHandler_Connect(wxObject, eventType, id, lastId, listeners.Count - 1);
		}
		
		public void AddCommandRangeListener(int eventType, int id, int lastId, EventListener listener, Object owner)
		{
			// I must keep a reference to the listener to prevent it from
			// being garbage collected. I had trouble passing the listener
			// delegate into C and back (.NET threw a runtime error, Mono
			// crashed) so I pass the index into the listeners array instead.
			// Works like a charm so far.
			
			// first we check if the listener is already in listeners
			// this can happen, when RemoveHandler gets called
			// if found, just set active to true and return
			foreach( SListener sl in listeners )
			{
				if ( sl.owner == owner && sl.listener == listener && sl.eventType == eventType )
				{
					int index = listeners.IndexOf(sl);
					SListener tmpsl = sl;
					tmpsl.active = true;
					listeners[index] = tmpsl;
					return;
				}
			}
			
			listeners.Add( new SListener(listener, owner, eventType) );
			wxEvtHandler_Connect(wxObject, eventType, id, lastId, listeners.Count - 1);
		}
		
		//---------------------------------------------------------------------
	
		public void AddEventListener(int eventType, EventListener listener)
		{
			AddCommandRangeListener(eventType, -1, -1, listener);
		}
		
		public void AddEventListener(int eventType, EventListener listener, Object owner)
		{
			AddCommandRangeListener(eventType, -1, -1, listener, owner);
		}
		
		//---------------------------------------------------------------------
	
		public void AddMenuListener(int id, EventListener listener)
		{
			AddCommandListener(Event.wxEVT_COMMAND_MENU_SELECTED, id, listener);
		}
		
		public void AddMenuListener(int id, EventListener listener, Object owner)
		{
			AddCommandListener(Event.wxEVT_COMMAND_MENU_SELECTED, id, listener, owner);
		}
	
		//---------------------------------------------------------------------
	
		public bool ProcessEvent(Event evt) 
		{
			return wxEvtHandler_ProcessEvent(wxObject, Object.SafePtr(evt));
		}
		
		//---------------------------------------------------------------------
		// This method doesn't do a real disconnect it only sets the active
		// flag to false, if it finds it in listeners.
		// MarshalEvent then doesn't call the listener
		
		public bool RemoveHandler(EventListener listener, Object owner)
		{
			foreach( SListener sl in listeners )
			{
				if ( sl.listener == listener && sl.owner == owner && sl.active )
				{
					int index = listeners.IndexOf(sl);
					SListener tmpsl = sl;
					tmpsl.active = false;
					listeners[index] = tmpsl;
					return true;
				}
			}
			
			return false;
		}
		
		//---------------------------------------------------------------------

		public void AddPendingEvent(Event evt)
		{
			wxEvtHandler_AddPendingEvent(wxObject, Object.SafePtr(evt));
		}
	
		//---------------------------------------------------------------------
	
		// All listened-for events are received here. The event code is
		// mapped to an actual Event type, and then the listener EventListener lsnrtion
		// is called.
	
		private void MarshalEvent(IntPtr wxEvent, int iListener)
		{
			// Create an appropriate .NET wrapper for the event object
				
			Event e = Event.CreateFrom(wxEvent);
	
			// Send it off to the registered listener
			SListener listener = (SListener)listeners[iListener];
		
			// only iterate through the list if listener.owner != null
			// Only the new event system can handle more then one EventListener
			// because the EventListener gets connected via its owner and not
			// via a Frame, Dialog, etc...
			if ( listener.owner != null )
			{
				foreach ( SListener sl in listeners )
				{
					// continue if listener equals sl, because it will be handled below
					if ( listener.Equals( sl ) ) continue;
				
					// if there is the same object in the list with the same
					// EventType then call its listener also
					if ( sl.owner != null )
					{
						if ( sl.owner.Equals( listener.owner ) && sl.eventType == listener.eventType )
						{
							if ( sl.active ) sl.listener(this, e);
						}
					}
				}
			}
		
			if ( listener.active ) listener.listener(this, e);
		}
		
		//---------------------------------------------------------------------

		// This handler is called whenever an object's associated C++ instance
		// is deleted, so that any C# references can be cleaned up.

		internal static void OnObjectDeleted(object sender, Event evt)
		{
			EvtHandler evthandler = sender as EvtHandler;
		
			if ( evthandler.ObjectDeleted != null )
				evthandler.ObjectDeleted();
		
			RemoveEvtHandler(evt.EventIntPtr);
		}

		//---------------------------------------------------------------------

		internal static void AddEvtHander(EvtHandler eh)
		{
			if (eh.wxObject != IntPtr.Zero && !evtHandlers.ContainsKey(eh.wxObject)) 
			{
				evtHandlers.Add(eh.wxObject, eh);
			}
		}

		internal static void RemoveEvtHandler(IntPtr ptr)
		{
			if ( ptr != IntPtr.Zero)
			{
				evtHandlers.Remove(ptr);
				RemoveObject(ptr);
				ptr = IntPtr.Zero;
			}
		}

		//---------------------------------------------------------------------

		public void EVT_SIZE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_SIZE, lsnr); }
		public void EVT_CLOSE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_CLOSE_WINDOW, lsnr); }
		public void EVT_PAINT(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_PAINT, lsnr); }
		public void EVT_ERASE_BACKGROUND(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_ERASE_BACKGROUND, lsnr); }
		public void EVT_IDLE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_IDLE, lsnr); }
		public void EVT_MOVE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MOVE, lsnr); }
		public void EVT_TIMER(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_TIMER, lsnr); }
		public void EVT_SOCKET(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_SOCKET, lsnr); }
		public void EVT_KILL_FOCUS(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_KILL_FOCUS, lsnr); }
		public void EVT_SET_FOCUS(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_SET_FOCUS, lsnr); }
            
		public void EVT_MOUSE_EVENTS(EventListener lsnr)
		{  
			EVT_ENTER_WINDOW(lsnr);
			EVT_LEAVE_WINDOW(lsnr);

			EVT_LEFT_DOWN(lsnr);
			EVT_RIGHT_DOWN(lsnr);
			EVT_MIDDLE_DOWN(lsnr);
			
			EVT_LEFT_DCLICK(lsnr);
			EVT_RIGHT_DCLICK(lsnr);
			EVT_MIDDLE_DCLICK(lsnr);
			
			EVT_MOTION(lsnr); 
			
			EVT_LEFT_UP(lsnr); 
			EVT_RIGHT_UP(lsnr);
			EVT_MIDDLE_UP(lsnr);
		}

		public void EVT_ENTER_WINDOW(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_ENTER_WINDOW, lsnr); }
		public void EVT_LEAVE_WINDOW(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_LEAVE_WINDOW, lsnr); }
		
		public void EVT_LEFT_DOWN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_LEFT_DOWN, lsnr); }
		public void EVT_RIGHT_DOWN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_RIGHT_DOWN, lsnr); }
		public void EVT_MIDDLE_DOWN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MIDDLE_DOWN, lsnr); }
		
		public void EVT_LEFT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_LEFT_DCLICK, lsnr); }
		public void EVT_RIGHT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_RIGHT_DCLICK, lsnr); }
		public void EVT_MIDDLE_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MIDDLE_DCLICK, lsnr); }
		
		public void EVT_MOTION(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MOTION, lsnr); }
		
		public void EVT_LEFT_UP(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_LEFT_UP, lsnr); }
		public void EVT_RIGHT_UP(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_RIGHT_UP, lsnr); }
		public void EVT_MIDDLE_UP(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MIDDLE_UP, lsnr); }
			
		public void EVT_UPDATE_UI(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_UPDATE_UI, id, lsnr); }
		public void EVT_MENU(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_MENU_SELECTED, id, lsnr); }
		public void EVT_BUTTON(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_BUTTON_CLICKED, id, lsnr); }
		public void EVT_CHECKBOX(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_CHECKBOX_CLICKED, id, lsnr); }
		public void EVT_LISTBOX(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LISTBOX_SELECTED, id, lsnr); }
		public void EVT_LISTBOX_DCLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LISTBOX_DOUBLECLICKED, id, lsnr); }
		public void EVT_CHOICE(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_CHOICE_SELECTED, id, lsnr); }
		public void EVT_COMBOBOX(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_COMBOBOX_SELECTED, id, lsnr); }
		public void EVT_TEXT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TEXT_UPDATED, id, lsnr); }
		public void EVT_TEXT_ENTER(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TEXT_ENTER, id, lsnr); }
		public void EVT_RADIOBOX(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_RADIOBOX_SELECTED, id, lsnr); }
		public void EVT_RADIOBUTTON(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_RADIOBUTTON_SELECTED, id, lsnr); }
		public void EVT_SLIDER(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_SLIDER_UPDATED, id, lsnr); }
		public void EVT_SPINCTRL(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_SPINCTRL_UPDATED, id, lsnr); }
		public void EVT_SPIN_UP(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_SCROLL_LINEUP, id, lsnr); }
		public void EVT_SPIN_DOWN(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_SCROLL_LINEDOWN, id, lsnr); }
		public void EVT_SPIN(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_SCROLL_THUMBTRACK, id, lsnr); }
		public void EVT_TOGGLEBUTTON(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TOGGLEBUTTON_CLICKED, id, lsnr); }
		
		public void EVT_KEY_DOWN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_KEY_DOWN, lsnr); }
		public void EVT_KEY_UP(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_KEY_UP, lsnr); }
		public void EVT_CHAR(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_CHAR, lsnr); }
		
		public void EVT_CALENDAR_SEL_CHANGED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_SEL_CHANGED, id, lsnr); }
		public void EVT_CALENDAR_DAY(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_DAY_CHANGED, id, lsnr); }
		public void EVT_CALENDAR_MONTH(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_MONTH_CHANGED, id, lsnr); }
		public void EVT_CALENDAR_YEAR(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_YEAR_CHANGED, id, lsnr); }
		public void EVT_CALENDAR_DOUBLECLICKED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_DOUBLECLICKED, id, lsnr); }
		public void EVT_CALENDAR_WEEKDAY_CLICKED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_CALENDAR_WEEKDAY_CLICKED, id, lsnr); }
		
		public void EVT_FIND(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_FIND, id, lsnr); }
		public void EVT_FIND_NEXT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_FIND_NEXT, id, lsnr); }
		public void EVT_FIND_REPLACE(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_FIND_REPLACE, id, lsnr); }
		public void EVT_FIND_REPLACE_ALL(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_FIND_REPLACE_ALL, id, lsnr); }
		public void EVT_FIND_CLOSE(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_FIND_CLOSE, id, lsnr); }
		
		public void EVT_TREE_BEGIN_DRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_BEGIN_DRAG, id, lsnr); }
		public void EVT_TREE_BEGIN_RDRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_BEGIN_RDRAG, id, lsnr); }
		public void EVT_TREE_BEGIN_LABEL_EDIT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_BEGIN_LABEL_EDIT, id, lsnr); }
		public void EVT_TREE_END_LABEL_EDIT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_END_LABEL_EDIT, id, lsnr); }
		public void EVT_TREE_DELETE_ITEM(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_DELETE_ITEM, id, lsnr); }
		public void EVT_TREE_GET_INFO(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_GET_INFO, id, lsnr); }
		public void EVT_TREE_SET_INFO(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_SET_INFO, id, lsnr); }
		public void EVT_TREE_ITEM_EXPANDED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_EXPANDED, id, lsnr); }
		public void EVT_TREE_ITEM_EXPANDING(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_EXPANDING, id, lsnr); }
		public void EVT_TREE_ITEM_COLLAPSED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_COLLAPSED, id, lsnr); }
		public void EVT_TREE_ITEM_COLLAPSING(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_COLLAPSING, id, lsnr); }
		public void EVT_TREE_SEL_CHANGED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_SEL_CHANGED, id, lsnr); }
		public void EVT_TREE_SEL_CHANGING(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_SEL_CHANGING, id, lsnr); }
		public void EVT_TREE_KEY_DOWN(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_KEY_DOWN, id, lsnr); }
		public void EVT_TREE_ITEM_ACTIVATED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_ACTIVATED, id, lsnr); }
		public void EVT_TREE_ITEM_RIGHT_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_RIGHT_CLICK, id, lsnr); }
		public void EVT_TREE_ITEM_MIDDLE_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_ITEM_MIDDLE_CLICK, id, lsnr); }
		public void EVT_TREE_END_DRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_TREE_END_DRAG, id, lsnr); }
		
		public void EVT_LIST_BEGIN_DRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_BEGIN_DRAG, id, lsnr); }
		public void EVT_LIST_BEGIN_RDRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_BEGIN_RDRAG, id, lsnr); }
		public void EVT_LIST_BEGIN_LABEL_EDIT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_BEGIN_LABEL_EDIT, id, lsnr); }
		public void EVT_LIST_END_LABEL_EDIT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_END_LABEL_EDIT, id, lsnr); }    
		public void EVT_LIST_DELETE_ITEM(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_DELETE_ITEM, id, lsnr); }
		public void EVT_LIST_DELETE_ALL_ITEMS(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_DELETE_ALL_ITEMS, id, lsnr); }    
		public void EVT_LIST_GET_INFO(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_GET_INFO, id, lsnr); }
		public void EVT_LIST_SET_INFO(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_SET_INFO, id, lsnr); }
		public void EVT_LIST_ITEM_SELECTED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_SELECTED, id, lsnr); }  
		public void EVT_LIST_ITEM_DESELECTED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_DESELECTED, id, lsnr); }     
		public void EVT_LIST_ITEM_ACTIVATED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_ACTIVATED, id, lsnr); }
		public void EVT_LIST_ITEM_FOCUSED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_FOCUSED, id, lsnr); }
		public void EVT_LIST_ITEM_MIDDLE_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_MIDDLE_CLICK, id, lsnr); } 
		public void EVT_LIST_ITEM_RIGHT_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_ITEM_RIGHT_CLICK, id, lsnr); }     
		public void EVT_LIST_KEY_DOWN(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_KEY_DOWN, id, lsnr); }  
		public void EVT_LIST_INSERT_ITEM(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_INSERT_ITEM, id, lsnr); }     
		public void EVT_LIST_COL_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_COL_CLICK, id, lsnr); }
		public void EVT_LIST_COL_RIGHT_CLICK(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_COL_RIGHT_CLICK, id, lsnr); }   
		public void EVT_LIST_COL_BEGIN_DRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_COL_BEGIN_DRAG, id, lsnr); }   
		public void EVT_LIST_COL_DRAGGING(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_COL_DRAGGING, id, lsnr); }
		public void EVT_LIST_COL_END_DRAG(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_COL_END_DRAG, id, lsnr); }
		public void EVT_LIST_CACHE_HINT(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_LIST_CACHE_HINT, id, lsnr); }
		
		public void EVT_NOTEBOOK_PAGE_CHANGED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_NOTEBOOK_PAGE_CHANGED, id, lsnr); }
		public void EVT_NOTEBOOK_PAGE_CHANGING(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_NOTEBOOK_PAGE_CHANGING, id, lsnr); }

		public void EVT_LISTBOOK_PAGE_CHANGED(int id, EventListener lsnr)
		{ AddCommandListener(Event.wxEVT_COMMAND_LISTBOOK_PAGE_CHANGED, id, lsnr); }
		public void EVT_LISTBOOK_PAGE_CHANGING(int id, EventListener lsnr)
		{ AddCommandListener(Event.wxEVT_COMMAND_LISTBOOK_PAGE_CHANGING, id, lsnr); }

#if __WXMSW__
		public void EVT_TAB_SEL_CHANGED(int id, EventListener lsnr)
		{ AddCommandListener(Event.wxEVT_COMMAND_TAB_SEL_CHANGED, id, lsnr); }
		public void EVT_TAB_SEL_CHANGING(int id, EventListener lsnr)
		{ AddCommandListener(Event.wxEVT_COMMAND_TAB_SEL_CHANGING, id, lsnr); }
#endif
		
		public void EVT_GRID_CELL_LEFT_CLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_CELL_LEFT_CLICK, lsnr); }
		public void EVT_GRID_CELL_RIGHT_CLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_CELL_RIGHT_CLICK, lsnr); }
		public void EVT_GRID_CELL_LEFT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_CELL_LEFT_DCLICK, lsnr); }
		public void EVT_GRID_CELL_RIGHT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_CELL_RIGHT_DCLICK, lsnr); }
		public void EVT_GRID_LABEL_LEFT_CLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_LABEL_LEFT_CLICK, lsnr); }
		public void EVT_GRID_LABEL_RIGHT_CLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_LABEL_RIGHT_CLICK, lsnr); }
		public void EVT_GRID_LABEL_LEFT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_LABEL_LEFT_DCLICK, lsnr); }
		public void EVT_GRID_LABEL_RIGHT_DCLICK(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_LABEL_RIGHT_DCLICK, lsnr); }
		public void EVT_GRID_ROW_SIZE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_ROW_SIZE, lsnr); }
		public void EVT_GRID_COL_SIZE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_COL_SIZE, lsnr); }
		public void EVT_GRID_RANGE_SELECT(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_RANGE_SELECT, lsnr); }
		public void EVT_GRID_CELL_CHANGE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_CELL_CHANGE, lsnr); }
		public void EVT_GRID_SELECT_CELL(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_SELECT_CELL, lsnr); }
		public void EVT_GRID_EDITOR_SHOWN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_EDITOR_SHOWN, lsnr); }
		public void EVT_GRID_EDITOR_HIDDEN(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_EDITOR_HIDDEN, lsnr); }
		public void EVT_GRID_EDITOR_CREATED(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_GRID_EDITOR_CREATED, lsnr); }
			
		public void EVT_ACTIVATE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_ACTIVATE, lsnr); }

		public void EVT_DISPLAY_CHANGED(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_DISPLAY_CHANGED, lsnr); }
			
		public void EVT_SASH_DRAGGED(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_SASH_DRAGGED, id, lsnr); }
		public void EVT_SASH_DRAGGED_RANGE(int id, int lastId, EventListener lsnr)
			{ AddCommandRangeListener(Event.wxEVT_SASH_DRAGGED, id, lastId, lsnr); }
			
		public void EVT_QUERY_LAYOUT_INFO(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_QUERY_LAYOUT_INFO, lsnr); }
		public void EVT_CALCULATE_LAYOUT(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_CALCULATE_LAYOUT, lsnr); }
			
		public void EVT_CHECKLISTBOX(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_COMMAND_CHECKLISTBOX_TOGGLED, id, lsnr); }
			
		public void EVT_CONTEXT_MENU(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_CONTEXT_MENU, lsnr); }
			
		public void EVT_SYS_COLOUR_CHANGED(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_SYS_COLOUR_CHANGED, lsnr); }
			
		public void EVT_QUERY_NEW_PALETTE(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_QUERY_NEW_PALETTE, lsnr); }
			
		public void EVT_PALETTE_CHANGED(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_PALETTE_CHANGED, lsnr); }
			
		public void EVT_INIT_DIALOG(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_INIT_DIALOG, lsnr); }
			
		public void EVT_SIZING(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_SIZING, lsnr); }
			
		public void EVT_MOVING(EventListener lsnr)
			{ AddEventListener(Event.wxEVT_MOVING, lsnr); }
			
		public void EVT_HELP(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_HELP, id, lsnr); }
			
		public void EVT_DETAILED_HELP(int id, EventListener lsnr)
			{ AddCommandListener(Event.wxEVT_DETAILED_HELP, id, lsnr); }


#if WXNET_STYLEDTEXTCTRL

		// StyledTextCtrl specific events
		
		public void EVT_STC_CHANGE(int id, EventListener lsnr)            
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_CHANGE, id, lsnr); }
		public void EVT_STC_STYLENEEDED(int id, EventListener lsnr)       
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_STYLENEEDED, id, lsnr); }
		public void EVT_STC_CHARADDED(int id, EventListener lsnr)         
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_CHARADDED, id, lsnr); }
		
		public void EVT_STC_SAVEPOINTREACHED(int id, EventListener lsnr)  
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_SAVEPOINTREACHED, id, lsnr); }
		public void EVT_STC_SAVEPOINTLEFT(int id, EventListener lsnr)     
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_SAVEPOINTLEFT, id, lsnr); }
		public void EVT_STC_ROMODIFYATTEMPT(int id, EventListener lsnr)   
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_ROMODIFYATTEMPT, id, lsnr); }
		
		public void EVT_STC_KEY(int id, EventListener lsnr)               
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_KEY, id, lsnr); }
		public void EVT_STC_DOUBLECLICK(int id, EventListener lsnr)       
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_DOUBLECLICK, id, lsnr); }
		public void EVT_STC_UPDATEUI(int id, EventListener lsnr)          
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_UPDATEUI, id, lsnr); }
		public void EVT_STC_MODIFIED(int id, EventListener lsnr)          
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_MODIFIED, id, lsnr); }
		public void EVT_STC_MACRORECORD(int id, EventListener lsnr)       
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_MACRORECORD, id, lsnr); }
		public void EVT_STC_MARGINCLICK(int id, EventListener lsnr)       
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_MARGINCLICK, id, lsnr); }
		public void EVT_STC_NEEDSHOWN(int id, EventListener lsnr)         
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_NEEDSHOWN, id, lsnr); }
		//public void EVT_STC_POSCHANGED(int id, EventListener lsnr)        
		//	{ AddCommandListener(StyledTextCtrl.wxEVT_STC_POSCHANGED, id, lsnr); }
		public void EVT_STC_PAINTED(int id, EventListener lsnr)           
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_PAINTED, id, lsnr); }
		public void EVT_STC_USERLISTSELECTION(int id, EventListener lsnr) 
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_USERLISTSELECTION, id, lsnr); }
		public void EVT_STC_URIDROPPED(int id, EventListener lsnr)        
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_URIDROPPED, id, lsnr); }
		
		public void EVT_STC_DWELLSTART(int id, EventListener lsnr)        
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_DWELLSTART, id, lsnr); }
		public void EVT_STC_DWELLEND(int id, EventListener lsnr)          
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_DWELLEND, id, lsnr); }
		
		public void EVT_STC_START_DRAG(int id, EventListener lsnr)        
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_START_DRAG, id, lsnr); }
		public void EVT_STC_DRAG_OVER(int id, EventListener lsnr)         
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_DRAG_OVER, id, lsnr); }
		public void EVT_STC_DO_DROP(int id, EventListener lsnr)           
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_DO_DROP, id, lsnr); }
		
		public void EVT_STC_ZOOM(int id, EventListener lsnr)              
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_ZOOM, id, lsnr); }
		
		public void EVT_STC_HOTSPOT_CLICK(int id, EventListener lsnr)     
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_HOTSPOT_CLICK, id, lsnr); }
		public void EVT_STC_HOTSPOT_DCLICK(int id, EventListener lsnr)    
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_HOTSPOT_DCLICK, id, lsnr); }
		
		public void EVT_STC_CALLTIP_CLICK(int id, EventListener lsnr)    
			{ AddCommandListener(StyledTextCtrl.wxEVT_STC_CALLTIP_CLICK, id, lsnr); }			
#endif
	}
}
