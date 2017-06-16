using UnityEngine;
using UnityEditor;


namespace XcodeBuilder
{
    [CustomEditor(typeof(ProjectBuilder))]
    public class BuilderWindow : EditorWindow {

        private readonly ProjectBuildData m_BuildData = new ProjectBuildData();

        [MenuItem ("Window/Package Build Window")]
        public static void Init()
        {
            BuilderWindow window = (BuilderWindow)EditorWindow.GetWindow(typeof(BuilderWindow));

            window.minSize = new Vector2(360, 300);
            window.maxSize = new Vector2(360, 300);

            window.ShowUtility();
        }

        public float rotationAmount = 0.33F;
        
        private void RandomizeSelected()
        {
            foreach (var transform in Selection.transforms)
            {
                Quaternion rotation = Random.rotation;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotationAmount);
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical(GUILayout.Width(110));
            GUILayout.Space(20);


            //ScriptingImplementation
            m_BuildData.ScriptingImpl = (ScriptingImpl)EditorGUILayout.EnumPopup(m_BuildData.ScriptingImpl, GUILayout.Width(100));

            //Platform Architecture
            GUILayout.Space(10);
            m_BuildData.Architecture = (ArchitectureType)EditorGUILayout.EnumPopup(m_BuildData.Architecture, GUILayout.Width(100));
        
        
            //Platform Architecture
            GUILayout.Space(10);
            m_BuildData.Channel = (PackageChannel)EditorGUILayout.EnumPopup(m_BuildData.Channel, GUILayout.Width(100));

            //Identify Version
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            m_BuildData.IdentityVersion = EditorGUILayout.TextField(m_BuildData.IdentityVersion, GUILayout.Width(100));

            if (string.IsNullOrEmpty(m_BuildData.IdentityVersion))
            {
                m_BuildData.IdentityVersion = "1.0.0";
            }
            GUILayout.EndHorizontal();


            //Identify Build
            GUILayout.Space(10);
            m_BuildData.IdentityBuild = EditorGUILayout.TextField(m_BuildData.IdentityBuild, GUILayout.Width(100));

            if (string.IsNullOrEmpty(m_BuildData.IdentityBuild))
            {
                m_BuildData.IdentityBuild = "100";
            }

            //Build Button
            GUILayout.Space(10);
            if (GUILayout.Button("Export", GUILayout.Width(100)))
            {
                ProjectBuilder.ExportForiOS(m_BuildData);
            }

            //Build Button
            GUILayout.Space(10);
            if (GUILayout.Button("Build", GUILayout.Width(100)))
            {
                ProjectBuilder.BuildForiOS(m_BuildData);
            }

            //Step Desc
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Step: " + BuildStaticData.Step);

            GUILayout.EndVertical();
            //GUILayout.EndHorizontal();
        }
    }

}