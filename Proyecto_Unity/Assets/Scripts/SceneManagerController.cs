using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    // Start is called before the first frame update
   public void EmpezarPartida()
    {
        SceneManager.LoadScene(1);
    }
}
