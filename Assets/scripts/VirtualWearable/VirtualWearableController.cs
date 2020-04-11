using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Leap;
using Leap.Unity;
using static Leap.Finger;

public class VirtualWearableController : MonoBehaviour
{
    public GameObject leapProviderObj;
    private LeapServiceProvider m_Provider;

    public Transform playerHeadTransform;
    public float HAND_AJUST__TOWARDS_FINGER = -0.058f;
    public float HAND_AJUST__TOWARDS_THUMB = 0.0045f;
    public GuidReference entireHandUI;
    public GuidReference icons;

    private GameObject arm;
    private GameObject armUI;
    private GameObject palmUI;
    private List<GameObject> iconObjects = new List<GameObject>();
    public GameObject debugSphere1;
    public GameObject debugSphere2;

    private const float ARM_WIDTH_METER_IN_BLENDER = 6.35024f * 0.01f; // = 6.35024cm
    private const float ARM_LENGTH_METER_IN_BLENDER = 25.6461f * 0.01f; // = 25.6461cm
    private HandUtil handUtil;
    private bool isActivedRightHand;

    void Start()
    {
        this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();
        this.handUtil = new HandUtil(playerHeadTransform);
        /*
        Util.EnableMeshRendererRecursively(this.entireHandUI.gameObject, false);
        Util.EnableMeshRendererRecursively(this.icons.gameObject, false);
        */
        this.arm = this.entireHandUI.gameObject.transform.Find("Arm").gameObject;
        this.armUI = this.entireHandUI.gameObject.transform.Find("ArmUI").gameObject;
        this.palmUI = this.entireHandUI.gameObject.transform.Find("PalmUI").gameObject;
        this.isActivedRightHand = false;
    }

    void Update()
    {
        //ブックを開ける
        Frame frame = this.m_Provider.CurrentFrame;
        Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT

        if (hands[HandUtil.RIGHT] == null) {
            /*
            if (this.isActivedRightHand) {
                Util.EnableMeshRendererRecursively(this.entireHandUI.gameObject, false);
                Util.EnableMeshRendererRecursively(this.icons.gameObject, false);
                this.isActivedRightHand = false;
            }
            */
        }
        else {
            if (! this.isActivedRightHand) {
                Util.EnableMeshRendererRecursively(this.entireHandUI.gameObject, true);
                Util.EnableMeshRendererRecursively(this.icons.gameObject, true);
                this.isActivedRightHand = true;
            }

            Hand hand = hands[HandUtil.RIGHT];
            Vector3 palmPosition = HandUtil.ToVector3(hand.PalmPosition);
            Vector3 directionTowardsFinger = HandUtil.ToVector3(hand.Direction);
            Vector3 handNormal = HandUtil.ToVector3(hand.PalmNormal);

            // Hand position & rotation
            //https://docs.unity3d.com/ScriptReference/Vector3.Cross.html
            Vector3 directionTowardsThumb = Vector3.Cross(handNormal, directionTowardsFinger).normalized;
            this.entireHandUI.gameObject.transform.position = palmPosition + directionTowardsFinger * HAND_AJUST__TOWARDS_FINGER;
            this.entireHandUI.gameObject.transform.position += directionTowardsThumb * HAND_AJUST__TOWARDS_THUMB;
            this.entireHandUI.gameObject.transform.rotation = HandUtil.ToQuaternion(hand.Rotation) *
                Quaternion.AngleAxis(180, Vector3.forward) *
                Quaternion.AngleAxis(180, Vector3.up);

            // Arm position, rotation, and scale
            this.arm.transform.rotation = HandUtil.ToQuaternion(hand.Arm.Rotation) * Quaternion.AngleAxis(270, Vector3.left);
            this.armUI.transform.rotation = HandUtil.ToQuaternion(hand.Arm.Rotation) * Quaternion.AngleAxis(270, Vector3.left);
            this.arm.transform.localScale = new Vector3(
                hand.Arm.Width / ARM_WIDTH_METER_IN_BLENDER,
                hand.Arm.Length / ARM_LENGTH_METER_IN_BLENDER,
                1);
            //Debug.Log("width: " + hand.Arm.Width);
            //Debug.Log("length: " + hand.Arm.Length);

            //iconの
            for (int i = 0; i < this.icons.gameObject.transform.childCount; i++) {
                icons.gameObject.transform.GetChild(i).position = palmUI.gameObject.transform.GetChild(i).position;
                icons.gameObject.transform.GetChild(i).rotation =
                    palmUI.gameObject.transform.GetChild(i).rotation * Quaternion.AngleAxis(270, Vector3.left); // * this.entireHandUI.gameObject.transform.rotation;
            }



            //Debug
            this.debugSphere1.transform.position = HandUtil.ToVector3(hand.Arm.PrevJoint);
            this.debugSphere2.transform.position = HandUtil.ToVector3(hand.Arm.NextJoint);
        }
    }

}
