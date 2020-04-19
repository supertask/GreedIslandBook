using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CardPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> cardsModelObj;

    //Timelineでいじるパラメータ
    public float maxScaleY;
    public bool isScalingUp;
    public bool isReplacingCard;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        CardPlayableBehaviour behaviour = new CardPlayableBehaviour();
        behaviour.cardsModelObj = this.cardsModelObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない
        behaviour.maxScaleY = this.maxScaleY;
        behaviour.isScalingUp = this.isScalingUp;
        behaviour.isReplacingCard = this.isReplacingCard;

        return ScriptPlayable<CardPlayableBehaviour>.Create(graph, behaviour);
    }
}
