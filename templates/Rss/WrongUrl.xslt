<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Неверный адрес</xsl:template>
	<xsl:template name="specific">
		<wrongUrl><xsl:value-of select="path"/></wrongUrl>
	</xsl:template>

</xsl:stylesheet>