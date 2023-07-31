using System;
using System.Collections.Generic;
using ShootyMood.Scripts.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace ShootyMood.Scripts.Config.Wave
{
    [CreateAssetMenu(fileName = "WavesConfig", menuName = "SlimeHunter/Configs/WavesConfig")]
    public class WavesConfig : ScriptableObject
    {
        [SerializeField] private float startLeftTime = 30;
        [SerializeField] private int scoreAddition = 1;
        [SerializeField] public List<SingleWaveConfig> waveConfigs;

        public float StartLeftTime => startLeftTime;
        public int ScoreAddition => scoreAddition;
    }

    [Serializable]
    public class SingleWaveConfig
    {
        public int waveSpawnCount = 10;
        public float spawnDuration = 1f;
        public int iterationSpawnCount = 1;
        public float timeAdditionOnDeath = 5f;

        [Range(0, .9f)]
        public float friendlyRatio = .1f;

        [Range(0, .9f)]
        public float mobAttackDurationDecreaseAmount = 0f;

        [Range(0, .9f)]
        public float mobEscapeDurationDecreaseAmount = 0f;
    }
}