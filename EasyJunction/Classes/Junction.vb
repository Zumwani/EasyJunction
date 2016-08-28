Public NotInheritable Class Junction

    Public Shared Sub Create(Link As String, Target As String)
        Create(New DirectoryInfo(Link), New DirectoryInfo(Target))
    End Sub

    Public Shared Sub Create(Link As DirectoryInfo, Target As DirectoryInfo)

        Try

            Dim actualLink As New DirectoryInfo(Link.FullName + "\" + GenerateUniqueName(Link, Target.Name))

            If Not Target.Exists Then
                Target.Create()
            End If

            _Create(actualLink.FullName, Target.FullName)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Could not create junction.", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

    Private Shared Sub _Create(Link As String, Target As String)

        Try

            Dim info As New ProcessStartInfo()
            With info
                .FileName = "cmd"
                .Arguments = String.Format("/c mklink /j ""{0}"" ""{1}""", Link, Target)
                .UseShellExecute = True
                .CreateNoWindow = True
                .Verb = "runas"
            End With

            Process.Start(info)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Shared Function GenerateUniqueName(Folder As DirectoryInfo, Name As String) As String

        If Folder.GetDirectories(Name).Count = 0 Then
            Return Name
        Else
            For i As Integer = 2 To 200
                If Folder.GetDirectories(Name + " (" + i.ToString + ")").Count = 0 Then
                    Return Name + " (" + i.ToString + ")"
                End If
            Next
        End If

        Throw New IOException(String.Format("The name: '{0}' already exists in folder: '{1}'.", Name, Folder.FullName))

    End Function

End Class