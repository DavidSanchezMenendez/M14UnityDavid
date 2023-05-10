using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lagrima : EnemigoController,IEnemigo
{
    public GameObject muerto;
    private void Update()
    {
        SeguirPlayer();
    }

    private void Awake()
    {
        vida = 1;
        damage = 1;
        poderGolpe = 50;
        distanciaEspera = 0f;
    }
    public void Golpeado()
    {
        vida--;
        vfxEnemigo.Play();
        if (vida<=0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;
                Destroy(gameObject);
            Destroy(muerto,2f);
        }
    }
    

    public float PoderGolpeao()
    {
        return poderGolpe;
    }

    public void Ataques()
    {
        //No ataca
    }
    public int DanoEnemigo()
    {
        return damage;
    }
    public void GolpeEspada()
    {
        Debug.Log("Holaa");
        vida--;
        vfxEnemigo.Play();
        if (vida <= 0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;
         
            Destroy(gameObject);
            Destroy(muerto, 2f);
        }
    }
}
