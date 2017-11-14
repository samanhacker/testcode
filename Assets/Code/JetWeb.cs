using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetWeb : MonoBehaviour {

    
  
   
    public Renderer rend;
    // Use this for initialization
    void Start () {



        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        rend.material.color = Color.red;
       
    }
    void OnMouseOver()
    {
        //rend.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
    }
    void OnMouseExit()
    {
        rend.material.color = Color.white;
    }
    void Update () {
        //var pos = Input.mousePosition;
        //pos.z = 45;
        //pos = Camera.main.ScreenToWorldPoint(pos);
        //transform.position = pos;
    
    }
}
