<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css" type="text/css" />
				<link rel="stylesheet" href="/static/css/coffeehaus.css" type="text/css" />
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<title><xsl:value-of select="title"/></title>
			</head>
			<body>
				<xsl:call-template name="header"/>
				<xsl:call-template name="specific"/>
				<br />
<span>Data used for authoring this XHTML document:</span>
<xmp><xsl:copy-of select="/"/></xmp>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="parent" mode="breadcrumbsPart">
		<xsl:apply-templates select="*/parent" mode="breadcrumbsPart"/>
		<xsl:if test="category/id">
			<a>
				<xsl:attribute name="href">/Boards/</xsl:attribute>
				<xsl:value-of select="category/name"/>
			</a>
			<span> &gt;&gt; </span>
		</xsl:if>
		<xsl:if test="board/id">
			<a>
				<xsl:attribute name="href">/Board/<xsl:value-of select="board/id"/>/</xsl:attribute>
				<xsl:value-of select="board/name"/>
			</a>
			<span> &gt;&gt; </span>
		</xsl:if>
	</xsl:template>

	<xsl:template match="currentBoard" mode="breadcrumbs">
		<xsl:apply-templates select="*/parent" mode="breadcrumbsPart"/>
		<xsl:value-of select="board/name"/>
	</xsl:template>

</xsl:stylesheet>

