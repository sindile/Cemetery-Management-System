<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" >
        <meta name="viewport" id="viewport"
  content="width=device-width, height=device-height,
  initial-scale=1.0, maximum-scale=1.0,
  user-scalable=no;" />
        <title>@ViewData("Title") - Winfield Cemetery Management System</title>
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <meta name="viewport" content="width=device-width" />
        @Scripts.Render("~/bundles/modernizr")
        <link rel="stylesheet" href="~/Content/jquery.mobile-1.3.1.min.css" />
        <link href="~/Content/jQuery-Mobile-Icon-Pack/font-awesome/jqm-icon-pack-fa.css" rel="stylesheet" />
        <link href="~/Content/jQuery-Mobile-Icon-Pack/original/jqm-icon-pack-2.0-original.css" rel="stylesheet" />
        <link rel="stylesheet" href="http://arcgis/arcgis_js_api/library/3.5/3.5compact/js/esri/css/esri.css">
        <script src="~/Scripts/config.js"></script>
        @Scripts.Render("~/bundles/jquery")
        <script src="~/Scripts/json2.js"></script>
        <script type="”text/javascript”">
            $(document).bind(“mobileinit”, function () {
                $.mobile.ajaxEnabled = true;
                $.mobile.loader.prototype.options.text = "loading";
                $.mobile.loader.prototype.options.textVisible = true;
                $.mobile.loader.prototype.options.theme = "b";
                $.mobile.loader.prototype.options.html = "";
            });
        </script>
        <script src="~/Scripts/jquery.mobile-1.3.1.min.js" type="text/javascript"></script>
        <script src="http://arcgis/arcgis_js_api/library/3.5/3.5compact/init.js" type="text/javascript"></script>
        <script src="~/Scripts/ui.js"></script>

        <style>
            html, body, #mapContent, #mapDiv {
                padding: 0;
                margin: 0;
                height: 100%;
            }

            #mapPage .ui-content {
                position: absolute;
                top: 0px;
                right: 0;
                bottom: 150px;
                left: 0;
            }

            #identifyPopup-screen {
                display: none;
            }
            #toolsPanel-screen {
                display: none;
            }

            [id$=-screen] {
                display: none;
            }

            #identifyPopup-popup {
                right: 0 !important;
                left: auto !important;
            }

            #identifyPopup {
                width: 270px;
                border: 1px solid #000;
                border-right: none;
                background: rgba(0,0,0,.5);
                margin: -1px 0;
            }

                #identifyPopup .ui-btn {
                    margin: 2em 15px;
                }
                .ui-field-contain div.ui-slider-switch { width: 9em; }
        </style>
    </head>
    <body>
@*        <header>
            <div class="content-wrapper">
                <div class="float-left">
                    <p class="site-title">@Html.ActionLink("your logo here", "Index", "Home")</p>
                </div>
                <div class="float-right">
                    <section id="login">
                        Hello, <span class="username">@User.Identity.Name</span>!
                    </section>
                    <nav>
                        <ul id="menu">
                            <li>@Html.ActionLink("Home", "Index", "Home")</li>
                            <li>@Html.ActionLink("About", "About", "Home")</li>
                            <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
        <div id="body">*@
      
                @RenderSection("featured", required:=False)
                @RenderBody()

            
@*        </div>
        <footer>
            <div class="content-wrapper">
                <div class="float-left">
                    <p>&copy; @DateTime.Now.Year - My ASP.NET MVC Application</p>
                </div>
            </div>
        </footer>*@

@*        @Scripts.Render("~/bundles/jquery")
        <script src="http://arcgis/arcgis_js_api/library/3.5/3.5compact/init.js" type="text/javascript"></script>
        <script src="~/Scripts/jquery.mobile-1.3.1.min.js" type="text/javascript"></script>*@
        @RenderSection("scripts", required:=False)
    </body>
</html>
