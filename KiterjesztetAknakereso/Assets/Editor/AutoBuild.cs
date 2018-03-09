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
            string[] scenes = { "Assets/Scenes/Menu.unity" };
            BuildPipeline.BuildPlayer(scenes, "Win.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        }
    }
}