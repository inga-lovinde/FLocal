<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>Модерирование сообщения - </xsl:text>
		<xsl:value-of select="post/title"/>
	</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Модерирование сообщения (</xsl:text>
								<xsl:value-of select="board/name"/>
								<xsl:text>)</xsl:text>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Punish/" name="replier">
									<input type="hidden" name="postId">
										<xsl:attribute name="value"><xsl:value-of select="post/id"/></xsl:attribute>
									</input>
									<xsl:text>Тип:</xsl:text><br/>
									<xsl:apply-templates select="punishmentTypes/punishmentType"/>
									<br/>
									<br/>
									<xsl:text>Комментарий: </xsl:text>
									<br/>
									<input type="text" tabindex="1" name="comment" class="formboxes" size="60"/>
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

	<xsl:template match="punishmentType">
		<input type="radio" name="punishmentTypeId">
			<xsl:attribute name="id">punishmentTypeId_<xsl:value-of select="id"/></xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
		</input>
		<label>
			<xsl:attribute name="for">punishmentTypeId_<xsl:value-of select="id"/></xsl:attribute>
			<xsl:text> </xsl:text>
			<xsl:value-of select="description"/>
			<xsl:text>, вес наказания </xsl:text>
			<xsl:value-of select="weight"/>
			<xsl:text> (</xsl:text>
			<xsl:value-of select="weightDescription"/>
			<xsl:text>), срок действия </xsl:text>
			<xsl:apply-templates select="timeSpan"/>
		</label>
		<br/>
	</xsl:template>

</xsl:stylesheet>