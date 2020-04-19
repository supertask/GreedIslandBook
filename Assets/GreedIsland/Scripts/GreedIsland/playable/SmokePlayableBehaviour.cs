using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SmokePlayableBehaviour : PlayableBehaviour
{
    public GameObject smokeObj; //子オブジェクトにカードを複数持つ

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log("Smoke start !!!!!!, " + this.smokeObj.name);
        this.smokeObj.GetComponent<SpriteRenderer>().enabled = true;
        this.smokeObj.GetComponent<Animator>().enabled = true;
        Debug.Log("1 !!!!!!, " + this.smokeObj.GetComponent<SpriteRenderer>().enabled);
        Debug.Log("2 !!!!!!, " + this.smokeObj.GetComponent<Animator>().enabled);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Debug.Log("Smoke end !!!!!!");
        this.smokeObj.GetComponent<Animator>().enabled = false;
        this.smokeObj.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("1 !!!!!!, " + this.smokeObj.GetComponent<SpriteRenderer>().enabled);
        Debug.Log("2 !!!!!!, " + this.smokeObj.GetComponent<Animator>().enabled);
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
    }

}
