using System.IO;
using System.Windows;
using UnityEngine;
using SFB;

public class ImageImporter : MonoBehaviour
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
    public void OnClick()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", filters, false);
        if (paths.Length == 0)
            return;

        string path = paths[0];

        Texture2D texture = new Texture2D(5, 5);
        ImageConversion.LoadImage(texture, File.ReadAllBytes(path));

        reactionDiffusion.importImage(texture);
    }
}
