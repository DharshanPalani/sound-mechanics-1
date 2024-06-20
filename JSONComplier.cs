using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JSONComplier
{

    //public TextAsset temp;

    //private void Start()
    //{
    //    LoadAndCompileJSON(temp);
    //}

    // Load JSON file, parse it, and compile audio clips
    public SoundClipData LoadAndCompileJSON(TextAsset JSON)
    {
        // Read the JSON file
        string jsonToString = JSON.text;

        Debug.Log(jsonToString);

        SoundClipData jsonCompliedData = JsonConvert.DeserializeObject<SoundClipData>(jsonToString);

        return jsonCompliedData;

        //Debug.Log(audioClipData.clipName);
    }
}

public class SoundClipData
{
    public string clipName;
    public float volume;
    public float pitch;
    public string filePath;
    public AudioSource audioSource;
}