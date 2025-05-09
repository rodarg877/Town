using UnityEngine;

public class KnifeAnimationBridge : MonoBehaviour
{
    public Knife knife;

    public void PerformAttack()
    {
        knife.PerformAttack(); 
    }

}
