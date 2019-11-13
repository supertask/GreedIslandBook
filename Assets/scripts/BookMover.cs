using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMover : MonoBehaviour
{
    public GameObject bookObj;

    void Start() { }
    void Update() { } 

    //指が本に触れている場合
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit on book!");
        Debug.Log(other.gameObject.name);
        //もし
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit from book!");
    }

    void OnTriggerStay(Collider other) {
    }
}
