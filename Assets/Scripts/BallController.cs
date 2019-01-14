using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

    public float maxSpeed = 10;
    public Text Velocidad;
    public Vector3 posicionInicial = new Vector3(0, 0, 0);
    public float tiempoFinDePunto = 5f;

    public float minFresnelPower = 1;

    private Rigidbody rb;
    private bool colisionTrigger = false;
    private bool pushBall = false;

    private string fresnelPowerName = "Vector1_E20CF98A";   //lo he sacado compilando y mostrando el shader, ahi te sale el nombre de la propiedad, segur que hay alguna otra forma, pero de momento no se como
    private Material materialPared;
    private float currentFresnelPower;
    private float initFresnelPower;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Velocidad)
            Velocidad.text = rb.velocity.ToString();
        
        //si se ha asignado la variable del material, es que hay que modificar el parametro
        if (materialPared != null)
        {
            currentFresnelPower += 0.1f;

            materialPared.SetFloat(fresnelPowerName, currentFresnelPower);

            //si se llega al maximo, desactivamos el material
            if (currentFresnelPower == initFresnelPower)
            {
                materialPared.SetFloat(fresnelPowerName, initFresnelPower);
                materialPared = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (pushBall)
        {
            rb.AddForce(getNewDirection() * maxSpeed, ForceMode.Impulse);
            pushBall = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //si chocamos con alguna pared lateral i no se ha activado el choque aun (para que solo puntue una vez si golpea mas veces)
        if (collision.gameObject.tag == "SideWall" && !colisionTrigger)
        {
            if (collision.gameObject.name == "Izquierda")
            {
                GameController.instance.setPoint(2);
            }
            else
            {
                GameController.instance.setPoint(1);
            }

            materialPared = collision.gameObject.GetComponent<Renderer>().material;
            initFresnelPower = materialPared.GetFloat(fresnelPowerName);
            currentFresnelPower = minFresnelPower;

            return;
        }

        if (collision.gameObject.name == "Raqueta 1" || collision.gameObject.name == "Raqueta 2")
        {

            //Dependiendo de la y de la raqueta
            //cuando más se acerque al borde superior, más se acercará 1
            //si esta en el centro será 0
            //cuando más se acerque al borde inferior, más se acercará a -1

            //restamos la posición de la raqueta a la posición de la bola, para conocer la posición relativa entre ellas dos en el eje y.
            float y = (transform.position.y - collision.transform.position.y);

            //si es menor que 0.5 o mayor que -0.5 vamos a indicar que golpea en el centro de la raqueta, por tanto no habrá una modficación en la dirección
            if (y < 0.5f && y > -0.5f)
                y = 0;

            //modificamos la velocidad actual normalizando el valor obtenido antes, de tal forma que si golpea en el centro de la raqueta la pelota no modifica
            //la dirección. En cambio si golpe en los bordes se modifica la dirección actual en +1 (si golpea en la parte superior) o en -1 (si golpea en la parte inferior)
            //como el valor sera un valor decimal se normaliza, para que si es mayor que 0 sea 1 y si es menor que 0 sea -1 y asi se modificará en cada golpeo la 
            //dirección de la pelota en +1, 0 o -1 dependiendo de la zona en la que golpee la pelota en cada raqueta.
            rb.velocity = rb.velocity + new Vector3(0, y, 0).normalized;

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        //si golpea en la pared superior o inferior
        if (collision.gameObject.tag == "updownWall")
        {
            //si la velocidad de y es 0 (o casi) (si solo se mueve en el eje horizontal, es decir la pelota se "pega" a la pared y ya no hay forma de que cambie en este eje)
            if (rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
            {
                //si es la pared superior
                if (transform.position.y > posicionInicial.y)
                {
                    //empujon hacia abajo
                    rb.AddForce(0, -1, 0, ForceMode.Impulse);
                }
                else
                {
                    //empojon hacia arriba
                    rb.AddForce(0, 1, 0, ForceMode.Impulse);
                }
            }
        }
    }

    public bool startMovement()
    {
        //si la velocidad es cero y no estamos en estado de colisión con pared
        if (rb.velocity == Vector3.zero && !colisionTrigger)
        {
            setPushBall(true);
            return true;
        }

        return false;
    }

    public bool isMoving()
    {
        return rb.velocity != Vector3.zero;
    }

    public void setPushBall( bool pushballState)
    {
        pushBall = pushballState;
    }

    public bool getPushBall()
    {
        return pushBall;
    }

    //devuelve una dirección aleatoria en el eje x,y
    private Vector3 getNewDirection(int defaultX = 0)
    {
        Vector3 direction;

        int randX;
        float randY = Random.Range(-0.5f, 0.5f);

        //si se define una X por parametro esta se asigna directamente
        if (defaultX != 0)
        {
            randX = defaultX;
        }
        else
        {
            //valor aleatorio entre 0 i 1
            randX = Random.Range(0, 2);

            //si randX es 0 se cambia por -1 para que vaya a izquierda, 1 es a derecha
            if (randX == 0)
            {
                randX = -1;
            }
        }

        direction = new Vector3(randX, randY, 0);

        return direction;
    }

    public IEnumerator setInitPosition(int delayTime = 0)
    {
        transform.gameObject.GetComponent<SphereCollider>().material.staticFriction = 1f;
        transform.gameObject.GetComponent<SphereCollider>().material.dynamicFriction = 1f;
        transform.gameObject.GetComponent<SphereCollider>().material.bounciness = 0.4f;
        colisionTrigger = true;

        yield return new WaitForSeconds(delayTime);

        colisionTrigger = false;
        transform.gameObject.GetComponent<SphereCollider>().material.staticFriction = 0f;
        transform.gameObject.GetComponent<SphereCollider>().material.dynamicFriction = 0f;
        transform.gameObject.GetComponent<SphereCollider>().material.bounciness = 1f;

        transform.position = posicionInicial;
        rb.velocity = Vector3.zero;
        pushBall = false;

        if (GameController.instance.getIsDemo() || (GameController.instance.player1.isIA && GameController.instance.player2.isIA))
        {
            startMovement();
        }
        else
        {
            GameController.instance.setStartPoint();
        }
    }

    /*private IEnumerator wait(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }*/
}
