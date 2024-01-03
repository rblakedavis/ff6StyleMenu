using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




    public class MenuManager : MonoBehaviour
    {
        #region Variables
        //------------------------------------------------------------------
        [SerializeField] private GameObject cursor;
        [SerializeField] private GameObject textElement;
        [SerializeField] private GameObject quantElement;
        [SerializeField] private RectTransform textContainer;
        [SerializeField] private RectTransform quantContainer;
        
        [SerializeField] public List<string> items = new List<string>();

        [SerializeField] private float textSpacingY;
        [SerializeField] private float textSpacingX;
        
        [SerializeField] private Menu menu;

        [SerializeField] private GameObject menuObject;

        [SerializeField] private Sprite mainMenuImage;
        [SerializeField] private Sprite subMenuImage;
        [SerializeField] private Sprite backMenuImage;
        [SerializeField] private Data data;
        [SerializeField] TextMeshProUGUI timerText;
        private List<GameObject> itemInstance = new List<GameObject>();
        private List<string> itemNames = new List<string>();
        private int currentItemIndex = 0;

        Dictionary<string, int> itemDict = new Dictionary<string, int>();

    


        //-------------------------------------------------------------
        #endregion

        #region Unity Methods
        //-------------------------------------------------------------
        void Awake()
        {
            data.cursorBehavior = "2Axis";
            DrawSubMenu(0);
        }

        void Update()
        {
            GameObject rtCorner = GameObject.Find("rtCorner");
            if (rtCorner != null)
            {
                float timeInSeconds = Time.time;
                string formattedTime = FormatTime(timeInSeconds);
                timerText.text = "<color=#00DBDE>Time:</color>\n" + formattedTime;
            }
            else if (rtCorner == null)
            {
                timerText.text = "";
            }
        }
        #endregion
        //-------------------------------------------------------------

        void DisplayText(RectTransform parent, List<string> contents, float scale, float offsetX = 0, float offsetY = 0)
        {
            data.listLength = contents.Count;
            itemInstance.Clear();
            itemNames.Clear();
            
            int writeArea = Mathf.RoundToInt((parent.rect.height - 6f) / textSpacingY);
            data.writeArea = writeArea;
            if (writeArea < 1)
            {
                writeArea = 1;
            }
            Debug.Log("Write Area is "+ writeArea);
            float xPos = (parent.sizeDelta.x  * scale) + offsetX;
            float yPos = (parent.sizeDelta.y * scale) - 12f + offsetY;


            for(int i = 0; i<writeArea; i++)
            {
                if(i >= contents.Count) continue;
                GameObject textInstance = Instantiate(textElement);

                textInstance.name = data.menuPrefix + "TextInstance_" + i.ToString();

                textInstance.transform.SetParent(parent);


                Vector3 position = new Vector3(xPos, yPos + (-i * textSpacingY), 0f);
                textInstance.GetComponent<RectTransform>().anchoredPosition = position;
                TextMeshProUGUI itemText = textInstance.GetComponent<TextMeshProUGUI>();
                itemText.text = contents[i];
                itemNames.Add(contents[i]);
                itemInstance.Add(textInstance);




            }
            data.writeArea = itemInstance.Count;
            DisplayQuant();
        }

        void DisplayTextX(RectTransform parent, List<string> contents, float scale, int defaultWriteArea = 0)
        {
            int writeArea = Mathf.RoundToInt((parent.sizeDelta.x - 6f) / 16);
            Debug.Log("parent.SizeDelta.x is " + parent.sizeDelta.x);
            Debug.Log("write area is " + writeArea);
            data.writeArea = writeArea;
            if (writeArea<1) writeArea=1;
            if (writeArea<defaultWriteArea) writeArea = defaultWriteArea;
            float xPos = (parent.sizeDelta.x + 1) * scale;
            float currentXPos = -18f;
            float yPos = ((parent.sizeDelta.y +1) * scale) - 5f;

            itemNames.Clear();
            for(int i = 0; i<writeArea; i++)
            {
                if(i >= contents.Count) continue;
                GameObject textInstance = Instantiate(textElement);

                textInstance.name = data.menuPrefix + "TextInstance_" + i.ToString();
                

                textInstance.transform.SetParent(parent);


                //Vector3 position = new Vector3(xPos + (i * textSpacingX), yPos, 0f);
                //textInstance.GetComponent<RectTransform>().anchoredPosition = position;
                TextMeshProUGUI itemText = textInstance.GetComponent<TextMeshProUGUI>();
                itemText.text = contents[i];
                itemNames.Add(contents[i]);
                itemInstance.Add(textInstance);

                float textWidth = itemText.preferredWidth;
           
                Vector3 positionDeux = new Vector3(currentXPos, yPos, 0f);
                textInstance.GetComponent<RectTransform>().anchoredPosition = positionDeux;
                if(textWidth<32) textWidth=32;
                Debug.Log("text width is "+ textWidth);
                currentXPos += (textWidth + textSpacingX);                

            }
            data.writeArea = itemInstance.Count;
            DisplayQuant();

        }

        void DisplayQuant()
        {/*
            
            if(data.writeArea != null) 
            {
                int writeArea = data.writeArea;
            }
            else
            {
                int writeArea = Mathf.RoundToInt((textContainer.sizeDelta.y - 16f) / textSpacingY);
                data.writeArea = writeArea;
            }
            int index = 0;
            foreach(string str in itemNames)
            {

                string quant = itemDict[str].ToString();
                
                GameObject quantInstance = Instantiate(quantElement);
                quantInstance.name = str + "Quant";
                quantInstance.transform.SetParent(quantContainer);
                Vector3 position = new Vector3 (0f, -index * textSpacingY, 0f);
                quantInstance.GetComponent<RectTransform>().anchoredPosition = position;
                TextMeshProUGUI quantText = quantInstance.GetComponent<TextMeshProUGUI>();
                quantText.text = quant;

                index++;

            }
        */}

        void DrawSubMenu(int menuType)
        {
            GameObject menuObjectInstance; //= Instantiate(menuObject, transform); // Set the current script's GameObject as the parent
            #region old code that used to work
            //Transform rtPanel = menuObjectInstance.transform.Find("RtPanel");
            //if(rtPanel == null) Debug.Log("RtPanel not found!");
            //Transform rtCorner = menuObjectInstance.transform.Find("RtCorner");
            //if(rtCorner == null) Debug.Log("RtCorner not found!");

            //Vector3 rtPanelPosition = rtPanel.GetComponent<RectTransform>().anchoredPosition;
            //Vector3 rtCornerPosition = rtCorner.GetComponent<RectTransform>().anchoredPosition;
            //Vector2 rtPanelSize = rtPanel.GetComponent<RectTransform>().sizeDelta;
            //Vector2 rtCornerSize = rtCorner.GetComponent<RectTransform>().sizeDelta;
            #endregion
            RectTransform menuRectTransform;
            Image menuImage;
            Transform gradientChild;
            Transform timerChild;
            RectTransform parent;
            RectTransform rt;

            switch(menuType)
            {
                //MenuType 0 = main menu 0-1
                #region case 0 (right corner)
                case 0: //rtCorner

                    Transform inactiveMenu = transform.Find("Menu");
                    inactiveMenu.gameObject.SetActive(true);                    
                    Image inactiveMenuImage = inactiveMenu.GetComponent<Image>();
                    inactiveMenuImage.sprite = mainMenuImage; 



                    

                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform rtCorner = menuObjectInstance.transform.Find("RtCorner");
                    if(rtCorner == null) Debug.Log("RtCorner not found!");
                    Vector3 rtCornerPosition = rtCorner.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 rtCornerSize = rtCorner.GetComponent<RectTransform>().sizeDelta;
                    
                    menuObjectInstance.name = "rtCorner";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null) menuImage.sprite = subMenuImage;

                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = rtCornerPosition;
                        menuRectTransform.sizeDelta = rtCornerSize;
                    }
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }

                    DrawSubMenu(1);
                    break;
                #endregion

                #region case 1 (right panel)
                case 1:
                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform rtPanel = menuObjectInstance.transform.Find("RtPanel");
                    if(rtPanel == null) Debug.Log("RtPanel not found!");
                    Vector3 rtPanelPosition = rtPanel.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 rtPanelSize = rtPanel.GetComponent<RectTransform>().sizeDelta;

                    menuObjectInstance.name = "rtPanel";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    //setting the size of the instantiated menu
                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = rtPanelPosition;
                        menuRectTransform.sizeDelta = rtPanelSize;
                    }
                    
                    //Whether to get rid of the gradient
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }

                    parent = menuObjectInstance.transform.Find("TextContainer").GetComponent<RectTransform>();
                    
                    RectTransform rtPanelRT = rtPanel.GetComponent<RectTransform>();

                    parent.anchoredPosition = new Vector2(0f, 0f);
                    parent.sizeDelta = new Vector2(46f, 96f);                 
                    textSpacingY = 11.5f;
                    data.content = items;
                    data.menuPrefix = "main";
                    DisplayText(parent, items, 0.5f);
                    data.isMenuSortable = false;
                    DrawCursor(parent);
                    break;
                #endregion
                //MenuType 2 = items menu 2-5
                #region case 2 (small menu says "items")
                case 2:
                    DestroyOldMenus();
                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform itemLCorner = menuObjectInstance.transform.Find("ItemLCorner");
                    if(itemLCorner == null) Debug.Log("ItemLCorner not found");
                    Vector3 itemLCornerPosition = itemLCorner.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 itemLCornerSize = itemLCorner.GetComponent<RectTransform>().sizeDelta;

                    menuObjectInstance.name = "itemLCorner";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = itemLCornerPosition;
                        menuRectTransform.sizeDelta = itemLCornerSize;
                    }
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }

                    parent = menuObjectInstance.transform.Find("TextContainer").GetComponent<RectTransform>();
                    
                    rt = menuObjectInstance.GetComponent<RectTransform>();

                    parent.anchoredPosition = new Vector3 (42f, 9f, 0f);
                    parent.sizeDelta = new Vector2 (0f, 0f);                 
                    DisplayText(parent, data.topLeftCorner, 1);
                    
                    

                    DrawSubMenu(3);
                    break;
                #endregion
                #region case 3 (use / arrange / rare)
                case 3:
                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform itemTPanel = menuObjectInstance.transform.Find("ItemTPanel");
                    if(itemTPanel == null) Debug.Log("ItemTPanel not found!");
                    Vector3 itemTPanelPosition = itemTPanel.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 itemTPanelSize = itemTPanel.GetComponent<RectTransform>().sizeDelta;

                    menuObjectInstance.name = "itemTPanel";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    //setting the size of the instantiated menu
                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = itemTPanelPosition;
                        menuRectTransform.sizeDelta = itemTPanelSize;
                    }
                    
                    //Whether to get rid of the gradient
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }
          

                    parent = menuObjectInstance.transform.Find("TextContainer").GetComponent<RectTransform>();
                    
                    rt = menuObjectInstance.GetComponent<RectTransform>();

                    parent.anchoredPosition = new Vector2(0f, 0f);
                    parent.sizeDelta = new Vector2(0f, 0f);         
                    data.menuPrefix = "itemsSupMenu";
                    DisplayTextX(parent, data.itemsMidPanel, 0.5f, 3);

                    //disable background menu (hide really)
                    inactiveMenu = transform.Find("Menu");                    
                    inactiveMenuImage = inactiveMenu.GetComponent<Image>();
                    inactiveMenuImage.sprite = backMenuImage; 
                    DrawSubMenu(4);
                    break;
                #endregion
                #region case 4 (description box)
                case 4:
                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform itemMidPanel = menuObjectInstance.transform.Find("ItemMidPanel");
                    if(itemMidPanel == null) Debug.Log("ItemMidPanel not found");
                    Vector3 itemMidPanelPosition = itemMidPanel.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 itemMidPanelSize = itemMidPanel.GetComponent<RectTransform>().sizeDelta;

                    menuObjectInstance.name = "itemMidPanel";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = itemMidPanelPosition;
                        menuRectTransform.sizeDelta = itemMidPanelSize;
                    }
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }
                    DrawSubMenu(5);

                    break;
                #endregion
                #region case 5 (items window)
                case 5:
                    menuObjectInstance = Instantiate(menuObject, transform);
                    Transform itemItemMenu = menuObjectInstance.transform.Find("ItemItemMenu");
                    if(itemItemMenu == null) Debug.Log("ItemItemMenu not found");
                    Vector3 itemItemMenuPosition = itemItemMenu.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 itemItemMenuSize = itemItemMenu.GetComponent<RectTransform>().sizeDelta;

                    menuObjectInstance.name = "itemItemMenu";
                    menuObjectInstance.tag = "menuDestroy";

                    menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = itemItemMenuPosition;
                        menuRectTransform.sizeDelta = itemItemMenuSize;
                    }
                    gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    timerChild = menuObjectInstance.transform.Find("TimerText");
                    if (timerChild != null)
                    {
                        Destroy(timerChild.gameObject);
                    }

                    parent = menuObjectInstance.transform.Find("TextContainer").GetComponent<RectTransform>();
                    
                    rt = menuObjectInstance.GetComponent<RectTransform>();

                    
                    parent.offsetMin = new Vector2(30, 20);
                    parent.offsetMax = new Vector2(-38, -15);
                    //parent.anchoredPosition = new Vector3 (0f, 0f, 0f);
                    
                    //parent.sizeDelta = new Vector2 (-12f, -6f);                 
                    textSpacingY = 11.5f;

                    data.menuPrefix = "itemsMenu";
                    DisplayText(parent, data.inventory, 1f, 23f, 85f);
                    data.content = data.inventory;
                    data.isMenuSortable = true;
                    DrawCursor(parent);
                    Debug.Log("parent rt is " + parent.rect);
                    Debug.Log("menu rt is " + rt.rect);
                    break;


                    break;
                #endregion
                default :
                Debug.Log("Menu Type out of range at = "+ menuType);
                break;

            }
        }

        void DrawCursor(RectTransform parent)
        {
            //Set the cursor's transform to the proper parent
            GameObject cursorInstance = Instantiate(cursor, transform);
            cursorInstance.name = "Cursor";
            cursorInstance.transform.SetParent(parent.transform);
            
            #region Local vars
            string childName = data.menuPrefix + "TextInstance_0";
            Transform textInstanceTransform = GameObject.Find(childName).transform;
            RectTransform rectTransform = textInstanceTransform.GetComponent<RectTransform>();
            float newY = rectTransform.anchoredPosition.y;
            float newX = rectTransform.anchoredPosition.x;
            float xPos = rectTransform.sizeDelta.x * 0.5f;
            float yPos = rectTransform.sizeDelta.y * 0.5f;
            RectTransform cursorTransform = cursorInstance.GetComponent<RectTransform>();
            #endregion
            
            //set the cursor position to the item list's position.
            cursorTransform.anchoredPosition = new Vector3 (newX-xPos, newY+yPos, 0f);
        }

        public void ScrollMenu(bool scrollDown, int trueIndex, List<string> content = null)
        {   
            if( content == null )content = data.content;        

            if(scrollDown)currentItemIndex = trueIndex - data.writeArea; 
            if(!scrollDown)currentItemIndex = trueIndex + 1;
            int scrollDirection = scrollDown ? 1 : -1;
            int newIndex = currentItemIndex + scrollDirection;

            if (newIndex < 0)
            {
                newIndex = 0;
            }
            else if (newIndex >= itemNames.Count)
            {
                newIndex = itemNames.Count - 1;
            }

            currentItemIndex = newIndex;

            UpdateMenuDisplay(content);
        }
        private void UpdateMenuDisplay(List<string> content = null)
        {
            if( content == null ) content = data.content;
            itemNames.Clear();
            for (int i = 0; i < data.writeArea; i++)
            {
                int itemIndex = currentItemIndex + i;
                if (itemIndex < content.Count)
                {
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = content[itemIndex];
                    itemNames.Add(content[itemIndex]);

                }
                else
                {
                    // Clear text for empty slots
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = "";
                }
            }
            /*GameObject quantList = GameObject.Find("QuantContainer");
            foreach (Transform child in quantList.transform)
            {
                Destroy(child.gameObject);
            }
            DisplayQuant();*/
        }

        public void SwapItems(int index1, int index2, int offset, List<string> content = null)
        {
            if(content == null) content = data.content;
            if (index1 >= 0 && index1 < content.Count && index2 >= 0 && index2 < content.Count)
            {
                string temp = content[index1];
                content[index1] = content[index2];
                content[index2] = temp;
                currentItemIndex = index2 - offset;
                UpdateMenuDisplay(content);
            }

        }

        string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);

            if (timeInSeconds < 3600)
            {
                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                int hours = Mathf.FloorToInt(timeInSeconds / 3600);
                return string.Format("{0:00}:{1:00}", hours, minutes);
            }
        }

        public void MenuSelect(int index)
        {

            switch(index)
            {
                case 0:
                    DrawSubMenu(2);
                    break;
                case 1:
                    menu.OpenSkillsMenu();
                    break;
                case 2:
                    menu.OpenEquipMenu();
                    break;
                case 3:
                    menu.OpenRelicMenu();
                    break;
                case 4:
                    menu.OpenStatusMenu();
                    break;
                case 5:
                    menu.OpenTrackMenu();
                    break;
                case 6:
                    menu.OpenConfigMenu();
                    break;
                case 7:
                    menu.OpenSaveMenu();
                    break;
                case 20:
                    DestroyOldMenus();
                    DrawSubMenu(0);
                    break;
                default:
                    break;
            }

        }
    
        public void DestroyOldMenus()
        {
            string tagToDestroy = "menuDestroy";
            GameObject[] destroyOldMenus = GameObject.FindGameObjectsWithTag("menuDestroy");
            foreach (var obj in destroyOldMenus)
            {
                Destroy(obj);                
            }



        }

    }
