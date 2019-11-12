using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class CardPlayableBehaviour : PlayableBehaviour
{
    public GameObject cardsModelObj; //子オブジェクトにカードを複数持つ
    public CardsModel cardsModelScript; //子オブジェクトにカードを複数持つ
    public float maxScaleY;
    public bool isReplacingCard;
    public bool isScalingUp;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
        this.cardsModelScript = this.cardsModelObj.GetComponent<CardsModel>();
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable) {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (this.isReplacingCard) {
            //カードのYスケールが0になったら，カードを置換する
            this.cardsModelScript.replaceCards();
            return;
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (isReplacingCard) { return; }

        float progress = Mathf.Clamp01((float)(playable.GetTime() / playable.GetDuration())); //0.0 - 1.0
        if (this.isScalingUp) {
            float nextY = Mathf.Lerp(0.0f, this.maxScaleY, progress);
            this.scaleCardY(nextY);
        }
        else {
            float nextY = Mathf.Lerp(this.maxScaleY, 0.0f, progress);
            this.scaleCardY(nextY);
        }
    }

    private void scaleCardY(float nextY)
    {
        foreach (Transform child in this.cardsModelObj.transform) {
            Vector3 s = child.localScale;
            s.y = nextY;
            child.localScale = s;
        }
    }
}
