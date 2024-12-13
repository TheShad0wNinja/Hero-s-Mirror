using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Drag_Camera : MonoBehaviour
{
    [SerializeField] private ParticleSystem beamMeUp;
    [SerializeField] private TextMeshProUGUI locationNameUI;
    public float dragSpeed = 2f;
    public float outerLeft = -10f;
    public float outerRight = 10f;
    public float outerTop = 10f;
    public float outerDown = -10f;
    public float startMoveSpeed = 2f;
    public float zoomSpeed = 2f;
    public float targetZoom = 5f;
    public float originZoom = 10f;
    public List<Transform> locations = new List<Transform>();
    public Vector3 targetStartPosition;
    private Vector3 dragOrigin;
    private Vector3 nextPosition;
    private bool hasReachedStart = false;
    private bool zoomOut = false;
    private string locationName = "Village";
    private int locationsUnlocked = 3;
    [SerializeField] GameObject unitAssemblyPanel;

    private void Start()
    {
        Transform initialLocation = locations[0];
        targetStartPosition = new Vector3(initialLocation.position.x, initialLocation.position.y, -10);
        transform.position = new Vector3(initialLocation.position.x, initialLocation.position.y, targetStartPosition.z);
        Camera.main.orthographicSize = targetZoom;
        locationNameUI.text = locationName;
        beamMeUp.transform.position = new Vector3(initialLocation.position.x, initialLocation.position.y, 0);
        beamMeUp.Clear();
        Invoke("PlayEffect", 0.2f);
    }

    public void TransferRevision(Transform newLocation, string newLocationName)
    {
        for (int i = 0; i <= locationsUnlocked; i++)
        {
            if (locations[i] == newLocation)
            {
                locationName = newLocationName;
                nextPosition = new Vector3(newLocation.position.x, newLocation.position.y, -10);
                zoomOut = true;
                break;
            }
        }
    }

    public void OpenNewLocation()
    {
        locationsUnlocked++;
        targetStartPosition = locations[locationsUnlocked].position;
    }

    void PlayEffect() 
    {
        beamMeUp.Play();
    }

    private void Update()
    {
        if (zoomOut)
        {
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, originZoom, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(Camera.main.orthographicSize - originZoom) < 0.01f)
            {
                targetStartPosition = nextPosition;
                hasReachedStart = false;
                zoomOut = false;
            }
            return;
        }

        if (!hasReachedStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetStartPosition, startMoveSpeed * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetStartPosition) < 0.01f && Mathf.Abs(Camera.main.orthographicSize - targetZoom) < 0.01f)
            {
                transform.position = targetStartPosition;
                Camera.main.orthographicSize = targetZoom;
                hasReachedStart = true;
                beamMeUp.transform.position = new Vector3(targetStartPosition.x, targetStartPosition.y, 0);
                beamMeUp.Clear();
                Invoke("PlayEffect", 0.2f);
                locationNameUI.text = locationName;
            }
            return;
        }
        if (!unitAssemblyPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
                dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 difference = dragOrigin - currentPos;
                Vector3 newPosition = transform.position + difference;

                newPosition.x = Mathf.Clamp(newPosition.x, outerLeft, outerRight);
                newPosition.y = Mathf.Clamp(newPosition.y, outerDown, outerTop);

                transform.position = newPosition;
            }
        }
    }
}
