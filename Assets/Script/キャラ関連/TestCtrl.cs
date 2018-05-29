using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCtrl : MonoBehaviour {

    private Rigidbody rig;

    float ii=0;
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 aaa = new Vector3(0 , Input.GetAxis("Horizontal") , 0); 
        ii += Input.GetAxis("Horizontal");
        rig.MoveRotation(Quaternion.AngleAxis(ii * Time.deltaTime, Vector3.up)); ;
	}
}
