using ShootyMood.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ShootyMood.Scripts.Managers
{
    public class AudioOptionManager
    {

        public bool AUDIO_ON = true;

        private AudioOptionManager()
        {
        }

        private static readonly object stLock = new object();
        private static AudioOptionManager instance = null;
        public static AudioOptionManager Instance
        {
            get
            {
                lock (stLock)
                {
                    if (instance == null)
                    {
                        instance = new AudioOptionManager();
                    }
                    return instance;
                }
            }
        }
    }
}
