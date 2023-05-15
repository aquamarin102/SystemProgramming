using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Homework2
{
    public class Task2 : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private const int NaxPositionRadius = 10;
        private const int MaxVelocityRadius = 10;
        private const int ArrayLength = 10;
        private void Start()
        {
            _button.onClick.AddListener(Handle);
        }

        private void FillArrayRandom(NativeArray<Vector3> array, int maxValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.insideUnitSphere * maxValue;
            }
        }

        private void PrintArray(NativeArray<Vector3> position, NativeArray<Vector3> velocities,
            NativeArray<Vector3> finalPositions)
        {
            for (int i = 0; i < position.Length; i++)
            {
                Debug.Log($"index {i}; position {position[i]}; velocity {velocities[i]}; final {finalPositions[i]}");
            }
        }

        private void Handle()
        {
            NativeArray<Vector3> positions = new NativeArray<Vector3>(ArrayLength, Allocator.Persistent);
            NativeArray<Vector3> velocities = new NativeArray<Vector3>(ArrayLength, Allocator.Persistent);
            NativeArray<Vector3> finalPositions = new NativeArray<Vector3>(ArrayLength, Allocator.Persistent);
            FillArrayRandom(positions, NaxPositionRadius);
            FillArrayRandom(velocities, MaxVelocityRadius);

            Debug.Log("Start job...");
            MyJob job = new MyJob();
            job.positions = positions;
            job.velocities = velocities;
            job.finalPositions = finalPositions;
            JobHandle handle = job.Schedule(ArrayLength, 0);
            handle.Complete();
            Debug.Log("Job Complete.");

            PrintArray(positions, velocities, finalPositions);

            positions.Dispose();
            velocities.Dispose();
            finalPositions.Dispose();
        }

        internal struct MyJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Vector3> positions;
            [ReadOnly] public NativeArray<Vector3> velocities;
            [WriteOnly] public NativeArray<Vector3> finalPositions;

            public void Execute(int index)
            {
                finalPositions[index] = positions[index] + velocities[index];
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}