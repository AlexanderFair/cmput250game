using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakSprite : MonoBehaviour
{
    // registers the template 
    void Start()
    {
        BasicPipe.LEAK_SPRITE_RENDERER_TEMPLATE = this.gameObject.GetComponent<SpriteRenderer>();
    }
}
