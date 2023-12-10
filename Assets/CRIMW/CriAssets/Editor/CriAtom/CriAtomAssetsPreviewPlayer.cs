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
using System.Linq;
using UnityEditor;

namespace CriWare.Assets
{
	internal class CriAtomAssetsPreviewPlayer : System.IDisposable
	{
		static CriAtomAssetsPreviewPlayer _instance = null;
		public static CriAtomAssetsPreviewPlayer Instance => _instance ?? (_instance = new CriAtomAssetsPreviewPlayer());

		Dictionary<string, CriAtomExAcb> loadedAcbs = new Dictionary<string, CriAtomExAcb>();

		public CriAtomExAcb GetAcb(CriAtomAcbAsset asset)
		{
			CriWare.Editor.CriAtomEditorUtilities.InitializeLibrary();

			if (Application.isPlaying)
				return asset.Loaded ? asset.Handle : null;

			var originalPath = (asset is ICriReferenceAsset) ?
				AssetDatabase.GetAssetPath((asset as ICriReferenceAsset).ReferencedAsset):
				AssetDatabase.GetAssetPath(asset);
			originalPath = System.IO.Path.GetFullPath(originalPath);
			if (!loadedAcbs.ContainsKey(originalPath))
				loadedAcbs.Add(originalPath, CriAtomExAcb.LoadAcbFile(null, originalPath, (asset.Awb == null) ? null : AssetDatabase.GetAssetPath(asset.Awb)));
			return loadedAcbs[originalPath];
		}

		CriAtomExPlayer _player = null;
		CriAtomExPlayer Player => _player ?? (_player = new CriAtomExPlayer());

		public CriAtomExPlayback Play(CriAtomAcbAsset asset, int cueId)
		{
			var acb = GetAcb(asset);
			if (acb == null) return new CriAtomExPlayback(CriAtomExPlayback.invalidId);
			Player.SetCue(acb, cueId);
			return Player.Start();
		}

		public void Stop()
		{
			Player.Stop();
		}

		public void Dispose()
		{
			if (CriAtomPlugin.IsLibraryInitialized())
			{
				_player?.Stop(true);
				_player?.Dispose();
				foreach (var acb in loadedAcbs.Values)
					acb?.Dispose();
				CriAtomPlugin.FinalizeLibrary();
				if (CriFsPlugin.IsLibraryInitialized())
					CriFsPlugin.FinalizeLibrary();
			}
			_player = null;
			loadedAcbs.Clear();
		}
	}
}

/** @} */
