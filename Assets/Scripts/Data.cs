using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Data", order = 1)]
public class Data : ScriptableObject
{
    public int writeArea;
    public int listLength;
    public int maxIndex;
    public bool isItemSelected  = false;
}
