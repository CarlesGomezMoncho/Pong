using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float velocidadRaqueta = 10;
    public bool isIA = false;
    public bool isPlayer1 = true;
    public float limiteY = 10;
    public Vector2 lerpMinMax = new Vector2(0.1f, 0.8f);

    private GameObject pelota;
    //private float interpolacion = 0.5f;
    private float oldVertical = 0;

	// Use this for initialization
	void Start ()
    {
        pelota = GameObject.FindGameObjectsWithTag("Pelota")[0];
    }
	
	// Update is called once per frame
	void Update ()
    {

        float posicionY;
        float movimientoRaqueta;
        float vertical;

        if (isIA)
        {
            //posicionY = Mathf.Lerp(transform.position.y, pelota.transform.position.y, interpolacion);
            //movimientoRaqueta = vertical * velocidadRaqueta 
            if (transform.position.y > pelota.transform.position.y + 0.5f)
            {
                vertical = Mathf.Lerp(oldVertical, -1, 0.1f);
                oldVertical = vertical;
            }
            else if (transform.position.y < pelota.transform.position.y - 0.5f)
            {
                vertical = Mathf.Lerp(oldVertical, 1, 0.1f);
                oldVertical = vertical;
            }
            else
                vertical = 0;
        }
        else if (isPlayer1)
        {
            //vertical = Input.GetAxis("Vertical");
            vertical = 0;
            if(InputManager.instance.getButtonDown("Player1Up"))
            {
                vertical = 1;
            }
            else if (InputManager.instance.getButtonDown("Player1Down"))
            {
                vertical = -1;
            }


            //movimientoRaqueta = vertical * velocidadRaqueta * Time.deltaTime;
            //posicionY = transform.position.y + movimientoRaqueta;
        }
        else
        {
            //vertical = Input.GetAxis("Vertical");
            vertical = 0;
            if (InputManager.instance.getButtonDown("Player2Up"))
            {
                vertical = 1;
            }
            else if (InputManager.instance.getButtonDown("Player2Down"))
            {
                vertical = -1;
            }


            //movimientoRaqueta = vertical * velocidadRaqueta * Time.deltaTime;
            //posicionY = transform.position.y + movimientoRaqueta;
        }

        movimientoRaqueta = vertical * velocidadRaqueta * Time.deltaTime;
        posicionY = transform.position.y + movimientoRaqueta;

        if (posicionY > limiteY)
        {
            posicionY = limiteY;
        }
        else if (posicionY < -limiteY)
        {
            posicionY = -limiteY;
        }

        transform.position = new Vector3(transform.position.x, posicionY, transform.position.z);


    }
}
