using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    // Start is called before the first frame update
   private void Die()
    {
        
    }
    private void Start()
    {
        OnPlayerDeath();
    }
}
