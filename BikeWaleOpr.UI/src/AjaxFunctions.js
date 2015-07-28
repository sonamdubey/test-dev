function FillCombo_Callback(response,cmbToFill, hdnId, dependentCmbs, selectString)
{
	var _delimeter = "|";
	if (response.error != null)
	{
		alert("ERROR : " + response.error);
		return;
	}
	
	var objHdn = document.forms[0][ hdnId ];
	
	//now fill the values to the drop down
		
	if ( cmbToFill )
	{
		//refresh the combo
		clearCombo ( cmbToFill, selectString );
		//also refresh the dependent combos
		if(dependentCmbs)
		{
			for(var i=0; i < dependentCmbs.length; i++)
			{
				var depCmb = document.forms[0][ dependentCmbs[i] ];
				if ( depCmb ) 
				{
					clearCombo ( depCmb, selectString );
				}
			}
		}
		
		var j = 1;
		var ds = response.value;
		
		if(ds != null && typeof(ds) == "object" && ds.Tables != null)
		{
			var content = "";
			for(var i=0; i<ds.Tables[0].Rows.length; i++)
			{
				cmbToFill.options[j] = new Option( ds.Tables[0].Rows[i].Text, 
													ds.Tables[0].Rows[i].Value );
				if(content == "")
					content = ds.Tables[0].Rows[i].Text 
								+ _delimeter
								+ ds.Tables[0].Rows[i].Value;
				else
					content += _delimeter + ds.Tables[0].Rows[i].Text
								+ _delimeter
								+ ds.Tables[0].Rows[i].Value;
				
				j++;
   			}
			
			//add the content to the hidden value
			if(objHdn)
			{
				objHdn.value = content;
			}
			
			if ( j > 1 ) cmbToFill.disabled = false;
			else 
			{	
				cmbToFill.disabled = true;
				if(dependentCmbs)
				{
					for(var i=0; i < dependentCmbs.length; i++)
					{
						var depCmb = document.forms[0][ dependentCmbs[i] ];
						if ( depCmb ) 
						{
							clearCombo ( depCmb, selectString );
							depCmb.disabled = true;
						}
					}
				}
			}
		}
	}
}

function clearCombo( cmb, selectString )
{
	cmb.options.length = null;
	if ( selectString == '' || !selectString ) selectString = "Any" 
	cmb.options[0] = new Option( selectString, 0 );
}

function FillTextBoxes(response, txtExpValidity, txtActualValidity,
		txtExpInquiry, txtActualInquiry, txtExpAmount, txtActualAmount, txtIstopup)
{
	var ds = response.value;
		
	if(ds != null && typeof(ds) == "object" && ds.Tables != null)
	{
		for(var i=0; i < ds.Tables[0].Rows.length; i++)
		{
			//alert(ds.Tables[0].Rows[i].Amount);
			txtExpValidity.value 	= ds.Tables[0].Rows[i].Validity;
			txtActualValidity.value = ds.Tables[0].Rows[i].Validity;
			
			txtExpInquiry.value 	= ds.Tables[0].Rows[i].InquiryPoints;
			txtActualInquiry.value 	= ds.Tables[0].Rows[i].InquiryPoints;
			
			txtExpAmount.value 		= ds.Tables[0].Rows[i].Amount;
			txtActualAmount.value 	= ds.Tables[0].Rows[i].Amount;
			txtIstopup.value 		= ds.Tables[0].Rows[i].IsTopup;
		}
	}
}

function bindDropDownList(response, cmbToFill, viewStateId, selectString) {
    if (response.Table != null) {
        if (!selectString || selectString == '') selectString = "--Select--";
        $(cmbToFill).empty().append("<option value=\"0\" title='" + selectString + "'>" + selectString + "</option>").removeAttr("disabled");
        var hdnValues = "";
        for (var i = 0; i < response.Table.length; i++) {
            $(cmbToFill).append("<option value=" + response.Table[i].Value + " title='" + response.Table[i].Text + "'>" + response.Table[i].Text + "</option>");
            if (hdnValues == "")
                hdnValues += response.Table[i].Text + "|" + response.Table[i].Value;
            else
                hdnValues += "|" + response.Table[i].Text + "|" + response.Table[i].Value;
        }
        if (viewStateId) $("#" + viewStateId).val(hdnValues);
    }
}
