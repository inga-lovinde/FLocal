<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="post">
		<xsl:param name="isReplyDisabled">true</xsl:param>
		<div class="postcontainer" style="clear:both">
			<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
			<div class="userbarcontainer" style="float:left">
				<xsl:apply-templates select="poster/user" mode="userInfoBar"/>
			</div>
			<div class="linkscontainer" style="float:right;text-align:right;">
				<a>
					<xsl:if test="$isReplyDisabled='false'">
						<xsl:if test="/root/session/sessionKey">
							<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Reply/</xsl:attribute>
							<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
							<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
						</xsl:if>
					</xsl:if>
					<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
						<xsl:with-param name="src">reply.gif</xsl:with-param>
						<xsl:with-param name="alt">Ответ</xsl:with-param>
					</xsl:apply-templates>
				</a>
				<xsl:text> </xsl:text>
				<a>
					<xsl:if test="/root/session/sessionKey">
						<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/PMReply/</xsl:attribute>
						<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
						<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
						<xsl:with-param name="src">email2.gif</xsl:with-param>
						<xsl:with-param name="alt">Ответить в ПМ</xsl:with-param>
					</xsl:apply-templates>
				</a>
				<xsl:text> </xsl:text>
				<xsl:choose>
					<xsl:when test="isPunishmentEnabled='true'">
						<a>
							<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Punish/</xsl:attribute>
							<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
								<xsl:with-param name="src">punish.gif</xsl:with-param>
								<xsl:with-param name="alt">Модерировать</xsl:with-param>
							</xsl:apply-templates>
						</a>
					</xsl:when>
					<xsl:otherwise>
						<a>
							<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
								<xsl:with-param name="src">notifymod.gif</xsl:with-param>
								<xsl:with-param name="alt">Известить модератора</xsl:with-param>
							</xsl:apply-templates>
						</a>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="isOwner='true'">
					<br/>
					<a>
						<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Edit/</xsl:attribute>
						<xsl:apply-templates select="postDate/date" mode="navigationImageFor">
							<xsl:with-param name="src">edit.gif</xsl:with-param>
							<xsl:with-param name="alt">Правка</xsl:with-param>
						</xsl:apply-templates>
					</a>
				</xsl:if>
			</div>
			<div class="postcontent">
				<xsl:variable name="postDate"><xsl:apply-templates select="postDate/date" mode="dateTime"/></xsl:variable>
				<a target="_blank" class="separate">
					<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/</xsl:attribute>
					<xsl:attribute name="title"><xsl:value-of select="$postDate"/></xsl:attribute>
					<img border="0" alt="" style="vertical-align: text-bottom">
						<xsl:choose>
							<xsl:when test="isUnread='true'">
								<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-notread.gif</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="src">/static/images/message-<xsl:value-of select="layerName"/>-read.gif</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</img>
				</a>
				<xsl:text> </xsl:text>
				<xsl:if test="parentPost/post">
					<font class="small separate">
						<xsl:text>[</xsl:text>
						<a>
							<xsl:attribute name="title"><xsl:value-of select="parentPost/post/bodyShort"/></xsl:attribute>
							<xsl:attribute name="href">/Thread/<xsl:value-of select="threadId"/>/p<xsl:value-of select="parentPost/post/id"/>/</xsl:attribute>
							<xsl:text>re: </xsl:text>
							<xsl:value-of select="parentPost/post/poster/user/name"/>
						</a>
						<xsl:text>]</xsl:text>
					</font>
				</xsl:if>
				<xsl:if test="not(parentPost/post/title = title) and not(concat('Re: ', parentPost/post/title) = title)">
					<xsl:value-of select="title"/>
				</xsl:if>
				<xsl:text>&#160;&#160;</xsl:text>
				<font style="font-size:0.6em">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$postDate"/>
					<xsl:text>)</xsl:text>
				</font>
				<div class="postbody">
					<xsl:value-of select="body" disable-output-escaping="yes" />
				</div>
				<div style="">
					<xsl:if test="poster/user/signature != ''">
						<div style="float:right;max-height:3.5em;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden;text-align:right;font-size:0.8em;">
							<xsl:value-of select="poster/user/signature" disable-output-escaping="yes" />
						</div>
					</xsl:if>
					<xsl:apply-templates select="specific"/>
					<xsl:comment>fill</xsl:comment>
				</div>
			</div>
			<div style="clear:both;" class="zerosize">
				<xsl:comment>fill</xsl:comment>
			</div>
		</div>
	</xsl:template>

	<xsl:template match="specific/changeInfo">
		<xsl:if test="(revision != '') and (revision != '0')">
			<span style="font-size:0.6em" class="revision">
				<xsl:variable name="lastChangeDate">
					<xsl:apply-templates select="lastChangeDate/date" mode="dateTime"/>
				</xsl:variable>
				<xsl:attribute name="title">
					<xsl:text>последняя </xsl:text>
					<xsl:value-of select="$lastChangeDate"/>
				</xsl:attribute>
				<xsl:value-of select="revision"/>
				<xsl:text> правок</xsl:text>
			</span>
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="specific/punishment">
		<xsl:if test="not(isWithdrawed='true')">
			<span style="font-size:0.6em" class="punishment">
				<xsl:value-of select="punishmentType/description"/>
				<xsl:text>. </xsl:text>
				<xsl:value-of select="comment"/>
				<xsl:text> (</xsl:text>
				<xsl:apply-templates select="moderator/user" mode="userLink"/>
				<xsl:text>, </xsl:text>
				<xsl:apply-templates select="punishmentDate/date" mode="dateTime"/>
				<xsl:text>).</xsl:text>
				<xsl:if test="transfer">
					<xsl:text> </xsl:text>
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
					<xsl:text>.</xsl:text>
				</xsl:if>
				<xsl:if test="layerChange">
					<xsl:text> </xsl:text>
					<xsl:text>Исходный слой </xsl:text>
					<xsl:value-of select="layerChange/oldLayer/layer/name"/>
					<xsl:text>.</xsl:text>
				</xsl:if>
			</span>
		</xsl:if>
	</xsl:template>

	<xsl:template match="specific/thread">
		<div class="discussionlinkcontainer">
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
				<xsl:text>Обсудить</xsl:text>
			</a>
			<xsl:if test="totalPosts &gt; 1">
				<span class="separate"><xsl:value-of select="totalPosts"/></span>
				<xsl:if test="totalNewPosts and totalNewPosts!='0'">
					<a class="cup separate">
						<xsl:if test="/root/session/sessionKey">
							<xsl:attribute name="href">/do/MarkThreadAsRead/<xsl:value-of select="id"/>/p<xsl:value-of select="lastPostId"/>/</xsl:attribute>
						</xsl:if>
						<font class="new"><i>(<xsl:value-of select="totalNewPosts"/>)</i></font>
					</a>
				</xsl:if>
			</xsl:if>
		</div>
	</xsl:template>

</xsl:stylesheet>