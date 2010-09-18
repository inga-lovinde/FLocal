<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="boardHeader">
		<xsl:variable name="baseLink">/Forum/Board/<xsl:value-of select="currentLocation/board/id"/>/</xsl:variable>
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Threads/</xsl:with-param>
									<xsl:with-param name="text">Темы</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Headlines/</xsl:with-param>
									<xsl:with-param name="text">Заглавные сообщения</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>NewThread/</xsl:with-param>
									<xsl:with-param name="text">Новая тема</xsl:with-param>
								</xsl:call-template>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>
</xsl:stylesheet>

