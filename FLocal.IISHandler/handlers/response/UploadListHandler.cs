﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using FLocal.Common;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using Web.Core.DB;
using Web.Core.DB.conditions;

namespace FLocal.IISHandler.handlers.response {
	class UploadListHandler : AbstractGetHandler<FLocal.Common.URL.upload.List> {

		protected override string templateName {
			get {
				return "UploadList.xslt";
			}
		}

		protected override IEnumerable<XElement> getSpecificData(WebContext context) {
			if(context.session == null) throw new AccessDeniedException();
			PageOuter pageOuter = PageOuter.createFromUrl(this.url, context.userSettings.uploadsPerPage);
			List<Upload> uploads = Upload.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Upload.TableSpec.instance,
					new EmptyCondition(),
					pageOuter,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Upload.TableSpec.instance.getColumnSpec(Upload.TableSpec.FIELD_UPLOADDATE),
							pageOuter.ascendingDirection
						),
						new SortSpec(
							Upload.TableSpec.instance.getIdSpec(),
							pageOuter.ascendingDirection
						),
					}
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
