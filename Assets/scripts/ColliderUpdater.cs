using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUpdater : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;
    private BoxCollider cL, cR;

    // Start is called before the first frame update
    void Start() {
        this.cL = leftHand.GetComponent<BoxCollider>();
        this.cR = rightHand.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //this.cL.center 
    }
}
