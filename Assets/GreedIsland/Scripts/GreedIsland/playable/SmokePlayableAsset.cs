using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SmokePlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> smokeObj;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        SmokePlayableBehaviour behaviour = new SmokePlayableBehaviour();
        behaviour.smokeObj = this.smokeObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        return ScriptPlayable<SmokePlayableBehaviour>.Create(graph, behaviour);
    }
}
