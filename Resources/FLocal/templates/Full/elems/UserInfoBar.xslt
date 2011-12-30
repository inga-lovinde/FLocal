<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="user" mode="userInfoBar">
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td>
					<xsl:apply-templates select="current()" mode="userLink"/>
				</td>
			</tr>
			<tr>
				<td class="small">
					<b><xsl:value-of select="title"/></b>
				</td>
			</tr>
			<tr>
				<td class="small">
					<i></i> 
				</td>
			</tr>
			<tr>
				<td class="small">
					<i><font color="red"></font></i>
				</td>
			</tr>
			<xsl:if test="avatar">
				<tr>
					<td class="small">
						<img alt="" class="avatar">
							<xsl:attribute name="src">/Upload/Item/<xsl:value-of select="avatar"/>/</xsl:attribute>
						</img>
					</td>
				</tr>
			</xsl:if>
			<tr>
				<td class="small">
					<xsl:text>Рег.: </xsl:text>
					<xsl:apply-templates select="regDate/date" mode="date"/>
				</td>
			</tr>
			<tr>
				<td class="small">
					<xsl:text>Сообщений: </xsl:text>
					<xsl:value-of select="totalPosts"/>
				</td>
			</tr>
			<xsl:if test="location!=''">
				<tr>
					<td class="small">
						<xsl:text>Из: </xsl:text>
						<xsl:value-of select="location"/>
					</td>
				</tr>
			</xsl:if>
		</table>
	</xsl:template>
</xsl:stylesheet>