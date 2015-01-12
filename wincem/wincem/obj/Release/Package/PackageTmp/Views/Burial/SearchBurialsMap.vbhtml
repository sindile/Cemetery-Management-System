@ModelType IEnumerable(Of wincem.Burial)
<ul data-role="listview" id="burialList">
    @For Each item In Model
        Dim currentItem = item
        @<li id="@currentItem.ID" class="burialRecord" data-space_id = "@(item.Space_ID)">
            @If currentItem.Date_of_Death.HasValue Then
                @Html.ActionLink(currentItem.Last_Name + ", " + currentItem.First_Name + " - " + currentItem.Date_of_Death.Value.ToShortDateString, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
            ElseIf currentItem.Last_Name Is Nothing And currentItem.First_Name Is Nothing Then
                @Html.ActionLink("No Information", "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
            Else
                @Html.ActionLink(currentItem.Last_Name + ", " + currentItem.First_Name, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
            End If
        </li>
    Next
    @If Model.Count = 0 Then
        @<li>
            No Burials Match Search Criteria!
        </li>
    End If
</ul>
