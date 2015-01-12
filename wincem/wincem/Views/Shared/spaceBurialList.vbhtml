@ModelType  IEnumerable(Of wincem.Burial)

<ul data-role="listview" data-inset="true">
    <li data-role="list-divider" role="heading" data-theme="a">
        <h3>Burials</h3>
    </li>
    @For Each item In Model
        Dim currentItem = item
        @<li><a href="#burialPopup_@currentItem.ID" data-rel="popup">@currentItem.Last_Name, @currentItem.First_Name</a>

        </li>
        
    Next
    @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Burial_Editors") Then
        @<li data-theme="b" data-icon="plus" data-iconpos="left">@Html.ActionLink("Add Burial", "Create", "Burial", New With {.Owner_ID = ViewData("Owner_ID"), .space_ID = ViewData("space_ID")}, New With {.data_ajax = "false"})</li>
    Else
        @<li data-theme="b" data-icon="plus" data-iconpos="left" class="ui-disabled">@Html.ActionLink("Add Burial", "Create", "Burial", New With {.Owner_ID = ViewData("Owner_ID"), .space_ID = ViewData("space_ID")}, New With {.data_ajax = "false"})</li>
    End If
</ul>
@For Each item In Model
    Dim currentItem = item
    @<div data-role="popup" id="burialPopup_@currentItem.ID" data-theme="a">
        <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
        <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
            <li data-icon="page">@Html.ActionLink("Details", "Details", "Burial", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})</li>
            @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Burial_Editors") Then
                @<li data-icon="pencil">@Html.ActionLink("Edit", "Edit", "Burial", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})</li>
    Else
                @<li data-icon="pencil" class="ui-disabled">@Html.ActionLink("Edit", "Edit", "Burial", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})</li>
    End If

            @If User.IsInRole("Cem_Admins") Then
                @<li data-icon="delete">@Html.ActionLink("Delete", "Delete", "Burial", New With {.id = currentItem.ID}, New With {.data_rel = "dialog"})</li>
    Else
                @<li data-icon="delete" class="ui-disabled">@Html.ActionLink("Delete", "Delete", "Burial", New With {.id = currentItem.ID}, New With {.data_rel = "dialog"})</li>
    End If
            <li data-icon="mappin">@Html.ActionLink("View Map", "Index", "Map", New With {.Space_ID = currentItem.Space_ID}, New With {.data_ajax = "false"})</li>
        </ul>
    </div>
Next