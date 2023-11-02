using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioPlaySample
{
    public class HowToUseIt : MonoBehaviour
    {
        public List<string> filename;
        private int count = 0;
        ExternalAudioManager eam;

        // Start is called before the first frame update
        void Start()
        {
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            eam = this.gameObject.AddComponent<ExternalAudioManager>();
            eam.AudioFileNames = new List<string>(filename);
            eam.audioSource = audioSource;
            play();
        }

        public void play()
        {
            eam.GetAudio(count, play, true);
            count++;
        }
    }
}