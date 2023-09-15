using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector2 clickPosition;

    public void OnMouseDown()
    {
        clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    public void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - clickPosition;
    }

    public void OnMouseUp()
    {
        transform.parent.GetComponent<TilePuzzle>().snap(this);
    }
}
