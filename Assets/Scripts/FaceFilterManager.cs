using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceFilterManager : MonoBehaviour
{
    private ARFaceManager faceManager;
    private ARSession arSession;
    public GameObject[] filters;

    void Start()
    {
        faceManager = GetComponent<ARFaceManager>();
        arSession = FindObjectOfType<ARSession>();

        if (filters.Length > 0)
        {
            ChangeFaceFilter(0);
        }
    }

    public void ChangeFaceFilter(int index)
    {
        if (index >= 0 && index < filters.Length)
        {
            faceManager.facePrefab = filters[index];
            ForceFaceRedetection();
        }
    }

    private void ForceFaceRedetection()
    {
        arSession.enabled = false;
        arSession.enabled = true;
    }
}
