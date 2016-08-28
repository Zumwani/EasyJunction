Public Class InstallWindow

    Public Shared Function OpenDialog(OwnerWindow As Window) As DirectoryInfo

        Dim w As New InstallWindow
        w.Owner = OwnerWindow
        w.PathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\EasyJunction"
        w.ShowDialog()

        If Not w.PathBox.Text = "" Then
            Return New DirectoryInfo(w.PathBox.Text)
        Else
            Return Nothing
        End If

    End Function

    Private Sub BrowseButton_Click(sender As Object, e As RoutedEventArgs)

        Using dlg As New Forms.FolderBrowserDialog

            With dlg
                .Description = "Select folder to install to..."
                .RootFolder = Environment.SpecialFolder.MyComputer
                .ShowNewFolderButton = True
            End With

            canClose = False

            If dlg.ShowDialog = Forms.DialogResult.OK Then

                Dim folder = New DirectoryInfo(dlg.SelectedPath)
                If Not folder.Name = "EasyJunction" Or Not folder.Name = "Easy Junction" Then
                    PathBox.Text = dlg.SelectedPath + "\EasyJunction"
                Else
                    PathBox.Text = dlg.SelectedPath
                End If

            End If

        End Using

    End Sub

    Private Sub DoneButton_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private canClose As Boolean = True
    Private Sub Window_KeyUp(sender As Object, e As KeyEventArgs)

        If e.Key = Key.Escape And canClose Then
            Close()
        End If

        canClose = True

    End Sub

End Class