<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.BlogControl"
    CodeBehind="Blog.ascx.cs" %>
<div class="blog">
    <div class="page-title">
        <table width="100%">
            <tr>
                <td style="text-align: left; vertical-align: middle;">
                    <h1><%=GetLocaleResourceString("Blog.Blog")%></h1>
                </td>
                <td style="text-align: right; vertical-align: middle;">
                    <a href="<%= GetBlogRSSUrl()%>">
                        <asp:Image ID="imgRSS" runat="server" ImageUrl="~/images/icon_rss.gif" AlternateText="RSS" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="blogposts">
        <asp:Repeater ID="rptrBlogPosts" runat="server" OnItemDataBound="rptrBlogPosts_ItemDataBound">
            <ItemTemplate>
                <div class="post">
                    <a class="blogtitle" href="<%#SEOHelper.GetBlogPostURL(Convert.ToInt32(Eval("BlogPostID")))%>">
                        <%#Server.HtmlEncode(Eval("BlogPostTitle").ToString())%></a><span class="blogdate">
                            -
                            <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString("d")%>
                        </span>
                    <div class="blogbody">
                        <%#Eval("BlogPostBody")%>
                    </div>
                    <a href="<%#SEOHelper.GetBlogPostURL(Convert.ToInt32(Eval("BlogPostID")))%>" class="blogdetails">
                        <asp:Literal ID="lComments" runat="server"></asp:Literal>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
