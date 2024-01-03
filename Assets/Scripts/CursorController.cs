using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class CursorController : MonoBehaviour
{

#region Variables
    [SerializeField] private Data data;
    [SerializeField] private GameObject cursorPrefab;
    private GameObject cursorClone;
    private int currentIndex = 0;
    private int trueIndex = 0;
    private int trueSelectedIndex = 0;
    private int selectedIndex = 0;

    private int writeArea;
    private int listLength;

    private int maxIndex;

    // Introduce a delay timer
    private float moveDelay = 0.1f; // Set the delay time in seconds
    private float selectDelay = 0.1f;
    private float timeSinceLastMove = 0f;
    private float timeSinceLastSelect = 0f;
    private bool isItemSelected = false;

    private Animator cursorCloneAnimator;
#endregion

    void Awake()
    {
        StartCoroutine(DelayedInitialization());
        writeArea = data.writeArea;
        //Debug.Log("write area is "+writeArea);
        maxIndex = data.maxIndex;
        //Debug.Log("max index is " +maxIndex);
        SnapToItem(0);
    }

    void Update()
    {
        float inputX = KeybindManager.instance.keybinds.horizontalAxis.ReadValue<float>();
        float inputY = KeybindManager.instance.keybinds.verticalAxis.ReadValue<float>();

        // Check if the input has changed (key released or pressed)
        if (inputY != 0)
        {
            // Check if enough time has passed since the last move
            if (Time.time - timeSinceLastMove > moveDelay)
            {
                if (inputY > 0)
                { 
                    MoveCursorUp();
                }
                else if (inputY < 0)
                {
                    MoveCursorDown();
                }
            }
        }
        else
        {
            // Reset the timer when the key is released
            timeSinceLastMove = 0f;
        }

        // account for x axis here?

        #region handle item selection
        // Handle item selection
        if (KeybindManager.instance.keybinds.confirm.triggered) ConfirmSelection();
        else if (KeybindManager.instance.keybinds.cancel.triggered) CancelSelection();
        #endregion

    }

    void MoveCursorUp()
    {
        
        if (currentIndex == 0 && trueIndex !=0) //if the cursor is at the top of the list and there are more items to scroll
        {
            trueIndex--;
            Debug.Log("moved up, true index is "+ trueIndex);
            GameObject menuContainer = GameObject.Find("MenuContainer");
            MenuManager menuManager = menuContainer.GetComponent<MenuManager>();
            menuManager.ScrollMenu(false, trueIndex);
            timeSinceLastMove = Time.time;        
            if(cursorClone != null) GhostSnap(selectedIndex, false);

            
        }       
        if (currentIndex > 0)
        {
            currentIndex--;
            trueIndex--;
            SnapToItem(currentIndex);
            timeSinceLastMove = Time.time;
        } 
    }

    void MoveCursorDown()
    {
        listLength = data.listLength;
        Debug.Log("list length is " + listLength);

        if (currentIndex < writeArea - 1)
        {  
            currentIndex++;
            trueIndex++;
            Debug.Log("moved down, true index is "+ trueIndex);

            SnapToItem(currentIndex);
            timeSinceLastMove = Time.time;


        }
        else if (trueIndex < (listLength -1))
        {
            trueIndex++;
            Debug.Log("listLength is " + listLength);
            Debug.Log("trueIndex is " + trueIndex);
            GameObject menuContainer = GameObject.Find("MenuContainer");
            MenuManager menuManager = menuContainer.GetComponent<MenuManager>();
            menuManager.ScrollMenu(true, trueIndex);
            
            timeSinceLastMove = Time.time;
            if(cursorClone != null)  GhostSnap(selectedIndex, true);
            

        } 

        // Update the time since the last move
    }


    void SnapToItem(int index)
    {
        
        // Construct the name of the desired child
        string childName = data.menuPrefix + "TextInstance_" + index.ToString();


        Transform textInstanceTransform = GameObject.Find(childName).transform;
        RectTransform rectTransform = textInstanceTransform.GetComponent<RectTransform>();
        float newY = rectTransform.anchoredPosition.y;
        float newX = rectTransform.anchoredPosition.x;
        float xPos = rectTransform.sizeDelta.x * 0.5f;
        float yPos = Mathf.Abs(rectTransform.rect.height);
        Debug.Log("yPos is "+ yPos);
        float oldYPos = rectTransform.sizeDelta.y * 0.5f;
        Debug.Log("oldYPos is " + yPos);
        GetComponent<RectTransform>().anchoredPosition = new Vector3 (newX-xPos, newY+oldYPos, 0f);
    }

    void SelectItem(int index)
    {
        Debug.Log("Selected item at index: " + index);

        if (cursorClone !=null)
        {
            if (cursorCloneAnimator == null)
            {
                cursorCloneAnimator = cursorClone.GetComponent<Animator>();
            }

            if (cursorCloneAnimator != null)
            {
                cursorCloneAnimator.SetTrigger("FlickerTrigger");
            }
        }
    }

    void SetSortingOrder(GameObject obj, int order)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }       

    int GetSortingOrder()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.sortingOrder;
        }
        return 0;
    }

    void GhostSnap(int index, bool down)
    {
        int scrollDirection = down ? -1 : 1;
        int newIndex = index +scrollDirection;
        selectedIndex = newIndex;
        // Construct the name of the desired child
        if(newIndex >= 0 && newIndex <= writeArea-1)
        {
            string childName = "TextInstance_" + newIndex.ToString();
            Transform textInstanceTransform = GameObject.Find(childName).transform;
            RectTransform rectTransform = textInstanceTransform.GetComponent<RectTransform>();
            float newY = rectTransform.anchoredPosition.y;
            cursorClone.GetComponent<RectTransform>().anchoredPosition = new Vector3(6f, 5f+newY, 0f);
        }
        else
        {
            cursorClone.GetComponent<RectTransform>().anchoredPosition = new Vector3 (-10000f, -10000f, 0f);
        }
    }

#region item selection methods
    void ConfirmSelection()
    {
        //enabale sorting (ex field / battle item menu)
        if(data.isMenuSortable)
        {
            if(Time.time - timeSinceLastSelect > selectDelay)
            {
                if (!isItemSelected)
                {
                    // Perform actions when an item is selected
                    SelectItem(trueIndex);
                    isItemSelected = true;

                    cursorClone = Instantiate(cursorPrefab, transform.position + new Vector3(6f, 5f, 0f), Quaternion.identity);
                    cursorClone.name = "GhostCursor";
                    cursorClone.transform.SetParent(GameObject.Find("CursorContainer").transform);
                    SetSortingOrder(cursorClone, GetSortingOrder() -2);
                    timeSinceLastSelect = 0f;
                    selectedIndex = currentIndex;
                    trueSelectedIndex = trueIndex;
                }
                else if (isItemSelected)
                {
                    GameObject menuContainer = GameObject.Find("MenuContainer");
                    MenuManager menuManager = menuContainer.GetComponent<MenuManager>();
                    menuManager.SwapItems(trueSelectedIndex, trueIndex, currentIndex);
                    isItemSelected = false;
                    if(cursorClone != null) Destroy(cursorClone);

                }

            }
        }
        else
        {   
            Debug.Log("True Index is " +trueIndex);
            MenuManager menuManager = GameObject.Find("MenuContainer").GetComponent<MenuManager>();
            menuManager.MenuSelect(trueIndex);
        }
    }

    
    void CancelSelection()
    {
        if(Time.time - timeSinceLastSelect > selectDelay)
        {
            if(isItemSelected) isItemSelected = false;
            if (cursorClone != null) Destroy(cursorClone);
            timeSinceLastSelect = 0f;

            if(!isItemSelected)
            {
                MenuManager menuManager = GameObject.Find("MenuContainer").GetComponent<MenuManager>();
                switch(data.menuPrefix)
                {
                    case "main":
                        break;
                    case "itemsMenu":
                        data.menuPrefix = "itemsSupMenu";
                        SnapToItem(0);
                        break;
                    case "itemsSupMenu":
                        data.menuPrefix = "main";
                        menuManager.MenuSelect(20);
                        SnapToItem(0);
                        break;
                    default:
                        break;
                }
            }
        }

    }
#endregion



    IEnumerator DelayedInitialization()
{
    yield return null; // Wait for the next frame

    writeArea = data.writeArea;
    // Debug.Log("write area is " + writeArea);

    maxIndex = data.maxIndex;
    // Debug.Log("max index is " + maxIndex);

    SnapToItem(0);
}
}


    
    
