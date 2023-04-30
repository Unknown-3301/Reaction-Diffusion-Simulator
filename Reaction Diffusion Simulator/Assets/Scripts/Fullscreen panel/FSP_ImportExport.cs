using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

public class FSP_ImportExport : MonoBehaviour
{
    [SerializeField] private ReactionDiffusion reactionDiffusion;

    ExtensionFilter[] filters;

    private void Awake()
    {
        filters = new ExtensionFilter[]
        {
            new ExtensionFilter("Image Files", "png", "jpg")
        };
    }

    public void Import()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", filters, false);
        if (paths.Length == 0)
            return;

        string path = paths[0];

        Texture2D texture = new Texture2D(5, 5);
        ImageConversion.LoadImage(texture, File.ReadAllBytes(path));

        reactionDiffusion.importImage(texture);
    }
    public void Export()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save Image", "", "RDImage", "png");

        if (string.IsNullOrEmpty(path))
            return;

        SaveFrame(path);
    }

    void SaveFrame(string path)
    {
        RenderTexture currentRT = RenderTexture.active;


        RenderTexture.active = reactionDiffusion.Image;

        Texture2D texture2D = new Texture2D(reactionDiffusion.Image.width, reactionDiffusion.Image.height);

        texture2D.ReadPixels(new Rect(0, 0, reactionDiffusion.Image.width, reactionDiffusion.Image.height), 0, 0);
        texture2D.Apply();

        File.WriteAllBytes(path, texture2D.EncodeToJPG(100));

        RenderTexture.active = currentRT;
    }
}
