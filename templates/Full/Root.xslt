<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template name="specificTitle">�������</xsl:template>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/coffeehaus.css" type="text/css" />
				<title><xsl:value-of select="title"/></title>
				<link rel="stylesheet" href="/static/css/decoration.css" />
			</head>
			<body>
				<center>
					<font size="-2">������ �������:</font>
					<br/>
					<a href="http://www.yandex.ru" target="_blank"><img src="/icons/yandex.gif" alt="Yandex"/></a>
					<a href="http://www.google.com" target="_blank"><img src="/icons/google.gif" alt="Google"/></a>
					<a href="http://www.mail.ru" target="_blank"><img src="/icons/mailru.gif" alt="Mail.ru"/></a>
					<a href="http://www.rambler.ru" target="_blank"><img src="/icons/rambler.gif" alt="Rambler"/></a>
					<br/>
					<br/>
					<a href="http://www.msumarket.ru/"><img src="/banners/msumarket-hor.jpg"/></a>
				</center>
				<br/>
				<table WIDTH="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
					<tr>
						<td>
							<table cellpadding="1" cellspacing="0" width="100%" class="tableborders" height="100%">
								<tr class="menubar">
									<td width="90">Internet &gt;&gt;</td>
									<td><a href="https://172.16.0.11/">��������...</a></td>
									<td align="right"><a><font color="red">�������� � ����� ��� ����������?</font></a></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="session/user">
									<xsl:text>Hello, </xsl:text><xsl:value-of select="session/user/name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>Hello, Guest</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
							<table cellpadding="1" cellspacing="0" width="100%" class="tableborders" height="100%">
								<xsl:variable name="prefix">
									<xsl:text>https://</xsl:text>
									<xsl:value-of select="url/host"/>
									<xsl:text>:</xsl:text>
									<xsl:if test="url/port &gt;= 1000">
										<xsl:value-of select="floor(url/port div 1000)"/>
									</xsl:if>
								</xsl:variable>
								<tr class="menubar">
									<td align="left" width="50%" id="decor">
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>443/Boards/</xsl:attribute>
											<xsl:text>������������</xsl:text>
										</a>
										<xsl:text>&#160;|&#160;</xsl:text>
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>447/Boards/</xsl:attribute>
											<xsl:text>����</xsl:text>
										</a>
										<xsl:text>&#160;|&#160;</xsl:text>
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>445/Boards/</xsl:attribute>
											<xsl:text>����� XML</xsl:text>
										</a>
									</td>
									<td align="right" width="50%">
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>445/Boards/</xsl:attribute>
											<xsl:text>Raw XML</xsl:text>
										</a>
										<xsl:text>&#160;|&#160;</xsl:text>
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>447/Boards/</xsl:attribute>
											<xsl:text>Lite</xsl:text>
										</a>
										<xsl:text>&#160;|&#160;</xsl:text>
										<a>
											<xsl:attribute name="href"><xsl:value-of select="$prefix"/>443/Boards/</xsl:attribute>
											<xsl:text>Classic</xsl:text>
										</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://search.snto.ru">����� � ����</a>
									</td>
									<td align="right" width="50%">
										<a href="http://search.snto.ru">Search in network</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://radio.local:8000">����� � ����</a>
									</td>
									<td align="right" width="50%">
										<a href="http://radio.local:8000">Radio</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://lib.mexmat.ru/">����������� ���������� �������</a>
									</td>
									<td align="right" width="50%">
										<a href="http://lib.mexmat.ru/">Scientific Library</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://lair.mexmat.net/wiki/weather">������</a>
									</td>
									<td align="right" width="50%">
										<a href="http://lair.mexmat.net/wiki/weather">Weather</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://lib.v.ru/">������� ���������� �������</a>
									</td>
									<td align="right" width="50%">
										<a href="http://lib.v.ru/">Lib.ru Mirror</a>
									</td>
								</tr>
								<tr class="menubar">
									<td align="left" width="50%">
										<a href="http://search.snto.ru/shownet.pl">����� ����</a>
									</td>
									<td align="right" width="50%">
										<a href="http://search.snto.ru/shownet.pl">Browse network</a>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
					<tr>
						<td>
							����� ���� � ����:
							<form name="searchform" method="get">
								<input type="text" name="what" size="25"/>
								<input type="hidden" name="restype" value="all"/>
							</form>
						</td>
					</tr>
				</table>
				<center>
					<font size="-2">����� �����������:</font>
					<br/>
					<a href="http://www.megabook.ru/Megabook2/"><img src="/icons/megabook.gif" alt="������������ &quot;������� � �������&quot;"/></a>
					<a href="http://lingvo.yandex.ru"><img src="/icons/lingvo.gif" alt="Lingvo - ����������� �������"/></a>
					<a href="http://www.eatlas.ru"><img src="/icons/eatlas.gif" alt="����� ������� � ����� ����"/></a>
					<a href="http://www.gismeteo.ru"><img src="/icons/gismeteo.gif" alt="������ �� �����"/></a>
					<a href="http://www.afisha.ru"><img src="/icons/afisha.gif" alt="Afisha - ��� ����������� ������"/></a>
					<a href="http://www.translate.ru"><img src="/icons/logo_a.gif" alt="Online ����������"/></a>
					<a href="http://metallibrary.ru/"><img src="/icons/metlib-88x31.gif" alt="Metal Library"/></a>
					<br/>
					<hr width="95%"/>
					<font size="-2">��� ������� ������:</font>
					<br/>
					<a href="http://www.cd-studio.ru/index_full.php"><img src="/icons/rabot.gif" alt="Rabot"/></a>
					<br/>
					<a href="http://www.fxmoney.ru/"><img src="/banners/fxmoney.gif" alt="FXMoney"/></a>
					<br/>
					<a href="http://www.zhelezona.ru/"><img src="/user/upload/file10116.png" alt="ZheleZona"/></a>
					<br/>
					<br/>
					<br/>
				</center>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>

