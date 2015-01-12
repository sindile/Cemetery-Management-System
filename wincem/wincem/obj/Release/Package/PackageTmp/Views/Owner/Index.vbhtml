@ModelType IEnumerable(Of wincem.Owner)

@Code
    ViewData("Title") = "Index"
End Code

    <div data-role="page" id="ownerPage">
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
            <h3>Deeds
            </h3>
                @Html.ActionLink("Create New", "Create", Nothing, New With {.data_role = "button", .data_icon = "plus", .class = "ui-btn-right processingStart", .data_ajax="false"})
            <a href="http://wincem.cloudapp.net/wincem" data-icon="home" class="ui-btn-left" data-ajax="false">Home</a>
        </div>
        <div data-role="content" id="ownerContent">
                <div>
                    <input type="search" name="searchString_deed" id="searchString_deed" value="" placeholder="Search Deeds ..." />
                    <div data-role="collapsible" data-theme="b" data-content-theme="c">
                        <h3>Advanced Search Options</h3>
                        <div data-role="fieldcontain">
                            <label for="number_of_records_deed"># of records to return: </label>
                            <input type="range" name="number_of_records_deed" id="number_of_records_deed" value="50" min="5" max="500" step="5" data-highlight="true" />
                        </div>
                        <div data-role="fieldcontain">
                            <fieldset data-role="controlgroup">
                                <legend>Fields to Search: </legend>
                                <input type="checkbox" name="deed_number" id="deed_number" value="deed_number" checked="checked" data-theme="b" />
                                <label for="deed_number">Deed Number</label>

                                <input type="checkbox" name="Date_of_Purchase" id="Date_of_Purchase" value="Date_of_Purchase" checked="checked" data-theme="b" />
                                <label for="Date_of_Purchase">Date of Purchase</label>

                                <input type="checkbox" name="Owner_Name" id="Owner_Name" value="Owner_Name" checked="checked" data-theme="b" />
                                <label for="Owner_Name">Owner Name</label>

                                <input type="checkbox" name="Deed_Name" id="Deed_Name" value="Deed_Name" checked="checked" data-theme="b" />
                                <label for="Deed_Name">Deed Name</label>
                            </fieldset>
                        </div>
                    </div>
                </div>
            <div id="deedListDiv">
                <ul data-role="listview" data-inset="true" id="deedList">
                    @For Each item In Model
                        Dim currentItem = item
                        @<li id="@currentItem.ID" class="deedRecord">
                            @If currentItem.Date_of_Purchase.HasValue Then
                                @Html.ActionLink(currentItem.Owner_Name + " - " + currentItem.Date_of_Purchase.Value.ToShortDateString, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            ElseIf currentItem.Owner_Name Is Nothing Then
                                @Html.ActionLink("No Information", "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            Else
                                @Html.ActionLink(currentItem.Owner_Name, "Details", New With {.id = currentItem.ID}, New With {.data_ajax = "false"})
                            End If
                        </li>
                    Next
                </ul>
            </div>
            <div data-role="popup" id="deedPopup" data-theme="a">
                <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete" data-iconpos="notext" class="ui-btn-left">Close</a>
                <ul data-role="listview" data-inset="true" style="min-width: 210px;" data-theme="b">
                        <li data-icon="pencil"><a href="#" id="editLink">Edit</a></li>
                        <li data-icon="delete"><a href="#" id="deleteLink" data-rel = "dialog" data-transition="slidedown">Delete</a></li>
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

    <script type="text/javascript" src="~/Scripts/Deed-index-ui.js"></script>