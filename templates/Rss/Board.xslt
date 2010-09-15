<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Темы раздела </xsl:text>
		<xsl:value-of select="currentLocation/board/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="threads/thread"/>
	</xsl:template>
</xsl:stylesheet>