using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class that manages saving and loading of scores in a binary file.
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
        SaveData data = new SaveData();

        string path = Application.persistentDataPath + "/Save.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

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
        SaveData data = new SaveData
        {
            activeLanguage = activeLanguage,
            muteVolume = muteVolume,
            gamesPlayed = gamesPlayed,
            gamesWon = gamesWon,
            gamesLost = gamesLost,
            gamesDraw = gamesDraw
        };

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Save.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }
}