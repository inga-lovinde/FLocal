<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="user" mode="userInfoBar">
		<div class="user">
			<xsl:attribute name="onMouseOver">for(var i=0;i&lt;this.childNodes.length;i++) if(this.childNodes[i].nodeName == "DIV") this.childNodes[i].style.display="block";</xsl:attribute>
			<xsl:attribute name="onMouseOut">for(var i=0;i&lt;this.childNodes.length;i++) if(this.childNodes[i].nodeName == "DIV" &amp;&amp; this.childNodes[i].getAttribute("default") != "default") this.childNodes[i].style.display="none";</xsl:attribute>
			<div class="userbar_name">
				<xsl:if test="not(avatar)">
					<xsl:attribute name="default">default</xsl:attribute>
				</xsl:if>
				<xsl:if test="avatar">
					<xsl:attribute name="style">display:none</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="current()" mode="userLink"/>
			</div>
			<div class="userbar_title" style="display:none">
				<xsl:value-of select="title"/>
			</div>
			<xsl:if test="avatar">
				<div class="userbar_avatar" default="default">
					<a>
						<xsl:attribute name="href">/Users/User/<xsl:value-of select="id"/>/Info/</xsl:attribute>
						<img alt="" class="avatar">
							<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="avatar"/>/</xsl:attribute>
						</img>
					</a>
				</div>
			</xsl:if>
			<div class="userbar_regdate" style="display:none">
				<xsl:text>Рег.: </xsl:text>
				<xsl:apply-templates select="regDate/date" mode="date"/>
			</div>
			<div class="userbar_totalposts" style="display:none">
				<xsl:text>Сообщений: </xsl:text>
				<xsl:value-of select="totalPosts"/>
			</div>
			<xsl:if test="location!=''">
				<div class="userbar_location" style="display:none">
					<xsl:text>Из: </xsl:text>
					<xsl:value-of select="location"/>
				</div>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>