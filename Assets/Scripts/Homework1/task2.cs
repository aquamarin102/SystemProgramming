using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class task2 : MonoBehaviour
{

    private CancellationTokenSource cancellationTokenSource;
    void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;
        
        Task1(cancelToken);
        Task2(cancelToken);
        
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();
    }

    private async Task Task1(CancellationToken token)
    {
        if(token.IsCancellationRequested)
            return;
        
        await Task.Delay(1000);
        Debug.Log("Task1 end.");
    }
    
    private async Task Task2(CancellationToken token)
    {
        if(token.IsCancellationRequested)
            return;
        
        
        for (int i = 0; i < 60; i++)
        {
            await Task.Yield();
        }
        
        Debug.Log("Task2 end.");
    }
}
