<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="UserHeader.xslt"/>

	<xsl:template name="usersHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/All/</xsl:with-param>
				<xsl:with-param name="text">Все</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/Active/</xsl:with-param>
				<xsl:with-param name="text">Активные</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/Online/</xsl:with-param>
				<xsl:with-param name="text">Онлайн</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="user">
				<xsl:text>&#160;&#160;&#160;</xsl:text>
				<xsl:call-template name="headerLink">
					<xsl:with-param name="url">/Users/User/<xsl:value-of select="user/id"/>/</xsl:with-param>
					<xsl:with-param name="text"><xsl:value-of select="user/name"/></xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="user">
				<xsl:call-template name="userHeader"/>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>

