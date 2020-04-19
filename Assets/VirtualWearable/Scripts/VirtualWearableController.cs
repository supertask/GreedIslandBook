using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Leap;
using Leap.Unity;
using static Leap.Finger;

namespace VW
{
    public class VirtualWearableController : MonoBehaviour
    {
        public GameObject leapProviderObj;
        private LeapServiceProvider m_Provider;

        public GameObject debugSphere1;
        public GameObject debugSphere2;

        private VirtualWearableModel model;
        private PlayableDirector openingVWDirector;

        void Start()
        {
            this.m_Provider = this.leapProviderObj.GetComponent<LeapServiceProvider>();
            this.model = this.GetComponent<VirtualWearableModel>();
            this.openingVWDirector = this.model.entireHandUI.gameObject.GetComponent<PlayableDirector>();
            Debug.Log(this.openingVWDirector);
            //this.model.VisibleVirtualWearable(false);
        }

        void Update()
        {
            Frame frame = this.m_Provider.CurrentFrame;
            Hand[] hands = HandUtil.GetCorrectHands(frame); //0=LEFT, 1=RIGHT

            if (hands[HandUtil.RIGHT] == null) {
                // if (this.IsVisibleVirtualWearable) { this.model.VisibleVirtualWearable(false); }
            }
            else {
                if (!this.model.IsVisibleVirtualWearable) { this.model.VisibleVirtualWearable(true); }
                this.model.AdjustVirtualWearable(hands[HandUtil.RIGHT]);

                if (this.model.handUtilAccess.JustOpenedHandOn(hands, HandUtil.RIGHT)
                        && this.openingVWDirector.state == PlayState.Paused) {
                    this.openingVWDirector.Play();
                }

                //Debug
                //this.debugSphere1.transform.position = HandUtil.ToVector3(hand.Arm.PrevJoint);
                //this.debugSphere2.transform.position = HandUtil.ToVector3(hand.Arm.NextJoint);
            }
            this.model.handUtilAccess.SavePreviousHands(hands);
        }
    }
}
