using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsServer;
    private NetWork.Server server;
    private NetWork.Client client;
    private void Awake()
    {
        if (!( Application.platform == RuntimePlatform.WindowsEditor))
        {
            IsServer = true;
        }
    }
    void Start()
    {
        Application.runInBackground = true;
       if (IsServer == true)
        {
            server = new NetWork.Server(6666);
        }
       else
        {
            client = new NetWork.Client("127.0.0.1",6666);
        }
    }
    private void Update()
    {
      

    }
}
