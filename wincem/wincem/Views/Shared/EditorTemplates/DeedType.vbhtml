@ModelType String

<select name="@ViewData.TemplateInfo.HtmlFieldPrefix" id="@ViewData.TemplateInfo.HtmlFieldPrefix" data-role="slider" data-theme="c" data-track-theme="a">
    @If ViewData.TemplateInfo.FormattedModelValue = "D" Then
        @<option value="D" selected ="selected">Deed</option>
    Else
        @<option value="D">Deed</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "A" Then
        @<option value="A" selected ="selected">Assignment</option>
    Else
        @<option value="A">Assignment</option>
    End If
</select>
