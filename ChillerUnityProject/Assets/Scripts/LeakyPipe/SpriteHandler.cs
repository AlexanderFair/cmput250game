using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// bind this script on a template leak sprite renderer
public class SpriteHandler : MonoBehaviour
{
    // leak sprite
    public GameObject leakSpriteRendererPrefab;
    // connected sprites
    public Sprite inputConn, outputConn, straightConn, bentConn, triangularConn;
    // frozen sprites
    public Sprite[] inputFrozen = new Sprite[BasicPipe.MAX_FROZEN_LAYERS], 
                    outputFrozen = new Sprite[BasicPipe.MAX_FROZEN_LAYERS], 
                    straightFrozen = new Sprite[BasicPipe.MAX_FROZEN_LAYERS], 
                    bentFrozen = new Sprite[BasicPipe.MAX_FROZEN_LAYERS], 
                    triangularFrozen = new Sprite[BasicPipe.MAX_FROZEN_LAYERS];
    // registers the template and sprite for other pipes
    void Start()
    {
        BasicPipe.LEAK_SPRITE_RENDERER_TEMPLATE = leakSpriteRendererPrefab;
        
        // setup pipe sprites
        BasicPipe.CONNECTED_SPRITE[PipeInput.PIPE_TYPE_IDX] = inputConn;
        BasicPipe.FROZEN_SPRITES[PipeInput.PIPE_TYPE_IDX] = inputFrozen;

        BasicPipe.CONNECTED_SPRITE[PipeOutput.PIPE_TYPE_IDX] = outputConn;
        BasicPipe.FROZEN_SPRITES[PipeOutput.PIPE_TYPE_IDX] = outputFrozen;
        
        BasicPipe.CONNECTED_SPRITE[PipeStraight.PIPE_TYPE_IDX] = straightConn;
        BasicPipe.FROZEN_SPRITES[PipeStraight.PIPE_TYPE_IDX] = straightFrozen;
        
        BasicPipe.CONNECTED_SPRITE[PipeBent.PIPE_TYPE_IDX] = bentConn;
        BasicPipe.FROZEN_SPRITES[PipeBent.PIPE_TYPE_IDX] = bentFrozen;
        
        BasicPipe.CONNECTED_SPRITE[PipeTriangular.PIPE_TYPE_IDX] = triangularConn;
        BasicPipe.FROZEN_SPRITES[PipeTriangular.PIPE_TYPE_IDX] = triangularFrozen;
    }
}
