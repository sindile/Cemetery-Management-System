@ModelType IEnumerable(Of wincem.Burial)

@Code
    ViewData("Title") = "Index"
End Code

    <div data-role="page" id="burialPage">
        <div data-role="panel" id="searchPanel" data-position="right" data-display="overlay" data-position-fixed="true">
            <!-- panel content goes here -->
            <label for="search-basic">Search Input:</label>
            <input type="search" name="search" id="search-basic" value="" />
            <br />
            <br />
            <div style="position: absolute; bottom: 0px; right: 0px; left: 0px; top: 100px;">
                <ul data-role="listview" id="searchListView">
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                    <li><a href="#">Test</a></li>
                </ul>
            </div>
        </div>
        <div data-role="panel" id="optionsPanel" data-position="left" data-display="overlay">
            <!-- panel content goes here -->
            Options
        </div>
        <div data-theme="b" data-role="header" data-position="fixed">
            <h3>Burials
            </h3>
            @Html.ActionLink("Home", "Index", "Map", Nothing, New With {.data_icon = "home", .class = "ui-btn-left", .data_ajax = "false"})
        </div>
        <div data-role="content" id="burialContent">
                <div>
                    <input type="search" name="searchString_burial" id="searchString_burial" value="" placeholder="Search Burials ..." />
                    <div data-role="collapsible" data-theme="a" data-content-theme="c">
                        <h3>Advanced Search Options</h3>
                        <div data-role="fieldcontain">
                            <label for="number_of_records_burial"># of records to return: </label>
                            <input type="range" name="number_of_records_burial" id="number_of_records_burial" value="50" min="5" max="500" step="5" data-highlight="true" />
                        </div>
                        <div data-role="fieldcontain">
                            <fieldset data-role="controlgroup">
                                <legend>Fields to Search: </legend>
                                <input type="checkbox" name="First_Name" id="First_Name" value="First_Name" checked="checked" data-theme="b" />
                                <label for="First_Name">First Name</label>

                                <input type="checkbox" name="Last_Name" id="Last_Name" value="Last_Name" checked="checked" data-theme="b" />
                                <label for="Last_Name">Last Name</label>

                                <input type="checkbox" name="Permit_Number" id="Permit_Number" value="Permit_Number" checked="checked" data-theme="b" />
                                <label for="Permit_Number">Burial Permit Number</label>

                                <input type="checkbox" name="Burial_Date" id="Burial_Date" value="Burial_Date" checked="checked" data-theme="b" />
                                <label for="Burial_Date">Burial Date</label>

                                <input type="checkbox" name="Work_Order_Date" id="Work_Order_Date" value="Work_Order_Date" checked="checked" data-theme="b" />
                                <label for="Work_Order_Date">Work Order Date</label>
                            </fieldset>
                        </div>
                    </div>
                </div>
            <div id="burialListDiv">
                <ul data-role="listview" data-inset="true" id="burialList">
                    @For Each item In Model
                        Dim currentItem = item
                        @<li id="@currentItem.ID" class="burialRecord">
                            @If currentItem.Date_of_Death.HasValue Then
                                @Html.ActionLink(currentItem.Last_Name + ", " + currentItem.First_Name + " - " + currentItem.Date_of_Death.Value.ToShortDateString, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            ElseIf currentItem.Last_Name Is Nothing And currentItem.First_Name Is Nothing Then
                                @Html.ActionLink("No Information", "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            Else
                                @Html.ActionLink(currentItem.Last_Name + ", " + currentItem.First_Name, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            End If
                        </li>
                    Next
                </ul>
            </div>
            <div data-role="popup" id="burialPopup" data-theme="a">
                <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
                <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
                    @If User.IsInRole("Cem_Admins") Or User.IsInRole("Cem_Burial_Editors") Then
                        @<li data-icon="pencil"><a href="#" id="editLink">Edit</a></li>
                    Else
                        @<li data-icon="pencil" class="ui-disabled"><a href="#" id="editLink">Edit</a></li>
                    End If

                    @If User.IsInRole("Cem_Admins") Then
                        @<li data-icon="delete"><a href="#" id="deleteLink" data-rel = "dialog" data-transition="slidedown">Delete</a></li>
                    Else
                        @<li data-icon="delete" class="ui-disabled"><a href="#" id="deleteLink" data-rel = "dialog" data-transition="slidedown">Delete</a></li>
                    End If
                    
                    
                </ul>
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="~/Map" data-icon="mappin" data-ajax="false" class="startProcessing">Map</a></li>
                    <li><a href="~/Owner" data-icon="user" data-ajax="false" class="startProcessing">Deeds</a></li>
                    <li><a href="~/Burial" data-icon="moon" data-ajax="false" class="startProcessing">Burials</a></li>
                </ul>
            </div>
            <!-- /navbar -->
        </div>
    </div>
 <script type="text/javascript" src="~/Scripts/Burial-index-ui.js"></script>