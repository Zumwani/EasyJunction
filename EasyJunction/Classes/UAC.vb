Imports System.Reflection
Imports System.Security.Principal

Public NotInheritable Class UAC

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property HasAdminAccess As Boolean
        Get
            Return GetHasAdminAccess()
        End Get
    End Property

    ''' <summary>Starts a new instance with admin request, returns a value whatever the user accepts or declines.</summary>
    Public Shared Function RequestAdminAccess() As Boolean

        Dim info As New ProcessStartInfo

        With info
            .FileName = Assembly.GetExecutingAssembly.Location
            .UseShellExecute = True
            .Verb = "runas"
        End With

        Return (New Process With {.StartInfo = info}).Start

    End Function

    ''' <summary>Starts a new instance with admin request, and automatically exits this instance.</summary>
    Public Shared Function RequestAdminAccessAndExit()
        If RequestAdminAccess() Then
            SetupWindow.CloseWindow()
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function GetHasAdminAccess() As Boolean
        Dim identity = WindowsIdentity.GetCurrent()
        Dim principal = New WindowsPrincipal(identity)
        Dim isElevated As Boolean = principal.IsInRole(WindowsBuiltInRole.Administrator)
        Return isElevated
    End Function

End Class