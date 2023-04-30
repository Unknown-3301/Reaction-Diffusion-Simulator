using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RDTabsManager : MonoBehaviour
{
    [SerializeField] private bool preLoad;
    [SerializeField] private GameObject tabsContainer;
    [SerializeField] private GameObject menusContainer;

    private Image[] tabsImages;
    private int preTabIndex = -1;

    private void Awake()
    {
        tabsImages = new Image[tabsContainer.transform.childCount];
        for (int i = 0; i < tabsImages.Length; i++)
        {
            tabsImages[i] = tabsContainer.transform.GetChild(i).GetComponent<Image>();
        }

        if (preLoad)
        {
            for (int i = 0; i < tabsContainer.transform.childCount; i++)
            {
                menusContainer.transform.GetChild(i).gameObject.SetActive(true);
                menusContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        OpenTab(0);
    }
    public void OpenTab(int index)
    {
        if (preTabIndex != -1)
        {
            tabsImages[preTabIndex].color = new Color(0.1698113f, 0.1698113f, 0.1698113f);
            menusContainer.transform.GetChild(preTabIndex).gameObject.SetActive(false);
        }

        tabsImages[index].color = new Color(0.2156863f, 0.2156863f, 0.2156863f);
        menusContainer.transform.GetChild(index).gameObject.SetActive(true);

        preTabIndex = index;
    }
}
