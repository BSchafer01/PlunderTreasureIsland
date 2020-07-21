using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;

    public void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("DifficultyValue");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("DifficultyValue", dropdown.value);
        PlayerPrefs.SetString("Difficulty", dropdown.options[dropdown.value].text);
    }

    

}
