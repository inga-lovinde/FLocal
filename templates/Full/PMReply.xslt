<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\MyHeader.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Ответ на личное сообщение - </xsl:text>
		<xsl:value-of select="message/interlocutor/account/user/name"/>
	</xsl:template>
	<xsl:template name="specific">
		<xsl:call-template name="myHeader"/>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Ответ на личное сообщение (</xsl:text>
								<xsl:value-of select="message/interlocutor/account/user/name"/>
								<xsl:text>)</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Заполните приведенную ниже форму для отправки личного сообщения. HTML отключен. UBBCode включен, и вы можете использовать UBBCode в ваших сообщениях. Анонимные сообщения разрешены, и вы можете выбрать любое незарегистрированное имя.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/SendPM/" name="replier">
									<input type="hidden" name="receiverId">
										<xsl:attribute name="value"><xsl:value-of select="message/interlocutor/account/id"/></xsl:attribute>
									</input>
									<xsl:text>Пользователь: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<br/>
									<xsl:text>Тема: </xsl:text>
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
									<input type="submit" tabindex="3" name="textcont" taborder="2" value="Продолжить" class="buttons"/>
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
								<xsl:text>Ответ на сообщение</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<b>
									<xsl:text>Автор: </xsl:text>
									<xsl:value-of select="message/poster/account/user/name"/>
									<br/>
									<xsl:text>Тема: </xsl:text>
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