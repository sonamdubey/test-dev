/*******************************************
 *	
 *	Chains for filling models and versions
 *
 *******************************************/
function fillChain( cmbToFill, parentCombo, arValues, cmbV  ) 
{
	var k;
	
	var cmb = document.getElementById(cmbToFill);
	var _parent = parentCombo.value;
	if ( cmb )
	{
		clearCombo ( cmb );
		k=1; 
		var j = 1;
		for( i = 0; i < arValues.length; i++ ) 
		{
			if( _parent == arValues[i][2]) 
			{
				cmb.options[j] = new Option( arValues[i][1], arValues[i][0] );
				j++ ;
			}
		}
		var cmbVersion = document.getElementById(cmbV);
		if ( j > 1 ) cmb.disabled = false;
		else 
		{	
			cmb.disabled = true;
			if ( cmbVersion ) 
			{
				clearCombo ( cmbVersion );
				cmbVersion.disabled = true;
			}
		}
	}
}
/*******************************************
 *	
 *	Chains for filling models and versions with two text box
 *
 *******************************************/
function fillChain( cmbToFill, parentCombo, arValues, cmbV, txt1, txt2  ) 
{
	var k;
	var cmb = document.getElementById(cmbToFill);
	var _parent = parentCombo.value;
	if ( cmb )
	{
		clearCombo ( cmb );
		k=1; 
		var j = 1;
		for( i = 0; i < arValues.length; i++ ) 
		{
			if( _parent == arValues[i][2]) 
			{
				cmb.options[j] = new Option( arValues[i][1], arValues[i][0] );
				j++ ;
			}
		}
		var cmbVersion = document.getElementById(cmbV);
		var txtCtrl1 = document.getElementById(txt1);
		var txtCtrl2 = document.getElementById(txt2);
		
		if ( j > 1 ) cmb.disabled = false;
		else 
		{	
			cmb.disabled = true;
			if ( cmbVersion ) 
			{
				clearCombo ( cmbVersion );
				cmbVersion.disabled = true;
			}
			if ( txtCtrl1 ) 
			{
				clearText ( txtCtrl1 );
			}
			if ( txtCtrl2 ) 
			{
				clearText ( txtCtrl2 );
			}
		}
	}
}
/*******************************************
 *	
 *	fill two text box wih two different value 
 *	according to the values of combo .
 *
 *******************************************/
function fillChainText( txt1ToFill, txt2ToFill ,parentCombo, arValues ) 
{
	var k;
	var txt1 = document.getElementById(txt1ToFill);
	var txt2 = document.getElementById(txt2ToFill);
	var _parent = parentCombo.value;
	if ( txt1 && txt2 )
	{
		clearText ( txt1 );
		clearText ( txt2 );
		k=1; 
		var j = 1;
		for( i = 0; i < arValues.length; i++ ) 
		{
			if( _parent == arValues[i][0]) 
			{
				txt1.value = arValues[i][1];
				txt2.value = arValues[i][2];
				j++ ;
			}
		}
		if ( j < 1 )
		{
			clearText ( txt1 );
			clearText ( txt2 );
		}

		
	}
}
/*******************************************
 *	
 *	Chains for filling section and subjects of same classes
 *
 *******************************************/
function fillChainTwoSameParent( firstCmbToFill, secondCmbToFill,parentCombo, arValue1, arValue2,cmbV ) 
{
	var k,l;
	var cmbFirst = document.forms[0][ firstCmbToFill ];
	var cmbSecond = document.forms[0][ secondCmbToFill ];
	var _parent = parentCombo.value;
	if ( cmbFirst )
	{
		clearCombo ( cmbFirst );
		k=1; 
		var j = 1;
		for( i = 0; i < arValue1.length; i++ ) 
		{
			if( _parent == arValue1[i][2]) 
			{
				cmbFirst.options[j] = new Option( arValue1[i][1], arValue1[i][0] );
				j++ ;
			}
		}
	}
	if ( j > 1 ) cmbFirst.disabled = false;
	else cmbFirst.disabled = true;
	if ( cmbSecond )
	{
		clearCombo ( cmbSecond );
		l=1; 
		var m = 1;
		for( n = 0; n < arValue2.length; n++ ) 
		{
			if( _parent == arValue2[n][2]) 
			{
				cmbSecond.options[m] = new Option( arValue2[n][1], arValue2[n][0] );
				m++ ;
			}
		}
	}
	if ( m > 1 ) cmbSecond.disabled = false;
	else cmbSecond.disabled = true;
}

/*******************************************
 *	
 *	Chains for List filling section and subjects of same classes(SameParent)
 *
 *******************************************/
function fillChainListTwo( firstCmbToFill, secondCmbToFill,parentCombo, arValue1, arValue2,cmbV ) 
{
	var k,l;
	var cmbFirst = document.forms[0][ firstCmbToFill ];
	var cmbSecond = document.forms[0][ secondCmbToFill ];
	var _parent = parentCombo.value;
	if ( cmbFirst )
	{
		k=1; 
		var j = 0;
		for( i = 0; i < arValue1.length; i++ ) 
		{
			if( _parent == arValue1[i][2]) 
			{
				cmbFirst.options[j] = new Option( arValue1[i][1], arValue1[i][0] );
				j++ ;
			}
		}
	}
	if ( cmbSecond )
	{
		l=1; 
		var m = 0;
		for( n = 0; n < arValue2.length; n++ ) 
		{
			if( _parent == arValue2[n][2]) 
			{
				cmbSecond.options[m] = new Option( arValue2[n][1], arValue2[n][0] );
				m++ ;
			}
		}
	}
}


/*******************************************
 *	
 *	Chains for filling two related combos
 *
 *******************************************/
function fillChainTwo( cmbToFill, parentCombo, arValues, selId ) 
{
	var k;
	var cmb = document.getElementById(cmbToFill);
	var _parent = parentCombo.value;
	clearCombo ( cmb );
	
	k=1; 
	var j = 1;
	for( i = 0; i < arValues.length; i++ ) 
	{
		//alert(i);
		if( _parent == arValues[i][2]) 
		{
			cmb.options[j] = new Option( arValues[i][1], arValues[i][0] );
			if((selId != "-1") && (arValues[i][0] == selId))
			{
				//alert("matched");
				cmb.options[j].selected = true;
			}
			j++ ;
		}
	}
	if(j > 1)
		cmb.disabled = false;
	else
		cmb.disabled = true;
}

/*******************************************
 *	
 *	Chains for filling two related combos that does match the values
 *
 *******************************************/
 
function fillReverseChain( cmbToFill, parentCombo, arValues, selId ) 
{
	var k;
	var cmb = document.getElementById(cmbToFill);
	var _parent = parentCombo.value;
	clearCombo ( cmb );
	
	k=1; 
	var j = 1;
	for( i = 0; i < arValues.length; i++ ) 
	{
		//alert(i);
		if( _parent != arValues[i][2]) 
		{
			cmb.options[j] = new Option( arValues[i][1], arValues[i][0] );
			if((selId != "-1") && (arValues[i][0] == selId))
			{
				//alert("matched");
				cmb.options[j].selected = true;
			}
			j++ ;
		}
	}
	if(j > 1)
		cmb.disabled = false;
	else
		cmb.disabled = true;
}
/*******************************************
 *	
 *	Chains for filling models and versions in list format with versionId:color 
 *
 *******************************************/
function fillList( lstToFill, versionID , arValues , checkedColors ) 
{
	var k;
	k=1; 
	var j = 1;
	var checked ;//alert(checkedColors);
	for( i = 0; i < arValues.length; i++ ) 
	{
		
		if( versionID == arValues[i][2]) 
		{
			for( m = 0 ; m < checkedColors.length ; m++ )
			{
				if ( arValues[i][0] == checkedColors[m] )
				{
					checked = 'checked';
					break;
				}
				else
					checked = '';
			}
			
			
			lstToFill.innerHTML += "<input type='checkbox' name='chkColor' value='"+ arValues[i][2] + ":" + arValues[i][1] + ":" + arValues[i][0] +"' " + checked + " /> " + arValues[i][1] + "<br>";
			j++ ;
		}
	}
}
/*******************************************
 *	
 *	Chains for filling models and versions in list format with versioncolor Id
 *
 *******************************************/
function fillListId( lstToFill, versionID , arValues , checkedColors  ) 
{
	var k;
	k=1; 
	var j = 1;
	var checked ;
	for( i = 0; i < arValues.length; i++ ) 
	{
		if( versionID == arValues[i][2]) 
		{
			for( m = 0 ; m < checkedColors.length ; m++ )
			{
				if ( arValues[i][0] == checkedColors[m] )
				{
					checked = 'checked';
					break;
				}
				else
					checked = '';
			}
			
			lstToFill.innerHTML += "<input type='checkbox' name='chkColor' id='chkColor" + i + "' value='"+ arValues[i][0] +"' " + checked + " /> " + arValues[i][1] + "<br>";
			j++ ;
		}
	}
}

function clearCombo( cmb )
{
	cmb.options.length = null;
	cmb.options[0] = new Option("-- Select --",0);
}
function clearText( txt )
{
	txt.value= '';
}
function clearList( lst )
{
	lst.innerHTML = '';
}