using System.IO;
using UnityEngine;

public class Utills : MonoBehaviour
{
    // public static string[] GetFilesInDirectoty(string path)
    // {
    //     DirectoryInfo dirInfo = new DirectoryInfo($"Assets/Resources/Prefabs/{path}");
    //     int count = dirInfo.GetFiles().Length;
    //     string[] files = new string[count];
    //     int j = 0;
    //     for (int i = 0; i < count; i++)
    //     {
    //         string str = dirInfo.GetFiles()[i].ToString();
    //         if (!(str.Contains("meta")))
    //         {
    //             int slashIdx = str.LastIndexOf('\\');
    //             if (slashIdx >= 0)
    //             {
    //                 str = str.Substring((slashIdx + 1));
    //                 int pointIdx = str.LastIndexOf('.');
    //                 str = str.Substring(0, pointIdx);
    //             }
    //             files[j] = str;
    //             j++;
    //         }
    //     }
    //     return files;
    // }
}
