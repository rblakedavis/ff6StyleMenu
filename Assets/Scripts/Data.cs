using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Data", order = 1)]
public class Data : ScriptableObject
{
    public int writeArea;
    public int listLength;
    public int maxIndex;
    public bool isMenuSortable;

    public List<string> topLeftCorner;
    public List<string> itemsMidPanel;
    public List<string> inventory;
}
