  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour, ICambiodeState
{
    public int vida = 3;
    public int contadorPlanear = -1;
    public GameObject muerteUI, menuWin;
   
    public static PlayerMove playerInstance { get; private set; }
    public Animator animPlayer;

    public GameObject espada;
    public Vector3 move;
    public CharacterController controller;
    public float CharacterVelocityY, JumpSpeed = 10f, segundosalto = 20f, speed = 10f, x, y,gravedad=50f;

    //Camera
    public GameObject playerMuerto;
    public bool atacado = false,planeando=false;
    private Camera cam;
    public Transform TargetaSeguir;



    public bool groundedPlayer,entrarUnaVez=true;

    bool dobleSaltoCompletado = false;


    public List<GameObject> trampa1 = new List<GameObject>();
    public List<GameObject> vidas = new List<GameObject>();

    public Transform player;
  

    public Vector3 inercia;

    //public static event PlayerDeath OnPlayerDeath;
    public bool invensible = false;

    public State currentstate;

    public Vector3 offset = new Vector3(0f, 1.5f, -3f);
    //public ParticleSystem luz;
    //public bool EnMuro = false;

    // public ParticleSystem GannchoLuz;
    public LayerMask groundMask;
    public Transform groundChecker;
    public Transform cameraLook;
    private bool camaraBloqueada = true;
    public bool animacionacabada = true;


    public List<ParticleSystem> particulas = new List<ParticleSystem>();//1-salto2-caminar

    RaycastHit ray;


    public bool isOnSlope = false;
    public Vector3 hitNormal;
    int contadorAtaque;
    

    private void Awake()//la instancia patron singleton
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
        }
        SujetoObservable.GenerarInstancia();
    }
   


    void Start()
    {
        vida = 3;

        //animPlayer = GetComponent<Animator>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;



        currentstate = State.DobleSalto;
        SujetoObservable.instancia.Subscribirse(this);
    }

    public void NotficiarCambiodeEstado(State state)
    {
        currentstate = state;
    }


    //public static event Action<State> OnStateChange;
    // Update is called once per frame
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BloqueMovible"&& Input.GetMouseButton(1) )//para mover el bloque tenemos que mantener pulasdo el click derecho
        {
           
                GameObject padre = other.transform.parent.gameObject;//aqui asignamos al gameobjet el padre, osea el cubo en si
                padre.transform.parent = transform;//lo enparentamos a nuetsr payer para poder moverlo
           

            
        }
        else 
        {
            if (other.tag == "BloqueMovible")
            {
                GameObject padre = other.transform.parent.gameObject;//aqui asignamos al gameobjet el padre, osea el cubo en si
                padre.transform.parent = null;//lo enparentamos a nuetsr payer para poder moverlo
            }
        }
       









    }

    

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "ZonaMuerte")
        {
            Debug.Log("Muere");
            Muerte();
        }
        if (other.tag == "Habilidad")
        {
           menuWin.SetActive(true);
        }
        if (other.tag == "Trampa")
        {
            foreach (var trampa in trampa1)
            {
                trampa.GetComponent<Rigidbody>().isKinematic = false;
            }
        }


        if (other.gameObject.CompareTag("GolpeoEnemigo")) //cuando golpea un enemigo este da un rebote        
        {
            other.GetComponentInParent<IEnemigo>().Golpeado();//Dependiendo que enemigo golpe hara una cosa o otra
            CharacterVelocityY = 0;
           
            CharacterVelocityY += JumpSpeed;
            planeando = false;
            contadorPlanear++;

        }
        if (other.tag == "Enemigo"&&!invensible)//si el enemigo te golpea a ti te va a alejar de un golpe.
        {
            float poderGolpe = other.GetComponentInParent<IEnemigo>().PoderGolpeao();
            //RecibirGolpe(other.transform);
           
            // atacado = true;
            //CharacterVelocityY += JumpSpeed;
            atacado = true;
            Vector3 direccionGolpe = (transform.position - other.GetComponentInParent<EnemigoController>().transform.position)*poderGolpe;
            planeando = false;
            contadorPlanear++;
         
            move = Vector3.zero;
            inercia = direccionGolpe ;
            CharacterVelocityY += 12f;

            particulas[3].Play();
            vida -= other.GetComponentInParent<IEnemigo>().DanoEnemigo();
            vidas[vida].SetActive(false);
           
            if (vida <=0)
            {
                Muerte();
            }




            // StartCoroutine(Golpeado(other.transform));




        }

    }
    
    private void OnTriggerExit(Collider other)
    {
       
       
        if (other.tag == "BloqueMovible")
        {
            GameObject padre = other.transform.parent.gameObject;
            padre.transform.parent = null;
        }
    }
    public void MovimientoPlayer()//movimiento Normal Basico
    {


        x = Input.GetAxis("Horizontal");//A D
        y = Input.GetAxis("Vertical");//W S




        Vector3 Adelante = cam.transform.forward;
        Adelante.y = 0;
        Adelante.Normalize();

        Vector3 Derecha = cam.transform.right;
        Derecha.y = 0;
        Adelante.Normalize();


      
        // move = (Derecha *x + Adelante * y ) * speed;//para que cambie la direcion con la camara



        move = (Derecha * 0 + Adelante * 0);//para que cambie la direcion con la camara


        if ((x != 0 || y != 0) && animacionacabada && !atacado &&!isOnSlope)//condiciones para que no se mueva cuando esta atacando o ha sido golpeado
        {
            animPlayer.SetFloat("x", x);
            animPlayer.SetFloat("y", y);
            move = (Derecha * x + Adelante * y) * speed;//para que cambie la direcion con la camara y camine
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.1f);

           // Instantiate(particulas[1], transform.position, Quaternion.identity);
        }













        //Finalizamos con el movimiento y dandole las variables













        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            inercia =  transform.forward* 200f;
            invensible = true;
        }


        if (inercia.magnitude >= 0)//para que una vez golpeado no se pare en seco, hacemos que vaya bajando la velocidad del golpe
        {
            float emuje = 24;//como menor sea mas pdoersos ser? el golpe

            inercia -= inercia * emuje*Time.deltaTime;

            if (inercia.magnitude < 0.1f)
            {

                inercia = Vector3.zero;
                atacado = false;
                invensible = false;
            }
            

        }
       move += inercia;
        move.y = CharacterVelocityY;

        if (true)
        {
            EstaEnRampa();
            controller.Move(move * Time.deltaTime);
        }

    }
    void Update()
    {
        //CogerObjeto();
        //hola
       
        groundedPlayer = Physics.CheckSphere(groundChecker.position, 0.4f, groundMask);//booleano que indica si esta tocando el suelo

        
        //brazo.position = BrazoBien.position;
        //brazo.position = BrazoBien;
        switch (currentstate)//Aqui es el control de States, dependiendo el que tenga hará unas cosas o otras
         {

             default:

             case State.MovimientosPrincipales:
                MovimientoPlayer();
               
        
                Jump();


                break;

             case State.DobleSalto:

                MovimientoPlayer();

                AtaqueEspada();

                DobleSalto();
                Agachado();



                break;
          
               
                
            case State.Planear:
                MovimientoPlayer();
                Planear();
                break;
                 //case State.GetShoot:

                 // break;



         }


        /*MovimientoPlayer();
        CameraRotativa3Persona();5rt
        CameraCosasRaras();
        MirarEnemigo();*/
    }
    

    private void DobleSalto()
    {
        
        if (groundedPlayer && CharacterVelocityY < 0)//si esta en el sueo y su velocidad no es menor que 0.
        {
            CharacterVelocityY = -4f;//este -4 es para que al bajar rampas no se bugue y haga el doble salto bien
            dobleSaltoCompletado = false;
        }
        else//Si no esta en el suelo vamos a ejerecer la gravedad para que caiga
        {
            CharacterVelocityY -= gravedad * Time.deltaTime;
            EstaEnRampa();
        }
        if (Input.GetButtonDown("Jump") && groundedPlayer)//si esta en el suelo y puslas espacio vas a saltar
        {
            CharacterVelocityY = 0;//resetaamos la velocidad de el eje Y para evitar errores y luego aplicamos la fuerza del salto
            animPlayer.SetTrigger("Saltar");
            CharacterVelocityY += JumpSpeed;
            particulas[0].Play();
        }
        if (Input.GetButtonDown("Jump") && !groundedPlayer && !dobleSaltoCompletado)//si no esta en el suelo pulsamos otra vez el espacio hara un doble salto, el booleano hace que solo entre una vez.
        {
            CharacterVelocityY = 0;
            animPlayer.SetTrigger("Saltar");
            CharacterVelocityY += segundosalto;
            dobleSaltoCompletado = true;//este bool evita que salte mas de una vez, una vez toque el suelo podrá volver a entrar al doble salto
            particulas[0].Play();
        }

    }

    public void Planear()//Para planear 
    {


        if (groundedPlayer && entrarUnaVez &&planeando)//Esto es basicamente para que contador sume si toca el suelo mientras esta planeando, si no el contador se desincroniza y hay errores
        {
            contadorPlanear++;
            entrarUnaVez = false;
           
        }
        if(groundedPlayer && CharacterVelocityY < 0 )//lo mismo que con el doble salto pero con planear
        {
          
            CharacterVelocityY = -4f;
            planeando = false;
           
            
            
        }
       
        if (Input.GetButtonDown("Jump") && groundedPlayer)//same
        {

            CharacterVelocityY += JumpSpeed;
        }
        if (Input.GetButtonDown("Jump") && !groundedPlayer)//en este caso si no esta en el suelo y pulsamos otra vez el space va a planear, activamos el bool lo que hará que cambie la gravedad
        {
            entrarUnaVez = true;
            planeando = true;
            CharacterVelocityY = 0;
            contadorPlanear++;//este contador sirve para que el jugador cada vez que de al espacio deje de planear o vuelva a planear


        }
        if (planeando && contadorPlanear%2 ==0)//Planea en par, deja de planear en inpar
        {
            
            CharacterVelocityY -= 2f * Time.deltaTime;
           
        }
        else
        {
            CharacterVelocityY -= gravedad * Time.deltaTime;
            planeando = false;
        }




        move.y = CharacterVelocityY;




    }
  
  void Agachado()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animPlayer.SetBool("Agachado",true);
        }
        else
        {
            animPlayer.SetBool("Agachado", false);
        }

    }


    public void AtaqueEspada()
    {
        if (Input.GetMouseButton(0) && contadorAtaque == 1)
        {
            animPlayer.SetTrigger("Ataque1");
        }
        if (Input.GetMouseButton(0) && animacionacabada)
        {
            contadorAtaque = 0;
            animacionacabada = false;
            contadorAtaque++;
            animPlayer.SetTrigger("Ataque");
            


            StartCoroutine(Ataque());










        }
    }
    IEnumerator Ataque()
    {

        yield return new WaitForSeconds(0.19f);
       
        animacionacabada = true;
    }

    
    public void Jump()
    {

        

        if (groundedPlayer && CharacterVelocityY < 0)
        {
            CharacterVelocityY = -4f;
        }
        else
        {
            CharacterVelocityY -= gravedad*Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
           
            CharacterVelocityY += JumpSpeed;
        }
       
        



        move.y = CharacterVelocityY;
       // controller.Move(move * Time.deltaTime);
    }
    
    void RecibirGolpe(Transform tr)
    {
       
         
       
    }
    public void EstaEnRampa()
    {
        isOnSlope = Vector3.Angle(Vector3.up,hitNormal) >= controller.slopeLimit;
        if (isOnSlope)
        {
            move.x += hitNormal.x * 2 ;
            move.z += hitNormal.z * 2;

            move.y += -4f; 
            
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

    }
    void Muerte()
    {
        playerMuerto.SetActive(true);
        playerMuerto.transform.parent = null;
        transform.gameObject.SetActive(false);
        Invoke("spawner", 3f);
    }   
    void spawner()
    {
        muerteUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // SceneManager.LoadScene(0);
    }
    public void respawn()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Scene1y2":
                SceneManager.LoadScene(1);
                break;
            case "Scene3":
                SceneManager.LoadScene(2);
                break;

            default:
                break;
        }
        
    }

    /* void MirarEnemigo()
     {
         if (Input.GetKey(KeyCode.LeftControl))//Metodos para LookAt pero que la Y se mantenga en 0 para que no rote
         {
             a = true;
             camaraBloqueada = false;
             cam.transform.position = Vector3.Slerp(cam.transform.position, cameraLook.position, 0.01f);
             cam.transform.LookAt(Enemigo);

             //Metodo1
             //Vector3 targetPostition = new Vector3(Enemigo.position.x,transform.position.y,Enemigo.position.z);
             //this.transform.LookAt(targetPostition);


             //Metodo 2
             var lookPos = Enemigo.position - transform.position;
             lookPos.y = 0;
             var rotation = Quaternion.LookRotation(lookPos);//Lo convertimos en Quatrerion para luego darle la info
             transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
             //anguloRotaci?n.x = Enemigo.position.x - transform.position.x * Mathf.Deg2Rad;
             anguloRotacion.x = cameraLook.transform.position.x * Mathf.Deg2Rad;
             anguloRotacion.y = cameraLook.transform.position.y * Mathf.Deg2Rad;





         }
         else
         {
             camaraBloqueada = true;
             if (a)
             {








                 a = false;
             }

         }
     }*/











}


