using System;

[Serializable]
public class Truck : Transport {
    public int weightInCargo = 200;

    public Truck(string nameTransport, float speed, int punctureWheelChance, int weightInCargo) : base(nameTransport, speed, punctureWheelChance) {
        this.weightInCargo = weightInCargo;
    }

}
