using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bourgeon : MonoBehaviour
{

    public Sprite selectedSprite;
    public Sprite normalSprite;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        float modifTaille = Random.Range(0.1f, 0.18f);
        this.transform.localScale = new Vector3(modifTaille,modifTaille,modifTaille);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }    

    public void retract()
    {
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.enabled = true;
        myAnimator.SetTrigger("onRetract");
    }

    public void Select()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
        spriteRenderer.sprite = selectedSprite;
    }

    public void UnSelect()
    {
        spriteRenderer.sortingOrder = 5;

        spriteRenderer.sprite = normalSprite;
    }

    public void endOfAnimation()
    {
        GetComponent<Animator>().enabled = false;
        spriteRenderer.sprite = normalSprite;
    }
}
