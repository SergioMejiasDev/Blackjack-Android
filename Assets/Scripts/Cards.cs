using UnityEngine;

/// <summary>
/// Class that stores the different variables of the cards.
/// </summary>
[CreateAssetMenu(menuName = "Card Object")]
public class Cards : ScriptableObject
{
    [SerializeField] int value;
    [SerializeField] Sprite image;

    /// <summary>
    /// Function to obtain the value of the card.
    /// </summary>
    /// <returns>The value of the card.</returns>
    public int GetValue()
    {
        return value;
    }

    /// <summary>
    /// Function to obtain the sprite of the card.
    /// </summary>
    /// <returns>The sprite of the card.</returns>
    public Sprite GetSprite()
    {
        return image;
    }
}