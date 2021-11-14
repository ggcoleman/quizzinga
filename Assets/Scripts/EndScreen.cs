using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreKeeper scoreKeeper;
    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }


    public void ShowFinalScoreText()
    {
        finalScoreText.text = "Congratulations:\nYou Scored " + scoreKeeper.CalculateScore().ToString() + "%";
    }

}
