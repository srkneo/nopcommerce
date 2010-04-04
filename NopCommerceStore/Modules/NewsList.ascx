<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.NewsListControl"
    CodeBehind="NewsList.ascx.cs" %>
<div class="newslist">
    <div class="title">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left; vertical-align: middle;">
                    <%=GetLocaleResourceString("News.News")%>
                </td>
                <td style="text-align: right; vertical-align: middle;">
                    <a href="<%= GetNewsRSSUrl()%>">
                        <asp:Image ID="imgRSS" runat="server" ImageUrl="~/images/icon_rss.gif" AlternateText="RSS" /></a>
                </td>
            </tr>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="newsitems">
        <asp:Repeater ID="rptrNews" runat="server">
            <ItemTemplate>
                <div class="item">
                    <a class="newstitle" href="<%#SEOHelper.GetNewsURL(Convert.ToInt32(Eval("NewsID")))%>">
                        <%#Server.HtmlEncode(Eval("Title").ToString())%></a> <span class="newsdate">-
                            <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString("d")%>
                        </span>
                    <div class="newsdetails">
                        <%#Eval("Short")%>
                    </div>
                    <a href="<%#SEOHelper.GetNewsURL(Convert.ToInt32(Eval("NewsID")))%>" class="readmore">
                        <%=GetLocaleResourceString("News.MoreInfo")%></a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
