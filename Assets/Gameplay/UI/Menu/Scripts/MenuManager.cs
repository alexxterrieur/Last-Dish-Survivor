using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] mainMenuPanel;
    public GameObject playerSelectionPanel;
    public GameObject StorePanel;
    public GameObject settingsnPanel;

    public GameObject GameModeSelectionPanel;
    public GameObject levelSelectionPanel;

    public void ChangeScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenPanel(GameObject panelToEnable)
    {
        foreach(GameObject panel in mainMenuPanel)
        {
            panel.SetActive(false);
        }
        panelToEnable.SetActive(true);
    }

    public void CloseButton()
    {
        Application.Quit();
    }
}
