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
        
        [SerializeField] private Menu menu;

        [SerializeField] private GameObject menuObject;

        [SerializeField] private Sprite subMenuImage;
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
        void Start()
        {
            DrawSubMenu();
            data.listLength = items.Count;
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
        }
        #endregion
        //-------------------------------------------------------------

        void DisplayText(RectTransform parent)
        {
            int writeArea = Mathf.RoundToInt((parent.sizeDelta.y - 6f) / textSpacingY);
            data.writeArea = writeArea;
            float xPos = parent.sizeDelta.x * 0.5f;
            float yPos = (parent.sizeDelta.y * 0.5f) - 12f;

            itemNames.Clear();
            for(int i = 0; i<writeArea; i++)
            {
                if(i >= items.Count) continue;
                GameObject textInstance = Instantiate(textElement);

                textInstance.name = "TextInstance_" + i.ToString();

                textInstance.transform.SetParent(parent);


                Vector3 position = new Vector3(xPos, yPos + (-i * textSpacingY), 0f);
                textInstance.GetComponent<RectTransform>().anchoredPosition = position;
                TextMeshProUGUI itemText = textInstance.GetComponent<TextMeshProUGUI>();
                itemText.text = items[i];
                itemNames.Add(items[i]);
                itemInstance.Add(textInstance);

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

        void DrawSubMenu()
        {
            for(int i = 0; i<2; i++)
            {
                GameObject menuObjectInstance = Instantiate(menuObject, transform); // Set the current script's GameObject as the parent
                Transform rtPanel = menuObjectInstance.transform.Find("RtPanel");
                if(rtPanel == null) Debug.Log("RtPanel not found!");
                Transform rtCorner = menuObjectInstance.transform.Find("RtCorner");
                if(rtCorner == null) Debug.Log("RtCorner not found!");

                Vector3 rtPanelPosition = rtPanel.GetComponent<RectTransform>().anchoredPosition;
                Vector3 rtCornerPosition = rtCorner.GetComponent<RectTransform>().anchoredPosition;
                Vector2 rtPanelSize = rtPanel.GetComponent<RectTransform>().sizeDelta;
                Vector2 rtCornerSize = rtCorner.GetComponent<RectTransform>().sizeDelta;

                if(i==1)
                { 
                    menuObjectInstance.name = "rtPanel";


                    // Replace the image component with your desired sprite
                    Image menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    //setting the size of the instantiated menu
                    RectTransform menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = rtPanelPosition;
                        menuRectTransform.sizeDelta = rtPanelSize;
                    }
                    
                    //Whether to get rid of the gradient
                    Transform gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }
                    RectTransform parent = menuObjectInstance.transform.Find("TextContainer").GetComponent<RectTransform>();
                    
                    RectTransform rtPanelRT = rtPanel.GetComponent<RectTransform>();

                    parent.anchoredPosition = new Vector2(0f, 0f);
                    parent.sizeDelta = new Vector2(46f, 96f);                 
                    textSpacingY = 11.5f;
                    DisplayText(parent);
                    data.isMenuSortable = false;
                    DrawCursor(parent);
                    i = 99;
        
                }
                else if(i==0)
                {   
                    /*GameObject destroy = GameObject.Find("RtCorner");
                    GameObject old = destroy;
                    Destroy(old);*/
                    
                    menuObjectInstance.name = "rtCorner";

                    Image menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    RectTransform menuRectTransform = menuObjectInstance.GetComponent<RectTransform>();
                    if (menuRectTransform != null)
                    {
                        menuRectTransform.anchoredPosition = rtCornerPosition;
                        menuRectTransform.sizeDelta = rtCornerSize;
                    }
                    Transform gradientChild = menuObjectInstance.transform.Find("Gradient");
                    if (gradientChild != null)
                    {
                        Destroy(gradientChild.gameObject);
                    }

                }
                else
                {
                    Debug.Log("i = "+i);
                    continue;

                }        

                
            }
        }

        void DrawCursor(RectTransform parent)
        {
            //Set the cursor's transform to the proper parent
            GameObject cursorInstance = Instantiate(cursor, transform);
            cursorInstance.name = "Cursor";
            cursorInstance.transform.SetParent(parent.transform);
            
            #region Local vars
            string childName = "TextInstance_0";
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

        public void ScrollMenu(bool scrollDown, int trueIndex)
        {           
            if(scrollDown)currentItemIndex = trueIndex - data.writeArea; 
            if(!scrollDown)currentItemIndex = trueIndex + 1;
            int scrollDirection = scrollDown ? 1 : -1;
            int newIndex = currentItemIndex + scrollDirection;

            if (newIndex < 0)
            {
                newIndex = 0;
            }
            else if (newIndex >= items.Count)
            {
                newIndex = items.Count - 1;
            }

            currentItemIndex = newIndex;

            UpdateMenuDisplay();
        }
        private void UpdateMenuDisplay()
        {
            itemNames.Clear();
            for (int i = 0; i < data.writeArea; i++)
            {
                int itemIndex = currentItemIndex + i;
                if (itemIndex < items.Count)
                {
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = items[itemIndex];
                    itemNames.Add(items[itemIndex]);

                }
                else
                {
                    // Clear text for empty slots
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = "";
                }
            }
            GameObject quantList = GameObject.Find("QuantContainer");
            foreach (Transform child in quantList.transform)
            {
                Destroy(child.gameObject);
            }
            DisplayQuant();
        }

        public void SwapItems(int index1, int index2, int offset)
        {
            if (index1 >= 0 && index1 < items.Count && index2 >= 0 && index2 < items.Count)
            {
                string temp = items[index1];
                items[index1] = items[index2];
                items[index2] = temp;
                currentItemIndex = index2 - offset;
                UpdateMenuDisplay();
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
                    menu.OpenItemsMenu();
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
                default:
                    break;
            }

        }
    }
