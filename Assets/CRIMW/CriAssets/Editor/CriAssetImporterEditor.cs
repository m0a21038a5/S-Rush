/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using System.Linq;

namespace CriWare.Assets
{
	[CustomEditor(typeof(CriAssetImporter), true)]
	class CriAssetImporterEditor : ScriptedImporterEditor
	{
		static List<System.Type> _assetImplCreators = null;
		static List<System.Type> AssetImplCreators
		{
			get
			{
				if (_assetImplCreators == null)
				{
					_assetImplCreators = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(assem => assem.GetTypes()).
						Where(t => typeof(ICriAssetImplCreator).IsAssignableFrom(t) && !t.IsInterface).ToList();
				}
				return _assetImplCreators;
			}
		}

		string GetDisplayName(System.Type type) =>
			(type.GetCustomAttributes(typeof(CriDisplayNameAttribute), false).FirstOrDefault() as CriDisplayNameAttribute)?.Name ?? type.Name;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var infoProp = serializedObject.FindProperty("assetInfo");
			if (infoProp != null)
				foreach (SerializedProperty prop in infoProp)
					EditorGUILayout.PropertyField(prop);

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			var creatorProp = serializedObject.FindProperty(nameof(CriAssetImporter.implementation));
			var index = AssetImplCreators.Select(t => string.Format("{0} {1}", t.Assembly.ToString().Split(',')[0], t.FullName)).ToList().IndexOf(creatorProp.managedReferenceFullTypename);
			var newindex = EditorGUILayout.Popup("Deploy Type", index, AssetImplCreators.Select(t => GetDisplayName(t)).ToArray());
			if (newindex != index)
			{
				index = newindex;
				creatorProp.managedReferenceValue = System.Activator.CreateInstance(AssetImplCreators.ToList()[index]);
			}
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(creatorProp, GUIContent.none, true);
			EditorGUI.indentLevel--;

			EditorGUILayout.HelpBox((target as CriAssetImporter).implementation.Description, MessageType.Info);

			if(!(target as CriAssetImporter).IsAssetImplCompatible)
				EditorGUILayout.HelpBox("選択された DeployType はこのアセットでは利用できません", MessageType.Error);

			serializedObject.ApplyModifiedProperties();

			base.ApplyRevertGUI();
		}
	}
}

/** @} */
