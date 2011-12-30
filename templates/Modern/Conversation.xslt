<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PMInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="conversation/interlocutor/account/user/name"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
		<div class="postscontainer">
			<xsl:apply-templates select="messages/message"/>
		</div>
		<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
	</xsl:template>

</xsl:stylesheet>