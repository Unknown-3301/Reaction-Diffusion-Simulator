using UnityEngine;
using TMPro;

public class VisualSL : MonoBehaviour
{
    private static VisualSL manager;

    [SerializeField] private GameObject visualElementPrefab;
    [SerializeField] private RectTransform elementsContainer;
    [SerializeField] private GameObject saveObj;
    [SerializeField] private GameObject loadObj;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI warningText;

    public static string path { get; private set; }
    public static string extention { get => ".VData"; }

    public static bool Active
    {
        get
        {
            if (manager == null)
                return false;

            return manager.gameObject.activeInHierarchy;
        }
    }

    private void Awake() => manager = this;
    private void Start()
    {
        path = Application.persistentDataPath + "/Visual Saves";

        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        VisualSettings[] settings = GameManager.LoadAll<VisualSettings>(path);
        for (int i = 0; i < settings.Length; i++)
        {
            CreateElement(settings[i]);
        }
    }

    private void CreateElement(VisualSettings settings) => Instantiate(visualElementPrefab, elementsContainer).GetComponent<VisualElement>().Init(settings);

    public static void Load(VisualSettings settings)
    {
        BloomManager.SetSettingsNonSilent(settings.BloomSettings);
        ColorsBandManager.SetColorBandsNonSilent(settings.ColorBands);

        manager.CloseAll();
    }
    public void Save()
    {
        string name = inputField.text;

        if (name == string.Empty)
        {
            warningText.text = "It must contain between 1 to 16 Charecters";
            return;
        }

        if (FoundDuplicate(name))
        {
            warningText.text = "This name already exists";
            return;
        }

        warningText.text = "";
        inputField.text = "";

        VisualSettings settings = new VisualSettings()
        {
            Name = name,
            ColorBands = new System.Collections.Generic.List<ColorBandPiece>(ColorsBandManager.ColorBands),
            BloomSettings = BloomManager.Settings
        };

        GameManager.Save(settings, path + "/" + name + extention);

        CreateElement(settings);

        CloseAll();
    }

    private bool FoundDuplicate(string name)
    {
        for (int i = 0; i < elementsContainer.childCount; i++)
        {
            if (elementsContainer.GetChild(i).GetComponent<VisualElement>().Name == name)
            {
                return true;
            }
        }

        return false;
    }

    public void OpenSaveUI()
    {
        gameObject.SetActive(true);
        saveObj.SetActive(true);
        warningText.text = "";
    }
    public void OpenLoadUI()
    {
        gameObject.SetActive(true);
        loadObj.SetActive(true);
    }
    public void CloseAll()
    {
        gameObject.SetActive(false);
        saveObj.SetActive(false);
        loadObj.SetActive(false);
    }
}
