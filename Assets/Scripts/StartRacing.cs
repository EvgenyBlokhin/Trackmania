using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class StartRacing : MonoBehaviour {
    public RacingTrack racingTrack;
    public Transform[] moveSpots;
    Transport transport;
    public Transport[] transports = new Transport[3];
    Transform punctureLocation;
    int passedInMeters;
    int nextSpots = 0;
    float totalDistance;
    float totalDistanceInMeters;
    float passedBetweenPoint;
    float passedAll;
    public bool isFinish;
    bool punctureWheel;
    float waitTime;
    float startWaitTime = 5;
    float coefficientDistance = 100;
    public static bool isStart = false;
    public GameObject restartButton;
    public GameObject startButton;
    public GameObject winnersPanel;
    public GameObject InfoPanel;
    public GameObject punctureNotificationPanel;
    public static ArrayList winners = new ArrayList();
    public Text passedInMetersText;
    public Text speedText;
    public Text punctureChanceText;
    public Text motorcycleStollerText;
    public Text weightInCargoText;
    public Text peopleCountText;
    public Text firstPosition;
    public Text secondPosition;
    public Text thirdPosition;
    public static string path;

    void Start() {
        path = Path.Combine(Application.dataPath, "Save.json");
        transports = JsonHelper.FromJson<Transport>(File.ReadAllText(path));        
        Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport + "ALLArrayStart");

        switch (name) {
                case "Car":
                    transport = transports[0];
                    if (transport.nameTransport == "" || transport == null) {
                    transport = new Car("Car", 2f, 10, 1);
                    transports[0] = transport;
                    Debug.Log(transports[0].nameTransport + "Array");
                    Debug.Log(transport.nameTransport + "NULL");
                    } else Debug.Log(transport.nameTransport + "DONT NULL");                  
                    //peopleCountText.text = ((Car)transport).peopleCount.ToString();
                    Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport + "ALLArrayCar");
                    break;
                case "Truck":
                    transport = transports[1];
                    if (transport.nameTransport == "" || transport == null) {
                    transport = new Truck("Truck", 2f, 10, 200);
                    transports[1] = transport;
                    Debug.Log(transports[1].nameTransport + "Array");
                    Debug.Log(transport.nameTransport + "NULL");
                    } else Debug.Log(transport.nameTransport + "DONT NULL");
                    //weightInCargoText.text = ((Truck)transport).weightInCargo.ToString();
                    Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport + "ALLArrayTruck");
                    break;
                case "Motorcycle":
                    transport = transports[2];
                    if (transport.nameTransport == "" || transport == null) {
                    transport = new Motorcycle("Motorcycle", 2f, 10, true);
                    transports[2] = transport;
                    Debug.Log(transports[2].nameTransport + "Array");
                    Debug.Log(transport.nameTransport + "NULL");
                    } else Debug.Log(transport.nameTransport + "DONT NULL");              
                    if (((Motorcycle)transport).motorcycleStoller) {
                        motorcycleStollerText.text = "Yes";
                    } else {
                        motorcycleStollerText.text = "No";
                    }
                    Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport + "ALLArrayMotorcycle");
                    break;
            }
        Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport + "ALLArrayEND");
        CalculateTotalDistance();
        CalculatePunctureWheelChance(transport.punctureWheelChance);
        speedText.text = (transport.speed * coefficientDistance).ToString();
        punctureChanceText.text = transport.punctureWheelChance.ToString();
        if (punctureWheel) {
            InitializePuncture();
        }
        waitTime = startWaitTime;
    }

    void Awake() {
        winners.Clear();
        //transports = new Transport[3];
    }

    void Update() {
        if (isStart) {
            startButton.SetActive(false);
            InfoPanel.SetActive(false);
            if (!isFinish) {
                if (punctureWheel) {
                    if (Vector2.Distance(transform.position, punctureLocation.position) < 0.01f) {
                        if (waitTime <= 0) {
                            MoveToNextSpots(nextSpots);
                            waitTime = startWaitTime;
                            punctureNotificationPanel.SetActive(false);
                        } else {
                            waitTime -= Time.deltaTime;
                            punctureNotificationPanel.SetActive(true);
                        }
                    } else MoveToNextSpots(nextSpots);
                } else MoveToNextSpots(nextSpots);
                if (Vector2.Distance(transform.position, moveSpots[nextSpots].position) < 0.01f) {
                    if (nextSpots < moveSpots.Length - 1) {
                        transform.rotation = moveSpots[nextSpots].rotation;
                        nextSpots++;
                        passedAll += passedBetweenPoint;
                    } else {
                        isFinish = true;
                        winners.Add(transport.nameTransport);
                        Debug.Log(winners.Count);
                        if (winners.Count == 3) {
                            restartButton.SetActive(true);
                            AssignWinners();
                            winners.Add("finish");
                        }
                    }
                }
                if (nextSpots > 0) {
                    passedBetweenPoint = Vector2.Distance(transform.position, moveSpots[nextSpots - 1].position);
                    CalculateDistanceInMeters();
                }
                passedInMetersText.text = transport.nameTransport + ": " + passedInMeters.ToString() + "m.";
            }
        } else {
            startButton.SetActive(true);
            InfoPanel.SetActive(true);
        }
    }

    private void MoveToNextSpots(int nextSpots) {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[nextSpots].position, transport.speed * Time.deltaTime);
    }

    private void CalculatePunctureWheelChance(int punctureWheelChance) {
        int random = UnityEngine.Random.Range(1, 100);
        if (punctureWheelChance < random) {
            punctureWheel = false;
        } else punctureWheel = true;
    }

    private void CalculateTotalDistance() {
        for (int i = 0; i < moveSpots.Length - 1; i++) {
            totalDistance += Vector2.Distance(moveSpots[i].position, moveSpots[i + 1].position);
        }
        totalDistanceInMeters = totalDistance * coefficientDistance;
    }

    private void InitializePuncture() {
        int numberPointPuncture = UnityEngine.Random.Range(0, moveSpots.Length - 1);
        punctureLocation = moveSpots[numberPointPuncture];
    }

    private void CalculateDistanceInMeters() {
        float passedInPercent = (passedAll + passedBetweenPoint) / totalDistance;
        passedInMeters = (int)(totalDistanceInMeters * passedInPercent);
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
    #if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause) {
    if (pause) SaveJSON.SaveFileJSON(transport);
    }
#endif
    private void OnApplicationQuit() {
        SaveJSON.SaveFileJSON(JsonHelper.ToJson(transports, false));
        //JsonHelper.JsonOverwrite(File.ReadAllText(path), transports);
        Debug.Log(transports[0].nameTransport + transports[1].nameTransport + transports[2].nameTransport);
    }
}
