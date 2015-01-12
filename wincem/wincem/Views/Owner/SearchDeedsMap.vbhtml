@ModelType IEnumerable(Of wincem.Owner)
<ul data-role="listview" id="deedList">
@For Each item In Model
    Dim currentItem = item
    Dim spaceList As String = ""
    For Each s In item.Spaces
        spaceList = spaceList + "'" + s.Space_ID + "',"
    Next
    spaceList = spaceList.TrimEnd(",")
    
    @<li id="@currentItem.ID" class="deedRecord" data-spaceList ="@(spaceList)">
        @If currentItem.Date_of_Purchase.HasValue Then
            @Html.ActionLink(currentItem.Owner_Name + " - " + currentItem.Date_of_Purchase.Value.ToShortDateString, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
    ElseIf currentItem.Owner_Name Is Nothing Then
            @Html.ActionLink("No Information", "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
    Else
            @Html.ActionLink(currentItem.Owner_Name, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
    End If
    </li>
Next
    @If Model.Count = 0 Then
        @<li>
            No Deeds Match Search Criteria!
        </li>
    End If
</ul>