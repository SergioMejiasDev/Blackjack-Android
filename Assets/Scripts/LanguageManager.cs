using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that is in charge of modifying the texts according to the active language.
/// </summary>
public class LanguageManager : MonoBehaviour
{
    public static LanguageManager languageManager;

    public string activeLanguage;

    [SerializeField] Image flag = null;
    [SerializeField] Sprite flagEN = null;
    [SerializeField] Sprite flagES = null;

    [SerializeField] GameObject panelMenu = null;

    private void Awake()
    {
        languageManager = this;

        activeLanguage = SaveManager.saveManager.activeLanguage;

        SelectFlag();
    }

    /// <summary>
    /// Function that is responsible for changing the language of all the texts in the game.
    /// </summary>
    /// <param name="newLanguage">The code of the language that we want to activate.</param>
    void ChangeLanguage(string newLanguage)
    {
        activeLanguage = newLanguage;

        SelectFlag();

        SaveManager.saveManager.activeLanguage = newLanguage;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Active language toggle function.
    /// </summary>
    public void AlternateLanguage()
    {
        switch (activeLanguage)
        {
            case "EN":
                ChangeLanguage("ES");
                break;
            case "ES":
                ChangeLanguage("EN");
                break;
        }

        panelMenu.SetActive(false);
        panelMenu.SetActive(true);
    }

    /// <summary>
    /// Function that updates the image of the flag based on the active language.
    /// </summary>
    void SelectFlag()
    {
        switch (activeLanguage)
        {
            case "EN":
                flag.sprite = flagEN;
                break;
            case "ES":
                flag.sprite = flagES;
                break;
        }
    }
}