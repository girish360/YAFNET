<%@ Control language="c#" Inherits="yaf.pages.admin.version" CodeFile="version.ascx.cs" CodeFileBaseClass="yaf.AdminPage" %>
<%@ Register TagPrefix="yaf" Namespace="yaf.controls" %>

<yaf:PageLinks runat="server" id="PageLinks"/>

<yaf:adminmenu runat="server">

<table width=100% cellspacing=0 cellpadding=0 class=content>
<tr>
	<td class="post">
		<table width=100% cellspacing=0 cellpadding=0>
		<tr class=header2>
			<td>Version Check</td>
		</tr>
		<tr class="post">
			<td>
				<p>You are running Yet Another Forum.net version <%=yaf.Data.AppVersionName%>.</p>
				
				<p>The latest available version is <%=LastVersion%> released <%=LastVersionDate%>.</p>
				
				<p runat="server" id="Upgrade" visible="false">You can download the latest version from <a target="_top" href="http://sourceforge.net/project/showfiles.php?group_id=90539">here</a>.</p>
			</td>
		</tr>
		</table>
	</td>
</tr>
</table>

</yaf:adminmenu>