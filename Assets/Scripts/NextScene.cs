using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class NextScene : MonoBehaviour
{
    public GameObject manual;
    public GameObject generate;
    public TMPro.TMP_InputField manualInput;
    public TMPro.TMP_InputField generateInput;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (manual.activeSelf)
        {
            List<string> values = manualInput.text.Split(',').ToList();
            PlayerPrefs.SetString("Loot", manualInput.text);
            SceneManager.LoadScene("Island " + values.Count);
        }
        else if (generate.activeSelf)
        {
            string values = "";
            System.Random rand = new System.Random();
            for (int i = 0; i < int.Parse(generateInput.text); i++)
            {
                if (i == int.Parse(generateInput.text) - 1)
                {
                    values += rand.Next(400);
                }
                else
                {
                    values += rand.Next(400) + ",";
                }

            }

            PlayerPrefs.SetString("Loot", values);
            SceneManager.LoadScene("Island " + generateInput.text);
        }
    }
}
