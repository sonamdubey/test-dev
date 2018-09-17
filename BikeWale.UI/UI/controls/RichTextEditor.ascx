<%@ Control Language="C#" Inherits="Bikewale.Controls.RichTextEditor" AutoEventWireUp="false" %>
<!-- TinyMCE -->
<script type="text/javascript" src="/UI/editor/tiny_mce/tiny_mce.js?v=1.0"></script>
<script type="text/javascript">
    tinyMCE.init({
        // General options
        mode: "textareas",
        theme: "advanced",
        plugins: "advimage,advlink,emotions,iespell,preview,visualchars,advlist",

        // Theme options
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,image,code,preview,|,removeformat,emotions,iespell",
        theme_advanced_buttons2: "",
        theme_advanced_buttons3: "",
        theme_advanced_buttons4: "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,

        // Example content CSS (should be your site CSS)
        content_css: "css/content.css",

        // Drop lists for link/image/media/template dialogs
        template_external_list_url: "/UI/editor/lists/template_list.js?v=1.0",
        external_link_list_url: "/UI/editor/lists/link_list.js?v=1.0",
        external_image_list_url: "/UI/editor/lists/image_list.js?v=1.0",
        media_external_list_url: "/UI/editor/lists/media_list.js?v=1.0",

        // Style formats
        style_formats: [
			{ title: 'Bold text', inline: 'b' },
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
			{ title: 'Example 1', inline: 'span', classes: 'example1' },
			{ title: 'Example 2', inline: 'span', classes: 'example2' },
			{ title: 'Table styles' },
			{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],

        // Replace values for the template plugin
        template_replace_values: {
            username: "Some User",
            staffid: "991234"
        }
    });
</script>
<!-- /TinyMCE -->


<asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine"></asp:TextBox>
