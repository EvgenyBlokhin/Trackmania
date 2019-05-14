using System;

[Serializable]
public class Motorcycle : Transport {
    public bool motorcycleStoller = true;

    public Motorcycle(string nameTransport, float speed, int punctureWheelChance, bool motorcycleStoller) : base(nameTransport, speed, punctureWheelChance) {
        this.motorcycleStoller = motorcycleStoller;
    } 
}
