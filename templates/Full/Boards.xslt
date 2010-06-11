<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
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
										Вы вошли в форум как summersun
										<br />
										18983 Зарегистрированных пользователей.
										<br />
										Приветствуем нового пользователя,
										<a href="/showprofile.php?User=_PC&amp;What=ubbthreads">_PC</a>
										<br />
										Сейчас 222 зарегистрированных и 54 анонимных пользователей в онлайне.
										<br />
										<a href="/editdisplay.php?Cat=#offset">Текущее время:</a>
										 08.06.2010 14:17, Вторник
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
										Новые сообщения
										<br />
										<img src="/static/images/nonewposts.gif" alt="*" />
										Нет новых сообщений
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

	<xsl:template match="board">
		<tr>
			<td width="4%" class="darktable" align="center" valign="middle">
				<a>
					<xsl:attribute name="onClick">if(!confirm('Пометить все сообщения как прочитанные?')) {event.returnValue=false; return false;} else { alert("Not implemented yet"); }</xsl:attribute>
					<xsl:choose>
						<xsl:when test="hasNewPosts='true'">
							<img border="0" width="17" height="21" src="/static/images/newposts.gif" alt=""/>
						</xsl:when>
						<xsl:otherwise>
							<img border="0" width="17" height="21" src="/static/images/nonewposts.gif" alt=""/>
						</xsl:otherwise>
					</xsl:choose>
				</a>
			</td>
			<td width="57%" class="darktable">
				<font class="forumtitle">
					<a>
						<xsl:attribute name="href">/Threads/<xsl:value-of select="id"/>/</xsl:attribute>
						<xsl:value-of select="name"/>
					</a>
				</font>
				<br />
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td class="forumdescript" style="padding-right:0.5em">
							<a href="/apostlist.php?Cat=&amp;Board=Common">
								<xsl:attribute name="href">/Threads/<xsl:value-of select="id"/>/</xsl:attribute>
								A
							</a>
						</td>
						<td class="forumdescript"><xsl:value-of select="description"/></td>
					</tr>
					<xsl:if test="subBoards/board">
						<tr>
							<td class="forumdescript">&#160;</td>
							<td class="forumdescript" style="padding-top:0.3em">
								<xsl:apply-templates select="subBoards/board"/>
							</td>
						</tr>
					</xsl:if>
				</table>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap"><xsl:value-of select="totalThreads"/></td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap"><xsl:value-of select="totalPosts"/></td>
			<td width="15%" nowrap="nowrap" class="posttime">
				<xsl:apply-templates select="lastPostInfo"/>
			</td>
			<td width="10%" class="modcolumn" align="center">
				<a href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>,
				<a href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="lastPostInfo">
		<xsl:choose>
			<xsl:when test="post">
				<xsl:value-of select="post/date"/><br />
				<a>
					<xsl:attribute name="href">/Thread/NOTIMPLEMENTED/p<xsl:value-of select="post/id"/>/</xsl:attribute>
					от <xsl:value-of select="post/user/name"/>
				</a>
			</xsl:when>
			<xsl:otherwise>
				N/A
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="subBoards/board">
		<span style="margin-left:0.5em;margin-right:0.5em">
			<a>
				<xsl:attribute name="href">/Threads/<xsl:value-of select="id"/>/</xsl:attribute>
				<xsl:value-of select="description"/>
			</a>
		</span>
	</xsl:template>
</xsl:stylesheet>