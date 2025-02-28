<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="no" />
	<xsl:template match="/root">
		<html>
			<head>
				<link rel="stylesheet" href="/static/css/global.css?v1" type="text/css" />
				<link rel="stylesheet" type="text/css" skin="skin">
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
				<xsl:if test="not(get/param[@name='headers'] = 'false')">
					<xsl:call-template name="header"/>
				</xsl:if>
				<xsl:call-template name="specific"/>
				<div style="display:none">
					<form action="" method="POST" id="systemForm">
						<input type="hidden" name="data"/>
					</form>
				</div>
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
				<xsl:attribute name="href">
					<xsl:text>/Thread/</xsl:text>
					<xsl:value-of select="thread/id"/>
					<xsl:if test="parent::post/id">
						<xsl:text>/p</xsl:text>
						<xsl:value-of select="parent::post/id"/>
					</xsl:if>
				</xsl:attribute>
				<xsl:value-of select="thread/name"/>
			</a>
			<xsl:text> &gt;&gt; </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="currentLocation" mode="breadcrumbs">
		<xsl:apply-templates select="*/parent" mode="breadcrumbsPart"/>
		<xsl:value-of select="*/name"/>
	</xsl:template>

	<xsl:template match="date" mode="navigationImageFor">
		<xsl:param name="alt"/>
		<xsl:param name="src"/>
		<img border="0" style="vertical-align: text-bottom">
			<xsl:attribute name="src">
				<xsl:text>/static/images/</xsl:text>
				<xsl:choose>
					<xsl:when test="year = /root/current/date/year">
						<xsl:value-of select="$src"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>slowpoke.png</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="alt">
				<xsl:value-of select="$alt"/>
			</xsl:attribute>
			<xsl:attribute name="title">
				<xsl:value-of select="$alt"/>
			</xsl:attribute>
		</img>
	</xsl:template>

	<xsl:template match="date" mode="_date">
		<xsl:choose>
			<xsl:when test="year=/root/current/date/year and month=/root/current/date/month and mday=/root/current/date/mday">
				<xsl:text>�������</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="year=/root/current/date/year and month=/root/current/date/month and (mday+1)=/root/current/date/mday">
						<xsl:text>�����</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number(mday, '00')"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="format-number(month, '00')"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="year"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="date" mode="dateTime">
		<span nowrap="nowrap">
			<xsl:apply-templates select="current()" mode="_date"/>
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
			<xsl:apply-templates select="current()" mode="_date"/>
		</span>
	</xsl:template>

	<xsl:template match="timeSpan">
		<span nowrap="nowrap">
			<xsl:if test="days &gt; 0">
				<xsl:value-of select="days"/>
				<xsl:text> ���� </xsl:text>
			</xsl:if>
			<xsl:if test="hours &gt; 0">
				<xsl:value-of select="hours"/>
				<xsl:text> ����� </xsl:text>
			</xsl:if>
			<xsl:if test="minutes &gt; 0">
				<xsl:value-of select="minutes"/>
				<xsl:text> ����� </xsl:text>
			</xsl:if>
			<xsl:if test="seconds &gt; 0">
				<xsl:value-of select="seconds"/>
				<xsl:text> ������ </xsl:text>
			</xsl:if>
		</span>
	</xsl:template>

	<xsl:template match="pageOuter/pages/page" mode="withoutCurrent">
		<xsl:param name="baseLink"/>
		<xsl:param name="postfix"/>
		<xsl:param name="selected">-1</xsl:param>
		<xsl:if test="current() != '0'">
			<xsl:text>&#8201;|&#8201;</xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="current() != $selected">
				<a>
					<xsl:attribute name="href">
						<xsl:value-of select="$baseLink"/>
						<xsl:value-of select="current()"/>
						<xsl:value-of select="$postfix"/>
					</xsl:attribute>
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
			<xsl:with-param name="postfix"/>
			<xsl:with-param name="selected">-1</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="pageOuter" mode="withCurrent">
		<xsl:param name="baseLink"/>
		<xsl:variable name="postfix">
			<xsl:if test="isReversed='true'">-reversed</xsl:if>
		</xsl:variable>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="pages/page" mode="withoutCurrent">
			<xsl:with-param name="baseLink"><xsl:value-of select="$baseLink"/></xsl:with-param>
			<xsl:with-param name="postfix">
				<xsl:value-of select="$postfix"/>
			</xsl:with-param>
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
		<xsl:if test="total &lt;= 1000">
			<xsl:text>&#8201;|&#8201;</xsl:text>
			<xsl:choose>
				<xsl:when test="unlimited='false'">
					<a>
						<xsl:attribute name="href">
							<xsl:value-of select="$baseLink"/>
							<xsl:text>all</xsl:text>
							<xsl:value-of select="$postfix"/>
						</xsl:attribute>
						<xsl:text>���</xsl:text>
					</a>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>���</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="next">
			<xsl:text>&#8201;|&#8201;</xsl:text>
			<a rel="next">
				<xsl:attribute name="href">
					<xsl:value-of select="$baseLink"/>
					<xsl:value-of select="next"/>
					<xsl:value-of select="$postfix"/>
				</xsl:attribute>
				<xsl:text>��������� ��������</xsl:text>
			</a>
		</xsl:if>
	</xsl:template>

	<xsl:template match="user" mode="userLink">
		<xsl:if test="position() &gt; 1">
			<xsl:text> </xsl:text>
		</xsl:if>
		<a nobr="nobr">
			<xsl:attribute name="class">
				<xsl:text>separate </xsl:text>
				<xsl:text>UG_</xsl:text><xsl:value-of select="group/name"/>
			</xsl:attribute>
			<xsl:attribute name="href">/Users/User/<xsl:value-of select="id"/>/Info/</xsl:attribute>
			<xsl:value-of select="name"/>
			<xsl:if test="group/name='Judges'">
				<img src="/static/images/judge.gif" border="0"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="isAdministrator='true'">
					<img src="/static/images/adm.gif" border="0"/>
				</xsl:when>
				<xsl:when test="isModerator='true'">
					<img src="/static/images/mod.gif" border="0"/>
				</xsl:when>
			</xsl:choose>
		</a>
	</xsl:template>

</xsl:stylesheet>

