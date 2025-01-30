using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSources;

namespace Camera
{
    [CreateAssetMenu(menuName = "Data/Sources/Camera", fileName = "Source_CameraData")]
    public class GameManagerSource : DataSource<CameraSetup> { }
}