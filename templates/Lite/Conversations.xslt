<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\ConversationInfo.xslt"/>
	<xsl:template name="specificTitle">Личные сообщения</xsl:template>
	<xsl:template name="specific">
		<p>
			<xsl:text>Приватные сообщения</xsl:text>
			<xsl:text> [</xsl:text>
			<a>
				<xsl:if test="session/sessionKey">
					<xsl:attribute name="href">/PMSend/</xsl:attribute>
				</xsl:if>
				<xsl:text>New Message</xsl:text>
			</a>
			<xsl:text>]</xsl:text>
		</p>
		<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
			<tr>
				<td align="left" nowrap="nowrap" class="tdheader">Автор</td>
			</tr>
			<!-- BEGIN POST LOOP DO NOT DELETE -->
			<xsl:apply-templates select="conversations/conversation"/>
			<!-- END OF LOOP -->
			<tr class="tdheader">
				<td colspan="3">
					<font class="onbody">
						<xsl:text>страницы:</xsl:text>
						<xsl:apply-templates select="conversations/pageOuter" mode="withCurrent">
							<xsl:with-param name="baseLink">/Conversations/</xsl:with-param>
						</xsl:apply-templates>
					</font>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>