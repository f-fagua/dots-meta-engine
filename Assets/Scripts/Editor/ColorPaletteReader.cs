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
        {
            ReadColorPaletteTextFile(filePath, out var colors);
            if (colors is { Count: > 0 })
            {
                CreateTexture(colors);
            }
        }
    }

    private static void ReadColorPaletteTextFile(string filePath, out List<Color> colorPalette)
    { // string relativePath = "Assets" + filePath.Substring(Application.dataPath.Length);
        colorPalette = new List<Color>();
            
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                var colors = 0;
                while (reader.ReadLine() is { } line)
                {
                    var currentColorStr = (++colors).ToString("D3");
                    GetRGB(line, out int r, out int g, out int b);
                    
                    Debug.Log($"Color {currentColorStr}: {line}." );
                    
                    var color = new Color(r/(float)255, g/(float)255, b/(float)255);
                    colorPalette.Add(color);
                    
                    Debug.Log($" Color {color}: {r}, {g}, {b}." );
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
    
    private static void CreateTexture(List<Color> colorPalette)
    {
        Texture2D texture = new Texture2D(colorPalette.Count, 1);
        
        for (int x = 0; x < colorPalette.Count; x++)
            texture.SetPixel(x, 1, colorPalette[x]);
        
        texture.Apply();
        SaveTextureAsAsset(texture, "GeneratedTexture.jpeg");
    }

    private static void SaveTextureAsAsset(Texture2D texture, string path)
    {
        
        byte[] bytes = texture.EncodeToJPG();
        Path.Combine(Application.dataPath, path);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Texture saved to: " + path);
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    private static void GetRGB(string line, out int r, out int g, out int b)
    {
        var strR = line.Substring(1, 2);
        Debug.Log(strR);
        r = int.Parse(strR, System.Globalization.NumberStyles.HexNumber);
        
        var strG = line.Substring(3, 2);
        Debug.Log(strG);
        g = int.Parse(strG, System.Globalization.NumberStyles.HexNumber);
        
        var strB = line.Substring(5, 2);
        Debug.Log(strB);
        b = int.Parse(strB, System.Globalization.NumberStyles.HexNumber);
    }
}
