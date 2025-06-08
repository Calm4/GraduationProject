using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseOpenerUI : MonoBehaviour
{
    [Header("UI Панель, которую нужно открыть")]
    public GameObject panelToOpen;

    [Header("Кнопка, по нажатию которой откроется панель")]
    public Button openButton;

    void Start()
    {
        if (openButton != null)
        {
            openButton.onClick.AddListener(OpenPanel);
        }
    }

    void OpenPanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
            var cg = panelToOpen.GetComponent<CanvasGroup>();
            cg.alpha = 1;
        }
    }
}
