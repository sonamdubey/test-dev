<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.SimilarCompareBikes" %>

<% if(fetchedCount > 0 && objSimilarBikes!=null) { %>
    &nbsp;People who compared above bikes also compared: <br /><br />
      
          <% foreach(var bike in objSimilarBikes) { %>
            <div class="related-comparison-wrapper padding-left10 padding-right10 margin-bottom15">
               <a href="/<%= Bikewale.Utility.UrlFormatter.CreateCompareUrl(bike.MakeMasking1,bike.ModelMasking1,bike.MakeMasking2,bike.ModelMasking2,bike.VersionId1,bike.VersionId2, bike.ModelId1,bike.ModelId2,Bikewale.Entities.Compare.CompareSources.Desktop_CompareBike_Details_MoreBike_Widget) %>" title ="<%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Model1,bike.Model2) %>">
                   <%= Bikewale.Utility.UrlFormatter.CreateCompareTitle(bike.Make1,bike.Model1,bike.Make2,bike.Model2) %>
                 </a>
            </div>
         <% } %>

<% }  %>

