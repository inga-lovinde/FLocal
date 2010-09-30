<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="boardHeader">
		<xsl:variable name="baseLink">/Forum/Board/<xsl:value-of select="currentLocation//board[not(name(../..)='board')]/id"/>-<xsl:value-of select="currentLocation//board[not(name(../..)='board')]/nameTranslit"/>/</xsl:variable>
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Threads/</xsl:with-param>
				<xsl:with-param name="text">Темы</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Headlines/</xsl:with-param>
				<xsl:with-param name="text">Заголовки</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>NewThread/</xsl:with-param>
				<xsl:with-param name="text">Новая (?)</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="currentLocation//thread">
				<xsl:text>&#160;&#160;&#160;</xsl:text>
				<xsl:call-template name="headerLink">
					<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Thread/<xsl:value-of select="currentLocation//thread/id"/>-<xsl:value-of select="currentLocation//thread/nameTranslit"/>/</xsl:with-param>
					<xsl:with-param name="postfix">
						<xsl:if test="currentLocation//post">
							<xsl:text>Posts/p</xsl:text>
							<xsl:value-of select="currentLocation//post/id"/>
						</xsl:if>
					</xsl:with-param>
					<xsl:with-param name="text"><xsl:value-of select="substring(currentLocation//thread/name, 1, 30)"/></xsl:with-param>
				</xsl:call-template>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>

