@ModelType wincem.Owner
@If Model Is Nothing Then
    If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Deed_Editors") Then
        @Html.ActionLink("Create Deed", "Create", "Owner", New With {.Space_ID = ViewBag.space_id}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "ownerCreateButton", .data_ajax = "false"}) 
    Else
        @Html.ActionLink("Create Deed", "Create", "Owner", New With {.Space_ID = ViewBag.space_id}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "ownerCreateButton", .data_ajax = "false", .class = "ui-disabled"}) 
    End If
Else
    @<p>Owner: @Model.Owner_Name</p>
    @Html.ActionLink("Deed Info.", "Details", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "ownerIdentifyButton" + Model.ID.ToString, .data_ajax = "false"})
    If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Burial_Editors") Then
        @Html.ActionLink("Add Burial", "Create", "Burial", New With {.Space_ID = ViewBag.space_id, .Owner_ID = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "addBurialButton" + Model.ID.ToString, .data_ajax = "false"})
        @Html.ActionLink("Space Info.", "Details", "Owner_Space", New With {.id = ViewBag.owned_space_id}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "spaceInfoButton" + Model.ID.ToString, .data_ajax = "false"})
Else
        @Html.ActionLink("Add Burial", "Create", "Burial", New With {.Space_ID = ViewBag.space_id, .Owner_ID = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "addBurialButton" + Model.ID.ToString, .data_ajax = "false", .class = "ui-disabled"})
        @Html.ActionLink("Space Info.", "Details", "Owner_Space", New With {.id = ViewBag.owned_space_id}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .id = "spaceInfoButton" + Model.ID.ToString, .data_ajax = "false"})
    End If
   
    For Each item In Model.Burials.Where(Function(w) w.Space_ID = ViewBag.space_id)
        If Not String.IsNullOrEmpty(item.Photo) Then
    @<a href="#photoPopup@(item.Permit_Number)" data-rel="popup">
        <img src ="@(item.Photo)" alt="Headstone Photo" width="250" name="Headstone_Photo" id="Headstone_Photo"/>
    </a>
    @<div data-role="popup" id="photoPopup@(item.Permit_Number)" data-overlay-theme="a" data-theme="d" data-corners="false" data-position-to="window" style="max-width: 1000px;">
        <a href="#" data-rel="back" id="ClosephotoPopup@(item.Permit_Number)" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
        <img src="@item.Photo" alt="Headstone Photo" name="Headstone_Photo_popup" id="Headstone_Photo_popup" />
    </div>  
    Exit For
End If
Next
End If