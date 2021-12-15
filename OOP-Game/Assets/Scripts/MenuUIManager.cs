using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuUIManager : MonoBehaviour
{
    private static string playerName;
    private static int playerIndex;

    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Image[] playerIcons;
    [SerializeField] private TMP_InputField playerNameText;
    [SerializeField] private Transform playerPreviewTransform;
    [SerializeField] private TextMeshProUGUI[] playersStats;
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip startFailedSound;
    [SerializeField] private AudioClip startSuccessSound;


    private GameObject currentPlayer;
    private bool characterChosen = false;
    private Image currentImage;
    private int prevIndex;
    private AudioSource audioSource;
    


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    public void StartGame()
    {
        playerName = playerNameText.text;
        if (CheckName() && characterChosen)
        {
            Debug.Log("Success");
            MainManager.playerName = playerName;
            MainManager.playerIndex = playerIndex;
            audioSource.PlayOneShot(startSuccessSound);
            GoToScene(1);
        }
        else
        {
            audioSource.PlayOneShot(startFailedSound);
            Debug.Log("Failure");
        }
    }

    private bool CheckName()
    {
        if (playerName.Length == 0) return false;
        if (!(InRange(playerName[0], 65, 90) || InRange(playerName[0], 97, 122)))
        {
            Debug.Log("First");
            return false;
        }
        else
        {
            for (int i = 1; i < playerName.Length; i++)
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

    public void SelectCharacter(int index)
    {
        playerIndex = index;
        characterChosen = true;
        Destroy(currentPlayer);
        currentPlayer = Instantiate(playerPrefabs[index]);
        CopyTransform(ref currentPlayer, playerPreviewTransform);
        currentPlayer.AddComponent<PlayerPreviewController>();

        if(currentImage!=null)
        RemoveHightlightFromIcon(currentImage);
        currentImage = playerIcons[index];
        HighlightIcon(currentImage);

        if (playersStats[prevIndex] != null) playersStats[prevIndex].enabled = false;
        playersStats[index].enabled = true;
        prevIndex = index;

        audioSource.PlayOneShot(selectSound);
    }

    private void CopyTransform(ref GameObject gameObject, Transform trans)
    {
        gameObject.transform.position = trans.position;
        gameObject.transform.rotation = trans.rotation;
        gameObject.transform.localScale = trans.localScale;
    }

    private void HighlightIcon(Image image)
    {
        image.color = Color.yellow;
    }

    private void RemoveHightlightFromIcon(Image image)
    {
        image.color = Color.white;
    }
}
