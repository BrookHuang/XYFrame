using System;
using System.Collections.Generic;
using System.Text;

namespace XyFrameConsole {
    class Program {
        #region
        static string _html = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html>
<head>
	<link rel=""stylesheet"" type=""text/css"" href=""http://192.168.1.230/css/default.css"" />
	<script type=""text/javascript""><!--
		if(document.layers) {document.write(""<style>td{padding:0px;}<\/style>"")}
	// --></script>
	<meta http-equiv=""Content-type"" content=""text/html; charset=utf-8"" />
	<meta http-equiv=""Pragma"" content=""no-cache"" />
	<meta http-equiv=""Cache-Control"" content=""no-cache"" />
	<meta http-equiv=""Pragma-directive"" content=""no-cache"" />
	<meta http-equiv=""Cache-Directive"" content=""no-cache"" />
	<meta http-equiv=""Expires"" content=""Fri, 04 Apr 2014 03:51:12 GMT"" />
	<meta name=""robots"" content=""noindex,follow"" />
	<link rel=""shortcut icon"" href=""/images/favicon.ico"" type=""image/x-icon"" />
	<link rel=""search"" type=""application/opensearchdescription+xml"" title=""MantisBT: Text Search"" href=""http://192.168.1.230/browser_search_plugin.php?type=text"" />	<link rel=""search"" type=""application/opensearchdescription+xml"" title=""MantisBT: Issue Id"" href=""http://192.168.1.230/browser_search_plugin.php?type=id"" />	<title>My View - MantisBT</title>
<script type=""text/javascript"" src=""/javascript/min/common.js""></script>
<script type=""text/javascript"">var loading_lang = ""Loading..."";</script><script type=""text/javascript"" src=""/javascript/min/ajax.js""></script>
	<meta http-equiv=""Refresh"" content=""1800;URL=http://192.168.1.230/my_view_page.php"" />
</head>
<body>
<div align=""left""><a href=""my_view_page.php""><img border=""0"" alt=""MantisBT"" src=""/images/mantis_logo.png"" /></a></div><table class=""hide""><tr><td class=""login-info-left"">Logged in as: <span class=""italic"">brook.huang</span> <span class=""small"">(developer)</span></td><td class=""login-info-middle""><span class=""italic"">2014-04-04 11:51 CST</span></td><td class=""login-info-right""><form method=""post"" name=""form_set_project"" action=""/set_project.php"">Project: <select name=""project_id"" class=""small"" onchange=""document.forms.form_set_project.submit();""><option value=""0"">All Projects</option>
<option value=""3"">NEW-Clickjia</option>
<option value=""4"" selected=""selected"" >NEW-Homesup</option>
</select> <input type=""submit"" class=""button-small"" value=""Switch"" /></form><a href=""http://192.168.1.230/issues_rss.php?username=brook.huang&amp;key=a3185fead962769a4292bb4ab2d25d52&amp;project_id=4""><img src=""/images/rss.png"" alt=""RSS"" style=""border-style: none; margin: 5px; vertical-align: middle;"" /></a></td></tr></table><table class=""width100"" cellspacing=""0""><tr><td class=""menu""><a href=""/main_page.php"">Main</a> | <a href=""/my_view_page.php"">My View</a> | <a href=""/view_all_bug_page.php"">View Issues</a> | <a href=""/bug_report_page.php"">Report Issue</a> | <a href=""/changelog_page.php"">Change Log</a> | <a href=""/roadmap_page.php"">Roadmap</a> | <a href=""/account_page.php"">My Account</a> | <a href=""/logout_page.php"">Logout</a></td><td class=""menu right nowrap""><form method=""post"" action=""/jump_to_bug.php""><input type=""text"" name=""bug_id"" size=""10"" class=""small"" value=""Issue #"" onfocus=""if (this.value == 'Issue #') this.value = ''"" onblur=""if (this.value == '') this.value = 'Issue #'"" />&#160;<input type=""submit"" class=""button-small"" value=""Jump"" />&#160;</form></td></tr></table><div align=""right""><small>Recently Visited: <a href=""/view.php?id=215"" title=""[assigned] some optimization in point system"">0000215</a></small></div>
<div align=""center"">
<table class=""hide"" border=""0"" cellspacing=""3"" cellpadding=""0"">

<tr><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;handler_id=3&amp;hide_status=80"">Assigned to Me (Unresolved)</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;handler_id=3&amp;hide_status=80"" target=""_blank"">^</a>&#160;]</span> (1 - 2 / 2)	</td>
</tr>


<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=215"" title=""[assigned] some optimization in point system"">0000215</a><br /><a href=""bug_update_page.php?bug_id=215""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		some optimization in point system		<br />
		[All Projects] Develop - 2014-04-03 18:00		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=108"" title=""[assigned] small Bug Fix"">0000108</a><br /><a href=""bug_update_page.php?bug_id=108""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		small Bug Fix		<br />
		[All Projects] Bug - 2013-12-09 10:40		</span>
	</td>
</tr>
</table>
</td><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;handler_id=[none]&amp;hide_status=90"">Unassigned</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;handler_id=[none]&amp;hide_status=90"" target=""_blank"">^</a>&#160;]</span> (0 - 0 / 0)	</td>
</tr>

</table>
</td></tr><tr><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;reporter_id=3&amp;hide_status=90"">Reported by Me</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;reporter_id=3&amp;hide_status=90"" target=""_blank"">^</a>&#160;]</span> (1 - 2 / 2)	</td>
</tr>


<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=179"" title=""[resolved] change stock display logic"" class=""resolved"">0000179</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		change stock display logic		<br />
		[All Projects] Develop - 2014-02-19 10:51		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=108"" title=""[assigned] small Bug Fix"">0000108</a><br /><a href=""bug_update_page.php?bug_id=108""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		small Bug Fix		<br />
		[All Projects] Bug - 2013-12-09 10:40		</span>
	</td>
</tr>
</table>
</td><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;show_status=80&amp;hide_status=80"">Resolved</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;show_status=80&amp;hide_status=80"" target=""_blank"">^</a>&#160;]</span> (1 - 10 / 55)	</td>
</tr>


<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=206"" title=""[resolved] bug customer pay in showroom can not send to ERP"" class=""resolved"">0000206</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		bug customer pay in showroom can not send to ERP		<br />
		[All Projects] Bug - 2014-03-14 17:05		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=208"" title=""[resolved] Disable 3-17 delivery time"" class=""resolved"">0000208</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Disable 3-17 delivery time		<br />
		[All Projects] Bug - 2014-03-14 17:05		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=199"" title=""[resolved] when only 1 product in shopping cart,end of the payment process will &quot;不同仓库发货&quot;"" class=""resolved"">0000199</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		when only 1 product in shopping cart,end of the payment process will &quot;不同仓库发货&quot;		<br />
		[All Projects] Bug - 2014-03-11 09:11		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=184"" title=""[resolved] shopping cart number is wrong"" class=""resolved"">0000184</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		shopping cart number is wrong		<br />
		[All Projects] Bug - 2014-03-11 09:05		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=191"" title=""[resolved] Sync product category fail."" class=""resolved"">0000191</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Sync product category fail.		<br />
		[All Projects] Bug - 2014-03-11 09:03		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=188"" title=""[resolved] Add return notice"" class=""resolved"">0000188</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Add return notice		<br />
		[All Projects] General - 2014-02-28 09:28		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=193"" title=""[resolved] http://www.homes-up.com/silk-sleeveless-embroidery-nightskirt-l-elegance-homes-up-10.html"" class=""resolved"">0000193</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		<a href=""http://www.homes-up.com/silk-sleeveless-embroidery-nightskirt-l-elegance-homes-up-10.html"">http://www.homes-up.com/silk-sleeveless-embroidery-nightskirt-l-elegance-homes-up-10.html</a> [<a href=""http://www.homes-up.com/silk-sleeveless-embroidery-nightskirt-l-elegance-homes-up-10.html"" target=""_blank"">^</a>]		<br />
		[All Projects] Bug - 2014-02-28 09:21		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=186"" title=""[resolved] when stick the words into search box and then click the the &quot;search&quot; button, it will refresh the page"" class=""resolved"">0000186</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		when stick the words into search box and then click the the &quot;search&quot; button, it will refresh the page		<br />
		[All Projects] Bug - 2014-02-19 17:33		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=145"" title=""[resolved] View the parcel logistics status"" class=""resolved"">0000145</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		View the parcel logistics status		<br />
		[All Projects] Develop - 2014-02-19 10:54		</span>
	</td>
</tr>

<tr bgcolor=""#d2f5b0"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=183"" title=""[resolved] Google Analytics code update"" class=""resolved"">0000183</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Google Analytics code update		<br />
		[All Projects] Develop - 2014-02-19 10:53		</span>
	</td>
</tr>
</table>
</td></tr><tr><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;hide_status=none"">Recently Modified</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;hide_status=none"" target=""_blank"">^</a>&#160;]</span> (1 - 10 / 109)	</td>
</tr>


<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=180"" title=""[assigned] Points System"">0000180</a><br /><a href=""bug_update_page.php?bug_id=180""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Points System		<br />
		[All Projects] Develop - <b>2014-04-04 11:09</b>		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=196"" title=""[assigned] Mobile store templates"">0000196</a><br /><a href=""bug_update_page.php?bug_id=196""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Mobile store templates		<br />
		[All Projects] Develop - <b>2014-04-04 09:33</b>		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=215"" title=""[assigned] some optimization in point system"">0000215</a><br /><a href=""bug_update_page.php?bug_id=215""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		some optimization in point system		<br />
		[All Projects] Develop - 2014-04-03 18:00		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=214"" title=""[assigned] alipay mobile wap payment"">0000214</a><br /><a href=""bug_update_page.php?bug_id=214""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		alipay mobile wap payment		<br />
		[All Projects] Develop - 2014-04-02 18:11		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=212"" title=""[assigned] 4 pages should add relevant link to other 3"">0000212</a><br /><a href=""bug_update_page.php?bug_id=212""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		4 pages should add relevant link to other 3		<br />
		[All Projects] Develop - 2014-04-01 10:16		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=213"" title=""[assigned] Order to ERP optimize"">0000213</a><br /><a href=""bug_update_page.php?bug_id=213""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Order to ERP optimize		<br />
		[All Projects] Bug - 2014-03-31 13:57		</span>
	</td>
</tr>

<tr bgcolor=""#c2dfff"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=125"" title=""[assigned] Some optimizing(e.g. translate, unused code...)"">0000125</a><br /><a href=""bug_update_page.php?bug_id=125""><img border=""0"" src=""http://192.168.1.230/images/update.png"" alt=""Edit"" /></a><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Some optimizing(e.g. translate, unused code...)		<br />
		[All Projects] Bug - 2014-03-31 13:13		</span>
	</td>
</tr>

<tr bgcolor=""#c9ccc4"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=209"" title=""[closed] color page speed optimization"" class=""resolved"">0000209</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		color page speed optimization		<br />
		[All Projects] Bug - 2014-03-25 17:41		</span>
	</td>
</tr>

<tr bgcolor=""#c9ccc4"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=211"" title=""[closed] out of stock product in the wishlist can be added to shopcart"" class=""resolved"">0000211</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		out of stock product in the wishlist can be added to shopcart		<br />
		[All Projects] Bug - 2014-03-25 17:41		</span>
	</td>
</tr>

<tr bgcolor=""#c9ccc4"">
		<td class=""center"" valign=""top"" width =""0"" nowrap=""nowrap"">
		<span class=""small"">
		<a href=""/view.php?id=143"" title=""[closed] Clean up the unused files"" class=""resolved"">0000143</a><br /><img src=""http://192.168.1.230/images/priority_normal.gif"" alt="""" title=""normal"" />		</span>
	</td>

		<td class=""left"" valign=""top"" width=""100%"">
		<span class=""small"">
		Clean up the unused files		<br />
		[All Projects] Bug - 2014-03-25 17:38		</span>
	</td>
</tr>
</table>
</td><td valign=""top"" width=""50%"">
<table class=""width100"" cellspacing=""1"">
<tr>
	<td class=""form-title"" colspan=""2"">
<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;user_monitor=3&amp;hide_status=90"">Monitored by Me</a>&#160;<span class=""bracket-link"">[&#160;<a class=""subtle"" href=""view_all_set.php?type=1&amp;temporary=y&amp;user_monitor=3&amp;hide_status=90"" target=""_blank"">^</a>&#160;]</span> (0 - 0 / 0)	</td>
</tr>

</table>
</td></tr>
</table>
<br /><table class=""width100"" cellspacing=""1""><tr><td class=""small-caption"" width=""14%"" bgcolor=""#fcbdbd"">new</td><td class=""small-caption"" width=""14%"" bgcolor=""#e3b7eb"">feedback</td><td class=""small-caption"" width=""14%"" bgcolor=""#ffcd85"">acknowledged</td><td class=""small-caption"" width=""14%"" bgcolor=""#fff494"">confirmed</td><td class=""small-caption"" width=""14%"" bgcolor=""#c2dfff"">assigned</td><td class=""small-caption"" width=""14%"" bgcolor=""#d2f5b0"">resolved</td><td class=""small-caption"" width=""14%"" bgcolor=""#c9ccc4"">closed</td></tr></table></div>

	<br />
	<hr size=""1"" />
<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0""><tr valign=""top""><td>	<address>Copyright &copy; 2000 - 2014 MantisBT Team</address>
	<address><a href=""mailto:clover.zhou@delboisgroup.com"">clover.zhou@delboisgroup.com</a></address>
</td><td>
	<div align=""right""><a href=""http://www.mantisbt.org"" title=""Free Web Based Bug Tracker""><img src=""/images/mantis_logo.png"" width=""145"" height=""50"" alt=""Powered by Mantis Bugtracker"" border=""0"" /></a></div>
</td></tr></table>
</body>
</html>
";
        #endregion
        static void Main(string[] args) {

            //Xy.Data.DataBase _db = new Xy.Data.DataBase("MySql");

            //Xy.Data.Procedure _procedure = new Xy.Data.Procedure("test", "select * from Test_Table");
            //System.Data.DataTable _dt = _procedure.InvokeProcedureFill(_db);
            //Console.WriteLine(Xy.Tools.IO.XML.ConvertDataTableToXMLWithFormat(_dt));

            //Xy.Data.Procedure _procedure = new Xy.Data.Procedure("GetCount", new Xy.Data.ProcedureParameter[]{
            //    new Xy.Data.ProcedureParameter("ID", System.Data.DbType.Int64, string.Empty, 2),
            //    new Xy.Data.ProcedureParameter("Count", System.Data.DbType.Int64){ Direction = System.Data.ParameterDirection.Output }
            //});
            //_procedure.InvokeProcedure(_db);
            //Console.WriteLine(_procedure.GetItem("Count").ToString());

            //Xy.Data.Procedure _procedure = new Xy.Data.Procedure("GetResult", new Xy.Data.ProcedureParameter[]{
            //    new Xy.Data.ProcedureParameter("NumA", System.Data.DbType.Int64, string.Empty, 2),
            //    new Xy.Data.ProcedureParameter("NumB", System.Data.DbType.Int64, string.Empty, 8)
            //});
            //Console.WriteLine(_procedure.InvokeProcedureResult(_db).ToString());

            //_db.Open();
            //_db.StartTransaction();
            //if (_db.IsInTransaction) {
            //    Xy.Data.Procedure _procedure1 = new Xy.Data.Procedure("Insert", new Xy.Data.ProcedureParameter[]{
            //        new Xy.Data.ProcedureParameter("Name", System.Data.DbType.AnsiString, string.Empty, "测试名称十二"),
            //        new Xy.Data.ProcedureParameter("ID", System.Data.DbType.Int64){ Direction = System.Data.ParameterDirection.Output }
            //    });
            //    _procedure1.InvokeProcedure(_db);
            //    Console.WriteLine(_procedure1.GetItem("ID").ToString());
            //    Xy.Data.Procedure _procedure2 = new Xy.Data.Procedure("Insert", new Xy.Data.ProcedureParameter[]{
            //        new Xy.Data.ProcedureParameter("Name", System.Data.DbType.AnsiString, string.Empty, "测试名称十三"),
            //        new Xy.Data.ProcedureParameter("ID", System.Data.DbType.Int64){ Direction = System.Data.ParameterDirection.Output }
            //    });
            //    _procedure2.InvokeProcedure(_db);
            //    Console.WriteLine(_procedure2.GetItem("ID").ToString());
            //    _db.RollbackTransation();
            //    _db.Close();
            //}

            //Array _MySqlDbType = Enum.GetValues(typeof(MySql.Data.MySqlClient.MySqlDbType));
            //foreach (int i in _MySqlDbType) {
            //    MySql.Data.MySqlClient.MySqlDbType _currentMySqlDbType = (MySql.Data.MySqlClient.MySqlDbType)i;
            //    MySql.Data.MySqlClient.MySqlParameter _mp = new MySql.Data.MySqlClient.MySqlParameter("Name", _currentMySqlDbType);
            //    Console.WriteLine(_currentMySqlDbType + " | " + _mp.DbType);
            //}
            //int test1 = 3;
            //DateTime test2 = new DateTime();
            //System.Data.DataTable test3 = new System.Data.DataTable();
            //TestClass test4 = new TestClass();
            //string[] test5 = new string[] { "abc", "bcd", "cde" };
            //object testObject = test5;
            //Console.WriteLine(testObject.ToString());
            //Console.WriteLine(Convert.ToString(test5));

//            string _xml =
//@"<DataTable>
//    <DataItem>
//        <ID>1</ID>
//    </DataItem>
//</DataTable>";
//            System.Xml.XPath.XPathDocument _xPathDocument = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_xml));
//            System.Xml.XPath.XPathNavigator _xPathNavigator = _xPathDocument.CreateNavigator();
//            System.Xml.XmlDocument _xmlDocument = new System.Xml.XmlDocument();
//            _xmlDocument.LoadXml(_xPathNavigator.OuterXml);
//            _xPathNavigator = _xmlDocument.CreateNavigator();
//            _xPathNavigator.MoveToFirstChild();
//            _xPathNavigator.AppendChild("<Page>1</Page>");
//            _xPathDocument = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_xPathNavigator.OuterXml));
//            Console.WriteLine(_xmlDocument.OuterXml);
            //string url = "http://sh.qq.com/a/20140827/035326.htm?ADUIN=2571538841&ADSESSION=1409106268&ADTAG=CLIENT.QQ.5347_.0&ADPUBNO=26378";

            //Xy.Tools.Web.UrlAnalyzer _url = new Xy.Tools.Web.UrlAnalyzer("http://admin.xiaoyang.local/test/");
            //Console.WriteLine(_url.Site + " | " + _url.Domain + " | " + _url.Path + " | " + _url.BasePath + " | " + _url.HasRoot("test"));
            //_url.SetRoot("test");
            //Console.WriteLine(_url.Site + " | " + _url.Domain + " | " + _url.Path + " | " + _url.BasePath);

            //_url = new Xy.Tools.Web.UrlAnalyzer("http://admin.xiaoyang.local/");
            //Console.WriteLine(_url.Site + " | " + _url.Domain + " | " + _url.Path);

            //System.Collections.Specialized.NameValueCollection _nvc = new System.Collections.Specialized.NameValueCollection();
            //Console.WriteLine(_nvc["abc"] == null);
            //Console.WriteLine(_nvc["abc"]);
            //Console.WriteLine(_nvc["abc"] == null);
            //_nvc["abc"] = "abc";
            //Console.WriteLine(_nvc["abc"]);
            //Console.WriteLine(_nvc["abc"] == null);

            testA testc = new testB();
            testc.functionA();
            testc.functionB();
            testc.functionC();
        }

        public abstract class testA {
            public void functionA() {
                Console.WriteLine("testA,functionA");
            }
            public virtual void functionB() {
                Console.WriteLine("testA,functionB");
            }
            public void functionC() {
                Console.WriteLine("testA,functionC");
            }
        }
        public class testB : testA {
            public new void functionA(){
                Console.WriteLine("testB,functionA");
            }
            public override void functionB() {
                Console.WriteLine("testB,functionB");
            }
            public void functionC() {
                Console.WriteLine("testB,functionC");
            }
        }
        #region Url Test
        //static string _urlString1 = "http://www.homes-up.com/mango-wood-tray-leaves-and-trees-homes-up-brown-25x9cm.html?test=abcd#tag";
        //static string _urlString2 = "http://123.103.21.196/index.php/rongguang/cms_page_revision/edit/page_id/32/revision_id/553/key/4741f1df80cd46768f49ecaa9a287097741db121a122d03c66b6c871f62968e6/index.php?test=%e6%b5%8b%e8%af%95";
        //static string _urlString3 = "/index.php/rongguang/cms_page_revision/edit/page_id/32/revision_id/553/key/4741f1df80cd46768f49ecaa9a287097741db121a122d03c66b6c871f62968e6/";

        //static void Main(string[] args) {
        //    //Console.WriteLine(Xy.Tools.Debug.Log.LogType.Report.ToString());
        //    //Console.WriteLine(DateTime.Now.Hour + " | " + DateTime.Now.Minute);
        //    //return;
        //    //System.Uri _url = new Uri(_urlString);
        //    //Console.WriteLine(Xy.Tools.Debug.Decompose.DecomposeObject(_url).ToString());

        //    //Xy.Tools.Debug.TimeWatch _tw = new Xy.Tools.Debug.TimeWatch();
        //    //_tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent1);
        //    //_tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent2);
        //    //_tw.Watch(100000, 3, true);
        //    //Console.WriteLine(Xy.Tools.Debug.Decompose.DecomposeObject(new Uri(_urlString2)).ToString());
        //    Console.WriteLine((float)25 / (float)100);
        //}

        //static void _tw_WatchEvent2() {
        //    Xy.Tools.Web.UrlAnalyzer _url = new Xy.Tools.Web.UrlAnalyzer(_urlString2);
        //    string _test = _url.Domain;
        //}

        //static void _tw_WatchEvent1() {
        //    System.Uri _url = new Uri(_urlString2);
        //    string _test = _url.Host;
        //}
        #endregion

        #region Analyze Test
        //        static System.Text.RegularExpressions.Regex _decodeReg1 = new System.Text.RegularExpressions.Regex(@"(@|\!|#)?\w+(=(?<qoute>'|"").*?\k<qoute>)?", System.Text.RegularExpressions.RegexOptions.Compiled);
        //        static System.Text.RegularExpressions.Regex _decodeReg2 = new System.Text.RegularExpressions.Regex(@"(@|\!|#)?\w+(=(""[^""]*""|'[^']*'))?", System.Text.RegularExpressions.RegexOptions.Compiled);
        //        static string[] _testString = new string[]{ 
        //                "@Test Tag=\'abc\' Double=\"ddc\" Test=\"b\'d\'c\"",
        //                @"@Data Provider=""Procedure"" Procedure=""XiaoYang_Post_Post_GetList"" Parameter=""{ PageIndex='Form|i' PageSize='Form|i' Title='Form' TypeID='Form|i' ClassID='Form|i' WebConfig='Form' #TotalCount='0|i' }""
        //                            DefaultParameter=""{ PageIndex='0' PageSize='10' Title='' WebConfig='' TypeID='-1' ClassID='-1' }""
        //                            EnableScript=""True"" Root=""/""",
        //                "@SetData Name=\"PageClass\"",
        //                "{ ID='Data:CurrentUser.UserGroup|i' }"
        //                };
        //        static void Main(string[] args) {
        //            //for (int i = 0; i < _testString.Length; i++) {
        //            //    Console.WriteLine(_testString[i] + "============================================");
        //            //    Decode3(_testString[i]);
        //            //    //System.Text.RegularExpressions.MatchCollection _matckresult = _decodeReg2.Matches(_testString[i]);
        //            //    //for (int j = 0; j < _matckresult.Count; j++) {
        //            //    //    System.Text.RegularExpressions.Match match = _matckresult[j];
        //            //    //    Console.WriteLine(match.Value);
        //            //    //}
        //            //}
        //            //return;
        //            Xy.Tools.Debug.TimeWatch tw = new Xy.Tools.Debug.TimeWatch();
        //            tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(tw_WatchEvent1);
        //            tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(tw_WatchEvent2);
        //            tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(tw_WatchEvent3);
        //            tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(tw_WatchEvent4);
        //            tw.Watch(10000, 3, true);
        //        }

        //        static void tw_WatchEvent4() {
        //            for (int i = 0; i < _testString.Length; i++) {
        //                Decode3(_testString[i]);
        //            }
        //        }

        //        static void tw_WatchEvent3() {
        //            for (int i = 0; i < _testString.Length; i++) {
        //                Decode2(_testString[i]);
        //            }
        //        }

        //        static void tw_WatchEvent2() {
        //            for (int i = 0; i < _testString.Length; i++) {
        //                Xy.Tools.Control.Tag.Decode(_testString[i]);
        //            }
        //        }

        //        static void tw_WatchEvent1() {
        //            for (int i = 0; i < _testString.Length; i++) {
        //                Decode1(_testString[i]);
        //            }
        //        }

        //        public static System.Collections.Specialized.NameValueCollection Decode1(string markString) {
        //            System.Text.RegularExpressions.MatchCollection _matckresult = _decodeReg1.Matches(markString);
        //            if (_matckresult.Count > 0) {
        //                System.Collections.Specialized.NameValueCollection marks = new System.Collections.Specialized.NameValueCollection();
        //                for (int i = 0; i < _matckresult.Count; i++) {
        //                    System.Text.RegularExpressions.Match match = _matckresult[i];
        //                    string tempname, tempvalue;
        //                    if (match.Value.IndexOf('=') > 0) {
        //                        tempname = match.Value.Substring(0, match.Value.IndexOf('='));
        //                        tempvalue = match.Value.Substring(match.Value.IndexOf('=') + 1);
        //                    } else {
        //                        tempname = match.Value;
        //                        tempvalue = string.Empty;
        //                    }
        //                    tempname = tempname.Trim();
        //                    tempvalue = tempvalue.Trim('\"').Trim('\'');
        //                    marks.Add(tempname, tempvalue);
        //                }
        //                return marks;
        //            } else {
        //                throw new Exception(string.Format("Can not analyze \"{0}\"", markString));
        //            }
        //        }

        //        public static System.Collections.Specialized.NameValueCollection Decode2(string markString) {
        //            System.Text.RegularExpressions.MatchCollection _matckresult = _decodeReg2.Matches(markString);
        //            if (_matckresult.Count > 0) {
        //                System.Collections.Specialized.NameValueCollection marks = new System.Collections.Specialized.NameValueCollection();
        //                for (int i = 0; i < _matckresult.Count; i++) {
        //                    System.Text.RegularExpressions.Match match = _matckresult[i];
        //                    string tempname, tempvalue;
        //                    if (match.Value.IndexOf('=') > 0) {
        //                        tempname = match.Value.Substring(0, match.Value.IndexOf('='));
        //                        tempvalue = match.Value.Substring(match.Value.IndexOf('=') + 1);
        //                    } else {
        //                        tempname = match.Value;
        //                        tempvalue = string.Empty;
        //                    }
        //                    tempname = tempname.Trim();
        //                    tempvalue = tempvalue.Trim('\"').Trim('\'');
        //                    marks.Add(tempname, tempvalue);
        //                }
        //                return marks;
        //            } else {
        //                throw new Exception(string.Format("Can not analyze \"{0}\"", markString));
        //            }
        //        }

        //        public static System.Collections.Specialized.NameValueCollection Decode3(string markString) {
        //            System.Collections.Specialized.NameValueCollection marks = new System.Collections.Specialized.NameValueCollection();
        //            bool _valueMode = false;
        //            char _qoute = ' ';
        //            StringBuilder _item = new StringBuilder();
        //            for (int i = 0; i < markString.Length; i++) {
        //                char c = markString[i];
        //                if (!_valueMode && (c == ' ' || c == '\r' || c == '\n')) {
        //                    foundone(marks, _item.ToString());
        //                    _item = new StringBuilder();
        //                    continue;
        //                }
        //                _item.Append(c);
        //                if (c == _qoute) {
        //                    _valueMode = false;
        //                    _qoute = ' ';
        //                    foundone(marks, _item.ToString());
        //                    _item = new StringBuilder();
        //                    continue;
        //                }
        //                if (!_valueMode && (c == '\'' || c == '\"')) {
        //                    _valueMode = true;
        //                    _qoute = c;
        //                }
        //                if (i == markString.Length - 1) 
        //                    foundone(marks, _item.ToString());
        //            }
        //            if (marks.Count == 0) throw new Exception(string.Format("Can not analyze \"{0}\"", markString));
        //            return marks;
        //        }

        //        private static void foundone(System.Collections.Specialized.NameValueCollection nvc, string item) {
        //            if (string.IsNullOrEmpty(item)) return;
        //            //Console.WriteLine(item);
        //            string tempname, tempvalue;
        //            if (item.IndexOf('=') > 0) {
        //                tempname = item.Substring(0, item.IndexOf('='));
        //                tempvalue = item.Substring(item.IndexOf('=') + 1);
        //            } else {
        //                tempname = item;
        //                tempvalue = string.Empty;
        //            }
        //            tempname = tempname.Trim();
        //            tempvalue = tempvalue.Trim('\"').Trim('\'');
        //            nvc.Add(tempname, tempvalue);

        //        }
        #endregion

        #region XSLT test resource
        //        static string _testDataString = @"<DataTable><DataItem><_RowNumber><![CDATA[1]]></_RowNumber><ID><![CDATA[49]]></ID><ClassID><![CDATA[8]]></ClassID><TypeID><![CDATA[0]]></TypeID><Title><![CDATA[测试用文章49]]></Title><Content><![CDATA[测试用文章49]]></Content><IsActive><![CDATA[False]]></IsActive><IssueTime><![CDATA[2007/6/30 0:00:00]]></IssueTime><Created><![CDATA[2007/6/21 13:55:46]]></Created><UpdateTime><![CDATA[2007/6/21 13:55:46]]></UpdateTime><WebConfig><![CDATA[default]]></WebConfig><ClassName><![CDATA[最新公告]]></ClassName></DataItem><DataItem><_RowNumber><![CDATA[2]]></_RowNumber><ID><![CDATA[48]]></ID><ClassID><![CDATA[6]]></ClassID><TypeID><![CDATA[0]]></TypeID><Title><![CDATA[测试用文章48]]></Title><Content><![CDATA[测试用文章48]]></Content><IsActive><![CDATA[False]]></IsActive><IssueTime><![CDATA[2007/6/30 0:00:00]]></IssueTime><Created><![CDATA[2007/6/21 13:55:46]]></Created><UpdateTime><![CDATA[2007/6/21 13:55:46]]></UpdateTime><WebConfig><![CDATA[default]]></WebConfig><ClassName><![CDATA[技术文档]]></ClassName></DataItem><DataItem><_RowNumber><![CDATA[3]]></_RowNumber><ID><![CDATA[47]]></ID><ClassID><![CDATA[5]]></ClassID><TypeID><![CDATA[0]]></TypeID><Title><![CDATA[测试用文章47]]></Title><Content><![CDATA[测试用文章47]]></Content><IsActive><![CDATA[False]]></IsActive><IssueTime><![CDATA[2007/6/30 0:00:00]]></IssueTime><Created><![CDATA[2007/6/21 13:55:46]]></Created><UpdateTime><![CDATA[2007/6/21 13:55:46]]></UpdateTime><WebConfig><![CDATA[default]]></WebConfig><ClassName><![CDATA[技术文档]]></ClassName></DataItem><DataItem><_RowNumber><![CDATA[4]]></_RowNumber><ID><![CDATA[46]]></ID><ClassID><![CDATA[4]]></ClassID><TypeID><![CDATA[0]]></TypeID><Title><![CDATA[测试用文章46]]></Title><Content><![CDATA[测试用文章46]]></Content><IsActive><![CDATA[False]]></IsActive><IssueTime><![CDATA[2007/6/30 0:00:00]]></IssueTime><Created><![CDATA[2007/6/21 13:55:46]]></Created><UpdateTime><![CDATA[2007/6/21 13:55:46]]></UpdateTime><WebConfig><![CDATA[default]]></WebConfig><ClassName><![CDATA[项目介绍]]></ClassName></DataItem><DataItem><_RowNumber><![CDATA[5]]></_RowNumber><ID><![CDATA[1]]></ID><ClassID><![CDATA[3]]></ClassID><TypeID><![CDATA[0]]></TypeID><Title><![CDATA[测试用文章1]]></Title><Content><![CDATA[测试用文章1]]></Content><IsActive><![CDATA[False]]></IsActive><IssueTime><![CDATA[2007/6/30 0:00:00]]></IssueTime><Created><![CDATA[2007/6/21 13:55:46]]></Created><UpdateTime><![CDATA[2007/6/21 13:55:46]]></UpdateTime><WebConfig><![CDATA[default]]></WebConfig><ClassName><![CDATA[项目介绍]]></ClassName></DataItem></DataTable>";
        //        static string _testTemplateString = 
        //@"<?xml version=""1.0"" encoding=""utf-8""?>
        //<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
        //  <xsl:output method=""html"" omit-xml-declaration=""yes"" version=""1.0"" encoding=""utf-8"" />
        //  <xsl:variable name=""PageIndex"">0</xsl:variable>
        //  <xsl:variable name=""PageSize"">10</xsl:variable>
        //  <xsl:variable name=""Title""></xsl:variable>
        //  <xsl:variable name=""TypeID"">-1</xsl:variable>
        //  <xsl:variable name=""ClassID"">-1</xsl:variable>
        //  <xsl:variable name=""TotalCount"">5</xsl:variable>
        //  <xsl:template match=""/"">
        //    <table class=""table table-bordered table-mid"">
        //      <!-- Table heading -->
        //      <thead>
        //        <tr>
        //          <th>ID</th>
        //          <th>标题</th>
        //          <th>分类</th>
        //          <th>类型</th>
        //          <th>修改时间</th>
        //          <th>操作</th>
        //        </tr>
        //      </thead>
        //      <!-- // Table heading END -->
        //      <!-- Table body -->
        //      <tbody>
        //        <!-- Table row -->
        //        <xsl:for-each select=""DataTable/DataItem"">
        //          <tr class=""item{ID}"">
        //            <td>
        //              <xsl:value-of select=""ID"" />
        //            </td>
        //            <td>
        //              <xsl:value-of select=""Title"" />
        //            </td>
        //            <td>
        //              <xsl:value-of select=""ClassName"" />
        //            </td>
        //            <td>
        //              <xsl:choose>
        //                <xsl:when test=""string-length(TypeName) = 0"">
        //                  未定义类型
        //                </xsl:when>
        //                <xsl:otherwise>
        //                  <xsl:value-of select=""TypeName"" />
        //                </xsl:otherwise>
        //              </xsl:choose>
        //            </td>
        //            <td>
        //              <xsl:value-of select=""UpdateTime"" />
        //            </td>
        //            <td>
        //              <a target=""_blank"" href=""post_edit_{ID}.[% @Tag Provider='Config' Name='ext' %]"" class=""btn"">修改</a>
        //              <a class=""btn ajaxlink"" ajax-confirm=""您确定要删除该发布?"" href=""/postAction_post_del.[% @Tag Provider='Config' Name='ext' %]"" ajax-data=""{{ ID:{ID} }}"" ajax-success=""function(){{UpdateContent();}}"">删除</a>
        //            </td>
        //          </tr>
        //        </xsl:for-each>
        //        <!-- // Table row END -->
        //      </tbody>
        //      <!-- // Table body END -->
        //    </table>
        //    <!-- // Table END -->
        //    <div class=""row-fluid mt10"">
        //      <div class=""span6"">
        //        <a class=""btn btn-default btn-primary"" href=""post_edit_0.[% @Tag Provider='Config' Name='ext' %]"">添加发布</a>
        //      </div>
        //    </div>
        //  </xsl:template>
        //</xsl:stylesheet>";
        //        static System.Xml.Xsl.XslCompiledTransform _testXSLTTransform = new System.Xml.Xsl.XslCompiledTransform();
        //        static System.Xml.XPath.XPathDocument _testXML = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_testDataString));
        //        #endregion

        //        static void Main(string[] args) {
        //            _testXSLTTransform.Load(System.Xml.XmlReader.Create(new System.IO.StringReader(_testTemplateString)));

        //            //Xy.Tools.Debug.TimeWatch _tw = new Xy.Tools.Debug.TimeWatch();
        //            //_tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent1);
        //            //_tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent2);
        //            //_tw.Watch(100, 3, true);

        //            Xy.Tools.Debug.TimeWatch _tw = new Xy.Tools.Debug.TimeWatch();
        //            _tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent1);
        //            _tw.WatchEvent += new Xy.Tools.Debug.TimeWatch.WatchFunction(_tw_WatchEvent2);
        //            _tw.Watch(100, 3, true);
        //        }

        //        //static void _tw_WatchEvent2() {
        //        //    _testTemplateString.GetHashCode();
        //        //}

        //        //static void _tw_WatchEvent1() {
        //        //    _testTemplateString.ToString();
        //        //}

        //        static void _tw_WatchEvent2() {
        //            System.Xml.XPath.XPathDocument xml = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_testDataString));
        //            using (System.IO.Stream str = new System.IO.MemoryStream()) {
        //                _testXSLTTransform.Transform(xml, null, str);
        //                str.Position = 0;
        //                byte[] result = new byte[str.Length];
        //                if (str.Length > 0) {
        //                    str.Read(result, 0, result.Length);
        //                } else {
        //                    result = new byte[0];
        //                }
        //                _testXSLTTransform.TemporaryFiles.Delete();
        //                //Console.WriteLine(System.Text.Encoding.UTF8.GetString(result));
        //            }
        //        }

        //        static void _tw_WatchEvent1() {
        //            System.Xml.XPath.XPathDocument xml = new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_testDataString));
        //            System.Xml.Xsl.XslCompiledTransform xslTransform = new System.Xml.Xsl.XslCompiledTransform();
        //            xslTransform.Load(System.Xml.XmlReader.Create(new System.IO.StringReader(_testTemplateString)));
        //            using (System.IO.Stream str = new System.IO.MemoryStream()) {
        //                xslTransform.Transform(xml, null, str);
        //                str.Position = 0;
        //                byte[] result = new byte[str.Length];
        //                if (str.Length > 0) {
        //                    str.Read(result, 0, result.Length);
        //                } else {
        //                    result = new byte[0];
        //                }
        //                xslTransform.TemporaryFiles.Delete();
        //                //Console.WriteLine(System.Text.Encoding.UTF8.GetString(result));
        //            }
        //        }
        #endregion
    }
}