using JetBrains.Annotations;
using UnityEngine;

namespace EyeTextureRemapper.Runtime
{
    public class ETRSetting : MonoBehaviour
    {
        [CanBeNull] public Transform LeftEye;
        [CanBeNull] public Transform RightEye;
        [CanBeNull] public Transform LeftEyeHighlight;
        [CanBeNull] public Transform RightEyeHighlight;
    }
}