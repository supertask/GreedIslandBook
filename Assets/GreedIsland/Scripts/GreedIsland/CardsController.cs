using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CardsController : MonoBehaviour
{
    public GameObject cardModelObj;

    private CardsModel model;
    private PlayableDirector director;

    void Start()
    {
        this.model = this.cardModelObj.GetComponent<CardsModel>();
        this.director = this.GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) {
            if (this.director.state == PlayState.Paused) {
                this.model.nextPage();
                this.director.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.K)) {
            if (this.director.state == PlayState.Paused) {
                this.model.previousPage();
                this.director.Play();
            }
        }
    }
}
