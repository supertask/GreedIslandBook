using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using static Leap.Finger;

//ブックを掴んで移動させる
public class GrabController : MonoBehaviour
{
    public Transform playerHeadTransform;
    public GameObject leapProviderObj;
    public List<GameObject> fingerTips;

    private HandUtil handUtil;
    private LeapServiceProvider m_Provider;
    private int activeHand;
    private Vector3 previousFingerPos;
    private bool isFingersInCollider;

    void Start() {
        this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();
        this.handUtil = new HandUtil(playerHeadTransform);
        this.activeHand = -1;
        this.previousFingerPos = Vector3.zero;
        this.isFingersInCollider = false;
    }
    void Update() { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        this.isFingersInCollider = false;
        foreach (GameObject tip in fingerTips) {
            if (tip == other.gameObject) { this.isFingersInCollider = true; }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        this.isFingersInCollider = false;
        this.previousFingerPos = Vector3.zero;
    }

    //
    // 指がブックバーに触れている状態
    //
    protected virtual void OnTriggerStay(Collider other)
    {
        if (! this.isFingersInCollider) { return; }

        Frame frame = this.m_Provider.CurrentFrame;
        Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT
        if (hands[HandUtil.LEFT] != null) {
            this.activeHand = HandUtil.LEFT;
        } else if (hands[HandUtil.RIGHT] != null) {
            this.activeHand = HandUtil.RIGHT;
        } else {
            this.activeHand = -1;
        }

        if (this.activeHand >=0 && hands[this.activeHand] != null)
        {
            Hand handInNav = hands[this.activeHand];
            if (handInNav.GrabStrength > 0.2f) { //つまむ程度の強さ
                Finger indexFinger = handInNav.Fingers[(int)FingerType.TYPE_INDEX];
                Vector3 fingerPos = HandUtil.ToVector3(indexFinger.TipPosition);
                if (this.previousFingerPos != Vector3.zero) {
                    Vector3 diffPos = fingerPos - this.previousFingerPos;
                    this.Move(diffPos);
                }
                this.previousFingerPos = fingerPos;
            }
        }
    }

    //This needs extendsion
    public virtual void Move(Vector3 diffPos) { }
}
