@ModelType wincem.Burial

@Code
    ViewData("Title") = "Edit"
End Code

    <div data-role="page" id="burialCreatePage">
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Create New Burial
            </h3>
            @Html.ActionLink("Home", "Index", "Map", Nothing, New With {.data_icon = "home", .class = "ui-btn-left", .data_ajax = "false"})
            @*@Html.ActionLink("Back", "Details", New With {.id = Model.ID}, New With {.data_role = "button", .data_icon = "arrow-l"})*@
        </div>
        <div data-role="content" id="burialCreateContent">

@Using Html.BeginForm("Create", "Burial", Nothing, FormMethod.Post, New With {.data_ajax = "false", .enctype = "multipart/form-data"})
    @Html.ValidationSummary(True)

    @<div>

        @Html.HiddenFor(Function(model) model.ID)
        @Html.HiddenFor(Function(model) model.OwnerID)
        @Html.HiddenFor(Function(model) model.Permit_Number)
        @Html.HiddenFor(Function(model) model.LF_Burial_Record)
        @Html.HiddenFor(Function(model) model.Space_ID)
        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Space_ID)
            @Html.EditorFor(Function(model) model.Space_ID, "disabledTextBox")
            @Html.ValidationMessageFor(Function(model) model.Space_ID)
        </div>
        <div data-role="fieldcontain">
            @Html.Label("Burial Position Example", New With {.for = "Burial_Position"})
            <img src="~/Images/@(Model.Burial_Position).png" alt="@(Model.Burial_Position)" id="burialImage" />
        </div>
        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Burial_Position)
            @Html.EditorFor(Function(model) model.Burial_Position, "BurialPosition")
            @Html.ValidationMessageFor(Function(model) model.Space_ID)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Burial_Record_Type)
            @Html.EditorFor(Function(model) model.Burial_Record_Type, "BurialRecordType")
            @Html.ValidationMessageFor(Function(model) model.Burial_Record_Type)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Ordered_BY)
            @Html.EditorFor(Function(model) model.Ordered_BY)
            @Html.ValidationMessageFor(Function(model) model.Ordered_BY)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Ordered_By_Address)
            @Html.EditorFor(Function(model) model.Ordered_By_Address)
            @Html.ValidationMessageFor(Function(model) model.Ordered_By_Address)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Ordered_By_Phone)
            @Html.EditorFor(Function(model) model.Ordered_By_Phone)
            @Html.ValidationMessageFor(Function(model) model.Ordered_By_Phone)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Burial_Date)
            @Html.EditorFor(Function(model) model.Burial_Date)
            @Html.ValidationMessageFor(Function(model) model.Burial_Date)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Burial_Time)
            @Html.EditorFor(Function(model) model.Burial_Time)
            @Html.ValidationMessageFor(Function(model) model.Burial_Time)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Cremation)
            @Html.EditorFor(Function(model) model.Cremation, "YesNo")
            @Html.ValidationMessageFor(Function(model) model.Cremation)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Garden)
            @Html.EditorFor(Function(model) model.Garden, "YesNo")
            @Html.ValidationMessageFor(Function(model) model.Garden)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Type_of_Service)
            @Html.EditorFor(Function(model) model.Type_of_Service)
            @Html.ValidationMessageFor(Function(model) model.Type_of_Service)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Container)
            @Html.EditorFor(Function(model) model.Container)
            @Html.ValidationMessageFor(Function(model) model.Container)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.First_Name)
            @Html.EditorFor(Function(model) model.First_Name)
            @Html.ValidationMessageFor(Function(model) model.First_Name)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Last_Name)
            @Html.EditorFor(Function(model) model.Last_Name)
            @Html.ValidationMessageFor(Function(model) model.Last_Name)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Gender)
            @Html.EditorFor(Function(model) model.Gender, "Gender")
            @Html.ValidationMessageFor(Function(model) model.Gender)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Date_of_Death)
            @Html.EditorFor(Function(model) model.Date_of_Death)
            @Html.ValidationMessageFor(Function(model) model.Date_of_Death)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Date_of_Birth)
            @Html.EditorFor(Function(model) model.Date_of_Birth)
            @Html.ValidationMessageFor(Function(model) model.Date_of_Birth)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Permit_Number)
            @Html.EditorFor(Function(model) model.Permit_Number, "disabledTextBox")
            @Html.ValidationMessageFor(Function(model) model.Permit_Number)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Work_Order_Date)
            @Html.EditorFor(Function(model) model.Work_Order_Date)
            @Html.ValidationMessageFor(Function(model) model.Work_Order_Date)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Remarks)
            @Html.EditorFor(Function(model) model.Remarks)
            @Html.ValidationMessageFor(Function(model) model.Remarks)
        </div>

        <div data-role="fieldcontain">
            <label for="file">Choose Photo</label>
            <input type="file" name="file" accept="image/*" id="file" />            
        </div>

        @*<div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Photo)
            @Html.EditorFor(Function(model) model.Photo)
            @Html.ValidationMessageFor(Function(model) model.Photo)
        </div>*@

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Birth_City_State)
            @Html.EditorFor(Function(model) model.Birth_City_State)
            @Html.ValidationMessageFor(Function(model) model.Birth_City_State)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Birth_County)
            @Html.EditorFor(Function(model) model.Birth_County)
            @Html.ValidationMessageFor(Function(model) model.Birth_County)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Death_City_State)
            @Html.EditorFor(Function(model) model.Death_City_State)
            @Html.ValidationMessageFor(Function(model) model.Death_City_State)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Death_County)
            @Html.EditorFor(Function(model) model.Death_County)
            @Html.ValidationMessageFor(Function(model) model.Death_County)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Other_Information)
            @Html.EditorFor(Function(model) model.Other_Information)
            @Html.ValidationMessageFor(Function(model) model.Other_Information)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Military_Service)
            @Html.EditorFor(Function(model) model.Military_Service)
            @Html.ValidationMessageFor(Function(model) model.Military_Service)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Fees)
            @Html.EditorFor(Function(model) model.Fees)
            @Html.ValidationMessageFor(Function(model) model.Fees)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Pre_Need_Payment)
            @Html.EditorFor(Function(model) model.Pre_Need_Payment)
            @Html.ValidationMessageFor(Function(model) model.Pre_Need_Payment)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Space_Purchase_1)
            @Html.EditorFor(Function(model) model.Space_Purchase_1)
            @Html.ValidationMessageFor(Function(model) model.Space_Purchase_1)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Space_Purchase_2)
            @Html.EditorFor(Function(model) model.Space_Purchase_2)
            @Html.ValidationMessageFor(Function(model) model.Space_Purchase_2)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Amount_Paid)
            @Html.EditorFor(Function(model) model.Amount_Paid)
            @Html.ValidationMessageFor(Function(model) model.Amount_Paid)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Receipt_Number)
            @Html.EditorFor(Function(model) model.Receipt_Number)
            @Html.ValidationMessageFor(Function(model) model.Receipt_Number)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Field_Verified)
            @Html.EditorFor(Function(model) model.Field_Verified)
            @Html.ValidationMessageFor(Function(model) model.Field_Verified)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Headstone_Type)
            @Html.EditorFor(Function(model) model.Headstone_Type)
            @Html.ValidationMessageFor(Function(model) model.Headstone_Type)
        </div>

        <div data-role="fieldcontain">
            @Html.LabelFor(Function(model) model.Footstone_Type)
            @Html.EditorFor(Function(model) model.Footstone_Type)
            @Html.ValidationMessageFor(Function(model) model.Footstone_Type)
        </div>


        <div class="ui-grid-a">
            <div class="ui-block-a">
                @Html.ActionLink("Cancel", "Index", Nothing, New With {.data_role = "button"})
                </div>
            <div class="ui-block-b">
                <button data-theme="b" type="button" id="createButton" class="startProcessing">Save</button></div>
        </div>
    </div>
End Using
        </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map" data-icon="mappin" data-ajax="false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner" data-icon="user" class="startProcessing">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
