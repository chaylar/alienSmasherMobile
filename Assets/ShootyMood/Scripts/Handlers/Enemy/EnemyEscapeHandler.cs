using DG.Tweening;
using ShootyMood.Scripts.ShootyGameEvents;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;

namespace Assets.ShootyMood.Scripts.Handlers.Enemy
{
    public class EnemyEscapeHandler : MonoBehaviour
    {
        [SerializeField] private EnemyModel characterModel;
        [SerializeField] private GameObject escapeParticle;

        [Inject] private SignalBus signalBus;

        //
        private float dissapearTimer = 0;

        //TODO : Optimize!
        public void GenerateGoPositions(float minDistanceToPos, float xMax, float yMax)
        {
            var currentPos = transform.position;
            bool isOkDistance = false;
            Vector3 selectedPos = Vector3.zero;
            var yMin = yMax * -1;
            var xMin = xMax * -1;
            while (!isOkDistance)
            {
                float x = Random.Range(xMin, xMax);
                float y = Random.Range(yMin, yMax);
                selectedPos = new Vector3(x, y, currentPos.z);

                if(Vector3.Distance(currentPos, selectedPos) > minDistanceToPos)
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

            float totalDistance = 0f;
            for(int i = 0; i < pathList.Count; i++)
            {
                totalDistance += i == 0 ? Vector3.Distance(currentPos, pathList[0]) : Vector3.Distance(pathList[i - 1], pathList[i]);
            }

            float escapeTime = totalDistance * characterModel.EscapeTime;

            pathList.Add(selectedPos);
            transform.DOPath(pathList.ToArray(), escapeTime, PathType.CatmullRom).OnComplete(DestroyEnemy);
        }

        public void DoPath(Vector3[] path)
        {
            float totalDistance = 0f;
            for (int i = 0; i < path.Length; i++)
            {
                totalDistance += i == 0 ? Vector3.Distance(transform.position, path[0]) : Vector3.Distance(path[i - 1], path[i]);
            }

            float escapeTimeFinal = totalDistance * characterModel.EscapeTime;
            transform.DOPath(path, escapeTimeFinal, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(DestroyEnemy);
        }

        private void DestroyEnemy()
        {
            characterModel.IsDead = true;
            var position = gameObject.transform.position;

            if (escapeParticle != null)
            {
                var part = Instantiate(escapeParticle, position, Quaternion.identity);
                Destroy(part, .1f);
            }

            signalBus.Fire(new EnemyEscaped());
            Destroy(gameObject);
        }


        //void FixedUpdate()
        //{
        //    //if (characterModel.IsDead || characterModel.IsDying)
        //    //{
        //    //    return;
        //    //}

        //    //if(dissapearTimer >= characterModel.EscapeTime)
        //    //{
        //    //    characterModel.IsDead = true;
        //    //    var position = gameObject.transform.position;

        //    //    if (escapeParticle != null)
        //    //    {
        //    //        Instantiate(escapeParticle, position, Quaternion.identity);
        //    //    }

        //    //    signalBus.Fire(new EnemyEscaped());
        //    //    Destroy(gameObject);
        //    //}

        //    //dissapearTimer += Time.deltaTime;
        //}
    }
}
