@Code
    ViewData("Title") = "Index"
End Code
        <div data-role="page" id="mapPage">
            <div data-role="panel" id="searchPanel" data-position="right" data-display="overlay" data-position-fixed="true">
                <!-- panel content goes here -->
                <label for="searchString">Search Input:</label>
                <input type="search" name="searchString" id="searchString" value="" />
                <fieldset data-role="controlgroup" data-type="horizontal">
                    <legend>Search Options:</legend>
                    <input type="radio" name="radio-Search-Options" id="radio-Search-Options-Burials" value="Burials" checked="checked">
                    <label for="radio-Search-Options-Burials">Burials</label>
                    <input type="radio" name="radio-Search-Options" id="radio-Search-Options-Deeds" value="Deeds">
                    <label for="radio-Search-Options-Deeds">Deeds</label>
                </fieldset>
                <div data-role="popup" id="burialPopup" data-theme="a">
                    <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
                    <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
                        <li data-icon="delete"><a href="#" id="editLink">Edit</a></li>
                        <li data-icon="delete"><a href="#" id="deleteLink" data-rel="dialog" data-transition="slidedown">Delete</a></li>
                    </ul>
                </div>
                <div style="position: absolute; bottom: 0px; right: 0px; left: 0px; top: 175px;" id="mapSearchResults">
                </div>
            </div>
            <div data-role="panel" id="toolsPanel" data-position="left" data-display="overlay" data-dismissible="false">
                <!-- panel content goes here -->
                <div data-role="header" data-position="fixed" data-theme="a">
                    <h3>Tools</h3>
                </div>
                <br />
                <br />
                <a href="#bookmarkMenu" id="bookmarkButton" data-role="button" data-rel="popup" data-icon="book">Bookmarks</a>
                <div data-role="popup" id="bookmarkMenu" data-theme="a">
                    <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
                        <li data-role="divider" data-theme="a">Bookmarks</li>
                        <li><a href="#" id="Highland" data-ajax="false" class="startProcessing">Highland</a></li>
                        <li><a href="#" id="Graham" data-ajax="false" class="startProcessing">Graham</a></li>
                        <li><a href="#" id="St_Marys" data-ajax="false" class="startProcessing">St Mary's</a></li>
                        <li><a href="#" id="Union" data-ajax="false" class="startProcessing">Union</a></li>
                    </ul>
                </div>
                <div data-role="collapsible-set" data-theme="c" data-content-theme="d">
                    @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Deed_Editors") Then
                        @<div data-role="collapsible" data-theme="a" data-content-theme="b" id="createDeedCollapse">
                            <h3 id="createDeedHeader">Create Deed</h3>
                            <p>Add Space(s) by clicking the map</p>
                            <ul data-role="listview" data-inset="true" id="createDeedList" data-icon="delete">
                                <li data-role="list-divider" role="heading" data-theme="a">Spaces()
                                </li>

                            </ul>
                            <fieldset class="ui-grid-a">
                                <div class="ui-block-a">
                                    <button id="createDeedCancel" data-theme="c">Cancel</button>
                                </div>
                                <div class="ui-block-b">
                                    <button id="createDeedOK" data-theme="b">OK</button>
                                </div>
                            </fieldset>
                        </div>
                    Else
                        @<div data-role="collapsible" data-theme="a" data-content-theme="b" id="createDeedCollapse" class="ui-disabled">
                            <h3 id="createDeedHeader">Create Deed</h3>
                            <p>Add Space(s) by clicking the map</p>
                            <ul data-role="listview" data-inset="true" id="createDeedList" data-icon="delete">
                                <li data-role="list-divider" role="heading" data-theme="a">Spaces()
                                </li>

                            </ul>
                            <fieldset class="ui-grid-a">
                                <div class="ui-block-a">
                                    <button id="createDeedCancel" data-theme="c">Cancel</button>
                                </div>
                                <div class="ui-block-b">
                                    <button id="createDeedOK" data-theme="b">OK</button>
                                </div>
                            </fieldset>
                        </div>
                    End If

                    @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Deed_Editors") Then
                        @<div data-role="collapsible" data-theme="a" data-content-theme="b" id="createAssignmentCollapse">
                            <h3 id="createAssignmentHeader">Assign Space(s)</h3>
                            <p>Add Space(s) by clicking the map</p>
                            <ul data-role="listview" data-inset="true" id="createAssignmentList" data-icon="delete">
                                <li data-role="list-divider" role="heading" data-theme="a">Spaces</li>
                            </ul>
                            <fieldset class="ui-grid-a">
                                <div class="ui-block-a">
                                    <button id="createAssignmentCancel" data-theme="c">Cancel</button>
                                </div>
                                <div class="ui-block-b">
                                    <button id="createAssignmentOK" data-theme="b">OK</button>
                                </div>
                            </fieldset>
                        </div>
                    Else
                        @<div data-role="collapsible" data-theme="a" data-content-theme="b" id="createAssignmentCollapse" class="ui-disabled">
                            <h3 id="createAssignmentHeader">Assign Space(s)</h3>
                            <p>Add Space(s) by clicking the map</p>
                            <ul data-role="listview" data-inset="true" id="createAssignmentList" data-icon="delete">
                                <li data-role="list-divider" role="heading" data-theme="a">Spaces</li>
                            </ul>
                            <fieldset class="ui-grid-a">
                                <div class="ui-block-a">
                                    <button id="createAssignmentCancel" data-theme="c">Cancel</button>
                                </div>
                                <div class="ui-block-b">
                                    <button id="createAssignmentOK" data-theme="b">OK</button>
                                </div>
                            </fieldset>
                        </div>
                    End If
                @*<a href="#dialog" id="burialReport" data-role="button" data-rel="dialog" data-icon="book">Burial Report</a>*@
                <div data-role="collapsible" id="burialReportMenu" data-content-theme="b" data-theme="a">
                    <h3 id ="burialReportHeader">Burial Report</h3>
                        <label for="start_date">Start Date</label>
                        <input type="date" id="start_date" value="@Date.Now.ToString("yyyy-01-01")" class="burialReport"/>

                        <label for="end_date">End Date</label>
                        <input type="date" id="end_date" value="@Date.Now.ToString("yyyy-MM-dd")" class="burialReport"/>

                    <select style="min-width: 210px;" data-native-menu="false" class="burialReport" id="burialReportCemetery">
                        <option value="">Select a Cemetery</option>
                        <option value="1">Highland</option>
                        <option value="3">Graham</option>
                        <option value="4">St Mary's</option>
                        <option value="2">Union</option>
                        <option value="0">All</option>
                    </select>
                    <div class="ui-grid-a">
                        <div class="ui-block-a">
                            <a href="#" data-role="button" data-theme="c" id ="burialReportMenuClose">Cancel</a>
                        </div>
                        <div class="ui-block-b">
                            <a href="burial/burialreport?cemetery=0&startDate=@Date.Now.ToString("yyyy-01-01")&endDate=@Date.Now.ToString("yyyy-MM-dd")" id="burialReportOK" data-role="button" target="_blank" data-theme="b">OK</a>
                        </div>
                    </div>
                </div>
                @*<a href="#" id="deedReport" data-role="button" data-rel="popup" data-icon="book">Deed Report</a>*@
                @*<a href="#" id="burialTotals" data-role="button" data-rel="popup" data-icon="book">Burial Totals</a>*@
                <div data-role="collapsible" id="burialTotalsCollapse" data-content-theme="b" data-theme="a">
                    <h3 id ="burialTotalsHeader">Burial Totals</h3>
                        <label for="bt_start_date">Start Date</label>
                        <input type="date" id="bt_start_date" value="@Date.Now.ToString("yyyy-01-01")" class="burialReport"/>

                        <label for="bt_end_date">End Date</label>
                        <input type="date" id="bt_end_date" value="@Date.Now.ToString("yyyy-MM-dd")" class="burialReport"/>

                    <div class="ui-grid-a">
                        <div class="ui-block-a">
                            <a href="#" data-role="button" data-theme="c" id="burialTotalsCollapseClose">Cancel</a>
                        </div>
                        <div class="ui-block-b">
                            <a href="#" id="burialTotals" data-role="button" data-rel="popup" data-theme="b">OK</a>
                        </div>
                    </div>
                </div>
                @*<a href="#" id="deedTotals" data-role="button" data-rel="popup" data-icon="book">Deed Totals</a>*@
                <div data-role="collapsible" id="deedTotalsCollapse" data-content-theme="b" data-theme="a">
                    <h3 id ="DeedTotalsHeader">Deed Totals</h3>
                        <label for="dt_start_date">Start Date</label>
                        <input type="date" id="dt_start_date" value="@Date.Now.ToString("yyyy-01-01")" class="burialReport"/>

                        <label for="dt_end_date">End Date</label>
                        <input type="date" id="dt_end_date" value="@Date.Now.ToString("yyyy-MM-dd")" class="burialReport"/>

                    <div class="ui-grid-a">
                        <div class="ui-block-a">
                            <a href="#" data-role="button" data-theme="c" id ="deedTotalsCollapseClose">Cancel</a>
                        </div>
                        <div class="ui-block-b">
                            <a href="#" id="deedTotals" data-role="button" data-rel="popup" data-theme="b">OK</a>
                        </div>
                    </div>
                </div>
                    </div>
                <div data-role="popup" id="burialTotalPopup" data-theme="b">
                    <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                    <div id="burialTotalPopupContent"></div>
                </div>
                <div data-role="popup" id="deedTotalPopup" data-theme="b">
                    <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-right">Close</a>
                    <div id="deedTotalPopupContent"></div>
                </div>
                    <button id="toolsPanelClose" data-theme="b" data-icon="delete">Close</button>
            </div>
            <div data-theme="b" data-role="header" data-position="fixed">
                <h3>Winfield Cemetery Management System
                </h3>
                <a href="#searchPanel" data-icon="search" class="ui-btn-right">Search</a>
                <a href="#toolsPanel" data-icon="gear" class="ui-btn-left">Tools</a>
            </div>
            <div data-role="content" id="mapContent">
                <div id="mapDiv"></div>
                <div data-role="popup" id="identifyPopup" data-dismissible="false" data-theme="none" data-corners="false" data-postion-to="window" data-transition="slide" data-tolerance="0,0">
                    <div data-role="header" role="banner" class="ui-corner-top ui-header ui-bar-a">
                        <h1 class="ui-title" role="heading">Space Info.</h1>
                    </div>
                    <div id="identifyContent" style="padding: 10px; color: white;"></div>
                    <button id="identifyClose" data-theme="b" data-icon="delete" data-ajax="false">Close</button>
                </div>
                <div data-role="popup" id="selectionErrorPopup" data-overlay-theme="a" data-theme="c" style="max-width: 400px; padding: 15px;" class="ui-corner-all">
                    Please select an availble space!
                </div>
            </div>
            <div data-role="footer" data-position="fixed">
                <div data-role="navbar">
                    <ul>
                        <li><a href="~/Map" data-icon="mappin" data-ajax="false" class="startProcessing">Map</a></li>
                        <li id="deedsNav"><a href="~/Owner" data-icon="user" data-ajax="false" class="startProcessing">Deeds</a></li>
                        <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                    </ul>
                </div>
                <!-- /navbar -->
            </div>
        </div>
@Section Scripts
    <script src="~/Scripts/map.js"></script>
end section