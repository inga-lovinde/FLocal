using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FLocal.Core;
using FLocal.Importer;
using FLocal.Common;
using FLocal.Common.actions;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.ImportConsole {
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
		};

		private readonly static DateTime UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

		public static void processDB(string filename) {
			HashSet<int> discussionsIds = new HashSet<int>();
			using(StreamReader reader = new StreamReader(filename)) {
				int i=0;
				while(!reader.EndOfStream) {
					string line = reader.ReadLine().Trim();
					if(line == "") {
						continue;
					}
					if(i%1000 == 0) {
						Console.Write("[" + (int)(i/1000) + "]");
					}
					Dictionary<string, string> data = DictionaryConverter.FromDump(line);
					int postId = int.Parse(data["Number"]);
					try {
						if(Config.instance.mainConnection.GetCountByConditions(Post.TableSpec.instance, new ComparisonCondition(Post.TableSpec.instance.getIdSpec(), ComparisonType.EQUAL, postId.ToString())) > 0) {
							Console.Write("-");
						} else {
							int localMain = int.Parse(data["Local_Main"]);
							int main = int.Parse(data["Main"]);
							DateTime date = UNIX.AddSeconds(int.Parse(data["UnixTime"])).ToLocalTime();
							User user;
							string username = data["Username"];
							try {
								user = User.LoadByName(username);
							} catch(NotFoundInDBException) {
								Console.Error.WriteLine("Cannot find user '" + username + "'; creating one...");
								ChangeSetUtil.ApplyChanges(
									new InsertChange(
										User.TableSpec.instance,
										new Dictionary<string, AbstractFieldValue> {
											{ User.TableSpec.FIELD_NAME, new ScalarFieldValue(username) },
											{ User.TableSpec.FIELD_REGDATE, new ScalarFieldValue(date.ToUTCString()) },
											{ User.TableSpec.FIELD_SHOWPOSTSTOUSERS, new ScalarFieldValue(User.ENUM_SHOWPOSTSTOUSERS_ALL) },
											{ User.TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_LOCATION, new ScalarFieldValue("") },
											{ User.TableSpec.FIELD_SIGNATURE, new ScalarFieldValue("") },
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
							if(postId == main || postId == localMain) {
								//first post in the thread
								string legacyBoardName;
								if(localMain != 0) {
									discussionsIds.Add(main);
									legacyBoardName = discussions[main];
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
								Post post;
								try {
									post = Post.LoadById(parentId);
								} catch(NotFoundInDBException) {
									throw new ApplicationException("Cannot find parent post #" + parentId);
								}
								post.Reply(user, title, body, layer, date, postId);
							}
							Console.Write("+");
						}
					} catch(Exception e) {
						Console.Error.WriteLine("Cannot process post #" + postId + ": " + e.GetType().FullName + ": " + e.Message);
						Console.Error.WriteLine(e.StackTrace);
						Console.Write("!");
//						Console.ReadLine();
					} finally {
						i++;
						if((i%50000)==0) {
							Core.RegistryCleaner.CleanRegistry<int, Post>();
							Core.RegistryCleaner.CleanRegistry<int, Thread>();
							GC.Collect();
							Console.Error.WriteLine();
							Console.Error.WriteLine("Registry cleaned; garbage collected");
							Console.Error.WriteLine();
						}
					}
				}
			}

			Console.WriteLine("Not found discussions:");
			foreach(int discussionId in discussionsIds.OrderBy(id => id)) {
				Console.WriteLine(discussionId);
			}
		}

	}
}
