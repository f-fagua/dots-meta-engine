using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ColorPaletteReader
{
    [MenuItem("Unity Support/Color Palettes/Read Color Palette")]
    public static void ReadColorPalette()
    {
        var colorPalettesPath = Path.Combine("Assets", "Data", "Text Files");
        string filePath = EditorUtility.OpenFilePanel("Select Texture", colorPalettesPath, "txt");

        if (!string.IsNullOrEmpty(filePath))
            ReadColorPaletteTextFile(filePath);
    }

    private static void ReadColorPaletteTextFile(string filePath)
    {
        string relativePath = "Assets" + filePath.Substring(Application.dataPath.Length);
            
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                var colors = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var currentColorStr = (++colors).ToString("D3");
                    Debug.Log($"Color {currentColorStr}: {line}." );

                    if (ColorUtility.TryParseHtmlString(currentColorStr, out var color))
                    {
                        Debug.Log($"{color.r}, {color.g}, {color.b}");    
                    }
                    else 
                        Debug.Log("Couldnt parse this shit!");
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
