using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraScript : MonoBehaviour
{
    [SerializeField] 
    Camera mainCamera;
    [SerializeField]
    CinemachineFreeLook freeLookCamera;
    [SerializeField]
    CinemachineVirtualCamera lockonCameral;
    readonly int LockonCameraActivePriority = 11;
    readonly int LockonCameraInactivePriority = 0;

    //カメラの角度をプレイヤーにリセット
    public void ResetFreeLookCamera()
    {

    }

    //ロックオン時のVirtualCamera切り替え
    public void ActiveLockonCamera(GameObject  target)
    {
        lockonCameral.Priority = LockonCameraActivePriority;
        lockonCameral.LookAt = target.transform;
    }

    //ロックオン解除時のVirtualCamera切り替え
    public void InactiveLockonCamera()
    {
        lockonCameral.Priority = LockonCameraInactivePriority;
        lockonCameral.LookAt = null;
    }

    public Transform GetLookAtTransform()
    {
        return lockonCameral.LookAt.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
