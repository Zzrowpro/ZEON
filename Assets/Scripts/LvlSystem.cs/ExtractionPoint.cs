using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
    [SerializeField] private float extractionTime = 5f;
    [SerializeField] private GameObject waypointLaser;
    [SerializeField] private Vector3 waypointLaserScale = new Vector3(5,5,5);
    [SerializeField ] private GameObject ship; 
    void OnEnable()  => GameEvents.ObjectivesCompleted += StartExtractionSequence;
    void OnDisable() => GameEvents.ObjectivesCompleted -= StartExtractionSequence;

    public void StartExtractionSequence()
    {
         StartCoroutine(ExtractionCoroutine());
    }

    private IEnumerator ExtractionCoroutine()
    {
        waypointLaser.SetActive(true);
        waypointLaser.transform.localScale = waypointLaserScale;
        yield return new WaitForSeconds(extractionTime);
        waypointLaser.transform.DOScale(0, 1f).OnComplete(() => {Destroy(waypointLaser);});
        ship.SetActive(true);
    }
}