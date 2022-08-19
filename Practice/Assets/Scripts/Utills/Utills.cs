using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utills : MonoBehaviour
{
    public static List<T> GetFileListInDir<T>(string path) where T : Object
    {
        DirectoryInfo dirInfo = new DirectoryInfo($"Assets/Resources/" + path);
        int count = dirInfo.GetFiles().Length;
        List<T> fileList = new List<T>();
        for (int i = 0; i < count; i++)
        {
            string str = dirInfo.GetFiles()[i].ToString();
            if (!(str.Contains("meta")))
            {
                int slashIdx = str.LastIndexOf('\\');
                if (slashIdx >= 0)
                {
                    str = str.Substring((slashIdx + 1));
                    int pointIdx = str.LastIndexOf('.');
                    str = str.Substring(0, pointIdx);
                    str = $"{path}" + str;
                    T content = Managers.Resource.Load<T>(str);
                    fileList.Add(content);
                    str = "";
                }
            }
        }
        return fileList;
    }
}
