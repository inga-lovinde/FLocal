<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="message">
		<div style="clear:both">
			<xsl:attribute name="class">
				<xsl:text>pmcontainer</xsl:text>
				<xsl:if test="isReadByInterlocutor='false'">
					<xsl:text> unread</xsl:text>
				</xsl:if>
			</xsl:attribute>
			<a><xsl:attribute name="name">Message<xsl:value-of select="id"/></xsl:attribute></a>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="poster/account/user" mode="userInfoBar"/>
			</div>
			<div class="linkscontainer" style="float:right;text-align:right;">
				<a>
					<xsl:attribute name="href">/My/Conversations/PMReply/<xsl:value-of select="id"/>/</xsl:attribute>
					<xsl:attribute name="onclick">submitSelText(this.href);return false;</xsl:attribute>
					<xsl:attribute name="onmousedown">submitSelText(this.href);return false;</xsl:attribute>
					<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
						<xsl:with-param name="src">mail</xsl:with-param>
						<xsl:with-param name="alt">Ответ</xsl:with-param>
					</xsl:apply-templates>
				</a>
			</div>
			<div class="pmcontent">
				<xsl:variable name="postDate"><xsl:apply-templates select="postDate/date" mode="dateTime"/></xsl:variable>
				<a target="_blank" class="separate">
					<xsl:attribute name="title"><xsl:value-of select="$postDate"/></xsl:attribute>
					<img border="0" alt="" style="vertical-align: text-bottom" width="16" height="16">
						<xsl:choose>
							<xsl:when test="isRead='false'">
								<xsl:attribute name="src">/static/images/woocons/Mail-notread.png</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="src">/static/images/woocons/Mail.png</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</img>
				</a>
				<xsl:text> </xsl:text>
				<xsl:value-of select="title"/>
				<xsl:text>&#160;&#160;</xsl:text>
				<font style="font-size:0.6em">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$postDate"/>
					<xsl:text>)</xsl:text>
				</font>
				<div class="pmbody">
					<xsl:value-of select="body" disable-output-escaping="yes" />
				</div>
				<xsl:if test="poster/account/user/signature != ''">
					<div class="signature" style="float:right;">
						<xsl:value-of select="poster/account/user/signature" disable-output-escaping="yes" />
					</div>
				</xsl:if>
			</div>
			<div style="clear:both;" class="zerosize">
				<xsl:comment>fill</xsl:comment>
			</div>
		</div>
	</xsl:template>


</xsl:stylesheet>