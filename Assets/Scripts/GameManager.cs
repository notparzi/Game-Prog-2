using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score;
    private TMP_Text scoreText;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        scoreText = FindFirstObjectByType<TMP_Text>();
        UpdateScoreText();
    }


    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        Debug.Log("Score: " + score);
    }


    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}