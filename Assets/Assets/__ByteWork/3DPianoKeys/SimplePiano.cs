using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePiano : MonoBehaviour
{
    /// <summary>
    /// 每组键盘的父节点
    /// </summary>
    public List<Transform> OctaveGroupRoot = new List<Transform>();
    /// <summary>
    /// 钢琴的所有键
    /// </summary>
    public List<SimplePianoKey> PianoKeys = new List<SimplePianoKey>();
    public List<AudioClip> Notes = new List<AudioClip>();
    public KeyMode KeyMode = KeyMode.Physical;
    public Dictionary<string, SimplePianoKey> PianoNotes = new Dictionary<string, SimplePianoKey>();

    public float PedalReleasedAngle=-90;        // Local angle that a pedal is considered to be released, or off.
    public float PedalPressedAngle=-99;         // Local angle that a pedal is considered to be pressed, or on.
    public float SustainSeconds = 5;        // May want to reduce this if there's too many AudioSources being generated per key.
    public float PressAngleThreshold = 355f;// Rate of keys being slowly released.
    public float PressAngleDecay = 0.5f;		// Rate of keys being slowly released.

    public bool SustainPedalPressed = true; // When enabled, keys will not stop playing immediately after release.
    public bool KeyPressAngleDecay = true;  // When enabled, keys will slowly be released.
    public bool RepeatedKeyTeleport = true; // When enabled, during midi mode, a note played on a pressed key will force the rotation to reset.
    public bool NoMultiAudioSource=false;			// Will prevent duplicates if true, if you need to optimise. Multiple Audio sources are necessary to remove crackling.

    /// <summary>
    /// If the first key is not "A", change it to the appropriate note.
    /// </summary>
    public string StartKey = "A";
    /// <summary>
    ///  Start Octave can be increased if the piano/keyboard is not full length. 
    /// </summary>
    public int StartOctave=1;

    private List<string> KeyNames = new List<string>();
    private readonly string[] _keyIndex = 
        new string[12] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    private void Awake()
    {
        for (int id = 0; id < PianoKeys.Count; id++)
        {
            SimplePianoKey spk = PianoKeys[id];
            AudioSource keyAudioSource = spk.GetComponent<AudioSource>();

            if (keyAudioSource)
            {
                keyAudioSource.clip = Notes[id];
                string keyName = KeyString(id + Array.IndexOf(_keyIndex, StartKey));
                //keyName: B8, pianoKey:PianoKey.087
                //Debug.Log($"count:{count}, keyName: {keyName}, pianoKey:{pianoKey.gameObject.name}");
                PianoNotes.Add(keyName, spk);
                KeyNames.Add(keyName);

                spk.SimplePiano = this;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string KeyString(int count)
    {
        return _keyIndex[count % 12] + (Mathf.Floor(count / 12) + StartOctave);
    }
}
