using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace VW
{
    // A behaviour that is attached to a playable
    public class AppIconsPlayableBehaviour : PlayableBehaviour
    {
        public GameObject vw;
        //public GameObject icons;

        //public float maxWingLeftVerticalClipPercent, minWingLeftVerticalClipPercent;
        public int appIndex;

        public bool isScalingUp;
        public bool applyAllIcons;

        private List<GameObject> handWingUIs;
        private List<List<Quaternion>> localRotationsOfAppIcons;

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable) {
            if (isScalingUp) {
                foreach (GameObject handWingUI in this.handWingUIs) { Util.EnableMeshRendererRecursively(handWingUI, true); }
            }
        }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable) {
            if (! isScalingUp) {
                foreach (GameObject handWingUI in this.handWingUIs) { Util.EnableMeshRendererRecursively(handWingUI, false); }
            }
        }

        public override void OnPlayableCreate(Playable playable)
        {
            this.initHandWingUIs();
            /*
            if (isScalingUp) {
                Util.EnableMeshRendererRecursively(this.firstAppIconsObj, false);
                Util.EnableMeshRendererRecursively(this.secondAppIconsObj, false);
            }*/
        }

        private void initHandWingUIs()
        {
            this.handWingUIs = new List<GameObject>();
            this.handWingUIs.Add(this.vw.transform.Find("FirstHandWingUI").gameObject);
            this.handWingUIs.Add(this.vw.transform.Find("SecondHandWingUI").gameObject);
            this.localRotationsOfAppIcons = new List<List<Quaternion>>();

            for (int i = 0; i < this.handWingUIs.Count; i++)
            {
                this.PopupIcons(this.handWingUIs[i], 0.0f);
                this.localRotationsOfAppIcons.Add(new List<Quaternion>());
                foreach (Transform child in this.handWingUIs[i].transform) {
                    if (child.childCount > 0) {
                        localRotationsOfAppIcons[i].Add(child.GetChild(0).rotation);
                    }
                }
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info) {
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info) {
            //Util.EnableMeshRendererRecursively(this.firstAppIconsObj, false);
            //Util.EnableMeshRendererRecursively(this.secondAppIconsObj, false);
        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
            foreach (GameObject handWingUI in this.handWingUIs) {
                this.PopupIcons(handWingUI, progress);
            }
        }

        private void PopupIcons(GameObject appIcons, float progress)
        {
            Vector3 beginScale, endScale;
            float beginPosY, endPosY, beginRotAngle, endRotAngle;
            if (isScalingUp) {
                beginScale = Vector3.zero;
                beginPosY = 0.0f;
                beginRotAngle = Constants.App.LocalRotationBeginAngle;
                endScale = new Vector3(Constants.App.Size, Constants.App.Size, Constants.App.Size);
                endPosY = Constants.App.LocalPositionY;
                endRotAngle = Constants.App.LocalRotationEndAngle;
            } else {
                beginScale = new Vector3(Constants.App.Size, Constants.App.Size, Constants.App.Size);
                beginPosY = Constants.App.LocalPositionY;
                beginRotAngle = Constants.App.LocalRotationEndAngle;
                endScale = Vector3.zero;
                endPosY = 0.0f;
                endRotAngle = Constants.App.LocalRotationBeginAngle;
            }
            Vector3 scale = Vector3.Lerp(beginScale, endScale, progress);
            float rotAngle = Mathf.Lerp(beginRotAngle, endRotAngle, progress);
            float posY = Mathf.Lerp(beginPosY, endPosY, progress);
            //Debug.Log(posY);

            if (this.applyAllIcons) {
                foreach (Transform child in appIcons.transform) {
                    if (child.childCount > 0) {
                        Transform grandChild = child.GetChild(0);
                        grandChild.localScale = scale;
                        grandChild.localPosition = Vector3.forward * posY;
                        grandChild.localRotation = Quaternion.Lerp * ();
                        //grandChild.localRotation = child.localRotation * Quaternion.AngleAxis(270, Vector3.left) *
                        //    Quaternion.AngleAxis( Mathf.Lerp(Constants.App.LocalRotationEndAngle, 0, progress), Vector3.up);
                    }
                }
            } else {
                Transform child = appIcons.transform.GetChild(this.appIndex);
                if (child.childCount > 0) {
                    Transform grandChild = child.GetChild(0);
                    //Debug.Log(child.localPosition.y);

                    grandChild.localScale = scale;
                    grandChild.localPosition = Vector3.forward * posY;

                    //grandChild.localRotation = child.localRotation * Quaternion.AngleAxis(270, Vector3.left) *
                    //   Quaternion.AngleAxis( Mathf.Lerp(0, Constants.App.LocalRotationEndAngle, progress), Vector3.up);
                }
            }
        }

    }
}
