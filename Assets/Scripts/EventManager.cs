using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager: MonoBehaviour {

    private EventSystem eventsystem;
    private GameObject currentSelected;

	// Use this for initialization
	void Start () {
        eventsystem = GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if mouse movement
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            //if any selected object
            if (eventsystem.currentSelectedGameObject)
            {
                currentSelected = eventsystem.currentSelectedGameObject;    //asign the last selected to var
                eventsystem.SetSelectedGameObject(null);                    //unasign the selected
            }
        }

        //if vertical key movement
        if (Input.GetAxis("Vertical") != 0)
        {
            //if not current selected
            if (!eventsystem.currentSelectedGameObject)
            {
                //if last selected saved
                if (currentSelected)
                    eventsystem.SetSelectedGameObject(currentSelected); //asign the last saved to the current
            }
            else
            {
                currentSelected = eventsystem.currentSelectedGameObject;    //asign the current selected as last current selected var
            }
        }
	}

}
