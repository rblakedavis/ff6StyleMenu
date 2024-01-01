using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class Keybinds
{
    /*
    public InputAction up;
    public InputAction down;
    public InputAction left;
    public InputAction right;
    */
    public InputAction confirm;
    public InputAction cancel;
    public InputAction menu;
    public InputAction horizontalAxis;
    public InputAction verticalAxis;
}


public class KeybindManager : MonoBehaviour
{
    public Keybinds keybinds;

    public static KeybindManager instance;
    
    private void Awake()
    {
        // Ensure there is only one instance of the KeybindManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Enable Input System actions
        keybinds.confirm.Enable();
        keybinds.cancel.Enable();
        keybinds.menu.Enable();
        keybinds.horizontalAxis.Enable();
        keybinds.verticalAxis.Enable();
    }
}
