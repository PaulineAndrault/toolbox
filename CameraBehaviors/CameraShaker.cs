using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] Transform _cam;

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = _cam.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _cam.localPosition += new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ne pas faire de retour à l'origine si la camera Follow le joueur
        //_cam.localPosition = originalPos;
    }
}
