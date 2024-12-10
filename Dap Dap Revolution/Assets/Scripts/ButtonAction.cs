using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Button choiceButton;
    [SerializeField] private GameObject GameManagement;
    [SerializeField] private int buttonID;
    // Start is called before the first frame update
    void Start()
    {
        choiceButton.onClick.AddListener(taskOnClick);
    }


    void taskOnClick()
    {
        if (buttonID == 9)
        {
            SceneManager.LoadScene("Fight scene");
        }
        GameManagement.GetComponent<GameManager>().continueButton(buttonID);
        gameObject.SetActive(false);
    }
}
