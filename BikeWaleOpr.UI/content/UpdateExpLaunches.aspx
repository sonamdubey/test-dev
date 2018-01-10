<%@ Page Inherits="BikeWaleOpr.Content.UpdateExpLaunches" AutoEventWireUp="false" Language="C#" trace="false" Debug="false" %>
<%@ Register TagPrefix="Vspl" TagName="Calendar" Src="/controls/DateControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
        <%--<link type="text/css" rel="stylesheet" href="/DCRM/css/style.css" />--%>
        <script type="text/javascript" src="/src/AjaxFunctions.js"></script>
        <script type="text/javascript" src="/src/jquery-1.6.min.js"></script>
        <title>Update Expected Launches</title>

        <style type="text/css">
	       .dvMargin{clear:both;float:left;margin:5px;color:#000000}
	       .dvRed{color: #FF3300; font-weight:bold;}
	        
	        #calFrom_cmbDay{width:40px; }
			#calFrom_cmbMonth{width:55px;}
			#calFrom_txtYear{height:25px; position:relative;top:2px;}
			#ddlHour{width:55px;}
			#ddlMinutes{width:55px;}
        </style>
    </head>
    <body style="margin:5px;">	
        <form runat="server">                  
	        <div class="dvMargin"> <span id="spnMessage" class="dvRed" runat="server"></span><span id="spnRes" class="dvRed" runat="server"></span></div>
            <div class="dvMargin"><strong>Bike Name: </strong>&nbsp;&nbsp; <span id="spnBikeName" runat="server"></span></div>
            <div class="dvMargin">
                <strong>Estimated Price(Min): </strong>&nbsp;&nbsp;<asp:textbox runat="server" id="txtEstMinPri" onkeydown="return onlyNumbers(event);"></asp:textbox>
            </div>
            <div class="dvMargin">
                <strong>Estimated Price(Max): </strong>&nbsp;&nbsp;<asp:textbox runat="server" id="txtEstMaxPri" onkeydown="return onlyNumbers(event);"></asp:textbox>
            </div>
            <div class="dvMargin">
                <strong>Expected Launch: </strong>&nbsp;&nbsp;<asp:textbox runat="server" id="txtExpLaunch"></asp:textbox>
            </div>
            <div class="dvMargin">
                <strong>Launch Date:</strong> &nbsp;&nbsp;
                <Vspl:Calendar DateId="calFrom" id="calFrom" runat="server" FutureTolerance="5" />
                    <select id="ddlHour" runat="server" >
                        <option value="0">00</option>
                        <option value="1">01</option>
                        <option value="2">02</option>
                        <option value="3">03</option>
                        <option value="4">04</option>
                        <option value="5">05</option>
                        <option value="6">06</option>
                        <option value="7">07</option>
                        <option value="8">08</option>
                        <option value="9">09</option>
                        <option value="10">10</option>
                        <option value="11">11</option>
                        <option value="12">12</option>
                        <option value="13">13</option>
                        <option value="14">14</option>
                        <option value="15">15</option>
                        <option value="16">16</option>
                        <option value="17">17</option>
                        <option value="18">18</option>
                        <option value="19">19</option>
                        <option value="20">20</option>
                        <option value="21">21</option>
                        <option value="22">22</option>
                        <option value="23">23</option>
                    </select>
                    <select id="ddlMinutes" runat="server">                    
                        <option value="00">00</option>
                        <option value="01">01</option>
                        <option value="02">02</option>
                        <option value="03">03</option>
                        <option value="04">04</option>
                        <option value="05">05</option>
                        <option value="06">06</option>
                        <option value="07">07</option>
                        <option value="08">08</option>
                        <option value="09">09</option>
                        <option value="10">10</option>
                        <option value="11">11</option>
                        <option value="12">12</option>
                        <option value="13">13</option>
                        <option value="14">14</option>
                        <option value="15">15</option>
                        <option value="16">16</option>
                        <option value="17">17</option>
                        <option value="18">18</option>
                        <option value="19">19</option>
                        <option value="20">20</option>
                        <option value="21">21</option>
                        <option value="22">22</option>
                        <option value="23">23</option>
                        <option value="24">24</option>
                        <option value="25">25</option>
                        <option value="26">26</option>
                        <option value="27">27</option>
                        <option value="28">28</option>
                        <option value="29">29</option>
                        <option value="30">30</option>
                        <option value="31">31</option>
                        <option value="32">32</option>
                        <option value="33">33</option>
                        <option value="34">34</option>
                        <option value="35">35</option>
                        <option value="36">36</option>
                        <option value="37">37</option>
                        <option value="38">38</option>
                        <option value="39">39</option>
                        <option value="40">40</option>
                        <option value="41">41</option>
                        <option value="42">42</option>
                        <option value="43">43</option>
                        <option value="44">44</option>
                        <option value="45">45</option>
                        <option value="46">46</option>
                        <option value="47">47</option>
                        <option value="48">48</option>
                        <option value="49">49</option>
                        <option value="50">50</option>
                        <option value="51">51</option>
                        <option value="52">52</option>
                        <option value="53">53</option>
                        <option value="54">54</option>
                        <option value="55">55</option>
                        <option value="56">56</option>
                        <option value="57">57</option>
                        <option value="58">58</option>
                        <option value="59">59</option>
                    </select>
            </div>
            <div class="dvMargin">
                <strong>Upload Photos: </strong> &nbsp;&nbsp;
                <div>
                    <div style="display:inline;">
                        <div style="width:237px;display:inline-block;border:1px solid #DBDBDC;padding:5px;">
                            <div id ="divMainImageRepTable" pending="<%= isReplicated =="0"? "true" : "false" %>">
                                    <img id="imgLargePicPath" image-id="<%=Id %>" src="<%= isReplicated =="0"? "https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/loader.gif" :  BikeWaleOpr.ImagingOperations.GetPathToShowImages(hostUrl,"227X128",originalImgPath)%>"/>
                            </div>
                        </div>
                        &nbsp;&nbsp;&nbsp;&nbsp;<input type="file" id="filLarge" runat="server" /><div>Original Image
                    </div>
                </div>
            </div>
            <div class="dvMargin">
                <input id="btnUpdate" type="button" value="Update" class="submit" runat="server" />
            </div>
       </form>
    </body>
    <script language="javascript" type="text/javascript">
        var refreshTime = 2000;

        $(document).ready(function () {
            setInterval(UpdatePendingMainImage, refreshTime)
        });

        function UpdatePendingMainImage() {
            var id = $("#divMainImageRepTable").find('img').attr('image-id');
            var pending = $("#divMainImageRepTable").attr('pending');
            if (pending == 'true') {
                CheckMainImageStatus(id);
            }
        }

        function CheckMainImageStatus(mainImageId) {
            $.ajax({
                type: "POST", url: "/AjaxPro/BikeWaleOpr.Common.Ajax.ImageReplication,BikewaleOpr.ashx",
                data: '{"imageId":"' + mainImageId + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckImageStatus_ExpLaunch"); },
                success: function (response) {
                    var ret_response = eval('(' + response + ')');
                    var obj_response = eval('(' + ret_response.value + ')');

                    if (obj_response.Table.length > 0) {
                        for (var i = 0; i < obj_response.Table.length; i++) {
                            var imgUrlLarge = obj_response.Table[i].HostUrl + "227X128" + obj_response.Table[i].OriginalImagePath;

                            $("#divMainImageRepTable").attr("pending", "false");
                            $("#imgLargePicPath").attr('src', imgUrlLarge);
                            $("#imgSmallPicPath").attr('src', imgUrlSmall);
                        }
                    }
                }
            })
        }

        function onlyNumbers(event) {
            var charCode = event.which || event.keyCode;

            if (charCode == 8 || charCode == 46 || charCode == 39 || charCode == 37 || charCode == 190 || charCode == 110)
                return true;
            else {
                if ((charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))
                    return false;
            }

            return true;
        }

        document.getElementById("#btnUpdate").onClick = validate();

        function validate() {
            var isError = false;
            if (document.getElementById("txtEstMinPri").value == "") {
                $('#spnMessage').text('Kindly Enter Estimated Minimum Price.');
                isError = true;
            }
            else if (document.getElementById("txtEstMaxPri").value == "") {
                $('#spnMessage').text('Kindly Enter Estimated Maximum Price.');
                isError = true;
            }
            else if (document.getElementById("txtExpLaunch").value == "") {
                $('#spnMessage').text('Kindly Enter Expected Launch.');
                isError = true;
            }

            if (isError)
                return false;
            else {
                $("#spnMessage").text('');
                return true;
            }
        }

       
    </script>
</html>