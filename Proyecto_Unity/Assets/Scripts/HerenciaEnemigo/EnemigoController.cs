using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoController : MonoBehaviour
{
    public int vida;
    public int damage;
    public Rigidbody rb;
    public NavMeshAgent agente;
    public float poderGolpe;
    bool puedenAtacar;
    public float distanciaEspera;
    public float distancia;
    public Vector3 followPlayer;
    public float speed = 10f;
    public bool playerEnZonaAtaque = false;
    public Vector3 posicionInicial;
    public TrailRenderer trail;
    // Start is called before the first frame update
    public Animator animEnemigo;
    float tiempo=5f;
    
    private void Start()
    {
        posicionInicial = transform.position;
        agente = GetComponent<NavMeshAgent>();
        animEnemigo = GetComponent<Animator>();
        trail = GetComponentInChildren<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Espada"&& !PlayerMove.playerInstance.animacionacabada)
        {


            transform.GetComponent<IEnemigo>().GolpeEspada();

        }
    }


    public void SeguirPlayer()
    {
        float tiempoAtaque = 1f;
        tiempo += Time.deltaTime;

        distancia = Vector3.Distance(transform.position, PlayerMove.playerInstance.transform.position);
        if (distancia<=40)
        {
            if (distancia>= distanciaEspera)
            {
                puedenAtacar = false;
                agente.speed = 5;
                agente.destination = PlayerMove.playerInstance.transform.position;
            }
            else//Para que cuando este quieto siga mirando al player para realizar la animacion de ataque hacia le direccion del player hacemos todo esto
            {
               

                followPlayer = (PlayerMove.playerInstance.transform.position - transform.position).normalized;//calculamos la direccion del vector3 de el enemigo al player
                puedenAtacar = true;
                agente.speed = 0;
                Quaternion seguir = Quaternion.LookRotation(followPlayer);//con la direccion la convertimos en Quaternion

                Quaternion rotacionSlerp = Quaternion.Slerp(transform.rotation, seguir, 0.02f);//un slerp para suavizar la rotación
                 transform.rotation = rotacionSlerp;//hacemos la rotación
                Vector3 rotacionOriginal = transform.eulerAngles;//con esto buscamos bloquear la rotacion x y la z para que al acercarse no se incline, solo necesitamos que rote en el eje Y
               transform.eulerAngles = new Vector3(0, rotacionOriginal.y, 0);
                if (tiempo >= tiempoAtaque)
                {
                   
                    tiempo = 0;
                    gameObject.GetComponent<IEnemigo>().Ataques();
                }
                

            }


        }
        
        else
        {
            agente.destination = posicionInicial;
            if (agente.destination == posicionInicial)
            {
                animEnemigo.speed = 0;
                animEnemigo.SetBool("Idle", true);
            }
            else
            {
                animEnemigo.SetBool("Idle", false);
            }
        }
    }
    public void enableTrail()
    {
       
        trail.startWidth = 1;
    }
    public void disableTrail()
    {
        trail.startWidth=0;
    }


}



/* public void SeguirPlayer(Transform Enemigo,Rigidbody rb)
 {
      distancia = Vector3.Distance(Enemigo.position, PlayerMove.playerInstance.transform.position);
     if (distancia <= 40)
     {
         distancia = Vector3.Distance(Enemigo.position, PlayerMove.playerInstance.transform.position);
         followPlayer = (PlayerMove.playerInstance.transform.position - this.transform.position).normalized;

         if (distancia > 0)
         {

             rb.velocity = new Vector3(followPlayer.x, 0, followPlayer.z) * speed;

         }
         else
         {
             rb.velocity = Vector3.zero;

         }

         //this.transform.LookAt(PlayerMove.playerInstance.transform.position);
         //transform.rotation = Quaternion.Euler(followPlayer);

         //transform.LookAt(PlayerMove.playerInstance.transform.position);
         Quaternion seguir = Quaternion.LookRotation(followPlayer);

         Quaternion rotacionSlerp = Quaternion.Slerp(Enemigo.rotation, seguir, 0.02f);
         Enemigo.rotation = rotacionSlerp;

         //para que al acercarse mucho no se incline, solo dejamos que rote en el eje Y
         Vector3 rotacionOriginal = transform.eulerAngles;
         Enemigo.eulerAngles = new Vector3(0, rotacionOriginal.y, 0);



     }
     else
     {


         followPlayer = (posicionInicial - Enemigo.position).normalized;
         rb.velocity = new Vector3(followPlayer.x, 0, followPlayer.z) * speed;
         Quaternion vuelta = Quaternion.LookRotation(posicionInicial - Enemigo.position);
         transform.rotation = Quaternion.Slerp(Enemigo.rotation, vuelta, 0.02f);

         Vector3 volver = Enemigo.eulerAngles;

         Enemigo.eulerAngles = new Vector3(0, volver.y, 0);









     }
 }*/