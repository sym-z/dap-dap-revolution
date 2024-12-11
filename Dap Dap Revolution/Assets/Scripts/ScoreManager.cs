using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0f);
    }

    [SerializeField] TextMeshProUGUI scoreText;
    private int[] scores = new int[4];
    private int lvl = 0;

    public int[] getScores()
    {
        return scores;
    }

    public void printScore(int score)
    {
        scores[lvl] = score;
        lvl++;
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        scoreText.text = "+" + score.ToString() + " pts";
        StartCoroutine(scoreFade(1f));
    }

    public void resetScore()
    {
        lvl = 0;
    }

    IEnumerator scoreFade(float timeLength)
    {
        float timeElapsed = 0;
        float targetTime = 1.5f;
        while (timeElapsed < targetTime)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / targetTime);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            yield return null;
        }
    }
}
