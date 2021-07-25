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

    public static string Base64Encode(string str)
    {
        string val = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(str));
        return val;
    }

    public static string Base64Decode(string b64)
    {
        string val = System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(b64));
        return val;
    }
}
