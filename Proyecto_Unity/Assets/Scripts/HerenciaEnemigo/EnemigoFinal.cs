using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoFinal : EnemigoController, IEnemigo
{
    public GameObject muerto;

    public void Update()
    {
        SeguirPlayer();

    }
    public float PoderGolpeao()
    {
        return poderGolpe;
    }
    private void Awake()//como un constructor
    {

        distanciaEspera = 6f;
        vida = 1;
        damage = 2;
        poderGolpe = 50;

    }
    public void Golpeado()
    {
        vida--;
        if (vida <= 0)
        {
            //muerto.SetActive(true);

            //muerto.transform.parent = null;

            Destroy(this.gameObject);
        }
    }
    public void Ataques()
    {


        int ataque = Random.Range(0, 4);
        switch (ataque)
        {
            case 0:
                animEnemigo.SetTrigger("Ataque1");
                break;
            case 1:
                animEnemigo.SetTrigger("Ataque2");
                break;
            case 2:
                animEnemigo.SetTrigger("Ataque3");
                break;
            case 3:
                animEnemigo.SetTrigger("Ataque4");
                break;

        }

    }

    public int DanoEnemigo()
    {
        return damage;
    }
    public void GolpeEspada()
    {
        vida--;
        if (vida <= 0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
