using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveActionEvent : MonoBehaviour
{
    [SerializeField]
    private CreateSwordTrail swordTrail;

    public void StartSwordTrail()
    {
        swordTrail.SetSwordTrail(true);
    }

    public void EndSwordTrail()
    {
        swordTrail.SetSwordTrail(false);
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
