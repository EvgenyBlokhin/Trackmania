using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Racing : MonoBehaviour
{
    public static float longTrack = 3000;
    public static bool isStart = false;
    public GameObject restartButton;
    public GameObject startButton;
    public GameObject winnersPanel;
    public GameObject InfoPanel;
    public static ArrayList winners = new ArrayList();
    public Text firstPosition;
    public Text secondPosition;
    public Text thirdPosition;

    void Awake() {
    winners = new ArrayList();
    }

    void Update() {
        if (!isStart) {
            startButton.SetActive(true);
            InfoPanel.SetActive(true);
        } else {
            startButton.SetActive(false);
            InfoPanel.SetActive(false);
        }         
        if (winners.Count == 3) {
            restartButton.SetActive(true);
            AssignWinners();
            winners.Add("finish");
        }
    }
    private void AssignWinners() {
        winnersPanel.SetActive(true);
        foreach (string winner in winners) {
            switch (winners.IndexOf(winner)) {
                case 0:
                    firstPosition.text = "1st. " + winner.ToString();
                    break;
                case 1:
                    secondPosition.text = "2st. " + winner.ToString();
                    break;
                case 2:
                    thirdPosition.text = "3st. " + winner.ToString();
                    break;
            }
        }
    }
}
