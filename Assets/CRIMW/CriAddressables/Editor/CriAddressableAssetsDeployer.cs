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
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using System.Linq;
using System.IO;

namespace CriWare.Assets
{
	/**
	 * <summary>CRI Addressables のデータデプロイを行うエディタクラス</summary>
	 */
	public static class CriAddressableAssetsDeployer
	{
		public static event System.Action OnComplete;

		[InitializeOnLoadMethod]
		static void RegisterHook()
		{
			BuildScript.buildCompleted += (AddressableAssetBuildResult result) => {
				if (EditorApplication.isPlayingOrWillChangePlaymode) return;
				Deploy();
			};
		}

		/**
		 * <summary>CRI Addressables 向けのデータデプロイ</summary>
		 * <remarks>
		 * <para header='説明'>
		 * CRI Addressables を利用したランタイムで必要になるリモート向けデータの書き出しを行います。<br/>
		 * データは CriAddressablesAsettings で設定したビルドパス以下に書き出されます。<br/>
		 * <br/>
		 * Addressable Group Setting 上からバンドルビルドを行った場合は本メソッドが自動的に呼び出されます。<br/>
		 * スクリプトからバンドルビルドを行っている場合は必要に応じてバンドルビルド後に本メソッドを呼び出してください。<br/>
		 * </para>
		 * </remarks>
		 */
		[MenuItem("CRIWARE/Deploy Cri Addressables")]
		public static void Deploy()
		{
			DeployLocalData();
			DeployRemoteData();
			OnComplete?.Invoke();
		}

		public static void DeployLocalData()
		{
			var addressablesBuildPath = CriAddressablesSetting.Instance.Local.buildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);

			EditorUtility.DisplayProgressBar("[CRIWARE][Addressables] Collectiong dependencies for local bundles", "", 0);

			var assets = CriAddressablesEditor.CollectDependentAssets<CriLocalAddressablesAssetImpl>();

			try
			{
				var count = 0;
				foreach (var asset in assets)
				{
					count++;
					EditorUtility.DisplayProgressBar("[CRIWARE][Addressables] Deploy data for local bundles", $"{count} / {assets.Count()} assets", (float)count / assets.Count());
					var srcPath = AssetDatabase.GetAssetPath(asset);
					var dstPath = Path.Combine(addressablesBuildPath, (asset.Implementation as CriLocalAddressablesAssetImpl).InternalPath);
					CriAddressablesEditor.DeployData(srcPath, dstPath);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

			Debug.Log($"[CRIWARE] CriAddressableAssetsDeployer copied {assets.Count()} files to {addressablesBuildPath}/{CriStreamingFolderAssetImplCreator.DirectoryName}.");

			CriLocalAddressableGroupGenerator.ValidateGroup();
		}

		public static void DeployRemoteData()
		{
			EditorUtility.DisplayProgressBar("[CRIWARE]  Collectiong dependencies for remote bundles", "", 0);
			var addressableReferences = CriAddressablesEditor.CollectDependentAssets<CriAddressableAssetImpl>();

			try
			{
				var count = 0;
				foreach (var obj in addressableReferences)
				{
					count++;
					EditorUtility.DisplayProgressBar("[CRIWARE] Deploy data for remote bundles", $"{count} / {addressableReferences.Count()} assets", (float)count / addressableReferences.Count());
					var impl = obj.Implementation as CriAddressableAssetImpl;
					var entry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(impl.anchor)));
					CriRemoteAddressableGroupGenerator.DeployData(AssetDatabase.GetAssetPath(obj), entry);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}

			Debug.Log($"[CRIWARE] Deploy Cri Addressable Assets : Complete, {addressableReferences.Count()} Files.");

			CriRemoteAddressableGroupGenerator.ValidateGroup();
		}
	}
}

#endif

/** @} */
