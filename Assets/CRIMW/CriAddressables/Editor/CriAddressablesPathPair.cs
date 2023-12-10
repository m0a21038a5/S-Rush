/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.AddressableAssets.Settings;

namespace CriWare.Assets
{
	internal class CriAddressablesPathPair : ScriptableObject
	{
		[SerializeField]
		public ProfileValueReference buildPath;
		[SerializeField]
		public ProfileValueReference loadPath;
	}

	[CustomEditor(typeof(CriAddressablesPathPair))]
	internal class CriAddressablesPathPairEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("buildPath"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("loadPath"));
			serializedObject.ApplyModifiedProperties();
		}
	}
}

#endif

/** @} */
