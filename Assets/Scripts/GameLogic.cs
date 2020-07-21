using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameLogic : MonoBehaviour
{
    public GameObject[] caves;
    public CoinBar coinBar;
    public GameObject helpMenu;
    public GameObject coinMenu;
    public TMPro.TMP_Text timer;
    private string values;
    private (int result, List<int> loots) res;
    private float currentTime;
    private float startTime;
    private bool isPaused = false;
    private bool isEnd = false;
    public LoseDialogueTrigger loseTrigger;
    public WinDialogueTrigger winTrigger;
    public Animator input;
    public GameObject inputForm;
    // Start is called before the first frame update
    void Start()
    {
        values = PlayerPrefs.GetString("Loot");
        List<string> ar = values.Split(',').ToList();
        List<int> vals = new List<int>();
        foreach (var l in ar)
        {

            vals.Add(int.Parse(l));
        }
        res = solve(vals);
        coinBar.SetMaxCoin(res.result);
        coinBar.SetCoin(0);
        coinMenu.transform.Find("coinText").gameObject.GetComponent<TMPro.TMP_Text>().text = "0/"+coinBar.GetMaxCoin().ToString();
        ResetTime();
        int i = 0;



        foreach (var c in caves)
        {
            TMPro.TMP_Text t =  c.transform.Find("Cave-Coin").gameObject.GetComponent<TMPro.TMP_Text> ();
            t.SetText(ar[i]);
            i++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpMenu.SetActive(!helpMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GiveUp();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMenu();
        }
        if (!isPaused)
        {
            currentTime -= (1 * Time.deltaTime);
            if (timer.text == "0")
            {
                isEnd = true;
                isPaused = true;
            }
            timer.text = currentTime.ToString("0");

        }
        if (!loseTrigger.isTriggered && !winTrigger.isTriggered)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }

            if (!isEnd)
            {
                int a = 0;
                foreach (var cave in caves)
                {
                    if (cave.transform.Find("Cave-Btn").gameObject.activeSelf)
                    {
                        a += 1;
                    }
                }
                if (a > 0 )
                {
                    isEnd = false;
                }
                else
                {
                    isEnd = true;
                }
                if (isEnd)
                {
                    isPaused = true;
                }
            }
            else
            {
                if (coinBar.GetCoin() < coinBar.GetMaxCoin())
                {
                    loseTrigger.TriggerDialogue();
                }
                else
                {
                    winTrigger.TriggerDialogue();
                }
            }
        }
        


    }
    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void ResetTime()
    {
        startTime = GetStartTime();
        currentTime = startTime;
        timer.text = currentTime.ToString();
        isPaused = false;
        isEnd = false;

        loseTrigger.isTriggered = false;
        winTrigger.isTriggered = false;

        input.SetBool("IsOpen", false);
        inputForm.SetActive(false);
        

    }
    private float GetStartTime()
    {
        float multiplier = 0f;
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                multiplier = 6f;
                break;
            case "Medium":
                multiplier = 4f;
                break;
            case "Hard":
                multiplier = 2f;
                break;
            default:
                multiplier = 6f;
                break;
        }
        return caves.Length * multiplier;
    }

    private void ResetLevel()
    {
        foreach (var cave in caves)
        {
            cave.transform.Find("Cave").gameObject.SetActive(true);
            cave.transform.Find("Cave-Btn").gameObject.SetActive(true);
            cave.transform.Find("Cave-Coin").gameObject.SetActive(true);
            cave.transform.Find("Cave-Exploded").gameObject.SetActive(false);
            cave.transform.Find("Cave-Coin").gameObject.GetComponent<TMPro.TMP_Text>().color = Color.black;

        }

        coinBar.SetCoin(0);
        coinMenu.transform.Find("coinText").gameObject.GetComponent<TMPro.TMP_Text>().text = "0/" + coinBar.GetMaxCoin().ToString();

        ResetTime();
    }

    private void GiveUp()
    {
        for (int i = 0; i < caves.Length; i++)
        {
            if (res.loots.Contains(i))
            {
                caves[i].transform.Find("Cave").gameObject.SetActive(true);
                caves[i].transform.Find("Cave-Coin").gameObject.SetActive(true);
                caves[i].transform.Find("Cave-Exploded").gameObject.SetActive(false);
                caves[i].transform.Find("Cave-Coin").gameObject.GetComponent<TMPro.TMP_Text>().color = Color.green;
                caves[i].transform.Find("Cave-Btn").gameObject.SetActive(false);
            }
            else
            {
                caves[i].transform.Find("Cave").gameObject.SetActive(false);
                caves[i].transform.Find("Cave-Btn").gameObject.SetActive(false);
                caves[i].transform.Find("Cave-Coin").gameObject.SetActive(false);
                caves[i].transform.Find("Cave-Exploded").gameObject.SetActive(true);
            }
        }
        timer.text = "0";
        isEnd = true;
        loseTrigger.TriggerDialogue();
    }

    public void ClaimLoot(int cave)
    {
        int update = cave - 1;
        if (caves.Length != 1)
        {
            if (update == 0)
            {

                caves[1].transform.Find("Cave").gameObject.SetActive(false);
                caves[1].transform.Find("Cave-Btn").gameObject.SetActive(false);
                caves[1].transform.Find("Cave-Coin").gameObject.SetActive(false);
                caves[1].transform.Find("Cave-Exploded").gameObject.SetActive(true);
            }
            else if (cave == caves.Length)
            {
                caves[update - 1].transform.Find("Cave").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Btn").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Coin").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Exploded").gameObject.SetActive(true);
            }
            else
            {
                caves[update - 1].transform.Find("Cave").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Btn").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Coin").gameObject.SetActive(false);
                caves[update - 1].transform.Find("Cave-Exploded").gameObject.SetActive(true);

                caves[update + 1].transform.Find("Cave").gameObject.SetActive(false);
                caves[update + 1].transform.Find("Cave-Btn").gameObject.SetActive(false);
                caves[update + 1].transform.Find("Cave-Coin").gameObject.SetActive(false);
                caves[update + 1].transform.Find("Cave-Exploded").gameObject.SetActive(true);
            }
        }
        
        

        caves[update].transform.Find("Cave-Coin").gameObject.GetComponent<TMPro.TMP_Text>().color = Color.green;
        caves[update].transform.Find("Cave-Btn").gameObject.SetActive(false);
        coinBar.SetCoin(coinBar.GetCoin() + int.Parse(caves[update].transform.Find("Cave-Coin").gameObject.GetComponent<TMPro.TMP_Text>().text));
        coinMenu.transform.Find("coinText").gameObject.GetComponent<TMPro.TMP_Text>().text = coinBar.GetCoin().ToString() + "/" + coinBar.GetMaxCoin().ToString();
    }

    static (int value, List<int> path) solve(List<int> vals)
    {
        int c = vals.Count;
        List<int> res = new List<int>();

        int[,] dp = new int[c, 2];
        dp[0, 1] = vals[0];
        for (int i = 1; i < c; i++)
        {
            dp[i, 1] = dp[i - 1, 0] + vals[i];
            dp[i, 0] = Math.Max(dp[i - 1, 0], dp[i - 1, 1]);
        }

        for (int i = c - 1; i >= 0; i--)
        {
            if (dp[i, 1] > dp[i, 0])
            {
                res.Add(i);
                i -= 1;
            }
        }
        res.Reverse();
        int result = 0;
        foreach (var r in res)
        {
            result += vals[r];
        }
        return (result, res);
    }
}
