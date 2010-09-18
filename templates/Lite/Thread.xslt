<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/thread/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="threadInfo"/>
		<div class="tdheader">
			<xsl:text>страницы:</xsl:text>
			<xsl:apply-templates select="posts/pageOuter" mode="withCurrent"/>
		</div>
		<xsl:apply-templates select="posts/post">
			<xsl:with-param name="isReplyDisabled"><xsl:value-of select="thread/isLocked"/></xsl:with-param>
		</xsl:apply-templates>
		<div class="tdheader">
			<xsl:apply-templates select="posts/pageOuter" mode="footer">
				<xsl:with-param name="baseLink">/Thread/<xsl:value-of select="currentLocation/thread/id"/>/</xsl:with-param>
			</xsl:apply-templates>
		</div>
	</xsl:template>

	<xsl:template name="threadInfo">
		<p>
			<font class="catandforum">
				<xsl:apply-templates select="currentLocation" mode="breadcrumbs"/>
			</font>
		</p>
	</xsl:template>

</xsl:stylesheet>