using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackp1 : MonoBehaviour
{
    public Transform player1; // 你可以在编辑器中设置player1或通过代码找到它

    // 使用 FixedUpdate 更新物理位置的跟随
    void Update()
    {
        if(player1 != null)
        {
            // 设置 track 对象的位置等于 player1 的位置
            transform.position = player1.position;

            // 如果你还想要 track 对象有相同的旋转，则取消注释下面一行代码
            // transform.rotation = player1.rotation;
        }
    }
}
