<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Новое приватное сообщение</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Заполните приведенную ниже форму для отправки приватного сообщения. HTML отключен. UBBCode включен, и вы можете использовать UBBCode в ваших сообщениях. Анонимные сообщения разрешены, и вы можете выбрать любое незарегистрированное имя.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/SendPM/" name="replier">
									<xsl:text>Пользователь: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<xsl:text>Получатель: </xsl:text>
									<xsl:choose>
										<xsl:when test="receiver">
											<input type="hidden" name="receiverId">
												<xsl:attribute name="value"><xsl:value-of select="receiver/account/id"/></xsl:attribute>
											</input>
											<xsl:value-of select="receiver/account/user/name"/>
										</xsl:when>
										<xsl:otherwise>
											<input type="text" name="receiverName" class="formboxes" size="15"/>
										</xsl:otherwise>
									</xsl:choose>
									<br/>
									<br/>
									<xsl:text>Тема: </xsl:text>
									<br/>
									<input type="text" tabindex="1" name="title" maxlength="70" class="formboxes" size="60"/>
									<br/>
									<br/>
									<xsl:call-template name="textEditor"/>
									<br/>
									<input type="checkbox" name="preview" value="1" class="formboxes" disabled="disabled" />
									<xsl:text>Я хочу предварительно просмотреть сообщение перед отправкой</xsl:text>
									<br/>
									<br/>
									<input type="checkbox" name="spellcheck" value="1" class="formboxes" onChange="document.replier.preview.checked=this.checked;" disabled="disabled" />
									<xsl:text>Проверить правописание</xsl:text>
									<br/>
									<br/>
									<input type="submit" tabindex="3" name="textcont" taborder="2" value="Продолжить" class="buttons"/>
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>

	</xsl:template>

</xsl:stylesheet>