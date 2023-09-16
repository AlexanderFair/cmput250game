using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{

    public static int GRID_HALF_SIZE = 4;
    public static int GRID_SIZE = (GRID_HALF_SIZE * 2 + 1) + 2;

    public Sprite pipeCorner;
    public Sprite pipeStraight;
    public Sprite pipeIntersection;
    public Sprite pipeStart;
    public Sprite door;

    public Sprite pipeWaterCorner;
    public Sprite pipeWaterStraight;
    public Sprite pipeWaterIntersection;
    public Sprite pipeWaterStart;
    public Sprite doorWater;

    public GameObject pipePrefab;

    public PipeSquare[,] pipeGrid = new PipeSquare[GRID_SIZE, GRID_SIZE];
    public DoorSquare[] doors;
    public PipeSquare start;

    // Start is called before the first frame update
    void Start()
    {
        PipeSquare.prefab = pipePrefab;
        Sprite[] pipeSprites = { pipeCorner, pipeStraight, pipeIntersection, pipeStart, door};
        Sprite[] pipeWaterSprites = { pipeWaterCorner, pipeWaterStraight, pipeWaterIntersection, pipeWaterStart, doorWater };

        for (int x=-GRID_HALF_SIZE; x<= GRID_HALF_SIZE; x++)
        {
            for(int y=-GRID_HALF_SIZE; y<= GRID_HALF_SIZE; y++)
            {
                if(x==0 && y == 0) { continue; }

                int pipeType = Random.Range(0, 3);
                int rotation = Random.Range(0, 4);

                pipeGrid[GetArrayLoc(x), GetArrayLoc(y)] = new PipeSquare(x,y,pipeType,rotation, pipeSprites[pipeType], pipeWaterSprites[pipeType]);
            }
        }

        doors = new DoorSquare[]
        { 
            new DoorSquare(0, GRID_HALF_SIZE + 1, 4, 0, door, doorWater, "Top"), 
            new DoorSquare(-GRID_HALF_SIZE - 1, 0, 4, 3, door, doorWater, "Left"), 
            new DoorSquare(0, -GRID_HALF_SIZE - 1, 4, 2, door, doorWater, "Bottom"), 
            new DoorSquare(GRID_HALF_SIZE + 1, 0, 4, 1, door, doorWater, "Right") 
        };
        start = new PipeSquare(0, 0, 3, 0, pipeStart, pipeWaterStart);

        pipeGrid[GRID_HALF_SIZE + 1, GRID_HALF_SIZE + 1] = start;   
        pipeGrid[GetArrayLoc(-GRID_HALF_SIZE - 1),GetArrayLoc(0)] = doors[1];//left
        pipeGrid[GetArrayLoc(GRID_HALF_SIZE+1), GetArrayLoc(0)] = doors[3];//right
        pipeGrid[GetArrayLoc(0), GetArrayLoc(-GRID_HALF_SIZE-1)] = doors[2];//bottom
        pipeGrid[GetArrayLoc(0), GetArrayLoc(GRID_HALF_SIZE+1)] = doors[0];//top
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate();
        }
        
    }


    void Rotate()
    {
        int x = Mathf.FloorToInt(PlayerScript.player.transform.position.x);
        int y = Mathf.FloorToInt(PlayerScript.player.transform.position.y);
        if (x<-GRID_SIZE || x>GRID_SIZE || y<-GRID_SIZE || y > GRID_SIZE) { return; }

        PipeSquare pipe = pipeGrid[GetArrayLoc(x), GetArrayLoc(y)];
        pipe.Rotate();
        Flow();
    }

    public void Flow()
    {

        for(int i=0; i<GRID_SIZE; i++)
        {
            for(int j=0; j<GRID_SIZE; j++)
            {
                PipeSquare s = pipeGrid[i, j];
                s?.SetWater(false);
            }
        }

        bool[,] checkedGrid = new bool[GRID_SIZE, GRID_SIZE];
        Queue<PipeSquare> queue = new Queue<PipeSquare>();
        queue.Enqueue(start);

        while(queue.Count > 0)
        {
            PipeSquare pipe = queue.Dequeue();
            pipe.SetWater(true);
            checkedGrid[GetArrayLoc(pipe.X), GetArrayLoc(pipe.Y)] = true;

            HashSet<PipeSquare> neighbours = pipe.GetNeighbours(pipeGrid);
            foreach (PipeSquare neighbour in neighbours)
            {
                if (neighbour.GetNeighbours(pipeGrid).Contains(pipe))
                {//Valid neighbour
                    if (!checkedGrid[GetArrayLoc(neighbour.X), GetArrayLoc(neighbour.Y)])
                    {
                        //Not already checked
                        queue.Enqueue(neighbour);
                    }

                }
            }
        }
    }

    public static int GetArrayLoc(int gridLoc)
    {
        return gridLoc + GRID_HALF_SIZE + 1;
    }
    public static int GetGridLoc(int arrayLoc)
    {
        return arrayLoc - 1 - GRID_HALF_SIZE;
    }
}
