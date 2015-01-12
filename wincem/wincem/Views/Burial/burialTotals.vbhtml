<ul data-role="listview" data-inset="true" id="burialTotalsList">
    <li data-role="divider" data-theme="a">Burial Totals <br />(@ViewBag.startDate to @ViewBag.endDate)</li>
    <li>Graham <span class="ui-li-count">@ViewBag.Graham</span></li>
    <li>Highland <span class="ui-li-count">@ViewBag.Highland</span></li>
    <li>St Mary's <span class="ui-li-count">@ViewBag.St_Marys</span></li>
    <li>Union <span class="ui-li-count">@ViewBag.Union</span></li>
    <li data-role="divider" data-theme="a">Total <span class="ui-li-count">@ViewBag.Total</span></li>
</ul>