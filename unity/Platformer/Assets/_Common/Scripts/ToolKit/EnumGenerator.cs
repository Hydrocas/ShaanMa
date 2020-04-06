///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 27/02/2020 21:09
///-----------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class EnumGenerator
{
    public static void Write(string filePath, string enumName, string[] enumEntries)
    {
        string filePathAndName = filePath + enumName + ".cs";
        
        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("public enum " + enumName);
            streamWriter.WriteLine("{");

            string enumEntrie;

            for (int i = 0; i < enumEntries.Length; i++)
            {
                enumEntrie = enumEntries[i].Trim(' ');
                streamWriter.WriteLine("\t" + enumEntrie + ",");
            }

            streamWriter.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }
}
#endif