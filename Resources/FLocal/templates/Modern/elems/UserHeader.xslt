<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="userHeader">
		<xsl:variable name="baseLink">/Users/User/<xsl:value-of select="user/id"/>/</xsl:variable>
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Info/</xsl:with-param>
				<xsl:with-param name="text">����������</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Threads/</xsl:with-param>
				<xsl:with-param name="text">����</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Posts/</xsl:with-param>
				<xsl:with-param name="text">���������</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Mentions/</xsl:with-param>
				<xsl:with-param name="text">�������</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>PollsParticipated/</xsl:with-param>
				<xsl:with-param name="text">������</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Conversations/Conversation/<xsl:value-of select="accountId"/>/</xsl:with-param>
				<xsl:with-param name="text">�������</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(accountId)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Conversations/PMSend/<xsl:value-of select="accountId"/>/</xsl:with-param>
				<xsl:with-param name="text">��������</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(accountId)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
		</div>
	</xsl:template>
</xsl:stylesheet>

