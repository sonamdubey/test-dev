<%@ Page Language="C#" AutoEventWireup="false" EnableEventValidation="false" Inherits="BikewaleOpr.NewBikeBooking.ManageBikeAvailabilityByColorAndCost" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="/src/AjaxFunctions.js" type="text/javascript"></script>
    <script src="/src/graybox.js"></script>
    <script language="javascript" src="/src/calender.js"></script>
    <link rel="stylesheet" href="/css/common.css?V1.2" type="text/css" />
    <title> Manage availability By Color </title>
    <style type="text/css">
        .errMessage {
            color: #FF4A4A;
        }

        .delete, .update {
            cursor: pointer;
        }

        .expired {
            background-color: #FF4A4A;
        }

        .yellow {
            background-color: #ffff48;
        }

    .doNotDisplay {
        display: none;
    }

    td, tr, table {
        border-color: white;
    }

    .ismodified {
        background-color: yellow;
    }

    .savedModelColors li {
        margin-top: 10px;
        min-width: 120px;
        float: left;
        text-align: center;
    }

    .addColorToVersions li {
        padding: 10px;
        float: left;
        text-align: center;
        border-bottom:1px solid #ccc;
    }

    .inline-block {
        display: inline-block;
        vertical-align: middle;
    }

    .leftfloat {
        float: left;
    }

    .rightfloat {
        float: right;
    }

    .clear {
        clear: both;
    }

    .updateModelColor {
        display: none;
        width: 300px;
        min-height: 200px;
        background: #fff;
        border: 2px solid #222;
        margin: 0 auto;
        position: fixed;
        top: 15%;
        left: 5%;
        right: 5%;
        padding: 0 10px;
    }

        .updateModelColor .closeBtn {
            position: absolute;
            right: 10px;
            top: 10px;
        }

        .updateModelColor table tr td input[type='text'] {
            width: 80px;
            margin-right: 15px;
        }


    .versionNameText {
        width: 90px;
        text-align: center;
    }

    .versionsBoxList {
        width: 950px;
        border-left: 1px solid #aaa;
    }

    #one {
        width: 50px;
        height: 50px;
        border: 1px solid #ccc;
        margin: 0 auto 10px;
    }

    #minVColor{
        width: 25px;
        height: 25px;
        border: 1px solid #ccc;
        margin: 0 auto 10px;
    }

    .noOfDays{
        margin:5px;
        width:20px;
        
    }

    #btnUpdateVersionColorAvailability{
        padding:10px;
        margin:10px;
    }

</style>
</head>
<body>
    <h1>Manage Bike Availability By Colors</h1>
    <hr />
    <form id="MangeBikeAvailabilityByColor" runat="server">
         <span id="spnError" class="error" runat="server"></span>
        <div style="padding:20px">
            <div style="margin-top: 15px;">
                <div class="addColorToVersions">                    
                    <ul class="inline-block ">
                        <%--<li class="colorTab">
                            <table border="0" id="minVColor" cellspacing="0">
                                Colors
                            </table>
                            <p> Availability (Days)</p>
                        </li> --%>
                        <asp:Repeater ID="rptColor" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <li class="colorTab <%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive")))?string.Empty:"hide" %>" >
                                    <table border="0" id="minVColor" cellspacing="0">
                                        <asp:Repeater ID="rptVColor" runat="server" EnableViewState="false">
                                            <ItemTemplate>
                                                <tr style='background: #<%#DataBinder.Eval(Container.DataItem,"HexCode")%>'>
                                                    <td></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <p><%# DataBinder.Eval(Container.DataItem,"ModelColorName") %></p> 
                                   
                                    <asp:HiddenField ID="hdnModelColorID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ModelColorID") %>' />
                                    <asp:TextBox ID="NoOfDaysByColor" runat="server" CssClass="noOfDays" Value='<%# DataBinder.Eval(Container.DataItem,"NoOfDays") %>' ></asp:TextBox> 
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <br />
                    <asp:Button ID="btnUpdateVersionColorAvailability" Text="Update Version Colors" runat="server" /> 
                    <input type="hidden" id="hdnColorDayObject" runat="server" value="" />
                </div>  
            </div>
           
        </div>

         

        <script src="/src/jquery-1.6.min.js" type="text/javascript"></script>
        <script>
              
            $(function () {

                //storeIntial Data
                var colorDays = [];
                var i = 0;
                $("li.colorTab").each(function () {
                    colorDays[$(this).find("input[type='hidden']").val()] = $(this).find("input[type='text']").val();                     
                });

                $("#btnUpdateVersionColorAvailability").live("click", function () {
                    str = "";
                    $("li.colorTab").each(function () {
                        colorCode  = $(this).find("input[type='hidden']").val();
                        noOfdays =  $(this).find("input[type='text']").val();

                        if(colorDays[colorCode]!=noOfdays)
                        {
                            str += colorCode + "_" + noOfdays + ",";
                        }
                        
                    });

                    if (str.length > 0)
                    {
                        str.substring(0, str.length - 1);
                        $("#hdnColorDayObject").val(str);

                    }
                   
               });
            });
            
                   

        </script>

    </form>
</body>
</html>
