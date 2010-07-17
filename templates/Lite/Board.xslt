<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:import href="elems\ThreadInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/board/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<p>
			<xsl:apply-templates select="currentLocation" mode="breadcrumbs"/>
			<xsl:text> [</xsl:text>
			<a>
				<xsl:if test="session/sessionKey">
					<xsl:attribute name="href">/Board/<xsl:value-of select="currentLocation/board/id"/>/NewThread/</xsl:attribute>
				</xsl:if>
				<xsl:text>New Post</xsl:text>
			</a>
			<xsl:text>]</xsl:text>
		</p>
		<xsl:if test="boards/board">
			<xsl:apply-templates select="boards/board"/>
		</xsl:if>
		<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
			<tr>
				<td align="left" width="55%" class="tdheader">Тема</td>
				<td align="left" nowrap="nowrap" width="15%" class="tdheader">Автор</td>
			</tr>
			<!-- BEGIN POST LOOP DO NOT DELETE -->
			<xsl:apply-templates select="threads/thread"/>
			<!-- END OF LOOP -->
			<tr class="tdheader">
				<td colspan="2">
					<font class="onbody">
						<xsl:text>страницы:</xsl:text>
						<xsl:apply-templates select="threads/pageOuter" mode="withCurrent">
							<xsl:with-param name="baseLink">/Board/<xsl:value-of select="currentLocation/board/id"/>/</xsl:with-param>
						</xsl:apply-templates>
					</font>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>