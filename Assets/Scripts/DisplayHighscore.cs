using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighscore : MonoBehaviour
{
    public TMP_Text highScoreField;
    [SerializeField]
    MongoControl mongoControl;
    public GameObject singleHighScore;
    public GameObject panel;
    
    void Start()
    {
        mongoControl = GameObject.Find("MongoControl").GetComponent<MongoControl>();
        // DisplayHighScores();
        Invoke("DisplayHighScores", 2);
    }

    
    void Update()
    {
        
    }

    private async void DisplayHighScores()
    {
        var task = mongoControl.GetScoresFromDatabase();
        var result = await task;
        string output = "";
        
        foreach(var score in result)
        {
            output += score.username + " Score:" + score.score + "\n";
            // Parempi keino
            GameObject singleScore = Instantiate(singleHighScore);
            singleScore.transform.SetParent(panel.transform);
            singleScore.GetComponent<TMP_Text>().text = score.username + " Score: " + score.score;
        }

        highScoreField.text = output;
    }

}
