using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combination
{
    [SerializeField] int elementID1, elementID2, elementOutcome;

    public int GetOutcome() { return elementOutcome; }
    public int GetID1() { return elementID1; }
    public int GetID2() { return elementID2; }
}
