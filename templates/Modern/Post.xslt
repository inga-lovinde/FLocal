<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/post/name"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div id="postsContainer">
			<xsl:apply-templates select="posts/post">
				<xsl:with-param name="isReplyDisabled"><xsl:value-of select="thread/isLocked"/></xsl:with-param>
			</xsl:apply-templates>
		</div>
	</xsl:template>

</xsl:stylesheet>