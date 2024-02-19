@ECHO OFF
REM INTELIGENT SEARCH DOCUMENTATION
REM SUMMARY.DM IS Primary fIle with links to all other Files
REM Which are Inserted as Menu Items
REM All files must be in 'src' folder
REM Start vaiables are automatically replaced

StartDrive
cd StartupPATH
del book /Q /S
mdbook.exe build