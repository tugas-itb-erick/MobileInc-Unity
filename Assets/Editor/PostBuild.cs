using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;

public class PostBuild
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Process process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.FileName = "copy.bat";
        process.Start();
    }
}