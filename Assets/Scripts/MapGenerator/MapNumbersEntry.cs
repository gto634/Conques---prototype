using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNumbersEntry
{
    public int value;
    public int amount;

    public static Dictionary<int, int> odds = new Dictionary<int, int> 
    {
        { 2,1 },
        { 3,2 },
        { 4,3 },
        { 5,4 },
        { 6,5 },
        { 7,6 },
        { 8,5 },
        { 9,4 },
        { 10,3 },
        { 11,2 },
        { 12,1 }
    };
    public int GetOdd()
    {
        if (odds.TryGetValue(value, out int result))
        {
            return result;
        }
        return 0;
    }
}
