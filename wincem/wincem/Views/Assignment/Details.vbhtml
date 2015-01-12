@ModelType wincem.Assignment

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Assignment</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Owned_Space.Space_ID)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Owned_Space.Space_ID)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Assignment_Date)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Assignment_Date)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Original_OwnerID)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Original_OwnerID)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.New_OwnerID)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.New_OwnerID)
    </div>
</fieldset>
<p>

    @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}) |
    @Html.ActionLink("Back to List", "Index")
</p>
