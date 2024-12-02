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
    }

    [SerializeField] TextMeshProUGUI scoreText;

    public void printScore(int score)
    {
        scoreText.text = "+" + score.ToString() + " pts";
        StartCoroutine(scoreFade(1f));
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
