using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Leap;
using Leap.Unity;
using static Leap.Finger;

//ブックを掴んで移動させる
public class BookUnderNavController : MonoBehaviour
{
    public Transform playerHeadTransform;
    public GameObject leapProviderObj;

    public GameObject thumbFingerL;
    public GameObject thumbFingerR;

    //private Vector3 relatedPos;
    private HandUtil handUtil;
    private LeapServiceProvider m_Provider;
    private int handInNav;
    private Vector3 previousFingerPos;

    void Start() {
        this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();
        this.handUtil = new HandUtil(playerHeadTransform);
        this.handInNav = -1;
        this.previousFingerPos = Vector3.zero;
    }
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit on bookbar!");
        if (thumbFingerL == other.gameObject) {
            this.handInNav = HandUtil.LEFT;
        } else if (thumbFingerR == other.gameObject) {
            this.handInNav = HandUtil.RIGHT;
        } else {
            this.handInNav = -1;
        }

        //Vector3 fingerPos = other.gameObject.transform.position; //TODO(Tasuku): LeapMotionからの人差し指情報にすり替える

        //相対位置 = ブックバーのポジション - 人差し指のポジション
        //this.relatedPos = this.transform.position - fingerPos;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit from bookbar!");
    }

    //
    // 指がブックバーに触れている状態
    //
    void OnTriggerStay(Collider other)
    {

        Frame frame = this.m_Provider.CurrentFrame;
        Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT
        if (this.handInNav >= 0 && hands[this.handInNav] != null) {
            Finger indexFinger = hands[this.handInNav].Fingers[(int)FingerType.TYPE_INDEX];
            Finger thumbFinger = hands[this.handInNav].Fingers[(int)FingerType.TYPE_THUMB];
            if( (!indexFinger.IsExtended) && (!thumbFinger.IsExtended) ) {
                Debug.Log("Grabed");
                Debug.Log(previousFingerPos);
                Vector3 fingerPos = HandUtil.ToVector3(indexFinger.TipPosition);
                if (this.previousFingerPos != Vector3.zero) {
                    this.transform.parent.position += fingerPos - this.previousFingerPos; //ブックの位置を移動
                }

                this.previousFingerPos = fingerPos;
            }
        }
    }
}
