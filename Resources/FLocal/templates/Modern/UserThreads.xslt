<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Темы - </xsl:text>
		<xsl:value-of select="user/name"/>
	</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		<div class="threadscontainer">
			<xsl:apply-templates select="threads/thread[not(isAnnouncement='true')]"/>
		</div>
		<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
	</xsl:template>

</xsl:stylesheet>
