rm -f bin.zip
7z a -t7z choanji.7z start.bat RELEASE.txt Binary Resources -x!Binary/bin_Data/output_log.txt > nul
