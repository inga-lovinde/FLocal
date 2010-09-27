<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Все темы</xsl:text>
	</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div>
			<xsl:text>страницы:</xsl:text>
			<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		</div>
		<div class="threadscontainer">
			<xsl:apply-templates select="threads/thread[not(isAnnouncement='true')]"/>
		</div>
		<div>
			<xsl:text>страницы:</xsl:text>
			<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		</div>
	</xsl:template>

</xsl:stylesheet>