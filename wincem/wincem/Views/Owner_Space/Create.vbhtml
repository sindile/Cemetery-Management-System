@ModelType wincem.Owned_Space

@Code
    ViewData("Title") = "Create"
End Code

    <div data-role="page" id="ownedSpaceCreatePage">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Add New Space
            </h3>
            @Html.ActionLink("Home", "Index", "Map", Nothing, New With {.data_icon = "home", .class = "ui-btn-left", .data_ajax = "false"})
            @*@Html.ActionLink("Back", "Index", Nothing, New With {.data_role = "button", .data_icon = "arrow-l"})*@
        </div>
        <div data-role="content" id="ownedSpaceCreateContent">

@Using Html.BeginForm("Create", "Owner_Space", FormMethod.Post, New With {.data_ajax = "false"})
    @Html.ValidationSummary(True)

    @<div>
        @Html.HiddenFor(Function(model) model.OwnerID)
        <fieldset data-role="controlgroup" data-type="horizontal">
            <legend>Please select a ...</legend>
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

        <div class="ui-grid-a">
            <div class="ui-block-a">
                @Html.ActionLink("Cancel", "Details", "Owner", New With {.id = Model.OwnerID}, New With {.data_role = "button"})
            </div>
            <div class="ui-block-b">
                <input type="submit" data-theme="b" value="Save" class="startProcessing"/>
            </div>
        </div>
    </div>
        End Using
            </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map" data-icon="mappin" data-ajax = "false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner" data-icon="user" data-ajax="false" class="startProcessing">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
