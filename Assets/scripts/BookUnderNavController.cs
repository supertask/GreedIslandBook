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
    public CardsModel cardsModel;

    private Vector3 relatedPos;
    private HandUtil handUtil;
    private LeapServiceProvider m_Provider;

    void Start() {
        this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();
        this.handUtil = new HandUtil(playerHeadTransform);
    }
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit on bookbar!");
        Frame frame = this.m_Provider.CurrentFrame;
        Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT
        if (hands[HandUtil.LEFT] != null) {
            Finger indexFinger = hands[HandUtil.LEFT].Fingers[(int)FingerType.TYPE_INDEX];
            Debug.Log(indexFinger.IsExtended);
        }

        Vector3 fingerPos = other.gameObject.transform.position; //TODO(Tasuku): LeapMotionからの人差し指情報にすり替える

        //相対位置 = ブックバーのポジション - 人差し指のポジション
        this.relatedPos = this.transform.position - fingerPos;
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
        Vector3 fingerPos = other.gameObject.transform.position; //TODO(Tasuku): LeapMotionからの人差し指情報にすり替える

        //人差し指のポジション + ブックバーとの相対的位置 -> ブックの位置とする
        this.transform.parent.position = fingerPos + this.relatedPos; //ブックの位置
    }
}
