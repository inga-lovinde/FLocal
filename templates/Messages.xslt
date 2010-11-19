<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:variable name="lang">ru</xsl:variable>

	<xsl:template name="Messages_AllPosts"><xsl:choose>
		<xsl:when test="$lang='ru'">Все сообщения</xsl:when>
		<xsl:otherwise>All posts</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AllThreads"><xsl:choose>
		<xsl:when test="$lang='ru'">Все темы</xsl:when>
		<xsl:otherwise>All threads</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_PageOuterTitle"><xsl:choose>
		<xsl:when test="$lang='ru'">страницы</xsl:when>
		<xsl:otherwise>pages</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_AvatarsSettings"><xsl:choose>
		<xsl:when test="$lang='ru'">Настройка аватарок</xsl:when>
		<xsl:otherwise>Avatars settings</xsl:otherwise>
	</xsl:choose></xsl:template>

	<xsl:template name="Messages_CurrentAvatars"><xsl:choose>
		<xsl:when test="$lang='ru'">Загруженные аватарки</xsl:when>
		<xsl:otherwise>Current avatars</xsl:otherwise>
	</xsl:choose></xsl:template>


</xsl:stylesheet>

