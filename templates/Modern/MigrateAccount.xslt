<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">��������</xsl:template>
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
								<xsl:text>������������ </xsl:text>
								<xsl:apply-templates select="migrationInfo/account/user" mode="userLink"/>
								<xsl:text> ������ � ������ ������������� ������� ������.</xsl:text>
								<br/>
								<xsl:text>������� ������ </xsl:text>
								<b>(fhn:<xsl:value-of select="migrationInfo/key"/>)</b>
								<xsl:text> (������ �� ��������) � ���� ��������� � ���� �������� �� ������ ������.</xsl:text>
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
									<xsl:text>����� ������</xsl:text><br /> 
									<input type="password" name="password" class="formboxes" /><br/>
									<xsl:text>��������� ������</xsl:text><br/> 
									<input type="password" name="password2" class="formboxes" /><br/>
									<xsl:text>e-mail ��� �������������� ������ (�������������)</xsl:text><br/>
									<input type="text" name="registrationEmail" class="formboxes" />
									<br/>
									<input type="checkbox" name="constitution" value="constitution" id="constitution"/>
									<label for="constitution"> ��, � ��������/��������/�������� � ���, ��� �� ���� ������ ��������� </label>
									<a href="/q/constitution/" style="text-decoration:underline">��������� �� ���� ������ �����������</a>
									<xsl:text>.</xsl:text>
									<i> (�����������)</i>
									<br/>
									<input type="checkbox" name="showPostsToAll" value="showPostsToAll" id="showPostsToAll"/>
									<label for="showPostsToAll"> ��, � ��������/��������/�������� � ���, ����� ��� ���������, ����������� �� ���� ������ ��� �� �����.������ (��������.��), ���� �������� ��������.</label>
									<i> (�����������)</i>
									<br/>
									<input type="checkbox" name="law" value="law" id="law"/>
									<label for="law"> ��, � �������� ��������� ������ ���������� ��������� � ������������ ���������� �������� � ������� �� ���� ������.</label>
									<i> (�����������)</i>
									<br/>
									<input type="submit" name="buttlogin" value="�����������!" class="buttons" /> 
								</form>
								<h3 style="color:red">
									<xsl:text>���� � ��� ���-�� �� ���������� - ���������� </xsl:text>
									<a href="http://penartur.narod.ru/flocal/migration/">���������� � ���������</a>
								</h3>
							</td> 
						</tr> 
					</table> 
				</td> 
			</tr> 
		</table> 
	</xsl:template>

</xsl:stylesheet>