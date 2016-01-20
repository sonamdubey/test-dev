<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsersTestimonials" %>
<asp:Repeater ID="rptTestimonial" runat="server">
    <ItemTemplate>
        <div class="swiper-slide rounded-corner2 content-box-shadow">
            <div class="testimonial-image-container rounded-corner50percent">
                <div class="testimonial-user-image rounded-corner50percent">
                    <img src="<%# String.Format("{0}{1}", DataBinder.Eval(Container.DataItem,"HostUrl"),DataBinder.Eval(Container.DataItem,"UserImgUrl")) %>" border="0" />
                </div>
            </div>
            <p class="testimonial-user-stmt font16 margin-top15 margin-bottom15"><%# DataBinder.Eval(Container.DataItem,"Content") %></p>
            <p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"UserName") %></p>
        </div>
    </ItemTemplate>
</asp:Repeater>
