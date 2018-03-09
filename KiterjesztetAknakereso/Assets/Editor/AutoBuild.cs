using UnityEditor;
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
        BuildPipeline.BuildPlayer(scenes," /Users/travis/build/foltandaniel/projekteszkozok/WIN",BuildTarget.StandaloneWindows,BuildOptions.None);
    }
}
