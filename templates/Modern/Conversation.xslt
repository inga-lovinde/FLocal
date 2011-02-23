<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PMInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="conversation/interlocutor/account/user/name"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div>
			<xsl:call-template name="Messages_PageOuter"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
		</div>
		<div class="postscontainer">
			<xsl:apply-templates select="messages/message"/>
		</div>
		<div>
			<xsl:call-template name="Messages_PageOuter"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
		</div>
	</xsl:template>

</xsl:stylesheet>