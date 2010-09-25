<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="user" mode="userInfoBar">
		<div class="userbar">
			<xsl:attribute name="onmouseover">showChildren(this);</xsl:attribute>
			<xsl:attribute name="onmouseout">hideChildren(this);</xsl:attribute>
			<div class="userbar_avatar" default="default">
				<a>
					<xsl:attribute name="href">/Users/User/<xsl:value-of select="id"/>/Info/</xsl:attribute>
					<xsl:choose>
						<xsl:when test="avatar">
							<img alt="" class="avatar">
								<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="avatar"/>/</xsl:attribute>
							</img>
						</xsl:when>
						<xsl:otherwise>
							<div class="userbar_name">
								<xsl:apply-templates select="current()" mode="userLink"/>
							</div>
						</xsl:otherwise>
					</xsl:choose>
				</a>
			</div>
			<div class="userbar_additionalcontainer" style="display:none">
				<div class="userbar_additional">
					<div class="userbar_name">
						<xsl:apply-templates select="current()" mode="userLink"/>
					</div>
					<div class="userbar_title">
						<xsl:value-of select="title"/>
					</div>
					<div class="userbar_regdate">
						<xsl:text>Рег.: </xsl:text>
						<xsl:apply-templates select="regDate/date" mode="date"/>
					</div>
					<div class="userbar_totalposts">
						<xsl:text>Сообщений: </xsl:text>
						<xsl:value-of select="totalPosts"/>
					</div>
					<xsl:if test="location!=''">
						<div class="userbar_location">
							<xsl:text>Из: </xsl:text>
							<xsl:value-of select="location"/>
						</div>
					</xsl:if>
				</div>
			</div>
		</div>
	</xsl:template>
</xsl:stylesheet>