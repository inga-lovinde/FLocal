<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PostInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="currentLocation/post/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="threadInfo"/>
		<br />
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="1" width="100%" class="tableborders">
						<xsl:apply-templates select="posts/post">
							<xsl:with-param name="isReplyDisabled"><xsl:value-of select="thread/isLocked"/></xsl:with-param>
						</xsl:apply-templates>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template name="threadInfo">
		<p>
			<font class="catandforum">
				<xsl:apply-templates select="currentLocation" mode="breadcrumbs"/>
			</font>
		</p>
	</xsl:template>

</xsl:stylesheet>