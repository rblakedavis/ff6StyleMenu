using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "menus", menuName = "ScriptableObjects/Menu", order = 1)]
public class Menu : ScriptableObject
{
    public GameObject menuContainer;
    public int menuType;
    public GameObject mainPanel;
    public GameObject upperPanel;
    public GameObject sidePanelHi;
    public GameObject sidePanelLo; 
    public RectTransform rectTransform;

    public void UpdateMenu()
    {
        switch (menuType)
        {
            case 0: //MainMenu
                
                mainPanel.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
                upperPanel.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
                sidePanelHi.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
                sidePanelLo.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
                break;
            default:
                break;
        }
    }
}