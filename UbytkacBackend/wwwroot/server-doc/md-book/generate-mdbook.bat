@ECHO OFF
REM INTELIGENT SEARCH DOCUMENTATION
REM SUMMARY.DM IS Primary fIle with links to all other Files
REM Which are Inserted as Menu Items
REM All files must be in 'src' folder

set scriptpath=%~dp0
rmdir %scriptpath%book /Q /S
%scriptpath%mdbook.exe init --theme "rust" --force --ignore=none
%scriptpath%mdbook.exe build