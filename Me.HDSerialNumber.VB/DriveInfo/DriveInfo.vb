'////////////////////////////////////////////////////////////////////////////////////////////////////
' AUTEUR : Jose ALVAREZ 
' SOCIETE: Institution Saint Joseph LAXOU 54 - FRANCE.
' DATE : 18/08/2008
'
' REVISIONS :
' – 18/08/2008 : Creation Version Alpha 1.0.0.
'////////////////////////////////////////////////////////////////////////////////////////////////////

Option Strict On
Option Explicit On 

Imports System.Runtime.InteropServices

''' <summary>
''' Classe fournissant les informations d'un disque dur.kkkk
''' </summary>
''' ''' <example>
''' Source VB de cette classe. 
''' <code source="..\Sj.HDSerialNumber.IHM\DriveInfo\DriveInfo.vb" lang="vbnet" title="VB.NET" />
''' </example>
''' <remarks>Provenance communaute Internet.</remarks>
Public Class DriveInfo

#Region "Membres prives"

	Private _serialNumber As String
	Private _model As String
	Private _firmware As String

	Private _driveType As DriveTypes = DriveTypes.Unknown
	Private _numberCylinders As Integer
	Private _numberHeads As Integer
	Private _sectorsPerTrack As Integer
	Private _bufferSize As Integer

#End Region

	''' <summary>
	''' Types de disque.
	''' </summary>
	''' <remarks></remarks>
	Public Enum DriveTypes
		Fixed
		Removable
		Unknown
	End Enum

#Region "Constructeurs"
	''' <summary>
	''' Constructeur.
	''' </summary>
	''' <param name="driveNumber">Numero disque.</param>
	''' <remarks></remarks>
	Public Sub New(ByVal driveNumber As Integer)

		Dim handle As Integer
		Dim returnSize As Integer

		Dim sci As New SENDCMDINPARAMS
		Dim sco As New SENDCMDOUTPARAMS

		If Environment.OSVersion.Platform = PlatformID.Win32NT Then
			handle = CreateFile("\\.\PhysicalDrive" & CStr(driveNumber), GENERIC_READ Or GENERIC_WRITE, FILE_SHARE_READ Or FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0)
		Else ' 9X
			handle = CreateFile("\\.\Smartvsd", 0, 0, 0, CREATE_NEW, 0, 0)
		End If

		If handle <> INVALID_HANDLE_VALUE Then

			sci.DriveNumber = CType(driveNumber, Byte)
			sci.BufferSize = Marshal.SizeOf(sco)
			sci.DriveRegs.DriveHead = CType(&HA0 Or (driveNumber << 4), Byte)
			sci.DriveRegs.Command = &HEC
			sci.DriveRegs.SectorCount = 1
			sci.DriveRegs.SectorNumber = 1

			If DeviceIoControl(handle, DFP_RECEIVE_DRIVE_DATA, sci, Marshal.SizeOf(sci), sco, Marshal.SizeOf(sco), returnSize, 0) <> 0 Then
				_serialNumber = SwapChars(sco.IDS.SerialNumber)
				_model = SwapChars(sco.IDS.ModelNumber)
				_firmware = SwapChars(sco.IDS.FirmwareRevision)
				_numberCylinders = sco.IDS.NumberCylinders
				_numberHeads = sco.IDS.NumberHeads
				_sectorsPerTrack = sco.IDS.SectorsPerTrack
				_bufferSize = sco.IDS.BufferSize * 512
				If (sco.IDS.GenConfig And &H80) = &H80 Then
					_driveType = DriveTypes.Removable
				ElseIf (sco.IDS.GenConfig And &H40) = &H40 Then
					_driveType = DriveTypes.Fixed
				Else
					_driveType = DriveTypes.Unknown
				End If
			End If

			CloseHandle(handle)

		End If

	End Sub

#End Region

#Region "Properties"

	''' <summary>
	''' SerialNumber
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property SerialNumber() As String
		Get
			Return _serialNumber
		End Get
	End Property
	''' <summary>
	''' Model
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Model() As String
		Get
			Return _model
		End Get
	End Property
	''' <summary>
	''' Firmware
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Firmware() As String
		Get
			Return _firmware
		End Get
	End Property
	''' <summary>
	''' NumberCylinders
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property NumberCylinders() As Integer
		Get
			Return _numberCylinders
		End Get
	End Property
	''' <summary>
	''' NumberHeads
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property NumberHeads() As Integer
		Get
			Return _numberHeads
		End Get
	End Property
	''' <summary>
	''' SectorsPerTrack
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property SectorsPerTrack() As Integer
		Get
			Return _sectorsPerTrack
		End Get
	End Property
	''' <summary>
	''' BufferSize
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property BufferSize() As Integer
		Get
			Return _bufferSize
		End Get
	End Property
	''' <summary>
	''' DriveType
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property DriveType() As DriveTypes
		Get
			Return _driveType
		End Get
	End Property

#End Region

#Region "Win32 Interop"

	<StructLayout(LayoutKind.Sequential, Size:=8)> _
	Private Class IDEREGS
		Public Features As Byte
		Public SectorCount As Byte
		Public SectorNumber As Byte
		Public CylinderLow As Byte
		Public CylinderHigh As Byte
		Public DriveHead As Byte
		Public Command As Byte
		Public Reserved As Byte
	End Class

	<StructLayout(LayoutKind.Sequential, Size:=32)> _
	Private Class SENDCMDINPARAMS

		Public BufferSize As Integer
		Public DriveRegs As IDEREGS
		Public DriveNumber As Byte
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> _
		Public Reserved() As Byte
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
		Public Reserved2() As Integer

		Public Sub New()
			DriveRegs = New IDEREGS
			Reserved = New Byte(2) {}
			Reserved2 = New Integer(3) {}
		End Sub

	End Class

	<StructLayout(LayoutKind.Sequential, Size:=12)> _
	Private Class DRIVERSTATUS

		Public DriveError As Byte
		Public IDEStatus As Byte
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
		Public Reserved() As Byte
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
		Public Reserved2() As Integer

		Public Sub New()
			Reserved = New Byte(1) {}
			Reserved2 = New Integer(1) {}
		End Sub

	End Class

	<StructLayout(LayoutKind.Sequential)> _
	Private Class IDSECTOR

		Public GenConfig As Short '0
		Public NumberCylinders As Short	'1
		Public Reserved As Short '2
		Public NumberHeads As Short	'3
		Public BytesPerTrack As Short '4
		Public BytesPerSector As Short '5
		Public SectorsPerTrack As Short	'6
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> _
		Public VendorUnique() As Short '7
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=20)> _
		Public SerialNumber() As Char '10
		Public BufferClass As Short	'20
		Public BufferSize As Short '21
		Public ECCSize As Short	'22
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> _
		Public FirmwareRevision() As Char '23
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=40)> _
		Public ModelNumber() As Char '27
		Public MoreVendorUnique As Short '47
		Public DoubleWordIO As Short '48
		Public Capabilities As Short '49
		Public Reserved1 As Short '50
		Public PIOTiming As Short '51
		Public DMATiming As Short '52
		Public BS As Short '53
		Public NumberCurrentCyls As Short '54
		Public NumberCurrentHeads As Short '55
		Public NumberCurrentSectorsPerTrack As Short '56
		Public CurrentSectorCapacity As Integer	'57
		Public MultipleSectorCapacity As Short '59
		Public MultipleSectorStuff As Short	'60
		Public TotalAddressableSectors As Integer '61
		Public SingleWordDMA As Short '63
		Public MultiWordDMA As Short '64
		<MarshalAs(UnmanagedType.ByValArray, SizeConst:=382)> _
		Public Reserved2() As Byte '65

		Public Sub New()
			VendorUnique = New Short(2) {}
			Reserved2 = New Byte(381) {}
			FirmwareRevision = New Char(7) {}
			SerialNumber = New Char(19) {}
			ModelNumber = New Char(39) {}
		End Sub

	End Class

	<StructLayout(LayoutKind.Sequential)> _
	Private Class SENDCMDOUTPARAMS

		Public BufferSize As Integer
		Public Status As DRIVERSTATUS
		Public IDS As IDSECTOR

		Public Sub New()
			Status = New DRIVERSTATUS
			IDS = New IDSECTOR
		End Sub

	End Class

	Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
	Private Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As Integer, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Integer

	Private Declare Function DeviceIoControl Lib "kernel32" (ByVal hDevice As Integer, ByVal dwIoControlCode As Integer, <[In](), Out()> ByVal lpInBuffer As SENDCMDINPARAMS, ByVal nInBufferSize As Integer, <[In](), Out()> ByVal lpOutBuffer As SENDCMDOUTPARAMS, ByVal nOutBufferSize As Integer, ByRef lpBytesReturned As Integer, ByVal lpOverlapped As Integer) As Integer

	Private Const CREATE_NEW As Integer = 1
	Private Const OPEN_EXISTING As Integer = 3

	Private Const GENERIC_READ As Integer = &H80000000
	Private Const GENERIC_WRITE As Integer = &H40000000

	Private Const FILE_SHARE_READ As Integer = &H1
	Private Const FILE_SHARE_WRITE As Integer = &H2

	Private Const VER_PLATFORM_WIN32_NT As Integer = 2

	Private Const DFP_RECEIVE_DRIVE_DATA As Integer = &H7C088

	Private Const INVALID_HANDLE_VALUE As Integer = -1

	Private Shared Function SwapChars(ByVal chars() As Char) As String
		For i As Integer = 0 To chars.Length - 2 Step 2
            Array.Reverse(chars, i, 2)
        Next
		Return New String(chars).Trim
	End Function

#End Region

End Class 'DriveInfo