using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int phase = 1; //phase indicator
    private int score = 0;
    public GameObject whatsUpText; 
    public GameObject dapMeUpText;
    public GameObject ContinueButton;
    public GameObject dapTarget;
    public GameObject dapTargetRange;
    public GameObject Arm;
    public GameObject GoodToSeeYouText;
    public bool dapHitCheck = false;
    public GameObject dapMissedText;

    public void continueButton()
    {
        if (phase == 1)
        {
            phase1();
        }
    }

    private void phase1() //Show "Dap me up" plus button
    {
        whatsUpText.SetActive(false);
        dapMeUpText.SetActive(true);
        dapTarget.SetActive(true);
        dapTargetRange.SetActive(true);
        Arm.SetActive(true);
        ContinueButton.SetActive(false);
        phase += 1;
        StartCoroutine(DapQTE());
    }

    public void dapHit() //Dap hit show text then on to phase 2
    {
        dapTarget.SetActive(false);
        dapTargetRange.SetActive(false);
        dapMeUpText.SetActive(false);
        GoodToSeeYouText.SetActive(true);
        ContinueButton.SetActive(true);
        dapHitCheck = true;
        Arm.SetActive(false);
        //score += 300; // Commented out for testing
    }

    IEnumerator DapQTE() //dap qte show take away button plus show text after time
    {
        StartCoroutine(targetScale(dapTarget, 1.5f));
        yield return new WaitForSeconds(1.5f);
        dapTarget.SetActive(false);
        dapTargetRange.SetActive(false);
        dapMeUpText.SetActive(false);
        if (!dapHitCheck) //dap missed case
        {
            dapMissedText.SetActive(true);
            ContinueButton.SetActive(true);
            Arm.SetActive(false);
        }
    }

    IEnumerator targetScale(GameObject target, float timeLength)
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
        if (dapHitCheck == true)
        {
            float distanceRatio = Mathf.Abs((1/2) - (timeElapsed/targetTime));
            float ptsEarned = (-(Mathf.Pow(distanceRatio, 2f) * 300));
            score += (300 + (int)(ptsEarned));
            Debug.Log(score);
            ScoreManager.Instance.printScore(score);
        }
    }

}
