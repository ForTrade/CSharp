using System;
namespace SmartQuant
{
	public enum ExecType
	{
		ExecNew,
		ExecRejected,
		ExecTrade,
		ExecPendingCancel,
		ExecCancelled,
		ExecCancelReject,
		ExecPendingReplace,
		ExecReplace,
		ExecReplaceReject
	}
}
