//-----------------------------------------------------------------------------
// wx.NET - Locale.cs
//
// The wxLocale wrapper class.
//
// Written by Alexander Olk (xenomorph2@onlinehome.de)
// (C) 2004 by Alexander Olk
// Licensed under the wxWidgets license, see LICENSE.txt for details.
//
// $Id: Locale.cs,v 1.1 2004/11/27 21:29:53 olkalex Exp $
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace wx
{
	public enum Language
	{
		wxLANGUAGE_DEFAULT,
		wxLANGUAGE_UNKNOWN,
		
		wxLANGUAGE_ABKHAZIAN,
		wxLANGUAGE_AFAR,
		wxLANGUAGE_AFRIKAANS,
		wxLANGUAGE_ALBANIAN,
		wxLANGUAGE_AMHARIC,
		wxLANGUAGE_ARABIC,
		wxLANGUAGE_ARABIC_ALGERIA,
		wxLANGUAGE_ARABIC_BAHRAIN,
		wxLANGUAGE_ARABIC_EGYPT,
		wxLANGUAGE_ARABIC_IRAQ,
		wxLANGUAGE_ARABIC_JORDAN,
		wxLANGUAGE_ARABIC_KUWAIT,
		wxLANGUAGE_ARABIC_LEBANON,
		wxLANGUAGE_ARABIC_LIBYA,
		wxLANGUAGE_ARABIC_MOROCCO,
		wxLANGUAGE_ARABIC_OMAN,
		wxLANGUAGE_ARABIC_QATAR,
		wxLANGUAGE_ARABIC_SAUDI_ARABIA,
		wxLANGUAGE_ARABIC_SUDAN,
		wxLANGUAGE_ARABIC_SYRIA,
		wxLANGUAGE_ARABIC_TUNISIA,
		wxLANGUAGE_ARABIC_UAE,
		wxLANGUAGE_ARABIC_YEMEN,
		wxLANGUAGE_ARMENIAN,
		wxLANGUAGE_ASSAMESE,
		wxLANGUAGE_AYMARA,
		wxLANGUAGE_AZERI,
		wxLANGUAGE_AZERI_CYRILLIC,
		wxLANGUAGE_AZERI_LATIN,
		wxLANGUAGE_BASHKIR,
		wxLANGUAGE_BASQUE,
		wxLANGUAGE_BELARUSIAN,
		wxLANGUAGE_BENGALI,
		wxLANGUAGE_BHUTANI,
		wxLANGUAGE_BIHARI,
		wxLANGUAGE_BISLAMA,
		wxLANGUAGE_BRETON,
		wxLANGUAGE_BULGARIAN,
		wxLANGUAGE_BURMESE,
		wxLANGUAGE_CAMBODIAN,
		wxLANGUAGE_CATALAN,
		wxLANGUAGE_CHINESE,
		wxLANGUAGE_CHINESE_SIMPLIFIED,
		wxLANGUAGE_CHINESE_TRADITIONAL,
		wxLANGUAGE_CHINESE_HONGKONG,
		wxLANGUAGE_CHINESE_MACAU,
		wxLANGUAGE_CHINESE_SINGAPORE,
		wxLANGUAGE_CHINESE_TAIWAN,
		wxLANGUAGE_CORSICAN,
		wxLANGUAGE_CROATIAN,
		wxLANGUAGE_CZECH,
		wxLANGUAGE_DANISH,
		wxLANGUAGE_DUTCH,
		wxLANGUAGE_DUTCH_BELGIAN,
		wxLANGUAGE_ENGLISH,
		wxLANGUAGE_ENGLISH_UK,
		wxLANGUAGE_ENGLISH_US,
		wxLANGUAGE_ENGLISH_AUSTRALIA,
		wxLANGUAGE_ENGLISH_BELIZE,
		wxLANGUAGE_ENGLISH_BOTSWANA,
		wxLANGUAGE_ENGLISH_CANADA,
		wxLANGUAGE_ENGLISH_CARIBBEAN,
		wxLANGUAGE_ENGLISH_DENMARK,
		wxLANGUAGE_ENGLISH_EIRE,
		wxLANGUAGE_ENGLISH_JAMAICA,
		wxLANGUAGE_ENGLISH_NEW_ZEALAND,
		wxLANGUAGE_ENGLISH_PHILIPPINES,
		wxLANGUAGE_ENGLISH_SOUTH_AFRICA,
		wxLANGUAGE_ENGLISH_TRINIDAD,
		wxLANGUAGE_ENGLISH_ZIMBABWE,
		wxLANGUAGE_ESPERANTO,
		wxLANGUAGE_ESTONIAN,
		wxLANGUAGE_FAEROESE,
		wxLANGUAGE_FARSI,
		wxLANGUAGE_FIJI,
		wxLANGUAGE_FINNISH,
		wxLANGUAGE_FRENCH,
		wxLANGUAGE_FRENCH_BELGIAN,
		wxLANGUAGE_FRENCH_CANADIAN,
		wxLANGUAGE_FRENCH_LUXEMBOURG,
		wxLANGUAGE_FRENCH_MONACO,
		wxLANGUAGE_FRENCH_SWISS,
		wxLANGUAGE_FRISIAN,
		wxLANGUAGE_GALICIAN,
		wxLANGUAGE_GEORGIAN,
		wxLANGUAGE_GERMAN,
		wxLANGUAGE_GERMAN_AUSTRIAN,
		wxLANGUAGE_GERMAN_BELGIUM,
		wxLANGUAGE_GERMAN_LIECHTENSTEIN,
		wxLANGUAGE_GERMAN_LUXEMBOURG,
		wxLANGUAGE_GERMAN_SWISS,
		wxLANGUAGE_GREEK,
		wxLANGUAGE_GREENLANDIC,
		wxLANGUAGE_GUARANI,
		wxLANGUAGE_GUJARATI,
		wxLANGUAGE_HAUSA,
		wxLANGUAGE_HEBREW,
		wxLANGUAGE_HINDI,
		wxLANGUAGE_HUNGARIAN,
		wxLANGUAGE_ICELANDIC,
		wxLANGUAGE_INDONESIAN,
		wxLANGUAGE_INTERLINGUA,
		wxLANGUAGE_INTERLINGUE,
		wxLANGUAGE_INUKTITUT,
		wxLANGUAGE_INUPIAK,
		wxLANGUAGE_IRISH,
		wxLANGUAGE_ITALIAN,
		wxLANGUAGE_ITALIAN_SWISS,
		wxLANGUAGE_JAPANESE,
		wxLANGUAGE_JAVANESE,
		wxLANGUAGE_KANNADA,
		wxLANGUAGE_KASHMIRI,
		wxLANGUAGE_KASHMIRI_INDIA,
		wxLANGUAGE_KAZAKH,
		wxLANGUAGE_KERNEWEK,
		wxLANGUAGE_KINYARWANDA,
		wxLANGUAGE_KIRGHIZ,
		wxLANGUAGE_KIRUNDI,
		wxLANGUAGE_KONKANI,
		wxLANGUAGE_KOREAN,
		wxLANGUAGE_KURDISH,
		wxLANGUAGE_LAOTHIAN,
		wxLANGUAGE_LATIN,
		wxLANGUAGE_LATVIAN,
		wxLANGUAGE_LINGALA,
		wxLANGUAGE_LITHUANIAN,
		wxLANGUAGE_MACEDONIAN,
		wxLANGUAGE_MALAGASY,
		wxLANGUAGE_MALAY,
		wxLANGUAGE_MALAYALAM,
		wxLANGUAGE_MALAY_BRUNEI_DARUSSALAM,
		wxLANGUAGE_MALAY_MALAYSIA,
		wxLANGUAGE_MALTESE,
		wxLANGUAGE_MANIPURI,
		wxLANGUAGE_MAORI,
		wxLANGUAGE_MARATHI,
		wxLANGUAGE_MOLDAVIAN,
		wxLANGUAGE_MONGOLIAN,
		wxLANGUAGE_NAURU,
		wxLANGUAGE_NEPALI,
		wxLANGUAGE_NEPALI_INDIA,
		wxLANGUAGE_NORWEGIAN_BOKMAL,
		wxLANGUAGE_NORWEGIAN_NYNORSK,
		wxLANGUAGE_OCCITAN,
		wxLANGUAGE_ORIYA,
		wxLANGUAGE_OROMO,
		wxLANGUAGE_PASHTO,
		wxLANGUAGE_POLISH,
		wxLANGUAGE_PORTUGUESE,
		wxLANGUAGE_PORTUGUESE_BRAZILIAN,
		wxLANGUAGE_PUNJABI,
		wxLANGUAGE_QUECHUA,
		wxLANGUAGE_RHAETO_ROMANCE,
		wxLANGUAGE_ROMANIAN,
		wxLANGUAGE_RUSSIAN,
		wxLANGUAGE_RUSSIAN_UKRAINE,
		wxLANGUAGE_SAMOAN,
		wxLANGUAGE_SANGHO,
		wxLANGUAGE_SANSKRIT,
		wxLANGUAGE_SCOTS_GAELIC,
		wxLANGUAGE_SERBIAN,
		wxLANGUAGE_SERBIAN_CYRILLIC,
		wxLANGUAGE_SERBIAN_LATIN,
		wxLANGUAGE_SERBO_CROATIAN,
		wxLANGUAGE_SESOTHO,
		wxLANGUAGE_SETSWANA,
		wxLANGUAGE_SHONA,
		wxLANGUAGE_SINDHI,
		wxLANGUAGE_SINHALESE,
		wxLANGUAGE_SISWATI,
		wxLANGUAGE_SLOVAK,
		wxLANGUAGE_SLOVENIAN,
		wxLANGUAGE_SOMALI,
		wxLANGUAGE_SPANISH,
		wxLANGUAGE_SPANISH_ARGENTINA,
		wxLANGUAGE_SPANISH_BOLIVIA,
		wxLANGUAGE_SPANISH_CHILE,
		wxLANGUAGE_SPANISH_COLOMBIA,
		wxLANGUAGE_SPANISH_COSTA_RICA,
		wxLANGUAGE_SPANISH_DOMINICAN_REPUBLIC,
		wxLANGUAGE_SPANISH_ECUADOR,
		wxLANGUAGE_SPANISH_EL_SALVADOR,
		wxLANGUAGE_SPANISH_GUATEMALA,
		wxLANGUAGE_SPANISH_HONDURAS,
		wxLANGUAGE_SPANISH_MEXICAN,
		wxLANGUAGE_SPANISH_MODERN,
		wxLANGUAGE_SPANISH_NICARAGUA,
		wxLANGUAGE_SPANISH_PANAMA,
		wxLANGUAGE_SPANISH_PARAGUAY,
		wxLANGUAGE_SPANISH_PERU,
		wxLANGUAGE_SPANISH_PUERTO_RICO,
		wxLANGUAGE_SPANISH_URUGUAY,
		wxLANGUAGE_SPANISH_US,
		wxLANGUAGE_SPANISH_VENEZUELA,
		wxLANGUAGE_SUNDANESE,
		wxLANGUAGE_SWAHILI,
		wxLANGUAGE_SWEDISH,
		wxLANGUAGE_SWEDISH_FINLAND,
		wxLANGUAGE_TAGALOG,
		wxLANGUAGE_TAJIK,
		wxLANGUAGE_TAMIL,
		wxLANGUAGE_TATAR,
		wxLANGUAGE_TELUGU,
		wxLANGUAGE_THAI,
		wxLANGUAGE_TIBETAN,
		wxLANGUAGE_TIGRINYA,
		wxLANGUAGE_TONGA,
		wxLANGUAGE_TSONGA,
		wxLANGUAGE_TURKISH,
		wxLANGUAGE_TURKMEN,
		wxLANGUAGE_TWI,
		wxLANGUAGE_UIGHUR,
		wxLANGUAGE_UKRAINIAN,
		wxLANGUAGE_URDU,
		wxLANGUAGE_URDU_INDIA,
		wxLANGUAGE_URDU_PAKISTAN,
		wxLANGUAGE_UZBEK,
		wxLANGUAGE_UZBEK_CYRILLIC,
		wxLANGUAGE_UZBEK_LATIN,
		wxLANGUAGE_VIETNAMESE,
		wxLANGUAGE_VOLAPUK,
		wxLANGUAGE_WELSH,
		wxLANGUAGE_WOLOF,
		wxLANGUAGE_XHOSA,
		wxLANGUAGE_YIDDISH,
		wxLANGUAGE_YORUBA,
		wxLANGUAGE_ZHUANG,
		wxLANGUAGE_ZULU,
		
		wxLANGUAGE_USER_DEFINED
	}
	
	//-----------------------------------------------------------------------------
	
	public enum LocaleCategory
	{
		wxLOCALE_CAT_NUMBER,
		wxLOCALE_CAT_DATE,
		wxLOCALE_CAT_MONEY,
		wxLOCALE_CAT_MAX
	}
	
	//-----------------------------------------------------------------------------

	public enum LocaleInfo
	{
		wxLOCALE_THOUSANDS_SEP,
		wxLOCALE_DECIMAL_POINT
	}
	
	//-----------------------------------------------------------------------------

	public enum LocaleInitFlags
	{
		wxLOCALE_LOAD_DEFAULT  = 0x0001,    
		wxLOCALE_CONV_ENCODING = 0x0002    
	}
	
	//-----------------------------------------------------------------------------
	
	public class LanguageInfo : Object
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLanguageInfo_ctor();
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLanguageInfo_dtor(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLanguageInfo_SetLanguage(IntPtr self, int value);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern int    wxLanguageInfo_GetLanguage(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLanguageInfo_SetCanonicalName(IntPtr self, string name);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLanguageInfo_GetCanonicalName(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLanguageInfo_SetDescription(IntPtr self, string name);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLanguageInfo_GetDescription(IntPtr self);
		
		//-----------------------------------------------------------------------------

		public LanguageInfo(IntPtr wxObject)
			: base(wxObject)
		{
			this.wxObject = wxObject;
		}
			
		internal LanguageInfo(IntPtr wxObject, bool memOwn)
			: base(wxObject)
		{
			this.memOwn = memOwn;
			this.wxObject = wxObject;
		}
		
		public LanguageInfo()
			: this(wxLanguageInfo_ctor(), true) {}
		
		//---------------------------------------------------------------------

		public override void Dispose()
		{
			if (!disposed)
			{
				if (wxObject != IntPtr.Zero)
				{
					if (memOwn)
					{
						wxLanguageInfo_dtor(wxObject);
					}
				}
				RemoveObject(wxObject);
				wxObject = IntPtr.Zero;
				disposed= true;
			}
			
			virtual_Dispose = null;
			base.Dispose();
			GC.SuppressFinalize(this);
		}
		
		//---------------------------------------------------------------------
		
		~LanguageInfo() 
		{
			Dispose();
		}
		
		//---------------------------------------------------------------------
		
		public int Language
		{
			get { return wxLanguageInfo_GetLanguage(wxObject); }
			set { wxLanguageInfo_SetLanguage(wxObject, value); }
		}
		
		//---------------------------------------------------------------------
		
		public string CanonicalName
		{
			get { return new wxString(wxLanguageInfo_GetCanonicalName(wxObject), true); }
			set { wxLanguageInfo_SetCanonicalName(wxObject, value); }
		}
		
		//---------------------------------------------------------------------
		
		public string Description
		{
			get { return new wxString(wxLanguageInfo_GetDescription(wxObject), true); }
			set { wxLanguageInfo_SetDescription(wxObject, value); }
		}
	}
	
	//-----------------------------------------------------------------------------
	
	public class Locale : Object
	{
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_ctor();
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_ctor2(int language, int flags);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLocale_dtor(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxLocale_Init(IntPtr self, int language, int flags);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxLocale_AddCatalog(IntPtr self, string szDomain);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxLocale_AddCatalog2(IntPtr self, string szDomain, int msgIdLanguage, string msgIdCharset);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLocale_AddCatalogLookupPathPrefix(IntPtr self, string prefix);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern void   wxLocale_AddLanguage(IntPtr info);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_FindLanguageInfo(string locale);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetCanonicalName(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern int    wxLocale_GetLanguage(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetLanguageInfo(int lang);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetLanguageName(int lang);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetLocale(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetName(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetString(IntPtr self, string szOrigString, string szDomain);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetHeaderValue(IntPtr self, string szHeader, string szDomain);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetSysName(IntPtr self);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern int    wxLocale_GetSystemEncoding();
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern IntPtr wxLocale_GetSystemEncodingName();
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern int    wxLocale_GetSystemLanguage();
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxLocale_IsLoaded(IntPtr self, string domain);
		[DllImport("wx-c"), System.Security.SuppressUnmanagedCodeSecurity] static extern bool   wxLocale_IsOk(IntPtr self);
		
		//-----------------------------------------------------------------------------

		public Locale(IntPtr wxObject)
			: base(wxObject)
		{
			this.wxObject = wxObject;
		}
			
		internal Locale(IntPtr wxObject, bool memOwn)
			: base(wxObject)
		{
			this.memOwn = memOwn;
			this.wxObject = wxObject;
		}
		
		public Locale()
			: this(wxLocale_ctor(), true) {}
			
		public Locale(int language)
			: this(language, LocaleInitFlags.wxLOCALE_LOAD_DEFAULT | LocaleInitFlags.wxLOCALE_CONV_ENCODING) {}
			
		public Locale(int language, LocaleInitFlags flags)
			: this(wxLocale_ctor2(language, (int)flags), true) {}
		
		//---------------------------------------------------------------------

		public override void Dispose()
		{
			if (!disposed)
			{
				if (wxObject != IntPtr.Zero)
				{
					if (memOwn)
					{
						wxLocale_dtor(wxObject);
					}
				}
				RemoveObject(wxObject);
				wxObject = IntPtr.Zero;
				disposed= true;
			}
			
			virtual_Dispose = null;
			base.Dispose();
			GC.SuppressFinalize(this);
		}
		
		//---------------------------------------------------------------------
		
		~Locale() 
		{
			Dispose();
		}
		
		//-----------------------------------------------------------------------------
		
		public bool Init()
		{
			return Init(Language.wxLANGUAGE_DEFAULT, LocaleInitFlags.wxLOCALE_LOAD_DEFAULT | LocaleInitFlags.wxLOCALE_CONV_ENCODING);
		}
		
		public bool Init(Language language)
		{
			return Init(language,  LocaleInitFlags.wxLOCALE_LOAD_DEFAULT | LocaleInitFlags.wxLOCALE_CONV_ENCODING);
		}
		
		public bool Init(Language language, LocaleInitFlags flags)
		{
			return wxLocale_Init(wxObject, (int)language, (int)flags);
		}
		
		//-----------------------------------------------------------------------------
		
		public bool AddCatalog(string szDomain)
		{
			return wxLocale_AddCatalog(wxObject, szDomain);
		}
		
		public bool AddCatalog(string szDomain, Language msgIdLanguage, string msgIdCharset)
		{
			return wxLocale_AddCatalog2(wxObject, szDomain, (int)msgIdLanguage, msgIdCharset);
		}
		
		//-----------------------------------------------------------------------------
		
		public void AddCatalogLookupPathPrefix(string prefix)
		{
			wxLocale_AddCatalogLookupPathPrefix(wxObject, prefix);
		}
		
		//-----------------------------------------------------------------------------
		
		public static void AddLanguage(LanguageInfo info)
		{
			wxLocale_AddLanguage(Object.SafePtr(info));
		}
		
		//-----------------------------------------------------------------------------
		
		public static LanguageInfo FindLanguageInfo(string locale)
		{
			return (LanguageInfo)Object.FindObject(wxLocale_FindLanguageInfo(locale), typeof(LanguageInfo));
		}
		
		//-----------------------------------------------------------------------------
		
		public string CanonicalName
		{
			get { return new wxString(wxLocale_GetCanonicalName(wxObject), true); }
		}
		
		//-----------------------------------------------------------------------------
		
		public Language Language
		{
			get { return (Language)wxLocale_GetLanguage(wxObject); }
		}
		
		//-----------------------------------------------------------------------------
		
		public static LanguageInfo GetLanguageInfo(Language lang)
		{
			return (LanguageInfo)Object.FindObject(wxLocale_GetLanguageInfo((int)lang), typeof(LanguageInfo));
		}
		
		//-----------------------------------------------------------------------------
		
		public static string GetLanguageName(Language lang)
		{
			return new wxString(wxLocale_GetLanguageName((int)lang), true);
		}
		
		//-----------------------------------------------------------------------------
		
		public string GetLocale()
		{
			return new wxString(wxLocale_GetLocale(wxObject), true);
		}
		
		//-----------------------------------------------------------------------------
		
		public string Name
		{
			get { return new wxString(wxLocale_GetName(wxObject), true); }
		}
		
		//-----------------------------------------------------------------------------
		
		public string GetString(string szOrigString)
		{
			return GetString(szOrigString, null);
		}
		
		public string GetString(string szOrigString, string szDomain)
		{
			return new wxString(wxLocale_GetString(wxObject, szOrigString, szDomain), true);
		}
		
		//-----------------------------------------------------------------------------
		
		public string GetHeaderValue(string szHeader)
		{
			return GetHeaderValue(szHeader, null);
		}
		
		public string GetHeaderValue(string szHeader, string szDomain)
		{
			return new wxString(wxLocale_GetHeaderValue(wxObject, szHeader, szDomain), true);
		}
		
		//-----------------------------------------------------------------------------
		
		public string SysName
		{
			get { return new wxString(wxLocale_GetSysName(wxObject), true); }
		}
		
		//-----------------------------------------------------------------------------
		
		public static FontEncoding SystemEncoding
		{
			get { return (FontEncoding)wxLocale_GetSystemEncoding(); }
		}
		
		//-----------------------------------------------------------------------------
		
		public static string SystemEncodingName
		{
			get { return new wxString(wxLocale_GetSystemEncodingName(), true); }
		}
		
		//-----------------------------------------------------------------------------
		
		public static Language SystemLanguage
		{
			get { return (Language)wxLocale_GetSystemLanguage(); }
		}
		
		//-----------------------------------------------------------------------------
		
		public bool IsLoaded(string domain)
		{
			return wxLocale_IsLoaded(wxObject, domain);
		}
		
		//-----------------------------------------------------------------------------
		
		public bool IsOk
		{
			get { return wxLocale_IsOk(wxObject); }
		}
	}
}