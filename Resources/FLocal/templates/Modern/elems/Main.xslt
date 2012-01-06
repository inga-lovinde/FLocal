<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="../../Messages.xslt"/>
	<xsl:import href="Header.xslt"/>
	<xsl:output method="xml" indent="no" />
	<xsl:template match="/root">
		<xsl:text disable-output-escaping="yes">&lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"&gt;</xsl:text>
		<html>
			<head>
				<meta http-equiv="x-ua-compatible" content="IE=8"/>
				<link rel="stylesheet" href="/static/css/modern/global.css?v6.5" type="text/css" />
				<xsl:comment>[if lte IE 7]&gt;<![CDATA[
					<link rel="stylesheet" href="/static/css/modern/iefixes.css?v6.5" type="text/css" />
				]]>&lt;![endif]</xsl:comment>
				<link rel="stylesheet" type="text/css" skin="skin">
					<xsl:attribute name="href">/static/css/modern/<xsl:value-of select="modernSkin/name"/>.css?v6.5</xsl:attribute>
				</link>
				<link rel="shortcut icon" href="/static/favicons/smirk.ico" type="image/x-icon" />
				<script language="Javascript" type="text/javascript" src="/static/js/common.js?v6.5"><xsl:text> </xsl:text></script>
				<script language="Javascript" type="text/javascript" src="/static/js/modern.js?v6.5"><xsl:text> </xsl:text></script>
				<script language="Javascript" type="text/javascript" src="/static/js/machichara/complex.js?v1"><xsl:text> </xsl:text></script>
				<script language="Javascript" type="text/javascript" src="/static/js/machichara/simple.js?v6.5"><xsl:text> </xsl:text></script>
				<script language="Javascript" type="text/javascript">
					<xsl:attribute name="src">
						<xsl:text>/static/js/machichara/</xsl:text>
						<xsl:value-of select="machichara/name"/>
						<xsl:text>-specific.js</xsl:text>
					</xsl:attribute>
					<xsl:text> </xsl:text>
				</script>
				<style>
					<xsl:text>.uploadImage { max-width:</xsl:text>
					<xsl:value-of select="uploadSettings/maxWidth"/>
					<xsl:text>px; max-height:</xsl:text>
					<xsl:value-of select="uploadSettings/maxHeight"/>
					<xsl:text>px; }</xsl:text>
				</style>
				<title>
					<xsl:call-template name="specificTitle"/>
					<xsl:text> - </xsl:text>
					<xsl:value-of select="title"/>
				</title>
			</head>
			<body>
				<xsl:attribute name="onload">onLoad();</xsl:attribute>
				<xsl:if test="not(get/param[@name='headers'] = 'false')">
					<div id="headerContainer">
						<xsl:call-template name="header"/>
					</div>
				</xsl:if>
				<div style="float:left;width:97%;max-width:97%;overflow:visible;" id="contentContainer">
					<xsl:call-template name="specific"/>
				</div>
				<xsl:if test="not(get/param[@name='headers'] = 'false')">
					<div style="min-width:20px;width:20px;max-width:20px;overflow:visible;position:absolute;top:4em;right:0px;text-align:right;z-index:4000" id="bjAndSlutsContainer">
						<div style="min-width:20px;width:20px;max-width:20px;overflow:visible;text-align:right;height:260px;min-height:260px;max-height:260px;">
							<div style="position:absolute;top:0px;right:20px;display:none" id="pokerPlaceholder">
								<img border="0" src="/static/images/poker.jpg" alt="poker" align="top"/>
							</div>
							<div style="min-width:20px;width:20px;max-width:20px;text-align:right;">
								<xsl:attribute name="onmouseover">document.getElementById("pokerPlaceholder").style.display = "block";</xsl:attribute>
								<xsl:attribute name="onmouseout">document.getElementById("pokerPlaceholder").style.display = "none";</xsl:attribute>
								<img border="0" src="/static/images/tab_poker.png" alt="tab_poker" align="top"/>
							</div>
						</div>
						<br/>
						<div style="min-width:20px;width:20px;max-width:20px;overflow:visible;text-align:right;height:260px;min-height:260px;max-height:260px;">
							<div style="position:absolute;top:0px;right:20px;display:none" id="courtesansPlaceholder">
								<img border="0" src="/static/images/courtesans.jpg" alt="courtesans" align="top"/>
							</div>
							<div style="min-width:20px;width:20px;max-width:20px;text-align:right;">
								<xsl:attribute name="onmouseover">document.getElementById("courtesansPlaceholder").style.display = "block";</xsl:attribute>
								<xsl:attribute name="onmouseout">document.getElementById("courtesansPlaceholder").style.display = "none";</xsl:attribute>
								<img border="0" src="/static/images/tab_courtesans.png" alt="tab_courtesans" align="top"/>
							</div>
						</div>
					</div>
				</xsl:if>
				<div style="display:none">
					<form action="" method="post" id="systemForm">
						<input type="hidden" name="data"/>
					</form>
					<span>Total time spent: <xsl:value-of select="processingTime"/> seconds</span>
				</div>
				<xsl:if test="get/param[@name='debug'] = 'true'">
					<xsl:text disable-output-escaping="yes"><![CDATA[<!--]]></xsl:text>
					<br />
					<xsl:text>Data used for authoring this XHTML document:</xsl:text>
					<xmp><xsl:copy-of select="/"/></xmp>
					<xsl:text disable-output-escaping="yes"><![CDATA[-->]]></xsl:text>
				</xsl:if>
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
			<xsl:attribute name="class">
				<xsl:text>metroicon</xsl:text>
				<xsl:text> </xsl:text>
				<xsl:text>metroicon-</xsl:text>
				<xsl:choose>
					<xsl:when test="year &gt; 2009">
						<xsl:value-of select="$src"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>slowpoke</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="src">
				<xsl:text>/static/images/metro.icons/placeholder.png</xsl:text>
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
				<xsl:text>сегодня</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="year=/root/current/date/year and month=/root/current/date/month and (mday+1)=/root/current/date/mday">
						<xsl:text>вчера</xsl:text>
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
				<xsl:text> дней </xsl:text>
			</xsl:if>
			<xsl:if test="hours &gt; 0">
				<xsl:value-of select="hours"/>
				<xsl:text> часов </xsl:text>
			</xsl:if>
			<xsl:if test="minutes &gt; 0">
				<xsl:value-of select="minutes"/>
				<xsl:text> минут </xsl:text>
			</xsl:if>
			<xsl:if test="seconds &gt; 0">
				<xsl:value-of select="seconds"/>
				<xsl:text> секунд </xsl:text>
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
				<a class="current">
					<xsl:value-of select="current()"/>
				</a>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="pageOuter" mode="withoutCurrent">
		<xsl:param name="baseLink"/>
		<xsl:if test="isEmpty = 'false'">
			<p>
				<xsl:text>Страницы: </xsl:text>
				<xsl:apply-templates select="pages/page" mode="withoutCurrent">
					<xsl:with-param name="baseLink"><xsl:value-of select="$baseLink"/></xsl:with-param>
					<xsl:with-param name="postfix"/>
					<xsl:with-param name="selected">-1</xsl:with-param>
				</xsl:apply-templates>
			</p>
		</xsl:if>
	</xsl:template>

	<xsl:template match="pageOuter" mode="withCurrent">
		<xsl:param name="baseLink"/>
		<xsl:variable name="postfix">
			<xsl:if test="isReversed='true'">-reversed</xsl:if>
		</xsl:variable>
		<xsl:if test="total &gt; 0">
			<div>
				<xsl:call-template name="Messages_PageOuterTitle"/>
				<xsl:text>: </xsl:text>
				<xsl:apply-templates select="pages/page" mode="withoutCurrent">
					<xsl:with-param name="baseLink"><xsl:value-of select="$baseLink"/></xsl:with-param>
					<xsl:with-param name="postfix" select="$postfix"/>
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
				<xsl:if test="(total &lt;= 1000) and isEmpty = 'false'">
					<xsl:text>&#8201;|&#8201;</xsl:text>
					<xsl:choose>
						<xsl:when test="unlimited='false'">
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="$baseLink"/>
									<xsl:text>all</xsl:text>
									<xsl:value-of select="$postfix"/>
								</xsl:attribute>
								<xsl:text>все</xsl:text>
							</a>
						</xsl:when>
						<xsl:otherwise>
							<a class="current">все</a>
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
						<xsl:text>Следующая страница</xsl:text>
					</a>
				</xsl:if>
			</div>
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
				<img src="/static/images/judge.gif" alt="Judge" border="0"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="isAdministrator='true'">
					<img src="/static/images/adm.gif" alt="Administrator" border="0"/>
				</xsl:when>
				<xsl:when test="isModerator='true'">
					<img src="/static/images/mod.gif" alt="Moderator" border="0"/>
				</xsl:when>
			</xsl:choose>
		</a>
	</xsl:template>

	<xsl:template name="log2">
		<xsl:param name="number"/>
		<xsl:choose>
			<xsl:when test="not($number) or $number=''"/>
			<xsl:when test="$number &lt; 1">-1</xsl:when>
			<xsl:when test="$number &lt; 2">0</xsl:when>
			<xsl:when test="$number &lt; 4">1</xsl:when>
			<xsl:when test="$number &lt; 8">2</xsl:when>
			<xsl:when test="$number &lt; 16">3</xsl:when>
			<xsl:when test="$number &lt; 32">4</xsl:when>
			<xsl:when test="$number &lt; 64">5</xsl:when>
			<xsl:when test="$number &lt; 128">6</xsl:when>
			<xsl:when test="$number &lt; 256">7</xsl:when>
			<xsl:when test="$number &lt; 512">8</xsl:when>
			<xsl:when test="$number &lt; 1024">9</xsl:when>
			<xsl:when test="$number &lt; 2048">10</xsl:when>
			<xsl:when test="$number &lt; 4096">11</xsl:when>
			<xsl:when test="$number &lt; 8192">12</xsl:when>
			<xsl:when test="$number &lt; 16384">13</xsl:when>
			<xsl:when test="$number &lt; 32768">14</xsl:when>
			<xsl:when test="$number &lt; 65536">15</xsl:when>
			<xsl:when test="$number &lt; 131072">16</xsl:when>
			<xsl:when test="$number &lt; 262144">17</xsl:when>
			<xsl:when test="$number &lt; 524288">18</xsl:when>
			<xsl:when test="$number &lt; 1048576">19</xsl:when>
			<xsl:otherwise>9000</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>

