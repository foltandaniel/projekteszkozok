#! /bin/sh

# Example build script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# Change this the name of your project. This will be the name of the final executables as well.
project="ci-build"
cd KiterjesztetAknakereso;
echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $(pwd) -batchmode -nographics -executeMethod Auto.AutoBuild.PerformBuild -logFile $(pwd)/unity.log -quit
#echo "Attempting to build $project for OS X"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity \
#  -batchmode \
#  -nographics \
#  -silent-crashes \
#  -logFile $(pwd)/unity.log \
#  -projectPath $(pwd) \
#  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
#  -quit

#echo "Attempting to build $project for Linux"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity \
#  -batchmode \
#  -nographics \
#  -silent-crashes \
#  -logFile $(pwd)/unity.log \
#  -projectPath $(pwd) \
#  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" \
#  -quit

echo 'Logs from build'
cat $(pwd)/unity.log
ls -all
cd KiterjesztetAknakereso
ls -all

zip -r build.zip build
ls
