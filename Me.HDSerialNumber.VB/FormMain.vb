'////////////////////////////////////////////////////////////////////////////////////////////////////
' AUTEUR : Jose ALVAREZ 
' SOCIETE: Institution Saint Joseph LAXOU 54 - FRANCE.
' DATE : 18/08/2008
'
' REVISIONS :
' - 21/08/2008 : Version Alpha 1.0.2
'	* Ajout: Localisation fr-FR et en-US. (A definir dans les Settings (Langue))
' - 18/08/2008 : Version Alpha 1.0.1
'	* Ajout: Utilisation de la classe DriveInfo.
'	* Ajout: Performances.
' – 18/08/2008 : Creation Version Alpha 1.0.0.
'	* Utilisation de WMI. 
'////////////////////////////////////////////////////////////////////////////////////////////////////

Imports System.Management
Imports System.Globalization
Imports System.Threading
Imports System.Resources

''' <summary>
''' Diverses methodes pour recuperer les informations d'un disque dur.
''' </summary>
''' ''' <example>
''' Source VB de cette classe. 
''' <code source="..\Sj.HDSerialNumber.IHM\FormMain.vb" lang="vbnet" title="VB.NET" />
''' </example>
''' <remarks>Provenance communaute Internet.</remarks>
Public Class FormMain

#Region "Membres Private"

	''' <summary>
	''' Pour les performances.
	''' </summary>
	''' <remarks></remarks>
	Private _executionTimeWatch As New Stopwatch

	''' <summary>
	''' Ressource manager pour la localisation.
	''' </summary>
	''' <remarks></remarks>
	Private _Rm As System.Resources.ResourceManager

	''' <summary>
	''' Culture info pour la localisation.
	''' </summary>
	''' <remarks></remarks>
	Private _Ci As CultureInfo

#End Region

#Region "Constructeurs"

	''' <summary>
	''' Definit la culture a utiliser d'apres la configuration <c>Langue</c>
	''' </summary>
	''' <remarks></remarks>
	Public Sub New()

		Dim message As String = My.Application.Info.AssemblyName & ".FormMain" & ".New()" & vbCrLf
		Dim langue As String = My.Settings.Langue

		' Doit etre place avant InitializeComponent().
		Try
			Thread.CurrentThread.CurrentUICulture = New CultureInfo(langue)
			Thread.CurrentThread.CurrentCulture = New CultureInfo(langue)
		Catch ex As Exception
			message += ex.Message & vbCrLf & "La culture par défaut sera utilisée."
			MessageBox.Show(message, My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Warning)
		End Try

		' Cet appel est requis par le Concepteur Windows Form.
		InitializeComponent()

	End Sub	'New

#End Region

#Region "Methodes Private"

	''' <summary>
	''' Procedure affiche les informations du disque dur avec DriveInfo.
	''' </summary>
	''' <remarks></remarks>
	Private Sub GetSerialNumberDriveInfo()

		Dim info As New DriveInfo(0)
		If Not info.SerialNumber Is Nothing Then
			TextBoxDisque.AppendText(_Rm.GetString("NumSerie", _Ci) & " = " & info.SerialNumber & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("Modele", _Ci) & " = " & info.Model & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("Type", _Ci) & " = " & info.DriveType & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("Firmware", _Ci) & " = " & info.Firmware & vbCrLf)
		End If

	End Sub	'GetSerialNumberDriveInfo

	''' <summary>
	''' Donnees à recuperer.
	''' </summary>
	''' <remarks></remarks>
	Private Class DonneesDisque
		Public DeviceID As String
		Public NumeroSerie As String
		Public Modele As String
		Public Type As String
	End Class 'DonneesDisque

	''' <summary>
	''' Procedure affiche les informations du disque dur avec WMI.
	''' </summary>
	''' <remarks></remarks>
	Private Sub GetSerialNumberWmi()

		Dim dd As New DonneesDisque

		Try
			Dim objSearcherMedia As New ManagementObjectSearcher( _
			   "root\CIMV2", _
			   "SELECT * FROM Win32_PhysicalMedia")
			For Each objQueryMedia As ManagementObject In objSearcherMedia.Get()
				dd.DeviceID = objQueryMedia("Tag")
				dd.NumeroSerie = Trim(objQueryMedia("SerialNumber"))
				If dd.NumeroSerie <> String.Empty Then
					Dim objSearcherDrive As New ManagementObjectSearcher( _
					 "root\CIMV2", _
					 "SELECT * FROM Win32_DiskDrive WHERE DeviceID='\\\\.\\" & Mid(dd.DeviceID, 5) & "'")
					For Each objQueryDrive As ManagementObject In objSearcherDrive.Get()
						dd.Modele = objQueryDrive("Model")
						dd.Type = objQueryDrive("InterfaceType")
					Next
					Exit For
				End If
			Next

			TextBoxDisque.AppendText(_Rm.GetString("NumSerie", _Ci) & " = " & dd.NumeroSerie & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("Modele", _Ci) & " = " & dd.Modele & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("Type", _Ci) & " = " & dd.Type & vbCrLf)
			TextBoxDisque.AppendText(_Rm.GetString("DeviceID", _Ci) & " = " & dd.DeviceID & vbCrLf)

		Catch ex As Exception
			MessageBox.Show(ex.Message)
		End Try

	End Sub	'GetSerialNumberWmi

	''' <summary>
	''' Affichage des performances.
	''' </summary>
	''' <param name="methodName"></param>
	''' <remarks></remarks>
	Private Sub LogMethodExcecutionTime(ByVal methodName As String)

		Me.TextBoxPerf.Clear()
		Me._executionTimeWatch.Stop()
		Me.TextBoxPerf.AppendText(methodName & ": " & Me._executionTimeWatch.ElapsedMilliseconds.ToString & " ms")
		Me._executionTimeWatch.Reset()

	End Sub	'LogMethodExcecutionTime

#End Region

#Region "Handles Evenements"

	Private Sub ButtonWmi_Click( _
	ByVal sender As System.Object, ByVal e As System.EventArgs) _
	Handles ButtonWmi.Click

		Me._executionTimeWatch.Start()
		TextBoxDisque.Clear()
		GetSerialNumberWmi()
		Me.LogMethodExcecutionTime("ButtonWmi_Click")

	End Sub	'ButtonWmi_Click

	Private Sub ButtonDriveInfo_Click( _
	ByVal sender As System.Object, ByVal e As System.EventArgs) _
	Handles ButtonDriveInfo.Click

		Me._executionTimeWatch.Start()
		TextBoxDisque.Clear()
		GetSerialNumberDriveInfo()
		Me.LogMethodExcecutionTime("ButtonDriveInfo_Click")

	End Sub	'ButtonDriveInfo_Click

	Private Sub FormMain_Load( _
	ByVal sender As Object, ByVal e As System.EventArgs) _
	Handles Me.Load

		' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

		' Creation d'un resource manager pour recuperer les ressources.
		_Rm = New ResourceManager( _
		 "Sj.HDSerialNumber.IHM.HDSerialNumber", _
		 System.Reflection.Assembly.GetExecutingAssembly)

		' Recupere la culture a utiliser.
		_Ci = Thread.CurrentThread.CurrentCulture

		Me.Text += ": " & _Ci.ToString

	End Sub	'FormMain_Load

#End Region

End Class 'FormMain
