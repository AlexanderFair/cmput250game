using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePuzzle : MonoBehaviour
{
    public Vector2[] snapablePoints;
    public int[] idSolution;
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

    }

    public void snap(Tile tile)
    {
        foreach (Vector2 point in snapablePoints)
        {
            if (Vector2.Distance(point, new Vector2(tile.transform.position.x, tile.transform.position.y)) < snapRadius){
                tile.transform.localPosition = point;
                check();
                break;
            }
        }
    }
    // check if puzzle is solved;
    private void check()
    {
        for (int i = 0; i < snapablePoints.Length; i++)
        {
            bool hasTile = false;
            foreach (Transform tile in tiles)
            {
                if (tile.position.x == snapablePoints[i].x && tile.position.y == snapablePoints[i].y && tile.gameObject.GetComponent<Tile>().id == idSolution[i])
                {
                    hasTile = true;
                    break;
                }
            }

            if (!hasTile)
            {
                return;
            }
        }

        Debug.Log("Solved!");
    }
}

