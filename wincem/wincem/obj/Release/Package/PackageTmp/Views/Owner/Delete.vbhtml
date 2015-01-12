@ModelType wincem.Owner

@Code
    ViewData("Title") = "Delete"
End Code

<div data-role="page" id="ownerDeletePage" data-add-back-btn="true">
    <div data-theme="b" data-role="header" data-position="fixed">
        <h3>Delete Deed?</h3>
    </div>
    <div data-role="content" id="ownedSpaceCreateContent">
        <br />
        <h3>Are you sure you want to delete this Deed?</h3>
        <br />
        @Using Html.BeginForm("Delete", "Owner", FormMethod.Post, New With {.data_ajax = "false"})
            @<div class="ui-grid-a">
                <div class="ui-block-a"><a href="~/Owner" data-role="button" data-rel="back" data-theme="c">Cancel</a></div>
                <div class="ui-block-b"><input type="submit" value="Delete" data-theme="b"/></div>
            </div>
        End Using
    </div>
</div>