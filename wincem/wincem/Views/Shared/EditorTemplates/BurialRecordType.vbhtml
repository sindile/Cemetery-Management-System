@ModelType String

<select name="@ViewData.TemplateInfo.HtmlFieldPrefix" id="@ViewData.TemplateInfo.HtmlFieldPrefix" data-native-menu="false">
    @If ViewData.TemplateInfo.FormattedModelValue = "BURIAL" Then
        @<option value="BURIAL" selected ="selected">Burial</option>
    Else
        @<option value="BURIAL">Burial</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "DISINTERMENT" Then
        @<option value="DISINTERMENT" selected ="selected">Disinterment</option>
    Else
        @<option value="DISINTERMENT">Disinterment</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "INDIVIDUAL_MAUS_SETTING" Then
        @<option value="INDIVIDUAL_MAUS_SETTING" selected ="selected">Individual Maus. Setting</option>
    Else
        @<option value="INDIVIDUAL_MAUS_SETTING">Individual Maus. Setting</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "MEMORIAL_HEADSTONE" Then
        @<option value="MEMORIAL_HEADSTONE" selected ="selected">Memorial Headstone</option>
    Else
        @<option value="MEMORIAL_HEADSTONE">Memorial Headstone</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "PRE_NEED_PAYMENT" Then
        @<option value="PRE_NEED_PAYMENT" selected ="selected">Pre-Need Payment</option>
    Else
        @<option value="PRE_NEED_PAYMENT">Pre-Need Payment</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue = "REINTERMENT" Then
        @<option value="REINTERMENT" selected ="selected">Reinterment</option>
    Else
        @<option value="REINTERMENT">Reinterment</option>
    End If
</select>
