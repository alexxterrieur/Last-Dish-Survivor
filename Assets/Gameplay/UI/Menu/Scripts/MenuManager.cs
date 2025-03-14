using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] mainMenuPanel;

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
