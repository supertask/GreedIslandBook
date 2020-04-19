using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GainPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> gainObj;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        GainPlayableBehaviour behaviour = new GainPlayableBehaviour();
        behaviour.gainObj = this.gainObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        return ScriptPlayable<GainPlayableBehaviour>.Create(graph, behaviour);
    }
}
