<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_Login"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_Login"/>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:call-template name="Messages_LoginText"/>
								<xsl:if test="isMigrationEnabled='true'">
									<br/>
									<xsl:call-template name="Messages_LoginMigrationText"/>
								</xsl:if>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/Login/">
									<xsl:call-template name="Messages_Username"/>
									<br/> 
									<input type="text" name="name" class="formboxes" /><br/>
									<xsl:call-template name="Messages_Password"/>
									<br/> 
									<input type="password" name="password" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" class="buttons">
										<xsl:attribute name="value"><xsl:call-template name="Messages_Login"/></xsl:attribute>
									</input>
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
		<br/>
		<xsl:if test="isMigrationEnabled='true'">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_Migration"/>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:call-template name="Messages_MigrationText"/>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/My/Login/Migrate/">
									<xsl:call-template name="Messages_Username"/>
									<br/> 
									<input type="text" name="username" class="formboxes" /><br/>
									<input type="submit" name="buttlogin" class="buttons">
										<xsl:attribute name="value"><xsl:call-template name="Messages_Migration"/></xsl:attribute>
									</input>
								</form> 
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
		<br/>
		</xsl:if>
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_Registration"/>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td>
								<xsl:choose>
									<xsl:when test="isLocalNetwork='false'">
										<xsl:call-template name="Messages_IpForbidden">
											<xsl:with-param name="ip"><xsl:value-of select="ip"/></xsl:with-param>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="Messages_IpForbidden">
											<xsl:with-param name="ip"><xsl:value-of select="ip"/></xsl:with-param>
										</xsl:call-template>
									</xsl:otherwise>
								</xsl:choose>
								<br/>
								<a href="/Maintenance/LocalNetworks/">
									<xsl:call-template name="Messages_LocalNetworks"/>
								</a>
							</td> 
						</tr>
						<xsl:if test="isLocalNetwork='true'">
							<tr> 
								<td class="lighttable"> 
									<form method="post" action="/do/Register/">
										<xsl:call-template name="Messages_Username"/>
										<br/>
										<input type="text" name="login" class="formboxes" maxlength="16"/><br/>
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
										<label for="constitution"> Да, я согласен/согласна/согласно с тем, что на этом форуме действует </label>
										<a href="/q/constitution/" style="text-decoration:underline">описанная по этой ссылке конституция</a>
										<xsl:text>.</xsl:text>
										<i> (обязательно)</i>
										<br/>
										<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
										<label for="showPostsToAll"> Да, я согласен/согласна/согласно с тем, чтобы мои сообщения, размещённые на этом форуме, были доступны публично.</label>
										<i> (обязательно)</i>
										<br/>
										<input type="checkbox" name="law" value="law" id="law"/>
										<label for="law"> Да, я обязуюсь соблюдать законы Российской Федерации и Федеративной Республики Германия в общении на этом форуме.</label>
										<i> (обязательно)</i>
										<br/>
										<input type="submit" name="buttlogin" value="Зарегистрироваться!" class="buttons" /> 
									</form>
								</td> 
							</tr> 
						</xsl:if>
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>