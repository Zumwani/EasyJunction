Imports System.Runtime.CompilerServices
Imports System.Windows.Media.Animation
Imports Microsoft.Win32

Public NotInheritable Class DirectoryUtils

    Private Sub New()
    End Sub

    ''' <summary>Returns True when compared to a string directory path.</summary>
    Public Shared Directory As New DirectoryComparer

    Public Class DirectoryComparer

        Public Shared Operator =(left As String, dir As DirectoryComparer)
            Return (IO.Directory.Exists(left))
        End Operator

        Public Shared Operator <>(left As String, dir As DirectoryComparer)
            Return Not (IO.Directory.Exists(left))
        End Operator

    End Class

End Class

Public Module Extensions

    <Extension>
    Public Sub SetValues(Key As RegistryKey, Values As Dictionary(Of String, Object))
        For Each value In Values
            Key.SetValue(value.Key, value.Value, ConvertTypeToRegistryRepresentation(value.Value))
        Next
    End Sub

    Private Function ConvertTypeToRegistryRepresentation(Obj As Object) As RegistryValueKind

        If TypeOf Obj Is String Then
            If CType(Obj, String).Contains(vbNewLine) Then
                Return RegistryValueKind.MultiString
            Else
                Return RegistryValueKind.String
            End If
        ElseIf (TypeOf Obj Is Integer) Or (TypeOf Obj Is Double) Or (TypeOf Obj Is UInteger) Then
            Return RegistryValueKind.DWord
        ElseIf (TypeOf Obj Is Long) Or (TypeOf Obj Is ULong) Then
            Return RegistryValueKind.QWord
        Else
            Return RegistryValueKind.String
        End If

    End Function

    <Extension>
    Public Function IsSameAs(left As DirectoryInfo, right As DirectoryInfo) As Boolean
        Return ((right IsNot Nothing) AndAlso (left.FullName = right.FullName))
    End Function

    <Extension>
    Public Function AsStringPath(Folder As DirectoryInfo)
        Return """" + Folder.FullName + """"
    End Function

    <Extension>
    Public Function AsStringPath(File As FileInfo)
        Return """" + File.FullName + """"
    End Function

    <Extension>
    Public Sub CopyTo(File As FileInfo, DestinationFile As FileInfo, Optional Overwrite As Boolean = False)
        File.CopyTo(DestinationFile.FullName, Overwrite)
    End Sub

    <Extension>
    Public Sub Launch(File As FileInfo, Optional Args As String = "", Optional AsAdmin As Boolean = False)
        Process.Start(New ProcessStartInfo(File.FullName, Args) With {.Verb = If(AsAdmin, "runas", "")})
    End Sub

    <Extension>
    Public Sub Show(Element As FrameworkElement)
        Element.Visibility = Visibility.Visible
    End Sub

    <Extension>
    Public Sub Hide(Element As FrameworkElement)
        Element.Visibility = Visibility.Collapsed
    End Sub

    <Extension>
    Public Sub Enable(Element As FrameworkElement)
        Element.IsEnabled = True
    End Sub

    <Extension>
    Public Sub Disable(Element As FrameworkElement)
        Element.IsEnabled = False
    End Sub

    <Extension>
    Public Function BeginAsync(storyboard As Storyboard) As Task
        Dim tcs As System.Threading.Tasks.TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        If storyboard Is Nothing Then
            tcs.SetException(New ArgumentNullException())
        Else
            Dim onComplete As EventHandler = Nothing
            onComplete = Sub(s, e)
                             RemoveHandler storyboard.Completed, onComplete
                             tcs.SetResult(True)
                         End Sub
            AddHandler storyboard.Completed, onComplete
            storyboard.Begin()
        End If
        Return tcs.Task
    End Function

End Module