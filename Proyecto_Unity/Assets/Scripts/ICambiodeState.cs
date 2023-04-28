using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    MovimientosPrincipales,
    DobleSalto,
    Planear,

}
public interface ICambiodeState //Una interficie junto al patron observer para que si tenemos esta clase heredada notifique los cambios de State
{
    // Start is called before the first frame update
  

    void NotficiarCambiodeEstado(State state);
   
}
