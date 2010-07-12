<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="no" encoding="UCS-2"/>
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css" type="text/css" />
				<link rel="stylesheet" type="text/css">
					<xsl:attribute name="href">/static/css/<xsl:value-of select="skin/name"/>.css</xsl:attribute>
				</link>
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<script language="Javascript" type="text/javascript" src="/static/js/common.js"><xsl:text> </xsl:text></script>
				<title>
					<xsl:call-template name="specificTitle"/>
					<xsl:text> - </xsl:text>
					<xsl:value-of select="title"/>
				</title>
			</head>
			<body>
				<xsl:call-template name="header"/>
				<xsl:call-template name="specific"/>
				<div style="display:none">
					<form action="" method="POST" id="systemForm">
						<input type="hidden" name="data"/>
					</form>
				</div>
				<xsl:text disable-output-escaping="yes"><![CDATA[<!--]]></xsl:text>
				<br />
				<xsl:text>Data used for authoring this XHTML document:</xsl:text>
				<xmp><xsl:copy-of select="/"/></xmp>
				<xsl:text disable-output-escaping="yes"><![CDATA[-->]]></xsl:text>
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
			<xsl:text> &gt;&gt; </xsl:text>
		</xsl:if>
		<xsl:if test="board/id">
			<a>
				<xsl:attribute name="href">/Board/<xsl:value-of select="board/id"/>/</xsl:attribute>
				<xsl:value-of select="board/name"/>
			</a>
			<xsl:text> &gt;&gt; </xsl:text>
		</xsl:if>
		<xsl:if test="thread/id">
			<a>
				<xsl:attribute name="href">/Thread/<xsl:value-of select="thread/id"/>/</xsl:attribute>
				<xsl:value-of select="thread/name"/>
			</a>
			<xsl:text> &gt;&gt; </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="currentLocation" mode="breadcrumbs">
		<xsl:apply-templates select="*/parent" mode="breadcrumbsPart"/>
		<xsl:value-of select="*/name"/>
	</xsl:template>

	<xsl:template match="date" mode="dateTime">
		<span nowrap="nowrap">
			<xsl:value-of select="format-number(mday, '00')"/>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="format-number(month, '00')"/>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="year"/>
			<xsl:text> </xsl:text>
			<xsl:value-of select="format-number(hour, '00')"/>
			<xsl:text>:</xsl:text>
			<xsl:value-of select="format-number(minute, '00')"/>
			<xsl:text>:</xsl:text>
			<xsl:value-of select="format-number(second, '00')"/>
		</span>
	</xsl:template>

	<xsl:template match="date" mode="date">
		<span nowrap="nowrap">
			<xsl:value-of select="format-number(mday, '00')"/>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="format-number(month, '00')"/>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="year"/>
		</span>
	</xsl:template>

	<xsl:template match="pageOuter/pages/page" mode="withoutCurrent">
		<xsl:param name="baseLink"/>
		<xsl:param name="selected">-1</xsl:param>
		<xsl:if test="current() != '0'">
			<xsl:text>&#8201;|&#8201;</xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="current() != $selected">
				<a>
					<xsl:attribute name="href"><xsl:value-of select="$baseLink"/><xsl:value-of select="current()"/></xsl:attribute>
					<xsl:value-of select="current()"/>
				</a>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="current()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="pageOuter" mode="withoutCurrent">
		<xsl:param name="baseLink"/>
		<xsl:apply-templates select="pages/page" mode="withoutCurrent">
			<xsl:with-param name="baseLink"><xsl:value-of select="$baseLink"/></xsl:with-param>
			<xsl:with-param name="selected">-1</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="pageOuter" mode="withCurrent">
		<xsl:param name="baseLink"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="pages/page" mode="withoutCurrent">
			<xsl:with-param name="baseLink"><xsl:value-of select="$baseLink"/></xsl:with-param>
			<xsl:with-param name="selected">
				<xsl:choose>
					<xsl:when test="unlimited='false'">
						<xsl:value-of select="start"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>-1</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:text>&#8201;|&#8201;</xsl:text>
		<xsl:choose>
			<xsl:when test="unlimited='false'">
				<a>
					<xsl:attribute name="href"><xsl:value-of select="$baseLink"/>all</xsl:attribute>
					<xsl:text>���</xsl:text>
				</a>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>���</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="next">
			<xsl:text>&#8201;|&#8201;</xsl:text>
			<a>
				<xsl:attribute name="href"><xsl:value-of select="$baseLink"/><xsl:value-of select="next"/></xsl:attribute>
				<xsl:text>��������� ��������</xsl:text>
			</a>
		</xsl:if>
	</xsl:template>

	<xsl:template match="user" mode="userLink">
		<a class="separate">
			<xsl:attribute name="href">/User/<xsl:value-of select="id"/>/</xsl:attribute>
			<xsl:value-of select="name"/>
		</a>
	</xsl:template>

</xsl:stylesheet>

