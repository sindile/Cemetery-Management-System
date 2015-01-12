@ModelType String

@Html.TextBox("", ViewData.TemplateInfo.FormattedModelValue, New With {.disabled = "disabled"})