Imports Microsoft.Win32

Public NotInheritable Class WindowsInstaller

    Private Sub New()
    End Sub

    Private Const WindowsInstallerKey As String =
        "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\EasyJunction"

    Public Shared ReadOnly Property IsInstalled As Boolean
        Get
            Return GetIsInstalled()
        End Get
    End Property

    Public Shared ReadOnly Property InstallLocation As DirectoryInfo
        Get
            Return GetInstallLocation()
        End Get
    End Property

    Public Shared ReadOnly Property InstallVersion As Version
        Get
            Return GetInstallVersion()
        End Get
    End Property

    Public Shared Sub InstallTo(Directory As DirectoryInfo)

        Uninstall()

        Using Key = Registry.LocalMachine.CreateSubKey(WindowsInstallerKey)
            Dim d As String = Date.Now.ToString("yyyyMMdd")

            Key.SetValues(New Dictionary(Of String, Object) From {
                          {"DisplayName", "EasyJunction"},
                          {"InstallLocation", Directory.FullName},
                          {"InstallDate", d},
                          {"NoRemove", 1},
                          {"NoRepair", 1},
                          {"Publisher", "Andreas Ingeholm"},
                          {"UninstallString", Directory.FullName + "\EasyJunction.exe"},
                          {"DisplayVersion", App.Version.ToString},
                          {"EstimatedSize", App.SizeOnDisk / 1024}})

        End Using

    End Sub

    Public Shared Sub Uninstall()
        Registry.LocalMachine.DeleteSubKey(WindowsInstallerKey, False)
    End Sub

    Private Shared Function GetIsInstalled() As Boolean
        Return (InstallLocation IsNot Nothing)
    End Function

    Private Shared Function GetInstallLocation() As DirectoryInfo

        Try

            Using Key = Registry.LocalMachine.OpenSubKey(WindowsInstallerKey)
                Dim value As String = Key.GetValue("InstallLocation", "")
                If value IsNot Nothing Then
                    Return New DirectoryInfo(value)
                Else
                    Return Nothing
                End If
            End Using

        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Private Shared Function GetInstallVersion() As Version

        Try

            Using Key = Registry.LocalMachine.OpenSubKey(WindowsInstallerKey)
                Dim value As String = Key.GetValue("DisplayVersion", "")
                If value IsNot Nothing Then
                    Return New Version(value)
                Else
                    Return Nothing
                End If
            End Using

        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Shared Sub AddItemToWindowsDeletionList(File As FileInfo)

        Try
            Static keyPath As String = "System\CurrentControlSet\Control\Session Manager"

            Using key = Registry.LocalMachine.OpenSubKey(keyPath, True)
                Dim value As List(Of String) = CType(key.GetValue("PendingFileRenameOperations", {""}), String()).ToList
                value.Insert(0, "\??\" + File.FullName)
                key.SetValue("PendingFileRenameOperations", value.ToArray)
            End Using
        Catch ex As Exception
        End Try

    End Sub

End Class