@ModelType wincem.Owner

@Code
    ViewData("Title") = "Create"
End Code

    <div data-role="page" id="ownerCreatePage">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Create New Deed
            </h3>
           @* @Html.ActionLink("Back", "Index", Nothing, New With {.data_role = "button", .data_icon = "arrow-l"})*@
            @Html.ActionLink("Home", "Index", "Map", Nothing, New With {.data_icon = "home", .class = "ui-btn-left", .data_ajax = "false"})
        </div>
        <div data-role="content" id="ownerCreateContent">

@Using Html.BeginForm("Create", "Owner", New With {.Space_ID = If(Not String.IsNullOrEmpty(ViewBag.Space_ID), ViewBag.Space_ID, ""), .spaceList = If(Not String.IsNullOrEmpty(ViewBag.spaceList), ViewBag.spaceList, ""), .deedType = If(Not String.IsNullOrEmpty(ViewBag.deedType), ViewBag.deedType, "")}, FormMethod.Post, New With {.data_ajax = "false"})
    @Html.ValidationSummary(True)

    @<div>

        @Html.HiddenFor(Function(model) model.LF_Deed_Card)
        @Html.HiddenFor(Function(model) model.LF_Cert_of_Ownership)
        @Html.HiddenFor(Function(model) model.Deed_No)
        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Deed_No)
            @Html.EditorFor(Function(model) model.Deed_No, "disabledTextBox")
            @Html.ValidationMessageFor(Function(model) model.Deed_No)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Deed_Type)
            @Html.EditorFor(Function(model) model.Deed_Type, "DeedType")
            @Html.ValidationMessageFor(Function(model) model.Deed_Type)
            @If Model.Deed_Type = "A" Then
                @Html.HiddenFor(Function(model) model.Deed_Type)
            End If
        </div>

        <div data-role="fieldcontain">
                @Html.LabelFor(Function(model) model.Date_of_Purchase)
                @Html.EditorFor(Function(model) model.Date_of_Purchase)
                @Html.ValidationMessageFor(Function(model) model.Date_of_Purchase)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Amount_Paid)
            @Html.EditorFor(Function(model) model.Amount_Paid)
            @Html.ValidationMessageFor(Function(model) model.Amount_Paid)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Owner_Name)
            @Html.EditorFor(Function(model) model.Owner_Name)
            @Html.ValidationMessageFor(Function(model) model.Owner_Name)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Owner_Address)
            @Html.EditorFor(Function(model) model.Owner_Address)
            @Html.ValidationMessageFor(Function(model) model.Owner_Address)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Phone_Number)
            @Html.EditorFor(Function(model) model.Phone_Number)
            @Html.ValidationMessageFor(Function(model) model.Phone_Number)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Deed_Name)
            @Html.EditorFor(Function(model) model.Deed_Name)
            @Html.ValidationMessageFor(Function(model) model.Deed_Name)
        </div>
        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Remarks)
            @Html.EditorFor(Function(model) model.Remarks)
            @Html.ValidationMessageFor(Function(model) model.Remarks)
        </div>
        <div class="ui-grid-a">
            <div class="ui-block-a">
                @Html.ActionLink("Cancel", "Index", Nothing, New With {.data_role = "button"})
                </div>
            <div class="ui-block-b">
                <input type="submit" data-theme="b" value="Save" class="startProcessing"/></div>
        </div>
    </div>
End Using
        </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map" data-icon="mappin" data-ajax="false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner" data-icon="user" data-ajax="false" class="startProcessing">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript" src="~/Scripts/Deed-create-ui.js"></script>
End Section