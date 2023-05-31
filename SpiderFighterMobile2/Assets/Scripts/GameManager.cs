using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController pc;
    
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameWinUI;
    [SerializeField] GameObject gameLoseUI;
    [SerializeField] GameObject controlsUI;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI shotText;
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI totalTimeWinText;
    [SerializeField] TextMeshProUGUI totalTimeLoseText;
    [SerializeField] TextMeshProUGUI totalScoreWinText;
    [SerializeField] TextMeshProUGUI totalScoreLoseText;
    [SerializeField] GameObject towerMainPrefab;

    public enum State
    {
        TITLE,
        START_GAME,
        PLAY_GAME,
        GAME_WIN,
        GAME_LOSE
    }

    public State state = State.TITLE;
    float stateTimer = 0;
    float timer = 0;
    float initialTime = 0;
    float score = 0;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        timer = 0;
        timer += initialTime;
    }

    public void Update()
    {
        switch (state)
        {
            case State.TITLE:
                pc.ResetGameOver();
                titleUI.SetActive(true);
                break;
            case State.START_GAME:
                titleUI.SetActive(false);
                pc.TankShots = 5;
                score = 0;
                timer = 0;
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                timer += Time.deltaTime;
                timerText.SetText("Time: " + timer.ToString("0.00"));
                scoreUI.SetText("Score: " + score.ToString("0"));
                shotText.SetText("Tanks Left: " + pc.TankShots.ToString());
                if (pc.GetGameOver() == true && score >= 3000)
                {
                    SetWin();
                }
                else if (pc.GetGameOver() == true && score < 3000)
                {
                    SetLose();
                }
                break;
            case State.GAME_WIN:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameWinUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
            case State.GAME_LOSE:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameLoseUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
        }
    }

    public void SetWin()
    {
        gameWinUI.SetActive(true);
        state = State.GAME_WIN;
        stateTimer = 3;

        // Show total time and score in the win UI
        totalTimeWinText.SetText("Total Time: " + timer.ToString("0.00"));
        totalScoreWinText.SetText("Final Score: " + score.ToString());
    }

    public void SetLose()
    {
        gameLoseUI.SetActive(true);
        state = State.GAME_LOSE;
        stateTimer = 5;

        // Show total time and score in the lose UI
        totalTimeLoseText.SetText("Total Time: " + timer.ToString("0.00"));
        totalScoreLoseText.SetText("Final Score: " + score.ToString());
    }

    public void StartGame()
    {

        Tower tower = FindObjectOfType<Tower>();
        if (tower != null)
        {
            Destroy(tower.gameObject);
        }
        GameObject towerSpawn = GameObject.Find("TowerSpawn");

        if (towerMainPrefab != null)
        {
            Instantiate(towerMainPrefab, towerSpawn.transform.position, towerSpawn.transform.rotation);
        }

        state = State.START_GAME;
    }

    public void SetScore(float points)
    {
        score += points;
        scoreUI.text = "Score: " + score.ToString();
    }
}
