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
        StartAddEffect();
    }
    //Called each frame
    void Update()
    {
        AddEffectUpdate();
        if (insanityStat < insanityThreshold || MenuObjectClass.IsMenuActive())
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
        AddedInsanity(add);
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



    //--add effect

    [Header("Add Effect")]
    //changes to min sat when min insanity or less is added
    [Range(-100, 0)]
    public float minSaturation;
    [Range(0, 100)]
    public float minInsanity;
    //changes to max sat when max insanity or more is added
    [Range(-100, 0)]
    public float maxSaturation;
    [Range(0, 100)]
    public float maxInsanity;
    //Takes desaturateTime to reach the full desaturation
    public float desaturateTime;
    //Takes saturation time to go back to normal after desaturating
    public float saturateTime;
    //Random Audio clip is played when insanity is added
    public AudioClip[] addEffects;


    private float targetDesat = 0f;
    private float desatTime = 0f;
    private float satTime = 0f;

    private bool sat = false;
    private bool desat = false;
    private float currentVal = 0f;

    private void StartAddEffect()
    {
        UpdateShader();
    }

    private void AddEffectUpdate()
    {

        float val = 0f;
        if (desat)
        {
            desatTime += Time.deltaTime;
            if (desatTime >= desaturateTime)
            {
                val = targetDesat;
                desatTime = 0f;
                desat = false;
                sat = true;
            }
            else
            {
                val = Mathf.Lerp(0f, targetDesat, Mathf.SmoothStep(0, 1, desatTime / desaturateTime));
            }
        }
        else if (sat)
        {
            satTime += Time.deltaTime;
            if (satTime >= saturateTime)
            {
                val = 0f;
                sat = false;
                satTime = 0f;
            }
            else
            {
                val = Mathf.Lerp(targetDesat, 0f, Mathf.SmoothStep(0, 1, satTime / saturateTime));
            }
        }

        if (val != currentVal)
        {
            currentVal = val;
            UpdateShader();
        }

    }

    private void UpdateShader()
    {
        ColorAdjustmentPostProcessing.Instance?.ChangeSaturation(currentVal);
    }

    private void AddedInsanity(float amount)
    {
        float interp = Mathf.Clamp01((amount - minInsanity) / (maxInsanity - minInsanity));
        targetDesat = Mathf.Lerp(minSaturation, maxSaturation, interp);
        desatTime = 0f;
        satTime = 0f;
        desat = true;
        AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(addEffects));
    }
}
