using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : Singleton<SoundsController>
{
    public AudioSource source;

    public Dictionary<SoundId, AudioClip> clips = new Dictionary<SoundId, AudioClip>();

    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlaySound(SoundId id)
    {
        source.PlayOneShot(clips[id]);
    }
}

public enum SoundId
{
    BUTTON_1 = 1,
    CLASH_1 = 2,
    MOVE_1 = 3,
    MOVE_2 = 4,
    LOOT_PICKUP = 5,
}
