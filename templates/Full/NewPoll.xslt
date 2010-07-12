<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Новый опорос</xsl:text>
	</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Создание нового опроса</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>Заполните приведенную ниже форму для отправки сообщения в форум. HTML отключен. UBBCode включен, и вы можете использовать UBBCode в ваших сообщениях. Анонимные сообщения разрешены, и вы можете выбрать любое незарегистрированное имя.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/NewPoll/" name="replier">
									<xsl:text>Пользователь: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<br/>
									<xsl:text>Тема: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="title">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<input type="checkbox" name="isDetailed" value="isDetailed"/><label for="isDetailed"> Показывать имена проголосовавших</label>
									<br/>
									<input type="checkbox" name="isMultiOption" value="isMultiOption"/><label for="isMultiOption"> Разрешить выбор нескольких вариантов</label>
									<br/>
									<br/>
									<xsl:text>Вариант 1: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 2: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 3: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 4: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 5: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 6: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 7: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 8: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 9: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 10: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 11: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 12: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 13: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 14: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 15: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 16: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 17: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 18: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 19: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 20: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
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