﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Web.Core;
using FLocal.Migration.Gateway;
using FLocal.Common;
using FLocal.Common.actions;
using FLocal.Common.dataobjects;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.Migration.Console {
	static class ShallerDBProcessor {

		private readonly static Dictionary<int, string> discussions = new Dictionary<int, string> {
			{ 384486, "Common.Photos" },
			{ 2665162, "sport" }, //Обсуждение игроков 
			{ 2099333, "hobby" }, //Клуб загадывателей ников 
			{ 2189773, "automoto" }, //ПРедлагаю ТОПИК! ФОТОГРАФИИ АВТО ФОРУМЧАН! И АВТО МИРА! (originally from common)
			{ 1961373, "sport" }, //Результаты европейских чемпионатов
			{ 2334188, "common" }, //Some foto about shanghai.
			{ 2467452, "hobby" }, //Пентагон, ЧГК, десяточка и т.д. 
			{ 121696, "media" }, //FAQ и Правила раздела (updated 14.11.2008)
			{ 5347936, "media" }, //FAQ по кодекам 6.0 от 28.10.06 (Не смотрится фильм?)
			{ 9268262, "media" }, //[Current video]
			{ 8315008, "media" }, //Трейлеры и ролики
			{ 255725, "media" }, //Новинки тяжелой музыки
			{ 5916119, "media" }, //Киноклуб МГУ

			{ 5315940, "zone.dedushka_bot" }, //Весёлый трафик
			{ 1575576, "zone.tufonkin" }, //Новые АНЕКДОТЫ и ИСТОРИИ.
			{ 1115597, "zone.zaboo" }, //Весёлые картинки
			{ 5976174, "zone.users.mondrogon" }, //Ж Е L E Z О н а
			{ 6935701, "zone.users.mandrogon" }, //Душевная зона | Блог Мондрагона
			{ 6381371, "zone.users.exogen" }, //"некрасивые" фотки... и не только... )))
			{ 9360035, "zone.users.ssestrenka" }, //Sse.Zone. (sequel. life 2)
			{ 9262889, "zone.users.soultrain" }, //Флаг ей в руки
			{ 6916214, "zone.users.soultrain" }, //Scooter
			{ 7734506, "zone.users.kobold" }, //ZOG
			{ 5961689, "zone.users.grizzl" }, //Первым делом САМОЛЕТЫ
			{ 9088688, "zone.users.troop" }, //Pigs rulezz!
			{ 847579, "zone.users.troop" }, //Dark Tranquillity - НЕ весёлые картинки
			{ 9079215, "zone.users.troop" }, //[DOGS] Wonderful Gdelitr
			{ 7829395, "zone.users.0013" }, //комиксзона +
			{ 3998922, "zone.users.Precious" }, //ИЗБРАННЫЕ высказывания форумчан
			{ 8172796, "zone.users.guz_kasperchuk" }, //Lights Out
			{ 4375761, "zone.users.mondragon" }, //| лучшая реклама
			{ 6154315, "zone.users.lelitsche" }, //HandMade <Самоделки>
			{ 8301201, "zone.users.beacon" }, //Разговоры по душам
			{ 1544532, "zone.users.ahtoh" }, //CUR_PHOTO
			{ 6825701, "zone.users.sash" }, //¤ ¤ H D R ¤ ¤
			{ 7139312, "zone.users.exobot" }, //мысли вслух или, выпуская газы мозга...
			{ 2341305, "zone.users.kudreashka_siu" }, //United Kingdom
			{ 3941231, "zone.users.sha" }, //ФОТКИ
			{ 4338097, "zone.users.camay" }, //Swing Dance
			{ 8557686, "zone.users.camay" }, //TOYS-Игрушечная зона
			{ 5956650, "zone.users.architect" }, //ЧУДЕСА АРХИТЕКТУРЫ
			{ 5003929, "zone.users.kpocab4er" }, //Зона как зона.
			{ 7355438, "zone.users.[drunk]troop" }, //Cats MUST Die!!!
			{ 3216110, "zone.users.odhinn" }, //PhotoS
			{ 4627028, "zone.users.anaharsis" }, //Н О :( Т А Л Ь Г И Я
			{ 9019009, "zone.users.bittershocko" }, //Хроники протеста
			{ 7331940, "zone.users.alla_mihelson" }, //Дневники Аллы Михельсон
			{ 1761557, "zone.users.dinara" }, //Dinarina Zона [DZ]
			{ 2350840, "zone.users.crotishka" }, //Old School of Rock
			{ 5340146, "zone.users.embryo" }, //bowl of spaghetti
			{ 6694057, "zone.users.reily" }, //blenderworld (Издавачка, упатство и програмирање)
			{ 8630270, "zone.users.gimli" }, //FAQ по мемам форума
			{ 8048659, "zone.users.maestro" }, //Карелия, Петрозаводск и Питер.
			{ 8007660, "zone.users.pingvin" }, //Синяя зона (алкашная)
			{ 9037280, "zone.users.sasisa" }, //< < < ( ( ( ш л а к О в О ч н а я ) ) ) > > >
			{ 7260522, "zone.users.qed" }, //Photos by A.Ioffe
			{ 5969907, "zone.users.nice_rabbit" }, //Radio-Rabbit
			{ 7228780, "zone.users.гулька" }, //Пустотреп
			{ 2297660, "zone.users.bay" }, //Склад
			{ 488201, "zone.users.zan" }, //00 + photos + graphics
			{ 2835960, "zone.users.jule" }, //про котoff...
			{ 3471847, "zone.users.pase4nik" }, //Pase4 и Lelya (теперь тут только фотки)
			{ 4379620, "zone.users.amis" }, //Мои фотографии
			{ 4926552, "zone.users.rosom" }, //советская - ПРОПАГАНДА - антисоветская
			{ 3487303, "zone.users.tipagleb" }, //ЖЖ влом заводить
			{ 3028114, "zone.users.micheal" }, //AmuseZone (lyrics)
			{ 4613754, "zone.users.vanda_blessen" }, //Самое время...

			{ 2078978, "garbage.gluk" }, //Ебал я в рот
			{ 747474, "garbage.sergeant" }, //Фотомусоровыставка

			{ 2259712, "health.anecdot" }, //Анекдоты о медицине и медиках
			{ 2259798, "health.faq" }, //FAQ по медицине и здравоохранению
			{ 1830891, "lovesex.photos" }, //Эротические фотографии форумчан
			{ 2018196, "lovesex.feelings" }, //Фотографии о Любви [чувства]
			{ 1977431, "lovesex.poetry" }, //Стихи о Любви
			{ 1836072, "lovesex.humour" }, //Забавные эротические фотографии
			{ 3046685, "lovesex.exhibition" }, //красивые фотки
			{ 6528708, "havcheg.wheretobuy" }, //Где купить (спрашивайте здесь)
			{ 4958967, "havcheg.photos" }, //Фотографии еды форумчан
			{ 2531277, "media.musicnews" }, //Музыкальные новинки
			{ 2673310, "automoto.photos" }, //Авто- и мотофотографии форумчан
			{ 8953694, "automoto.law" }, //Правовые авто-мото вопросы (ПДД, страхование, купля-продажа)
			{ 9280892, "automoto.advices" }, //Посоветуйте авто/мото
			{ 3147003, "automoto.news" }, //Интересные автоновости
			{ 6437071, "automoto.troubles" }, //[Current] Траблы в тачке!
			{ 7407540, "automoto.current" }, //Current ДТП
			{ 8899146, "automoto.keramzit" }, //[атмо]вымораживает
			{ 5920859, "media.downloads" }, //Скачал с нета (фильмы, музыка etc)
			{ 6250058, "media.search" }, //Ищу/скачайте фильм/музыку (ваши заявки)
			{ 9548522, "sport.forecast" }, //Конкурс прогнозов на ЧМ в ЮАР
			{ 2206695, "sport.photos" }, //Спортивные ФОТОГРАФИИ форумчан
			{ 6931744, "sport.translations" }, //ТРАНСЛЯЦИИ спортивных передач в локальной сети (вопросы, заявки, ...)
			{ 6431963, "games.flash" }, //Флэш игры
			{ 5461467, "market.rent" }, //Сдача/съем жилья, поиск мертвых душ
			{ 1615007, "job.blacklist" }, //Черный список работодателей
			{ 1614989, "job.whitelist" }, //Белый список работодателей
			{ 5313058, "job.humour" }, //Требуются упаковщицы. Веселуха.
			{ 5647033, "development.humour" }, //юмор в этом разделе
			{ 5727387, "soft.linuxnews" }, //Linux: мировой опыт внедрения
			{ 6821755, "soft.pcchoose" }, //Здесь выбираем конфигурацию компьютера
			{ 7135524, "soft.nbchoose" }, //Здесь выбираем ноутбуки и комплектующие для них
			{ 974645, "soft.photos" }, //фотографии компьютеров
			{ 5872076, "mobile.humour" }, //Мобильный юмор
			{ 5403826, "mobile.photos" }, //Фотографии мобильных девайсов форумчан.
			{ 982390, "diaspora.photos" }, //Фотовыставка "Малая родина"
			{ 2554967, "diaspora.vietnam" }, //маленькая зоночка про Вьетнам
			{ 6865321, "study.downloads" }, //Просьбы скачать книгу с lib.mexmat.ru или статьи
			{ 929573, "study.photos" }, //Фотографии преподавателей
			{ 9520368, "common.bur" }, //Буревестник-2010 (вопросы-ответы, советы, инфо, др.)
			{ 479355, "common.exhibition" }, //Фото-выставка.
			{ 4266942, "common.faq" }, //Правила раздела
			{ 2570240, "society.humour" }, //Политический юмор
			{ 6400517, "society.photos" }, //Серьезные картинки
			{ 2541128, "society.links" }, //Society links.
			{ 7186791, "society.negative" }, //Как страшно жить. (негатив сюда).
			{ 4934799, "society.quotes" }, //Цитаты
			{ 2366633, "society.nostalgie" }, //Nostalgie
			{ 9507245, "society.forecasts" }, //Пророчества о скорой гибели с.р.
			{ 9277324, "society.lawlessness" }, //[current] Ментовской беспредел
			{ 1699598, "hobby.pictures" }, //Тред с картинками
			{ 2340623, "hobby.movies" }, //Хорошие фильмы [с комментариями и ссылками]
			{ 6727481, "hobby.youtube" }, //Что я видел на Youtube (все самое лучшее

			{ 9510866, "flood.guessuser" }, //[нИгра] Угадайте форумчанина
			{ 9032894, "hobby.contemporary" }, //Современные художники (с картинками)
		};

		private readonly static DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

		public static void processDB(string filename) {
			try {
			Dictionary<int, Action> inserts = new Dictionary<int, Action>();
			HashSet<int> discussionsIds = new HashSet<int>();
			using(StreamReader reader = new StreamReader(filename)) {
				int i=0;
				while(!reader.EndOfStream) {
					string line = reader.ReadLine().Trim();
					if(line == "") {
						continue;
					}
					if(i%1000 == 0) {
						System.Console.Write("[" + (int)(i/1000) + "]");
					}
					Dictionary<string, string> data;
					try {
						data = DictionaryConverter.FromDump(line);
					} catch(Exception e) {
						System.Console.Error.WriteLine("Error while trying to parse line: " + e.GetType().FullName + ": " + e.Message);
						System.Console.Error.WriteLine(e.StackTrace);
						continue;
					}
					int postId = int.Parse(data["Number"]);
					try {
						if(inserts.ContainsKey(postId)) {
							System.Console.Write("-");
						} else if(Config.instance.mainConnection.GetCountByConditions(Post.TableSpec.instance, new ComparisonCondition(Post.TableSpec.instance.getIdSpec(), ComparisonType.EQUAL, postId.ToString())) > 0) {
/*							Post post = Post.LoadById(postId);
							if(post.title.StartsWith("%") || post.title.StartsWith("Re%3A") || post.body.StartsWith("%") || (post.thread.firstPost.id == post.id && post.thread.title.StartsWith("%"))) {
								string title = data["Subject"];
								string body = data["Body"];
								inserts[postId] = () => {
									List<AbstractChange> changes = new List<AbstractChange> {
										new UpdateChange(
											Post.TableSpec.instance,
											new Dictionary<string, AbstractFieldValue> {
												{ Post.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
												{ Post.TableSpec.FIELD_BODY, new ScalarFieldValue(body) },
											},
											post.id
										)
									};
									if(post.thread.firstPost.id == post.id) {
										changes.Add(
											new UpdateChange(
												Thread.TableSpec.instance,
												new Dictionary<string, AbstractFieldValue> {
													{ Thread.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
												},
												post.thread.id
											)
										);
									}

									ChangeSetUtil.ApplyChanges(changes.ToArray());
								};
								Console.Write("+");
							} else {
								Console.Write("-");
							}*/
							System.Console.Write("-");
						} else {
							int localMain = int.Parse(data["Local_Main"]);
							int main = int.Parse(data["Main"]);
							int UnixTime;
							try {
								UnixTime = int.Parse(data["UnixTime"]);
							} catch(OverflowException) {
								UnixTime = 1000*1000*1000;
							}
							if(UnixTime <= 0) {
								UnixTime = 1000*1000*1000;
							}
							DateTime date = UNIX.AddSeconds(UnixTime).ToLocalTime();
							User user;
							string username = data["Username"];
							try {
								user = User.LoadByName(username);
							} catch(NotFoundInDBException) {
								System.Console.Error.WriteLine("Cannot find user '" + username + "'; creating one...");
								ChangeSetUtil.ApplyChanges(
									new InsertChange(
										User.TableSpec.instance,
										new Dictionary<string, AbstractFieldValue> {
												{ User.TableSpec.FIELD_NAME, new ScalarFieldValue(username) },
												{ User.TableSpec.FIELD_REGDATE, new ScalarFieldValue(date.ToUTCString()) },
												{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue(User.ENUM_SHOWPOSTSTOUSERS_ALL) },
												{ User.TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_BIOGRAPHYUBB, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_LOCATION, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_SIGNATURE, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_SIGNATUREUBB, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_TITLE, new ScalarFieldValue("") },
												{ User.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue("0") },
												{ User.TableSpec.FIELD_USERGROUPID, new ScalarFieldValue("1") },
											}
									)
								);
								user = User.LoadByName(data["Username"]);
							}
							string title = data["Subject"];
							string body = data["Body"];
							PostLayer layer = PostLayer.LoadById(1);
							if(data.ContainsKey("Layer")) {
								layer = PostLayer.LoadById(int.Parse(data["Layer"]));
							}
							inserts[postId] = () => {
								bool isDiscussion = false;
								if(postId == main || postId == localMain) {
									//first post in the thread
									string legacyBoardName;
									if(discussions.ContainsKey(main) || (localMain != 0 && (localMain != postId || localMain != main))) {
										discussionsIds.Add(main);
										legacyBoardName = discussions[main];
										isDiscussion = true;
									} else {
										legacyBoardName = data["Board"];
									}
									Board board;
									try {
										board = Board.LoadByLegacyName(legacyBoardName);
									} catch(NotFoundInDBException) {
										throw new ApplicationException("Cannot find board '" + legacyBoardName + "'");
									}
									board.CreateThread(user, title, body, layer, date, postId);
								} else {
									int parentId = int.Parse(data["Parent"]);
									if(parentId == 0) {
										parentId = localMain;
										if(parentId == 0) {
											parentId = main;
										}
									}
									Post parent;
									try {
										parent = Post.LoadById(parentId);
									} catch(NotFoundInDBException) {
										throw new ApplicationException("Cannot find parent post #" + parentId);
									}

									if(!isDiscussion && parent.thread.firstPostId != localMain) {
										System.Console.Write("d");
									} else {
										parent.Reply(user, title, body, layer, date, postId);
									}
								}
							};
							System.Console.Write("+");
						}
					} catch(Exception e) {
						System.Console.Error.WriteLine("Cannot process post #" + postId + ": " + e.GetType().FullName + ": " + e.Message);
						System.Console.Error.WriteLine(e.StackTrace);
						System.Console.Write("!");
//						Console.ReadLine();
					} finally {
						i++;
						if((i%50000)==0) {
							Web.Core.RegistryCleaner.CleanRegistry<int, Post>();
							Web.Core.RegistryCleaner.CleanRegistry<int, Thread>();
							GC.Collect();
							System.Console.Error.WriteLine();
							System.Console.Error.WriteLine("Registry cleaned; garbage collected");
							System.Console.Error.WriteLine();
						}
					}
				}
			}

			System.Console.WriteLine();
			System.Console.WriteLine("Finished parsing");
			System.Console.WriteLine("Total inserts: " + inserts.Count);
			System.Console.ReadLine();
			int j=0;
			foreach(var insert in inserts) {
				if(j%1000 == 0) {
					System.Console.Write("[" + (int)(j/1000) + "]");
				}
				try {
					insert.Value();
					System.Console.Write("+");
				} catch(Exception e) {
					System.Console.Error.WriteLine("Cannot process post #" + insert.Key + ": " + e.GetType().FullName + ": " + e.Message);
					System.Console.Error.WriteLine(e.StackTrace);
					System.Console.Write("!");
//						Console.ReadLine();
				} finally {
					j++;
				}
			}

			System.Console.WriteLine("Not found discussions:");
			foreach(int discussionId in discussionsIds.OrderBy(id => id)) {
				System.Console.WriteLine(discussionId);
			}
			} catch(Exception e) {
				System.Console.Error.WriteLine(e.GetType().FullName + ": " + e.Message);
				System.Console.Error.WriteLine(e.StackTrace);
			}

		}

	}
}
