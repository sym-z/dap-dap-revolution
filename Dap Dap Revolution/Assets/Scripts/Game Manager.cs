using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private int phase = 1; //phase indicator
    private int score = 0;
    public TextMeshProUGUI whatsUpText;
    public GameObject opener;
    public GameObject ContinueButton;
    public GameObject GoodToSeeYouText;
    public GameObject dapTarget;
    public bool dapHitCheck = false;
    public GameObject dapMissedText;
    private GameObject[] phase1Objects;
    public GameObject[] buttons;
    private float difficulty = (1f);

    private void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("choiceButton");
        Activator(buttons, false);
        phase1Objects = GameObject.FindGameObjectsWithTag("Phase1");
        Activator(phase1Objects, false);
    }

    public void continueButton(int buttonID)
    {
        switch (phase)
        {
            case 1:
                phase1(buttonID);
                break;
            case 2:
                phase2(); 
                break;
             case 3:
                phase3(buttonID);
                break;
            case 4:
                phase4();
                break;
            default:
                break;
        }
    }

    private void phase1(int buttonID) //Show "Dap me up" plus button
    {
        opener.SetActive(false);
        ContinueButton.SetActive(false);
        Activator(phase1Objects, true);
        phase += 1;
        StartCoroutine(DapQTE(buttonID));
    }

    private void phase2()   // Second Phase, asks player question
    {
        GoodToSeeYouText.SetActive(false);
        dapMissedText.SetActive(false);
        opener.SetActive(true);
        whatsUpText.text = "You still on for Friday?";
        Activator(buttons, true);
        phase += 1;
    }

    private void phase3(int buttonID) // Finish button placement and insert dialogue choices here
    {
        switch (buttonID)
        {
            case 1:
                whatsUpText.text = "Sounds great!";
                score += 150;
                ContinueButton.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void phase4()   // Begins goodbye QTE choice
    {
        // Insert Sonic Unleashed style QTE
    }

    public void dapHit() //Dap hit show text then on to phase 2
    {
        Activator(phase1Objects, false);
        GoodToSeeYouText.SetActive(true);
        ContinueButton.SetActive(true);
        dapHitCheck = true;
        //score += 300; // Commented out for testing
    }

    private void Activator(GameObject[] objList, bool positive)
    {
        foreach(GameObject obj in objList)
        {
            obj.SetActive(positive);
        }
    }

    IEnumerator DapQTE(int buttonID) //dap qte, removes button + shows text if failed
    {
        StartCoroutine(targetScale(dapTarget, 1.5f, buttonID));
        yield return new WaitForSeconds(1.5f);
        Activator(phase1Objects, false);
        if (!dapHitCheck) //dap missed case
        {
            dapMissedText.SetActive(true);
            ContinueButton.SetActive(true);
        }
    }

    //  Scales QTE target as time passes, granting points on time and greeting type
    IEnumerator targetScale(GameObject target, float timeLength, int dapType)
    {
        float timeElapsed = 0;
        float targetTime = timeLength;
        Vector3 endScale = target.transform.localScale;
        while ((timeElapsed < targetTime) && dapHitCheck == false)
        {
            timeElapsed += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(Vector3.zero, endScale*2, timeElapsed/targetTime);
            yield return null;
        }
        // Changes timeframe based on difficulty of introduction
        switch (dapType)
        {
            case 1:
                difficulty = .5f; 
                break;
        }
        // Calculates points on hit based on time taken
        if (dapHitCheck == true)
        {
            float distanceRatio = Mathf.Abs(difficulty - (timeElapsed/targetTime));
            float ptsEarned = (-(Mathf.Pow(distanceRatio, 2f) * 300));
            score += (300 + (int)(ptsEarned));
            Debug.Log(score);
            ScoreManager.Instance.printScore(score);
        }
    }

}
