Imports System.Windows.Media.Animation

Public Class SetupWindow

    ''' <summary>Opens the window as a dialog.</summary>
    Public Shared Sub Open()
        current = New SetupWindow
        current.ShowDialog()
    End Sub

    Private Shared current As SetupWindow
    Public Shared Sub CloseWindow()
        current.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Refresh()
    End Sub

    Private canClose As Boolean = True
    Private Sub Window_KeyUp(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Escape And canClose Then
            Close()
        End If
    End Sub

    Private Sub InstallAppLink_Click(sender As Object, e As RoutedEventArgs)

        canClose = False
        Dim folder = InstallWindow.OpenDialog(Me)

        If folder IsNot Nothing Then
            App.PrepareToInstall(folder)
            Close()
        End If

        canClose = True

    End Sub

    Private Sub UninstallAppLink_Click(sender As Object, e As RoutedEventArgs)
        App.PrepareToUninstall()
        Close()
    End Sub

    Private Sub UpgradeAppLink_Click(sender As Object, e As RoutedEventArgs)
        App.Upgrade()
        Close()
    End Sub

    Private Sub DowngradeAppLink_Click(sender As Object, e As RoutedEventArgs)
        App.Upgrade()
        Close()
    End Sub

    Private Sub InstallItemLink_Click(sender As Object, e As RoutedEventArgs)

        ContextMenuItem.Install()
        Refresh()

        If ContextMenuItem.IsInstalled Then
            CType(FindResource("IsItemInstalledCheckboxShowAnimation"), Storyboard).Begin()
        End If

    End Sub

    Private Async Sub UninstallItemLink_Click(sender As Object, e As RoutedEventArgs)

        ContextMenuItem.Uninstall()

        If Not ContextMenuItem.IsInstalled Then
            Await CType(FindResource("IsItemInstalledCheckboxHideAnimation"), Storyboard).BeginAsync
        End If

        Refresh()

    End Sub

    Private Sub Refresh()

        SetDefaultState()

        SetStep1()
        If App.IsCurrentVersionInstalled Then
            SetStep2()
        End If

    End Sub

    Private Sub SetDefaultState()

        InstallAppLink.Hide
        UninstallAppLink.Hide
        UpgradeAppLink.Hide
        DowngradeAppLink.Hide
        IsAppInstalledCheckBox.Hide

        InstallItemLink.Disable
        InstallItemLink.Show
        UninstallItemLink.Hide
        IsItemInstalledCheckBox.Hide

    End Sub

    Private Sub SetStep1()

        Dim installVersion = WindowsInstaller.InstallVersion

        If App.IsInstalled AndAlso App.InstallExecutable.Exists Then

            If App.Version = installVersion Then
                UninstallAppLink.Show
                IsAppInstalledCheckBox.Show

            ElseIf App.Version > installVersion Then
                UpgradeAppLink.Show
                UninstallAppLink.Show

            ElseIf App.Version < installVersion Then
                DowngradeAppLink.Show
                UninstallAppLink.Show

            End If

        Else
            InstallAppLink.Show
        End If

    End Sub

    Private Sub SetStep2()

        If ContextMenuItem.IsInstalled Then
            InstallItemLink.Hide
            UninstallItemLink.Show
            IsItemInstalledCheckBox.Show
            IsItemInstalledCheckBox.Width = 50
        Else
            InstallItemLink.Show
            UninstallItemLink.Hide
            IsItemInstalledCheckBox.Hide
            InstallItemLink.Enable
        End If

    End Sub

    Private Sub IsItemInstalledCheckboxHideAnimation_Completed(sender As Object, e As EventArgs)
        IsItemInstalledCheckBox.Opacity = 1
        IsItemInstalledCheckBox.Width = 0
    End Sub

End Class