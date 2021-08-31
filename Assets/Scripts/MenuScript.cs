using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject selectedLevel;

    public void GoToLevel()
    {
        Debug.Log("Clicked");
        selectedLevel = EventSystem.current.currentSelectedGameObject;
        SceneManager.LoadScene(selectedLevel.gameObject.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}