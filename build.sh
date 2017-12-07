mcs -nologo -target:library -lib:bin  -r:System.Data.dll -r:System.Drawing.dll  -out:bin/wxnet.dll src/wx/*.cs 
