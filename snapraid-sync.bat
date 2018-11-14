IFF %@LEN[%_MONTH] EQ 1 THEN
	SET MN=0%_MONTH
ELSE
	 SET MN=%_MONTH
ENDIFF
IFF %@LEN[%_DAY] EQ 1 THEN
	SET DY=0%_DAY
ELSE
	SET DY=%_DAY
ENDIFF
C:\Applications\SnapRAID\snapraid.exe -c C:\Applications\SnapRAID\SnapRAID_%1.config sync -l \\ikkokkan.com\DFS\ONLINE\Logs\SnapRAID\snapraid-%1-sync-%%D.log
C:\Applications\cmdline\sendmail -server smtp.ikkokkan.com -from "SnapRAID_%1<snapraid@ikkokkan.com>" -to "admin@ikkokkan.com" -subject "SnapRAID %1 Sync Report" -body "%1 Sync Report" -attach "\\ikkokkan.com\DFS\ONLINE\Logs\SnapRAID\snapraid-%1-sync-%_YEAR%%MN%%DY.log" -type "text/plain"
UNSET MN
UNSET DY
EXIT
