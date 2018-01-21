using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSound : MonoBehaviour
{
    [SerializeField] GameObject[] clickers;
    [SerializeField] AudioClip sound;
    [SerializeField] AudioSource source;

    void Awake()
    {
        int length = clickers.Length;
        for (int i = 0; i < length; i++)
        {
            if (clickers[i].GetComponent<EventTrigger>() == null)
            {
                EventTrigger trigger = clickers[i].AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) =>
                {
                    PlaySound();
                });
                trigger.triggers.Add(entry);
            }
        }
    }

    void PlaySound()
    {
        source.PlayOneShot(sound);
    }

}
