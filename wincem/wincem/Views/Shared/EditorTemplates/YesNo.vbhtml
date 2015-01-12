@ModelType String

<select name="@ViewData.TemplateInfo.HtmlFieldPrefix" id="@ViewData.TemplateInfo.HtmlFieldPrefix" data-role="slider" data-theme="c" data-track-theme="a">
    @If ViewData.TemplateInfo.FormattedModelValue = 0 Then
        @<option value="0" selected ="selected">No</option>
    Else
        @<option value="0">No</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = 1 Then
        @<option value="1" selected ="selected">Yes</option>
    Else
        @<option value="1">Yes</option>
    End If
</select>
