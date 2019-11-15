using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{

    private int _index = 0;
    public int Index { get {
            return _index;
        } set {
      
            _index = value;
            this.name =  "Item_" + _index;
        } }

    private int _element = 0;
    public int Element {

        get
        {
            return _element;
        }
        set
        {

            _element = value;

            var renderer = this.GetComponent<Renderer>();
            if(renderer != null){ 
            //Call SetColor using the shader property name "_Color" and setting the color to red
            renderer.material.SetColor("_Color", Elements.ElementsArray[_element % Elements.ElementsArray.Length]);
            }
        }
    }

    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void OnMouseDown()
    {
       // Debug.Log("OnMouseDown ");
        // load a new scene
    
        originalPosition = this.gameObject.transform.position;
        GameController.selectedItemflag = true;
        GameController.selectedItem = this.gameObject;
      
    }


    private void OnGUI()
    {
        var renderer = this.GetComponent<Renderer>();
        if (renderer == null)
        {
            //Call SetColor using the shader property name "_Color" and setting the color to red
           
        }
    }

}
