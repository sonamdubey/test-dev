/*****************************************************************
this function takes as input the response which contains as value
a dataset, with the value to be displayed named as Text and id as 
Value.
it also takes as input the reference of the combo to be filled.
Next it takes as input the id of the hidden field to which the 
name and values are to be added separated with delimiter with 
two pipe, ||,.
Next the array of combo which are dependent on this combo, which 
are to be disabled if the current combo does not have any values.
*****************************************************************/
function FillCombo_Callback(response, cmbToFill, hdnId, dependentCmbs, selectString) {
    var _delimeter = "|";
    if (response.error != null) {
        alert("ERROR : " + response.error);
        return;
    }
    var objHdn = document.forms[0][hdnId];

    //now fill the values to the drop down
    if (cmbToFill) {
        //refresh the combo
        clearCombo(cmbToFill, selectString, cmbToFill.id);

        //also refresh the dependent combos
        if (dependentCmbs) {
            for (var i = 0; i < dependentCmbs.length; i++) {
                var depCmb = document.forms[0][dependentCmbs[i]];
                if (depCmb) {
                    clearCombo(depCmb, selectString, dependentCmbs[i]);
                }
            }
        }

        var j = 1;
        var ds = response.value;
        if (ds != null && typeof (ds) == "object" && ds.Tables != null) {
            var content = "";
            for (i = 0; i < ds.Tables[0].Rows.length; i++) {
                cmbToFill.options[j] = new Option(ds.Tables[0].Rows[i].Text, ds.Tables[0].Rows[i].Value);
                if (content == "") {
                    content = ds.Tables[0].Rows[i].Text + _delimeter + ds.Tables[0].Rows[i].Value;
                } else {
                    content += _delimeter + ds.Tables[0].Rows[i].Text + _delimeter + ds.Tables[0].Rows[i].Value;
                }
                j++;
            }

            //add the content to the hidden value
            if (objHdn) {
                objHdn.value = content;
            }

            if (j > 1) {
                cmbToFill.disabled = false;
            } else {
                cmbToFill.disabled = true;
                if (dependentCmbs) {
                    for (var i = 0; i < dependentCmbs.length; i++) {
                        var depCmb = document.forms[0][dependentCmbs[i]];
                        if (depCmb) {
                            clearCombo(depCmb, selectString, dependentCmbs[i]);
                            depCmb.disabled = true;
                        }
                    }
                }
            }
        }
    }
}

function clearCombo(cmb, selectString, dependentCmbs) {
    cmb.options.length = null;
    if (selectString == '' || !selectString) {
        if (dependentCmbs == 'drpMake')
            selectString = "--Select Make--";
        else if (dependentCmbs == 'drpModel')
            selectString = "--Select Model--";
        else if (dependentCmbs == 'drpVersion')
            selectString = "--Select Version--";
        else if (dependentCmbs == 'drpState')
            selectString = "--Select State--";
        else if (dependentCmbs == 'drpCity')
            selectString = "--Select City--";
        else
            selectString = "--Select--";
    }
    cmb.options[0] = new Option(selectString, 0);
}