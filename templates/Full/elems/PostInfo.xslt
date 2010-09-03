<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="post">
		<xsl:param name="isReplyDisabled">true</xsl:param>
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<a><xsl:attribute name="name">Post<xsl:value-of select="id"/></xsl:attribute></a>
							<xsl:apply-templates select="poster/user" mode="userInfoBar"/>
						</td>
						<td class="subjecttable">
							<table width="100%" border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td align="left" width="80%" valign="top">
										<a target="_blank" class="separate">
											<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/</xsl:attribute>
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
										<b class="separate"><xsl:value-of select="title"/></b>
										<xsl:if test="isUnread='true'">
											<img alt="new" src="/static/images/new.gif" />
										</xsl:if>
										<xsl:if test="parentPost/post">
											<font class="small separate">
												<xsl:text>[</xsl:text>
												<a>
													<xsl:attribute name="title"><xsl:value-of select="parentPost/post/bodyShort"/></xsl:attribute>
													<xsl:attribute name="href">/Post/<xsl:value-of select="parentPost/post/id"/>/</xsl:attribute>
													<xsl:text>re: </xsl:text>
													<xsl:value-of select="parentPost/post/poster/user/name"/>
												</a>
												<xsl:text>]</xsl:text>
											</font>
										</xsl:if>
										<br />
										<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
									</td>
									<td align="right" width="20%">
										<table cellpadding="0" cellspacing="0" border="0">
											<tr>
												<td align="right">
													<table class="tablesurround" border="0">
														<tr>
															<xsl:if test="isPunishmentEnabled='true'">
																<td class="navigation">
																	<a>
																		<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Punish/</xsl:attribute>
																		<img src="/static/images/punish.gif" border="0" alt="Модерировать сообщение" title="Модерировать сообщение" style="vertical-align: text-bottom" />
																	</a>
																</td>
															</xsl:if>
															<td class="navigation">
																<a>
																	<xsl:if test="$isReplyDisabled='false'">
																		<xsl:if test="/root/session/sessionKey">
																			<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Reply/</xsl:attribute>
																			<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
																			<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
																		</xsl:if>
																	</xsl:if>
																	<img src="/static/images/reply.gif" border="0" alt="Ответ на сообщение" width="27" height="14" title="Ответ на сообщение" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:if test="isOwner='true'">
																		<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/Edit/</xsl:attribute>
																	</xsl:if>
																	<img src="/static/images/edit.gif" border="0" alt="Правка сообщения" title="Правка сообщения" width="21" height="14" style="vertical-align: text-bottom" />
																</a>
															</td>
															<td class="navigation">
																<a target="_blank">
																	<img src="/static/images/print.gif" border="0" alt="Печать сообщения" title="Печать сообщения" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/addreminder.gif" border="0" alt="Добавить тему в напоминания!" title="Добавить тему в напоминания!" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<img src="/static/images/notifymod.gif" border="0" alt="Известить модератора" title="Известить модератора" />
																</a>
															</td>
															<td class="navigation">
																<a>
																	<xsl:if test="/root/session/sessionKey">
																		<xsl:attribute name="href">/Post/<xsl:value-of select="id"/>/PMReply/</xsl:attribute>
																		<xsl:attribute name="onClick">submitSelText(this.href);return false;</xsl:attribute>
																		<xsl:attribute name="onMouseDown">submitSelText(this.href);return false;</xsl:attribute>
																	</xsl:if>
																	<img src="/static/images/email2.gif" border="0" alt="Ответить приватом" title="Ответить приватом" />
																</a>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="lighttable">
							<table width="100%" cellspacing="0" cellpadding="0" style="table-layout: fixed">
								<tr>
									<td>
										<br />
										<font class="post">
											<xsl:value-of select="body" disable-output-escaping="yes" />
											<!--<xsl:copy-of select="body"/>-->
											<br />
											<br />
										</font>
									</td>
								</tr>
								<xsl:apply-templates select="specific"/>
								<xsl:if test="poster/user/signature != ''">
									<tr>
										<td>
											<div style="width:100%;max-height:50px;height: expression( this.scrollHeight > 49 ? '50px' : 'auto' );overflow:hidden">
												<font size="-2"><xsl:value-of select="poster/user/signature"/><br /></font>
											</div>
										</td>
									</tr>
								</xsl:if>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="specific/changeInfo">
		<xsl:if test="(revision != '') and (revision != '0')">
			<tr>
				<td>
					<font size="-2">
						<xsl:text>Это сообщение было отредактировано пользователем </xsl:text>
						<xsl:value-of select="revision"/>
						<xsl:text> раз, последний раз </xsl:text>
						<xsl:apply-templates select="lastChangeDate/date" mode="dateTime"/>
						<br/>
						<br/>
					</font>
				</td>
			</tr>
		</xsl:if>
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
					<br/>
				</font>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="specific/thread">
		<tr>
			<td>
				<font size="-1">
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
					<br/>
					<br/>
				</font>
			</td>
		</tr>
	</xsl:template>

</xsl:stylesheet>