<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="UserHeader.xslt"/>

	<xsl:template name="usersHeader">
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/All/</xsl:with-param>
									<xsl:with-param name="text">Все</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/Active/</xsl:with-param>
									<xsl:with-param name="text">Активные (смигрированные)</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/Users/Online/</xsl:with-param>
									<xsl:with-param name="text">В онлайне</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:if test="user">
									<xsl:call-template name="headerLink">
										<xsl:with-param name="url">/Users/User/<xsl:value-of select="user/id"/>/</xsl:with-param>
										<xsl:with-param name="text"><xsl:value-of select="user/name"/></xsl:with-param>
									</xsl:call-template>
								</xsl:if>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<xsl:if test="user">
			<xsl:call-template name="userHeader"/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

