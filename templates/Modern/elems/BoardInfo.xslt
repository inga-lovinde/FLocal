<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="board">
		<xsl:param name="maxPerLine">100</xsl:param>
		<xsl:if test="((position() mod $maxPerLine) = 1) and (position() != 1)">
			<br/>
		</xsl:if>
		<li>
			<xsl:attribute name="class">
				<xsl:text>boardcontainer</xsl:text>
				<xsl:if test="(hasNewPosts='true') and not(lastPostInfo/post/poster/user/id = /root/session/user/id)">
					<xsl:text> updated</xsl:text>
				</xsl:if>
			</xsl:attribute>
			<a>
				<xsl:attribute name="href">/Forum/Board/<xsl:value-of select="id"/>/Threads/</xsl:attribute>
				<h3 class="boardcontainer_name">
					<xsl:value-of select="name"/>
				</h3>
				<xsl:if test="description != ''">
					<p class="boardcontainer_description">
						<xsl:attribute name="href">/Forum/Board/<xsl:value-of select="id"/>/Threads/</xsl:attribute>
						<xsl:value-of select="description"/>
					</p>
				</xsl:if>
			</a>
			<p>���: <xsl:value-of select="totalThreads"/></p>
			<p>���������: <xsl:value-of select="totalPosts"/></p>
			<xsl:if test="subBoards/board">
				<div style="float:left">
					<xsl:attribute name="onMouseOver">showChildren(this);</xsl:attribute>
					<xsl:attribute name="onMouseOut">hideChildren(this);</xsl:attribute>
					<div default="default">
						<a class="pseudolink">����������</a>
					</div>
					<div class="board_additionalcontainer" style="display:none">
						<div class="board_additional">
							<p>
								<xsl:apply-templates select="subBoards/board"/>
							</p>
						</div>
					</div>
				</div>
			</xsl:if>
			<div style="float:right;">
				<xsl:attribute name="onMouseOver">showChildren(this);</xsl:attribute>
				<xsl:attribute name="onMouseOut">hideChildren(this);</xsl:attribute>
				<div default="default">
					<a class="pseudolink">����������</a>
				</div>
				<div class="board_additionalcontainer" style="display:none;">
					<div class="board_additional">
						<xsl:apply-templates select="lastPostInfo"/>
						<p>
							<xsl:text>����������:</xsl:text>
							<br/>
							<xsl:apply-templates select="administrator/user | moderators/user" mode="userLink"/>
						</p>
					</div>
				</div>
			</div>
		</li>
	</xsl:template>

	<xsl:template match="lastPostInfo">
		<xsl:if test="post">
			<p>
				<xsl:text>��������� ���������:</xsl:text>
				<br/>
				<a>
					<xsl:attribute name="href">/Thread/<xsl:value-of select="post/threadId"/>/p<xsl:value-of select="post/id"/>/</xsl:attribute>
					<xsl:apply-templates select="post/postDate/date" mode="dateTime"/>
					<xsl:text> �� </xsl:text>
					<xsl:value-of select="post/poster/user/name"/>
				</a>
			</p>
		</xsl:if>
	</xsl:template>

	<xsl:template match="subBoards/board">
		<xsl:if test="position() &gt; 1">
			<hr/>
		</xsl:if>
		<a>
			<xsl:attribute name="href">/Board/<xsl:value-of select="id"/>/</xsl:attribute>
			<xsl:value-of select="name"/>
			<xsl:if test="description != ''">
				<br/>
				<xsl:value-of select="description"/>
			</xsl:if>
		</a>
	</xsl:template>
</xsl:stylesheet>