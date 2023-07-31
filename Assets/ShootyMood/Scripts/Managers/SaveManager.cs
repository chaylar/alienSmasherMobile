using UnityEngine;

namespace Assets.ShootyMood.Scripts.Managers
{
    public class SaveManager
    {

        private static readonly string lastScoreKey = "LastScore";
        private static readonly string bestScoreKey = "BestScore";

        public static void SaveScore(int score)
        {
            PlayerPrefs.SetInt(lastScoreKey, score);
            if (PlayerPrefs.HasKey(bestScoreKey))
            {
                int bestScore = PlayerPrefs.GetInt(bestScoreKey);
                if(score > bestScore)
                {
                    PlayerPrefs.SetInt(bestScoreKey, score);
                }
            }
            else
            {
                PlayerPrefs.SetInt(bestScoreKey, score);
            }

            PlayerPrefs.Save();
        }

        public static int GetLastScore()
        {
            int result = 0;
            if (PlayerPrefs.HasKey(lastScoreKey))
            {
                result = PlayerPrefs.GetInt(lastScoreKey);
            }

            return result;
        }

        public static int GetBestScore()
        {
            int result = 0;
            if (PlayerPrefs.HasKey(bestScoreKey))
            {
                result = PlayerPrefs.GetInt(bestScoreKey);
            }

            return result;
        }

    }
}
