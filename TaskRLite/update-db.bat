@echo off
set scriptDir=%~dp0
echo aktueller Projekt-Pfad: %scriptDir%
rem pushd scriptDir
rem cd /d %C:\Users\Reha-TN\source\repos\TaskRLite\TaskRLite%
cd /d %scriptDir%

if exist TaskRLite.db (
    del "TaskRLite.db"
    echo file TaskRLite.db deleted ! 
)
echo running database update . . .
dotnet ef database update

pause
