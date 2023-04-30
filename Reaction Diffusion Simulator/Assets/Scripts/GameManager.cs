using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Toggle fullScreenToggle;

    public static bool SelectedInputField { get; private set; }

    private void Awake() => FullScreen(true);

    void Update()
    {
        if (!ReactionDiffusion.FullScreen)
        {
            float ratio = (float)Screen.width / Screen.height;
            float minRatio = 16f / 9;

            if (ratio > minRatio)
            {
                Screen.SetResolution((int)(Screen.height * minRatio), Screen.height, false);
            }
        }
    }

    public void OnInputFieldSelect() => SelectedInputField = true;
    public void OnInputFieldDeselect() => SelectedInputField = false;

    public void FullScreen(bool full)
    {
        Screen.fullScreen = full;
        fullScreenToggle.isOn = full;
    }
    public static void Quit() => Application.Quit();
    public static float StringToFloat(string text, float min, float max)
    {
        if (string.IsNullOrEmpty(text) || text == "-" || text == ".")
        {
            text = "0";
        }

        return Mathf.Clamp(float.Parse(text), min, max);
    }
    public static float StringToFloat(string text)
    {
        if (string.IsNullOrEmpty(text) || text == "-" || text == ".")
        {
            text = "0";
        }

        return float.Parse(text);
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * transform.pivot), size);
    }
    /// <summary>
    /// Saves to The current path
    /// </summary>
    /// <param name="Object">The object to save</param>
    /// <param name="path">The path to save the object in</param>
    public static void Save<T>(T Object, string path) where T : class
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, Object);
        stream.Close();
    }
    /// <summary>
    /// Load and return the saved object in the current path
    /// </summary>
    /// <returns></returns>
    public static T Load<T>(string path) where T : class
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T Object = formatter.Deserialize(stream) as T;
            stream.Close();

            return Object;
        }
        else
        {
            throw new System.Exception($"LoadError: there is no file in {path} to load!!");
        }
    }

    public static T[] LoadAll<T>(string path) where T : class
    {
        if (Directory.Exists(path))
        {
            string[] paths = Directory.GetFiles(path);
            T[] output = new T[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                output[i] = Load<T>(paths[i]);
            }

            return output;
        }
        else
        {
            throw new System.Exception($"LoadError: there is no folder in {path} to load from!!");
        }
    }

    public static void Delete(string path)
    {
        File.Delete(path);
    }
}
