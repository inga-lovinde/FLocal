<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ConversationInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_PrivateMessages"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent"/>
		<div class="conversationscontainer">
			<xsl:apply-templates select="conversations/conversation"/>
		</div>
		<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent"/>
	</xsl:template>

</xsl:stylesheet>