<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template name="specificTitle">Главная</xsl:template>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css" type="text/css" />
				<link rel="stylesheet" type="text/css">
					<xsl:attribute name="href">/static/css/<xsl:value-of select="skin/name"/>.css</xsl:attribute>
				</link>
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<center>
					<table width="95%" height="95%">
						<tr>
							<td align="center" valign="middle">
								<h1>
									<xsl:choose>
										<xsl:when test="session/user">
											<xsl:text>Hello, </xsl:text><xsl:value-of select="session/user/name"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>Hello, Guest</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</h1>
								<xsl:variable name="prefix">
									<xsl:text>https://</xsl:text>
									<xsl:value-of select="url/host"/>
									<xsl:text>:</xsl:text>
									<xsl:if test="url/port &gt;= 1000">
										<xsl:value-of select="floor(url/port div 1000)"/>
									</xsl:if>
								</xsl:variable>
								<h2>Выберите дизайн</h2>
								<h4>
									<a>
										<xsl:attribute name="href"><xsl:value-of select="$prefix"/>443/Boards/</xsl:attribute>
										<xsl:text>Классический</xsl:text>
									</a>
								</h4>
								<h4>
									<a>
										<xsl:attribute name="href"><xsl:value-of select="$prefix"/>447/Boards/</xsl:attribute>
										<xsl:text>Лайт</xsl:text>
									</a>
								</h4>
								<h4>
									<a>
										<xsl:attribute name="href"><xsl:value-of select="$prefix"/>445/Boards/</xsl:attribute>
										<xsl:text>Сырой XML</xsl:text>
									</a>
								</h4>
							</td>
						</tr>
					</table>
				</center>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>

