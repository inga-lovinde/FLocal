<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>

	<xsl:template match="conversation">
		<div>
			<xsl:attribute name="class">
				<xsl:text>conversationcontainer</xsl:text>
				<xsl:if test="totalNewMessages and totalNewMessages!='0'">
					<xsl:text> updated</xsl:text>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>z-index:</xsl:text>
				<xsl:value-of select="1000-position()"/>
			</xsl:attribute>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="interlocutor/account/user" mode="userInfoBar"/>
			</div>
			<h4 class="conversationcontainer_title">
				<a>
					<xsl:attribute name="href">
						<xsl:text>/My/Conversations/Conversation/</xsl:text>
						<xsl:value-of select="interlocutor/account/id"/>
						<xsl:text>/</xsl:text>
						<xsl:if test="afterLastRead&lt;=lastMessageId">
							<xsl:text>p</xsl:text>
							<xsl:value-of select="afterLastRead"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="lastMessage/message/title"/>
				</a>
			</h4>
			<div>
				<xsl:attribute name="style">
					<xsl:variable name="rawHotness">
						<xsl:call-template name="log2">
							<xsl:with-param name="number">
								<xsl:value-of select="totalMessages"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="hotness">
						<xsl:choose>
							<xsl:when test="$rawHotness &lt; 0">0</xsl:when>
							<xsl:when test="$rawHotness &gt; 12">12</xsl:when>
							<xsl:otherwise><xsl:value-of select="$rawHotness"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:text>position:absolute;text-align:right;right:0px;bottom:0px;padding:2pt 5pt 0pt 5pt;</xsl:text>
					<xsl:text>background-color:rgb(</xsl:text>
					<xsl:value-of select="round($hotness*21.25)"/>
					<xsl:text>,0,</xsl:text>
					<xsl:value-of select="255-round($hotness*21.25)"/>
					<xsl:text>)</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="onmouseover">showChildren(this);</xsl:attribute>
				<xsl:attribute name="onmouseout">hideChildren(this);</xsl:attribute>
				<div default="default" style="z-index:1;" class="pseudolink">
					<a style="color:black;">?</a>
				</div>
				<div class="conversationcontainer_additionalcontainer" style="display:none;">
					<div class="conversationcontainer_additional" style="right:-5pt;">
						<p>
							<xsl:text>Сообщений: </xsl:text>
							<xsl:value-of select="totalMessages"/>
							<xsl:if test="totalNewMessages and totalNewMessages!='0'">
								<font class="new"><i>(<xsl:value-of select="totalNewMessages"/>)</i></font>
							</xsl:if>
						</p>
						<p>
							<xsl:text>Последнее: </xsl:text>
							<xsl:apply-templates select="lastMessageDate/date" mode="dateTime"/>
						</p>
						<xsl:apply-templates select="pageOuter" mode="withoutCurrent">
							<xsl:with-param name="baseLink">/My/Conversations/Conversation/<xsl:value-of select="interlocutor/account/id"/>/</xsl:with-param>
						</xsl:apply-templates>
					</div>
				</div>
			</div>
		</div>
	</xsl:template>


</xsl:stylesheet>