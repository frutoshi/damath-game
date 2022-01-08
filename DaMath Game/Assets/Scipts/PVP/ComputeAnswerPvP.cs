using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputeAnswerPvP : MonoBehaviour
{
    public Text answerUI;
    public Animator animCompute;
    public Animator animCorrect;
    public Animator animIncorrect;
    public Pieces pieceInstance;
    public  BoardPvP boardInstance;

    

    public GameObject computeCanvas;
    public GameObject answerCanvas;
    public GameObject correctUI;
    public GameObject incorrectUI;
    private Text rightAnswerUI;

    private bool hasNegative;
    private bool hasDecimal;

    public static int num1;
    public static int num2;
    public static string op;
    public static string strPlayer;
    public static string strRight;

    private static int playersAnswer;
    private static int rightAnswer;

    public Text whiteText;
    public Text blackText;

    public static int whitePoints;
    public static int blackPoints;

    private static string player;

    public void Button(int number)
    {
        if (answerUI.text == null || answerUI.text == "0" )
        {
            answerUI.text = number.ToString();
        }
        else
        {
            answerUI.text += number.ToString();
        }
    }
    public void Negative()
    {
        if (!hasNegative)
        {
            if (answerUI.text.StartsWith("0") && !hasDecimal)
            {
                answerUI.text = "-";
            }
            else
            {
                answerUI.text = "-" + answerUI.text;
                
            }
            hasNegative = true;

        }
        else if (hasNegative)
        {
            return;
        }
    }
    public void Decimal()
    {
        if (!hasDecimal)
        {
            if (answerUI.text == "-" && hasNegative)
            {
                answerUI.text += "0";
            }
            answerUI.text += ".";
            hasDecimal = true;
        }
        else if (hasDecimal)
        {
            return;
        }
    }
    public void Clear()
    {
        answerUI.text = "0";
        hasDecimal = false;
        hasNegative = false;
    }

    public void Submit()
    {
        playersAnswer = int.Parse(answerUI.text);
        Debug.Log(playersAnswer);
       // StartCoroutine(PlayAndDisappear("compute"));
        answerUI.text = "0";
        CheckAnswer(playersAnswer);
    }

    public static void ComputeAns(int num1, int num2, string op, string pl)
    {
        player = pl;

        if (op == "+")
        {
            rightAnswer = num1 + num2;
        }
        if (op == "-")
        {
            rightAnswer = num1 - num2;
        }
        if (op == "x")
        {
            rightAnswer = num1 * num2;
        }
        if (op == "%")
        {
            rightAnswer = num1 / num2;
                
        }


        Debug.Log(player);
        Debug.Log(rightAnswer.ToString());
    }

    public void CheckAnswer(int answer)
    {
        StartCoroutine(PlayAndDisappear("compute"));
        answerCanvas.SetActive(true);
        strRight = rightAnswer.ToString();
        strPlayer = playersAnswer.ToString();
        if (strRight == strPlayer) // pag tama sagot
        {
            rightAnswerUI = correctUI.GetComponentInChildren<Text>();
            rightAnswerUI.text = rightAnswer.ToString();
            correctUI.SetActive(true);
          
 
            //checker lang
            Debug.Log("TAMA");

            Score(player, true);

            StartCoroutine(PlayAndDisappear("correct"));
        }
        else // pag mali sagot
        {
            rightAnswerUI = incorrectUI.GetComponentInChildren<Text>();
            rightAnswerUI.text = rightAnswer.ToString();
            incorrectUI.SetActive(true);
           
            //checker lang
            Debug.Log("DUH EDI MALI");

            Score(player, false);

            StartCoroutine(PlayAndDisappear("incorrect"));
        }

    }

    public void Score(string player, bool won )
    {
        if(player == "white")
        {
            if (won)
            {
                whitePoints += rightAnswer;
                whiteText.text = whitePoints.ToString();
            }
            else
            {
                whitePoints += 0;
                whiteText.text = whitePoints.ToString();
            }
        }
        else
        {
            if (won)
            {
                blackPoints += rightAnswer;
                blackText.text = blackPoints.ToString();
            }
            else
            {
                blackPoints += 0;
                blackText.text = blackPoints.ToString();
            }
        }
    }


    // play and disappear after delay: coroutine
    IEnumerator PlayAndDisappear(string ani)
    {
        if (ani == "compute")
        {
            animCompute.SetTrigger("Closing");
            yield return new WaitForSeconds(3f);
            computeCanvas.SetActive(false); // deactivate object
        }
        if (ani == "correct")
        {
            yield return new WaitForSeconds(3f);

            animCorrect.SetTrigger("Closing");

            yield return new WaitForSeconds(3f);

            correctUI.SetActive(false); // deactivate object
            StartCoroutine(PlayAndDisappear("answer"));
        }
        if (ani == "incorrect")
        {
            yield return new WaitForSeconds(3f);

            animIncorrect.SetTrigger("Closing");

            yield return new WaitForSeconds(3f);

            incorrectUI.SetActive(false); // deactivate object
            StartCoroutine(PlayAndDisappear("answer"));
        }
        if (ani == "answer")
        {
            yield return new WaitForSeconds(3f);
            answerCanvas.SetActive(false); // deactivate object
        }

    }
}
