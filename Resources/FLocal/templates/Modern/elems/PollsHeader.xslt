<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="pollsHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Polls/List/</xsl:with-param>
				<xsl:with-param name="text">Список</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/User/<xsl:value-of select="session/user/id"/>/PollsParticipated/</xsl:with-param>
				<xsl:with-param name="text">Мои (?)</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Polls/New/</xsl:with-param>
				<xsl:with-param name="text">Новый</xsl:with-param>
			</xsl:call-template>
		</div>
	</xsl:template>
</xsl:stylesheet>

