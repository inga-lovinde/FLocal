<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/board/name"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="isRssEnabled">true</xsl:template>
	<xsl:template name="specific">
		<xsl:if test="boards/board">
			<div class="boardscontainer" style="position:relative;z-index:2000;">
				<xsl:apply-templates select="boards/board"/>
			</div>
		</xsl:if>
		<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
		<div class="threadscontainer">
			<xsl:if test="threads/thread[isAnnouncement='true']">
				<div style="position:relative;z-index:1000">
					<xsl:apply-templates select="threads/thread[isAnnouncement='true']"/>
				</div>
			</xsl:if>
			<div style="position:relative;z-index:0">
				<xsl:apply-templates select="threads/thread[not(isAnnouncement='true')]"/>
			</div>
		</div>
		<xsl:apply-templates select="threads/pageOuter" mode="withCurrent"/>
	</xsl:template>

</xsl:stylesheet>