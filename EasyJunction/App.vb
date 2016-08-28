Imports System.Reflection

Public NotInheritable Class App

#Region "Entry points"

    Public NotInheritable Class EntryPoints

        Private Sub New()
        End Sub

        Public Shared Sub Main(Args As String())

            Dim entryPoint As String = Args.FirstOrDefault
            Dim path As String = GetPath(Args)

            Select Case Args.FirstOrDefault
                Case Is = String.Empty
                    General()

                Case Is = DirectoryUtils.Directory
                    CreateJunction(path)

                Case Is = "-install"
                    Install(path)

                Case Is = "-uninstall"
                    Uninstall(path)

            End Select

        End Sub

        Private Shared Sub General()

            If ShouldLaunchInstalledApp() Then
                'Launch actual installed file
                Dim file As New FileInfo(App.InstallLocation.FullName + "\EasyJunction.exe")
                If file.Exists Then
                    file.Launch()
                End If

            Else
                SetupWindow.Open()

            End If

        End Sub

        Private Shared Sub CreateJunction(LinkFolder As String)
            If Directory.Exists(LinkFolder) Then
                JunctionWizardWindow.Open(LinkFolder)
            End If
        End Sub

        Private Shared Sub Install(InstallFolder As String)
            App.Install(New DirectoryInfo(InstallFolder))
        End Sub

        Private Shared Sub Uninstall(InstallFolder As String)
            App.Uninstall(New DirectoryInfo(InstallFolder))
        End Sub

        Private Shared Function GetPath(Args As String()) As String
            If Args.Count > 1 Then
                Return Args(1)
            Else
                If Directory.Exists(Args.FirstOrDefault) Then
                    Return Args.FirstOrDefault
                Else
                    Return String.Empty
                End If
            End If
        End Function

        Private Shared Function ShouldLaunchInstalledApp() As Boolean

            Return (App.IsInstalled AndAlso
                   App.Version = WindowsInstaller.InstallVersion AndAlso
                   Not App.StartupPath.Directory.IsSameAs(App.InstallLocation) AndAlso
                   New FileInfo(InstallLocation.FullName + "\EasyJunction.exe").Exists)

        End Function

    End Class

#End Region
#Region "Properties"

    Public Shared ReadOnly Property StartupPath As FileInfo =
        New FileInfo(Assembly.GetExecutingAssembly.Location)

    Public Shared ReadOnly Property Version As Version =
        Assembly.GetExecutingAssembly.GetName.Version

    Public Shared ReadOnly Property SizeOnDisk As Long =
        StartupPath.Length

    Public Shared ReadOnly Property IsInstalled As Boolean
        Get
            Return WindowsInstaller.IsInstalled
        End Get
    End Property

    Public Shared ReadOnly Property InstallLocation As DirectoryInfo
        Get
            Return WindowsInstaller.InstallLocation
        End Get
    End Property

    Public Shared ReadOnly Property IsCurrentVersionInstalled As Boolean
        Get
            Return (WindowsInstaller.IsInstalled AndAlso Version = WindowsInstaller.InstallVersion)
        End Get
    End Property

    Public Shared ReadOnly Property InstallExecutable As FileInfo
        Get
            Return New FileInfo(InstallLocation.FullName + "\EasyJunction.exe")
        End Get
    End Property

#End Region
#Region "Install / Uninstall"

    Public Shared Sub PrepareToInstall(Folder As DirectoryInfo)

        'If UAC.HasAdminAccess Then
        Install(Folder)
        'Else
        '    StartupPath.Launch("-install " + Folder.AsStringPath)
        'End If

    End Sub

    Public Shared Sub PrepareToUninstall()

        Dim targetFile As New FileInfo(My.Computer.FileSystem.SpecialDirectories.Temp + "\" + "EasyJunction.exe")
        StartupPath.CopyTo(targetFile, True)

        targetFile.Launch("-uninstall " + StartupPath.Directory.AsStringPath)

    End Sub

    Private Shared Sub Install(Folder As DirectoryInfo, Optional SupressDeletionMessage As Boolean = False)

        Try

            KillActiveInstances()

            If (Not SupressDeletionMessage) AndAlso
                (Folder.Exists AndAlso Folder.GetFiles.Count > 0) Then
                If Not MessageBox.Show("This folder is not empty, and will be cleared before install. Do you want to continue?",
                                   "Folder contains files", MessageBoxButton.YesNoCancel) = MessageBoxResult.Yes Then _
                                   Exit Sub

                Folder.Delete(True)

            End If

            Folder.Create()

            Dim targetFile As New FileInfo(Folder.FullName + "\EasyJunction.exe")
            StartupPath.CopyTo(targetFile, True)

            WindowsInstaller.AddItemToWindowsDeletionList(StartupPath)
            WindowsInstaller.InstallTo(Folder)
            StartMenu.AddToAllAppsMenu(Folder)

            targetFile.Launch()

        Catch ex As Exception
            MessageBox.Show("The app was not properly installed, you might want to try installing again." + vbNewLine + vbNewLine + ex.Message,
                            "The app was not properly installed.")
            SetupWindow.Open()
        End Try

    End Sub

    Private Shared Sub Uninstall(InstallFolder As DirectoryInfo)

        Try

            ContextMenuItem.Uninstall()
            WindowsInstaller.Uninstall()
            WindowsInstaller.AddItemToWindowsDeletionList(StartupPath)
            StartMenu.RemoveFromAllAppsMenu()

            KillActiveInstances()
            InstallFolder.Delete(True)

        Catch UAex As IOException
            MessageBox.Show("The install folder, or exe file inside, could not be deleted, it can be deleted manually." + vbNewLine + vbNewLine + UAex.GetType.FullName + ":" + vbNewLine + UAex.Message,
                            "The app was not fully uninstalled.")

        Catch ex As Exception
            MessageBox.Show("The app was not properly uninstalled, you might want to try installing again and then uninstalling." + vbNewLine + vbNewLine + ex.GetType.FullName + ":" + vbNewLine + ex.Message,
                            "The app was not properly uninstalled.")

        End Try

        SetupWindow.Open()

    End Sub

    Public Shared Sub Upgrade()
        Install(InstallLocation, True)
    End Sub

    Private Shared Sub KillActiveInstances()

        Dim currentID As Integer = Process.GetCurrentProcess.Id
        For Each instance In Process.GetProcessesByName("EasyJunction")

            If Not instance.Id = currentID Then

                instance.Kill()
                instance.WaitForExit(10000)

            End If
        Next

    End Sub

#End Region

End Class