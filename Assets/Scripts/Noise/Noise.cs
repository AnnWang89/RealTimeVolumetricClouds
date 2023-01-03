using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///* Coefficient */
//bool choose_buttom_out = false;
//int choose_A_out;
//int choose_B_out;
//int choose_C_out;

[ExecuteInEditMode]
public class Noise : MonoBehaviour
{
    public Button btnCumulus;
    public Button btnCumulonimbus;
    public Button btnStratus;
    public Button btnStratocumulus;
    public Button btnAltocumulus;
    public Button btnCirrus;
    public Button btnCirrocumulus;


    ///* Coefficient */
    private int choose_A;
    private int choose_B;
    private int choose_C;
    private string load_name = "";

    private bool choose_buttom = false;

    [HideInInspector]
    public enum NoiseType { Shape, Detail }
    [HideInInspector]
    public enum NoiseChannel { R, G, B, A }

    public ComputeShader noiseCompute;

    [HideInInspector]
    public NoiseType activeTextureType;
    [HideInInspector]
    public NoiseChannel activeChannel;
    [HideInInspector]
    public NoiseSettings activeSettings;

    readonly int[] TEXTURE_SIZE = { 128, 64 };

    public ComputeShader crossSection;

    [SerializeField, HideInInspector]
    public NoiseSettings[] settingsList;
    bool updateNoise;
    List<ComputeBuffer> buffers;

    [SerializeField, HideInInspector]
    public RenderTexture shapeTexture;
    [SerializeField, HideInInspector]
    public RenderTexture detailTexture;

    void Awake()
    {
        shapeTexture = CreateTexture(TEXTURE_SIZE[0]);
        detailTexture = CreateTexture(TEXTURE_SIZE[1]);

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        updateNoise = true;
        activeSettings.Set(settingsList[0]);

        /*Button*/
        btnCumulus.onClick.AddListener(Click_btnCumulus);
        btnCumulonimbus.onClick.AddListener(Click_btnCumulonimbus);
        btnStratus.onClick.AddListener(Click_btnStratus);
        btnStratocumulus.onClick.AddListener(Click_btnStratocumulus);
        btnAltocumulus.onClick.AddListener(Click_btnAltocumulus);
        btnCirrus.onClick.AddListener(Click_btnCirrocumulus);
        btnCirrocumulus.onClick.AddListener(Click_btnCirrocumulus);
    }

    /*Button Control*/
    private void Click_btnCumulus()
    {
        Debug.Log("CLICK btnCumulus IN!");
        choose_buttom = true;
        //choose_A = 3;
        //choose_B = 1;
        //choose_C = 2;
        load_name = "Cumulus";

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);
    }
    private void Click_btnCumulonimbus()
    {
        Debug.Log("CLICK btnCumulonimbus IN!");
        choose_buttom = true;
        //choose_A = 3;
        //choose_B = 3;
        //choose_C = 11;
        load_name = "Cumulonimbus";

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);
    }
    private void Click_btnStratus()
    {
        Debug.Log("CLICK btnStratus IN!");
        choose_buttom = true;
        //choose_A = 5;
        //choose_B = 1;
        //choose_C = 30;
        load_name = "Stratus";

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);
    }
    private void Click_btnStratocumulus()
    {
        Debug.Log("CLICK btnStratocumulus IN!");
        choose_buttom = true;
        //choose_A = 3;
        //choose_B = 40;
        //choose_C = 60;

        load_name = "Stratocumulus";

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);
    }
    private void Click_btnAltocumulus()
    {
        Debug.Log("CLICK btnAltocumulus IN!");
        choose_buttom = true;
        //choose_A = 12;
        //choose_B = 13;
        //choose_C = 11;

        load_name = "Altocumulus";

        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);
    }
    private void Click_btnCirrus()
    {
        Debug.Log("CLICK btnCirrus IN!");
        choose_buttom = true;
        //choose_A = 3;
        //choose_B = 40;
        //choose_C = 60;
        load_name = "Cirrus";


        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);

    }
    private void Click_btnCirrocumulus()
    {
        Debug.Log("CLICK btnCirrocumulus IN!");
        choose_buttom = true;
        //choose_A = 25;
        //choose_B = 40;
        //choose_C = 47;
        load_name = "Cirrocumulus";
        
        
        settingsList = LoadSettings();
        foreach (var settings in settingsList)
        {
            if (settings != null)
                ForceUpdate(settings);
            else
                ForceUpdate(new NoiseSettings());
        }
        activeSettings.Set(settingsList[0]);

    }

    public void UpdateNoise(NoiseSettings settings = null)
    {
        settings = settings == null ? activeSettings : settings;

        //if(choose_buttom)
        //{
        //    settings.frequencyA = choose_A;
        //    settings.frequencyB = choose_B;
        //    settings.frequencyC = choose_C;
        //}
        if (updateNoise && noiseCompute && settings != null)
        {
            RenderTexture texture = GetTexture(settings.type);
            updateNoise = false;
            buffers = new List<ComputeBuffer>();

            noiseCompute.SetFloat("layerMix", settings.mix);
            noiseCompute.SetInt("resolution", TEXTURE_SIZE[settings.type]);
            noiseCompute.SetVector("channelMask", ChannelMask((NoiseChannel)settings.channel));
            noiseCompute.SetTexture(0, "result", texture);
            var limitsBuffer = SetBuffer(new int[] { int.MaxValue, 0 }, sizeof(int), "limits");
            UpdateProperties(settings);
 
            int threads = Mathf.CeilToInt(TEXTURE_SIZE[settings.type] / 8.0f);

            noiseCompute.Dispatch(0, threads, threads, threads);
           
            noiseCompute.SetBuffer(1, "limits", limitsBuffer);
            noiseCompute.SetTexture(1, "result", texture);
            noiseCompute.Dispatch(1, threads, threads, threads);

            foreach (var buffer in buffers)
                buffer.Release();
        }
    }

    public RenderTexture GetTexture(int index)
    {
        if (index == 0)
        {
            return shapeTexture;
        }
        return detailTexture;
    }

    public NoiseSettings GetSetting(int shapeIndex, int channelIndex)
    {
        if (shapeIndex > 1 || channelIndex > 3 - shapeIndex)
            return null;
        return settingsList[shapeIndex * 4 + channelIndex];
    }

    public Vector4 ChannelMask(NoiseChannel index)
    {
        Vector4 channelWeight = new Vector4();
        channelWeight[(int)index] = 1;
        return channelWeight;
    }

    void UpdateProperties(NoiseSettings settings)
    {
        System.Random rand = new System.Random(settings.seed);
        GenerateRandomPoints(rand, settings.frequencyA, "pointsA");
        GenerateRandomPoints(rand, settings.frequencyB, "pointsB");
        GenerateRandomPoints(rand, settings.frequencyC, "pointsC");

        noiseCompute.SetInt("frequencyA", settings.frequencyA);
        noiseCompute.SetInt("frequencyB", settings.frequencyB);
        noiseCompute.SetInt("frequencyC", settings.frequencyC);

        //if (!choose_buttom)
        //{
        //    GenerateRandomPoints(rand, settings.frequencyA, "pointsA");
        //    GenerateRandomPoints(rand, settings.frequencyB, "pointsB");
        //    GenerateRandomPoints(rand, settings.frequencyC, "pointsC");

        //    noiseCompute.SetInt("frequencyA", settings.frequencyA);
        //    noiseCompute.SetInt("frequencyB", settings.frequencyB);
        //    noiseCompute.SetInt("frequencyC", settings.frequencyC);
        //}
        //else
        //{
        //    settings.frequencyA = choose_A;
        //    settings.frequencyB = choose_B;
        //    settings.frequencyC = choose_C;
        //    activeSettings.Set(settings);
        //    activeSettings = settings;

        //    GenerateRandomPoints(rand, choose_A, "pointsA");
        //    GenerateRandomPoints(rand, choose_B, "pointsB");
        //    GenerateRandomPoints(rand, choose_C, "pointsC");

        //    noiseCompute.SetInt("frequencyA", choose_A);
        //    noiseCompute.SetInt("frequencyB", choose_B);
        //    noiseCompute.SetInt("frequencyC", choose_C);



        //}


    }

    void GenerateRandomPoints(System.Random rand, int numCells, string buffer)
    {
        Vector3[] points = new Vector3[(int)Math.Pow(numCells, 3)];
        
        for (int x = 0; x < numCells; x++)
        {
            for (int y = 0; y < numCells; y++)
            {
                for (int z = 0; z < numCells; z++)
                {
                    Vector3 randomPosition = new Vector3(
                        (float)rand.NextDouble(),
                        (float)rand.NextDouble(),
                        (float)rand.NextDouble());
                    int index = x + numCells * (y + z * numCells);
                    points[index] = (new Vector3(x, y, z) + randomPosition) / (float)numCells;
                }
            }
        }

        SetBuffer(points, sizeof(float) * 3, buffer);
    }

    ComputeBuffer SetBuffer(Array data, int stride, string bufferName)
    {
        var buffer = new ComputeBuffer(data.Length, stride, ComputeBufferType.Structured);
        buffer.SetData(data);
        buffers.Add(buffer);
        noiseCompute.SetBuffer(0, bufferName, buffer);
        return buffer;
    }

    RenderTexture CreateTexture(int size)
    {
        RenderTexture output = new RenderTexture(size, size, 0);
        output.wrapMode = TextureWrapMode.Repeat;
        output.filterMode = FilterMode.Bilinear;
        output.volumeDepth = size;
        output.enableRandomWrite = true;
        output.dimension = TextureDimension.Tex3D;
        output.graphicsFormat = GraphicsFormat.R16G16B16A16_UNorm;
        output.Create();
        return output;
    }

    public void ForceUpdate(NoiseSettings settings = null)
    {
        if (settings == null)
            settings = activeSettings;
        updateNoise = true;
        UpdateNoise(settings);
    }

    public bool OnSettingsChange()
    {
        if (activeTextureType != (NoiseType)activeSettings.type || activeChannel != (NoiseChannel)activeSettings.channel)
        {
            NoiseSettings settings;

            settings = GetSetting((int)activeTextureType, (int)activeChannel);
            if (settings == null)
            {
                return false;
            }
            activeSettings = settings;
            return true;
        }
        updateNoise = true;
        return false;
    }

    public void SaveSettings()
    {
        NoiseSettingsCollection save = new NoiseSettingsCollection(settingsList);
        string jsonString = JsonUtility.ToJson(save);
        System.IO.File.WriteAllText(Application.dataPath + "/Settings/" + SceneManager.GetActiveScene().name + ".json", jsonString);
    }

    public NoiseSettings[] LoadSettings()
    {
        string path;
        if (!choose_buttom)
        {
            path = Application.dataPath + "/Settings/" + SceneManager.GetActiveScene().name + ".json";
        }
        else
        {
            path = Application.dataPath + "/Settings/" + load_name + ".json";
        }
            
        NoiseSettings[] output;
        try
        {
            string contents = File.ReadAllText(path);
            output = JsonUtility.FromJson<NoiseSettingsCollection>(contents).settings;
        }
        catch (FileNotFoundException) { return null; }

        return output;
    }
}


[Serializable]
public class NoiseSettingsCollection
{
    public NoiseSettings[] settings;
    public NoiseSettingsCollection(NoiseSettings[] settings) {
        this.settings = settings;
    }
}

[Serializable]
public class NoiseSettings
{

    public int type;
    public int channel;
    public int seed;
    public float mix;
    public int frequencyA;
    public int frequencyB;
    public int frequencyC;

    //public Buttom test;

    public NoiseSettings Clone()
    {
        return JsonUtility.FromJson<NoiseSettings>(JsonUtility.ToJson(this));
    }

    public void Set(NoiseSettings settings)
    {
        type = settings.type;
        channel = settings.channel;
        seed = settings.seed;
        mix = settings.mix;
        frequencyA = settings.frequencyA;
        frequencyB = settings.frequencyB;
        frequencyC = settings.frequencyC;
        //frequencyA = 5;
        //frequencyB = 40;
        //frequencyC = 60;


    }
}
