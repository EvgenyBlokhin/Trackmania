using UnityEngine;
using System.IO;

public class SaveJSON : MonoBehaviour {
        public static void SaveFileJSON(string transportToJason) { 
        File.WriteAllText(StartRacing.path, transportToJason);
    }
}
