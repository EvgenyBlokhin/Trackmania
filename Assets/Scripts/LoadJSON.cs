using UnityEngine;
using System.IO;

public class LoadJSON : MonoBehaviour {

    static public T LoadingJSON<T>() {
        if (File.Exists(StartRacing.path)) {
            return JsonUtility.FromJson<T>(File.ReadAllText(StartRacing.path));
        } else return default(T);
    }
    
}
