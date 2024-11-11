using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSourceFactory
{
    private readonly int poolSize = 15;
    private Transform parent;
    private List<AudioSource> sources;

    public AudioSourceFactory(Transform parent)
    {
        this.parent = parent;
        sources = new(poolSize);
    }

    public AudioSource Get(float volume)
    {
        var source = sources.FirstOrDefault(x => !x.isPlaying);

        if (source == null)
        {
            if (sources.Count >= poolSize)
            {
                return null;
            }

            source = parent.AddComponent<AudioSource>();
            sources.Add(source);
        }

        source.volume = volume;
        return source;
    }
}