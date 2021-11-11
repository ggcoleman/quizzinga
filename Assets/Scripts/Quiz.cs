using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Quiz : MonoBehaviour
{

    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    QuestionSO currentQuestion;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        //GetNextQuestion();
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion()
    {
        if (questions.Any())
        {
            GetRandomQuestion();
            SetButtonState(true);
            SetDefaultButtonSprite();
            DisplayQuestion();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (var i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestion.GetAnswerIndex())
        {
            questionText.text = "Correct.";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = $"Sorry the correct answer was;\n{correctAnswer}";
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void SetButtonState(bool state)
    {
        foreach (var answerButton in answerButtons)
        {
            answerButton.GetComponent<Button>().interactable = state;
        }
    }

    private void SetDefaultButtonSprite()
    {
        foreach (var answerButton in answerButtons)
        {
            answerButton.GetComponentInChildren<Image>().sprite = defaultAnswerSprite;
        }
    }
}
