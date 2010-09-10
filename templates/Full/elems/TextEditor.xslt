<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>

	<xsl:template name="textEditor">
		<xsl:param name="body"/>
		<input type="hidden" name="convert" value="markup"/>
		<xsl:text>Сообщение</xsl:text>
		<br/>
		<textarea cols="100" tabindex="2" rows="10" class="formboxes" name="Body">
			<xsl:attribute name="onKeyUp">storeCaret(this);</xsl:attribute>
			<xsl:attribute name="onClick">storeCaret(this);</xsl:attribute>
			<xsl:attribute name="onKeyPress">checkKeyPressed(document.replier,event,false);</xsl:attribute>
			<xsl:choose>
				<xsl:when test="bodyUBB">
					<xsl:value-of select="bodyUBB"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$body"/>
				</xsl:otherwise>
			</xsl:choose>
		</textarea>
		<br/>
		<br/>
		<script language="Javascript" type="text/javascript" src="/static/js/textEditor.js"><xsl:text> </xsl:text></script>
		<script language="Javascript" type="text/javascript">
<![CDATA[
function insertInBody(str) {
	insertAtCaret(document.replier.Body, str);
	document.replier.Body.focus();
}
]]>
		</script>
		<table border="0">
			<tr class="tdheader">
				<td><b>Смайлики</b></td>
				<td valign="top"><b>UBBCode</b></td>
				<td valign="top"><b>Шрифт</b></td>
			</tr>
			<tr>
				<td valign="top" align="left" nowrap="nowrap">
					<a pseudolink="pseudolink" onClick="insertInBody(' :) ');">
						<img src="/static/smileys/smile.gif" border="0" alt="smile" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :( ');">
						<img src="/static/smileys/frown.gif" border="0" alt="frown" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :o ');">
						<img src="/static/smileys/blush.gif" border="0" alt="blush" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :D ');">
						<img src="/static/smileys/laugh.gif" border="0" alt="laugh" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' ;) ');">
						<img src="/static/smileys/wink.gif" border="0" alt="wink" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :p ');">
						<img src="/static/smileys/tongue.gif" border="0" alt="tongue" />
					</a>
					<br/>
					<a pseudolink="pseudolink" onClick="insertInBody(' :cool: ');">
						<img src="/static/smileys/cool.gif" border="0" alt="cool" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :crazy: ');">
						<img src="/static/smileys/crazy.gif" border="0" alt="crazy" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :mad: ');">
						<img src="/static/smileys/mad.gif" border="0" alt="mad" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :shocked: ');">
						<img src="/static/smileys/shocked.gif" border="0" alt="shocked" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :smirk: ');">
						<img src="/static/smileys/smirk.gif" border="0" alt="smirk" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :grin: ');">
						<img src="/static/smileys/grin.gif" border="0" alt="grin" />
					</a>
					<br/>
					<a pseudolink="pseudolink" onClick="insertInBody(' :ooo: ');">
						<img src="/static/smileys/ooo.gif" border="0" alt="ooo" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a pseudolink="pseudolink" onClick="insertInBody(' :confused: ');">
						<img src="/static/smileys/confused.gif" border="0" alt="confused" />
					</a>
					<xsl:text> &#160; </xsl:text>
					<a>
						<xsl:text>More!</xsl:text>
					</a>
					<xsl:text> &#160; </xsl:text>
					<a href="/Upload/New/" target="_blank">
						<xsl:text>Upload</xsl:text>
					</a>
					<xsl:text> &#160; </xsl:text>
					<a>
						<xsl:text>Mix</xsl:text>
					</a>
				</td>
				<td valign="top" align="left" nowrap="nowrap">
					<table border="0" cellpadding="3" cellspacing="1" class="tablesurround">
						<tr>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('url');">URL</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('code');">Код</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('image');">Картинка</a>
							</td>
						</tr>
						<tr>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('liststart');">Начало списка</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('listitem');">Эл-т списка</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('listend');">Конец списка</a>
							</td>
						</tr>
						<tr>
							<!--
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('pollstart');">Начало<br/>голосования</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('polloption');">Вариант<br/>голосования</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('pollstop');">Конец<br/>голосования</a>
							</td>
							-->
							<td class="darktable" colspan="3" align="center">
								<a href="/Poll/Create/">Создать опрос</a>
							</td>
						</tr>
						<tr>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('bold');">Жирный</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('italics');">Наклон</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('quote');">Цитата</a>
							</td>
						</tr>
						<tr>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('user');">Пользователь</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('table');">Таблица</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('ecode');">Ecode</a>
							</td>
						</tr>
						<tr>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="alert('Not implemented');return;DoPrompt('video');">YouTube</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('math');">Math</a>
							</td>
							<td class="darktable">
								<a pseudolink="pseudolink" onclick="DoPrompt('tex');">TeX</a>
							</td>
						</tr>
					</table>
				</td>
				<td valign="top">
					<table border="0">
						<tr>
							<td>
								<select id="fontselect" class="formboxes" style="font-size:10px">
									<xsl:attribute name="onChange">idx = this.selectedIndex; this.selectedIndex = 0; DoFont(this.options[idx].value);</xsl:attribute>
									<option value="">[Font Face]</option>
									<script type="text/javascript">fontlist();</script>
								</select>
							</td>
						</tr>
						<tr>
							<td>
								<select id="sizeselect" class="formboxes" style="font-size:10px">
									<xsl:attribute name="onChange">idx = this.selectedIndex; this.selectedIndex = 0; DoSize(this.options[idx].value);</xsl:attribute>
									<option value="">[Font Size]</option>
									<option value="1">1</option>
									<option value="2">2</option>
									<option value="3">3</option>
									<option value="4">4</option>
									<option value="5">5</option>
									<option value="6">6</option>
								</select>
							</td>
						</tr>
					</table>
					<table border="1">
						<tr>
							<td bgcolor="red">
								<a pseudolink="pseudolink" onclick="DoColor('red');">&#160; &#160; </a>
							</td>
							<td bgcolor="green">
								<a pseudolink="pseudolink" onclick="DoColor('green');">&#160; &#160; </a>
							</td>
							<td bgcolor="blue">
								<a pseudolink="pseudolink" onclick="DoColor('blue');">&#160; &#160; </a>
							</td>
							<td bgcolor="white">
								<a pseudolink="pseudolink" onclick="DoColor('white');">&#160; &#160; </a>
							</td>
						</tr>
						<tr>
							<td bgcolor="orange">
								<a pseudolink="pseudolink" onclick="DoColor('orange');">&#160; &#160; </a>
							</td>
							<td bgcolor="yellow">
								<a pseudolink="pseudolink" onclick="DoColor('yellow');">&#160; &#160; </a>
							</td>
							<td bgcolor="black">
								<a pseudolink="pseudolink" onclick="DoColor('black');">&#160; &#160; </a>
							</td>
							<td bgcolor="purple">
								<a pseudolink="pseudolink" onclick="DoColor('purple');">&#160; &#160; </a>
							</td>
						</tr>
						<tr>
							<td bgcolor="pink">
								<a pseudolink="pseudolink" onclick="DoColor('pink');">&#160; &#160; </a>
							</td>
							<td bgcolor="brown">
								<a pseudolink="pseudolink" onclick="DoColor('brown');">&#160; &#160; </a>
							</td>
							<td bgcolor="#666666">
								<a pseudolink="pseudolink" onclick="DoColor('#666666');">&#160; &#160; </a>
							</td>
							<td>
								<a pseudolink="pseudolink" onclick="DoPrompt('color');">?</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>

		<br/>
		<input type="checkbox" name="preview" value="1" class="formboxes" onClick="changeActionToSelf(this)" id="preview" />
		<label for="preview"> Я хочу предварительно просмотреть сообщение перед отправкой</label>
		<xsl:if test="bodyIntermediate">
			<br/>
			<table width="100%" cellspacing="1" cellpadding="3" border="0">
				<tr>
					<td width="120" valign="top" class="darktable" rowspan="2">
						<xsl:apply-templates select="session/user" mode="userInfoBar"/>
					</td>
					<td class="subjecttable">
						<table width="100%" border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td align="left" width="65%" valign="top">
									<a target="_blank" class="separate">
										<img border="0" alt="" style="vertical-align: text-bottom">
											<xsl:attribute name="src">/static/images/message-normal-read.gif</xsl:attribute>
										</img>
									</a>
									<b class="separate"><xsl:value-of select="title"/></b>
									<br />
									<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="lighttable">
						<table width="100%" cellspacing="0" cellpadding="0" style="table-layout: fixed">
							<tr>
								<td>
									<br />
									<font class="post">
										<xsl:value-of select="bodyIntermediate" disable-output-escaping="yes" />
										<br />
										<br />
									</font>
								</td>
							</tr>
							<xsl:if test="session/user/signature != ''">
								<tr>
									<td>
										<div style="width:100%;max-height:50px;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden">
											<font size="-2"><xsl:value-of select="poster/user/signature" disable-output-escaping="yes" /><br /></font>
										</div>
									</td>
								</tr>
							</xsl:if>
						</table>
					</td>
				</tr>
			</table>
		</xsl:if>
		<br/>
		<br/>
	</xsl:template>

	<xsl:template match="layer">
		<xsl:param name="defaultLayerId"/>
		<option>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
			<xsl:if test="id=$defaultLayerId">
				<xsl:attribute name="selected">selected</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="name"/>
		</option>
	</xsl:template>

</xsl:stylesheet>