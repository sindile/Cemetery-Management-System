@ModelType wincem.Assignment

@Code
    ViewData("Title") = "Create"
End Code

    <div data-role="page" id="assignmentCreatePage" data-add-back-btn="true">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Edit burial Data
            </h3>
            @*@Html.ActionLink("Back", "Details", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-l"})*@
        </div>
        <div data-role="content" id="assignmentCreateContent">

@Using Html.BeginForm("Create", "Assignment", Nothing, FormMethod.Post, New With {.data_ajax = "false"})
    @Html.ValidationSummary(True)

    @<div>
        @Html.HiddenFor(Function(model) model.Owned_SpaceID)
        <div class="editor-label">
            @Html.LabelFor(Function(model) model.Owned_SpaceID, "Owned_Space")
        </div>
        <div class="editor-field">
            @Html.TextBox("Space_ID", Model.Owned_Space.Space_ID)
            @Html.ValidationMessageFor(Function(model) model.Owned_SpaceID)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.Assignment_Date)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.Assignment_Date)
            @Html.ValidationMessageFor(Function(model) model.Assignment_Date)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.Original_OwnerID)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.Original_OwnerID)
            @Html.ValidationMessageFor(Function(model) model.Original_OwnerID)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.New_OwnerID)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.New_OwnerID)
            @Html.ValidationMessageFor(Function(model) model.New_OwnerID)
        </div>

        <div class="ui-grid-a">
            <div class="ui-block-a">
                @Html.ActionLink("Cancel", "Details", "Owner_Space", New With {.id = Model.Owned_SpaceID}, New With {.data_role = "button"})
                </div>
            <div class="ui-block-b">
                <input type="submit" data-theme="b" value="Save" /></div>
        </div>
    </div>
End Using
        </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map" data-icon="mappin"  data-ajax="false">Map</a></li>
                    <li><a href="~/Owner" data-icon="user">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
