using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    public static List<Decoy> decoys = new List<Decoy>();
    void OnEnable()
    {
        decoys.Add(this);
    }
    void OnDisable()
    {
        decoys.Remove(this);
    }
}