<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_NewPM">
			<xsl:with-param name="userName" select="message/interlocutor/account/user/name"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_NewPM">
									<xsl:with-param name="userName" select="message/interlocutor/account/user/name"/>
								</xsl:call-template>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/SendPM/" name="replier">
									<input type="hidden" name="receiverId">
										<xsl:attribute name="value"><xsl:value-of select="message/interlocutor/account/id"/></xsl:attribute>
									</input>
									<xsl:apply-templates select="session/user" mode="Messages_CurrentUser"/>
									<br/>
									<xsl:apply-templates select="message/interlocutor/account/user" mode="Messages_Receiver"/>
									<br/>
									<br/>
									<xsl:call-template name="Messages_Title"/>
									<xsl:text>:</xsl:text>
									<br/>
									<input type="text" tabindex="1" name="title" maxlength="70" class="formboxes" size="60">
										<xsl:choose>
											<xsl:when test="substring(message/title, 1, 4)='Re: '">
												<xsl:attribute name="value"><xsl:value-of select="message/title"/></xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="value">
													<xsl:text>Re: </xsl:text>
													<xsl:value-of select="message/title"/>
												</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
									</input>
									<br/>
									<br/>
									<xsl:call-template name="textEditor">
										<xsl:with-param name="body">
											<xsl:text>[q]</xsl:text>
											<xsl:value-of select="quoted"/>
											<xsl:text>[/q]
</xsl:text>
										</xsl:with-param>
									</xsl:call-template>
									<input type="submit" tabindex="3" name="textcont" taborder="2" class="buttons">
										<xsl:attribute name="value">
											<xsl:call-template name="Messages_NewPM">
												<xsl:with-param name="userName" select="message/interlocutor/account/user/name"/>
											</xsl:call-template>
										</xsl:attribute>
									</input>
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>

		<br/>

		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_OriginalMessage"/>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<b>
									<xsl:apply-templates select="message/interlocutor/account/user" mode="Messages_Author"/>
									<br/>
									<xsl:call-template name="Messages_Title"/>
									<xsl:text>: </xsl:text>
									<xsl:value-of select="message/title"/>
								</b>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable">
								<xsl:value-of select="message/body" disable-output-escaping="yes"/>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>