@ModelType wincem.Assignment

@Code
    ViewData("Title") = "Delete"
End Code

<div data-role="page" id="ownedSpaceDeletePage" data-add-back-btn="true">
    <div data-theme="b" data-role="header" data-position="fixed">
        <h3>Delete Assignment?</h3>
    </div>
    <div data-role="content" id="ownedSpaceCreateContent">
        <br />
        <h3>Are you sure you want to delete this Assignment?</h3>
        <br />
        @Using Html.BeginForm("Delete", "Assignment", FormMethod.Post, New With {.data_ajax = "false"})
            @<div class="ui-grid-a">
                <div class="ui-block-a">@Html.ActionLink("Cancel", "Index", "Burial", New With {.id = ViewBag.space_id}, New With {.data_role = "button", .data_theme = "c"})</div>
                <div class="ui-block-b"><input type="submit" value="Delete" data-theme="b" /></div>
            </div>
        End Using
    </div>
</div>
