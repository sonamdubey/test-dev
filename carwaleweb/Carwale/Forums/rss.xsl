<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <title><xsl:value-of select="/rss/channel/title" /></title>
        <style type="text/css">
		  body { font-family: Verdana, Helvetica, sans-serif; font-size: 13px; font-weight:400; }
		  h1 { color:#003366; border-bottom:1px solid #FF9900; font-size:22px; }
		  h2 { font-size:15px;font-weight:bold; }
          #header { text-align: center; width:90%; }
		  a, a:link, a:visited, a:active { color:#03c; }
		  a:hover { text-decoration:none; }
          #feedItems { width: 80%; border: 1px solid #333; padding:5px 15px 5px 15px; text-align: left; }
          #headerText { text-align: center; padding: 8px; border: 1px solid #FF9900; background-color: #FFF3E8; }
          .rssDescription { padding-left: 25px; }
		  .dt { color:#555; }
        </style>    
		<script type="text/javascript" src="/src/xsl.js" />
      </head>
      
      <body onload="go_decoding();">
        <div id="cometestme" style="display:none;">
          <xsl:text disable-output-escaping="no">&amp;amp;</xsl:text>
        </div>

        <div align="center">
        	<div id="header">
				<h1><xsl:value-of select="/rss/channel/title" /></h1>
            	<p align="center" id="headerText"><xsl:value-of select="/rss/channel/description" /> Subscribe to <b><a href="{link}"><xsl:value-of select="/rss/channel/title" /></a></b> 
				syndication feed by copying and pasting <a href="{link}"><b>https://www.carwale.com<xsl:value-of select="/rss/channel/link" /></b></a> into your favourite RSS reader.</p>
          	</div>
        	<br />
        	<div align="center">
				<div id="feedItems">
            		<xsl:for-each select="/rss/channel/item">
              			<div class="rssItem">
                			<h2 class="rssTitle"><a href="{link}"><xsl:value-of select="title" /></a></h2>
                			<div name="decodeable" class="rssDescription">
                  				<xsl:value-of select="description" disable-output-escaping="yes" /><br />
				  				<div class="dt" align="right"><xsl:value-of select="pubDate" /></div>
              				</div> 
			  			</div>            
            		</xsl:for-each>
				</div>
			</div>
		</div>
    </body>
    </html>
  </xsl:template>
  
</xsl:stylesheet>