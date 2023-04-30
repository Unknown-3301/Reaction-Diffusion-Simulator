using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SimulationElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUGUI;

    public string Name { get => nameUGUI.text; set => nameUGUI.text = value; }
    public SimulationContainer Settings { get; private set; }

    public void Init(SimulationContainer settings)
    {
        Settings = settings;
        Name = settings.Name;
    }

    public void Select() => SimulationSL.Load(Settings);
    public void Delete()
    {
        GameManager.Delete(SimulationSL.path + "/" + Name + SimulationSL.extention);
        Destroy(gameObject);
    }
}
