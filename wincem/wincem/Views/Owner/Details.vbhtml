@ModelType wincem.Owner

@Code
    ViewData("Title") = "Details"
End Code

    <div data-role="page" id="ownerDetailsPage">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Deed Details
            </h3>
            @*@Html.ActionLink("Back", "Index", Nothing, New With {.data_role = "button", .data_icon = "arrow-l"})*@
            @Html.ActionLink("Home", "Index", "Map", Nothing, New With {.data_icon = "home", .class = "ui-btn-left", .data_ajax = "false"})
            @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Deed_Editors") Then
                @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "pencil", .class = "ui-btn-right"})
            Else
                @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "pencil", .class = "ui-btn-right ui-disabled"})
            End If
        </div>
        <div data-role="content" id="ownerEditContent">

@Using Html.BeginForm()
    @Html.ValidationSummary(True)

    @<div>
        @Html.HiddenFor(Function(model) model.ID)
        @Html.HiddenFor(Function(model) model.LF_Deed_Card)
        @Html.HiddenFor(Function(model) model.LF_Cert_of_Ownership)
        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Deed_No)
            @Html.EditorFor(Function(model) model.Deed_No)
            @Html.ValidationMessageFor(Function(model) model.Deed_No)
        </div>

        <div data-role="fieldcontain">
                @Html.LabelFor(Function(model) model.Deed_Type)
                @Html.EditorFor(Function(model) model.Deed_Type, "DeedType")
                @Html.ValidationMessageFor(Function(model) model.Deed_Type)
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
        @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Deed_Editors") Then
                If Model.Deed_Type = "A" Then
                    @Html.ActionLink("View Assignment of Interest (PDF)", "assignmentOfInterest", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .target = "_blank"})
        Else
                    @Html.ActionLink("View Certificate of Ownership (PDF)", "certificateOfOwnership", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .target = "_blank"})
                    
        End If
            @Html.ActionLink("View Deed Card (PDF)", "DeedCard", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .target = "_blank"})
        Else
            @Html.ActionLink("View Certificate of Ownership (PDF)", "certificateOfOwnership", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .target = "_blank", .class = "ui-disabled"})
            @Html.ActionLink("View Deed Card (PDF)", "DeedCard", "Owner", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-r", .data_iconpos = "right", .target = "_blank", .class = "ui-disabled"})
        End If
        @If Not String.IsNullOrEmpty(Model.LF_Deed_Card) Then
               @<a href ="@Model.LF_Deed_Card" data-role="button" data-icon="arrow-r" target="_blank" data-iconpos="right">View Deed Card in Laserfiche</a>
            End If
        @Html.Partial("Owner_Space_List", Model.Spaces.OrderBy(Function(o) o.Space_ID), New ViewDataDictionary From {{"Owner_ID", Model.ID}, {"Deed_Type", Model.Deed_Type}})
    </div>
        End Using
        </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map?SpaceList=@(ViewBag.SpaceList)" data-icon="mappin" data-ajax="false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner" data-icon="user" data-ajax="false" class="startProcessing">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript" src="~/Scripts/Deed-details-ui.js"></script>
End Section
