using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSquare
{
    private readonly GameObject prefab;

    private GameObject obj;
    public int rotation = 0;


    private static HashSet<int>[] neighbours =
    {
        new HashSet<int> { 0, 1 },  //Corner
        new HashSet<int> { 0, 2 },  //Stragiht
        new HashSet<int> { 0, 1, 2 },  //Int
        new HashSet<int> { 0, 1, 2, 3 },  //Start
        new HashSet<int> { 1 }      //door
    };
    private Vector3[] directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up };

    public PipeSquare(int x, int y, int pipeType, int rotation, GameObject pipePrefab)
    {
        X = x;
        Y = y;
        PipeType = pipeType;
        prefab = pipePrefab;
        DefaultRotation = rotation;

        this.obj = Object.Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);

        SetRotation(rotation);
    }

    public PipeSquare(int x, int y, int pipeType, int rotation, GameObject pipePrefab, string name) : this(x, y, pipeType, rotation, pipePrefab)
    {
        Name = name;
    }

    public int X { get; }
    public int Y { get; }
    public int PipeType { get; }
    public int DefaultRotation { get; }
    public string Name { get; }

    public void SetRotation(int i)
    {
        i %= 4;
        while(rotation != i)
        {
            Rotate();
        }
    }

    public void Rotate()
    {

        rotation++;
        rotation %= 4;


        Debug.Log("rotate" + rotation);

        obj.transform.RotateAround(new Vector3(X + 0.5f, Y + 0.5f, obj.transform.position.z), new Vector3(0, 0, 1), -90);
    }

    public HashSet<PipeSquare> GetNeighbours(PipeSquare[,] grid)
    {
        HashSet<int> dirs = neighbours[PipeType];
        HashSet<PipeSquare> neighbourPipes = new HashSet<PipeSquare>();

        foreach (int i in dirs)
        {
            int roti = (i + rotation) % 4;
            int nx = X+(int)directions[roti].x;
            int ny = Y+(int)directions[roti].y;
            PipeSquare neigh = grid[GridScript.GetArrayLoc(nx),GridScript.GetArrayLoc(ny)];
            if (neigh != null)
            {
                neighbourPipes.Add(neigh);
            }
        }
        return neighbourPipes;
    }
}
