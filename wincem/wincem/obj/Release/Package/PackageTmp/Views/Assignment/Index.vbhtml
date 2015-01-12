@ModelType IEnumerable(Of wincem.Assignment)

@Code
    ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table>
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Owned_Space.Space_ID)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Assignment_Date)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Original_OwnerID)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.New_OwnerID)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    Dim currentItem = item
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Owned_Space.Space_ID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Assignment_Date)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Original_OwnerID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.New_OwnerID)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = currentItem.ID}) |
            @Html.ActionLink("Details", "Details", New With {.id = currentItem.ID}) |
            @Html.ActionLink("Delete", "Delete", New With {.id = currentItem.ID})
        </td>
    </tr>
Next

</table>
