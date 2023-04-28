  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour, ICambiodeState
{
    public int vida = 3;
    public int contadorPlanear = -1;

    public static PlayerMove playerInstance { get; private set; }
    public Animator animPlayer;
   

    public Vector3 move;
    public CharacterController controller;
    public float CharacterVelocityY, JumpSpeed = 10f, segundosalto = 20f, speed = 10f, x, y,gravedad=50f;

    //Camera

    public bool atacado = false,planeando=false;
    private Camera cam;
    public Transform TargetaSeguir;



    public bool groundedPlayer,entrarUnaVez=true;

    bool dobleSaltoCompletado = false;
    



    public Transform player;
  

    public Vector3 inercia;
  
    //public static event PlayerDeath OnPlayerDeath;
   

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




    RaycastHit ray;


    public bool isOnSlope = false;
    public Vector3 hitNormal;


  

    private void Awake()//la instancia patron singleton
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SujetoObservable.GenerarInstancia();
    }
   


    void Start()
    {


        cam = GameObject.Find("Camera").GetComponent<Camera>();

        


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
        }
        
      

        if (other.gameObject.CompareTag("GolpeoEnemigo")) //cuando golpea un enemigo este da un rebote        
        {
            other.GetComponentInParent<IEnemigo>().Golpeado();//Dependiendo que enemigo golpe hara una cosa o otra
            CharacterVelocityY = 0;
           
            CharacterVelocityY += JumpSpeed;
            planeando = false;
            contadorPlanear++;

        }
        if (other.tag == "Enemigo")//si el enemigo te golpea a ti te va a alejar de un golpe.
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

            
            vida -= other.GetComponentInParent<IEnemigo>().DañoEnemigo();




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
            move = (Derecha * x + Adelante * y) * speed;//para que cambie la direcion con la camara y camine
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.1f);


        }












        //Finalizamos con el movimiento y dandole las variables















      
        if (inercia.magnitude >= 0)//para que una vez golpeado no se pare en seco, hacemos que vaya bajando la velocidad del golpe
        {
            float emuje = 24;//como menor sea mas pdoersos ser? el golpe

            inercia -= inercia * emuje*Time.deltaTime;

            if (inercia.magnitude < 0.1f)
            {

                inercia = Vector3.zero;
                atacado = false;
            }
            

        }
       move += inercia;
        move.y = CharacterVelocityY;

        if (animacionacabada)
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



                DobleSalto();



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
            CharacterVelocityY += JumpSpeed;
        }
        if (Input.GetButtonDown("Jump") && !groundedPlayer && !dobleSaltoCompletado)//si no esta en el suelo pulsamos otra vez el espacio hara un doble salto, el booleano hace que solo entre una vez.
        {
            CharacterVelocityY = 0;
            CharacterVelocityY += segundosalto;
            dobleSaltoCompletado = true;//este bool evita que salte mas de una vez, una vez toque el suelo podrá volver a entrar al doble salto
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
  
  
 

    public void AtaqueEspada()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            animacionacabada = false;
            animPlayer.SetTrigger("Ataque");

          

        }

    }
    public void ataqueFinalizado()
    {
    
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

