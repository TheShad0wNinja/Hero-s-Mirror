using UnityEngine;

public class Trap_test : MonoBehaviour
{
    public bool goToTest = false;
    bool didTeleport = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (didTeleport) return;

        if (other.CompareTag("Player"))    
        {
            if (goToTest)
                Scene_Manager.Instance.ChangeSceneAdditive("Traps_Test");
            else
                Scene_Manager.Instance.GoToPreviousSceneAdditive();

            didTeleport = true;
        }
    }
    // void OnParticleSystemStopped()
    // {
        
    // }
}