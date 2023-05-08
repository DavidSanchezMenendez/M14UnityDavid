using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinxoEneigo : EnemigoController, IEnemigo
{
    // Start is called before the first frame update
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

        distanciaEspera = 4f;
        vida = 1;
        damage = 1;
        poderGolpe = 100;

    }
    public void Golpeado()
    {
        vida--;
        vfxEnemigo.Play();
        if (vida <= 0)
        {
            muerto.SetActive(true);

            muerto.transform.parent = null;

            Destroy(this.gameObject);
        }
    }
    public void Ataques()
    {
        int ataque = Random.Range(0, 1);
        switch (ataque)
        {
            case 0:
                animEnemigo.SetTrigger("Ataque");
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