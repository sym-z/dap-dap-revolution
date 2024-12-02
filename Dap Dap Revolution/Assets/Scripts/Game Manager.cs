using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
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
        Arm.SetActive(true);
        ContinueButton.SetActive(false);
        phase += 1;
        StartCoroutine(DapQTE());
    }

    public void dapHit() //Dap hit show text then on to phase 2
    {
        dapTarget.SetActive(false);
        dapMeUpText.SetActive(false);
        GoodToSeeYouText.SetActive(true);
        ContinueButton.SetActive(true);
        dapHitCheck = true;
        Arm.SetActive(false);
        score += 300;
    }

    IEnumerator DapQTE() //dap qte show take away button plus show text after time
    {
        yield return new WaitForSeconds(1.5f);
        dapTarget.SetActive(false);
        dapMeUpText.SetActive(false);
        if (!dapHitCheck) //dap missed case
        {
            dapMissedText.SetActive(true);
            ContinueButton.SetActive(true);
            Arm.SetActive(false);
        }
    }

}
