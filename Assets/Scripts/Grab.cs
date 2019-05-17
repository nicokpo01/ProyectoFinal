using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    public Transform PosSlot;
    public float GrabRange;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C))
        {
            RaycastHit2D hitInfoArriba = Physics2D.Raycast(PosSlot.position, PosSlot.right, GrabRange);
            RaycastHit2D hitInfoMedio = Physics2D.Raycast(PosSlot.position, PosSlot.right, GrabRange);
            RaycastHit2D hitInfoAbajo = Physics2D.Raycast(PosSlot.position, PosSlot.right, GrabRange);
            if (hitInfoMedio)
            {
                Debug.Log(hitInfoMedio.transform.name);
                Weapon weapon = hitInfoMedio.transform.GetComponent<Weapon>();
                if (weapon != null)
                {
                    Debug.Log("Naisu");
                }
            }
            else
            {
                if (hitInfoArriba)
                {

                }
                else if (hitInfoAbajo)
                {

                }

            }
        }
    }
}
