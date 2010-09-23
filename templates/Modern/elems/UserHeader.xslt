<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template name="userHeader">
		<xsl:variable name="baseLink">/Users/User/<xsl:value-of select="user/id"/>/</xsl:variable>
		<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
			<tr>
				<td>
					<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
						<tr>
							<td align="center" class="menubar">
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Info/</xsl:with-param>
									<xsl:with-param name="text">Информация</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Threads/</xsl:with-param>
									<xsl:with-param name="text">Темы</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Posts/</xsl:with-param>
									<xsl:with-param name="text">Сообщения</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>Replies/</xsl:with-param>
									<xsl:with-param name="text">Ответы</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url"><xsl:value-of select="$baseLink"/>PollsParticipated/</xsl:with-param>
									<xsl:with-param name="text">Опросы</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<img src="/static/images/shortcut.png" border="0"/>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/Conversations/Conversation/<xsl:value-of select="accountId"/>/</xsl:with-param>
									<xsl:with-param name="text">История</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(accountId)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:text> | </xsl:text>
								<img src="/static/images/shortcut.png" border="0"/>
								<xsl:call-template name="headerLink">
									<xsl:with-param name="url">/My/Conversations/PMSend/<xsl:value-of select="accountId"/>/</xsl:with-param>
									<xsl:with-param name="text">Написать</xsl:with-param>
									<xsl:with-param name="isDisabled">
										<xsl:if test="not(accountId)">true</xsl:if>
									</xsl:with-param>
								</xsl:call-template>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>
</xsl:stylesheet>

