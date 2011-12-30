<?xml version="1.0" encoding="ASCII"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Миграция</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_Migration">
									<xsl:with-param name="userName"><xsl:value-of select="migrationInfo/account/user/name"/></xsl:with-param>
								</xsl:call-template>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<xsl:apply-templates mode="Messages_MigrationUserFound" select="migrationInfo/account/user"/>
								<br/>
								<xsl:call-template name="Messages_MigrationKeyInfo">
									<xsl:with-param name="key"><xsl:value-of select="migrationInfo/key"/></xsl:with-param>
								</xsl:call-template>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/MigrateAccount/">
									<input type="hidden" name="accountId">
										<xsl:attribute name="value"><xsl:value-of select="migrationInfo/account/id"/></xsl:attribute>
									</input>
									<input type="hidden" name="check">
										<xsl:attribute name="value"><xsl:value-of select="migrationInfo/check"/></xsl:attribute>
									</input>
									<xsl:call-template name="Messages_NewPassword"/>
									<br/>
									<input type="password" name="password" class="formboxes"/>
									<br/>
									<xsl:call-template name="Messages_NewPasswordRepeat"/>
									<br/>
									<input type="password" name="password2" class="formboxes" /><br/>
									<xsl:call-template name="Messages_AccountEmail"/>
									<br/>
									<input type="text" name="registrationEmail" class="formboxes" />
									<br/>
									<input type="checkbox" name="constitution" value="constitution" id="constitution"/>
									<label for="constitution"> <xsl:call-template name="Messages_AcceptConstitution"/></label>
									<i> (<xsl:call-template name="Messages_AcceptanceRequired"/>)</i>
									<br/>
									<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
									<label for="showPostsToAll"> <xsl:call-template name="Messages_AcceptPostsVisibility"/></label>
									<i> (<xsl:call-template name="Messages_AcceptanceRequired"/>)</i>
									<br/>
									<input type="checkbox" name="law" value="law" id="law"/>
									<label for="law"> <xsl:call-template name="Messages_AcceptLaws"/></label>
									<i> (<xsl:call-template name="Messages_AcceptanceRequired"/>)</i>
									<br/>
									<input type="submit" name="buttlogin" class="buttons">
										<xsl:call-template name="Messages_Migration"/>
									</input>
								</form>
								<h3 style="color:red">
									<xsl:text>Если у вас что-то не получается - прочитайте </xsl:text>
									<a href="http://penartur.narod.ru/flocal/migration/">инструкцию в картинках</a>
								</h3>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>