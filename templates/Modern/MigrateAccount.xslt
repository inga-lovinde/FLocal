<?xml version="1.0" encoding="Windows-1251"?>
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
								<xsl:text>Пользователь </xsl:text>
								<xsl:apply-templates select="migrationInfo/account/user" mode="userLink"/>
								<xsl:text> найден в списке пользователей старого форума.</xsl:text>
								<br/>
								<xsl:text>Введите строку </xsl:text>
								<b>(fhn:<xsl:value-of select="migrationInfo/key"/>)</b>
								<xsl:text> (вместе со скобками) в поле биографии в своём профайле на старом форуме.</xsl:text>
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
									<xsl:text>Новый пароль</xsl:text><br /> 
									<input type="password" name="password" class="formboxes" /><br/>
									<xsl:text>Повторите пароль</xsl:text><br/> 
									<input type="password" name="password2" class="formboxes" /><br/>
									<xsl:text>e-mail для восстановления пароля (необязательно)</xsl:text><br/>
									<input type="text" name="registrationEmail" class="formboxes" />
									<br/>
									<input type="checkbox" name="constitution" value="constitution" id="constitution"/>
									<label for="constitution"> Да, я согласен/согласна/согласно с тем, что на этом форуме действует </label>
									<a href="/q/constitution/" style="text-decoration:underline">описанная по этой ссылке конституция</a>
									<xsl:text>.</xsl:text>
									<i> (обязательно)</i>
									<br/>
									<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
									<label for="showPostsToAll"> Да, я согласен/согласна/согласно с тем, чтобы мои сообщения, размещённые на этом форуме или на форум.локале (форумбгз.ру), были доступны публично.</label>
									<i> (обязательно)</i>
									<br/>
									<input type="checkbox" name="law" value="law" id="law"/>
									<label for="law"> Да, я обязуюсь соблюдать законы Российской Федерации и Федеративной Республики Германия в общении на этом форуме.</label>
									<i> (обязательно)</i>
									<br/>
									<input type="submit" name="buttlogin" value="Мигрировать!" class="buttons" /> 
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