using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoObervador : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Player.OnPlayerDeath += Die;
    }
    void Die()
    {
        Debug.Log("ha muerto");
    }
}
