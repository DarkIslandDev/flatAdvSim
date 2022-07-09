using UnityEngine;
using UnityEditor;
using System.IO;

public class GameMenu : EditorWindow 
{

    [MenuItem("Extras/Clear player prefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Extras/Take Screenshot")]
    public static void TakeScreenshot()
    {
        string path = "Screenshots";

        Directory.CreateDirectory(path);

        int i = 0;
        while(File.Exists(path + "/" + i + ".png"))
        {
            i++;
        }

        ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");
    }
}