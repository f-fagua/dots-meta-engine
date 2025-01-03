using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MaterialCreator
{
    public static Texture2D s_SelectedTexture;

    public static List<Material> s_Materials;

    [MenuItem("Unity Support/Materials/Create Color Palette", priority = 1)]
    public static void CreateColorPalette()
    {
        var texturesPath = Path.Combine("Assets", "Textures", "Color Palettes");
        string path = EditorUtility.OpenFilePanel("Select Texture", texturesPath, "png,jpg,tga");

        if (!string.IsNullOrEmpty(path))
        {
            // Load file as Texture2D
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            s_SelectedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(relativePath);

            if (s_SelectedTexture != null)
            {
                s_Materials = new List<Material>();
                ReadTexture(s_SelectedTexture);
            }
            else
            {
                Debug.LogError("Failed to load texture at path: " + relativePath);
            }
        }
    }

    private static void ReadTexture(Texture2D texture)
    {
        s_Materials = new List<Material>();
        
        // Make sure the texture is readable
        if (!texture.isReadable)
        {
            Debug.LogError("Selected texture needs to have 'Read/Write Enabled' set in the import settings.");
            return;
        }

        // Get all pixels of the texture
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;

        var directory = Path.Combine("Assets", "Materials", "Color Palettes", texture.name);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        else
            CleanUpDirectory(directory);
        // Read each pixel
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixelColor = pixels[y * width + x];
                Debug.Log($"Pixel at ({x},{y}): {pixelColor}");

                // Save the material as an asset in the project
                var r = (int) (pixelColor.r * 256);
                var g = (int) (pixelColor.g * 256);
                var b = (int) (pixelColor.b * 256);

                
                var fullPath = Path.Combine(directory, $"r{r}_g{g}_b{b}.mat");
                    
                if (!ColorMaterialExist(pixelColor))
                {
                    var newMaterial = CreateMaterial(pixelColor, fullPath);
                    s_Materials.Add(newMaterial);
                }
            }
        }
    }

    private static void CleanUpDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Target directory does not exist.");
            return;
        }

        // Delete files with specific extension
        var files = Directory.GetFiles(directoryPath, $"*{directoryPath}", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
                Debug.Log($"Deleted file: {file}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"Error deleting file {file}: {ex.Message}");
            }
        }
    }

    private static bool ColorMaterialExist(Color pixelColor)
    {
        foreach (var material in s_Materials)
        {
            if (material.color == pixelColor)
                return true;
        }

        return false;
    }

    [MenuItem("Unity Support/Materials/Create Material (Test)", priority = 2)]
    public static void CreateMaterial()
    {
        string path = Path.Combine("Assets", "Materials", $"r{Color.cyan.r}_g{Color.cyan.g}_b{Color.cyan.b}.mat");
        CreateMaterial(Color.cyan, path);
    }

    private static Material CreateMaterial(Color pixelColor, string path)
    {
        Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        newMaterial.color = pixelColor;

        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(newMaterial, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        return newMaterial;
    }
}
