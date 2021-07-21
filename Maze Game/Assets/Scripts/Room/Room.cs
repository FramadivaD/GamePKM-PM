using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;

    public void Initialize()
    {
        topDoor.SetActive(true);
        rightDoor.SetActive(true);
        bottomDoor.SetActive(true);
        leftDoor.SetActive(true);
    }

    public void OpenTopDoor()
    {
        topDoor.SetActive(false);
    }

    public void OpenRightDoor()
    {
        rightDoor.SetActive(false);
    }

    public void OpenBottomDoor()
    {
        bottomDoor.SetActive(false);
    }

    public void OpenLeftDoor()
    {
        leftDoor.SetActive(false);
    }

    public int GetUnlockedDoorCount()
    {
        int val = 0;
        val += topDoor.activeSelf ? 1 : 0;
        val += rightDoor.activeSelf ? 1 : 0;
        val += bottomDoor.activeSelf ? 1 : 0;
        val += leftDoor.activeSelf ? 1 : 0;

        return val;

    }
}
