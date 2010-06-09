<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="xml" indent="yes" encoding="UTF-8"/>
	<xsl:template name="header">
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
	</xsl:template>
</xsl:stylesheet>

