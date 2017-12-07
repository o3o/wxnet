-- Premake script for the wx.NET package.
-- See http://premake.sourceforge.net/ for more info about Premake.

package.name     = "wx.NET"
package.language = "c#"
package.kind     = "dll"
package.links    = { "System.Xml", "System", "System.Drawing", "System.Data" }
package.files    = { matchfiles("*.cs") }
             
if (OS == "windows") then
    package.defines = { "__WXMSW__" }
elseif (OS == "macosx") then
    package.defines = { "__WXMAC__" }
end

-----------------------------------------------------------------------

if (options["enable-stc"]) then
    tinsert(package.defines, { "WXNET_STYLEDTEXTCTRL" })
end

if (options["with-display"]) then
    tinsert(package.defines, { "WXNET_DISPLAY" })
end

