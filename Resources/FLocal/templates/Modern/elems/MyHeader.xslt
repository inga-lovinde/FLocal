<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="ConversationsHeader.xslt"/>

	<xsl:template name="myHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Login/</xsl:with-param>
				<xsl:with-param name="text">Вход</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="session/sessionKey">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Conversations/</xsl:with-param>
				<xsl:with-param name="text">
					<xsl:choose>
						<xsl:when test="session/sessionKey and (session/indicators/unreadPrivateMessages != '0')">
							<b>
								<xsl:text>Инбокс (</xsl:text>
								<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
								<xsl:text>)</xsl:text>
							</b>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>Инбокс</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Settings/</xsl:with-param>
				<xsl:with-param name="text">Настройки</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/UserData/</xsl:with-param>
				<xsl:with-param name="text">Данные (?)</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/My/Avatars/</xsl:with-param>
				<xsl:with-param name="text">Аватарки</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/User/<xsl:value-of select="session/user/id"/>/Info/</xsl:with-param>
				<xsl:with-param name="text">Профиль</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/do/Logout/?sessionKey=<xsl:value-of select="session/sessionKey"/></xsl:with-param>
				<xsl:with-param name="text">Выход</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
				<xsl:with-param name="skipDatePostfix">true</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="starts-with(/root/currentUrl, '/My/Conversations/')">
				<xsl:call-template name="conversationsHeader"/>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>

