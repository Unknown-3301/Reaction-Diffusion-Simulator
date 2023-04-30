using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecieManager : MonoBehaviour
{
    public static SpecieManager manager;

    [SerializeField] private GameObject specieElementPrefab;
    [SerializeField] private GameObject listContainer;
    [SerializeField] private TMP_InputField nameInput;

    private List<SpecieElement> species;
    private SpecieElement selectedElement;

    public static int NumberOfSpecies { get { return manager.species.Count; } }
    public static bool IsInputOnFocused
    {
        get
        {
            if (manager == null)
                return false;

            return manager.nameInput.isFocused;
        }
    }

    public static ListComputeBuffer<SimulationSettings> SimulationBuffer { get; private set; }
    public static ListComputeBuffer<ColorBandPiece> ColorsBuffer { get; private set; }

    public static Action OnAddSpecie { get; set; }
    public static Action<int> OnDeleteSpecie { get; set; }
    public static Action<int> OnSelectSpecie { get; set; }
    public static Action OnManagerDisable { get; set; }

    private void OnEnable()
    {
        if (manager == null)
        {
            manager = this;
            species = new List<SpecieElement>();

            ColorsBandManager.OnChange = OnColorChange;
            SimulationSettingsManager.OnChange = OnSimulationChange;

            AddSpecie("specie");
        }
    }
    private void OnDisable()
    {
        if (!gameObject.activeSelf)
        {
            ColorsBandManager.OnChange = null;
            SimulationSettingsManager.OnChange = null;

            manager = null;

            for (int i = 0; i < species.Count; i++)
            {
                Destroy(species[i].gameObject);
            }

            species = null;

            SimulationBuffer.Buffer.Release();
            ColorsBuffer.Buffer.Release();

            ColorsBuffer = null;
            SimulationBuffer = null;

            OnManagerDisable();
        }
    }

    public void OnColorChange()
    {
        selectedElement.SetColors(ColorsBandManager.ColorBands);
        ColorsBuffer.SetRange(selectedElement.Index * ColorsBandManager.NumberOfBands, ColorsBandManager.ColorBands);
    }
    public void OnSimulationChange()
    {
        selectedElement.SimulationSettings = SimulationSettingsManager.Settings;
        SimulationBuffer.SetData(selectedElement.Index, selectedElement.SimulationSettings);
    }

    public void TryAddSpecie()
    {
        string name = nameInput.text;

        if (name == "")
            return;

        if (species.Any(x => x.Name == name))
        {
            return;
        }

        AddSpecie(name);
        nameInput.text = "";
    }
    public void AddSpecie(string name)
    {
        SpecieElement element = Instantiate(specieElementPrefab, listContainer.transform).GetComponent<SpecieElement>();
        element.Index = species.Count;
        element.SimulationSettings = SimulationSettingsManager.Settings;
        element.SetColors(ColorsBandManager.ColorBands);
        element.Deletable = species.Count != 0;
        element.Name = name;

        if (species.Count == 1)
        {
            species[0].Deletable = true;
        }
        species.Add(element);

        if (manager.selectedElement != null)
        {
            manager.selectedElement.Selected = false;
        }
        element.Selected = true;
        manager.selectedElement = element;

        if (ColorsBuffer == null)
        {
            ColorsBuffer = new ListComputeBuffer<ColorBandPiece>(new List<ColorBandPiece>(element.ColorBands), ColorBandPiece.Size);
        }
        else
        {
            ColorsBuffer.AddRange(ColorsBandManager.ColorBands);
        }

        if (SimulationBuffer == null)
        {
            SimulationBuffer = new ListComputeBuffer<SimulationSettings>(SimulationSettingsManager.Settings, SimulationSettings.Size);
        }
        else
        {
            SimulationBuffer.AddData(SimulationSettingsManager.Settings);
        }

        OnAddSpecie();
        OnSelectSpecie(species.Count - 1);
    }

    public static void SelectSpecie(SpecieElement specie)
    {
        if (manager.selectedElement == specie)
            return;
        else if(manager.selectedElement != null)
            manager.selectedElement.Selected = false;

        specie.Selected = true;
        manager.selectedElement = specie;

        SimulationSettingsManager.SetSettingsSilent(specie.SimulationSettings);
        ColorsBandManager.SetColorBandsSilent(specie.ColorBands);

        OnSelectSpecie(specie.Index);
    }
    public static void SelectSpecie(int index)
    {
        if (index < 0)
            return;

        if (index >= manager.species.Count)
            return;

        SelectSpecie(manager.species[index]);
    }
    public static void DeleteSpecie(int index)
    {
        SpecieElement element = manager.species[index];
        manager.species.RemoveAt(index);

        for (int i = index; i < manager.species.Count; i++)
        {
            manager.species[i].Index--;
        }
        int newIndex = index >= manager.species.Count ? index - 1 : index;

        if (element == manager.selectedElement)
        {

            SelectSpecie(manager.species[newIndex]);
        }

        ColorsBuffer.RemoveRange(index * ColorsBandManager.NumberOfBands, ColorsBandManager.NumberOfBands);
        SimulationBuffer.RemoveAt(index);

        Destroy(element.gameObject);

        if (manager.species.Count == 1)
            manager.species[0].Deletable = false;

        OnDeleteSpecie(index);
        OnSelectSpecie(newIndex);
    }

    private void OnDestroy()
    {
        if (SimulationBuffer != null)
            SimulationBuffer.Buffer.Release();

        if (ColorsBuffer != null)
            ColorsBuffer.Buffer.Release();
    }
}
