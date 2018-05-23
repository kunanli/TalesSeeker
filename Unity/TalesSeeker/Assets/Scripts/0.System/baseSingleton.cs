using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// baseSingleton
/// ***Need addcomponent to GameObject by Youself
/// </summary>

public class baseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    Debug.Log("[Singleton]Init faild Manager Name : " +
                        _instance.name);
                }

                return _instance;
            }
        }
    }

    private void Awake()
    {
        lock (_lock)
        {
            if (_instance == null )
            {
                _instance = this.GetComponent<T>();
            }
        }

        doAwake();
    }

    // Use this for initialization
    private void Start()
    {
        doStart();
    }

    // Update is called once per frame
    private void Update()
    {
        doUpdate();
    }

    private void LateUpdate()
    {
        doLateUpdate();
    }

    private void FixedUpdate()
    {
        doFixedUpdate();
    }

    public virtual void doAwake() { }
    public virtual void doStart() { }
    public virtual void doUpdate() { }
    public virtual void doLateUpdate() { }
    public virtual void doFixedUpdate() { }

}
