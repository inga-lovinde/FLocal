<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="conversation">
		<item>
			<title>
				<xsl:text>Новое сообщение в общении с пользователем </xsl:text>
				<xsl:value-of select="interlocutor/account/user/name"/>
			</title>
			<description>
				<xsl:text>Всего в обсуждении </xsl:text>
				<xsl:value-of select="totalMessages"/>
				<xsl:text> сообщений</xsl:text>
				<xsl:if test="totalNewMessages &gt; 0">
					<xsl:text>, из них </xsl:text>
					<xsl:value-of select="totalNewMessages"/>
					<xsl:text> непрочитанных</xsl:text>
				</xsl:if>
			</description>
			<link>
				<xsl:value-of select="$mainUrl"/>
				<xsl:text>/My/Conversations/Conversation/</xsl:text>
				<xsl:value-of select="interlocutor/account/id"/>
				<xsl:text>/</xsl:text>
				<xsl:if test="afterLastRead&lt;=lastMessageId">
					<xsl:text>p</xsl:text>
					<xsl:value-of select="afterLastRead"/>
				</xsl:if>
				<xsl:text>?</xsl:text>
				<xsl:value-of select="lastMessageDate/date/ticks"/>
			</link>
			<guid>
				<xsl:text>conversation-</xsl:text>
				<xsl:value-of select="interlocutor/account/id"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="lastMessageId"/>
			</guid>
			<pubDate>
				<xsl:apply-templates select="lastMessageDate/date" mode="dateTime"/>
			</pubDate>
		</item>
	</xsl:template>

</xsl:stylesheet>