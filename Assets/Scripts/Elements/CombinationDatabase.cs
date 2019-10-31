using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombinationDatabase : MonoBehaviour
{
    [SerializeField] List<Combination> combinations = new List<Combination>();

    public int[] GetCombinations(int id1, int id2)
    {
        List<int> combos = new List<int>();
        int ctr = 0;

        foreach(Combination c in combinations)
        {
            if ((c.GetID1() == id1 && c.GetID2() == id2) ||
                (c.GetID2() == id1 && c.GetID1() == id2))
            {
                combos.Add(c.GetOutcome());
                ctr++;
            }
        }

        return combos.ToArray();
    }
}
