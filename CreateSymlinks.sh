#!/bin/bash

cd ..

rm -r "./Assets/Plugins/NorskaLibSymlinks"
mkdir "./Assets/Plugins/NorskaLibSymlinks"
# BODY_START

ln -s "../../../NorskaLib/DependencyInjection" "./Assets/Plugins/NorskaLib/DependencyInjection"
ln -s "../../../NorskaLib/Extensions" "./Assets/Plugins/NorskaLib/Extensions"
ln -s "../../../NorskaLib/Handies" "./Assets/Plugins/NorskaLib/Handies"
ln -s "../../../NorskaLib/Localization" "./Assets/Plugins/NorskaLib/Localization"
ln -s "../../../NorskaLib/Pools" "./Assets/Plugins/NorskaLib/Pools"
ln -s "../../../NorskaLib/Storage" "./Assets/Plugins/NorskaLib/Storage"
ln -s "../../../NorskaLib/TimeController" "./Assets/Plugins/NorskaLib/TimeController"
ln -s "../../../NorskaLib/UI" "./Assets/Plugins/NorskaLib/UI"
ln -s "../../../NorskaLib/Utilities" "./Assets/Plugins/NorskaLib/Utilities"