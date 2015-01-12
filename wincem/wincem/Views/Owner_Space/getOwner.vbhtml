@ModelType wincem.Owned_Space

@Html.ActionLink("Owner Data", "Details", "Owner", New With {.id = Model.OwnerID}, New With {.data_role="button", .data_icon="arrow-r", .data_iconpos = "right", .id="ownerIdentifyButton" + Model.OwnerID.ToString})