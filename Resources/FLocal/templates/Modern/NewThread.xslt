<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_NewThread">
			<xsl:with-param name="boardName" select="board/name"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_NewThread">
									<xsl:with-param name="boardName" select="board/name"/>
								</xsl:call-template>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/NewThread/" name="replier">
									<input type="hidden" name="board">
										<xsl:attribute name="value"><xsl:value-of select="board/id"/></xsl:attribute>
									</input>
									<xsl:apply-templates select="session/user" mode="Messages_CurrentUser"/>
									<br/>
									<br/>
									<xsl:call-template name="Messages_Title"/>
									<xsl:text>:</xsl:text>
									<br/>
									<input type="text" tabindex="1" name="title" maxlength="70" class="formboxes" size="60">
										<xsl:attribute name="value"><xsl:value-of select="newTitle"/></xsl:attribute>
									</input>
									<xsl:call-template name="Messages_Layer"/>
									<xsl:text>:</xsl:text>
									<select class="formboxes" name="layerId">
										<xsl:apply-templates select="layers">
											<!--xsl:with-param name="defaultLayerId"><xsl:value-of select="post/layerId"/></xsl:with-param-->
										</xsl:apply-templates>
									</select>
									<br/>
									<br/>
									<xsl:call-template name="textEditor"/>
									<input type="submit" tabindex="3" name="textcont" taborder="2" class="buttons">
										<xsl:attribute name="value">
											<xsl:call-template name="Messages_NewThread">
												<xsl:with-param name="boardName" select="board/name"/>
											</xsl:call-template>
										</xsl:attribute>
									</input>
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>