using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBox : MonoBehaviour {
    Rigidbody rb;
    public float speed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
        rb.velocity = new Vector3(0,-speed,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
