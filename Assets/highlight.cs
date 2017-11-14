using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class highlight : MonoBehaviour {
    public int row = 1;
	private bool _clickstate=false;
    public List<int> DiceNeed = new List<int>();
    // Use this for initialization
    void Start () {
		
	}


	void OnMouseDown()
	{
		if (!_clickstate) {
			_clickstate = true;
			Debug.Log ("Selected " + row.ToString ());
			foreach (var item in DiceNeed) {
				Debug.Log (item);
			}
			if (Socket.SOC.GameBG.Selected_Row != 0)
				Socket.SOC.GameBG.moveBead (Socket.SOC.GameBG.Selected_Row, DiceNeed, true);

			Socket.SOC.GameBG.Selected_Row = 0;
		}
	}
	void onMouseUp()
	{
		_clickstate = false;
		
		Debug.Log ("uped " + row.ToString ());
	}

    

    // Update is called once per frame
    void Update () {
        transform.position = Socket.SOC.GameBG.RowPos_highlight(row);
    }
}
