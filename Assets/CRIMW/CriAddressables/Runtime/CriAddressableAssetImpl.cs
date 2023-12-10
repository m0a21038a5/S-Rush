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

using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace CriWare.Assets
{
	/**
	 * <summary>Addressables 配置の実データ格納先 (Remote)</summary>
	 */
	public class CriAddressableAssetImpl : ICriFileAssetImpl
	{
		[SerializeField]
		string fileName;
		[SerializeField]
		internal CriAddressablesAnchor anchor;
		[SerializeField]
		string _originalId;

		public CriAddressableAssetImpl(string fileName, CriAddressablesAnchor anchor, string originalId)
		{
			this.fileName = fileName;
			this.anchor = anchor;
			this._originalId = originalId;
		}

		string CachePath => CriAddressables.Filename2CachePath(fileName);

		public string Path =>
#if UNITY_EDITOR
		anchor == null ?
			CachePath :
			System.IO.Path.GetFullPath(AssetDatabase.GUIDToAssetPath(_originalId));
#else
		CachePath;
#endif

		public bool HasCache => File.Exists(CachePath);

		public void ClearCache()
		{
			if (HasCache)
				File.Delete(CachePath);
		}

		public void OnEnable() { }

		public void OnDisable() { }

		public ulong Offset => 0;

		public long Size => new FileInfo(Path).Length;

		public bool IsReady => true;
	}


}

#endif

/** @} */
