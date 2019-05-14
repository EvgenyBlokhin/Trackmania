using System;

[Serializable]
public class Transport {
    public string nameTransport;
    public float speed;
    public int punctureWheelChance;

    public Transport(string nameTransport, float speed, int punctureWheelChance) {
        this.nameTransport = nameTransport;
        this.speed = speed;
        this.punctureWheelChance = punctureWheelChance;
    }
}
