using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    private Dictionary<string, KeyCode> buttonKeys;

    void Awake()
    {
        //If we don't currently have an instance...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start ()
    {
        buttonKeys = new Dictionary<string, KeyCode>();

        buttonKeys["Player1Up"] =   (KeyCode)PlayerPrefs.GetInt("Player1Up",   (int)KeyCode.W); 
        buttonKeys["Player1Down"] = (KeyCode)PlayerPrefs.GetInt("Player1Down", (int)KeyCode.S);
        buttonKeys["Player2Up"] =   (KeyCode)PlayerPrefs.GetInt("Player2Up",   (int)KeyCode.UpArrow);
        buttonKeys["Player2Down"] = (KeyCode)PlayerPrefs.GetInt("Player2Down", (int)KeyCode.DownArrow);
    }
	
    public KeyCode getValue(string key)
    {
        return buttonKeys[key];
    }

    public bool getButtonDown(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("InputManager::GetButtonDown -- no button named: " + buttonName);
            return false;
        }

        return Input.GetKey(buttonKeys[buttonName]);
    }

    public void setButtonForKey( string buttonName, KeyCode keyCode)
    {
        buttonKeys[buttonName] = keyCode;
        PlayerPrefs.SetInt(buttonName, (int)keyCode);
        PlayerPrefs.Save();
    }
}
