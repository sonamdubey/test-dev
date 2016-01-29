<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsersTestimonials" %>
<div class="content-box-shadow text-center">
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
                                <p class="testimonial-user-stmt font16 margin-top15 margin-bottom15"><%# DataBinder.Eval(Container.DataItem,"Content") %></p>
                                <p class="font14 text-light-grey"><%# DataBinder.Eval(Container.DataItem,"UserName") %></p>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <p class="jcarousel-pagination padding-bottom20"></p>
    </div>
</div>
