<%@ Control Language="C#"%>
<h3>Calculate Loan EMI</h3>
<p>Bike loan EMI calculation was never this easy</p>
<div class="left-float margin-top10 margin-right10 padding-bottom20">
    <input type="text" class="form-control text-light-grey" id="txtLoanAmount" maxlength="10" value="Enter loan amount" tabindex="1"/>
</div>
<div class="margin-top10"><a id="btnLoanAmtGo" class="action-btn" tabindex="2" onclick="javascript:calculateEMI();">Go</a></div>
<div class="clear"></div>
<script type="text/javascript">
    var re = /^[0-9]*$/;
    var currentPage = parseInt($(this).attr('id'), 10);
    ++currentPage;

    $( "#txtLoanAmount" ).focusin( function () {          
        if ( $.trim($( this ).val()) == "Enter loan amount" )              
        $( this ).val( "" );
    } );

    $( "#txtLoanAmount" ).focusout( function () {        
        if(!$.isNumeric( $.trim($( this ).val() ) ))
            $( this ).val( "Enter loan amount" );
    } );


    function calculateEMI() {
        var loanAmt = $("#txtLoanAmount").val();
        if (loanAmt == "" || loanAmt == "Enter loan amount") {
            alert("Please enter loan amount.");
            return false;
        } else if (loanAmt != "" && re.test(loanAmt) == false) {
            alert("Please provide numeric data only for loan amount.");
            return false;
        } else if (parseInt(loanAmt, 10) < 5000) {
            alert("Please enter loan amount atleast 5000 or greater.");
            return false;
        }else {
            var lamt = $("#txtLoanAmount").val();
            window.location = "/finance/emicalculator.aspx?la=" + lamt;
        }
    }    
</script>