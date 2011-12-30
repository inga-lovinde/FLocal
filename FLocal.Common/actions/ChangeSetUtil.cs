using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core.DB;

namespace FLocal.Common.actions {
	public static class ChangeSetUtil {

		internal static void WithChangeSet(Action<ChangeSet, Transaction> action) {
			using(ChangeSet changeSet = new ChangeSet()) {
				Config.Transactional(transaction => {
					action(changeSet, transaction);
					changeSet.Apply(transaction);
				});
			}
		}

		internal static void WithChangeSet(Action<ChangeSet> action) {
			WithChangeSet((changeset, transaction) => action(changeset));
		}

		public static void ApplyChanges(params AbstractChange[] changes) {
			using(ChangeSet changeSet = new ChangeSet()) {
				foreach(AbstractChange change in changes) {
					changeSet.Add(change);
				}
				Config.Transactional(transaction => {
					changeSet.Apply(transaction);
				});
			}
		}

	}
}
