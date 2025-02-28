<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:template name="header">
		<p>
			<xsl:if test="session/sessionKey">
				<a>
					<xsl:attribute name="href">/My/Conversations/?<xsl:value-of select="current/date/ticks"/></xsl:attribute>
					<xsl:if test="session/indicators/unreadPrivateMessages != '0'">
						<img src="/static/images/newpm.gif" border="0">
							<xsl:attribute name="alt">
								<xsl:value-of select="session/indicators/unreadPrivateMessages"/>
								<xsl:text> new messages</xsl:text>
							</xsl:attribute>
						</img>
					</xsl:if>
					<xsl:text>PM</xsl:text>
				</a>
				<xsl:text> | </xsl:text>
				<a href="/Boards/">�������</a>
				<xsl:text> | </xsl:text>
				<a target="_top">
					<xsl:attribute name="href">/do/Logout/?sessionKey=<xsl:value-of select="session/sessionKey"/></xsl:attribute>
					<xsl:text>�����</xsl:text>
				</a>
			</xsl:if>
			<xsl:if test="session/notLoggedIn">
				<a target="_top">
					<xsl:attribute name="href">/My/Login/</xsl:attribute>
					<xsl:text>����</xsl:text>
				</a>
			</xsl:if>
		</p>
	</xsl:template>
</xsl:stylesheet>

