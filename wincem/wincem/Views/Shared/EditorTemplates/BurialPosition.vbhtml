@ModelType String

<select name="@ViewData.TemplateInfo.HtmlFieldPrefix" id="@ViewData.TemplateInfo.HtmlFieldPrefix" data-native-menu="false">    
    
    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "" Then
        @<option value="" selected ="selected">Select Burial Postion</option>
    Else
        @<option value="">Select Burial Postion</option>
    End If
    
    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "CREMATION1" Then
        @<option value="Cremation1" selected ="selected">Cremation 1</option>
    Else
        @<option value="Cremation1">Cremation 1</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "CREMATION2" Then
        @<option value="Cremation2" selected ="selected">Cremation 2</option>
    Else
        @<option value="Cremation2">Cremation 2</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "CREMATION3" Then
        @<option value="Cremation3" selected ="selected">Cremation 3</option>
    Else
        @<option value="Cremation3">Cremation 3</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "FULL_BURIAL" Then
        @<option value="Full_Burial" selected ="selected">Full Burial</option>
    Else
        @<option value="Full_Burial">Full Burial</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "FULL_BURIAL_2_SPACES_EW" Then
        @<option value="Full_Burial_2_Spaces_EW" selected ="selected">Full Burial - 2 Spaces East/West</option>
    Else
        @<option value="Full_Burial_2_Spaces_EW">Full Burial - 2 Spaces East/West</option>
    End If

     @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "FULL_BURIAL_2_SPACES_NS" Then
        @<option value="Full_Burial_2_Spaces_NS" selected ="selected">Full Burial - 2 Spaces North/South</option>
    Else
        @<option value="Full_Burial_2_Spaces_NS">Full Burial - 2 Spaces North/South</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "INFANT1" Then
        @<option value="Infant1" selected ="selected">Infant 1</option>
    Else
        @<option value="Infant1">Infant 1</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "INFANT2" Then
        @<option value="Infant2" selected ="selected">Infant 2</option>
    Else
        @<option value="Infant2">Infant 2</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "INFANT3" Then
        @<option value="Infant3" selected ="selected">Infant 3</option>
    Else
        @<option value="Infant3">Infant 3</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "INFANT4" Then
        @<option value="Infant4" selected ="selected">Infant 4</option>
    Else
        @<option value="Infant4">Infant 4</option>
    End If

    @If ViewData.TemplateInfo.FormattedModelValue.ToString.ToUpper() = "MEMORIALHEADSTONE" Then
        @<option value="MemorialHeadstone" selected ="selected">Memorial Headstone</option>
    Else
        @<option value="MemorialHeadstone">Memorial Headstone</option>
    End If
</select>