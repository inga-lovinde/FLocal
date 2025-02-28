<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="board">
		<tr>
			<td width="4%" class="darktable" align="center" valign="middle">
				<a>
					<xsl:attribute name="onClick">if(!confirm('�������� ��� ��������� ��� �����������?')) {event.returnValue=false; return false;} else { alert("Not implemented yet"); }</xsl:attribute>
					<xsl:choose>
						<xsl:when test="(hasNewPosts='true') and not(lastPostInfo/post/poster/user/id = /root/session/user/id)">
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
				<xsl:if test="(description != '') or (subBoards/board)">
					<table cellpadding="0" cellspacing="0">
						<xsl:if test="description != ''">
							<tr>
								<td class="forumdescript"><xsl:value-of select="description"/></td>
							</tr>
						</xsl:if>
						<xsl:if test="subBoards/board">
							<tr>
								<td class="forumdescript" style="padding-top:0.3em">
									<xsl:apply-templates select="subBoards/board"/>
								</td>
							</tr>
						</xsl:if>
					</table>
				</xsl:if>
			</td>
			<td width="7%" align="center" class="threadtotal" nowrap="nowrap"><xsl:value-of select="totalThreads"/></td>
			<td width="7%" align="center" class="posttotal" nowrap="nowrap"><xsl:value-of select="totalPosts"/></td>
			<td width="15%" nowrap="nowrap" class="posttime">
				<xsl:apply-templates select="lastPostInfo"/>
			</td>
			<td width="10%" class="modcolumn" align="center">
				<xsl:apply-templates select="administrator/user | moderators/user" mode="userLink"/>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="lastPostInfo[not(post/hidden)]">
		<xsl:if test="post">
			<xsl:apply-templates select="post/postDate/date" mode="dateTime"/><br />
			<a>
				<xsl:attribute name="href">/Thread/<xsl:value-of select="post/threadId"/>/p<xsl:value-of select="post/id"/>/</xsl:attribute>
				<xsl:text>�� </xsl:text>
				<xsl:value-of select="post/poster/user/name"/>
			</a>
		</xsl:if>
	</xsl:template>

	<xsl:template match="lastPostInfo[post/hidden]">
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