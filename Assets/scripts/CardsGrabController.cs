using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ブックを掴んで移動させる
public class CardsGrabController : GrabController
{
    private Vector3 cardPos;

    public override void Move(Vector3 diffPos)
    {
        this.transform.position += diffPos; //ブックの位置を移動
    }

    protected override void OnTriggerEnter(Collider other) {
        Debug.Log("Hit on card!");
        base.OnTriggerEnter(other);
        this.cardPos = this.transform.position;
    }

    protected override void OnTriggerExit(Collider other) {
        Debug.Log("Exit on card!");
        base.OnTriggerExit(other);
        this.transform.position = this.cardPos ;
    }
}
