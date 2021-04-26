using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that controls the main functions of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelGame = null;
    [SerializeField] GameObject panelStats = null;
    [SerializeField] GameObject panelInstructions = null;
    [SerializeField] GameObject panelCredits = null;

    [Header("Cards")]
    [SerializeField] Blackjack gameClass = null;
    [SerializeField] Cards[] cardsList = null;

    [Header("Volume")]
    [SerializeField] Image volumeImage = null;
    [SerializeField] Sprite volumeOn = null;
    [SerializeField] Sprite volumeOff = null;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI gamesPlayedText = null;
    [SerializeField] TextMeshProUGUI gamesWonText = null;
    [SerializeField] TextMeshProUGUI gamesLostText = null;
    [SerializeField] TextMeshProUGUI gamesDrawText = null;

    void Awake()
    {
        manager = this;

        CheckVolume();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Screen.height / Screen.width < 2.16)
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.MaximizedWindow);
        }
    }

    /// <summary>
    /// Function that starts the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelGame.SetActive(true);

        gameClass.ResetGame();
    }

    /// <summary>
    /// Function that closes the game and returns to the menu.
    /// </summary>
    public void CloseGame()
    {
        panelMenu.SetActive(true);
        panelGame.SetActive(false);

        GameObject[] cardsInScreen = GameObject.FindGameObjectsWithTag("Card");

        if (cardsInScreen != null)
        {
            for (int i = 0; i < cardsInScreen.Length; i++)
            {
                Destroy(cardsInScreen[i]);
            }
        }
    }

    /// <summary>
    /// Function that opens the statistics panel.
    /// </summary>
    public void OpenStats()
    {
        if (panelStats.activeSelf)
        {
            panelStats.SetActive(false);
        }

        else
        {
            UpdateStats();

            panelStats.SetActive(true);
        }
    }

    /// <summary>
    /// Function that opens the instructions panel.
    /// </summary>
    public void OpenInstructions()
    {
        if (panelInstructions.activeSelf)
        {
            panelInstructions.SetActive(false);
        }

        else
        {
            panelInstructions.SetActive(true);
        }
    }

    /// <summary>
    /// Function that opens the credits panel.
    /// </summary>
    public void OpenCredits()
    {
        if (panelCredits.activeSelf)
        {
            panelCredits.SetActive(false);
        }

        else
        {
            panelCredits.SetActive(true);
        }
    }

    /// <summary>
    /// Function that opens external links.
    /// </summary>
    /// <param name="link">The reference of the link that we want to open.</param>
    public void OpenLink(int link)
    {
        switch (link)
        {
            case 1:
                Application.OpenURL("https://play.google.com/store/apps/developer?id=Sergio+Mejias");
                break;
            case 2:
                Application.OpenURL("https://freesound.org/people/f4ngy/");
                break;
            case 3:
                Application.OpenURL("https://github.com/SergioMejiasDev/Blackjack-Android");
                break;
        }
    }

    /// <summary>
    /// Function that updates the statistics panel.
    /// </summary>
    void UpdateStats()
    {
        gamesPlayedText.text = SaveManager.saveManager.gamesPlayed.ToString();
        gamesWonText.text = SaveManager.saveManager.gamesWon.ToString();
        gamesLostText.text = SaveManager.saveManager.gamesLost.ToString();
        gamesDrawText.text = SaveManager.saveManager.gamesDraw.ToString();
    }

    /// <summary>
    /// Function to obtain playable cards from outside the class.
    /// </summary>
    /// <returns>The list of playable cards.</returns>
    public Cards[] GetCardsList()
    {
        return cardsList;
    }

    /// <summary>
    /// Function that activates or deactivates the sound according to the established configuration.
    /// </summary>
    void CheckVolume()
    {
        if (!SaveManager.saveManager.muteVolume)
        {
            AudioListener.volume = 1f;

            volumeImage.sprite = volumeOn;
        }

        else
        {
            AudioListener.volume = 0f;

            volumeImage.sprite = volumeOff;
        }
    }

    /// <summary>
    /// Function that activates or deactivates the sound.
    /// </summary>
    public void ManageVolume()
    {
        if (SaveManager.saveManager.muteVolume)
        {
            AudioListener.volume = 1f;

            volumeImage.sprite = volumeOn;

            SaveManager.saveManager.muteVolume = false;
        }

        else
        {
            AudioListener.volume = 0f;

            volumeImage.sprite = volumeOff;

            SaveManager.saveManager.muteVolume = true;
        }

        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Function called to close the application.
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}