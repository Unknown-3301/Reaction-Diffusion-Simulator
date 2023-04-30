using UnityEngine;
using TMPro;

public class SimulationSL : MonoBehaviour
{
    private static SimulationSL manager;

    [SerializeField] private GameObject simulationElementPrefab;
    [SerializeField] private RectTransform elementsContainer;
    [SerializeField] private GameObject saveObj;
    [SerializeField] private GameObject loadObj;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI warningText;

    public static string path { get; private set; }
    public static string extention { get => ".SData"; }

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
        path = Application.persistentDataPath + "/Simulation Saves";

        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        SimulationContainer[] settings = GameManager.LoadAll<SimulationContainer>(path);
        for (int i = 0; i < settings.Length; i++)
        {
            CreateElement(settings[i]);
        }
    }

    private void CreateElement(SimulationContainer settings) => Instantiate(simulationElementPrefab, elementsContainer).GetComponent<SimulationElement>().Init(settings);

    public static void Load(SimulationContainer settings)
    {
        SimulationSettingsManager.SetSettingsNonSilent(settings.Settings);

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

        SimulationContainer settings = new SimulationContainer()
        {
            Name = name,
            Settings = SimulationSettingsManager.Settings
        };

        GameManager.Save(settings, path + "/" + name + extention);

        CreateElement(settings);

        CloseAll();
    }

    private bool FoundDuplicate(string name)
    {
        for (int i = 0; i < elementsContainer.childCount; i++)
        {
            if (elementsContainer.GetChild(i).GetComponent<SimulationElement>().Name == name)
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
