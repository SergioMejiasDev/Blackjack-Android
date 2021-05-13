using System;

/// <summary>
/// Class with the possible options variables that can be saved.
/// </summary>
[Serializable]
public class SaveData
{
    public string activeLanguage;
    public bool muteVolume;
    public int gamesPlayed;
    public int gamesWon;
    public int gamesLost;
    public int gamesDraw;
}