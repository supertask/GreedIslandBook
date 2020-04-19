using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ブックを掴んで移動させる
public class BookUnderNavController : GrabController
{
    public GameObject cardsObj;

    public override void Move(Vector3 diffPos)
    {
        this.transform.parent.position += diffPos; //ブックの位置を移動
        this.cardsObj.transform.position += diffPos;
    }

    protected override void OnTriggerEnter(Collider other) {
        Debug.Log("Hit on nav bar!");
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other) {
        Debug.Log("Exit on nav bar!");
        base.OnTriggerExit(other);
    }
}
