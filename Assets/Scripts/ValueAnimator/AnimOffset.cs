using UnityEngine;


public class AnimOffset : MonoBehaviour
{
    public float start;
    
    public GameObject[] gameObjects;


    private void Start()
    {
        int count = gameObjects.Length;
        for (int i = 0; i < count; i++)
        {
            ValueAnimator[] animators = gameObjects[i].GetComponents<ValueAnimator>();
            int animCount = animators.Length;
            for (int e = 0; e < animCount; e++)
                animators[e].SetAnimOffset(this);
        }
    }
}
