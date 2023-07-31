using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.ShootyMood.Scripts.Handlers.Enemy;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ShootyMood.Scripts.Config.Wave;
using ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.Models;
using ShootyMood.Scripts.ShootyGameEvents;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ShootyMood.Scripts.Handlers
{
    public class GameController : MonoBehaviour, IInitializable, IDisposable, IFixedTickable
    {
        [SerializeField] private List<EnemyModel> enemyPrefabs;
        [SerializeField] private EnemyModel friendlyPrefab;

        [SerializeField] private GameObject spawnParticle;

        [Inject] private SignalBus signalBus;
        [Inject] private PositionSpawnService positionSpawnService;
        [Inject] private DiContainer diContainer;
        [Inject] private WavesConfig wavesConfig;
        [Inject] private SpawnPointConfig spConfig;
        [Inject] private PlayerHitHandler playerHealthHandler;
        
        //
        private int waveIndex = 0;
        private int currentWaveSpawnCount = 0;
        private int iterationSpawnCount = 1;
        private float timer;
        private float spawnDuration = 1f;
        private float mobAttackDurationDegradeRatio = 0f;
        private float mobEscapeDurationDegradeRatio = 0f;

        private float friendlySpawnRatio = 0f;
        private float timeAdditionOnDeath = 0f;

        private float screenMaxX;
        private float screenMaxY;
        private float minEscapeDistance;

        private void Start()
        {
            screenMaxX = Camera.main.orthographicSize * Camera.main.aspect;
            screenMaxY = Camera.main.orthographicSize;

            minEscapeDistance = screenMaxY / 3;
        }

        public void Initialize()
        {
            signalBus.Subscribe<PlayerKilled>(OnPlayerKilled);
        }
        
        public void Dispose()
        {
            signalBus.TryUnsubscribe<PlayerKilled>(OnPlayerKilled);
        }

        private void OnPlayerKilled(PlayerKilled evt)
        {
            waveIndex = 0;
            timer = 0;
            OrganizeCurrentWave(GetCurrentWave());
            playerHealthHandler.ResetHealth();
            positionSpawnService.ResetSpawns();
        }

        public void FixedTick()
        {
            if (GameStateManager.Instance.GetState() != GameStateManager.GameState.PLAY)
            {
                return;
            }
            
            timer += Time.deltaTime;
            if (timer >= spawnDuration)
            {
                for (int i = 0; i < iterationSpawnCount; i++)
                {
                    SpawnOnPositions().Forget();
                }
                
                timer = 0f;
            }
        }

        private SingleWaveConfig GetCurrentWave()
        {
            SingleWaveConfig result = null;
            if (wavesConfig.waveConfigs.Count > waveIndex && wavesConfig.waveConfigs[waveIndex] != null)
            {
                result = wavesConfig.waveConfigs[waveIndex];
            }

            return result;
        }

        private void OrganizeCurrentWave(SingleWaveConfig currentWave)
        {
            if (currentWave == null)
                return;

            spawnDuration = currentWave.spawnDuration;
            mobAttackDurationDegradeRatio = currentWave.mobAttackDurationDecreaseAmount;
            mobEscapeDurationDegradeRatio = currentWave.mobEscapeDurationDecreaseAmount;
            friendlySpawnRatio = currentWave.friendlyRatio;
            iterationSpawnCount = currentWave.iterationSpawnCount;
            timeAdditionOnDeath = currentWave.timeAdditionOnDeath;


            if (currentWaveSpawnCount > currentWave.waveSpawnCount)
            {
                waveIndex++;
                currentWaveSpawnCount = 0;
            }
        }

        private async UniTaskVoid SpawnOnPositions()
        {
            List<EnemySpawnPosition> availablePositions = positionSpawnService.GetAvailablePositions();
            if (availablePositions == null || availablePositions.Count == 0)
            {
                return;
            }
            
            availablePositions = availablePositions.OrderBy(x => Random.Range(-10.0f, 10.0f)).ToList();
            var sp = availablePositions[0];

            bool isSpawnFriendly = false;
            float ran = Random.Range(0f, 1f);
            if(ran <= friendlySpawnRatio && friendlyPrefab != null)
            {
                isSpawnFriendly = true;
            }

            float posDefX = Random.Range((-1 * spConfig.SpawnPosDeviation), spConfig.SpawnPosDeviation);
            float posDefY = Random.Range((-1 * spConfig.SpawnPosDeviation), spConfig.SpawnPosDeviation);
            var spawnPos = new Vector3((sp.X + posDefX), (sp.Y + posDefY));
            var pathList = GenerateGoPositions(spawnPos, minEscapeDistance, screenMaxX, screenMaxY);

            int selected = Random.Range(0, enemyPrefabs.Count);
            for (int i = 0; i < 5; i++)
            {
                EnemyModel newSpawn = null;
                float timeAddition = timeAdditionOnDeath;
                if (!isSpawnFriendly)
                {
                    newSpawn = diContainer.InstantiatePrefabForComponent<EnemyModel>(enemyPrefabs[selected]);
                }
                else
                {
                    newSpawn = diContainer.InstantiatePrefabForComponent<EnemyModel>(friendlyPrefab);
                    newSpawn.isFriendly = true;
                    timeAddition *= -2;
                }

                newSpawn.AttackDelayDecrementRatio = mobAttackDurationDegradeRatio;
                newSpawn.EscapeDelayDecrementRatio = mobEscapeDurationDegradeRatio;
                newSpawn.TimeAddition = timeAddition;
                newSpawn.gameObject.SetActive(false);

                InstSpawnParticle(spawnParticle, spawnPos);

                newSpawn.transform.position = spawnPos;
                positionSpawnService.SpawnOnPos(ref sp, newSpawn);

                //TODO : 
                newSpawn.GetComponent<EnemyEscapeHandler>().DoPath(pathList.ToArray());
                signalBus.Fire(new EnemySpawned());

                await UniTask.Delay((int)Math.Floor((newSpawn.EscapeTime * 1000f) / 2f));
            }

            currentWaveSpawnCount++;
            //
            OrganizeCurrentWave(GetCurrentWave());
        }

        private void InstSpawnParticle(GameObject origSpawnParticle, Vector3 pos)
        {
            if(origSpawnParticle == null)
            {
                return;
            }

            var particlePos = new Vector3(pos.x, pos.y, pos.z - 2);
            var spawnParticleObject = Instantiate(origSpawnParticle, particlePos, Quaternion.identity);
            var origSpawnParticleScale = spawnParticleObject.transform.localScale;
            spawnParticleObject.transform.localScale = Vector3.zero;
            
            spawnParticleObject.transform.DOScale(origSpawnParticleScale, spConfig.SpawnDuration - .1f);
            Destroy(spawnParticleObject, spConfig.SpawnDuration);
        }

        private List<Vector3> GenerateGoPositions(Vector3 spawnPos, float minDistanceToPos, float xMax, float yMax)
        {
            var currentPos = spawnPos;
            bool isOkDistance = false;
            Vector3 selectedPos = Vector3.zero;
            var yMin = yMax * -1;
            var xMin = xMax * -1;
            while (!isOkDistance)
            {
                float x = Random.Range(xMin, xMax);
                float y = Random.Range(yMin, yMax);
                selectedPos = new Vector3(x, y, currentPos.z);

                if (Vector3.Distance(currentPos, selectedPos) > minDistanceToPos)
                {
                    isOkDistance = true;
                }
            }

            List<Vector3> pathList = new List<Vector3>();
            int totalPoints = 2;
            float xDeviationMax = xMax / 4;
            float yDeviationMax = yMax / 8;
            Vector3 calculationDraw = (selectedPos - currentPos) / (totalPoints + 1);
            for (int i = 0; i < totalPoints; i++)
            {
                Vector3 pathPoint = Vector3.zero;

                float xDeviation = Random.Range(-1 * xDeviationMax, xDeviationMax);
                float yDeviation = Random.Range(-1 * yDeviationMax, yDeviationMax);

                pathPoint = i == 0 ? currentPos + new Vector3(calculationDraw.x + xDeviation, calculationDraw.y, 0f) : pathList[i - 1] + new Vector3(calculationDraw.x + xDeviation, calculationDraw.y + yDeviation, 0f);
                pathList.Add(pathPoint);
            }

            pathList.Add(selectedPos);
            //transform.DOPath(pathList.ToArray(), escapeTime, PathType.CatmullRom).OnComplete(DestroyEnemy);
            return pathList;
        }
    }
}