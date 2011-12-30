<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="BoardHeader.xslt"/>

	<xsl:template name="forumHeader">
		<div class="header">
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Forum/Boards/</xsl:with-param>
				<xsl:with-param name="text">Разделы</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Forum/AllThreads/</xsl:with-param>
				<xsl:with-param name="text">Темы</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Forum/AllPosts/</xsl:with-param>
				<xsl:with-param name="text">Сообщения</xsl:with-param>
			</xsl:call-template>
			<xsl:text>&#160;&#160;&#160;</xsl:text>
			<xsl:call-template name="headerLink">
				<xsl:with-param name="url">/Users/User/<xsl:value-of select="session/user/id"/>/Replies/</xsl:with-param>
				<xsl:with-param name="text">Ответы</xsl:with-param>
				<xsl:with-param name="isDisabled">
					<xsl:if test="not(session/sessionKey)">true</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="currentLocation//board[not(name(../..)='board')]">
				<xsl:text>&#160;&#160;&#160;</xsl:text>
				<xsl:apply-templates select="currentLocation//board[not(name(../..)='board')]" mode="headerBoardLink"/>
			</xsl:if>
			<xsl:if test="currentLocation//board">
				<xsl:call-template name="boardHeader"/>
			</xsl:if>
		</div>
	</xsl:template>

	<xsl:template match="board" mode="headerParentBoardLink">
		<xsl:apply-templates select="parent/board" mode="headerParentBoardLink"/>
		<xsl:call-template name="headerLink">
			<xsl:with-param name="url">/Forum/Board/<xsl:value-of select="id"/>-<xsl:value-of select="nameTranslit"/>/</xsl:with-param>
			<xsl:with-param name="text"><xsl:value-of select="substring(name, 1, 1)"/></xsl:with-param>
		</xsl:call-template>
		<xsl:text> &#8594; </xsl:text>
	</xsl:template>

	<xsl:template match="board" mode="headerBoardLink">
		<xsl:apply-templates select="parent/board" mode="headerParentBoardLink"/>
		<xsl:call-template name="headerLink">
			<xsl:with-param name="url">/Forum/Board/<xsl:value-of select="id"/>-<xsl:value-of select="nameTranslit"/>/</xsl:with-param>
			<xsl:with-param name="text"><xsl:value-of select="name"/></xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>

