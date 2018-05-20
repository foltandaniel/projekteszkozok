using UnityEditor;
namespace Auto
{
    class AutoBuild
    {
        [MenuItem("Tools/Clear PlayerPrefs")]
        private static void NewMenuOption()
        {
            AutoBuild.PerformBuild();
        }
        static void PerformBuild()
        {
			string[] scenes = { "Assets/Scenes/Menu.unity","Assets/Scenes/SinglePlayerScene.unity", "Assets/Scenes/MultiPlayerScene.unity" };

			EditorPrefs.SetString("AndroidSdkRoot", "/home/tools/");
			EditorPrefs.SetString("JdkPath", "/usr/lib/jvm/java-8-oracle");

            BuildPipeline.BuildPlayer(scenes, "build/Win32/Win.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
       
             BuildPipeline.BuildPlayer(scenes, "build/Android/android-build.apk", BuildTarget.Android, BuildOptions.None);
        }
    }
}
