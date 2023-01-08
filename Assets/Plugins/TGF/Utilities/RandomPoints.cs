using System.Collections.Generic;
using UnityEngine;

namespace TGF.Utilities
{
    public class RandomPoints
    {
        private readonly float _minAngle;
        private readonly float _maxAngle;
        private readonly float _minMagnitude;
        private readonly float _maxMagnitude;
        private readonly float _pointRadius;
        private readonly int _maxIteration;
        public DirectionOverride directionOverride = DirectionOverride.None;

        private List<Vector3> GeneratedPointsCache = new List<Vector3>();
        public int TotalPointsGenerated => GeneratedPointsCache.Count;
        public int BadPointsCount { get; private set; } = 0;

        public enum DirectionOverride
        {
            Negative = -1, None = 0, Positive = 1
        }

        public RandomPoints(float minAngle, float maxAngle, float minMagnitude, float maxMagnitude, float pointRadius, int maxIteration = 5)
        {
            _minAngle = minAngle;
            _maxAngle = maxAngle;
            _minMagnitude = minMagnitude;
            _maxMagnitude = maxMagnitude;
            _pointRadius = pointRadius;
            _maxIteration = maxIteration;
        }

        public Vector3 GetNext()
        {
            Vector3 p = GetPoissonSampledPoint();
            GeneratedPointsCache.Add(p);
            return p;
        }

        private Vector3 GetPoissonSampledPoint()
        {
            Vector3 point = Vector3.zero;
            for (int i = 0; i < _maxIteration; i++)
            {
                point = GetRandomPointInPartialDisk();
                if (IsPointValid(point, _pointRadius)) 
                    return point;
            }

            BadPointsCount++;
            return point;
        }

        bool IsPointValid(Vector3 point, float radius)
        {
            for (int i = 0; i < GeneratedPointsCache.Count; i++)
            {
                float distance = Vector3.Distance(point, GeneratedPointsCache[i]);
                if (distance <= radius*2)
                {
                    return false;
                }
            }

            return true;
        }

        Vector3 GetRandomPointInPartialDisk()
        {
            float randomAngle = Random.Range(_minAngle, _maxAngle);
            float randomMag = Random.Range(_minMagnitude, _maxMagnitude);
            float side = directionOverride != DirectionOverride.None ? (int)directionOverride : RandomNumber.GetRandomPositiveNegative();
            
            Vector3 point = Quaternion.AngleAxis(randomAngle * side, Vector3.forward) * (Vector3.right * side);
            point = point.normalized * randomMag;
            return point;
        }
    }
}