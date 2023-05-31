using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    PlayerController pc = new PlayerController();

    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameWinUI;
    [SerializeField] GameObject gameLoseUI;
    [SerializeField] GameObject controlsUI;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI shotText;
    //[SerializeField] AudioSource gameMusic;
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI totalTimeWinText;
    [SerializeField] TextMeshProUGUI totalTimeLoseText;

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
    public float timer = 0;
    float initialTime = 0;

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
                titleUI.SetActive(true);
                //gameMusic.Stop();
                break;
            case State.START_GAME:
                titleUI.SetActive(false);
                //gameMusic.Play();
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                timer += Time.deltaTime;
                timerText.SetText("Time: " + timer.ToString("0.00"));
                shotText.SetText("Tanks Left: " + pc.TankShots.ToString());
                break;
            case State.GAME_WIN:
                timer = 0;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameWinUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
            case State.GAME_LOSE:
                timer = 0;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameLoseUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
            default:
                break;
        }
    }


    public void SetWin()
    {
        gameWinUI.SetActive(true);
        //gameMusic.Stop();
        state = State.GAME_WIN;
        stateTimer = 3;

        // Show total time in the win UI
        totalTimeWinText.SetText("Total Time: " + timer.ToString("0.00"));
    }

    public void SetLose()
    {
        gameLoseUI.SetActive(true);
        //gameMusic.Stop();
        state = State.GAME_LOSE;
        stateTimer = 5;

        // Show total time in the Lose UI
        totalTimeLoseText.SetText("Total Time: " + timer.ToString("0.00"));
    }

    public void StartGame()
    {
        state = State.START_GAME;
    }
}
