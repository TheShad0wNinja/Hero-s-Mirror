using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float duration = 1f;
    public float speed = 0.1f;

    void Start()
    {
        Debug.Log("SPAWNING");
        // StartCoroutine(DeleteAfterX());
    }

    // void Update()
    // {
    //     transform.position += speed * Time.deltaTime * transform.forward ;
    // }

    // IEnumerator DeleteAfterX()
    // {
    //     yield return new WaitForSeconds(duration);
    //     Destroy(this.gameObject);
    // }
}