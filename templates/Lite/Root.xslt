<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/coffeehaus.css" type="text/css" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<p>
					<xsl:choose>
						<xsl:when test="session/user">
							<xsl:text>Hello, </xsl:text><xsl:value-of select="session/user/name"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>Hello, Guest</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</p>
				<xsl:variable name="prefix">
					<xsl:text>https://</xsl:text>
					<xsl:value-of select="url/host"/>
					<xsl:text>:</xsl:text>
					<xsl:if test="url/port &gt;= 1000">
						<xsl:value-of select="floor(url/port div 1000)"/>
					</xsl:if>
				</xsl:variable>
				<p>
					<a>
						<xsl:attribute name="href"><xsl:value-of select="$prefix"/>443/Boards/</xsl:attribute>
						<xsl:text>Классический</xsl:text>
					</a>
					<xsl:text>&#160;|&#160;</xsl:text>
					<a>
						<xsl:attribute name="href"><xsl:value-of select="$prefix"/>447/Boards/</xsl:attribute>
						<xsl:text>Лайт</xsl:text>
					</a>
					<xsl:text>&#160;|&#160;</xsl:text>
					<a>
						<xsl:attribute name="href"><xsl:value-of select="$prefix"/>445/Boards/</xsl:attribute>
						<xsl:text>Сырой XML</xsl:text>
					</a>
				</p>
				<p>
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
				</p>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>

