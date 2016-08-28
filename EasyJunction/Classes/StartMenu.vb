Imports IWshRuntimeLibrary

Public NotInheritable Class StartMenu

    Private Sub New()
    End Sub

    Private Const StartmenuPath As String =
        "C:\ProgramData\Microsoft\Windows\Start Menu\Programs"

    Public Shared Sub AddToAllAppsMenu(InstallFolder As DirectoryInfo)

        Dim StartMenu As New DirectoryInfo(StartmenuPath)
        Dim target As New FileInfo(InstallFolder.FullName + "\EasyJunction.exe")
        CreateShortcut(target, StartMenu)

    End Sub

    Public Shared Sub RemoveFromAllAppsMenu()

        Dim shortcut As New FileInfo(StartmenuPath + "\EasyJunction.lnk")
        If shortcut.Exists Then
            shortcut.Delete()
        End If

    End Sub

    Private Shared Sub CreateShortcut([To] As FileInfo, ShortcutFolder As DirectoryInfo)
        Try
            Dim WshShell As New WshShell
            Dim shortcut As IWshShortcut = WshShell.CreateShortcut(ShortcutFolder.FullName + "\" + IO.Path.GetFileNameWithoutExtension([To].FullName) + ".lnk")

            ' set the shortcut properties
            With shortcut
                .TargetPath = [To].FullName
                .WindowStyle = 1I
                .Description = "Open " + [To].Name
                .WorkingDirectory = [To].Directory.FullName
                ' the next line gets the first Icon from the executing program
                .IconLocation = [To].FullName + ", 0"
                .Arguments = String.Empty
                .Save() ' save the shortcut file
            End With
        Catch ex As Exception
        End Try
    End Sub

End Class