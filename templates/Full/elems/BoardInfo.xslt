<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="board">
		<tr>
			<td width="4%" class="darktable" align="center" valign="middle">
				<a>
					<xsl:attribute name="onClick">if(!confirm('Пометить все сообщения как прочитанные?')) {event.returnValue=false; return false;} else { alert("Not implemented yet"); }</xsl:attribute>
					<xsl:choose>
						<xsl:when test="hasNewPosts='true'">
							<img border="0" width="17" height="21" src="/static/images/newposts.gif" alt=""/>
						</xsl:when>
						<xsl:otherwise>
							<img border="0" width="17" height="21" src="/static/images/nonewposts.gif" alt=""/>
						</xsl:otherwise>
					</xsl:choose>
				</a>
			</td>
			<td width="57%" class="darktable">
				<font class="forumtitle">
					<a>
						<xsl:attribute name="href">/Board/<xsl:value-of select="id"/>/</xsl:attribute>
						<xsl:value-of select="name"/>
					</a>
				</font>
				<br />
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td class="forumdescript" style="padding-right:0.5em">
							<a>
								<xsl:attribute name="href">/Board/<xsl:value-of select="id"/>/</xsl:attribute>
								<xsl:text>A</xsl:text>
							</a>
						</td>
						<td class="forumdescript"><xsl:value-of select="description"/></td>
					</tr>
					<xsl:if test="subBoards/board">
						<tr>
							<td class="forumdescript">&#160;</td>
							<td class="forumdescript" style="padding-top:0.3em">
								<xsl:apply-templates select="subBoards/board"/>
							</td>
						</tr>
					</xsl:if>
				</table>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap"><xsl:value-of select="totalThreads"/></td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap"><xsl:value-of select="totalPosts"/></td>
			<td width="15%" nowrap="nowrap" class="posttime">
				<xsl:apply-templates select="lastPostInfo"/>
			</td>
			<td width="10%" class="modcolumn" align="center">
				<a>Sash</a>
				<xsl:text>, </xsl:text>
				<a>DeadmoroZ</a>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="lastPostInfo">
		<xsl:choose>
			<xsl:when test="post">
				<xsl:apply-templates select="post/postDate/date" mode="dateTime"/><br />
				<a>
					<xsl:attribute name="href">/Thread/<xsl:value-of select="post/threadId"/>/p<xsl:value-of select="post/id"/>/</xsl:attribute>
					<xsl:text>от </xsl:text>
					<xsl:value-of select="post/poster/user/name"/>
				</a>
				<xsl:text> </xsl:text>
				<img alt="new" src="/static/images/new.gif">
					<xsl:if test="post/id &lt; 10000000">
						<xsl:attribute name="style">visibility:hidden</xsl:attribute>
					</xsl:if>
				</img>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>N/A</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="subBoards/board">
		<xsl:text> [</xsl:text>
		<a>
			<xsl:attribute name="href">/Board/<xsl:value-of select="id"/>/</xsl:attribute>
			<xsl:value-of select="name"/>
		</a>
		<xsl:text>]</xsl:text>
	</xsl:template>
</xsl:stylesheet>