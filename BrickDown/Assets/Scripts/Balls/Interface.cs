using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBall {

    bool isMoving { get; set; }
    void Shot( Vector3 pos );
}
