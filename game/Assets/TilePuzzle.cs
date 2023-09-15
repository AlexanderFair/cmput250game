using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePuzzle : MonoBehaviour
{
    public Vector2[] placeablepoints;
    public double snapRadius = 0.25;
    //public Sprite spritevar;
    private List<Transform> tiles = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            tiles.Add(this.gameObject.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {

        foreach(Transform tile in tiles)
        {
            //tile.position += Vector3.right * Time.deltaTime;
        }
    }

    public void snap(Tile tile)
    {
        foreach (Vector2 point in placeablepoints)
        {
            if (Vector2.Distance(point, new Vector2(tile.transform.position.x, tile.transform.position.y)) < snapRadius){
                Debug.Log(new Vector2(tile.transform.position.x, tile.transform.position.y));
                Debug.Log(point);
                Debug.Log(Vector2.Distance(point, new Vector2(tile.transform.position.x, tile.transform.position.y)));
                Debug.Log(snapRadius);
                tile.transform.localPosition = point;
            }
        }
    }
}

