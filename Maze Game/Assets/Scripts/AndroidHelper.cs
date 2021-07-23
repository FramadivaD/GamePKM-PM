using UnityEngine;
using System.Collections;
using System.IO;

public class AndroidHelper
{
    public static string MainGateSavePath { get { return GetAndroidExternalStoragePath() + "/MathGame"; } }

    public static string GetAndroidExternalStoragePath()
    {
        string path = "";
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
            path = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
        }
        catch { path = @"D:/Music"; }

        return path;
    }

    public static void CheckAndCreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
