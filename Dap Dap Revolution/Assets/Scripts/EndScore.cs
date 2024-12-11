using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScore : MonoBehaviour
{
    private int[] scores;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI endText;
    public GameObject again;
    private int endScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        scores = ScoreManager.Instance.getScores();
        StartCoroutine(ScorePrint());
    }

    IEnumerator ScorePrint()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            scoreTexts[i].text += scores[i];
            endScore += scores[i];
        }
        yield return new WaitForSeconds(1.5f);
        endText.text += endScore;
        yield return new WaitForSeconds(1f);
        again.SetActive(true);
    }
}
