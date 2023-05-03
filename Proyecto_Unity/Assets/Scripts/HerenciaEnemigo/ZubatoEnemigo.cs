using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZubatoEnemigo : EnemigoController, IEnemigo
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
            muerto.SetActive(true);

            muerto.transform.parent = null;

            Destroy(this.gameObject);
        }
    }
    public void Ataques()
    {


        animEnemigo.SetTrigger("Ataque");

    }

    public int DañoEnemigo()
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
