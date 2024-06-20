using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SoundManager : MonoBehaviour
{

    //public AudioClip test;

    private JSONComplier SoundDataComplier;

    public SoundData[] soundDatas;

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public TextAsset[] JSONAsset;
    }

    private void Start()
    {

        SoundDataComplier = new JSONComplier();
    }

    public TextAsset clipAssigner(string type, string name)
    {
        SoundData soundData_temp = null;

        foreach (SoundData data in soundDatas)
        {
            if (data.name == (type + " SFX's"))
            {
                soundData_temp = data;
                Debug.Log("stage 1 clear!");
            }
        }

        if (soundData_temp == null)
        {
            Debug.LogWarning("The given SFX named: " + type + " not found");
        }
        else
        {
            foreach (TextAsset asset in soundData_temp.JSONAsset)
            {
                if (asset.name == name)
                {
                    Debug.Log("Stage 2 clear");
                    return asset;
                }
            }
        }


        return null;
    }

    public void TriggerAudioPlay(string inputStringData)

    {
        string[] SFX_DATA = inputStringData.Split('.');

        if (SFX_DATA.Length != 2)
        {
            Debug.LogWarning("error!");
        }

        string SFX_name = SFX_DATA[0];
        string SFX_type = SFX_DATA[1];

        TextAsset JSONAudioData = clipAssigner(SFX_type,SFX_name);

        SoundClipData soundClipData = SoundDataComplier.LoadAndCompileJSON(JSONAudioData);

        AudioClip clip = null;

        // Assuming soundClipData.filePath contains the absolute path
        string absolutePath = soundClipData.filePath;

        // Find the index of "Assets" in the absolute path
        int index = absolutePath.IndexOf("Assets");
        if (index != -1)
        {
            // Extract the portion of the path after "Assets"
            string relativePath = absolutePath.Substring(index);

            // Load the audio clip using the relative path
            clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);
        }
        else
        {
            Debug.LogError("Invalid file path: " + absolutePath);
        }



        soundClipData.audioSource = gameObject.AddComponent<AudioSource>();
        soundClipData.audioSource.hideFlags = HideFlags.HideInInspector;
        soundClipData.audioSource.clip = clip;
        soundClipData.audioSource.volume = soundClipData.volume;
        soundClipData.audioSource.pitch = soundClipData.pitch;
        //soundClipData.audioSource.Play();

        StartCoroutine(DelayAudioPlay(soundClipData));

        float destroyDuration = clip.length + 1f;

        Destroy(soundClipData.audioSource, destroyDuration);

        //test = clip;
    }

    private IEnumerator DelayAudioPlay(SoundClipData audioToPlayDELAYED)
    {


        yield return new WaitForSeconds(audioToPlayDELAYED.audioSource.clip.length + 100f);
        audioToPlayDELAYED.audioSource.Play();
    }
}
