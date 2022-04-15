using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questionBox : MonoBehaviour
{
    public static questionBox Instance {get; private set;}
    [SerializeField] TMPro.TextMeshProUGUI question;
    [SerializeField] Button yesButton, noButton;
    Action Action1,Action0;
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void showQuestion(string questionText, Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);
        question.text = questionText;
        Action1 = yesAction;
        Action0 = noAction;
    }

    public void yesButtonClick()
    {
        Action1();
        gameObject.SetActive(false);
    }
    public void noButtonClick()
    {
        Action0();
        gameObject.SetActive(false);
    }
}
