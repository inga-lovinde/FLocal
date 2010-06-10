<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css" type="text/css" />
				<link rel="stylesheet" href="/static/css/coffeehaus.css" type="text/css" />
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<xsl:call-template name="header"/>
				<xsl:call-template name="specific"/>
				<br />
Data used for authoring this XHTML document:
<xmp><xsl:copy-of select="/"/></xmp>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>

<!--
        <h1>Persons</h1>
        <ul>
          <xsl:apply-templates select="person">
            <xsl:sort select="family-name" />
          </xsl:apply-templates>
        </ul>
      </body>
    </html>
  </xsl:template>
 
  <xsl:template match="person">
    <li>
      <xsl:value-of select="family-name"/><xsl:text>, </xsl:text>
      <xsl:value-of select="name"/>
    </li>
  </xsl:template>
 
-->