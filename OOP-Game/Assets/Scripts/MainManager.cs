using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public string playerName;
    
    public bool gameStarted = false;
    public TMP_InputField playerNameText;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartGame()
    {
        playerName = playerNameText.text;
        if (CheckName())
        {
            Debug.Log("Success");
            gameStarted = true;
            GoToScene(1);
        }
        else
        {
            Debug.Log("Failure");
        }
    }

    private bool CheckName()
    {

        if(!(InRange(playerName[0], 65, 90) || InRange(playerName[0], 97, 122))){
            Debug.Log("First");
            return false;
        }
        else
        {
            for(int i=1; i<playerName.Length; i++)
            {
                if (!(InRange(playerName[i], 65, 90) || InRange(playerName[i], 97, 122) || InRange(playerName[i], 48, 57)))
                {
                    Debug.Log("Second");
                    return false;
                }
                
            }
        }
        return true;
    }

    private bool InRange(int currentChar, int start, int end)
    {
        if (currentChar >= start && currentChar <= end) return true;
        else return false;
    }

    public static void GoToScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
