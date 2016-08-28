Public Class JunctionWizardWindow

    Private Link As String
    ''' <summary>Opens the window as a dialog.</summary>
    Public Shared Sub Open(Link As String)

        Dim w As New JunctionWizardWindow

        w.Refresh()
        w.Link = Link

        w.ShowDialog()

    End Sub

    Private Sub BrowseButton_Click(sender As Object, e As RoutedEventArgs)

        Using dlg As New Forms.FolderBrowserDialog

            With dlg
                .Description = "Select target for junction point..."
                .RootFolder = Environment.SpecialFolder.MyComputer
                .ShowNewFolderButton = True
            End With

            canClose = False

            If dlg.ShowDialog() Then
                PathBox.Text = dlg.SelectedPath
            End If

            Refresh()

        End Using

    End Sub

    Private Sub Refresh()
        DoneButton.IsEnabled = (Directory.Exists(PathBox.Text))
        UACImage.Visibility = If(UAC.HasAdminAccess, Visibility.Collapsed, Visibility.Visible)
    End Sub

    Private Sub PathBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Refresh()
    End Sub

    Private Sub DoneButton_Click(sender As Object, e As RoutedEventArgs)
        Junction.Create(Link, PathBox.Text)
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