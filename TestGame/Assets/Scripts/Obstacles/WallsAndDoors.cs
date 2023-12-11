using System;
using UnityEngine;

public class WallsAndDoors : MonoBehaviour
{
    [System.Serializable]
    public struct Doors
    {
        public Rigidbody door1;
        public Rigidbody door2;
    }
    public Doors[] doors;

    void Start()
    {
        int i = 0;
        i = UnityEngine.Random.Range(0, doors.Length);
        for (int j = 0; j < doors.Length; j++) 
        {
            if(i == j)
            {
                SetKinematic(j, false);
                Debug.Log($"Door {j} is breakable");
            }
            else
            {
                SetKinematic(j, true);
            }
        }
    }

    private void SetKinematic(int index,bool isKinematic)
    {
        doors[index].door1.isKinematic = isKinematic;
        doors[index].door2.isKinematic = isKinematic;
    }
}
