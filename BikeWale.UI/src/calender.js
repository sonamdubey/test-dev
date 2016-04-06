// Validations for Calender Control.

function Calender( calenderId, curYear,curMonth, tol )
{
	// Will hold the calender id prefix.
    var calId 			= calenderId;
	
	// Current Year. In case user inputs invalid date, this will be restored.
    var currentYear = curYear;
    //var currentMonth = curMonth;

	// tolerance in future date
	var tolerance		= 0;
	if ( tol ) tolerance = tol;

	// maxYear. The maximum year allowed.
	var maxYear = currentYear + tolerance;
	
	this.init 			= initializeControl;
	this.getId			= getControlId;
	this.adjust			= adjustDays;
	this.populate		= populateDays;
	
	function initializeControl()
	{
		var cmbDay 		= getControlId( "cmbDay" );
		var cmbMonth 	= getControlId( "cmbMonth" );
		txtYear = getControlId("txtYear");
		
		currentYear = txtYear.value; // Current Date

		
		adjustDays(); // 
		
		cmbMonth.onchange 	= adjustDays;
		txtYear.onchange	= adjustDays;
	}
	
	function getControlId( ctrl )
	{
		return document.getElementById( calId + "_" + ctrl );	
	}
	
	function adjustDays()
	{
	    var month = getControlId("calMakeYear_cmbMonth");
	    var monthNo = getControlId("cmbMonth").value;
		var txtYear = getControlId("txtYear");
		var calMakeYear_cmbMonth = getControlId("calMakeYear_cmbMonth");

		var re = /^\d*$/
		
		if ( re.test( txtYear.value ) )
		{
			if ( txtYear.value > 1900 && txtYear.value <= maxYear )
			{
			    currentDate = txtYear.value;
			}
			else
			{
			    txtYear.value = currentYear;
			}
		}
		else
		{
		    txtYear.value = currentDate;
		}

	    //Validate Make year month
		if (calMakeYear_cmbMonth && calMakeYear_cmbMonth.value == curYear) {
		    if (monthNo > curMonth) {
		        $("#calMakeYear_cmbMonth").val(curMonth);
		    }
		}



        
		var isLeap = txtYear.value % 4 == 0 ? true : false;
		
		if ( monthNo == 2 )
		{
			if ( isLeap ) populateDays( 29 ); // in leap year, feb is of 29 days
			else  populateDays( 28 ); // its of 28 days otherwise.
		}
		else if ( monthNo == 4 || monthNo == 6 || monthNo == 9 || monthNo == 11  )
		{
			populateDays( 30 ); // April(4), June(6), September(9), November(11) are of 30 days.
		}
		else 
		{
			populateDays( 31 ); // All others are of 31 days.
		}
	}
	
	function populateDays( no )
	{
		cmbDay 	= getControlId( "cmbDay" );
		var diff = cmbDay.options.length - no;
	
		if ( diff > 0 ) // There are more days in dropdown than needed. Remove unwanted!
		{
			for ( var i=0; i < diff; i++ )
			{
				cmbDay.remove( cmbDay.options.length - 1 );	
			}
		}
		else if( diff < 0 ) // There are less days. Add Needed!
		{
			diff = -diff;
			for ( var i=diff; i > 0; i-- )
			{
				cmbDay.options[ cmbDay.options.length ] = new Option( cmbDay.options.length + 1, cmbDay.options.length + 1 );
			}
		}
		
	}
}