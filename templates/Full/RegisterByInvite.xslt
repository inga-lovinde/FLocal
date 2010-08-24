<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">Регистрация</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>Регистрация по инвайту</xsl:text>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/RegisterByInvite/">
									<input type="hidden" name="inviteId">
										<xsl:attribute name="value"><xsl:value-of select="invite/id"/></xsl:attribute>
									</input>
									<input type="hidden" name="code">
										<xsl:attribute name="value"><xsl:value-of select="invite/code"/></xsl:attribute>
									</input>
									<xsl:text>Имя пользователя</xsl:text><br/>
									<input type="text" name="login" class="formboxes" maxlength="16"/><br/>
									<xsl:text>Новый пароль</xsl:text><br /> 
									<input type="password" name="password" class="formboxes" /><br/>
									<xsl:text>Повторите пароль</xsl:text><br/> 
									<input type="password" name="password2" class="formboxes" /><br/>
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
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>