using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class that controls the different functions within the Blackjack game.
/// </summary>
public class Blackjack : MonoBehaviour
{
    [Header("Cards")]
    List<Cards> cardsList = new List<Cards>();
    [SerializeField] GameObject card = null;
    List<GameObject> cardsPlayer = new List<GameObject>();
    List<GameObject> cardsAI = new List<GameObject>();

    [Header("Score")]
    int scorePlayer1;
    int scorePlayer2;
    int finalScorePlayer;
    int scoreAI1;
    int scoreAI2;
    [SerializeField] TextMeshProUGUI scorePlayer = null;
    [SerializeField] TextMeshProUGUI scoreAI = null;
    enum Result { Victory, Lose, Draw }

    [Header("Buttons")]
    [SerializeField] GameObject[] buttons = null;
    [SerializeField] GameObject[] messages = null;

    /// <summary>
    /// Coroutine that deals the first three cards.
    /// </summary>
    /// <returns></returns>
    IEnumerator InitialSpawn()
    {
        StartCoroutine(InstantiateCardPlayer());

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(InstantiateCardAI());

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(InstantiateCardPlayer());

        yield return new WaitForSeconds(0.75f);

        if (scorePlayer1 != 21)
        {
            EnableButtons(true);
        }

        else
        {
            EndGame(Result.Victory);
        }
    }

    /// <summary>
    /// Function that updates the scoring data on the screen.
    /// </summary>
    void WriteScore()
    {
        if (scorePlayer1 == scorePlayer2)
        {
            scorePlayer.text = scorePlayer1.ToString();
        }

        else if ((scorePlayer1 != scorePlayer2) && scorePlayer1 <= 21)
        {
            scorePlayer.text = scorePlayer1.ToString() + "/" + scorePlayer2.ToString();
        }

        else if ((scorePlayer1 != scorePlayer2) && scorePlayer1 > 21)
        {
            scorePlayer.text = scorePlayer2.ToString();
        }

        if (scoreAI1 == scoreAI2)
        {
            scoreAI.text = scoreAI1.ToString();
        }

        else if ((scoreAI1 != scoreAI2) && scoreAI1 <= 21)
        {
            scoreAI.text = scoreAI1.ToString() + "/" + scoreAI2.ToString();
        }

        else if ((scoreAI1 != scoreAI2) && scoreAI1 > 21)
        {
            scoreAI.text = scoreAI2.ToString();
        }
    }

    /// <summary>
    /// Function that checks if the player has lost the game during his turn.
    /// </summary>
    /// <returns>True if you have not lost, false if you have lost.</returns>
    bool CheckFirstVictory()
    {
        if ((scorePlayer1 > 21 && scorePlayer2 == 0) || scorePlayer2 > 21)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Function that checks if the game has ended after each turn of the computer.
    /// </summary>
    void CheckLastVictory()
    {
        if (scoreAI1 > finalScorePlayer && scoreAI1 <= 21)
        {
            EndGame(Result.Lose);
        }

        else if (scoreAI2 > finalScorePlayer && scoreAI2 <= 21 && scoreAI1 > 21)
        {
            EndGame(Result.Lose);
        }

        else if (scoreAI1 > 21 && scoreAI2 > 21)
        {
            EndGame(Result.Victory);
        }

        else if ((scoreAI1 == finalScorePlayer && scoreAI1 >= 17) || (scoreAI2 == finalScorePlayer && scoreAI2 >= 17))
        {
            EndGame(Result.Draw);
        }

        else
        {
            NewCardAI();
        }
    }

    /// <summary>
    /// Function that we call from the "Stand Up" button.
    /// </summary>
    public void StandUp()
    {
        EnableButtons(false);

        if (!CheckFirstVictory())
        {
            EndGame(Result.Lose);

            return;
        }

        finalScorePlayer = scorePlayer1 <= 21 ? scorePlayer1 : scorePlayer2;

        NewCardAI();
    }

    /// <summary>
    /// Function that activates or deactivates the buttons on the screen.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    public void EnableButtons(bool enable)
    {
        if (enable)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
        }

        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
        }
    }

    /// <summary>
    /// Game ending function.
    /// </summary>
    /// <param name="result">The result of the game.</param>
    void EndGame(Result result)
    {
        switch (result)
        {
            case Result.Victory:
                SaveManager.saveManager.gamesWon += 1;
                messages[0].SetActive(true);
                buttons[2].SetActive(true);
                break;

            case Result.Lose:
                SaveManager.saveManager.gamesLost += 1;
                messages[1].SetActive(true);
                buttons[2].SetActive(true);
                break;

            case Result.Draw:
                SaveManager.saveManager.gamesDraw += 1;
                messages[2].SetActive(true);
                buttons[2].SetActive(true);
                break;
        }

        SaveManager.saveManager.gamesPlayed += 1;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Function that resets the game.
    /// </summary>
    public void ResetGame()
    {
        EnableButtons(false);

        for (int i = 0; i < messages.Length; i++)
        {
            messages[i].SetActive(false);
        }

        GameObject[] cardsInScreen = GameObject.FindGameObjectsWithTag("Card");

        if (cardsInScreen != null)
        {
            for (int i = 0; i < cardsInScreen.Length; i++)
            {
                Destroy(cardsInScreen[i]);
            }
        }

        cardsPlayer.Clear();
        cardsAI.Clear();

        scoreAI1 = 0;
        scoreAI2 = 0;
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        finalScorePlayer = 0;

        WriteScore();

        cardsList.Clear();

        Cards[] tempCards = GameManager.manager.GetCardsList();
        
        for (int i = 0; i < tempCards.Length; i++)
        {
            cardsList.Add(tempCards[i]);
        }

        StartCoroutine(InitialSpawn());
    }

    #region Player

    /// <summary>
    /// Coroutine that deals a new card to the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateCardPlayer()
    {
        ReorganizeCardsPlayer();

        int randomCard = Random.Range(0, cardsList.Count);
        GameObject newCard = Instantiate(card, SpawnPositionPlayer(), Quaternion.identity);
        cardsPlayer.Add(newCard);

        yield return new WaitForSeconds(0.5f);

        newCard.GetComponent<SpriteRenderer>().sprite = cardsList[randomCard].GetSprite();
        UpdateScorePlayer(cardsList[randomCard].GetValue());
        cardsList.Remove(cardsList[randomCard]);

        if (!CheckFirstVictory())
        {
            EndGame(Result.Lose);
            yield break;
        }

        if (cardsPlayer.Count > 2)
        {
            EnableButtons(true);
        }
    }

    /// <summary>
    /// Function to calculate the place where the new card will appear.
    /// </summary>
    /// <returns>Vector 2 where the letter will appear.</returns>
    Vector2 SpawnPositionPlayer()
    {
        int activeCards = cardsPlayer.Count;

        return new Vector2(activeCards * 0.75f, -3.48f);
    }

    /// <summary>
    /// Function that organizes the player's cards on the screen when a new card is requested.
    /// </summary>
    void ReorganizeCardsPlayer()
    {
        int activeCards = cardsPlayer.Count;

        for (int i = 0; i < activeCards; i++)
        {
            cardsPlayer[i].transform.position = new Vector2(cardsPlayer[i].transform.position.x - 0.75f, -3.48f);
        }
    }

    /// <summary>
    /// Function to deal a new card to the player.
    /// </summary>
    public void NewCard()
    {
        EnableButtons(false);

        StartCoroutine(InstantiateCardPlayer());
    }

    /// <summary>
    /// Function that updates the player's score.
    /// </summary>
    /// <param name="score">The score of the card.</param>
    void UpdateScorePlayer(int score)
    {
        if (score != 1)
        {
            scorePlayer1 += score;
            scorePlayer2 += score;
        }

        else
        {
            scorePlayer1 += 11;
            scorePlayer2 += 1;
        }

        WriteScore();
    }

    #endregion

    #region AI

    /// <summary>
    /// Coroutine that deals a new card to the computer.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateCardAI()
    {
        ReorganizeCardsAI();

        int randomCard = Random.Range(0, cardsList.Count);
        GameObject newCard = Instantiate(card, SpawnPositionAI(), Quaternion.identity);
        cardsAI.Add(newCard);

        yield return new WaitForSeconds(0.5f);

        newCard.GetComponent<SpriteRenderer>().sprite = cardsList[randomCard].GetSprite();
        UpdateScoreAI(cardsList[randomCard].GetValue());
        cardsList.Remove(cardsList[randomCard]);

        if (cardsAI.Count > 1)
        {
            yield return new WaitForSeconds(0.25f);

            CheckLastVictory();
        }
    }

    /// <summary>
    /// Function to calculate the place where the new card will appear.
    /// </summary>
    /// <returns>Vector 2 where the letter will appear.</returns>
    Vector2 SpawnPositionAI()
    {
        int activeCards = cardsAI.Count;

        return new Vector2(activeCards * 0.75f, 3.48f);
    }

    /// <summary>
    /// Function that organizes the computer cards on the screen when a new card is requested.
    /// </summary>
    void ReorganizeCardsAI()
    {
        int activeCards = cardsAI.Count;

        for (int i = 0; i < activeCards; i++)
        {
            cardsAI[i].transform.position = new Vector2(cardsAI[i].transform.position.x - 0.75f, 3.48f);
        }
    }

    /// <summary>
    /// Function to deal a new card to the computer.
    /// </summary>
    void NewCardAI()
    {
        StartCoroutine(InstantiateCardAI());
    }

    /// <summary>
    /// Function that updates the computer's score.
    /// </summary>
    /// <param name="score">The score of the card.</param>
    void UpdateScoreAI(int score)
    {
        if (score != 1)
        {
            scoreAI1 += score;
            scoreAI2 += score;
        }

        else
        {
            scoreAI1 += 11;
            scoreAI2 += 1;
        }

        WriteScore();
    }

    #endregion
}
