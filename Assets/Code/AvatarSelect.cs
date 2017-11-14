using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelect : MonoBehaviour {
    SpriteRenderer sr;
    public Sprite[] pack;
    public int MyID = 0;
    // Use this for initialization
    void Start () {
        //  pack = Resources.LoadAll<Sprite>("Avatar");
  

    }

    // Update is called once per frame
    void Update () {
      //  if(pack.Length>0)
       // gameObject.GetComponent<SpriteRenderer>().sprite = pack[1];
    }
    public void next()
    {
        if (MyID < pack.Length) MyID++;

    }
}
