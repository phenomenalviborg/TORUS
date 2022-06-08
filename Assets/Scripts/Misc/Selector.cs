using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;     
#endif

public class Selector : MonoBehaviour
{
    public GameObject[] select;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Select(0);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            Select(1);
        if(Input.GetKeyDown(KeyCode.Alpha3))
            Select(2);
        if(Input.GetKeyDown(KeyCode.Alpha4))
            Select(3);
        if(Input.GetKeyDown(KeyCode.Alpha5))
            Select(4);
        if(Input.GetKeyDown(KeyCode.Alpha6))
            Select(5);
        if(Input.GetKeyDown(KeyCode.Alpha7))
            Select(6);
        if(Input.GetKeyDown(KeyCode.Alpha8))
            Select(7);
        if(Input.GetKeyDown(KeyCode.Alpha9))
            Select(8);
    }


    private void Select(int id)
    {
        if(id >= select.Length)
            return;
        
        #if UNITY_EDITOR
        Selection.activeObject = @select[id];
        #endif
    }
}
