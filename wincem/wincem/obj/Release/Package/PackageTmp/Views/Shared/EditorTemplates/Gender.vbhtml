@ModelType String

<select name="@ViewData.TemplateInfo.HtmlFieldPrefix" id="@ViewData.TemplateInfo.HtmlFieldPrefix" data-role="slider" data-theme="c" data-track-theme="a">
    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "MALE" Then
        @<option value="Male" selected ="selected">Male</option>
    Else
        @<option value="Male">Male</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "FEMALE" Then
        @<option value="Female" selected ="selected">Female</option>
    Else
        @<option value="Female">Female</option>
    End If
</select>
