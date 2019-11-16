using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Leap;
using Leap.Unity;
using static Leap.Finger;

public class BookController : MonoBehaviour
{
    public GameObject leapProviderObj;
    private LeapServiceProvider m_Provider;

    public GameObject cardModelObj;
    private CardsModel cardModel;
    private PlayableDirector cardSlideDirector;

    public GameObject bookBodyObj;
    public GameObject bookUnderNavObj;
    public PlayableDirector openingBookDirector;
    public bool isDebug;
    public Transform playerHeadTransform;

    public GameObject leftHandObj;
    public GameObject rightHandObj;

    private HandUtil handUtil;
    private bool isOpenedbook;
    private int activeHand;

    void Start()
    {
        this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();

        if (isDebug) {
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", 0.02f);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_HorizontalClipPercent", 0.02f);
            this.bookUnderNavObj.GetComponent<MeshRenderer>().material.SetFloat("_ClipPercent", 0.1f);
        }
        else {
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", 0.48f);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_HorizontalClipPercent", 0.18f);
            this.bookUnderNavObj.GetComponent<MeshRenderer>().material.SetFloat("_ClipPercent", 0.5f);
            this.bookBodyObj.GetComponent<MeshRenderer>().enabled = false;
            this.bookUnderNavObj.GetComponent<MeshRenderer>().enabled = false;
        }

        this.cardModel = this.cardModelObj.GetComponent<CardsModel>();
        this.cardSlideDirector = this.GetComponent<PlayableDirector>();
        this.isOpenedbook = false;

        this.handUtil = new HandUtil(playerHeadTransform);
        this.activeHand = -1;
    }

    void Update()
    {
        //ブックと唱える
        if (Input.GetKeyDown(KeyCode.G)) {
            if (isOpenedbook) {
                //ブックを閉じる（未実装）
            }
            else {
                //ブックを開ける
                Frame frame = this.m_Provider.CurrentFrame;
                Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT
                Finger activeIndexFinger = null;

                Vector3 activeIndexFingerPos;
                if (hands[HandUtil.LEFT] != null) {
                    Finger indexFinger = hands[HandUtil.LEFT].Fingers[(int)FingerType.TYPE_INDEX];
                    if (indexFinger.IsExtended) {
                        activeIndexFinger = indexFinger;
                    }
                }
                else if (hands[HandUtil.RIGHT] != null) {
                    Finger indexFinger = hands[HandUtil.RIGHT].Fingers[(int)FingerType.TYPE_INDEX];
                    if (indexFinger.IsExtended) {
                        activeIndexFinger = indexFinger;
                    }
                }

                if (activeIndexFinger != null && this.openingBookDirector.state == PlayState.Paused) {
                    //ブックのコリダーのワールド座標を
                    Vector3 relatedPos = this.transform.TransformPoint(this.GetComponent<BoxCollider>().center);
                    Debug.Log("relatedPos: ");
                    Debug.Log(relatedPos);
                    this.transform.parent.position = HandUtil.ToVector3(activeIndexFinger.TipPosition) - relatedPos;
                    this.cardModel.moveCardsToBook();

                    this.bookBodyObj.GetComponent<MeshRenderer>().enabled = true;
                    this.bookUnderNavObj.GetComponent<MeshRenderer>().enabled = true;
                    this.openingBookDirector.Play();
                    this.isOpenedbook = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit on book!");
        //Debug.Log(other.gameObject.name + ", " + leftHandObj.name);
        //Debug.Log(other.gameObject.name + ", " + rightHandObj.name);
        if (other.gameObject == leftHandObj) {
            this.activeHand = HandUtil.LEFT;
        }
        else if (other.gameObject == rightHandObj) {
            this.activeHand = HandUtil.RIGHT;
        }
        else {
            this.activeHand = -1;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit from book!");
    }

    //
    // 指が本に触れている状態
    //
    void OnTriggerStay(Collider other)
    {
        /*
        Frame frame = this.m_Provider.CurrentFrame;
        Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT
        if (hands[activeHand] != null) {
            if (this.handUtil.IsMoveLeft(hands[activeHand]) ) {
                //ユーザが手を右にスライドさせたとき
                if (this.cardSlideDirector.state == PlayState.Paused) {
                    this.cardModel.nextPage();
                    this.cardSlideDirector.Play();
                }
            }
            else if (this.handUtil.IsMoveRight(hands[activeHand]) ) {
                //ユーザが手を左にスライドさせたとき
                if (this.cardSlideDirector.state == PlayState.Paused) {
                    this.cardModel.previousPage();
                    this.cardSlideDirector.Play();
                }
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.N)) {
            //ユーザが手を右にスライドさせたとき
            if (this.cardSlideDirector.state == PlayState.Paused) {
                this.cardModel.nextPage();
                this.cardSlideDirector.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B)) {
            //ユーザが手を左にスライドさせたとき
            if (this.cardSlideDirector.state == PlayState.Paused) {
                this.cardModel.previousPage();
                this.cardSlideDirector.Play();
            }
        }
    }
}
