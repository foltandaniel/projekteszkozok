#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
echo 'Downloading from http://netstorage.unity3d.com/unity/3757309da7e7/MacEditorInstaller/Unity-5.2.2f1.pkg: '
curl -o Unity.pkg https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorInstaller/Unity-2017.3.1f1.pkg?_ga=2.211363349.168156340.1520523193-492440057.1520523193

curl -o WinSupport.pkg https://netstorage.unity3d.com/unity/fc1d3344e6ea/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.3.1f1.pkg
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
sudo installer WinSupport.pkg -target /
