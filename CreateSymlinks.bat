@echo off

pushd "%~dp0"

cd ..\
echo %cd%

rem rd /s /q "Assets\Plugins\NorskaLibSymlinks"
mkdir "Assets\Plugins\NorskaLibSymlinks"

rem BODY_START

if exist "Assets\Plugins\NorskaLibSymlinks\DependencyInjection\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\DependencyInjection" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\DependencyInjection" (
del "Assets\Plugins\NorskaLibSymlinks\DependencyInjection" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\DependencyInjection" "..\..\..\NorskaLib\DependencyInjection"


if exist "Assets\Plugins\NorskaLibSymlinks\Extensions\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\Extensions" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\Extensions" (
del "Assets\Plugins\NorskaLibSymlinks\Extensions" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\Extensions" "..\..\..\NorskaLib\Extensions"


if exist "Assets\Plugins\NorskaLibSymlinks\Handies\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\Handies" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\Handies" (
del "Assets\Plugins\NorskaLibSymlinks\Handies" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\Handies" "..\..\..\NorskaLib\Handies"


if exist "Assets\Plugins\NorskaLibSymlinks\Localization\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\Localization" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\Localization" (
del "Assets\Plugins\NorskaLibSymlinks\Localization" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\Localization" "..\..\..\NorskaLib\Localization"


if exist "Assets\Plugins\NorskaLibSymlinks\Storage\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\Storage" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\Storage" (
del "Assets\Plugins\NorskaLibSymlinks\Storage" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\Storage" "..\..\..\NorskaLib\Storage"


if exist "Assets\Plugins\NorskaLibSymlinks\TimeController\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\TimeController" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\TimeController" (
del "Assets\Plugins\NorskaLibSymlinks\TimeController" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\TimeController" "..\..\..\NorskaLib\TimeController"


if exist "Assets\Plugins\NorskaLibSymlinks\UI\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\UI" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\UI" (
del "Assets\Plugins\NorskaLibSymlinks\UI" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\UI" "..\..\..\NorskaLib\UI"


if exist "Assets\Plugins\NorskaLibSymlinks\Utilities\" (
rmdir "Assets\Plugins\NorskaLibSymlinks\Utilities" /s /q
)
if exist "Assets\Plugins\NorskaLibSymlinks\Utilities" (
del "Assets\Plugins\NorskaLibSymlinks\Utilities" /f /q
)
mklink /d "Assets\Plugins\NorskaLibSymlinks\Utilities" "..\..\..\NorskaLib\Utilities"