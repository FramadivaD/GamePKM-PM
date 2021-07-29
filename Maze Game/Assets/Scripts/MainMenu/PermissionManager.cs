using UnityEngine;
using UnityEngine.Android;

using System.Collections;

public class PermissionManager : MonoBehaviour
{
    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)
            || !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)
            || !Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            string[] perm = new string[] { Permission.ExternalStorageRead, Permission.ExternalStorageWrite, Permission.Camera };
            Permission.RequestUserPermissions(perm);
        }
    }
}
