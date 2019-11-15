using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject pivot;

    private Vector3 pivotPosition;

    public Vector3 Size;

    private Vector3 HalfSize;

    public int matrixrows = 0;

    private int scalerows = 2;

    public int matrixcolumns = 0;

    private GameObject[][] matrix;



    public static GameObject selectedItem;

    private static GameObject secondselectedItem;

    public static bool selectedItemflag;




    // Start is called before the first frame update

    float cooldown = 0.250f;
    float lastime = 0;
    void Start()
    {
        if (this.pivot == null)
            throw new UnityException("Pivote no definido");

        if (this.Size == Vector3.zero)
            throw new UnityException("Tamaño de tile no definido");

        if (matrixrows <= 0 || matrixcolumns <= 0)
        {
            throw new System.Exception("Matriz de tiles no tiene valores validos");
        }

        this.pivotPosition = new Vector3(this.pivot.transform.position.x, this.pivot.transform.position.y, this.pivot.transform.position.z);

        this.HalfSize = this.Size * 0.5f;

        Random r = new Random();
        matrix = new GameObject[matrixrows * scalerows][];

        int counter = 0;
        for (int i = 0; i < matrix.Length; i++)
        {

            matrix[i] = new GameObject[matrixcolumns];


            for (int columns = 0; columns < matrix[i].Length; columns++, counter++)
            {

                var position = this.pivotPosition;

                position.x = pivot.transform.position.x * Vector3.right.x;

                position += Size.x * (counter % matrixcolumns) * Vector3.right;

                //if (counter > 0 && counter % matrixcolumns == 0)
                {
                    //dependiendo de la orientacion 
                    position += Vector3.up * Size.y * ((int)(counter / matrixcolumns));
                }

                var go = Instantiate(pivot, position, Quaternion.identity);


                var item = go.GetComponent<ItemBehaviour>();

                item.Index = counter;

                //TODO remover solo se prueba la asginacion de elementos
                item.Element = Random.Range(0, 100) % Elements.ElementsArray.Length;


                matrix[i][columns] = go;

                go.GetComponent<ItemBehaviour>().enabled = true;

                go.SetActive(true);


            }

        }

        this.pivot.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(LayerMask.NameToLayer("Items") + " layer");
        Vector3 pos = Camera.main.WorldToScreenPoint(Input.mousePosition);


        if (selectedItemflag)
        {

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                //Debug.Log("Input.GetMouseButtonUp ");
                selectedItemflag = false;
                selectedItem = null;
                secondselectedItem = null;
                return;
            }

            // Debug.DrawRay(selectedItem.transform.position,  (selectedItem.transform.right * 5), Color.red);
            // Debug.DrawRay(selectedItem.transform.position,  (selectedItem.transform.up * 5), Color.yellow);

            if (Time.time > lastime)
            {


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 9 << 8;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {

                    if (hit.collider.gameObject == selectedItem)
                    {

                        //Debug.Log("same selected item");
                        return;
                    }


                    Debug.DrawRay(hit.point, Camera.main.transform.position, Color.cyan);

                    Vector3 result = hit.point - selectedItem.transform.position;


                    var itemselected = selectedItem.GetComponent<ItemBehaviour>();

                    var selectedindex = itemselected.Index;

                    ItemBehaviour itemtoswap = null;
                    int row = 0;
                    int col = 0;
                    //if (Mathf.Abs(result.y) > 2 || Mathf.Abs(result.x)> 2) {
                    //    return;
                    //}
                    //Debug.Log("Raycast "+difx+" "+ Mathf.Sign(difx)+" "+dify+" "+ Mathf.Sign(dify));
                    int newindex = selectedindex + matrixcolumns;

                    Vector3 direction = Vector3.zero;

                    if (Mathf.Abs(result.y) < Mathf.Abs(result.x))
                    {
                        // set selected null
                        // movement animation
                        // find next right or left 

                        Debug.DrawRay(selectedItem.transform.position, (selectedItem.transform.right * Mathf.Sign(result.x) * Mathf.Abs(result.x)), Color.green);
                        // a la derecha sumar uno
                        // a la izquerda sumar dos
                        if (selectedItem.transform.position.x < hit.collider.transform.position.x)
                        {
                            newindex = selectedindex + 1;
                            direction = Vector3.right;
                        }
                        else
                        {
                            newindex = selectedindex - 1;
                            direction = Vector3.left;
                        }
                    }
                    else
                    {

                        // find next up or  down 
                        // si es arriba sumar el numero de columnas
                        // es abajo restar

                        if (selectedItem.transform.position.y < hit.collider.transform.position.y) { 
                            newindex = selectedindex + matrixcolumns;
                            direction = Vector3.up;
                        }
                        else { 
                            newindex = selectedindex - matrixcolumns;
                            direction = Vector3.down;
                        }
                        Debug.DrawRay(selectedItem.transform.position, (selectedItem.transform.up * Mathf.Sign(result.y) * Mathf.Abs(result.y)), Color.green);
                    }

                    row = newindex / matrixcolumns;
                    col = newindex % matrixcolumns;
                    secondselectedItem= this.matrix[row][col];
                    itemtoswap = secondselectedItem.GetComponent<ItemBehaviour>();

                    if (itemtoswap != null)
                    {
                        Debug.Log("Index " + selectedindex + ", [" + row + "," + col + "] ,Newindex " + newindex + ", Title to swap " + itemtoswap.Index);

                        //validar matriz logica

                        //si hay match mover y quedar quiero
                        itemtoswap.Index = selectedindex;

                        row = itemtoswap.Index / matrixcolumns;
                        col = itemtoswap.Index % matrixcolumns;
                         this.matrix[row][col] = secondselectedItem;

                        itemselected.Index = newindex;
                        row = itemselected.Index / matrixcolumns;
                        col = itemselected.Index % matrixcolumns;
                        this.matrix[row][col] = selectedItem;


                        var b1 = selectedItem.GetComponent<MovementBehaviour>();

                        var b2 = secondselectedItem.GetComponent<MovementBehaviour>();
                      
                        b1.Positiontogo = new Vector3( secondselectedItem.transform.position.x,
                            secondselectedItem.transform.position.y,
                            secondselectedItem.transform.position.z);


                      

                        b2.Positiontogo = new Vector3(selectedItem.transform.position.x,
                            selectedItem.transform.position.y,
                            selectedItem.transform.position.z);


                        b1.direction = direction;

                        b2.direction = -direction;

                        b1.enabled = true;
                        b2.enabled = true;

                        //si no entonces devolver

                        //todo remover es provisional
                        selectedItemflag = false;
                        selectedItem = null;
                        secondselectedItem = null;
                        return;

                    }
                    else {
                        Debug.Log("itemtoswap is null!!!");
                    }

                }


                lastime = Time.time + cooldown;
                //Debug.Log(" lastime " + lastime);
            }

        }
    }




}
