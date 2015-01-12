@ModelType wincem.Owner
<div style="font: bold 22px times; text-align: center; padding: 20px; width:800px; font-family: 'Bernard MT'">Certificate of Ownership</div>
<table style="width: 800px; border-collapse: collapse; font-family: Arial">
    <tr><td style="width: 130px;padding: 2px">Deed No.:</td><td colspan="2">@Model.Deed_No</td><td style="text-align: right">Amount Paid : @Format(Model.Amount_Paid, "$#,###.00")</td></tr>
    <tr><td style="width: 130px;padding: 2px">Issued To:</td><td colspan="3">@IIf(String.IsNullOrEmpty(Model.Deed_Name), Model.Owner_Name, Model.Deed_Name)</td></tr>
    <tr><td style="width: 130px;padding: 2px">Address:</td><td colspan="3">@Model.Owner_Address</td></tr>
    <tr><td style="width: 130px;padding: 2px">Space No.:</td><td>@Model.spaceList().Item(0)</td><td>Remarks:</td><td rowspan="6"></td></tr>
    <tr><td style="width: 130px;padding: 2px">Lot/Row:</td><td>@Model.lotRowList().Item(0)</td></tr>
    <tr><td style="width: 130px;padding: 2px">Block:</td><td>@Model.blockList().Item(0)</td></tr>
    <tr><td style="width: 130px;padding: 2px">Division:</td><td>@Model.divisionList().Item(0)</td></tr>
    <tr><td style="width: 130px;padding: 2px">Cemetery:</td><td>@Model.cemeteryList().Item(0)</td></tr>
    <tr><td style="width: 130px;padding: 2px">Phone:</td><td>@Model.Phone_Number</td></tr>
    <tr><td style="width: 130px;padding: 2px">Date of Purchase:</td><td colspan="3">@Model.Date_of_Purchase.Value.ToShortDateString</td></tr>

</table>
<br />
<br />
<div style="width: 800px; border: 20px outset lightgrey; padding:10px; font-family: 'Bernard MT';">
    <div style="display: inline-block; width: 398px;">Deed No. @Model.Deed_No</div>
    <div style="display: inline-block; width: 398px; text-align: right">Amount Paid: @Format(Model.Amount_Paid, "$#,###.00")</div>
    <div style="font: bold 22px times; text-align: center; padding: 20px;">Certificate of Ownership</div>
    <div style="font: italic; text-align: right; padding-bottom: 15px;">Winfield, Kansas, @Model.Date_of_Purchase.Value.ToString("MMMM dd, yyyy")</div>
    <div>THIS IS TO CERTIFY, That @IIf(String.IsNullOrEmpty(Model.Deed_Name), Model.Owner_Name, Model.Deed_Name) has purchased of the City of Winfield, Kansas, Space No. @Model.spaceList().Item(0) in Lot/Row No. @Model.lotRowList().Item(0)
        in Block No. @Model.blockList().Item(0), @Model.divisionList().Item(0) Division or Re-Plat, @Model.cemeteryList().Item(0) Cemetery, of THE WINFIELD PUBLIC CEMETERIES, for the sum
        of @Format(Model.Amount_Paid, "$#,###.00") Dollars, the receipt of which is hereby acknowledged: and it is further acknowledged that said purchase is the owner of said 
        property for the purpose of internment only.
        <br />
        <br />
        And the City of Winfield obligates itself to forever warrant and defend the title and peaceable possession of the same to the said @IIf(String.IsNullOrEmpty(Model.Deed_Name), Model.Owner_Name, Model.Deed_Name), the heirs and assigns, 
        on condition that said land be used only for burying the dead, under the provisions of the Ordinances of said City relating to Cemeteries, assignment of any portion of 
        interest being valid only upon the written consent of the City of Winfield.
        <br />
        <br />
        IN WITNESS WHEREOF, The City of Winfield has, this @ViewBag.DayWithSuffix day of @Model.Date_of_Purchase.Value.ToString("MMMM, yyyy"), caused this certificate to be signed by its Mayor and City Clerk.
        <br />
        <br />
        Attest:
        <br />
        <br />
        <div style="display: inline-block; padding-left: 50px;"><div style="border-bottom: solid black 1px; width: 250px; display: inline-block"><img src="~/Images/Brenda_Peters.png" /></div><br />City Clerk</div>
        <div style="display: inline-block; padding-left: 100px;"><div style="border-bottom: solid black 1px; width: 250px; display: inline-block">
            <img src="~/Images/Thomas_McNeish.png" height="36" /></div><br />Mayor of Winfield</div>
    </div>
</div>