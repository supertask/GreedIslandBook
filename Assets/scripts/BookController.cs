using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BookController : MonoBehaviour
{
    public GameObject cardModelObj;
    public PlayableDirector openingCardDirector;

    private CardsModel model;

    void Start()
    {
        //this.model = this.cardModelObj.GetComponent<CardsModel>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (this.openingCardDirector.state == PlayState.Paused) {
                this.openingCardDirector.Play();
            }
        }
    }
}
