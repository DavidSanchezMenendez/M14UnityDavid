using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossEnemigo : EnemigoController,IEnemigo
{
    public GameObject muerto;
    private void Update()
    {
        SeguirPlayer();
    }

    private void Awake()
    {
        vida = 3;
        damage = 1;
        poderGolpe = 80;
        distanciaEspera = 6f;
    }
    public void Golpeado()
    {
        vida--;
        vfxEnemigo.Play();
        if (vida <= 0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;
            Destroy(gameObject);
        }
    }


    public float PoderGolpeao()
    {
        return poderGolpe;
    }

    public void Ataques()
    {
        int ataque = Random.Range(0, 3);
        switch (ataque)
        {
            case 0:
                animEnemigo.SetTrigger("Golpear");
                break;
            case 1:
                animEnemigo.SetTrigger("Golpear1");
                break;
            case 2:
                animEnemigo.SetTrigger("Golpear3");
                break;

        }
    }
    public int DanoEnemigo()
    {
        return damage;
    }
    public void GolpeEspada()
    {
        Debug.Log("Holaa");
        vida--;
        if (vida <= 0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
