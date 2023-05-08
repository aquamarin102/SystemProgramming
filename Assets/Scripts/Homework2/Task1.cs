using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Homework2
{
    public class Task1 : MonoBehaviour
    {
        private NativeArray<int> intArray;
        
        private void Start()
        {
            intArray = new NativeArray<int>(new [] { 1, 2, 3, 50, 23, -35, 0 }, Allocator.Persistent);

            MyJob myJob = new MyJob()
            {
                INTArray = intArray
            };
            JobHandle jobHandle = myJob.Schedule();
            jobHandle.Complete();
        }

        private struct MyJob : IJob
        {
            public NativeArray<int> INTArray;

            public void Execute()
            {
                for (int i = 0; i < INTArray.Length; i++)
                {
                    if (INTArray[i] >= 10)
                    {
                        INTArray[i] = 0;
                    }

                    Debug.Log(INTArray[i]);
                }
            }
        }

        private void OnDestroy()
        {
            intArray.Dispose();
        }
    }
}