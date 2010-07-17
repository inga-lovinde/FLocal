<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specificTitle">Разделы</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="categories/category"/>
	</xsl:template>

	<xsl:template match="category">
		<hr/>
		<p><b><xsl:value-of select="name"/></b></p>
		<xsl:apply-templates select="boards/board"/>
	</xsl:template>

</xsl:stylesheet>