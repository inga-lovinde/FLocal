<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\UploadInfo.xslt"/>
	<xsl:template name="specificTitle">Аплоад</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="rssRelativeLink"><xsl:value-of select="/root/currentBaseUrl"/>0-reversed</xsl:template>
	<xsl:template name="specific">
		<xsl:apply-templates select="uploads/pageOuter" mode="withCurrent"/>
		<div class="uploadscontainer">
			<xsl:apply-templates select="uploads/upload"/>
		</div>
		<xsl:apply-templates select="uploads/pageOuter" mode="withCurrent"/>
	</xsl:template>

</xsl:stylesheet>