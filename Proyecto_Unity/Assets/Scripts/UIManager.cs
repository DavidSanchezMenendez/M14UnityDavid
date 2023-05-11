using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIHabilidades;
    private int contador = 0;
    CameraMove cam;
    // Start is called before the first frame update
    private void Awake()
    {
       cam = GameObject.Find("CameraPrincipal").GetComponent<CameraMove>();
        Cursor.visible = false;
        UIHabilidades.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            MenuHablididades();
            contador++;
            
        }
    }


    public void DobleSalto()
    {
        
        SujetoObservable.instancia.CambiarState(State.DobleSalto);
        MenuHablididades();
        contador++;
        PlayerMove.playerInstance.alaDelta.SetActive(false);
        PlayerMove.playerInstance.animPlayer.SetBool("Planeando", false);

    }
    public void Planear()
    {
        SujetoObservable.instancia.CambiarState(State.Planear);
        MenuHablididades();
        contador++;
        PlayerMove.playerInstance.CharacterVelocityY = 0;
    }
    public void MenuHablididades()
    {
        if (contador%2 ==0)
        {
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;//pause Game
            Debug.Log("Juego Pausado");
            UIHabilidades.SetActive(true);
            cam.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 1;//pause Game
            Debug.Log("ResumeGame");
            UIHabilidades.SetActive(false);
            cam.enabled = true;
        }
      
        
    }
}
