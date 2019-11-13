using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BookController : MonoBehaviour
{
    public GameObject bookBodyObj;
    public GameObject bookUnderNavObj;
    public GameObject cardModelObj;
    public PlayableDirector openingBookDirector;
    public bool isDebug;

    private CardsModel model;

    void Start()
    {
        if (isDebug) {
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", 0.02f);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_HorizontalClipPercent", 0.02f);
            this.bookUnderNavObj.GetComponent<MeshRenderer>().material.SetFloat("_ClipPercent", 0.1f);
        }
        else {
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_VerticalClipPercent", 0.48f);
            this.bookBodyObj.GetComponent<MeshRenderer>().material.SetFloat("_HorizontalClipPercent", 0.18f);
            this.bookUnderNavObj.GetComponent<MeshRenderer>().material.SetFloat("_ClipPercent", 0.5f);
            this.bookBodyObj.GetComponent<MeshRenderer>().enabled = false;
            this.bookUnderNavObj.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (this.openingBookDirector.state == PlayState.Paused) {
                this.bookBodyObj.GetComponent<MeshRenderer>().enabled = true;
                this.bookUnderNavObj.GetComponent<MeshRenderer>().enabled = true;
                this.openingBookDirector.Play();
            }
        }
    }
}
