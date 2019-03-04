using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorPathScript : MonoBehaviour {
    public Color rayColor = Color.white;
    public List<Transform> path_objs = new List<Transform>();
    Transform[] arr;

    private void OnDrawGizmos() {
        Gizmos.color = rayColor;
        arr = GetComponentsInChildren<Transform>();
        path_objs.Clear();

        foreach (Transform x in arr) {
            if (x != this.transform) {
                path_objs.Add(x);
            }
        }

        for(int i = 0; i < path_objs.Count; i++) {
            Vector3 pos = path_objs[i].position;
            if (i > 0) {
                Vector3 previous = path_objs[i-1].position;
                Gizmos.DrawLine(previous, pos);
                Gizmos.DrawWireSphere(pos, 0.3f);
            }
        }
    }
}