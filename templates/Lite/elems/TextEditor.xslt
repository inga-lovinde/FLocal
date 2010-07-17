<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">

	<xsl:import href="UserInfoBar.xslt"/>

	<xsl:template name="textEditor">
		<xsl:param name="body"/>
		<input type="hidden" name="convert" value="markup"/>
		<xsl:text>Сообщение</xsl:text>
		<br/>
		<textarea cols="100" tabindex="2" rows="10" class="formboxes" name="Body">
			<xsl:attribute name="onKeyUp">storeCaret(this);</xsl:attribute>
			<xsl:attribute name="onClick">storeCaret(this);</xsl:attribute>
			<xsl:attribute name="onKeyPress">checkKeyPressed(document.replier,event,false);</xsl:attribute>
			<xsl:choose>
				<xsl:when test="bodyUBB">
					<xsl:value-of select="bodyUBB"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$body"/>
				</xsl:otherwise>
			</xsl:choose>
		</textarea>
		<br/>
		<br/>
		<script language="Javascript" type="text/javascript" src="/static/js/textEditor.js"><xsl:text> </xsl:text></script>
		<script language="Javascript" type="text/javascript">
<![CDATA[
function insertInBody(str) {
	insertAtCaret(document.replier.Body, str);
	document.replier.Body.focus();
}
]]>
		</script>
		<input type="checkbox" name="preview" value="1" class="formboxes" onClick="changeActionToSelf(this)" id="preview" />
		<label for="preview"> Я хочу предварительно просмотреть сообщение перед отправкой</label>
		<xsl:if test="bodyIntermediate">
			<div class="post">
				<xsl:value-of select="bodyIntermediate" disable-output-escaping="yes" />
			</div>
		</xsl:if>
		<br/>
		<br/>
	</xsl:template>

	<xsl:template match="layer">
		<xsl:param name="defaultLayerId"/>
		<option>
			<xsl:attribute name="value"><xsl:value-of select="id"/></xsl:attribute>
			<xsl:if test="id=$defaultLayerId">
				<xsl:attribute name="selected">selected</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="name"/>
		</option>
	</xsl:template>

</xsl:stylesheet>