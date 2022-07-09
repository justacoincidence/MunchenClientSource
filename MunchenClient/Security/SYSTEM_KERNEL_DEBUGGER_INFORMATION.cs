using System.Runtime.InteropServices;

namespace MunchenClient.Security
{
	internal struct SYSTEM_KERNEL_DEBUGGER_INFORMATION
	{
		[MarshalAs(UnmanagedType.U1)]
		internal bool KernelDebuggerEnabled;

		[MarshalAs(UnmanagedType.U1)]
		internal bool KernelDebuggerNotPresent;
	}
}
