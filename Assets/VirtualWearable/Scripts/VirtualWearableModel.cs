using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Leap;
using Leap.Unity;
using static Leap.Finger;

namespace VW
{
    public class VirtualWearableModel: MonoBehaviour
    {
        public GuidReference entireHandUI;
        public GuidReference icons;

        private GameObject arm;
        private GameObject armUI;
        private GameObject palmUI;

        public float HAND_AJUST__TOWARDS_FINGER = -0.058f;
        public float HAND_AJUST__TOWARDS_THUMB = 0.0045f;
        public const float ARM_WIDTH_METER_IN_BLENDER = 6.35024f * 0.01f; // = 6.35024cm
        public const float ARM_LENGTH_METER_IN_BLENDER = 25.6461f * 0.01f; // = 25.6461cm

        private HandUtil handUtil;
        public HandUtil handUtilAccess {
            get { return handUtil; }
        }
        public Transform playerHeadTransform;

        private bool isVisibleVirtualWearable;
        public bool IsVisibleVirtualWearable { get { return isVisibleVirtualWearable; } }

        public void Start() {
            this.arm = this.entireHandUI.gameObject.transform.Find("Arm").gameObject;
            this.armUI = this.entireHandUI.gameObject.transform.Find("ArmUI").gameObject;
            this.palmUI = this.entireHandUI.gameObject.transform.Find("PalmUI").gameObject;
            this.handUtil = new HandUtil(playerHeadTransform);
            this.isVisibleVirtualWearable = false;
        }

        public void AdjustVirtualWearable(Hand hand)
        {
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
                for (int i = 0; i < this.icons.gameObject.transform.childCount; i++)
                {
                    icons.gameObject.transform.GetChild(i).position = this.palmUI.gameObject.transform.GetChild(i).position;
                    icons.gameObject.transform.GetChild(i).rotation =
                        this.palmUI.gameObject.transform.GetChild(i).rotation * Quaternion.AngleAxis(270, Vector3.left); // * this.entireHandUI.gameObject.transform.rotation;
                }
        }

        public void VisibleVirtualWearable(bool isVisible) {
            Util.EnableMeshRendererRecursively(this.entireHandUI.gameObject, isVisible);
            Util.EnableMeshRendererRecursively(this.icons.gameObject, isVisible);
            this.isVisibleVirtualWearable = isVisible;
        }

    }
}
