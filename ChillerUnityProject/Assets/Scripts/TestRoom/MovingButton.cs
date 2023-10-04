using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingButton : DisplayUIRoomObject
{
    public float speed = 3f;
    public float bounds = 4.8f;
    private int dir = -1;


    // Update is called once per frame
    protected override void UpdateRoomObject()
    {
        transform.position += Vector3.left * speed * dir * Time.deltaTime;
        if(Mathf.Abs(transform.position.x) > bounds) { dir *= -1; }

        base.UpdateRoomObject();

        if(Interactable)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        
    }

    int count = 0;

    protected override void DisplayedUI()
    {
        Debug.Log("Clicked " + (count++));
        //UIManager uim = ui.GetComponent<UIManager>();
        //uim.RoomObjectClass = this;
    }

    public void FinishedUI(GameObject o)
    {
        UIObjectClass.DestroyUIObject(o);
        Debug.Log(UIObjectClass.IsUIActive());
    }
}
