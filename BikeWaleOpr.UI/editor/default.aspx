<%@ Page Language="C#" ContentType="text/html" ValidateRequest="false" ResponseEncoding="iso-8859-1" %>

<script language="c#" runat="server">
	void SaveContent(object sender, EventArgs e)
	{
		Response.Write(Request.Form["txt"]);
	}
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Untitled Document</title>
</head>
<body>

<!-- TinyMCE -->
<script type="text/javascript" src="tiny_mce/tiny_mce.js"></script>
<script type="text/javascript">
	tinyMCE.init({
		// General options
		mode : "textareas",
		theme : "advanced",
		plugins : "safari,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

		// Theme options
		theme_advanced_buttons1 : "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
		theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
		theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
		theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
		theme_advanced_toolbar_location : "top",
		theme_advanced_toolbar_align : "left",
		theme_advanced_statusbar_location : "bottom",
		theme_advanced_resizing : true,

		// Example content CSS (should be your site CSS)
		content_css : "css/content.css",

		// Drop lists for link/image/media/template dialogs
		template_external_list_url : "lists/template_list.js",
		external_link_list_url : "lists/link_list.js",
		external_image_list_url : "lists/image_list.js",
		media_external_list_url : "lists/media_list.js",

		// Replace values for the template plugin
		template_replace_values : {
			username : "Some User",
			staffid : "991234"
		}
	});
</script>
<!-- /TinyMCE -->

<form runat="server">
	<h3>Full featured example</h3>

	<p>
		This page shows all available buttons and plugins that are included in the TinyMCE core package.
		There are more examples on how to use TinyMCE in the <a href="http://wiki.moxiecode.com/examples/tinymce/">Wiki</a>.
	</p>

	<!-- Gets replaced with TinyMCE, remember HTML in a textarea should be encoded -->
	<textarea id="txt" name="txt" rows="15" cols="80" style="width: 80%">
		&lt;p&gt;
		&lt;img src="media/logo.jpg" alt=" " hspace="5" vspace="5" width="250" height="48" align="right" /&gt;	TinyMCE is a platform independent web based Javascript HTML &lt;strong&gt;WYSIWYG&lt;/strong&gt; editor control released as Open Source under LGPL by Moxiecode Systems AB. It has the ability to convert HTML TEXTAREA fields or other HTML elements to editor instances. TinyMCE is very easy to integrate into other Content Management Systems.
		&lt;/p&gt;
		&lt;p&gt;
		We recommend &lt;a href="http://www.getfirefox.com" target="_blank"&gt;Firefox&lt;/a&gt; and &lt;a href="http://www.google.com" target="_blank"&gt;Google&lt;/a&gt; &lt;br /&gt;
		&lt;/p&gt;
	</textarea>

	<div>
		<!-- Some integration calls -->
		<a href="javascript:;" onMouseDown="tinyMCE.execCommand('mceInsertContent',false,'<b>Hello world!!</b>');">[Insert HTML]</a>
	</div>

	<br />
	<asp:Button ID="butSave" runat="server" Text="Save Content" OnClick="SaveContent"></asp:Button>
	<input type="reset" name="reset" value="Reset" />
</form>

</body>
</html>
