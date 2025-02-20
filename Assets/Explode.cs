using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public Vector3 finalExplosionScale;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(explosion());
    }

    IEnumerator explosion()
    {
        Vector3 startingScale = transform.localScale; 
        float elapsedTime = 0f, duration = 0.75f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startingScale, finalExplosionScale, elapsedTime / duration);
        } 
        transform.localScale = finalExplosionScale;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
