﻿<%@ Page Language="C#" %>
<%
    title = "EMI Calculator | Calculate Bike Loan EMI - BikeWale";       
    keywords = "calculate emi, emi calculator, calculate loan, loan calculator, indian emi calculator, used Bike emi, new Bike emi, BMW, Chevrolet, Daewoo, Fiat, Ford, Hindustan Motors, Honda, Hyundai, Mahindra, Maruti, Mercedes-Benz, Mitsubishi, Opel, Premier, Skoda, Tata, Toyota";
    description = "EMI Calculator for new and used Bike loans. Calculate accurate Bike loan emi in advanced and arrears finance modes.";
    AdId = "1395991249804";
    AdPath = "/1017752/BikeWale_EMICalculator_";
%>
<!-- #include file="/includes/headTools.aspx" -->
<script type="text/javascript" src="/src/calculateemi.js?v=1.0"></script>
<script language="C#" runat="server">
    string loanAmount = "";
    string rate = "";
    string loanType = "";
    protected override void OnInit(EventArgs e)
    {
        base.Load += new EventHandler(Page_Load);
    }

    void Page_Load(object Sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["la"] != null)
            {
                loanAmount = Request.QueryString["la"];
                txtloanamount.Value = loanAmount;
            }
            rate = "12";
            if (Request.QueryString["rt"] != null)
            {
                rate = Request.QueryString["rt"];
                interestRate.Value = rate;
            }

            //if (Request.QueryString["loantype"] != null)
            //	loanType = Request.QueryString["loantype"];
            loanType = "0";
            if (loanType == "0")
                R1.Checked = true;
            else
                R2.Checked = true;
        }
    }
</script>
<style type="text/css">
    
</style>
<div class="container_12">
    <div class="grid_12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li><a href="/">Home</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li><a href="/finance/emicalculator.aspx">Tools</a></li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>EMI Calculator</strong></li>
        </ul><div class="clear"></div>
    </div>
    <div class="grid_8 margin-top10">
        <div id="ncd_bikes_testdrive">
            <h1>EMI Calculator - Calculate Your Bike Loan EMI</h1>
            <p class="margin-top10">Enter the amount of loan you want to get financed and interest rate. EMI Calculator calculates installment on reducing balance. EMI Calculator does not include any other processing fee or possible charges which may be applicable as per the rules of financing institutions.</p>
            <div class="grid_4 alpha margin-top15">
                <table border="0" cellpadding="0" cellspacing="0" class="tbl-default" width="100%">
                    <tr>
                        <td class="td-width">Loan Amount (&#8377;)</td>
                        <td>
                            <input style="text-align: right; width:100px;" type="text" maxlength="10" id="txtloanamount" size="10" value="100000" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Rate Of Interest (<strong>% </strong>)</td>
                        <td>
                            <input style="text-align: right; width:100px;" id="interestRate" type="text" maxlength="5" value="12.5" size="5" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:radiobutton id="R1" runat="server" text=" EMI in Advance" name="opt" groupname="EMIType"></asp:radiobutton>
                            &nbsp;<br />
                            <asp:radiobutton id="R2" runat="server" text=" EMI in Arrears" name="opt" groupname="EMIType"></asp:radiobutton>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>                         
                            <input id="btnCalcEmi" class="buttons" type="submit" title="Calculate EMI" name="Button" value="Calculate EMI" />                        
                        </td>
                    </tr>
                </table>
            </div>
            <div id="emiChart" class="grid_4 grey-bg omega margin-top15">
                <div class="content-block">
                    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="tbl-emi tbl-default">
                        <tr>
                            <th>Months </th>
                            <th>EMI</th>
                            <th class="doNotShow">&nbsp;</th>
                        </tr>
                        <tr>
                            <td><b>12</b></td>
                            <td><span id="12months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="12print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>24</b></td>
                            <td><span id="24months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="24print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>36</b></td>
                            <td><span id="36months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="36print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>48</b></td>
                            <td><span id="48months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="48print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>60</b></td>
                            <td><span id="60months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="60print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>72</b></td>
                            <td><span id="72months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="72print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td><b>84</b></td>
                            <td><span id="84months">&nbsp;</span></td>
                            <td class="doNotShow"><span id="84print">&nbsp;</span></td>
                        </tr>
                        <tr>
                            <td style="display: none;" colspan="2">
                                <textarea name="sched" id="txtSchedule" readonly="readonly" rows="22" cols="82" tabindex="7"></textarea>
                                <div id="divChartVal"></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="clear"></div>
            <p class="margin-top10">The calculation performed by EMI Calculator is based on the information you entered and is for illustrative purposes only.</p>
        </div>
    </div>
    <div class="grid_4">
        <div class="margin-top15">
        <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
        <!-- #include file="/ads/Ad300x250.aspx" -->   
        </div>
    </div>
</div>
</form>

<script type="text/javascript">
    if (document.getElementById("txtloanamount").value != '') {
        //alert(document.forms[0].Emi_txtloanamount.value);
        //alert(document.getElementById("<%=txtloanamount.ClientID.ToString() %>"));
        calculateAllEMI();
    }


    $("#btnCalcEmi").click(function () {
        var re = /^[0-9]*$/;
        var loanAmt = $("#txtloanamount").val();
        //if ($("#txtloanamount").val() == "" || $("#txtloanamount").val() == "0")
        //{
        //    alert("Please enter valid Loan Amount.");
        //    return false;
        //}
        if (loanAmt == "" || loanAmt == "Enter loan amount") {
            alert("Please enter loan amount.");
            return false;
        } else if (loanAmt != "" && re.test(loanAmt) == false) {
            alert("Please provide numeric data only for loan amount.");
            return false;
        } else if (parseInt(loanAmt, 10) < 5000) {
            alert("Please enter loan amount atleast 5000 or greater.");
            return false;
        } else {
            var lamt = $("#txtLoanAmount").val();
            window.location = "/finance/emicalculator.aspx?la=" + lamt;
        }
    });    
</script>
<!-- #include file="/includes/footerInner.aspx" -->



