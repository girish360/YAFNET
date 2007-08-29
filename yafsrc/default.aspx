<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="YAF._default" %>

<script runat="server">
    public void Page_Error( object sender, System.EventArgs e )
    {
        Exception x = Server.GetLastError();
        DB.eventlog_create( forum.PageUserID, this, x );
        General.LogToMail( x );
    }
</script>
<html>
<head id="HeadTag" runat="server">
    <meta name="Description" content="A bulletin board system written in ASP.NET">
    <meta name="Keywords" content="Yet Another Forum.net, Forum, ASP.NET, BB, Bulletin Board, opensource">
    <%-- If you don't want the forum to set the page title, you can remove runat and id --%>
    <title runat="server" id="ForumTitle">This title is overwritten</title>
</head>
<body>
    <img src="images/YAFLogo.jpg" runat="server" id="imgBanner" />
    <br />
    <form id="form1" runat="server" enctype="multipart/form-data">
        <YAF:Forum runat="server" ID="forum"></YAF:Forum>
    </form>
</body>
</html>
