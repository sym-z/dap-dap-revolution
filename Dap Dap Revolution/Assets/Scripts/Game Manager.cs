using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private int phase = 1; //phase indicator
    private int score = 0;
    public TextMeshProUGUI whatsUpText;
    public TextMeshProUGUI scoreUI;
    private Animator animator;
    public GameObject cutscene;
    public GameObject opener;
    public GameObject ContinueButton;
    public GameObject GoodToSeeYouText;
    public GameObject dapTarget;
    public GameObject DapTargetRange;
    public bool dapHitCheck = false;
    private bool saluteCheck = true;
    public GameObject dapMissedText;
    private GameObject[] phase1Objects;
    public GameObject[] buttons;
    public GameObject[] goodbyeQTE;
    public GameObject[] cutsceneObj;
    private string animName;
    private float difficulty = (1f);
    public GameObject aKey;
    public GameObject fKey;
    public GameObject xKey;
    public bool aKeyHit;
    public bool fKeyHit;
    public bool xKeyHit;
    public bool aKeyNext;
    public bool fKeyNext;
    public bool xKeyNext;
    public Slider timeSlider;
    public bool inGoodbyeQTE;
    public bool KeysHit;
    public Sprite salutePrefab;
    public Sprite handshakePrefab;
    public Sprite dapPrefab;
    public Sprite nod;
    public Sprite yes;
    public Sprite no;
    public AudioSource DanAudio;
    public AudioClip approve;
    public AudioClip greeting;
    public AudioClip grumble;



    private void Start()
    {
        animator = cutscene.GetComponent<Animator>();
        buttons = GameObject.FindGameObjectsWithTag("choiceButton");
        Activator(buttons, true);
        ContinueButton.SetActive(false);
        phase1Objects = GameObject.FindGameObjectsWithTag("Phase1");
        Activator(phase1Objects, false);
        goodbyeQTE = GameObject.FindGameObjectsWithTag("Goodbye");
        Activator(goodbyeQTE, false);
        cutsceneObj = GameObject.FindGameObjectsWithTag("Cutscene");
        Activator(cutsceneObj, false);
        DanAudio.PlayOneShot(greeting);
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
            case 5:
                phase5(buttonID);
                break;
            case 6:
                SceneManager.LoadScene("Score Menu");
                break;
            default:
                SceneManager.LoadScene("Fight scene");
                break;
        }
    }

    private void phase1(int buttonID) //Show "Dap me up" plus button
    {
        opener.SetActive(false);
        Activator(buttons, false);
        Activator(phase1Objects, true);
        opener.SetActive(true);
        if (buttonID == 1)
        {
            whatsUpText.text = "Dap Me Up!!";
        }
        else
        {
            whatsUpText.text = "Put 'er there!";
        }
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
        GameObject button = buttons[0];
        button.GetComponent<Image>().sprite = yes;
        button = buttons[1];
        button.GetComponent<Image>().sprite = no;
        //buttons[2].SetActive(false);
        phase += 1;
    }

    private void phase3(int buttonID) // Finish button placement and insert dialogue choices here
    {
        switch (buttonID)
        {
            case 1:
                Activator(buttons, false);
                whatsUpText.text = "Sounds great!";
                DanAudio.PlayOneShot(approve);
                score += 150;
                scoreUI.text = "" + score;
                ScoreManager.Instance.printScore(150);
                phase += 1;
                ContinueButton.SetActive(true);
                break;
            default:
                Activator(buttons, false);
                whatsUpText.text = "Cringe...";
                DanAudio.PlayOneShot(grumble);
                score += 0;
                ScoreManager.Instance.printScore(0);
                phase += 1;
                ContinueButton.SetActive(true);
                break;
        }
    }

    private void phase4()   // Begins goodbye QTE choice
    {
        whatsUpText.text = ("Remember the goodbye?");
        Activator(buttons, true);
        GameObject button = buttons[0];
        button.GetComponent<Image>().sprite = salutePrefab;
        button = buttons[1];
        button.GetComponent<Image>().sprite = nod;
        phase += 1;
        //whatsUpText.text = "Later.";
        // Insert Sonic Unleashed style QTE
    }
    
    private void phase5(int buttonID)
    {
        Activator(buttons, false);
        Activator(goodbyeQTE, true);
        inGoodbyeQTE = true;
        StartCoroutine(HugQTE(null, aKey, false, buttonID));
    }

    public void dapHit() //Dap hit show text then on to phase 2
    {
        Activator(phase1Objects, false);
        opener.SetActive(false);
        GoodToSeeYouText.SetActive(true);
        ContinueButton.SetActive(true);
        dapHitCheck = true;
        DanAudio.PlayOneShot(approve);
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
        opener.SetActive(false);
        if (!dapHitCheck) //dap missed case
        {
            dapMissedText.SetActive(true);
            ContinueButton.SetActive(true);
            DanAudio.PlayOneShot(grumble);
        }
    }

    IEnumerator HugQTE(GameObject firstButton, GameObject secondButton, bool started = true, int buttonID = 0)
    {
        difficulty = 4f;
        if (started == false)
        {
            started = true;
            if (buttonID == 2)
            {
                difficulty = 2f;
                saluteCheck = false;
            }
            StartCoroutine(sliderScale());

        }
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
        yield return new WaitForSeconds(difficulty);
        if (!KeysHit)
        {
            Activator(goodbyeQTE, false);
            whatsUpText.text = "Really? You don't remember?";
            DanAudio.PlayOneShot(grumble);
            phase++;
            ContinueButton.SetActive(true);
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
                dapTarget.GetComponent<Image>().sprite = dapPrefab;
                break;
            case 2:
                difficulty = .75f;
                dapTarget.GetComponent<Image>().sprite = handshakePrefab;
                DapTargetRange.transform.localScale *= 1.5f;
                break;
            case 3:
                difficulty = .25f;
                dapTarget.GetComponent<Image>().sprite = dapPrefab;
                DapTargetRange.transform.localScale *= .5f;
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
            if (dapType == 1)
            {
                StartCoroutine(animPlayer(0)); //DapAnimation
            }
            else
            {
                StartCoroutine(animPlayer(1));
            }
            float distanceRatio = Mathf.Abs(difficulty - (timeElapsed / targetTime));
            //float ptsEarned = (-(Mathf.Pow(distanceRatio, 2f) * 300));
            float ptsEarned = -distanceRatio * 300;
            score += (300 + (int)(ptsEarned));
            scoreUI.text = "" + score;
            Debug.Log(score);
            ScoreManager.Instance.printScore(score);
        }
    }

    IEnumerator sliderScale()
    {
        float timeElapsed = 0;
        float targetTime = difficulty;
        timeSlider.value = 0;
        timeSlider.maxValue = targetTime;
        // Changes timeframe and target size based on difficulty of introduction
        while ((timeElapsed < targetTime) && KeysHit == false)
        {
            timeElapsed += Time.deltaTime;
            timeSlider.value += Time.deltaTime;
            yield return null;
        }
        Debug.Log(difficulty);
        // Calculates points on hit based on time taken
        if (KeysHit == true)
        {
            float ptsEarned = ((targetTime - timeElapsed)*(300/difficulty));
            score += (int)(ptsEarned);
            scoreUI.text = "" + score;
            Debug.Log(score);
            ScoreManager.Instance.printScore((int)(ptsEarned));
        }
    }

    IEnumerator animPlayer(int clipNum)
    {
        Activator(cutsceneObj, true);
        if (clipNum == 2)
        {
            cutscene.transform.rotation = Quaternion.identity;
        }
        animator.SetInteger("PhaseNum", clipNum);
        yield return new WaitForSeconds(1.1f);
        Activator(cutsceneObj, false);
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
                KeysHit = true;
                xKeyNext = true;
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
            inGoodbyeQTE = false;
        }
        if (xKeyNext)
        {
            xKeyNext = false;
            if (saluteCheck)
            {
                StartCoroutine(animPlayer(2));
            }
            whatsUpText.text = "Nice. I'll see you later.";
            DanAudio.PlayOneShot(approve);
            phase++;
            ContinueButton.SetActive(true);
        }

    }
}
