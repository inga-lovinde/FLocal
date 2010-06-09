<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="xml" indent="yes" encoding="UTF-8"/>
	<xsl:template match="/root">
		<html>
			<head>
				<base href="http://forumlocal.ru/"/>
				<link rel="stylesheet" href="/stylesheets/global.css" type="text/css" />
				<link rel="stylesheet" href="/stylesheets/coffeehaus.css" type="text/css" />
				<link rel="stylesheet" href="/stylesheets/decoration.css" type="text/css" />
				<link type="text/css" rel="stylesheet" href="SyntaxHighlighter/styles/shCore.css"/>
				<link type="text/css" rel="stylesheet" href="SyntaxHighlighter/styles/shThemeDefault.css"/>
				<link rel="shortcut icon" href="/favicons/smirk.ico" type="image/x-icon" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<table border="0" cellpadding="2" cellspacing="0" width="95%" class="tableborders" align="center">
					<tr>
						<td align="center">
							<table border="0" cellpadding="2" cellspacing="0" width="100%">
								<tr>
									<td width="100%" class="topmenu" align="center">
										<a href ="/" >Root</a>
										| <a href ="http://google.com/" target="_blank">Google</a>
										| <a href ="http://yandex.ru/" target="_blank">Yandex</a>
										| <a href ="http://mail.ru" target="_blank">Mail.ru</a>
										| <a href ="http://www.vedomosti.ru/" target="_blank">Vedomosti</a>
										| <a href ="http://www.afisha.ru/" target="_blank">Afisha</a>
										| <a href ="http://weather.yandex.ru/27612/" target="_blank">Weather</a>
										| <a href ="/sendmail.php" target="_blank">LAN Support</a>
									</td>
								</tr>
								<tr>
									<td width="100%" colspan="3" height="1" class="tableborders">
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<table width="95%" align="center" class="tablesurround" cellpadding="1" cellspacing="1">
					<tr>
						<td>
							<table width="100%" class="tableborders" cellpadding="3" cellspacing="1">
								<tr>
									<td align="center" class="menubar">
										<a href = "/ubbthreads.php?Cat=" target="_top">Список форумов</a>
										 |
										<a href = "/word-search.php" target="_top">Поиск</a>
										 |
										<a href = "/login.php?Cat=" target="_top">My Home</a>
										 |
										<a href = "/online.php?Cat=" target="_top">Кто в онлайне</a>
										 |
										<a href = "/faq_russian.php" target="_top">FAQ</a>
										 |
										<a href = "/logout.php?key=8b46d53aaa0096f6695478f81503082c" target="_top">Выход</a>
										 | <a href="/showmembers.php?Cat=&amp;page=1" target="_top">Пользователи</a>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<center>&#160;&#160;</center>
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
										<img src="/images/newposts.gif" alt="*" />
										Новые сообщения
										<br />
										<img src="/images/nonewposts.gif" alt="*" />
										Нет новых сообщений
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<br />
			</body>
		</html>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center">Темы</td>
				<td class="tdheader" align="center">Сообщений</td>
				<td class="tdheader" align="center">Последнее</td>
				<td class="tdheader" align="center">Модератор</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

	<xsl:template match="board">
		<tr>
			<td valign="top" width="1%" class="darktable">
				<a href="/ubbthreads.php?Cat=&amp;C=&amp;check=Common&amp;src=">
					<xsl:attribute name="onClick">if (!confirm('Пометить все сообщения как прочитанные?')) {event.returnValue=false; return false;}</xsl:attribute>
					<img border="0" width="17" height="21" src="/images/newposts.gif" alt=""/>
				</a>
			</td>
			<td width="55%" class="darktable">
				<font class="forumtitle">
					<a href="/postlist.php?Cat=&amp;Board=Common&amp;time=1275991384&amp;showlite="><xsl:value-of select="name"/></a>
				</font>
				<br />
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td class="forumdescript">
							<a href="/apostlist.php?Cat=&amp;Board=Common">A</a>&#160;&#160;
						</td>
						<td class="forumdescript"><xsl:value-of select="description"/></td>
					</tr>
				</table>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap">373</td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap">128648</td>
			<td width="15%" nowrap="nowrap" class="posttime">
				08.06.2010 14:03<br />
				<a href="/showflat.php?Cat=&amp;Board=Common&amp;Number=9554129&amp;src=#Post9554129">от igor</a>
			</td>
			<td width="10%" class="modcolumn" align="center">
				&#160;<a href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>,
				<a href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>

<!--
        <h1>Persons</h1>
        <ul>
          <xsl:apply-templates select="person">
            <xsl:sort select="family-name" />
          </xsl:apply-templates>
        </ul>
      </body>
    </html>
  </xsl:template>
 
  <xsl:template match="person">
    <li>
      <xsl:value-of select="family-name"/><xsl:text>, </xsl:text>
      <xsl:value-of select="name"/>
    </li>
  </xsl:template>
 
-->