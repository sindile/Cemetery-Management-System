@ModelType  IEnumerable(Of wincem.Assignment)

<ul data-role="listview" data-inset="true">
    <li data-role="list-divider" role="heading" data-theme="a"><h3>Assignments</h3></li>
    @For Each item In Model
        Dim currentItem = item
        @<li><a href="#burialPopup_@currentItem.ID" data-rel="popup">@currentItem.Original_Owner.Owner_Name <div style="display:inline-block"><img src="~/Images/right_arrow.png" height="16px" /></div> @currentItem.New_Owner.Owner_Name</a>
            
         </li>
        
    Next
    <li  data-theme="b" data-icon="plus" data-iconpos="left" class="ui-disabled">@Html.ActionLink("Add Assignments from the map!".ToUpper(), "Create", "Assignment", New With {.Owner_ID = ViewData("Owner_ID"), .owned_space_ID = ViewData("owned_space_ID")}, New With {.data_ajax = "false"})</li>    
</ul>
@For Each item In Model
    Dim currentItem = item
    @<div data-role="popup" id="burialPopup_@currentItem.ID" data-theme="a">
        <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
        <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
            <li data-icon="pencil">@Html.ActionLink("Edit", "Edit", "Assignment", New With {.id = currentItem.ID}, Nothing)</li>
            <li data-icon="delete">@Html.ActionLink("Delete", "Delete", "Assignment", New With {.id = currentItem.ID}, New With {.data_rel = "dialog"})</li>
            <li data-icon="mappin">@Html.ActionLink("View Map", "Index", "Map", New With {.Space_ID = currentItem.Owned_Space.Space_ID}, New With {.data_ajax = "false"})</li>
        </ul>
    </div>
Next