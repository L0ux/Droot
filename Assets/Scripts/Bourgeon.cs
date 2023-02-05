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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }    

 

    public void Select()
    {
        if(spriteRenderer == null )
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = selectedSprite;
    }

    public void UnSelect()
    {
        spriteRenderer.sprite = normalSprite;
    }

    public void endOfAnimation()
    {
        GetComponent<Animator>().enabled = false;
        spriteRenderer.sprite = normalSprite;
    }
}
