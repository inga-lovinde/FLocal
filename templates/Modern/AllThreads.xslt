<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_AllThreads"/>
	</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<div>
			<xsl:call-template name="Messages_PageOuterTitle"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		</div>
		<div class="threadscontainer">
			<xsl:apply-templates select="threads/thread"/>
		</div>
		<div>
			<xsl:call-template name="Messages_PageOuterTitle"/>
			<xsl:text>:</xsl:text>
			<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		</div>
	</xsl:template>

</xsl:stylesheet>