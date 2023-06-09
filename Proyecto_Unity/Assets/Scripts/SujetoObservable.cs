using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SujetoObservable : MonoBehaviour
{
    private int estrella;

    private List<ICambiodeState> observers = new List<ICambiodeState>();

    public State currentState;

    public static SujetoObservable instancia { get; private set; }


    //public static event Action<State> OnStateChange;
    public static void GenerarInstancia()
    {
        if (instancia == null)
        {
            instancia = new SujetoObservable();
        }
        else
        {
            Destroy(instancia);
        }
    }
    
    
    // Start is called before the first frame update
  
    public void Subscribirse(ICambiodeState observer)
    {
        observers.Add(observer);
    }
    public void Unsubscribirse(ICambiodeState observer)
    {
        observers.Remove(observer);
    }
    
 
    public void CojerEstrella()
    {
        estrella++;
    }
    public void CambiarState(State _newState)
    {
        currentState = _newState;
        NotificarATodos();

    }
    public void NotificarATodos()
    {

        foreach (ICambiodeState observer in observers)
        {
            observer.NotficiarCambiodeEstado(currentState);
        }
        
    }
  
}
