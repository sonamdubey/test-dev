<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsersTestimonials" %>
<div class="text-center">
    <div class="jcarousel-wrapper">
        <div class="jcarousel">
            <ul>
                <asp:Repeater ID="rptTestimonial" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="testimonial-card">
                                <div class="testimonial-image-container rounded-corner50">
                                    <div class="testimonial-user-image rounded-corner50">
                                        <img src="<%# String.Format("{0}{1}", DataBinder.Eval(Container.DataItem,"HostUrl"),DataBinder.Eval(Container.DataItem,"UserImgUrl")) %>" border="0" />
                                    </div>
                                </div>
                                <p class="font18 text-bold margin-top15 margin-bottom10"><%# DataBinder.Eval(Container.DataItem,"Title") %></p>
                                <p class="testimonial-user-stmt font16 margin-bottom15"><%# DataBinder.Eval(Container.DataItem,"Content") %></p>
                                <p class="font14"><%# DataBinder.Eval(Container.DataItem,"UserName") %></p>
                                <p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"City") %></p>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <p class="jcarousel-pagination padding-bottom30"></p>
    </div>
</div>
