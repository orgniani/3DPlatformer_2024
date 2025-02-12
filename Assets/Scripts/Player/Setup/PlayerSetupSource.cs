using UnityEngine;
using DataSources;

namespace Player.Setup
{
    [CreateAssetMenu(menuName = "Data/Sources/PlayerSetup", fileName = "Source_PlayerSetupData")]
    public class PlayerSetupSource : DataSource<PlayerSetup> { }
}