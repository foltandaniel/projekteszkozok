using UnityEditor;
class AutoBuild
{
    static void PerformBuild()
    {
        string[] scenes = { "Assets/Scenes/Menu.unity" };
        BuildPipeline.BuildPlayer(scenes,".",BuildTarget.StandaloneWindows,BuildOptions.None);
    }
}
