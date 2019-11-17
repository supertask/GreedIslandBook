using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Text.RegularExpressions;

//ブックを掴んで移動させる
public class CardsGrabController : GrabController
{
    public List<GameObject> magicalAssets;
    public GameObject gain;
    public PlayableDirector cardMagicDirector;
    public GameObject smokeObj;
    private Vector3 cardPos;

    public override void Move(Vector3 diffPos)
    {
        this.transform.position += diffPos; //ブックの位置を移動
    }

    protected override void OnTriggerEnter(Collider other) {
        Debug.Log("Hit on card!");
        base.OnTriggerEnter(other);
        this.cardPos = this.transform.position;
    }

    protected override void OnTriggerExit(Collider other) {
        Debug.Log("Exit on card!");
        base.OnTriggerExit(other);
        this.transform.position = this.cardPos ;
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        GameObject cardImageObj = this.transform.Find("CardImage").gameObject;
        Material mat = cardImageObj.GetComponent<MeshRenderer>().material;
        string texName = mat.GetTexture("_MainImage").name;
        if (Input.GetKeyDown(KeyCode.G))
        {
            //煙の位置を移動，煙アニメーションの作動
            Vector3 viewDir = (this.playerHeadTransform.position - this.transform.position).normalized; //頭の位置 - カード位置
            this.smokeObj.transform.position = this.transform.position + 0.05f * viewDir;
            if (this.cardMagicDirector.state == PlayState.Paused) {
                this.cardMagicDirector.Play();
            }

            //アイテムの具現化
            Match match = Regex.Match(texName, @"(\d+)_");
            int assetIndex = int.Parse(match.Result("$1")) - 1; //asset index
            GameObject gainAsset = Instantiate( magicalAssets[assetIndex] );
            Util.EnableMeshRendererRecursively(gainAsset, false); //非表示にする
            gainAsset.transform.position = this.transform.position + 0.1f * Vector3.up; //カードよりもちょい上目
            gainAsset.transform.parent = gain.transform;

            //カードの消滅
            this.GetComponent<BoxCollider>().enabled = false;
            Util.EnableMeshRendererRecursively(this.transform.gameObject, false); //非表示にする
        }
    }

}
