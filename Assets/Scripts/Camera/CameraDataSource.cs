using UnityEngine;
using DataSources;

namespace Camera
{
    [CreateAssetMenu(menuName = "Data/Sources/Camera", fileName = "Source_CameraData")]
    public class CameraSetupSource : DataSource<CameraSetup> { }
}