﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {
	class UploadListHandler : AbstractGetHandler {

		protected override string templateName {
			get {
				return "UploadList.xslt";
			}
		}

		protected override System.Xml.Linq.XElement[] getSpecificData(WebContext context) {
			if(context.session == null) throw new AccessViolationException();
			PageOuter pageOuter = PageOuter.createFromGet(context.requestParts, 100, 2);
			List<Upload> uploads = Upload.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Upload.TableSpec.instance,
					new MultiValueCondition(
						Upload.TableSpec.instance.getColumnSpec(Upload.TableSpec.FIELD_EXTENSION),
						new string[] { "jpg", "gif", "png" }
					),
					pageOuter,
					new JoinSpec[0]
				) select int.Parse(stringId)
			);
			return new XElement[] {
				new XElement("uploads",
					from upload in uploads select upload.exportToXml(context),
					pageOuter.exportToXml(2, 5, 2)
				)
			};
		}

	}
}
