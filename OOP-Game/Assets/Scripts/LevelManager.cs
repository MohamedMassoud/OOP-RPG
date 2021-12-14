using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private Creature levelTrollScript;
    [SerializeField] private Creature levelWolfScript;
    [SerializeField] private GameObject trollDoor;
    [SerializeField] private GameObject wolfDoor;
    [SerializeField] private GameObject GameOverCanvas;
    private Player playerScript;
    


    private bool checkTroll = true;
    private bool checkWolf = true;
    private float doorUnlockSpeed = 3f;
    private float bottomBoundary = -10f;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
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

        if (!playerScript.isAlive)
        {
            GameOver();
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

    }

    private void GameOver()
    {
        GameOverCanvas.SetActive(true);
    }

    public void GoToSelectionScreen()
    {
        SceneManager.LoadScene(0);
    }
}
