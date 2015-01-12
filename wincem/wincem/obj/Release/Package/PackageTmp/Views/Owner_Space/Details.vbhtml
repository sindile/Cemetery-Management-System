@ModelType wincem.Owned_Space

@Code
    ViewData("Title") = "Details"
End Code

    <div data-role="page" id="ownedSpaceDetailsPage">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Space Details
            </h3>
            @*@Html.ActionLink("Back", "Index", Nothing, New With {.data_role = "button", .data_icon = "arrow-l"})*@
            <a href="http://wincem.cloudapp.net/wincem" data-icon="home" class="ui-btn-left" data-ajax="false">Home</a>
                @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "pencil", .class = "ui-btn-right", .data_ajax = "false"})
        </div>
        <div data-role="content" id="ownedSpaceDetailsContent">

    <div>
        @Html.HiddenFor(Function(model) model.OwnerID)
        <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Space Details ...</legend>
        <select name="cemeterySelect" id ="cemeterySelect" data-native-menu="false">
            <option data-placeholder="true">Cemetery</option>
        </select>
        <select name="divisionSelect" id ="divisionSelect" data-native-menu="false" disabled="disabled">
            <option data-placeholder="true">Division</option>
        </select>
        <select name="blockSection" id ="blockSection" data-native-menu="false" disabled="disabled">
            <option data-placeholder="true">Block / Section</option>
        </select>
        <select name="lotRow" id ="lotRow" data-native-menu="false" disabled="disabled">
            <option>Lot / Row</option>
        </select>
        <select name="space" id ="space" data-native-menu="false" disabled="disabled">
            <option data-placeholder="true">Space</option>
        </select>
        </fieldset>
        <div class="editor-label">
            @Html.LabelFor(Function(model) model.Space_ID)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.Space_ID)
            @Html.ValidationMessageFor(Function(model) model.Space_ID)
        </div>

        @Html.Partial("spaceBurialList", ViewBag.Burials, New ViewDataDictionary From {{"Owner_ID", Model.OwnerID}, {"Space_ID", Model.Space_ID}})
        @Html.Partial("spaceAssignmentList", Model.Assignments.OrderByDescending(Function(o) o.ID).OrderByDescending(Function(o) o.Assignment_Date), New ViewDataDictionary From {{"Owner_ID", Model.OwnerID}, {"owned_space_ID", Model.ID}})

    </div>
            </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map?Space_ID=@Model.Space_ID" data-icon="mappin" data-ajax = "false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner/Details/@Model.OwnerID" data-icon="user" data-ajax="false" class="startProcessing">Deed</a></li>
                    <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript" src="~/Scripts/Owner_Space-details-ui.js"></script>
End Section
