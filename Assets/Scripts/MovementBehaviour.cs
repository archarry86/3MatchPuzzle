using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Vector3 defpos = new Vector3(0, 0, -1);



    public float velocity = 0.01f;

    private Vector3 positiontogo = Vector3.zero;


    internal Vector3 direction;
  
    public Vector3 Positiontogo
    {

        get
        {

            return positiontogo;
        }
        set
        {

            positiontogo = value;

          
        }
    }



    private int Movements = 0;

    private int MaxMovements = 9;

    float coldown = 0.01f;

    float next = 0;
    // Start is called before the first frame update
    void Awake()
    {
        positiontogo = defpos;
    }

    // Update is called once per frame
    void Update()
    {
        //if (positiontogo != defpos)
        {

            if (Movements < MaxMovements)
            {
           
                if (Time.time >= next)
                { 
                    var pos = this.transform.position;
                    this.transform.position = (pos + ((direction)*2 * Time.deltaTime));
           
                    Movements++;
           
                    next = Time.time + coldown;
                }
            }
            else
            {
             
                this.transform.position = positiontogo;
                Movements = 0;
                this.enabled = false;
                positiontogo = defpos;
            }

        }
    }
}
