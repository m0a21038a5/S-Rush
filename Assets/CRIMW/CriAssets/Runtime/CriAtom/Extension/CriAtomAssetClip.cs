/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */


#if CRIWARE_TIMELINE_1_OR_NEWER

using UnityEngine;
using System.IO;

namespace CriWare.Assets
{
	public class CriAtomAssetClip : CriWare.CriTimeline.Atom.CriAtomClipBase
	{
		[SerializeField]
		CriAtomCueReference cue;

		public override string CueName
		{
			get
			{
				var acb = GetAcb();
#if UNITY_EDITOR
				if (acb == null)
					acb = CriWare.CriTimeline.Atom.CriAtomTimelinePreviewer.Instance.GetAcb(AcbPath, AwbPath);
#endif
				if (acb == null) return null;
				acb.GetCueInfo(cue.CueId, out CriAtomEx.CueInfo info);
				return info.name;
			}
		}

		public override CriAtomExAcb GetAcb() => cue.AcbAsset.Handle;

		public override string AcbPath =>
#if UNITY_EDITOR
			Path.GetFullPath(UnityEditor.AssetDatabase.GetAssetPath((cue.AcbAsset is ICriReferenceAsset) ? (cue.AcbAsset as ICriReferenceAsset).ReferencedAsset : cue.AcbAsset));
#else
			cue.AcbAsset.FilePath;
#endif

		public override string AwbPath =>
#if UNITY_EDITOR
			(cue.AcbAsset.Awb == null) ? null : Path.GetFullPath(UnityEditor.AssetDatabase.GetAssetPath(cue.AcbAsset.Awb));
#else
			cue.AcbAsset.Awb?.FilePath;
#endif

		public override void SetCueFromAtomSource(CriAtomSourceBase atomSource)
		{
			if (!(atomSource is CriAtomSourceForAsset)) return;

			if (cue.AcbAsset == null)
				cue = (atomSource as CriAtomSourceForAsset).Cue;
		}
	}
}

#endif

/** @} */
