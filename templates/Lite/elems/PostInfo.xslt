<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:template match="post[hidden]">
		<div>
			<div class="darktable">
				<a>
					<xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute>
					<xsl:comment>fill</xsl:comment>
				</a>
				<div class="lighttable post">
					<p>Вам нельзя видеть этот сладкий и запретный плод.</p>
					<xsl:if test="/root/session/notLoggedIn">
						<p>Возможно, <a href="/My/Login/Login/">вход на форум</a> исправит ситуацию</p>
					</xsl:if>
				</div>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="post[not(hidden)]">
		<xsl:param name="isReplyDisabled">true</xsl:param>
		<div>
			<div class="darktable">
				<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
				<b><xsl:apply-templates select="poster/user" mode="userLink"/></b>
				<xsl:text> : </xsl:text>
				<xsl:if test="layerName='offtop'">{o}</xsl:if>
				<xsl:if test="layerName='garbage'">{g}</xsl:if>
				<xsl:choose>
					<xsl:when test="isUnread='true'">
						<xsl:text>(*)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
				<!--b><xsl:value-of select="title"/></b-->
				<xsl:if test="parentPost/post">
					<font class="small separate">
						<xsl:text>[</xsl:text>
						<a>
							<xsl:attribute name="href">/Post/<xsl:value-of select="parentPost/post/id"/>/</xsl:attribute>
							<xsl:text>re: </xsl:text>
							<xsl:value-of select="parentPost/post/poster/user/name"/>
						</a>
						<xsl:text>]</xsl:text>
					</font>
				</xsl:if>
				<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
				<span style="padding-left:1em">
					<xsl:if test="isPunishmentEnabled='true'">
						<xsl:text> | </xsl:text>
						<a class="mod">
							<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Punish/</xsl:attribute>
							<xsl:text>Mod</xsl:text>
						</a>
					</xsl:if>
					<xsl:text> | </xsl:text>
					<a>
						<xsl:if test="$isReplyDisabled='false'">
							<xsl:if test="/root/session/sessionKey">
								<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Reply/</xsl:attribute>
								<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
								<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
							</xsl:if>
						</xsl:if>
						<xsl:text>Reply</xsl:text>
					</a>
					<xsl:text> | </xsl:text>
					<a>
						<xsl:if test="isOwner='true'">
							<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Edit/</xsl:attribute>
						</xsl:if>
						<xsl:text>Edit</xsl:text>
					</a>
					<xsl:text> | </xsl:text>
				</span>
			</div>
			<div class="lighttable post">
				<xsl:value-of select="body" disable-output-escaping="yes" />
				<br/>
				<xsl:apply-templates select="specific"/>
				<br/>
			</div>
		</div>
	</xsl:template>

	<xsl:template match="specific/changeInfo">
	</xsl:template>

	<xsl:template match="specific/punishment">
		<tr>
			<td>
				<font size="-2">
					<xsl:attribute name="class">
						<xsl:text>punishment</xsl:text>
						<xsl:if test="isWithdrawed='true'">
							<xsl:text> withdrawed</xsl:text>
						</xsl:if>
					</xsl:attribute>
					<xsl:value-of select="punishmentType/description"/>
					<xsl:text> (</xsl:text>
					<xsl:value-of select="punishmentType/weightDescription"/>
					<xsl:text>). </xsl:text>
					<xsl:value-of select="comment"/>
					<xsl:text> (</xsl:text>
					<xsl:apply-templates select="moderator/user" mode="userLink"/>
					<xsl:text>, </xsl:text>
					<xsl:apply-templates select="punishmentDate/date" mode="dateTime"/>
					<xsl:text>)</xsl:text>
					<br/>
					<xsl:if test="transfer">
						<xsl:choose>
							<xsl:when test="transfer/isSubthreadTransfer='true'">
								<xsl:text>Подветка была перенесена</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>Сообщение было перенесено</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="transfer/oldParentPost">
							<xsl:text> из обсуждения сообщения </xsl:text>
							<a>
								<xsl:attribute name="href">
									<xsl:text>/Post/</xsl:text>
									<xsl:value-of select="transfer/oldParentPost/post/id"/>
									<xsl:text>/</xsl:text>
								</xsl:attribute>
								<xsl:value-of select="transfer/oldParentPost/post/title"/>
							</a>
						</xsl:if>
						<xsl:text> из раздела </xsl:text>
						<a>
							<xsl:attribute name="href">
								<xsl:text>/Board/</xsl:text>
								<xsl:value-of select="transfer/oldBoard/board/id"/>
								<xsl:text>/</xsl:text>
							</xsl:attribute>
							<xsl:value-of select="transfer/oldBoard/board/name"/>
						</a>
						<br/>
					</xsl:if>
					<xsl:if test="layerChange">
						<xsl:choose>
							<xsl:when test="layerChange/isSubthreadTransfer='true'">
								<xsl:text>Слой подветки был изменён</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>Слой сообщения был изменён</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text> с </xsl:text>
						<xsl:value-of select="layerChange/oldLayer/layer/name"/>
						<br/>
					</xsl:if>
					<br/>
				</font>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="specific/thread">
		<xsl:choose>
			<xsl:when test="afterLastRead&lt;=lastPostId">
				<xsl:text>(*</xsl:text>
				<xsl:value-of select="totalNewPosts"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:otherwise>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text> </xsl:text>
		<a>
			<xsl:attribute name="href">
				<xsl:text>/Thread/</xsl:text>
				<xsl:value-of select="id"/>
				<xsl:text>/</xsl:text>
				<xsl:if test="afterLastRead&lt;=lastPostId">
					<xsl:text>p</xsl:text>
					<xsl:value-of select="afterLastRead"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="totalPosts &gt; 1">
					<xsl:text>Обсуждение этого сообщения</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>Начать обсуждение</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</a>
	</xsl:template>

</xsl:stylesheet>