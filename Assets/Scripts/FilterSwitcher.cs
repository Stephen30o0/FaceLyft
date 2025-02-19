using UnityEngine;

public class FilterSwitcher : MonoBehaviour
{
    public GameObject[] filters; // Assign filters in Inspector
    private int currentFilter = 0;

    public void ChangeFilter(int index)
    {
        foreach (GameObject filter in filters)
        {
            filter.SetActive(false);
        }
        filters[index].SetActive(true);
    }
}
