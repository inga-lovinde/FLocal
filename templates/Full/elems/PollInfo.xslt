<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>
	<xsl:template match="poll">
		<tr>
			<td>
				<table width="100%" cellspacing="1" cellpadding="3" border="0">
					<tr>
						<td width="120" valign="top" class="darktable" rowspan="2">
							<xsl:apply-templates select="poster" mode="userInfoBar"/>
						</td>
						<td class="subjecttable">
							<a target="_blank" class="separate">
								<img border="0" src="/static/images/message-normal-read.gif"  alt="" style="vertical-align: text-bottom" />
							</a>
							<b class="separate"><xsl:value-of select="title" disable-output-escaping="yes"/></b>
							<br />
							<font class="small" style="padding-left:2em"><xsl:apply-templates select="postDate/date" mode="dateTime"/></font>
						</td>
					</tr>
					<tr>
						<td class="lighttable">
							<table width="100%" cellspacing="0" cellpadding="0" style="table-layout: fixed">
								<tr>
									<td>
										<font class="post">
											<form action="/do/Vote/" method="POST">
												<input type="hidden" name="pollId">
													<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
												</input>
												<h2><xsl:value-of select="title" disable-output-escaping="yes"/></h2>
												<p>
													<xsl:choose>
														<xsl:when test="isDetailed='true'">
															<xsl:text>Детализация по голосам в этом опросе открыта</xsl:text>
														</xsl:when>
														<xsl:otherwise>
															<xsl:text>Это анонимный опрос</xsl:text>
														</xsl:otherwise>
													</xsl:choose>
												</p>
												<p>
													<xsl:choose>
														<xsl:when test="isMultiOption='true'">
															<xsl:text>Этот опрос допускает выбор нескольких вариантов ответа</xsl:text>
														</xsl:when>
														<xsl:otherwise>
															<xsl:text>Этот опрос допускает выбор только одного варианта ответа</xsl:text>
														</xsl:otherwise>
													</xsl:choose>
												</p>
												<p>
													<xsl:text>Всего голосов: </xsl:text>
													<xsl:value-of select="options/total"/>
												</p>
												<table border="2" width="100%">
													<xsl:apply-templates select="options/option">
														<xsl:with-param name="inputType">
															<xsl:choose>
																<xsl:when test="isMultiOption='true'">
																	<xsl:text>checkbox</xsl:text>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:text>radio</xsl:text>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:with-param>
														<xsl:with-param name="totalVotes">
															<xsl:value-of select="options/total"/>
														</xsl:with-param>
													</xsl:apply-templates>
												</table>
												<input type="submit" value="Проголосовать"/>
											</form>
											<br/>
											<br/>
											<xsl:text>Для вставки ссылки в форум используйте тэг [poll]</xsl:text>
											<xsl:value-of select="id"/>
											<xsl:text>[/poll]</xsl:text>
											<br/>
										</font>
									</td>
								</tr>
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

	<xsl:template match="option">
		<xsl:param name="inputType">radio</xsl:param>
		<xsl:param name="totalVotes">0</xsl:param>
		<tr>
			<td width="50%">
				<input name="option">
					<xsl:attribute name="type"><xsl:value-of select="$inputType"/></xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
					<xsl:if test="isSelected='true'">
						<xsl:attribute name="checked">checked</xsl:attribute>
					</xsl:if>
					<xsl:if test="not(/root/session/sessionKey)">
						<xsl:attribute name="disabled">disabled</xsl:attribute>
					</xsl:if>
				</input>
				<label for="option">
					<xsl:text> </xsl:text>
					<xsl:value-of select="name" disable-output-escaping="yes"/>
				</label>
			</td>
			<td width="50%">
				<div class="pollcolor">
					<xsl:attribute name="style">
						<xsl:text>height:1em;</xsl:text>
						<xsl:text>width:</xsl:text>
						<xsl:choose>
							<xsl:when test="votes &gt; 0">
								<xsl:value-of select="round(100 * votes div $totalVotes)"/>
								<xsl:text>%</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>3px</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:text> </xsl:text>
				</div>
				<p>
					<xsl:value-of select="votes"/>
					<xsl:text> голосов (</xsl:text>
					<xsl:choose>
						<xsl:when test="votes &gt; 0">
							<xsl:value-of select="round(100 * votes div $totalVotes)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>0</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>%)</xsl:text>
				</p>
				<xsl:if test="voters/user">
					<p>
						<xsl:apply-templates select="voters/user" mode="userLink"/>
					</p>
				</xsl:if>
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="user" mode="userLink">
		<a class="separate">
			<xsl:attribute name="href">/User/<xsl:value-of select="id"/>/</xsl:attribute>
			<xsl:value-of select="name"/>
		</a>
	</xsl:template>

</xsl:stylesheet>