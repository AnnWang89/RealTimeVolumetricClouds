using Assets.Scripts.Clouds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[ExecuteInEditMode, ImageEffectAllowedInSceneView]

public class Clouds : MonoBehaviour
{
    public Shader shader;  
    public GameObject cloudBox;
    private CloudSettings cs;

    public int avgFrameRate;
    public Text displayText;
    private float fpsDisplayUpdate = 1;

    private bool choose_buttom = false;
    // Button
    public Button btnCumulus;
    public Button btnCumulonimbus;
    public Button btnStratus;
    public Button btnStratocumulus;
    public Button btnAltocumulus;
    public Button btnCirrus;
    public Button btnCirrocumulus;

    /* Coefficient */
    private float buttom_scale;
    private float buttom_densityMultiplier;
    private float buttom_densityOffset;
    private float buttom_volumeOffset;
    private float buttom_detailScale;
    private float buttom_detailMultiplier;
    private Vector4 buttom_noiseWeights;
    private Vector3 buttom_detailNoiseWeights;
            
    private float buttom_heightMapFactor;

    private int buttom_marchSteps;
    private float buttom_rayOffset;

    private float buttom_brightness;
    private float buttom_transmitThreshold;
    private float buttom_inScatterMultiplier;
    private float buttom_outScatterMultiplier;

    [HideInInspector]
    public Material material;

    

    void Start()
    {
        /*Button*/
        btnCumulus.onClick.AddListener(Click_btnCumulus);
        btnCumulonimbus.onClick.AddListener(Click_btnCumulonimbus);
        btnStratus.onClick.AddListener(Click_btnStratus);
        btnStratocumulus.onClick.AddListener(Click_btnStratocumulus);
        btnAltocumulus.onClick.AddListener(Click_btnAltocumulus);
        btnCirrus.onClick.AddListener(Click_btnCirrocumulus);
        btnCirrocumulus.onClick.AddListener(Click_btnCirrocumulus);


        /*Coefficient Initial*/
        
        buttom_scale = Settings.cloudScale;
        buttom_densityMultiplier = Settings.densityMultiplier;
        buttom_densityOffset = Settings.densityOffset;
        buttom_volumeOffset=Settings.volumeOffset;
        buttom_detailScale = Settings.detailScale;
        buttom_detailMultiplier = Settings.detailMultiplier;

        buttom_heightMapFactor = Settings.heightMapFactor;
        buttom_noiseWeights = Settings.noiseWeights;
        buttom_detailNoiseWeights = Settings.detailNoiseWeights;
        buttom_marchSteps = Settings.marchSteps;
        buttom_rayOffset = Settings.rayOffset;

        buttom_brightness= Settings.brightness;
        buttom_transmitThreshold= Settings.transmitThreshold;
        buttom_inScatterMultiplier = Settings.inScatterMultiplier;
        buttom_outScatterMultiplier = Settings.outScatterMultiplier;

}
    
    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Validate inputs
        if (material == null)
        {
            material = new Material(shader);
        }
        // Noise
        var noise = FindObjectOfType<Noise>();
        noise.UpdateNoise();

        Vector3 size = cloudBox.transform.localScale;
        Vector3 position = cloudBox.transform.position;

        if(!choose_buttom)
        {
            material.SetFloat("scale", Settings.cloudScale);
            material.SetFloat("densityMultiplier", Settings.densityMultiplier);
            material.SetFloat("densityOffset", Settings.densityOffset);
            material.SetFloat("volumeOffset", Settings.volumeOffset);
            material.SetFloat("detailNoiseScale", Settings.detailScale);
            material.SetFloat("detailNoiseMultiplier", Settings.detailMultiplier);

            material.SetVector("detailWeights", Settings.detailNoiseWeights);
            material.SetVector("noiseWeights", Settings.noiseWeights);
            material.SetFloat("heightMapFactor", Settings.heightMapFactor);

            material.SetInt("marchSteps", Settings.marchSteps);
            material.SetFloat("rayOffset", Settings.rayOffset);

            material.SetFloat("brightness", Settings.brightness);
            material.SetFloat("transmitThreshold", Settings.transmitThreshold);
            material.SetFloat("inScatterMultiplier", Settings.inScatterMultiplier);
            material.SetFloat("outScatterMultiplier", Settings.outScatterMultiplier);
        }
        else
        {
            material.SetFloat("scale", buttom_scale);
            material.SetFloat("densityMultiplier", buttom_densityMultiplier);
            material.SetFloat("densityOffset", buttom_densityOffset);
            material.SetFloat("volumeOffset", buttom_volumeOffset);
            material.SetFloat("detailNoiseScale", buttom_detailScale);
            material.SetFloat("detailNoiseMultiplier", buttom_detailMultiplier);

            material.SetVector("detailWeights", buttom_detailNoiseWeights);
            material.SetVector("noiseWeights", buttom_noiseWeights);
            material.SetFloat("heightMapFactor", buttom_heightMapFactor);

            material.SetInt("marchSteps", buttom_marchSteps);
            material.SetFloat("rayOffset", buttom_rayOffset);

            material.SetFloat("brightness", buttom_brightness);
            material.SetFloat("transmitThreshold", buttom_transmitThreshold);
            material.SetFloat("inScatterMultiplier", buttom_inScatterMultiplier);
            material.SetFloat("outScatterMultiplier", buttom_outScatterMultiplier);

        }


        material.SetTexture("NoiseTex", noise.shapeTexture);
        material.SetTexture("DetailNoiseTex", noise.detailTexture);
        //material.SetFloat("scale", Settings.cloudScale);
        // material.SetFloat("densityMultiplier", Settings.densityMultiplier);
        //material.SetFloat("densityOffset", Settings.densityOffset);
        //material.SetFloat("volumeOffset", Settings.volumeOffset);
        //material.SetFloat("detailNoiseScale", Settings.detailScale);
        //material.SetFloat("detailNoiseMultiplier", Settings.detailMultiplier);
        //material.SetVector("detailWeights", Settings.detailNoiseWeights);
        //material.SetVector("noiseWeights", Settings.noiseWeights);
        material.SetVector("boundsMin", position - size / 2);
        material.SetVector("boundsMax", position + size / 2);
        //material.SetFloat("heightMapFactor", Settings.heightMapFactor);

        //material.SetInt("marchSteps", Settings.marchSteps);
        //material.SetFloat("rayOffset", Settings.rayOffset);
        material.SetTexture("BlueNoise", Settings.blueNoise);
        //material.SetFloat("brightness", Settings.brightness);
        //material.SetFloat("transmitThreshold", Settings.transmitThreshold);
        //material.SetFloat("inScatterMultiplier", Settings.inScatterMultiplier);
        //material.SetFloat("outScatterMultiplier", Settings.outScatterMultiplier);
        material.SetFloat("forwardScatter", Settings.forwardScattering);
        material.SetFloat("backwardScatter", Settings.backwardScattering);
        material.SetFloat("scatterMultiplier", Settings.scatterMultiplier);
        material.SetFloat("timeScale", (Application.isPlaying) ? 1 : 0); // Prevent cloud movement during editing 
        material.SetVector("cloudSpeed", Settings.cloudSpeed);
        material.SetVector("detailSpeed", Settings.detailSpeed);

        displayFPS();

        Graphics.Blit (src, dest, material);
    }

    private void displayFPS() {
        if (displayText == null)
            return;
        float current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;

        if (Time.time >= fpsDisplayUpdate && Application.isPlaying)
        {
            
            fpsDisplayUpdate = Time.time + 0.5f;
            displayText.text = "FPS: " + avgFrameRate.ToString();
        }
    }

    /*Button Control*/
    private void Click_btnCumulus()
    {
        Debug.Log("CLICK btnCumulus IN!");
        choose_buttom = true;

        buttom_scale = (float)1.5;
        buttom_densityMultiplier = (float)21;
        buttom_densityOffset = (float)4.16;
        buttom_volumeOffset = (float)3.82;
        buttom_detailScale = (float)0.93;
        buttom_detailMultiplier = (float)2.1;
        buttom_noiseWeights = new Vector4(9.05f, 1.4f, 0.88f, 0.12f);
        buttom_detailNoiseWeights = new Vector3(0.67f, 0.3f, 0.15f);
        buttom_heightMapFactor = (float)0.97;

        buttom_marchSteps = 18;
        buttom_rayOffset = (float)30.2;

        buttom_brightness = (float)1.0;
        buttom_transmitThreshold = (float)0.52;
        buttom_inScatterMultiplier = (float)0.32;
        buttom_outScatterMultiplier = (float)0.52;

    }
    private void Click_btnCumulonimbus()
    {
        Debug.Log("CLICK btnCumulonimbus IN!");
        choose_buttom = true;
        buttom_scale = (float)2;
        buttom_densityMultiplier = (float)21.3;
        buttom_densityOffset = (float)3.74;
        buttom_volumeOffset = (float)3.82;
        buttom_detailScale = (float)0.7;
        buttom_detailMultiplier = (float)1.24;
        buttom_noiseWeights = new Vector4(9.05f, 1.4f, 0.88f, 0.12f);
        buttom_detailNoiseWeights = new Vector3(0.67f, 0.21f, 0.15f);
        buttom_heightMapFactor = (float)0.94;

        buttom_marchSteps = 18;
        buttom_rayOffset = (float)30.2;

        buttom_brightness = (float)0.72;
        buttom_transmitThreshold = (float)0.39;
        buttom_inScatterMultiplier = (float)0.32;
        buttom_outScatterMultiplier = (float)0.52;

    }
    private void Click_btnStratus()
    {
        Debug.Log("CLICK btnStratus IN!");
        choose_buttom = true;
        buttom_scale = (float)1.96;
        buttom_densityMultiplier = (float)2.75;
        buttom_densityOffset = (float)1.25;
        buttom_volumeOffset = (float)4.16;
        buttom_detailScale = (float)0.57;
        buttom_detailMultiplier = (float)2.17;
        buttom_noiseWeights = new Vector4(15.8f, 1.24f, -1.6f, 4.1f);
        buttom_detailNoiseWeights = new Vector3(0.81f, 1.54f, 8.65f);
        buttom_heightMapFactor = (float)0.96;

        buttom_marchSteps = 1;
        buttom_rayOffset = (float)41.5;

        buttom_brightness = (float)0.98;
        buttom_transmitThreshold = (float)0.45;
        buttom_inScatterMultiplier = Settings.inScatterMultiplier;
        buttom_outScatterMultiplier = Settings.outScatterMultiplier;

    }
    private void Click_btnStratocumulus()
    {
        Debug.Log("CLICK btnStratocumulus IN!");
        choose_buttom = true;
        buttom_scale = Settings.cloudScale;
        buttom_densityMultiplier = Settings.densityMultiplier;
        buttom_densityOffset = Settings.densityOffset;
        buttom_volumeOffset = Settings.volumeOffset;
        buttom_detailScale = Settings.detailScale;
        buttom_detailMultiplier = Settings.detailMultiplier;
        buttom_noiseWeights = Settings.noiseWeights;
        buttom_detailNoiseWeights = Settings.detailNoiseWeights;
        buttom_heightMapFactor = Settings.heightMapFactor;

        buttom_marchSteps = Settings.marchSteps;
        buttom_rayOffset = Settings.rayOffset;

        buttom_brightness = Settings.brightness;
        buttom_transmitThreshold = Settings.transmitThreshold;
        buttom_inScatterMultiplier = Settings.inScatterMultiplier;
        buttom_outScatterMultiplier = Settings.outScatterMultiplier;

    }
    private void Click_btnAltocumulus()
    {
        Debug.Log("CLICK btnAltocumulus IN!");
        choose_buttom = true;
        buttom_scale = Settings.cloudScale;
        buttom_densityMultiplier = Settings.densityMultiplier;
        buttom_densityOffset = Settings.densityOffset;
        buttom_volumeOffset = Settings.volumeOffset;
        buttom_detailScale = Settings.detailScale;
        buttom_detailMultiplier = Settings.detailMultiplier;
        buttom_noiseWeights = Settings.noiseWeights;
        buttom_detailNoiseWeights = Settings.detailNoiseWeights;
        buttom_heightMapFactor = Settings.heightMapFactor;

        buttom_marchSteps = Settings.marchSteps;
        buttom_rayOffset = Settings.rayOffset;

        buttom_brightness = Settings.brightness;
        buttom_transmitThreshold = Settings.transmitThreshold;
        buttom_inScatterMultiplier = Settings.inScatterMultiplier;
        buttom_outScatterMultiplier = Settings.outScatterMultiplier;

    }
    private void Click_btnCirrus()
    {
        Debug.Log("CLICK btnCirrus IN!");
        choose_buttom = true;
        
        buttom_scale = Settings.cloudScale;
        buttom_densityMultiplier = Settings.densityMultiplier;
        buttom_densityOffset = Settings.densityOffset;
        buttom_volumeOffset = Settings.volumeOffset;
        buttom_detailScale = Settings.detailScale;
        buttom_detailMultiplier = Settings.detailMultiplier;
        buttom_noiseWeights = Settings.noiseWeights;
        buttom_detailNoiseWeights = Settings.detailNoiseWeights;
        buttom_heightMapFactor = Settings.heightMapFactor;

        buttom_marchSteps = Settings.marchSteps;
        buttom_rayOffset = Settings.rayOffset;

        buttom_brightness = Settings.brightness;
        buttom_transmitThreshold = Settings.transmitThreshold;
        buttom_inScatterMultiplier = Settings.inScatterMultiplier;
        buttom_outScatterMultiplier = Settings.outScatterMultiplier;

    }
    private void Click_btnCirrocumulus()
    {
        Debug.Log("CLICK btnCirrocumulus IN!");
        choose_buttom = true;
        buttom_scale = 1.06f;
        buttom_densityMultiplier = 13.86f;
        buttom_densityOffset = 1.36f;
        buttom_volumeOffset = 4.11f;
        buttom_detailScale = 2.42f;
        buttom_detailMultiplier = 2f;
        buttom_noiseWeights = new Vector4(13.4f, 1.63f, -1.11f, 4.1f);
        buttom_detailNoiseWeights = new Vector3(0.81f, 1.54f, 8.65f);
        buttom_heightMapFactor = 0.965f;

        buttom_marchSteps = 14;
        buttom_rayOffset = 29f;

        buttom_brightness = (float)0.75;
        buttom_transmitThreshold = (float)0.3;
        buttom_inScatterMultiplier = (float)0.49;
        buttom_outScatterMultiplier = (float)0.6;

    }

    private CloudSettings Settings {
        get {
            if( cs == null)
                cs = cloudBox.GetComponent<CloudSettings>();
            return cs;
        }
    }
}