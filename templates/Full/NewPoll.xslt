<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\TextEditor.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:text>����� ������</xsl:text>
	</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:text>�������� ������ ������</xsl:text>
							</td> 
						</tr>
						<tr class="darktable"> 
							<td> 
								<xsl:text>��������� ����������� ���� ����� ��� �������� ��������� � �����. HTML ��������. UBBCode �������, � �� ������ ������������ UBBCode � ����� ����������. ��������� ��������� ���������, � �� ������ ������� ����� �������������������� ���.</xsl:text>
							</td> 
						</tr> 
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/NewPoll/" name="replier">
									<xsl:text>������������: </xsl:text>
									<xsl:value-of select="session/user/name"/>
									<br/>
									<br/>
									<xsl:text>����: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="title">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<input type="checkbox" name="isDetailed" value="isDetailed"/><label for="isDetailed"> ���������� ����� ���������������</label>
									<br/>
									<input type="checkbox" name="isMultiOption" value="isMultiOption"/><label for="isMultiOption"> ��������� ����� ���������� ���������</label>
									<br/>
									<br/>
									<xsl:text>������� 1: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 2: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 3: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 4: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 5: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 6: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 7: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 8: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 9: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 10: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 11: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 12: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 13: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 14: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 15: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 16: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 17: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 18: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 19: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>������� 20: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<input type="submit" tabindex="3" name="textcont" taborder="2" value="����������" class="buttons"/>
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

</xsl:stylesheet>