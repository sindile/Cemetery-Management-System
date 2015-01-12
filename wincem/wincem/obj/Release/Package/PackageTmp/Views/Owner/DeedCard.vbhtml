@ModelType wincem.Owner
<table style="width: 475px">
    <tr>
        <td>Amount Paid: @Format(Model.Amount_Paid, "$#,###.00")</td>
        <td style="text-align: right">Deed #: @Model.Deed_No</td>
    </tr>
    <tr>
        <td colspan="2">
            @If String.IsNullOrEmpty(Model.Deed_Name) Then
                @<div>Owner: @Model.Owner_Name</div>
            Else
                @<div>Owner: @Model.Deed_Name</div>    
            End If</td>
    </tr>
    <tr>
        <td colspan="2">Address: @Model.Owner_Address</td>
    </tr>
    <tr>
        <td>Phone Number: @Model.Phone_Number</td>
        <td style="text-align: right">
            @If Model.Date_of_Purchase Is Nothing Then
                @<span>Date: </span>
            Else
                @<span>Date: </span>@Model.Date_of_Purchase.Value.ToShortDateString()
            End If
        </td>
    </tr>
</table>

@If Model.Deed_Type = "A" Then
    @<table style="width: 500px; border-collapse: collapse">
        <tr>
            <td colspan="5" style="text-align: center">ASSIGNMENTS</td>
        </tr>
        <tr>
            <td style="text-align: center">Cemetery</td>
            <td style="text-align: center">Division</td>
            <td style="text-align: center">Block</td>
            <td style="text-align: center">Lot/Row</td>
            <td style="text-align: center">Space No</td>
        </tr>
        @For Each item In Model.Spaces
            @<tr>
                <td style="text-align: center; border: solid black 1px">@item.cemetery()</td>
                <td style="text-align: center; border: solid black 1px">@item.division()</td>
                <td style="text-align: center; border: solid black 1px">@item.Space_ID.Substring(5, 1)</td>
                <td style="text-align: center; border: solid black 1px">@item.Space_ID.Substring(7, 3)</td>
                <td style="text-align: center; border: solid black 1px">@item.Space_ID.Substring(11, 3)</td>
            </tr>
        Next
    </table>
Else
    Dim lot_row_id As String = ""
    Dim SpaceList As New List(Of String)
    Dim Lot_row As New List(Of String)
    Dim Cemetery As New List(Of String)
    Dim Division As New List(Of String)
    Dim x = 0
    @<table style="width: 475px; border-collapse: collapse; table-layout:fixed">
        <tr>
            <td colspan="5" style="text-align: center">PURCHASES</td>
        </tr>
        <tr>
            <td style="text-align: center; width: 70px">Cemetery</td>
            <td style="text-align: center; width: 175px">Division</td>
            <td style="text-align: center; width: 50px">Block</td>
            <td style="text-align: center; width: 75px">Lot/Row</td>
            <td style="text-align: center; width: 105px">Space No</td>
        </tr>
        @For Each item In Model.Spaces.OrderBy(Function(o) o.Space_ID)
                If lot_row_id = "" Then
                    SpaceList.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                    Lot_row.Add(item.Space_ID.Substring(0, 10))
                    Cemetery.Add(item.cemetery())
                    Division.Add(item.division())
                ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                    If SpaceList.Item(x).Length Mod 11 = 0 Then
                        SpaceList.Item(x) = SpaceList.Item(x) + vbNewLine + item.Space_ID.Substring(11, 3).TrimStart("0")
                    Else
                        SpaceList.Item(x) = SpaceList.Item(x) + "," + item.Space_ID.Substring(11, 3).TrimStart("0")
                    End If
                ElseIf lot_row_id = item.Space_ID.Substring(0, 10) Then
                    SpaceList.Add(item.Space_ID.Substring(11, 3).TrimStart("0"))
                    Lot_row.Add(item.Space_ID.Substring(0, 10))
                    Cemetery.Add(item.cemetery())
                    Division.Add(item.division())
                    x += 1
                End If
                lot_row_id = item.Space_ID.Substring(0, 10)
            Next
        @If x <> 0 Then
                x = 0
            End If
        @For Each item In Lot_row
            @<tr>
                <td style="text-align: center; border: solid black 1px">@Cemetery.Item(x)</td>
                <td style="text-align: center; border: solid black 1px">@Division.Item(x)</td>
                <td style="text-align: center; border: solid black 1px">@item.Substring(5, 1)</td>
                <td style="text-align: center; border: solid black 1px">@item.Substring(7, 3)</td>
                <td style="text-align: center; border: solid black 1px">@SpaceList.Item(x)</td>
            </tr>
            x += 1
        Next
    </table>
End If
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<br />
<div style="display: inline-block; width: 475px; font-size: 12px">
@For Each item In Model.Burials.OrderBy(Function(o) o.Space_ID)
    
    @<div style="display: inline-block; border: solid black 1px; padding: 2px; width: 48%">
    <div style="border-bottom: solid black 1px">Permit No.: @item.Permit_Number | Space No.: @item.Space_ID.Substring(11, 3).TrimStart("0")</div>
    <div><div style="border-bottom: solid black 1px">@item.Work_Order_Date.Value.ToLongDateString</div></div>
    <div><div style="border-bottom: solid black 1px">@((item.First_Name + " " + item.Last_Name).substring(0, IIf((item.First_Name + " " + item.Last_Name).Length > 31, 31, (item.First_Name + " " + item.Last_Name).Length)))</div></div>
@*    <div>
    @If Not item.Date_of_Birth Is Nothing Then
            @<div style="border-bottom: solid black 1px;">DOD: item.Date_of_Death.Value.ToShortDateString()</div>
    Else
        @<div style="border-bottom: solid black 1px;">DOD: 00/00/0000</div>
    End If
     </div>*@
    </div>
Next
</div>

@*@If Model.dee Then*@