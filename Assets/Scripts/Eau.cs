using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eau : MonoBehaviour
{
    Bout pickedBy;

    public void pickWater(Bout leGourmand)
    {
        pickedBy = leGourmand;
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.SetTrigger("onPick");
    }

    void onEndFillScreen()
    {
        pickedBy.die();
        GameManager.instance.dezoomCamera();
        
    }


}
