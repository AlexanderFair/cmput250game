using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingButton : RoomObjectClass
{
    public float speed = 3f;
    public float bounds = 4.8f;
    private int dir = -1;

    public GameObject uiPrefab;

    private BoxCollider2D bcollider;

    // Start is called before the first frame update
    void Start()
    {
        bcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public override void UpdateRoomObject()
    {
        transform.position += Vector3.left * speed * dir * Time.deltaTime;
        if(Mathf.Abs(transform.position.x) > bounds) { dir *= -1; }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Util.GetKeyDownWithMouseOverObject(KeyCode.Mouse0, bcollider))
            {
                MouseClicked();
            }
        }
        
    }

    int count = 0;
    void MouseClicked()
    {
        Debug.Log("Clicked "+(count++));
        UIManager ui = UIObjectClass.InstantiateNewUIElement(uiPrefab).GetComponent<UIManager>();
        ui.RoomObjectClass = this;
        
    }

    public void FinishedUI(GameObject o)
    {
        Debug.Log("Clicked");
        UIObjectClass.DestroyUIObject(o);
    }
}
