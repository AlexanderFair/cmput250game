using UnityEngine;

/*
 * The class which deals with the insanity metre
 * To add to the insanity, call Insanity.Instance.AddInsanity(float) or Insanity.Add(float)
 */
public class Insanity : MonoBehaviour
{

    [Header("Insanity settings")]
    // The Min value before insanity starts kicking in
    public float insanityThreshold;
    // function of insanity to tics per second
    public AnimationCurve insanityToTicRateGraph;
    // scales the result
    public float insanityToTicRateGraphScalar;
    // function of insanity to max tic class before vision (gets rounded to int after evaluating)
    public AnimationCurve insanityToMaxTicBeforeVisionGraph;

    // The insanity is low if it is below lowInsanityMax
    public float lowInsanityMax;
    // The insanity is high if it is above medInsanityMax
    public float medInsanityMax;


    private float insanityStat = 0;

    private float ticTimer = 0;
    private int ticsWithoutVision = 0;


    private static Insanity _instance;
    private static bool isDefined = false;
    public static Insanity Instance
    {
        get
        {
            if(_instance == null)
            {
                throw new System.NullReferenceException("There is no insanity instance in the scene");
            }
            return _instance;
        }
    }

    void Start()
    {
        if(isDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }
        isDefined = true;
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //Called each frame
    void Update()
    {
        if(insanityStat < insanityThreshold || MenuObjectClass.IsMenuActive())
        {
            return;
        }

        ticTimer += Time.deltaTime;
        float ticker = 1f / insanityToTicRateGraph.Evaluate(insanityStat);
        if (ticTimer > ticker)
        {
            Tic();
            ticTimer %= ticker;
        }

    }
    //Called each tic
    private void Tic()
    {
        int ticCallPerVision = Mathf.RoundToInt(insanityToMaxTicBeforeVisionGraph.Evaluate(insanityStat));
        int ticProb = ticCallPerVision - ticsWithoutVision;
        float call = Random.Range(0.0f, 1.0f);
        if (ticProb <= 0 || call < 1f / ticProb)
        {
            ticsWithoutVision = 0;
            Visions.Instance.HaveVision();
        }
        else
        {
            ticsWithoutVision++;
        }
    }

    
    // Adds the amount to the insanity level
    public void AddInsanity(float add)
    {
        if (add <= 0) {
            throw new System.Exception("added insanity must be a positive amount");
        } 
        insanityStat += add;
    }

    public bool IsHigh()
    {
        return insanityStat > medInsanityMax;
    }

    public bool IsMedium()
    {
        return insanityStat > lowInsanityMax && insanityStat <= medInsanityMax;
    }

    public bool IsLow()
    {
        return insanityStat <= lowInsanityMax;
    }

    // Adds the amount to the insanity level
    public static void Add(float add)
    {
        Instance.AddInsanity(add);
    }


    public float getInsanity()
    {
        return insanityStat;
    }
}
