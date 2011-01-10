<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ConversationInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_PrivateMessages"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div>
			<xsl:call-template name="Messages_PageOuter"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent"/>
		</div>
		<div class="conversationscontainer">
			<xsl:apply-templates select="conversations/conversation"/>
		</div>
		<div>
			<xsl:call-template name="Messages_PageOuter"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent"/>
		</div>
	</xsl:template>

</xsl:stylesheet>