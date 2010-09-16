<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ConversationInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Личные сообщения</xsl:text>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="conversations/conversation"/>
	</xsl:template>
</xsl:stylesheet>