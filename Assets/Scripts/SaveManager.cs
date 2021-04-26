using System.IO;
using UnityEngine;

/// <summary>
/// Class that manages saving and loading of scores in a JSON file.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    [Header("Options")]
    public string activeLanguage = "EN";
    public bool muteVolume = false;

    [Header("Stats")]
    public int gamesPlayed = 0;
    public int gamesWon = 0;
    public int gamesLost = 0;
    public int gamesDraw = 0;

    private void Awake()
    {
        saveManager = this;

        LoadOptions();
    }

    /// <summary>
    /// Function to save options values.
    /// </summary>
    public void LoadOptions()
    {
        OptionsData data = new OptionsData();

        string json;

        string path = Application.persistentDataPath + "/Save.json";

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            JsonUtility.FromJsonOverwrite(json, data);

            activeLanguage = data.activeLanguage;
            muteVolume = data.muteVolume;
            gamesPlayed = data.gamesPlayed;
            gamesWon = data.gamesWon;
            gamesLost = data.gamesLost;
            gamesDraw = data.gamesDraw;
        }
    }

    /// <summary>
    /// Function to load options values.
    /// </summary>
    public void SaveOptions()
    {
        OptionsData data = new OptionsData
        {
            activeLanguage = activeLanguage,
            muteVolume = muteVolume,
            gamesPlayed = gamesPlayed,
            gamesWon = gamesWon,
            gamesLost = gamesLost,
            gamesDraw = gamesDraw
        };

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "/Save.json";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }
}