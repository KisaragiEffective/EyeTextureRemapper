using System;
using System.Linq;
using System.Reflection;
using EyeTextureRemapper.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDK3.Avatars.Components;

namespace EyeTextureRemapper.Editor
{
    [CustomEditor(typeof(ETRSetting))]
    public class ETRSettingEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            // ReSharper disable once LocalVariableHidesMember
            var target = this.target as ETRSetting;
            if (target == null)
            {
                Debug.Log("target is not ETRSetting");
                return root;
            }

            var transform = typeof(Transform);
            root.Add(new ObjectField
            {
                bindingPath = nameof(ETRSetting.LeftEye), 
                label = "左目", 
                objectType = target.GetType()
                    .GetField(nameof(ETRSetting.LeftEye))!.FieldType,
            });
            root.Add(new ObjectField
            {
                bindingPath = nameof(ETRSetting.LeftEyeHighlight),
                label = "左目瞳孔",
                objectType = target.GetType()
                    .GetField(nameof(ETRSetting.LeftEyeHighlight))!.FieldType
            });
            root.Add(new ObjectField
            {
                bindingPath = nameof(ETRSetting.RightEye),
                label = "右目",
                objectType = target.GetType()
                    .GetField(nameof(ETRSetting.RightEye))!.FieldType
            });
            root.Add(new ObjectField
            {
                bindingPath = nameof(ETRSetting.RightEyeHighlight), 
                label = "右目瞳孔",
                objectType = target.GetType()
                    .GetField(nameof(ETRSetting.RightEyeHighlight))!.FieldType
            });
            var b = new Button(onClickDetectButton);
            b.Add(new Label("両目を検出"));
            root.Add(b);

            return root;
        }

        // TODO: もしVRChat SDKが存在しないのであればこの関数ごと潰す？
        private void onClickDetectButton()
        {
            var go = (this.target as ETRSetting)!.gameObject;
            var f = go.TryGetComponent<VRCAvatarDescriptor>(out var e);
            if (!f)
            {
                return;
            }

            if (!e.gameObject.TryGetComponent(out Animator animator))
            {
                throw new Exception("ill-setup Avatar Descriptor, please contact to avatar author: Animator is not attached on the GameObject which has VRCAvatarDescriptor");
            }

            if (!animator.isHuman)
            {
                throw new Exception("this tool only recognizes humanoid rig!");
            }

            #region left eye
            {
                // ボーンを取得
                var theBone = animator.GetBoneTransform(HumanBodyBones.LeftEye)!;
                var headSMR = e.customEyeLookSettings.eyelidsSkinnedMesh;
                var headMesh = headSMR.sharedMesh;
                var totalPolygonCount = headMesh.vertexCount;
                var boneIndex = 0;
                var boneWeight = headMesh.boneWeights[boneIndex];
                // TODO: ウェイトが非零のポリゴンを集める
                var affectedPolygonsByLeftEye = Enumerable.Range(0, totalPolygonCount - 1); // .Select(null);
                // TODO: UVの取得
                // TODO: UVが楕円に近似できなければ警告を送出？ (例: UV展開の面積とUVを内包する最小楕円体の面積があまりにも乖離している場合)
                //                                                                ^~~~~~~ そもそも最小楕円体の計算どうやるの、だれか助けて
            }
            #endregion
            
            // TODO: Right boneも同様に
            // TODO: 瞳孔も検出できたほうが親切かもしれない。MMD互換のシェイプキーで検知できたら良いかも？
        }
    }
}