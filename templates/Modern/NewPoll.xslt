<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:template name="specificTitle">
		<xsl:call-template name="Messages_NewPoll"/>
	</xsl:template>
	<xsl:template name="isLiteEnabled">true</xsl:template>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td class="tdheader">
								<xsl:call-template name="Messages_NewPoll"/>
							</td> 
						</tr>
						<tr> 
							<td class="lighttable"> 
								<form method="post" action="/do/NewPoll/" name="replier">
									<xsl:apply-templates select="session/user" mode="Messages_User"/>
									<br/>
									<br/>
									<xsl:call-template name="Messages_Title"/>
									<xsl:text>:</xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="title">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<input type="checkbox" name="isDetailed" value="isDetailed" id="isDetailed"/>
									<label for="isDetailed"> <xsl:call-template name="Messages_PollIsDetailed"/></label>
									<br/>
									<input type="checkbox" name="isMultiOption" value="isMultiOption" id="isMultiOption"/>
									<label for="isMultiOption"> <xsl:call-template name="Messages_PollIsMultioption"/></label>
									<br/>
									<br/>
									<xsl:text>Вариант 1: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 2: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 3: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 4: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 5: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 6: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 7: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 8: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 9: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 10: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 11: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 12: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 13: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 14: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 15: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 16: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 17: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 18: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 19: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<xsl:text>Вариант 20: </xsl:text>
									<br/>
									<textarea cols="100" tabindex="2" rows="3" class="formboxes" name="option">
										<xsl:text> </xsl:text>
									</textarea>
									<br/>
									<br/>
									<input type="submit" tabindex="3" name="textcont" taborder="2" class="buttons">
										<xsl:call-template name="Messages_NewPoll"/>
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