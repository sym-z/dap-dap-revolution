using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private int phase = 1; //phase indicator
    private int score = 0;
    public TextMeshProUGUI whatsUpText;
    public GameObject opener;
    public GameObject ContinueButton;
    public GameObject GoodToSeeYouText;
    public GameObject dapTarget;
    public GameObject DapTargetRange;
    public bool dapHitCheck = false;
    public GameObject dapMissedText;
    private GameObject[] phase1Objects;
    public GameObject[] buttons;
    public GameObject[] goodbyeQTE;
    private float difficulty = (1f);
    public GameObject aKey;
    public GameObject fKey;
    public GameObject xKey;
    public bool aKeyHit;
    public bool fKeyHit;
    public bool xKeyHit;
    public bool aKeyNext;
    public bool fKeyNext;
    public bool inGoodbyeQTE;
    public bool KeysHit;



    private void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("choiceButton");
        Activator(buttons, true);
        ContinueButton.SetActive(false);
        phase1Objects = GameObject.FindGameObjectsWithTag("Phase1");
        Activator(phase1Objects, false);
        goodbyeQTE = GameObject.FindGameObjectsWithTag("Goodbye");
        Activator(goodbyeQTE, false);
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
        Activator(buttons, false);
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
                Activator(buttons, false);
                whatsUpText.text = "Sounds great!";
                score += 150;
                phase += 1;
                ContinueButton.SetActive(true);
                break;
            default:
                Activator(buttons, false);
                whatsUpText.text = "Cringe...";
                score += 0;
                phase += 1;
                ContinueButton.SetActive(true);
                break;
        }
    }

    private void phase4()   // Begins goodbye QTE choice
    {
        whatsUpText.text = ("Remember the goodbye?");
        Activator(goodbyeQTE, true);
        inGoodbyeQTE = true;
        StartCoroutine(HugQTE(null, aKey));


        //whatsUpText.text = "Later.";
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
        foreach (GameObject obj in objList)
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

    IEnumerator HugQTE(GameObject firstButton, GameObject secondButton)
    {
        if (firstButton != null)
        {
            Image firstimage = firstButton.GetComponent<Image>();
            Color firstcolor = firstimage.color;
            firstcolor = Color.green;
            firstimage.color = firstcolor;
        }
        if (secondButton != null)
        {
            Image image = secondButton.GetComponent<Image>();
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        yield return new WaitForSeconds(4f);
        if (!KeysHit)
        {
            Activator(goodbyeQTE, false);
        }
    }


    //  Scales QTE target as time passes, granting points on time and greeting type
    IEnumerator targetScale(GameObject target, float timeLength, int dapType)
    {
        float timeElapsed = 0;
        float targetTime = timeLength;
        Vector3 endScale = target.transform.localScale;
        // Changes timeframe and target size based on difficulty of introduction
        switch (dapType)
        {
            case 1:
                difficulty = .5f;
                break;
            case 2:
                difficulty = .75f;
                DapTargetRange.transform.localScale = endScale * 1.5f;
                break;
            case 3:
                difficulty = .25f;
                DapTargetRange.transform.localScale = endScale * .5f;
                break;
            default:
                break;
        }
        while ((timeElapsed < targetTime) && dapHitCheck == false)
        {
            timeElapsed += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(Vector3.zero, endScale * 2, timeElapsed / targetTime);
            yield return null;
        }
        Debug.Log(difficulty);
        // Calculates points on hit based on time taken
        if (dapHitCheck == true)
        {
            float distanceRatio = Mathf.Abs(difficulty - (timeElapsed / targetTime));
            float ptsEarned = (-(Mathf.Pow(distanceRatio, 2f) * 300));
            score += (300 + (int)(ptsEarned));
            Debug.Log(score);
            ScoreManager.Instance.printScore(score);
        }
    }

    private void Update()
    {
        if (inGoodbyeQTE)
        {
            if (Input.GetKeyDown(KeyCode.A) && !aKeyHit)
            {
                aKeyHit = true;
                aKeyNext = true;
            }
            if (Input.GetKeyDown(KeyCode.F) && aKeyHit && !fKeyHit)
            {
                fKeyHit = true;
                fKeyNext = true;
            }
            if (Input.GetKeyDown(KeyCode.X) && fKeyHit && aKeyHit && !xKeyHit)
            {
                xKeyHit = true;
            }
        }

        if (aKeyNext)
        {
            aKeyNext = false;
            StartCoroutine(HugQTE(aKey, fKey));
        }
        if (fKeyNext)
        {
            fKeyNext = false;
            StartCoroutine(HugQTE(fKey, xKey));
        }
        if (xKeyHit)
        {
            Activator(goodbyeQTE, false);
            whatsUpText.text = "I'll see you later";
        }

    }
}
