using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public  int tempScore = 0;
    public  int bankedScore = 0;

    private  float loseScoreTimer = 0;
    const int LOSE_SCORE_TIME = 8;

    [SerializeField] TextMeshProUGUI tempScoreText;
    [SerializeField] TextMeshProUGUI bankedScoreText;

    private void Update()
    {
        loseScoreTimer += Time.deltaTime;
        if(loseScoreTimer >= LOSE_SCORE_TIME)
        {
            ResetTempScore();
        }
        //print("Time to lose score: " + (LOSE_SCORE_TIME - loseScoreTimer).ToString());

        tempScoreText.text = ("+ " + tempScore);
        bankedScoreText.text = ("SCORE: " + bankedScore);
    }

    public void IncreaseTempScore(int score)
    {
        tempScore += score;
        loseScoreTimer = 0;
        // Play text animation where it gets bigger for an instant then returns to normal size, and maybe for certain number thresholds, the normal size will be increased
        //print("Scoring: Increase Temp Score...");
    }

    public void ResetTempScore()
    {
        // Play text animation where score rapidly decreases to 0
        tempScore = 0;
        //print("Scoring: Reset Temp Score...");
    }

    public void BankScore()
    {
        bankedScore += tempScore;
        tempScore = 0;
        loseScoreTimer = 0;
        //print("Scoring: Banked Score...");
    }
}
