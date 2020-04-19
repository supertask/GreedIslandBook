using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class HandWingPlayableBehaviour : PlayableBehaviour
{
    public GameObject handWingObj;
    public GameObject handPalmObj;
    public GameObject thumbButtonObj;

    public float maxWingLeftVerticalClipPercent, minWingLeftVerticalClipPercent;
    public float maxPalmRightVerticalClipPercent, minPalmRightVerticalClipPercent;
    public float maxThumbTopHorizontalClipPercent, minThumbTopHorizontalClipPercent;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        this.handWingObj.GetComponent<MeshRenderer>().enabled = true;
        this.handPalmObj.GetComponent<MeshRenderer>().enabled = true;
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0

        float wingPercent = Mathf.Lerp(this.maxWingLeftVerticalClipPercent, this.minWingLeftVerticalClipPercent, progress);
        this.handWingObj.GetComponent<MeshRenderer>().material.SetFloat("_LeftVerticalClipPercent", wingPercent);

        float palmPercent = Mathf.Lerp(this.maxPalmRightVerticalClipPercent, this.minPalmRightVerticalClipPercent, progress);
        this.handPalmObj.GetComponent<MeshRenderer>().material.SetFloat("_RightVerticalClipPercent", palmPercent);

        float thumbPercent = Mathf.Lerp(this.maxThumbTopHorizontalClipPercent, this.minThumbTopHorizontalClipPercent, progress);
        this.thumbButtonObj.GetComponent<MeshRenderer>().material.SetFloat("_TopHorizontalClipPercent", thumbPercent);
    }
}
