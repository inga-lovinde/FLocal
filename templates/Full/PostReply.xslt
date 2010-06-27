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
								<xsl:text>Ответ на сообщение (</xsl:text>
								<xsl:value-of select="board/name"/>
								<xsl:text>)</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Заполните приведенную ниже форму для отправки сообщения в форум. HTML отключен. UBBCode включен, и вы можете использовать UBBCode в ваших сообщениях. Анонимные сообщения разрешены, и вы можете выбрать любое незарегистрированное имя.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Reply/" name="replier">
									<input type="hidden" name="parent">
										<xsl:attribute name="value"><xsl:value-of select="post/id"/></xsl:attribute>
									</input>
									<xsl:text>Пользователь: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<br/>
									<xsl:text>Тема: </xsl:text>
									<br/>
									<input type="text" tabindex="1" name="title" maxlength="70" class="formboxes" size="60">
										<xsl:choose>
											<xsl:when test="substring(post/title, 1, 4)='Re: '">
												<xsl:attribute name="value"><xsl:value-of select="post/title"/></xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="value">
													<xsl:text>Re: </xsl:text>
													<xsl:value-of select="post/title"/>
												</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
									</input>
									<span class="small">Слой сообщения:</span> 
									<select class="formboxes" name="layerId">
										<xsl:apply-templates select="layers/layer">
											<xsl:with-param name="defaultLayerId"><xsl:value-of select="post/layerId"/></xsl:with-param>
										</xsl:apply-templates>
									</select>
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
									<xsl:value-of select="post/poster/user/name"/>
									<br/>
									<xsl:text>Тема: </xsl:text>
									<xsl:value-of select="post/title"/>
								</b>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable">
								<xsl:value-of select="post/body" disable-output-escaping="yes"/>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>