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
        [SerializeField] private RectTransform textContainer;
        
        [SerializeField] public List<string> items = new List<string>();

        [SerializeField] private float textSpacingY;
        
        [SerializeField] private Menu menu;

        [SerializeField] private GameObject menuObject;

        [SerializeField] private Sprite subMenuImage;
        [SerializeField] private Data data;
        private List<GameObject> itemInstance = new List<GameObject>();
        private int currentItemIndex = 0;

        //-------------------------------------------------------------
        #endregion

        #region Unity Methods
        //-------------------------------------------------------------
        void Start()
        {
            DrawSubMenu();
            DisplayText();
            DrawCursor();
            data.listLength = items.Count;
        }

        void Update()
        {

        }
        #endregion
        //-------------------------------------------------------------

        void DisplayText()
        {
            int writeArea = Mathf.RoundToInt((textContainer.sizeDelta.y - 16f) / textSpacingY);
            data.writeArea = writeArea;

            for(int i = 0; i<writeArea; i++)
            {
                if(i >= items.Count) continue;
                GameObject textInstance = Instantiate(textElement);

                textInstance.name = "TextInstance_" + i.ToString();

                textInstance.transform.SetParent(textContainer);

                Vector3 position = new Vector3(0f, -i * textSpacingY, 0f);
                textInstance.GetComponent<RectTransform>().anchoredPosition = position;
                TextMeshProUGUI itemText = textInstance.GetComponent<TextMeshProUGUI>();
                itemText.text = items[i];
                itemInstance.Add(textInstance);

            }
            data.writeArea = itemInstance.Count;
        }

        void DrawSubMenu()
        {
            for(int i = 0; i<2; i++)
            {
                GameObject menuObjectInstance = Instantiate(menuObject, transform); // Set the current script's GameObject as the parent
                Transform rtPanel = menuObjectInstance.transform.Find("RtPanel");
                if(rtPanel == null) Debug.Log("rtPanel not found!");
                Transform rtCorner = menuObjectInstance.transform.Find("RtCorner");
                if(rtCorner == null) Debug.Log("rtCorner not found!");

                Vector3 rtPanelPosition = rtPanel.GetComponent<RectTransform>().anchoredPosition;
                Vector3 rtCornerPosition = rtCorner.GetComponent<RectTransform>().anchoredPosition;
                Vector2 rtPanelSize = rtPanel.GetComponent<RectTransform>().sizeDelta;
                Vector2 rtCornerSize = rtCorner.GetComponent<RectTransform>().sizeDelta;

                if(i==1)
                { 
                    Debug.Log("i = "+i);

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
        
                }
                else if(i==0)
                {   
                    Debug.Log("i = "+i);
                    
                    // Replace the image component with your desired sprite
                    Image menuImage = menuObjectInstance.GetComponent<Image>();
                    if (menuImage != null)
                    {
                        menuImage.sprite = subMenuImage;
                    }

                    // Scale the RectTransform to be a fifth of its original size on x, y, and z
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

        void DrawCursor()
        {
            GameObject cursorInstance = Instantiate(cursor, transform);
            cursorInstance.name = "Cursor";
            cursorInstance.transform.SetParent(GameObject.Find("CursorContainer").transform);
            Vector2 position = GameObject.Find("TextInstance_0").GetComponent<RectTransform>().anchoredPosition;
            cursorInstance.GetComponent<RectTransform>().anchoredPosition = position;
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
            for (int i = 0; i < data.writeArea; i++)
            {
                int itemIndex = currentItemIndex + i;
                if (itemIndex < items.Count)
                {
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = items[itemIndex];
                }
                else
                {
                    // Clear text for empty slots
                    TextMeshProUGUI itemText = itemInstance[i].GetComponent<TextMeshProUGUI>();
                    itemText.text = "";
                }
            }
        }
    }
