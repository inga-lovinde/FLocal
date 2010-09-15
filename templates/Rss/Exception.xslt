<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Ошибка</xsl:template>
	<xsl:template name="specific">
		<exception>
			<type><xsl:value-of select="exception/type"/></type>
			<message><xsl:value-of select="exception/message"/></message>
			<guid><xsl:value-of select="exception/guid"/></guid>
		</exception>
	</xsl:template>

</xsl:stylesheet>