@ModelType System.DateTime?

@Html.TextBox("", ViewData.TemplateInfo.FormattedModelValue, New With {.type = "date"})