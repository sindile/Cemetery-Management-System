Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations

Public Class Owner
    Public Property ID() As Integer
    <Display(Name:="Deed Number")> _
    Public Property Deed_No() As String
    <Display(Name:="Deed Type")> _
    Public Property Deed_Type() As String
    <DataType(DataType.Date)> _
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    <Display(Name:="Date of Purchase")> _
    Public Property Date_of_Purchase() As DateTime?
    <Display(Name:="Amount Paid")> _
    Public Property Amount_Paid() As Double?
    <Display(Name:="Owner Name")> _
    Public Property Owner_Name() As String
    <Display(Name:="Owner Address")> _
    Public Property Owner_Address() As String
    <Display(Name:="Phone Number")> _
    Public Property Phone_Number() As String
    <Display(Name:="Deed Name")> _
    Public Property Deed_Name() As String
    <Display(Name:="Deed Card")> _
    Public Property LF_Deed_Card() As String
    <Display(Name:="Certificate of Ownership")> _
    Public Property LF_Cert_of_Ownership() As String
    <Display(Name:="Remarks")> _
    Public Property Remarks() As String
    Public Property Mayor() As String
    Public Property CityClerk() As String

    Public Overridable Property Burials As List(Of Burial)
    Public Overridable Property Spaces As List(Of Owned_Space)

    Public Function spaceList() As List(Of String)
        Dim lot_row_id As String = ""
        Dim sl As New List(Of String)
        Dim Lot_row As New List(Of String)
        Dim Cemetery As New List(Of String)
        Dim Division As New List(Of String)
        Dim x = 0

        For Each item In Me.Spaces.OrderBy(Function(o) o.Space_ID)
            If lot_row_id = "" Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Item(x) = sl.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
                x += 1
            End If
            lot_row_id = item.Space_ID.Substring(0, 10)
        Next

        Return sl
    End Function

    Public Function cemeteryList() As List(Of String)
        Dim lot_row_id As String = ""
        Dim sl As New List(Of String)
        Dim Lot_row As New List(Of String)
        Dim Cemetery As New List(Of String)
        Dim Division As New List(Of String)
        Dim x = 0

        For Each item In Me.Spaces.OrderBy(Function(o) o.Space_ID)
            If lot_row_id = "" Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Item(x) = sl.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
                x += 1
            End If
            lot_row_id = item.Space_ID.Substring(0, 10)
        Next

        Return Cemetery
    End Function

    Public Function divisionList() As List(Of String)
        Dim lot_row_id As String = ""
        Dim sl As New List(Of String)
        Dim Lot_row As New List(Of String)
        Dim Cemetery As New List(Of String)
        Dim Division As New List(Of String)
        Dim x = 0

        For Each item In Me.Spaces.OrderBy(Function(o) o.Space_ID)
            If lot_row_id = "" Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Item(x) = sl.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                Cemetery.Add(item.cemetery())
                Division.Add(item.division())
                x += 1
            End If
            lot_row_id = item.Space_ID.Substring(0, 10)
        Next

        Return Division
    End Function

    Public Function blockList() As List(Of String)
        Dim lot_row_id As String = ""
        Dim sl As New List(Of String)
        Dim Lot_row As New List(Of String)
        Dim blocks As New List(Of String)
        Dim x = 0

        For Each item In Me.Spaces.OrderBy(Function(o) o.Space_ID)
            If lot_row_id = "" Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                blocks.Add(item.Space_ID.Substring(5, 1))
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Item(x) = sl.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                blocks.Add(item.Space_ID.Substring(5, 1))
                x += 1
            End If
            lot_row_id = item.Space_ID.Substring(0, 10)
        Next

        Return blocks
    End Function

    Public Function lotRowList() As List(Of String)
        Dim lot_row_id As String = ""
        Dim sl As New List(Of String)
        Dim Lot_row As New List(Of String)
        Dim lotRow As New List(Of String)
        Dim x = 0

        For Each item In Me.Spaces.OrderBy(Function(o) o.Space_ID)
            If lot_row_id = "" Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                lotRow.Add(item.Space_ID.Substring(7, 3))
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Item(x) = sl.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
            ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                sl.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                Lot_row.Add(item.Space_ID.Substring(0, 10))
                lotRow.Add(item.Space_ID.Substring(7, 3))
                x += 1
            End If
            lot_row_id = item.Space_ID.Substring(0, 10)
        Next

        Return lotRow
    End Function
End Class
