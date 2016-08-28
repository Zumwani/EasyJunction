Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public NotInheritable Class ContextMenuItem

    Private Shared Keys As String() =
        {FolderKey, DesktopKey, LibraryKey}

    Private Const FolderKey As String =
        "Directory\Background"

    Private Const DesktopKey As String =
        "DesktopBackground"

    Private Const LibraryKey As String =
        "LibraryFolder\Background"

    Private Shared Command As String =
        String.Format("{0}{1}{0} {0}%V{0}", ControlChars.Quote, Assembly.GetExecutingAssembly.Location)

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property IsInstalled As Boolean
        Get
            Return GetIsInstalled()
        End Get
    End Property

    Public Shared Sub Install()

        For Each key In Keys
            CreateItem(key, "CreateJunction", "Create new junction point", Command)
        Next

        NotifyContextMenusChange()

    End Sub

    Public Shared Sub Uninstall()

        For Each key In Keys
            RemoveItem(key, "CreateJunction")
        Next

        NotifyContextMenusChange()

    End Sub

    Private Shared Function GetIsInstalled() As Boolean

        Dim i As Integer = 0
        For Each key In Keys
            If Exists(key, "CreateJunction", Command) Then
                i += 1
            End If
        Next

        Return (i = 3)

    End Function

    Private Shared Sub CreateItem(ShellTypeKey As String, CommandName As String, CommandDisplayName As String, Command As String)

        Dim stKey As RegistryKey = Nothing
        Dim shellKey As RegistryKey = Nothing
        Dim commandKey As RegistryKey = Nothing
        Dim commandSubKey As RegistryKey = Nothing

        Try

            stKey = Registry.CurrentUser.CreateSubKey("software\classes\" + ShellTypeKey)
            shellKey = stKey.CreateSubKey("shell")
            commandKey = shellKey.CreateSubKey(CommandName)
            commandSubKey = commandKey.CreateSubKey("command")

            commandKey.SetValue("", CommandDisplayName)
            commandSubKey.SetValue("", Command)

        Catch ex As Exception
        Finally
            If stKey IsNot Nothing Then stKey.Close()
            If shellKey IsNot Nothing Then stKey.Close()
            If commandKey IsNot Nothing Then stKey.Close()
            If commandSubKey IsNot Nothing Then stKey.Close()
        End Try

    End Sub

    Private Shared Sub RemoveItem(ShellTypeKey As String, CommandName As String)
        Registry.CurrentUser.DeleteSubKeyTree(String.Format("software\classes\{0}\shell\{1}", ShellTypeKey, CommandName), False)
    End Sub

    Private Shared Function Exists(ShellTypeKey As String, CommandName As String, Optional Command As String = Nothing) As Boolean

        Try
            Using key = Registry.CurrentUser.OpenSubKey(String.Format("software\classes\{0}\shell\{1}\command", ShellTypeKey, CommandName))
                If key IsNot Nothing Then
                    Return (key.GetValue("", "") = Command)
                End If
            End Using
        Catch ex As Exception
        End Try

        Return False

    End Function

    <DllImport("shell32.dll")>
    Private Shared Sub SHChangeNotify(ByVal wEventId As Integer, ByVal uFlags As Integer, ByVal dwItem1 As Integer, ByVal dwItem2 As Integer)
    End Sub

    Private Shared Sub NotifyContextMenusChange()
        Const SHCNE_ASSOCCHANGED = &H8000000
        Const SHCNF_IDLIST = 0
        SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, 0, 0)
    End Sub

End Class