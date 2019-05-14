using System;

[Serializable]
public class Car : Transport {
    public int peopleCount = 1;

    public Car(string nameTransport, float speed, int punctureWheelChance, int peopleCount) : base(nameTransport, speed, punctureWheelChance) {
        this.peopleCount = peopleCount;
    }
}
