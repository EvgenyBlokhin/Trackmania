using UnityEngine;
using UnityEngine.UI;

public class StartTruck : MonoBehaviour
{
    public float speed = 2f;
    int passedInMeters;
    public Transform[] moveSpots;
    int nextSpots = 0;
    float totalDistance;
    float totalDistanceInMeters;
    float passedBetweenPoint;
    float passedAll;
    int punctureWheelChance = 20;
    bool punctureWheel;
    Transform punctureLocation;
    public static bool isFinish;
    float waitTime;
    float startWaitTime = 5;
    public Text passedInMetersText;
    readonly string truck = "Truck";
    public GameObject punctureNotificationPanel;
    public Text speedText;
    public Text punctureText;
    public Text weightInCargoText;
    int weightInCargo = 200;
    float coefficientDistance = 100;

    void Start() {
        CalculateTotalDistance();
        CalculatePunctureWheelChance(punctureWheelChance);
        speedText.text = (speed * coefficientDistance).ToString();
        punctureText.text = punctureWheelChance.ToString();
        weightInCargoText.text = weightInCargo.ToString();
        if (punctureWheel) {
            InitializePuncture();
        }
        waitTime = startWaitTime;
    }

    void Awake() {
        isFinish = false;
    }

    void Update() {
        if (Racing.isStart) {
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
                    } else {
                        MoveToNextSpots(nextSpots);
                    }
                } else {
                    MoveToNextSpots(nextSpots);
                }
                if (Vector2.Distance(transform.position, moveSpots[nextSpots].position) < 0.01f) {
                    if (nextSpots < moveSpots.Length - 1) {
                        transform.rotation = moveSpots[nextSpots].rotation;
                        nextSpots++;
                        passedAll += passedBetweenPoint;
                    } else {
                        isFinish = true;
                        Debug.Log(isFinish);
                        Racing.winners.Add(truck);
                    }
                }
                if (nextSpots > 0) {
                    passedBetweenPoint = Vector2.Distance(transform.position, moveSpots[nextSpots - 1].position);
                    CalculateDistanceInMeters();
                }
            }
        }
        passedInMetersText.text = "Truck: " + passedInMeters.ToString() + "m.";
    }

    void MoveToNextSpots(int nextSpots) {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[nextSpots].position, speed * Time.deltaTime);
    }

    void CalculatePunctureWheelChance(int punctureWheelChance) {
        int random = Random.Range(1, 100);
        if (punctureWheelChance < random) {
            punctureWheel = false;
        } else {
            punctureWheel = true;
        }
    }

    void CalculateTotalDistance() {
        for (int i = 0; i < moveSpots.Length - 1; i++) {
            totalDistance += Vector2.Distance(moveSpots[i].position, moveSpots[i + 1].position);
        }
        totalDistanceInMeters = totalDistance * coefficientDistance;
    }

    void InitializePuncture() {
        int numberPointPuncture = Random.Range(0, moveSpots.Length - 1);
        punctureLocation = moveSpots[numberPointPuncture];
    }

    void CalculateDistanceInMeters() {
        float passedInPercent = (passedAll + passedBetweenPoint) / totalDistance;
        passedInMeters = (int)(totalDistanceInMeters * passedInPercent);
    }
}
