using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "menus", menuName = "ScriptableObjects/Menu", order = 1)]
public class Menu : ScriptableObject
{
    public void OpenItemsMenu()
    {
        Debug.Log("items Menu opened!");
    }

    public void OpenSkillsMenu()
    {
        Debug.Log("skills menu opened!");
    }

    public void OpenEquipMenu()
    {
        Debug.Log("equip menu opened!");
    }

    public void OpenRelicMenu()
    {
        Debug.Log("relic menu opened!");
    }

    public void OpenStatusMenu()
    {
        Debug.Log("status menu opened!");
    }

    public void OpenTrackMenu()
    {
        Debug.Log("Track menu opened!");
    }

    public void OpenConfigMenu()
    {
        Debug.Log("Config menu opened!");
    }

    public void OpenSaveMenu()
    {
        Debug.Log("save menu opened!");
    }
}  