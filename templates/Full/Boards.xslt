<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specific">
				<table width="95%" align="center" class="tablesurround" cellspacing="1" cellpadding="1">
					<tr>
						<td>
							<xsl:apply-templates select="categories/category"/>
						</td>
					</tr>
				</table>
				<br />
				<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
					<tr>
						<td>
							<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
								<tr>
									<td colspan="3" class="tdheader">
										<b>Дополнительная информация</b>
									</td>
								</tr>
								<tr class="lighttable">
									<td width="45%" class="small" valign="top">
										<span>Вы вошли в форум как summersun</span>
										<br />
										<span>18983 Зарегистрированных пользователей.</span>
										<br />
										<span>Приветствуем нового пользователя,</span>
										<a href="/showprofile.php?User=_PC&amp;What=ubbthreads">_PC</a>
										<br />
										<span>Сейчас 222 зарегистрированных и 54 анонимных пользователей в онлайне.</span>
										<br />
										<a href="/editdisplay.php?Cat=#offset">Текущее время:</a>
										<span>08.06.2010 14:17, Вторник</span>
									</td>
									<td width="30%" class="small" valign="top">
										<b>Просмотр новых сообщений</b>
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Words=&amp;Match=Entire+Phrase&amp;Limit=25&amp;src=all">Последние 7 дней</a> 
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Uname=summersun&amp;search_replies=1&amp;Limit=25&amp;src=all">Последние ответы на мои сообщения</a>
										<br />
										<a href="/dosearch.php?Cat=&amp;Forum=All_Forums&amp;Uname=summersun&amp;Limit=25&amp;src=all&amp;onlyrated=1">Мои сообщения с оценками</a>
										<br />
										<a href="/toprated.php?showlite=">Рейтинги сообщений</a>
									</td>
									<td class="small" valign="top">
										<b>Легенда:</b>
										<br />
										<img src="/static/images/newposts.gif" alt="*" />
										<span>Новые сообщения</span>
										<br />
										<img src="/static/images/nonewposts.gif" alt="*" />
										<span>Нет новых сообщений</span>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2" width="61%"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center" width="7%">Темы</td>
				<td class="tdheader" align="center" width="7%">Сообщений</td>
				<td class="tdheader" align="center" width="15%">Последнее</td>
				<td class="tdheader" align="center" width="10%">Модератор</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

</xsl:stylesheet>