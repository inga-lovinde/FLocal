<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\PMInfo.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:value-of select="conversationInfo/interlocutor/account/user/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="conversationInfo"/>
		<br />
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="1" width="100%" class="tableborders">
						<tr class="tdheader">
							<td>
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<xsl:text>страницы:</xsl:text>
											<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<xsl:apply-templates select="messages/message"/>
						<tr class="tdheader">
							<td>
								<table width="100%" cellspacing="1" cellpadding="3" border="0">
									<tr>
										<td>
											<xsl:text>страницы:</xsl:text>
											<xsl:apply-templates select="messages/pageOuter" mode="withCurrent"/>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<xsl:call-template name="conversationInfo"/>
	</xsl:template>

	<xsl:template name="conversationInfo">
		<p>
			<font class="catandforum">
				<a href="/My/Conversations/"><xsl:text>Приватные сообщения</xsl:text></a>
				<xsl:text> &gt;&gt; </xsl:text>
				<xsl:value-of select="conversation/interlocutor/account/user/name"/>
			</font>
		</p>
	</xsl:template>

</xsl:stylesheet>