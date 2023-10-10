using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class MenuClickableObject : MenuObjectClass
{
    public Settings.Controls clickKey = Settings.Controls.LeftClick;
    public Collider2D clickCollider;
    public MenuClickCaller menuObject;
    
    protected override void UpdateMenuObject()
    {
        if(Util.GetKeyDownWithMouseOverObject(clickKey.Get(), clickCollider))
        {
            menuObject.OnMenuClick(this);
        }
    }
}

public abstract class MenuClickCaller : MenuObjectClass
{
    public abstract void OnMenuClick(MenuClickableObject obj);
}
