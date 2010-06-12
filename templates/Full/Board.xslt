<?xml version="1.0" encoding="Windows-1251"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
	<xsl:import href="elems\Main.xslt"/>
	<xsl:import href="elems\BoardInfo.xslt"/>
	<xsl:template name="specific">
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" border="0" class="tableborders">
						<tr class="darktable">
							<td colspan="6">
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr>
										<td colspan="2" class="catandforum">
											<xsl:apply-templates select="currentBoard" mode="breadcrumbs"/>
										</td>
										<td align="right" valign="bottom">
											<table border="0" class="tablesurround">
												<tr>
													<td class="navigation" nowrap="nowrap">
														<!-- postoption is either newpost.gif or greynewpost.gif -->
														<img src="/static/images/newpost.gif" alt="Новое сообщение" border="0" width="13" height="15" style="vertical-align: text-bottom" />
														<a href="/newpost.php?Cat=&amp;Board=Common&amp;page=0&amp;src=&amp;sb=5&amp;o=&amp;showlite=">Сообщение</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<form method="get" action="/postlist.php" name="fullview">
															<select name="fullview" class="formboxes" style="font-size:10px" onchange="document.forms.fullview.submit()">
																<option value="0">Норм</option>
																<option value="1">Офт</option>
																<option value="2" selected="selected">Мус</option>
															</select>
														</form>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- prevoption is either previous.gif or greyprevious.gif -->
														<a>
															<img alt="Предыдущая страница" border="0" width="12" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/greyprevious.gif</xsl:attribute>
															</img>
															<span>Пред.</span>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<a href="/ubbthreads.php?Cat=&amp;C=1">
															<img src="/static/images/all.gif" alt="Список форумов" border="0" width="19" height="15" style="vertical-align: text-bottom" />
															<span>Список</span>
														</a>
													</td>
													<td class="navigation" nowrap="nowrap">
														<!-- nextoption is either next.gif or greynext.gif -->
														<a href="/postlist.php?Cat=&amp;Board=Common&amp;page=1&amp;fullview=&amp;src=&amp;sb=5&amp;o=&amp;showlite=">
															<img alt="Следующая страница" border="0" width="14" height="15" style="vertical-align: text-bottom">
																<xsl:attribute name="src">/static/images/next.gif</xsl:attribute>
															</img>
															<span>След.</span>
														</a>
													</td>
													<td class="navigation">
														<a href="/ubbthreads.php?check=Common&amp;src=&amp;showlite=">
															<img src="/static/images/lb.gif" border="0" alt="Прочитать всё" style="vertical-align: text-bottom" />
														</a>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br/>
		<xsl:if test="boards/board">
			<table width="95%" align="center" class="tableborders" border="1">
				<tr>
					<td class="tdheader" colspan="2" width="61%">Подразделы</td>
					<td class="tdheader" align="center" width="7%">Темы</td>
					<td class="tdheader" align="center" width="7%">Сообщений</td>
					<td class="tdheader" align="center" width="15%">Последнее</td>
					<td class="tdheader" align="center" width="10%">Модератор</td>
				</tr>
				<xsl:apply-templates select="boards/board"/>
			</table>
			<br/>
		</xsl:if>
		<table width="95%" align="center" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" class="tableborders">
						<tr>
							<td align="left" width="55%" class="tdheader">Тема</td>
							<td align="left" nowrap="nowrap" width="15%" class="tdheader">Автор</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">Просмотры</td>
							<td nowrap="nowrap" width="5%" class="tdheader" align="center">Ответы</td>
							<td nowrap="nowrap" width="20%" class="tdheader" align="center">Последнее</td>
						</tr>
						<!-- BEGIN POST LOOP DO NOT DELETE -->
						<tr>
							<td align="left" class="lighttable">
								<xsl:attribute name="title">Давайте выкладывать сюда свои фотографии с претензией на художественность или просто красивые.
А потом проведем конкурс и дадим какой-нть приз     

&#160;Если Вы нашли этот тред в мусорке, верните его пожалуйста в Common.  Atilla


 Специализированный сайт фото-выставки: http://hg.b.gz.ru/peg/  by heGoat.</xsl:attribute>
								<img alt="*" src="/static/images/book-notread.gif" hspace="5" style="vertical-align: text-bottom" />
								<a href="/showflat.php?Cat=&amp;Board=Common&amp;Number=9546068&amp;Main=479355&amp;fullview=&amp;src=&amp;o=&amp;showlite=&amp;tistart=#Post9546068">
									<img src="/static/images/sticky.gif" width="16" height="16" alt="" border="0" style="vertical-align: text-bottom" />
									<span>Фото-выставка.</span>
								</a>
								<span class="small" style="margin-left:1.5em">
									<a class="separate" href="/Thread/NOTIMPLEMENTED/p0/">0</a>
									<a class="separate" href="/Thread/NOTIMPLEMENTED/p20/">20</a>
									<a class="separate" href="/Thread/NOTIMPLEMENTED/p4620/">4620</a>
									<a class="separate" href="/Thread/NOTIMPLEMENTED/p4640/">4640</a>
									<a class="separate" href="/Thread/NOTIMPLEMENTED/all/">Все</a>
								</span>
							</td>
							<td align="left" nowrap="nowrap" class="lighttable">
								<a href="/showprofile.php?Cat=&amp;User=heGoat&amp;Board=Common&amp;what=ubbthreads&amp;page=0&amp;src=&amp;sb=5&amp;o="><font color="#0000FF">heGoat</font></a>
							</td>
							<td align="center" class="lighttable">
								<span>811077</span>
							</td>
							<td align="center" nowrap="nowrap" class="lighttable">
								<span class="separate">26633</span>
								<a class="cup separate" href="/postlist.php?Cat=&amp;Board=Common&amp;page=0&amp;fullview=&amp;src=&amp;sb=5&amp;o=&amp;cupthread=479355&amp;cupmaxnumber=9559703&amp;showlite=" >
									<font class="new"><i>(10)</i></font>
								</a>
								<a class="cup separate" href="/postlist.php?Cat=&amp;Board=Common&amp;page=0&amp;fullview=&amp;src=&amp;sb=5&amp;o=&amp;cupthread=479355&amp;cupdisc=1&amp;showlite=" >
									<font class="new2"><i>(41)</i></font>
								</a>
							</td>
							<td nowrap="nowrap" align="center" class="lighttable">
								<span>13.09.2003 15:08</span>
							</td>
						</tr>
						<!-- END OF LOOP -->
						<tr class="tdheader">
							<td colspan="5">
								<font class="onbody">
									<span class="separate">страницы:</span>
									<a class="separate">0</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p20/</xsl:attribute>
										<span>20</span>
									</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p40/</xsl:attribute>
										<span>40</span>
									</a>
									<a class="separate">
										<xsl:attribute name="href">/Board/1/p60/</xsl:attribute>
										<span>60</span>
									</a>
								</font>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<table width="95%" align="center" cellpadding="1" cellspacing="1" class="tablesurround">
			<tr>
				<td>
					<table cellpadding="3" cellspacing="1" width="100%" border="0" class="tableborders">
						<tr class="tdheader">
							<td colspan="3">Дополнительная информация</td>
						</tr>
						<tr class="lighttable">
							<td width="33%" class="small" valign="top">
								<span>11 зарегистрированных и 1 анонимных пользователей просматривают этот форум.</span>
								<br />
								<br />
								<span>Модераторы:</span>
								<a class="separate" href="/showprofile.php?User=Sash&amp;What=ubbthreads">Sash</a>
								<a class="separate" href="/showprofile.php?User=DeadmoroZ&amp;What=ubbthreads">DeadmoroZ</a>&#160;
								<br />
								<br />
							</td>
							<td valign="top" align="left" class="small" width="33%">
								<b>Права</b>
								<table style="margin-left:4em">
									<tr><td>Вы можете создавать новые темы</td></tr>
									<tr><td>Вы можете отвечать на сообщения</td></tr>
									<tr><td>HTML отключен</td></tr>
									<tr><td>UBBCode включен</td></tr>
								</table>
							</td>
							<td class="small" valign="top">
								<b>Легенда:</b>
								<table style="margin-left:4em">
									<tr>
										<td><img src="/static/images/book-notread.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>Новые сообщения</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/book-read.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>Нет новых сообщений</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/chat-notread.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>Новые сообщения</span></td>
									</tr>
									<tr>
										<td><img src="/static/images/chat-read.gif" alt="" style="vertical-align: text-bottom" /></td>
										<td><span>Нет новых сообщений</span></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr class="lighttable">
							<td align="left" width="33%" class="small">
								<form method="get" action="/dosearch.php">
									<span>Поиск в форуме</span>
									<input type="text" name="Words" class="formboxes" style="font-size:10px" />
									<input type="submit" name="textsearch" value="Поиск" class="buttons" style="font-size:10px" />
								</form>
							</td>
							<td align="left" width="33%" class="small">
								<form method="post" action="/addfav.php?Cat=&amp;Board=Common&amp;type=board&amp;src=&amp;showlite=">
									<input type="submit" value="В фавориты!" class="buttons iconize" style="background-image:url('/images/favorites.gif');" />
								</form>
								<form method="post" action="/togglesub.php?Cat=&amp;Board=Common&amp;page=0&amp;src=&amp;sb=5&amp;o=">
									<input type="submit" value="Спрятать" class="buttons iconize" style="background-image:url('/images/hide.gif');" />
								</form>
								<form action="/changerss.php?Cat=&amp;rss_board_add=Common&amp;src=" method="post">
									<input type="submit" value="&#160;&#160;" class="buttons iconize" style="width:32px;background-image:url('/images/rss.png');" />
								</form>
							</td>
							<td align="left" class="small">
								<form method="post" action="/jumper.php">
									<span>Переход в</span>
									<select name="board" class="formboxes" style="font-size:10px" >
										<option value ="-CATJUMP-1">-General-</option>
										<option value="Common" selected="selected">&#160;&#160;&#160;Common</option>
										<option value="current" >&#160;&#160;&#160;Current</option>
										<option value="University" >&#160;&#160;&#160;University</option>
									</select>
									<input type="submit" style="font-size:10px" name="Jump" value="Перейти" class="buttons" />
								</form>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template match="category">
		<table width="100%" align="center" class="tableborders" cellpadding="3" cellspacing="1">
			<tr>
				<td class="tdheader" colspan="2" width="61%"><xsl:value-of select="name"/></td>
				<td class="tdheader" align="center" width="7%">Темы</td>
				<td class="tdheader" align="center" width="7%">Сообщений</td>
				<td class="tdheader" align="center" width="15%">Последнее</td>
				<td class="tdheader" align="center" width="10%">Модератор</td>
			</tr>
			<xsl:apply-templates select="boards/board"/>
		</table>
	</xsl:template>

</xsl:stylesheet>