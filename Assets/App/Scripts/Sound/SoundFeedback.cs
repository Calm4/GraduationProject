using System;
using UnityEngine;

namespace App.Scripts.Sound
{
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField]
        private AudioClip clickSound, placeSound, removeSound, wrongPlacementSound;

        [SerializeField]
        private AudioSource audioSource;

        public void PlaySound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Click:
                    audioSource.PlayOneShot(clickSound);
                    break;
                case SoundType.Place:
                    audioSource.PlayOneShot(placeSound);
                    break;
                case SoundType.Remove:
                    audioSource.PlayOneShot(removeSound);
                    break;
                case SoundType.WrongPlacement:
                    audioSource.PlayOneShot(wrongPlacementSound);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
        }
    }

    public enum SoundType
    {
        Click,
        Place,
        Remove,
        WrongPlacement
    }
}