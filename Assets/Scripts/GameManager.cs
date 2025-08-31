using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // <-- use TMP

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;       // TMP instead of Legacy Text
    public TMP_Text bestText;        // TMP instead of Legacy Text
    public GameObject gameOverPanel; // still a GameObject
    public TMP_Text finalScoreText;  // TMP instead of Legacy Text

    [Header("Refs")]
    public ObstacleSpawner spawner;
    public PlayerController player;

    float score;
    bool over;
    const string BestKey = "TD_BEST";

    void Start()
    {
        score = 0f;
        over = false;
        Time.timeScale = 1f;

        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (bestText) bestText.text = "Best: " + GetBest().ToString();
    }

    void Update()
    {
        if (over) return;

        score += Time.deltaTime;
        if (scoreText) scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public void GameOver()
    {
        if (over) return;
        over = true;
        Time.timeScale = 0f;

        int final = Mathf.FloorToInt(score);
        int best = GetBest();
        if (final > best) { best = final; SetBest(best); }

        if (finalScoreText) finalScoreText.text = $"Score: {final}\nBest: {best}";
        if (bestText) bestText.text = "Best: " + best;
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    public void Restart() => SceneManager.LoadScene("Game");
    public void ToMenu()  => SceneManager.LoadScene("MainMenu");

    int GetBest() => PlayerPrefs.GetInt(BestKey, 0);
    void SetBest(int v) { PlayerPrefs.SetInt(BestKey, v); PlayerPrefs.Save(); }
}
