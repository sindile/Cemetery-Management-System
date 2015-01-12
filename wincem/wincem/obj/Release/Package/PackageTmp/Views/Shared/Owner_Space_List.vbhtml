@ModelType  IEnumerable(Of wincem.Owned_Space)

<ul data-role="listview" data-inset="true">
    <li data-role="list-divider" role="heading" data-theme="a"><h3>Spaces</h3></li>
    @For Each item In Model
        Dim currentItem = item
        @<li><a href="#ownedSpacePopup_@currentItem.ID" data-rel="popup">@currentItem.Space_ID</a>
            
         </li>
        
    Next
    <li data-theme="b" data-icon="plus" data-iconpos="left">@Html.ActionLink("Add Space", "Create", "Owner_Space", New With {.Owner_ID = ViewBag.Owner_ID, .Deed_Type = ViewBag.Deed_Type, .Available = If(ViewBag.Deed_Type = "A", "No", "Yes")}, New With {.data_ajax = "false"})</li>

</ul>
@For Each item In Model
    Dim currentItem = item
    @<div data-role="popup" id="ownedSpacePopup_@currentItem.ID" data-theme="a">
        <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
        <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
            <li data-icon="page">@Html.ActionLink("Details", "Details", "Owner_Space", New With {.id = currentItem.ID, .Available = "No"}, New With {.data_ajax = "false"})</li>
            <li data-icon="pencil">@Html.ActionLink("Edit", "Edit", "Owner_Space", New With {.id = currentItem.ID, .Available = "No' OR Available = 'Yes"}, New With {.data_ajax = "false"})</li>

            @If User.IsInRole("Cem_Admins") Then
                @<li data-icon="delete">@Html.ActionLink("Delete", "Delete", "Owner_Space", New With {.id = currentItem.ID}, New With {.data_rel = "dialog"})</li>
            Else
                @<li data-icon="delete" class = "ui-disabled">@Html.ActionLink("Delete", "Delete", "Owner_Space", New With {.id = currentItem.ID}, New With {.data_rel = "dialog"})</li>
            End If
            <li data-icon="mappin">@Html.ActionLink("View Map", "Index", "Map", New With {.Space_ID = currentItem.Space_ID}, New With {.data_ajax = "false"})</li>

        </ul>
    </div>
Next