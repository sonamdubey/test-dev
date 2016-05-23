<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Bikewale.test" Trace="false" Debug="true" %>
<%@ Register Src="/controls/ModelPriceInNearestCities.ascx" TagPrefix="BW" TagName="ModelPriceInNearestCities" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <BW:ModelPriceInNearestCities ID="ctrlTopCityPrices" runat="server" />
    </div>
    </form>
</body>
</html>
