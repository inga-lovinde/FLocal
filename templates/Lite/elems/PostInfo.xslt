<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="post">
		<xsl:param name="isReplyDisabled">true</xsl:param>
		<div>
			<div class="darktable">
				<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
				<b><xsl:apply-templates select="poster/user" mode="userLink"/></b>
				<xsl:text> : </xsl:text>
				<xsl:if test="layerName='offtop'">{o}</xsl:if>
				<xsl:if test="layerName='garbage'">{g}</xsl:if>
				<xsl:choose>
					<xsl:when test="isUnread='true'">
						<xsl:text>(*)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
				<!--b><xsl:value-of select="title"/></b-->
				<xsl:if test="parentPost/post">
					<font class="small separate">
						<xsl:text>[</xsl:text>
						<a>
							<xsl:attribute name="href">/Post/<xsl:value-of select="parentPost/post/id"/>/</xsl:attribute>
							<xsl:text>re: </xsl:text>
							<xsl:value-of select="parentPost/post/poster/user/name"/>
						</a>
						<xsl:text>]</xsl:text>
					</font>
				</xsl:if>
				<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
				<span style="padding-left:1em">
					<xsl:text> | </xsl:text>
					<a>
						<xsl:if test="$isReplyDisabled='false'">
							<xsl:if test="/root/session/sessionKey">
								<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Reply/</xsl:attribute>
								<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
								<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
							</xsl:if>
						</xsl:if>
						<xsl:text>Reply</xsl:text>
					</a>
					<xsl:text> | </xsl:text>
					<a>
						<xsl:if test="isOwner='true'">
							<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Edit/</xsl:attribute>
						</xsl:if>
						<xsl:text>Edit</xsl:text>
					</a>
					<xsl:text> | </xsl:text>
				</span>
			</div>
			<div class="lighttable post">
				<xsl:value-of select="body" disable-output-escaping="yes" />
				<br/>
				<br/>
			</div>
		</div>
	</xsl:template>


</xsl:stylesheet>