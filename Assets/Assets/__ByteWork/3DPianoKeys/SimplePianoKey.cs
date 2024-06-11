using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SimplePianoKey : MonoBehaviour
{
    public List<AudioSource> AudioSources { get; set; }
    public AudioSource CurrentAudioSource { get; set; }
    public SimplePiano SimplePiano { get; set; }

    public bool Sustain { get; set; }
    public float SustainSeconds { get; set; }
    public Material Material { get; set; }

    private bool _play = false;
    private bool _played = false;
    private float _velocity;
    private float _length;
    private float _speed;
    private Color _colour;
    private Color _originalColour;
    private float _Timer;
    private float _keyAngle = 360f;

    private List<AudioSource> _toFade = new List<AudioSource>();

    private bool _depression;
    private float _startAngle;

    // Debug
    public bool TestPlay = false;

    void Awake()
    {
        AudioSources = new List<AudioSource>();
        AudioSources.Add(GetComponent<AudioSource>());
        CurrentAudioSource = AudioSources[0];

        Material = GetComponentInChildren<MeshRenderer>().material;
        _originalColour = Material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_play)
        {
            KeyPlayMechanics();
        }

        if (SimplePiano.KeyMode == KeyMode.Physical)
        {
            if (transform.eulerAngles.x > 350 && transform.eulerAngles.x < 359.5f && !_played)
            {
                if (CurrentAudioSource.clip)
                    StartCoroutine(PlayPressedAudio());

                _played = true;

                if (_toFade.Count > 0)
                {
                    FadeList();
                }
            }
            else if (transform.eulerAngles.x > 359.9 || transform.eulerAngles.x < 350)
            {
                FadeAll();

                _played = false;
            }
        }
        else if (SimplePiano.KeyMode == KeyMode.ForShow)
        {
            if (_Timer >= 1)
            {
                FadeAll();
            }

            if (_toFade.Count > 0)
            {
                FadeList();
            }
        }

        // Debug
        if (TestPlay)
        {
            Play();
            TestPlay = false;
        }
    }

    void KeyPlayMechanics()
    {
        if (_Timer < 1)
        {
            transform.DOLocalRotate(new Vector3(3.8f,0,0), 0.5f);

            if (transform.eulerAngles.x > 1)
            {
                if (SimplePiano.KeyPressAngleDecay && _depression && transform.eulerAngles.x > SimplePiano.PressAngleThreshold
                    || !SimplePiano.KeyPressAngleDecay && transform.eulerAngles.x < _keyAngle)
                {
                    _keyAngle = transform.eulerAngles.x;
                }
                else
                {
                    if (transform.eulerAngles.x <= SimplePiano.PressAngleThreshold)
                        _depression = false;

                    transform.rotation = Quaternion.Euler(_keyAngle, transform.eulerAngles.y, transform.eulerAngles.z);

                    if (SimplePiano.KeyPressAngleDecay && !_depression && transform.eulerAngles.x < 359.5f)
                        _keyAngle += Time.deltaTime * SimplePiano.PressAngleDecay;
                }
            }

            Material.color = Color.Lerp(_colour, _originalColour, _Timer);

            _Timer += Time.deltaTime / _length * _speed;
        }
        else
        {
            Material.color = _originalColour;
            transform.DOLocalRotate(Vector3.zero, 0.5f);
            _play = false;
        }
    }

    void FadeAll()
    {
        if (_toFade.Count > 0)
            _toFade.RemoveRange(0, _toFade.Count);

        foreach (var audioSource in AudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.volume -= Time.deltaTime / (SimplePiano.SustainPedalPressed ? SimplePiano.SustainSeconds : 1f);

                if (audioSource.volume <= 0)
                    audioSource.Stop();
            }
        }
    }

    void FadeList()
    {
        for (int i = 0; i < _toFade.Count; i++)
        {
            if (_toFade[i].isPlaying)
            {
                _toFade[i].volume -= Time.deltaTime * 2;

                if (_toFade[i].volume <= 0)
                {
                    _toFade[i].volume = 0;
                    _toFade[i].Stop();
                    _toFade.Remove(_toFade[i]);
                    break;
                }
            }
        }
    }

    public void Play(float velocity = 10, float length = 1, float speed = 1)
    {
        _keyAngle = 360f;

        if (_play)
        {
            if (SimplePiano.RepeatedKeyTeleport)
            {
                transform.rotation = Quaternion.Euler(_keyAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else
            {
                //_rigidbody.AddTorque(Vector3.right * 127);
            }
        }

        _velocity = velocity;
        _length = length;
        _speed = speed;
        _Timer = 0;
        _play = true;
        _depression = true;

        if (SimplePiano.KeyMode == KeyMode.ForShow)
            PlayVirtualAudio();
    }

    public void Play(Color colour, float velocity = 10, float length = 1, float speed = 1)
    {
        _colour = colour;
        this.Play(velocity, length, speed);
    }

    IEnumerator PlayPressedAudio()
    {
        if (!SimplePiano.NoMultiAudioSource && CurrentAudioSource.isPlaying)
        {
            bool foundReplacement = false;
            int index = AudioSources.IndexOf(CurrentAudioSource);

            for (int i = 0; i < AudioSources.Count; i++)
            {
                if (i != index && (!AudioSources[i].isPlaying || AudioSources[i].volume <= 0))
                {
                    foundReplacement = true;
                    CurrentAudioSource = AudioSources[i];
                    _toFade.Remove(AudioSources[i]);
                    break;
                }
            }

            if (!foundReplacement)
            {
                AudioSource newAudioSource = CloneAudioSource();
                AudioSources.Add(newAudioSource);
                CurrentAudioSource = newAudioSource;
            }

            _toFade.Add(AudioSources[index]);
        }

        _startAngle = transform.eulerAngles.x;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        if (Mathf.Abs(_startAngle - transform.eulerAngles.x) > 0)
        {
            CurrentAudioSource.volume = Mathf.Lerp(0, 1, Mathf.Clamp((Mathf.Abs(_startAngle - transform.eulerAngles.x) / 2f), 0, 1));
        }

        CurrentAudioSource.Play();
    }

    void PlayVirtualAudio()
    {
        if (!SimplePiano.NoMultiAudioSource && CurrentAudioSource.isPlaying)
        {
            bool foundReplacement = false;
            int index = AudioSources.IndexOf(CurrentAudioSource);

            for (int i = 0; i < AudioSources.Count; i++)
            {
                if (i != index && (!AudioSources[i].isPlaying || AudioSources[i].volume <= 0))
                {
                    foundReplacement = true;
                    CurrentAudioSource = AudioSources[i];
                    _toFade.Remove(AudioSources[i]);
                    break;
                }
            }

            if (!foundReplacement)
            {
                AudioSource newAudioSource = CloneAudioSource();
                AudioSources.Add(newAudioSource);
                CurrentAudioSource = newAudioSource;
            }

            _toFade.Add(AudioSources[index]);
        }

        CurrentAudioSource.volume = _velocity / 127f;

        CurrentAudioSource.Play();
    }

    AudioSource CloneAudioSource()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.volume = CurrentAudioSource.volume;
        newAudioSource.playOnAwake = CurrentAudioSource.playOnAwake;
        newAudioSource.spatialBlend = CurrentAudioSource.spatialBlend;
        newAudioSource.clip = CurrentAudioSource.clip;
        newAudioSource.outputAudioMixerGroup = CurrentAudioSource.outputAudioMixerGroup;

        return newAudioSource;
    }
}
