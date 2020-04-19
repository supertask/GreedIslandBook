using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class HandWingPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> handWingObj;
    public ExposedReference<GameObject> handPalmObj;
    public ExposedReference<GameObject> thumbButtonObj;

    //Timelineでいじるパラメータ
    public float maxWingLeftVerticalClipPercent, minWingLeftVerticalClipPercent;
    public float maxPalmRightVerticalClipPercent, minPalmRightVerticalClipPercent;
    public float maxThumbTopHorizontalClipPercent, minThumbTopHorizontalClipPercent;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        HandWingPlayableBehaviour behaviour = new HandWingPlayableBehaviour();
        behaviour.handWingObj = this.handWingObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.handPalmObj = this.handPalmObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.thumbButtonObj = this.thumbButtonObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.maxWingLeftVerticalClipPercent = this.maxWingLeftVerticalClipPercent;
        behaviour.minWingLeftVerticalClipPercent = this.minWingLeftVerticalClipPercent;
        behaviour.maxPalmRightVerticalClipPercent = this.maxPalmRightVerticalClipPercent;
        behaviour.minPalmRightVerticalClipPercent = this.minPalmRightVerticalClipPercent;
        behaviour.maxThumbTopHorizontalClipPercent = this.maxThumbTopHorizontalClipPercent;
        behaviour.minThumbTopHorizontalClipPercent = this.minThumbTopHorizontalClipPercent;

        return ScriptPlayable<HandWingPlayableBehaviour>.Create(graph, behaviour);
    }
}
