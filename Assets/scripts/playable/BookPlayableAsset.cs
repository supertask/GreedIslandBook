using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BookPlayableAsset : PlayableAsset
{
    public ExposedReference<GameObject> bookBodyObj;

    //Timelineでいじるパラメータ

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        //behaviourに対してパラメータを入れ込んでいく
        BookPlayableBehaviour behaviour = new BookPlayableBehaviour();
        behaviour.bookBodyObj = this.bookBodyObj.Resolve(graph.GetResolver()); //ExposedReferenceからとる時のおまじない

        return ScriptPlayable<BookPlayableBehaviour>.Create(graph, behaviour);
    }
}
