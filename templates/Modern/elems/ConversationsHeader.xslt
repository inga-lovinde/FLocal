<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="conversationsHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Conversations/List/</xsl:with-param>
				<xsl:with-param name="text">Список</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Conversations/NewPM/</xsl:with-param>
				<xsl:with-param name="text">Новое (?)</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="conversation">
				<xsl:text>&#160;&#160;&#160;</xsl:text>
				<xsl:call-template name="headerLink">
					<xsl:with-param name="url">
						<xsl:text>/My/Conversations/Conversation/</xsl:text>
						<xsl:value-of select="conversation/interlocutor/account/id"/>
						<xsl:text>/</xsl:text>
					</xsl:with-param>
					<xsl:with-param name="text"><xsl:value-of select="conversation/interlocutor/account/user/name"/></xsl:with-param>
				</xsl:call-template>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>

