using UnityEngine;

public class RandomAnimationOffset : MonoBehaviour
{
    void Start() 
    {
        var anim = GetComponent<Animator>();
        anim.SetFloat("offset", Random.Range(0f, 1f));
    }
}
