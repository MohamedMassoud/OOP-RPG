using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private Creature levelTrollScript;
    [SerializeField] private Creature levelWolfScript;
    [SerializeField] private GameObject trollDoor;
    [SerializeField] private GameObject wolfDoor;
    private static GameObject gameOverCanvas;
    private static GameObject gameEndCanvas;
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Sprite[] skillIcons;
    [SerializeField] private Image currentIcon;
    [SerializeField] private Transform playerStartTransfrom;
    

    private Player playerScript;
    


    private bool checkTroll = true;
    private bool checkWolf = true;
    private float doorUnlockSpeed = 3f;
    private float bottomBoundary = -10f;

    private void Awake()
    {
        
        StartGame();
        
    }
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<Player>();
        gameOverCanvas = GameObject.Find("Game Over").transform.GetChild(0).gameObject;
        gameEndCanvas = GameObject.Find("Game End").transform.GetChild(0).gameObject;
    }

    private void StartGame()
    {
        LoadPlayer();
        LoadIcon();
        
    }

    private void LoadIcon()
    {
        currentIcon.sprite = skillIcons[MainManager.playerIndex];
    }

    private void LoadPlayer()
    {
        Instantiate(playerPrefabs[MainManager.playerIndex], playerStartTransfrom.position, playerStartTransfrom.rotation);
    }
    void FixedUpdate()
    {
        if (checkTroll && !levelTrollScript.isAlive)
        {
            if (!UnlockDoor(trollDoor))
            {
                checkTroll = false;
            }
        }

        if(checkWolf && !levelWolfScript.isAlive)
        {
            if (!UnlockDoor(wolfDoor))
            {
                checkWolf = false;
            }
        }

        if(!checkTroll && !checkWolf)
        {
            Congrats();
        }

    }

    private bool UnlockDoor(GameObject door)
    {
        if (door.transform.position.y > bottomBoundary)
        {
            door.transform.Translate(-door.transform.up * doorUnlockSpeed * Time.fixedDeltaTime, Space.World);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void Congrats()
    {
        playerScript.enabled = false;
        gameEndCanvas.SetActive(true);
    }

    public static void GameOver()
    {
        gameOverCanvas.SetActive(true);
    }



    public void GoToSelectionScreen()
    {
        SceneManager.LoadScene(0);
    }
}
